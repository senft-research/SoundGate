using HarmonyLib;
using PEAKLib.UI;

namespace SoundGate.patches;
/// <summary>
/// Prefix patch for <see cref="MainMenu"/> that adds the localisation for the "Input Sensitivity" slider that is to
/// be added to the Audio Settings menu.
/// </summary>
[HarmonyPatch(typeof(MainMenu))]
internal static class MenuPatch
{
    [HarmonyPrefix]
    [HarmonyPatch(nameof(MainMenu.Start))]
    static void Start_Prefix(MainMenu __instance)
    {
        Plugin.Log.LogInfo("Did the sensitivity stuff");
        MenuAPI.CreateLocalization("input_sensitivity")
            .AddLocalization("Input Sensitivity", LocalizedText.Language.English);
    }
}

