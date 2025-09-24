using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using PEAKLib.UI;
using Photon.Voice.Unity;
using UnityEngine;

namespace SoundGate;

[BepInAutoPlugin]
public partial class Plugin : BaseUnityPlugin
{

    void Start()
    {
        NoiseGate.InitSettings();
    }
    internal static Recorder LocalRecorder;

    internal static ManualLogSource Log { get; private set; } = null!;
    GameObject _pageGO;

    void Awake()
    {
        Log = Logger;
        new Harmony("io.senftresearch.soundgate").PatchAll();
        Log.LogInfo("Noise Gate mod loaded");
    }

    
}