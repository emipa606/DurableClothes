using Mlie;
using UnityEngine;
using Verse;

namespace DurableClothes;

[StaticConstructorOnStartup]
internal class DurableClothesMod : Mod
{
    /// <summary>
    ///     The instance of the settings to be read by the mod
    /// </summary>
    public static DurableClothesMod instance;

    private static string currentVersion;

    /// <summary>
    ///     The private settings
    /// </summary>
    private DurableClothesSettings settings;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="content"></param>
    public DurableClothesMod(ModContentPack content) : base(content)
    {
        instance = this;
        currentVersion =
            VersionFromManifest.GetVersionFromModMetaData(ModLister.GetActiveModWithIdentifier("Mlie.DurableClothes"));
    }

    /// <summary>
    ///     The instance-settings for the mod
    /// </summary>
    internal DurableClothesSettings Settings
    {
        get
        {
            if (settings == null)
            {
                settings = GetSettings<DurableClothesSettings>();
            }

            return settings;
        }
        set => settings = value;
    }

    /// <summary>
    ///     The title for the mod-settings
    /// </summary>
    /// <returns></returns>
    public override string SettingsCategory()
    {
        return "Durable Clothes";
    }

    /// <summary>
    ///     The settings-window
    ///     For more info: https://rimworldwiki.com/wiki/Modding_Tutorials/ModSettings
    /// </summary>
    /// <param name="rect"></param>
    public override void DoSettingsWindowContents(Rect rect)
    {
        var listing_Standard = new Listing_Standard();
        listing_Standard.Begin(rect);
        listing_Standard.Gap();
        listing_Standard.CheckboxLabeled("DC_FullRepairLabel".Translate(), ref Settings.ToggleFullRepair,
            "DC_FullRepairTooltip".Translate());
        listing_Standard.Label("DC_WearDamageLabel_new".Translate(), -1, "DC_WearDamageTooltip".Translate());
        Settings.WearPercent = Widgets.HorizontalSlider(listing_Standard.GetRect(20), Settings.WearPercent, 0, 1f,
            false, Settings.WearPercent.ToStringPercent());
        if (currentVersion != null)
        {
            listing_Standard.Gap();
            GUI.contentColor = Color.gray;
            listing_Standard.Label("DC_CurrentModVersionLabel".Translate(currentVersion));
            GUI.contentColor = Color.white;
        }

        listing_Standard.End();
    }
}