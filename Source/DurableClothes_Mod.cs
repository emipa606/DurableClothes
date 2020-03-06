// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Global
// ReSharper disable InconsistentNaming

using HarmonyLib;
using HugsLib;
using HugsLib.Settings;
using RimWorld;
using System.Reflection;
using Verse;

namespace DurableClothes
{
    public class DurableClothes_Settings : ModBase
    {
        public override string ModIdentifier
        {
            get { return "DurableClothes"; }
        }

        public static SettingHandle<bool> toggleFullRepair;

        public override void DefsLoaded()
        {
            toggleFullRepair = Settings.GetHandle("toggleFullRepair",
                // TODO translation support
                "Full Repair",
                "When enabled, fully repairs all clothing instead of simply stopping degradation.",
                true);
        }
    }

    [HarmonyPatch(typeof(Pawn_ApparelTracker))]
    [HarmonyPatch("TakeWearoutDamageForDay")]
    internal class HarmonyPatches
    {
        static HarmonyPatches()
        {
            Harmony harmony = new Harmony("rimworld.mlie.durableclothes");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        [HarmonyPrefix]
        public static bool PatchMethod_Prefix(ref Thing ap)
        {
            if ((bool)DurableClothes_Settings.toggleFullRepair) // Only do full repair if the setting is enabled
            {
                ap.HitPoints = ap.MaxHitPoints;
            }
            return false; // don't run the original logic to degrade apparel either way
        }
    
    }
}
