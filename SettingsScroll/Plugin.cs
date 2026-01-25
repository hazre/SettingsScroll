// Scroll fix logic derived from MonkeyLoader.GamePacks.Resonite
// https://github.com/ResoniteModdingGroup/MonkeyLoader.GamePacks.Resonite
// Original: MonkeyLoader.Resonite.Integration/UI/Facets/SettingsFacetCategoryScrollingFix.cs
// License: LGPL-3.0 - See THIRD-PARTY-NOTICES.md for details

using BepInEx;
using BepInEx.Logging;
using BepInEx.NET.Common;
using Elements.Core;
using FrooxEngine;
using FrooxEngine.FrooxEngine.ProtoFlux.CoreNodes;
using FrooxEngine.ProtoFlux.Runtimes.Execution.Nodes.Operators;
using FrooxEngine.UIX;
using HarmonyLib;

namespace SettingsScroll;

[BepInAutoPlugin]
[BepInDependency(BepInExResoniteShim.PluginMetadata.GUID, BepInDependency.DependencyFlags.HardDependency)]
public partial class Plugin : BasePlugin
{
    internal static new ManualLogSource Log = null!;

    public override void Load()
    {
        Log = base.Log;

        HarmonyInstance.PatchAll();
        Log.LogInfo($"Plugin {GUID} loaded");
    }
}

[HarmonyPatch(typeof(FacetPreset), "OnLoading")]
internal static class FacetPresetPatches
{
    [HarmonyPrefix]
    private static void OnLoadingPrefix(FacetPreset __instance, LoadControl control)
    {
        if (__instance is not SettingsFacetPreset settingsPreset)
            return;

        control.OnLoaded(__instance, () =>
        {
            try
            {
                var facetSlot = settingsPreset.Facet?.Slot;
                var canvas = facetSlot?.GetComponentInChildren<Canvas>();
                if (facetSlot == null || canvas == null)
                    return;

                SettingsScrollFix.ApplyScrollFix(facetSlot, canvas.Slot);
            }
            catch (Exception ex)
            {
                Plugin.Log.LogError($"Failed to apply scroll fix: {ex}");
            }
        }, int.MaxValue);
    }
}

internal static class SettingsScrollFix
{
    // Same marker as MonkeyLoader.Resonite.Integration for cross-mod compatibility
    private const string FixedMarker = "Category Scrolling Fixed";

    public static void ApplyScrollFix(Slot facetSlot, Slot canvasSlot)
    {
        if (facetSlot.GetComponent<Comment>(c => c.Text.Value == FixedMarker) != null)
            return;

        var rootCategoryView = canvasSlot.GetComponentInChildren<RootCategoryView>();
        if (rootCategoryView == null)
            return;

        var scrollAreaSlot = canvasSlot.FindChildInHierarchy("Scroll Area");
        if (scrollAreaSlot?.GetComponent<Mask>() is not Mask mask ||
            scrollAreaSlot.GetComponent<Image>() is not Image image ||
            scrollAreaSlot.GetComponent<OverlappingLayout>() is not OverlappingLayout overlappingLayout)
            return;

        var scrollRectSlot = scrollAreaSlot.FindChild("Scroll Rect");
        if (scrollRectSlot?.GetComponent<ContentSizeFitter>() is not ContentSizeFitter contentSizeFitter ||
            scrollRectSlot.GetComponent<ScrollRect>() is not ScrollRect scrollRect)
            return;

        // Enable scroll area components
        mask.Enabled = true;
        image.Enabled = true;
        overlappingLayout.Enabled = false;
        var layoutElement = scrollAreaSlot.AttachComponent<LayoutElement>();

        // Configure scroll rect
        contentSizeFitter.Enabled = true;
        contentSizeFitter.HorizontalFit.Value = SizeFit.PreferredSize;
        contentSizeFitter.VerticalFit.Value = SizeFit.PreferredSize;
        scrollRect.Enabled = true;

        // Create fresh vertical layout for categories
        rootCategoryView.CategoryManager.ContainerRoot.Target = null!;
        scrollRectSlot.DestroyChildren();

        var verticalLayoutSlot = scrollRectSlot.AddSlot("Vertical Layout");
        var verticalLayout = verticalLayoutSlot.AttachComponent<VerticalLayout>();
        verticalLayout.ForceExpandHeight.Value = false;
        verticalLayout.VerticalAlign.Value = LayoutVerticalAlignment.Top;

        // Setup size tracking via ProtoFlux
        var float2Field = verticalLayoutSlot.AttachComponent<ValueField<float2>>();
        var rectSizeDriver = verticalLayoutSlot.AttachComponent<RectSizeDriver>();
        rectSizeDriver.TargetSize.Target = float2Field.Value;

        var valueSource = verticalLayoutSlot.AttachComponent<ValueSource<float2>>();
        valueSource.TrySetRootSource(float2Field.Value);

        var unpack = verticalLayoutSlot.AttachComponent<Unpack_Float2>();
        unpack.V.Target = valueSource;

        var minWidthDriver = verticalLayoutSlot.AttachComponent<ValueFieldDrive<float>>();
        minWidthDriver.TrySetRootTarget(layoutElement.MinWidth);
        minWidthDriver.Value.Target = unpack.X;

        rootCategoryView.CategoryManager.ContainerRoot.Target = verticalLayoutSlot;

        // Mark as fixed to prevent reapplication
        facetSlot.AttachComponent<Comment>().Text.Value = FixedMarker;

        Plugin.Log.LogInfo("Settings scroll fix applied");
    }
}
