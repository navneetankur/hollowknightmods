using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace NoHazardHealthLoss;

[BepInPlugin("com.navneetaman.nohazardhealthloss", MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;
    internal static bool hazardDamage = false;

    private void Awake()
    {
        // Plugin startup logic
        Logger = base.Logger;
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        Harmony.CreateAndPatchAll(typeof(Plugin), null);

    }

    [HarmonyPatch(typeof(HeroController), nameof(HeroController.TakeDamage))]
    private static void Prefix(int hazardType)
    {
        if (hazardType >= 2 && hazardType <= 5)
        {
            hazardDamage = true;
            // Logger.LogInfo("tdb: hazard damage. "+hazardType);
        }
        else
        {
            hazardDamage = false;
            // Logger.LogInfo("tdb: not hazard damage. " + hazardType);
        }
            
    }
    [HarmonyPatch(typeof(HeroController), nameof(HeroController.TakeDamage))]
    private static void Postfix(int hazardType)
    {
        hazardDamage = false;
        // Logger.LogInfo("tda: set hazardDamage false");
    }
    [HarmonyPatch(typeof(PlayerData), nameof(PlayerData.TakeHealth))]
    private static void Prefix(ref int amount)
    {
        if (hazardDamage)
        {
            amount = 0;// don't take damage
            // Logger.LogInfo("thb: don't take damage.");
        }
        else
        {
            // Logger.LogInfo("thb: take damage. "+amount);
        }
            
    }


}
