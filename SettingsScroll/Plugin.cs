using BepInEx;
using BepInEx.Logging;
using BepInEx.NET.Common;
using BepisResoniteWrapper;

namespace SettingsScroll;

[BepInAutoPlugin]
[BepInDependency(BepInExResoniteShim.PluginMetadata.GUID, BepInDependency.DependencyFlags.HardDependency)]
public partial class Plugin : BasePlugin
{
    internal static new ManualLogSource Log = null!;

    public override void Load()
    {
        Log = base.Log;
        ResoniteHooks.OnEngineReady += OnEngineReady;
        Log.LogInfo($"Plugin {GUID} is loaded!");
    }

    private void OnEngineReady()
    {
        // The Resonite engine is now fully initialized
        // Safe to access FrooxEngine classes and functionality
        Log.LogInfo("Engine is ready!");
    }
}
