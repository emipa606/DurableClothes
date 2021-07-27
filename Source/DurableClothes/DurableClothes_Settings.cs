using HugsLib;
using HugsLib.Settings;
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
                "DC_FullRepairLabel".Translate(),
                "DC_FullRepairTooltip".Translate(),
                true);
            wearPercent = Settings.GetHandle("wearPercent",
                "DC_WearDamageLabel".Translate(),
                "DC_WearDamageTooltip".Translate(),
                0);
        }
    }
}