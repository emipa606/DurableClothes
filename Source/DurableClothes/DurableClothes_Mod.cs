// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Global
// ReSharper disable InconsistentNaming

using System.Reflection;
using HarmonyLib;
using HugsLib;
using HugsLib.Settings;
using RimWorld;
using Verse;

namespace DurableClothes
{
    public class DurableClothes_Settings : ModBase
    {
        public static SettingHandle<bool> toggleFullRepair;
        public static SettingHandle<int> wearPercent;
        public override string ModIdentifier => "DurableClothes";

        public override void DefsLoaded()
        {
            toggleFullRepair = Settings.GetHandle("toggleFullRepair",
                // TODO translation support
                "Full Repair",
                "When enabled, fully repairs all clothing instead of simply stopping degradation.",
                true);
            wearPercent = Settings.GetHandle("wearPercent",
                // TODO translation support
                "Wear damage %",
                "Instead of no deterioration, you can change it to a percent of the original here.",
                0);
        }
    }

    [HarmonyPatch(typeof(Pawn_ApparelTracker))]
    [HarmonyPatch("TakeWearoutDamageForDay")]
    internal class HarmonyPatches
    {
        static HarmonyPatches()
        {
            var harmony = new Harmony("rimworld.mlie.durableclothes");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        [HarmonyPrefix]
        public static bool PatchMethod_Prefix(ref Thing ap, Pawn_ApparelTracker __instance)
        {
            if ((bool) DurableClothes_Settings.toggleFullRepair) // Only do full repair if the setting is enabled
            {
                ap.HitPoints = ap.MaxHitPoints;
                //Log.Message("Repair to full");
                return false;
            }

            if (DurableClothes_Settings.wearPercent == 0)
            {
                //Log.Message("No deterioraton");
                return false;
            }

            var num = GenMath.RoundRandom(ap.def.apparel.wearPerDay * DurableClothes_Settings.wearPercent / 100);
            //Log.Message("Take damage "+ num);
            if (num > 0)
            {
                ap.TakeDamage(new DamageInfo(DamageDefOf.Deterioration, num));
            }

            if (ap.Destroyed && PawnUtility.ShouldSendNotificationAbout(__instance.pawn) && !__instance.pawn.Dead)
            {
                Messages.Message(
                    "MessageWornApparelDeterioratedAway"
                        .Translate(GenLabel.ThingLabel(ap.def, ap.Stuff), __instance.pawn).CapitalizeFirst(),
                    __instance.pawn, MessageTypeDefOf.NegativeEvent);
            }

            return false; // don't run the original logic to degrade apparel either way
        }
    }
}