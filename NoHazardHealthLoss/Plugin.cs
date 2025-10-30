using System.Runtime.CompilerServices;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace NoHazardHealthLoss;

[BepInPlugin("com.navneetaman.nohazardhealthloss", MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;
    internal static int? prevHealth = null;

    private void Awake()
    {
        // Plugin startup logic
        Logger = base.Logger;
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        Harmony.CreateAndPatchAll(typeof(Plugin), null);

    }

    [HarmonyPatch(typeof(HeroController), nameof(HeroController.HazardRespawn))]
    private static void Postfix(HeroController __instance)
    {
        var pd = __instance.playerData;
        if(!Plugin.prevHealth.HasValue)
        {
            Logger.LogError("Got to hazard fn without setting prevHealth correctly.");
            return;
        }
        int prevHealth = Plugin.prevHealth.Value;
        if(pd.health > prevHealth)
        {
            Logger.LogError("Health increased after hazard? How?");
            return;
        }
        pd.health = prevHealth;
    }
    [HarmonyPatch(typeof(PlayerData), nameof(PlayerData.TakeHealth))]
    private static void Prefix(PlayerData __instance)
    {
        prevHealth = __instance.health;
    }


}
