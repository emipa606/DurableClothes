using System;
using System.Linq;
using Mlie;
using RimWorld;
using UnityEngine;
using Verse;

namespace DurableClothes;

[StaticConstructorOnStartup]
internal class DurableClothesMod : Mod
{
    /// <summary>
    ///     The instance of the settings to be read by the mod
    /// </summary>
    public static DurableClothesMod Instance;

    private static string currentVersion;

    private static Vector2 scrollPosition;

    /// <summary>
    ///     The private settings
    /// </summary>
    public readonly DurableClothesSettings Settings;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="content"></param>
    public DurableClothesMod(ModContentPack content) : base(content)
    {
        Instance = this;
        currentVersion =
            VersionFromManifest.GetVersionFromModMetaData(content.ModMetaData);
        Settings = GetSettings<DurableClothesSettings>();
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
        Settings.IgnoredCategories ??= [];

        var listingStandard = new Listing_Standard();
        listingStandard.Begin(rect);
        listingStandard.Gap();
        listingStandard.CheckboxLabeled("DC_FullRepairLabel".Translate(), ref Settings.ToggleFullRepair,
            "DC_FullRepairTooltip".Translate());
        listingStandard.Label("DC_WearDamageLabel_new".Translate(), -1, "DC_WearDamageTooltip".Translate());
        Settings.WearPercent = Widgets.HorizontalSlider(listingStandard.GetRect(20), Settings.WearPercent, 0,
            1f,
            false, Settings.WearPercent.ToStringPercent());

        listingStandard.Gap();
        Settings.OnlyAboveQuality = (int)Math.Round(listingStandard.SliderLabeled(
            "DC_OnlyAboveQuality".Translate(((QualityCategory)Settings.OnlyAboveQuality).ToString()),
            Settings.OnlyAboveQuality, 0f, Enum.GetNames(typeof(QualityCategory)).Length - 1, 0.5f,
            "DC_OnlyAboveQualityTT".Translate()));

        if (currentVersion != null)
        {
            listingStandard.Gap();
            GUI.contentColor = Color.gray;
            listingStandard.Label("DC_CurrentModVersionLabel".Translate(currentVersion));
            GUI.contentColor = Color.white;
        }

        listingStandard.Gap();
        listingStandard.GapLine();
        listingStandard.Label("DC_IgnoreCategories".Translate(), -1f, "DC_IgnoreCategoriesTooltip".Translate());
        var apparelCategories = ThingCategoryDefOf.Apparel.childCategories;
        var scrollRect = rect;
        scrollRect.y += listingStandard.CurHeight;
        scrollRect.height -= listingStandard.CurHeight;
        var viewRect = scrollRect;
        viewRect.position = Vector2.zero;
        viewRect.height = 15 + (25 * ThingCategoryDefOf.Apparel.ThisAndChildCategoryDefs.Count());
        viewRect.width *= 0.95f;
        listingStandard.End();

        Widgets.BeginScrollView(scrollRect, ref scrollPosition, viewRect);

        listingStandard.Begin(viewRect);
        foreach (var thingCategoryDef in apparelCategories)
        {
            listChildCategories(thingCategoryDef, ref listingStandard, 0, false, false);
        }


        listingStandard.End();
        Widgets.EndScrollView();
    }

    private void listChildCategories(ThingCategoryDef categoryDef, ref Listing_Standard listing_Standard, int tab,
        bool changeValue, bool changeTo)
    {
        var spacing = string.Join(" ", new string[tab * 4]);
        if (changeValue)
        {
            switch (changeTo)
            {
                case true when !Settings.IgnoredCategories.Contains(categoryDef.defName):
                    Settings.IgnoredCategories.Add(categoryDef.defName);
                    break;
                case false when Settings.IgnoredCategories.Contains(categoryDef.defName):
                    Settings.IgnoredCategories.Remove(categoryDef.defName);
                    break;
            }
        }

        var isSelected = Settings.IgnoredCategories.Contains(categoryDef.defName);
        var originalValue = isSelected;
        listing_Standard.CheckboxLabeled($"{spacing}{categoryDef.LabelCap}", ref isSelected,
            string.Join("\r\n", categoryDef.childThingDefs.OrderBy(def => def.label).Select(def => def.LabelCap)));
        var changeChild = isSelected != originalValue || changeValue;
        if (isSelected != originalValue)
        {
            if (isSelected)
            {
                Settings.IgnoredCategories.Add(categoryDef.defName);
            }
            else
            {
                Settings.IgnoredCategories.Remove(categoryDef.defName);
            }
        }

        foreach (var categoryDefChildCategory in categoryDef.childCategories)
        {
            listChildCategories(categoryDefChildCategory, ref listing_Standard, tab + 1, changeChild, isSelected);
        }
    }
}