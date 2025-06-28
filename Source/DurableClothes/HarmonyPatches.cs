using System.IO;
using System.Reflection;
using System.Xml.Linq;
using HarmonyLib;
using Verse;

namespace DurableClothes;

[StaticConstructorOnStartup]
public class HarmonyPatches
{
    static HarmonyPatches()
    {
        var harmony = new Harmony("rimworld.mlie.durableclothes");
        harmony.PatchAll(Assembly.GetExecutingAssembly());

        var hugsLibConfig = Path.Combine(GenFilePaths.SaveDataFolderPath, Path.Combine("HugsLib", "ModSettings.xml"));
        if (!new FileInfo(hugsLibConfig).Exists)
        {
            return;
        }

        var xml = XDocument.Load(hugsLibConfig);

        var modSettings = xml.Root?.Element("DurableClothes");
        if (modSettings == null)
        {
            return;
        }

        foreach (var modSetting in modSettings.Elements())
        {
            if (modSetting.Name == "wearPercent")
            {
                DurableClothesMod.Instance.Settings.WearPercent = int.Parse(modSetting.Value) / 100f;
            }

            if (modSetting.Name == "toggleFullRepair")
            {
                DurableClothesMod.Instance.Settings.ToggleFullRepair = bool.Parse(modSetting.Value);
            }
        }

        xml.Root.Element("DurableClothes")?.Remove();
        xml.Save(hugsLibConfig);

        Log.Message("[DurableClothes]: Imported old HugLib-settings");
    }
}