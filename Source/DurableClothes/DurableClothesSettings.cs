using Verse;

namespace DurableClothes;

/// <summary>
///     Definition of the settings for the mod
/// </summary>
internal class DurableClothesSettings : ModSettings
{
    public bool ToggleFullRepair;
    public float WearPercent = 0.05f;

    /// <summary>
    ///     Saving and loading the values
    /// </summary>
    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref ToggleFullRepair, "ToggleFullRepair");
        Scribe_Values.Look(ref WearPercent, "WearPercent", 0.05f);
    }
}