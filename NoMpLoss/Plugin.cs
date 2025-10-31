using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;

namespace NoMpLoss;

[BepInPlugin("com.navneetaman.nomploss", MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    internal static ManualLogSource logger;
    private static ConfigEntry<bool> enableMod;
    private static ConfigEntry<bool> enableAll;

    private void Awake()
    {
        // Plugin startup logic
        logger = base.Logger;
        logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");

        enableMod = Config.Bind("General",      // The section under which the option is shown
            "enable",  // The key of the configuration option in the configuration file
            true, // The default value
            "Enable mod."
        );
        enableAll = Config.Bind("General",      // The section under which the option is shown
            "all",  // The key of the configuration option in the configuration file
            false, // The default value
            "Enable for non-heal mp usage."
        );
        if (enableMod.Value)
        {
            Harmony.CreateAndPatchAll(typeof(Plugin), null);
        }

    }

    [HarmonyPatch(typeof(PlayerData), nameof(PlayerData.TakeMP))]
    private static void Prefix(ref int amount)
    {
        if (enableAll.Value)
        {
            amount = 0;
        }
    }
    [HarmonyPatch(typeof(HeroController), nameof(HeroController.TakeMP))]
    private static void Prefix(HeroController __instance, ref int amount)
    {
        if (Traverse.Create(__instance).Field<bool>("drainMP").Value && amount == 1)
            amount = 0;
    }


}
