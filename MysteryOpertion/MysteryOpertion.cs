using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using HarmonyLib;
using Hazel;
using MysteryOpertion.Model;
using Reactor;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static GameData;

namespace MysteryOpertion;

[BepInAutoPlugin]
[BepInProcess("Among Us.exe")]
[BepInDependency(ReactorPlugin.Id)]
public partial class MysteryOpertionPlugin : BasePlugin
{
    private static readonly System.Random random = new System.Random((int)DateTime.Now.Ticks);
    public static MysteryOpertionPlugin Instance;

    public Harmony Harmony { get; } = new(Id);

    public ConfigEntry<string> ConfigName { get; private set; }

    public override void Load()
    {
        Instance = this;
        ConfigLoader.Load();
        Harmony.PatchAll();
    }
}

// Debugging tools
[HarmonyPatch(typeof(KeyboardJoystick), nameof(KeyboardJoystick.Update))]
public static class DebugManager
{
    private static readonly System.Random random = new System.Random((int)DateTime.Now.Ticks);
    private static List<PlayerControl> bots = new List<PlayerControl>();

    public static void Postfix(KeyboardJoystick __instance)
    {
        // Spawn dummys
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            var playerControl = UnityEngine.Object.Instantiate(AmongUsClient.Instance.PlayerPrefab);
            var i = playerControl.PlayerId = (byte)GameData.Instance.GetAvailableId();

            bots.Add(playerControl);
            GameData.Instance.AddPlayer(playerControl);
            AmongUsClient.Instance.Spawn(playerControl, -2, InnerNet.SpawnFlags.None);

            playerControl.transform.position = PlayerControl.LocalPlayer.transform.position;
            playerControl.GetComponent<DummyBehaviour>().enabled = true;
            playerControl.NetTransform.enabled = false;
            playerControl.SetName(RandomString(10));
            playerControl.SetColor((byte)random.Next(Palette.PlayerColors.Length));
            playerControl.AddSystemTask(SystemTypes.Outside);
            GameData.Instance.RpcSetTasks(playerControl.PlayerId, new byte[2] { 1, 2 });
        }

        // Terminate round
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, 62, Hazel.SendOption.Reliable, -1);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            foreach (PlayerControl player in PlayerControl.AllPlayerControls)
            {
                if (!player.Data.Role.IsImpostor)
                {
                    player.RemoveInfected();
                    player.MurderPlayer(player);
                    player.Data.IsDead = true;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            ToolBox.ReSetPlayerTask(PlayerControl.LocalPlayer.PlayerId, 1, 1, 1);
        }
    }

    public static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
