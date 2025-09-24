using Photon.Voice.Unity;
using UnityEngine;

namespace SoundGate;

/// <summary>
/// Provides a configurable noise gate for the game's Photon Voice Recorder by utilising its in-built "voice detection
/// threshold". 
/// </summary>
internal static class NoiseGate
{
    public static BepInExFloat Sensitivity { get; private set; }

    /// <summary>
    /// Initializes the Noise Gate's "Input Sensitivity" slider and adds it to the Vanilla Audio Settings 
    /// </summary>
    public static void InitSettings()
    {
        Sensitivity = new BepInExFloat(
            "input_sensitivity",
            0.5f,
            "Audio",
            0.0f,
            1.0f,
            0f,
            saveCallback: v => ApplyThresholdTo(Plugin.LocalRecorder, v),
            onApply: s => ApplyThresholdTo(Plugin.LocalRecorder, s.Value)
        );
        SettingsHandler.Instance.AddSetting(Sensitivity);
        
    }

    public static void ApplyToLocalRecorder()
    {
        ApplyThresholdTo(Plugin.LocalRecorder, Sensitivity?.Value ?? 0.5f);
    }

    static void ApplyThresholdTo(Recorder recorder, float value01)
    {
        if (recorder == null) return;

        recorder.VoiceDetectionThreshold = Mathf.Clamp01(value01);
        Plugin.Log?.LogInfo($"VAD threshold set -> {recorder.VoiceDetectionThreshold:0.###}");
    }
}
