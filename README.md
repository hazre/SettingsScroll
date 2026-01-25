# SettingsScroll
[![Thunderstore Badge](https://modding.resonite.net/assets/available-on-thunderstore.svg)](https://thunderstore.io/c/resonite/)

A [Resonite](https://resonite.com/) mod that makes settings page scrollable. Fixes Resonite Issue [#2609](https://github.com/Yellow-Dog-Man/Resonite-Issues/issues/2609)

## Installation (Manual)
1. Install [BepisLoader](https://github.com/ResoniteModding/BepisLoader) for Resonite.
2. Download the latest release ZIP file (e.g., `hazre-SettingsScroll-1.0.0.zip`) from the [Releases](https://github.com/hazre/SettingsScroll/releases) page.
3. Extract the ZIP and copy the `plugins` folder to your BepInEx folder in your Resonite installation directory:
   - **Default location:** `C:\Program Files (x86)\Steam\steamapps\common\Resonite\BepInEx\`
4. Start the game. If you want to verify that the mod is working you can check your BepInEx logs.

## Configuration

The mod can be configured via `BepInEx/config/dev.hazre.settingsscroll.cfg`:

| Setting | Default | Description |
|---------|---------|-------------|
| `Enabled` | `true` | Enables the settings scroll fix. Requires a restart to take effect. |

## Acknowledgments

This mod's scroll fix logic is derived from [MonkeyLoader.GamePacks.Resonite](https://github.com/ResoniteModdingGroup/MonkeyLoader.GamePacks.Resonite). See [THIRD-PARTY-NOTICES.md](THIRD-PARTY-NOTICES.md) for details.

## License

This project is licensed under LGPL-3.0. See [LICENSE](LICENSE) for details.