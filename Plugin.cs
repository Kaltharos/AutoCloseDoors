using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using BepInEx.Logging;
using HarmonyLib;
using System.Reflection;
using Unity.Entities;
using UnityEngine;
using AutoCloseDoors.Systems;
using AutoCloseDoors.Config;
using AutoCloseDoors.Hooks;

#if WETSTONE
using Wetstone.API;
#endif

[assembly: AssemblyVersion(BuildConfig.Version)]
[assembly: AssemblyTitle(BuildConfig.Name)]

namespace AutoCloseDoors
{
    [BepInPlugin(BuildConfig.PackageID, BuildConfig.Name, BuildConfig.Version)]

#if WETSTONE
    [BepInDependency("xyz.molenzwiebel.wetstone")]
    [Reloadable]
    public class Plugin : BasePlugin, IRunOnInitialized
#else
    public class Plugin : BasePlugin
#endif
    {
        private Harmony harmony;

        private static ConfigEntry<bool> AutoCloseDoors;
        private static ConfigEntry<float> AutoCloseDoors_Timer;
        private static ConfigEntry<bool> AlwaysAutoClose;
        private static ConfigEntry<bool> EnableUninstall;
        private static ConfigEntry<string> UninstallCommand;

        public static bool isInitialized = false;

        public static ManualLogSource Logger;

        private static World _serverWorld;
        public static World Server
        {
            get
            {
                if (_serverWorld != null) return _serverWorld;

                _serverWorld = GetWorld("Server")
                    ?? throw new System.Exception("There is no Server world (yet). Did you install a server mod on the client?");
                return _serverWorld;
            }
        }

        public static bool IsServer => Application.productName == "VRisingServer";

        private static World GetWorld(string name)
        {
            foreach (var world in World.s_AllWorlds)
            {
                if (world.Name == name)
                {
                    return world;
                }
            }

            return null;
        }

        public void InitConfig()
        {
            AutoCloseDoors = Config.Bind("Config", "Enable Auto Close Doors", true, "Switch on/off auto close for doors.");
            AutoCloseDoors_Timer = Config.Bind("Config", "Auto Close Timer", 2.0f, "How many second(s) to wait before door is automatically closed.");
            AlwaysAutoClose = Config.Bind("Config", "Always Auto Close Doors", false, "When this is set to false, doors will not automatically close if castle is decaying, under attack, or being sieged.");
            EnableUninstall = Config.Bind("Config", "Enable Uninstall", false, "Do not enable for better performance on server.\n" +
                "This uninstallation method is only required on servers that can't shutdown properly, like VRising on Linux Wine.\n" +
                "On Windows, servers can be shutdown properly, and all doors is by default reverted back to normal on server shutdown.");
            UninstallCommand = Config.Bind("Config", "Uninstall Command", "~autoclosedooruninstall", "Chat command to uninstall mod. Only work if \"Enable Uninstall\" is set to true & the user is an Admin (adminauth).");
        }

        public override void Load()
        {
            InitConfig();
            Logger = Log;
            harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());

            Log.LogInfo($"Plugin {BuildConfig.Name}-v{BuildConfig.Version} is loaded!");
        }

        public override bool Unload()
        {
            Config.Clear();
            harmony.UnpatchSelf();
            return true;
        }

        public void OnGameInitialized()
        {
            if (isInitialized) return;
            AutoCloseDoor.isAutoCloseDoor = AutoCloseDoors.Value;
            AutoCloseDoor.AutoCloseTimer = AutoCloseDoors_Timer.Value;
            AutoCloseDoor.isAlwaysAutoClose = AlwaysAutoClose.Value;
            AutoCloseDoor.isEnableUninstall = EnableUninstall.Value;
            AutoCloseDoor.UninstallCommand = UninstallCommand.Value;
            isInitialized = true;
        }
    }
}
