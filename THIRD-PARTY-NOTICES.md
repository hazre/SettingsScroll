# Third-Party Notices

## MonkeyLoader.GamePacks.Resonite

The scroll fix in this mod is based on code from [MonkeyLoader.GamePacks.Resonite](https://github.com/ResoniteModdingGroup/MonkeyLoader.GamePacks.Resonite), specifically `SettingsFacetCategoryScrollingFix.cs`.

Licensed under LGPL-3.0.

### What's Changed

The core UIX fix logic is the same - I just ported it to work with BepisLoader instead of MonkeyLoader:

- Swapped MonkeyLoader's event system for a MonoDetour patch on `FacetPreset.OnLoading`
- Removed the `TemplateFacetPresetFallbackBuiltEvent` check since it doesn't apply here
- Kept the same marker text so both mods can coexist without applying the fix twice
