using System.IO;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using LCSoundTool;
using UnityEngine;

namespace JesterDebris
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    [BepInDependency("LCSoundTool")]
    public class JesterDebris : BaseUnityPlugin
    {
        private const string PLUGIN_GUID = "JesterDebris";
        public static JesterDebris Instance { get; private set; }

        private readonly Harmony _harmony = new(PLUGIN_GUID);

        public AudioClip Crank { get; private set; }
        public AudioClip Scream { get; private set; }

        public ConfigEntry<bool> CrankEnabled { get; private set; }
        public ConfigEntry<int> CrankVolume { get; private set; }
        public ConfigEntry<bool> ScreamEnabled { get; private set; }
        public ConfigEntry<int> ScreamVolume { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Logger.Log(LogLevel.Info, $"{PLUGIN_GUID} instance already running");
                return;
            }
            Instance = this;

            var isEnabled = Config.Bind("Mod", "EnableMod", true, "Enables the mod");
            CrankEnabled = Config.Bind("Sound", "EnableCrank", true, "Enables the cranking to be replaced by Greenwich Debris");
            CrankVolume = Config.Bind("Sound", "CrankVolume", 100, new ConfigDescription("Sets the volume of the cranking phase (in %)", new AcceptableValueRange<int>(0, 100)));
            ScreamEnabled = Config.Bind("Sound", "EnableScream", true, "Enables the scream to be replaced by Greenwich Debris");
            ScreamVolume = Config.Bind("Sound", "ScreamVolume", 100, new ConfigDescription("Sets the volume of the scream phase (in %)", new AcceptableValueRange<int>(0, 100)));

            if (!isEnabled.Value)
            {
                Logger.Log(LogLevel.Info, "JesterDebris is disabled in config!");
                return;
            }

            Logger.Log(LogLevel.Info, $"{PLUGIN_GUID} is starting!");

            Crank = SoundTool.GetAudioClip(Path.GetDirectoryName(Info.Location), "crankwich.wav");
            Scream = SoundTool.GetAudioClip(Path.GetDirectoryName(Info.Location), "debream.wav");

            _harmony.PatchAll(typeof(JesterAiPatch));
            _harmony.PatchAll(typeof(JesterDebris));

            Logger.Log(LogLevel.Info, $"{PLUGIN_GUID} is loaded.");
        }
    }
}