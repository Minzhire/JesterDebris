using System.Diagnostics.CodeAnalysis;
using HarmonyLib;
using UnityEngine;

namespace JesterDebris;

[HarmonyPatch(typeof(JesterAI))]
public static class JesterAiPatch {
    [HarmonyPatch(nameof(JesterAI.Start))]
    [HarmonyPrefix]
    public static void JesterDebrisPatch(
        [SuppressMessage("ReSharper", "InconsistentNaming")] ref AudioClip ___popGoesTheWeaselTheme,
        [SuppressMessage("ReSharper", "InconsistentNaming")] ref AudioClip ___screamingSFX,
        [SuppressMessage("ReSharper", "InconsistentNaming")] ref AudioSource ___farAudio,
        [SuppressMessage("ReSharper", "InconsistentNaming")] ref AudioSource ___creatureVoice
        ) {
        if (JesterDebris.Instance.CrankEnabled.Value) {
            ___popGoesTheWeaselTheme = JesterDebris.Instance.Crank;
            ___farAudio.volume = JesterDebris.Instance.CrankVolume.Value / 100.0f;
        }
        if (JesterDebris.Instance.ScreamEnabled.Value) {
            ___screamingSFX = JesterDebris.Instance.Scream;
            ___creatureVoice.volume = JesterDebris.Instance.ScreamVolume.Value / 100.0f;
        }
    }
}