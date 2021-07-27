using HarmonyLib;
using RimWorld;
using Verse;

namespace DurableClothes
{
    [HarmonyPatch(typeof(Pawn_ApparelTracker), "TakeWearoutDamageForDay")]
    public static class Pawn_ApparelTracker_TakeWearoutDamageForDay
    {
        public static bool Prefix(ref Thing ap, Pawn_ApparelTracker __instance)
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