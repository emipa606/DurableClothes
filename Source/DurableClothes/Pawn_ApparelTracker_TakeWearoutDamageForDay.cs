using HarmonyLib;
using RimWorld;
using Verse;

namespace DurableClothes;

[HarmonyPatch(typeof(Pawn_ApparelTracker), "TakeWearoutDamageForDay")]
public static class Pawn_ApparelTracker_TakeWearoutDamageForDay
{
    public static bool Prefix(ref Thing ap, Pawn_ApparelTracker __instance)
    {
        if (ap?.def == null)
        {
            return true;
        }

        if (DurableClothesMod.Instance.Settings.IgnoredCategories?.Contains(ap.def.FirstThingCategory?.defName) == true)
        {
            return true;
        }

        if (!ap.def.useHitPoints)
        {
            return true;
        }

        if (DurableClothesMod.Instance.Settings.OnlyAboveQuality > 0)
        {
            if (!ap.TryGetQuality(out var quality))
            {
                return true;
            }

            if (quality < (QualityCategory)DurableClothesMod.Instance.Settings.OnlyAboveQuality)
            {
                return true;
            }
        }

        if (DurableClothesMod.Instance.Settings.ToggleFullRepair) // Only do full repair if the setting is enabled
        {
            ap.HitPoints = ap.MaxHitPoints;
            return false;
        }

        if (DurableClothesMod.Instance.Settings.WearPercent == 0)
        {
            return false;
        }

        var num = GenMath.RoundRandom(ap.def.apparel.wearPerDay * DurableClothesMod.Instance.Settings.WearPercent);
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