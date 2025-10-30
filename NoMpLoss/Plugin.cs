using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;

namespace NoMpLoss;

[BepInPlugin("com.navneetaman.nomploss", MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    internal static ManualLogSource logger;
    private ConfigEntry<bool> config;

    private void Awake()
    {
        // Plugin startup logic
        logger = base.Logger;
        logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");

        config = Config.Bind("General",      // The section under which the option is shown
                    "enable",  // The key of the configuration option in the configuration file
                    true, // The default value
                    "Enable mod."
                );
        if (config.Value)
        {
            Harmony.CreateAndPatchAll(typeof(Plugin), null);
        }

    }
    
    [HarmonyPatch(typeof(PlayerData), nameof(PlayerData.TakeMP))]
    private static void Prefix(ref int amount)
    {
        amount = 0;
    }


}
