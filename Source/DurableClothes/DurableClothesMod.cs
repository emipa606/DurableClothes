using System.Collections.Generic;
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
    public static DurableClothesMod instance;

    private static string currentVersion;

    private static Vector2 scrollPosition;

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
            if (settings != null)
            {
                return settings;
            }

            settings = GetSettings<DurableClothesSettings>();
            if (settings.IgnoredCategories == null)
            {
                settings.IgnoredCategories = new List<string>();
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
        Settings.WearPercent = Widgets.HorizontalSlider_NewTemp(listing_Standard.GetRect(20), Settings.WearPercent, 0,
            1f,
            false, Settings.WearPercent.ToStringPercent());

        if (currentVersion != null)
        {
            listing_Standard.Gap();
            GUI.contentColor = Color.gray;
            listing_Standard.Label("DC_CurrentModVersionLabel".Translate(currentVersion));
            GUI.contentColor = Color.white;
        }

        listing_Standard.Gap();
        listing_Standard.GapLine();
        listing_Standard.Label("DC_IgnoreCategories".Translate(), -1f, "DC_IgnoreCategoriesTooltip".Translate());
        var apparelCategories = ThingCategoryDefOf.Apparel.childCategories;
        var scrollRect = rect;
        scrollRect.y += listing_Standard.CurHeight;
        scrollRect.height -= listing_Standard.CurHeight;
        var viewRect = scrollRect;
        viewRect.position = Vector2.zero;
        viewRect.height = 15 + (25 * ThingCategoryDefOf.Apparel.ThisAndChildCategoryDefs.Count());
        viewRect.width *= 0.95f;
        listing_Standard.End();

        Widgets.BeginScrollView(scrollRect, ref scrollPosition, viewRect);

        listing_Standard.Begin(viewRect);
        foreach (var thingCategoryDef in apparelCategories)
        {
            listChildCategories(thingCategoryDef, ref listing_Standard, 0, false, false);
        }


        listing_Standard.End();
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