using HarmonyLib;
using Photon.Voice.Unity;

namespace SoundGate.patches;
/// <summary>
/// Postfix patch that ensures that the assigned <see cref="Recorder"/> is configured with noise suppression, High Pass
/// (for reducing low frequencies such as fan hum) and applies the custom <see cref="NoiseGate"/> for the mod.
/// </summary>
[HarmonyPatch(typeof(VoiceClientHandler))]
internal static class VoiceClientHandlerPatch
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(VoiceClientHandler.LocalPlayerAssigned))]
    static void LocalPlayerAssigned_Postfix(Recorder r)
    {
        if (r == null) return;

        Plugin.LocalRecorder = r;

        r.VoiceDetection = true;
        r.VoiceDetectionDelayMs = 200;

        var rtcAudioDsp = r.GetComponent<WebRtcAudioDsp>() ?? r.gameObject.AddComponent<WebRtcAudioDsp>();
        rtcAudioDsp.NoiseSuppression = true;
        rtcAudioDsp.HighPass = true;
        //Developer note: AEC turned off by standard, because the noise supression honestly is more than enough
        rtcAudioDsp.AEC = false;
        NoiseGate.ApplyToLocalRecorder();
        Plugin.Log?.LogInfo("Configured Recorder VAD + WebRTC DSP");
    }
}
