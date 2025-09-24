using HarmonyLib;

namespace SoundGate.patches;
/// <summary>
/// Patch for <see cref="AnimatedMouth"/> that ensures that the mouth animation trigger accounts for the new
/// <see cref="NoiseGate"/> implemented on the game's Photon Voice Recorder. Without this patch, the mouth would still
/// move, even if the Noise Gate has prevented transmission. 
/// </summary>
[HarmonyPatch(typeof(AnimatedMouth))]
internal static class AnimatedMouthPatch
{
    
    [HarmonyPostfix]
    [HarmonyPatch("ProcessMicData")]
    static void ProcessMicData_Postfix(AnimatedMouth __instance)
    {
        if (__instance == null) return;
        var character = __instance.character;

        if (character == null || !character.IsLocal || __instance.isGhost)
            return;
        
        bool gateOpen = IsRecorderTransmitting();
        
        if (gateOpen)
            return;
        
        __instance.isSpeaking = false;
        
        if (__instance.mouthRenderer == null)
            return;
        
        __instance.mouthRenderer.material.SetInt("_UseTalkSprites", 0);

        if (__instance.mouthTextures == null || __instance.mouthTextures.Length <= 0)
            return;
        
        __instance.amplitudeIndex = 0;
        __instance.mouthRenderer.material.SetTexture(
            "_TalkSprite",
            __instance.mouthTextures[0]);
    }

    static bool IsRecorderTransmitting()
    {
        var recorder = Plugin.LocalRecorder;
        if (recorder == null) return true;
        try
        {
            return recorder.IsCurrentlyTransmitting ||
                   (recorder.VoiceDetection && recorder.VoiceDetector != null && recorder.VoiceDetector.Detected);
        }
        catch
        {
            return true;
        }
    }
    
}
