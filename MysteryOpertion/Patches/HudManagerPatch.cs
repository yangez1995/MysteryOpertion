using MysteryOpertion.Model;
using MysteryOpertion.Model.Roles.ChaosRoles;
using MysteryOpertion.Model.Roles.CrewmateRoles;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using MysteryOpertion.Model.Roles.ImpostorRoles;

namespace MysteryOpertion.Patches
{
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public static class HudManagerUpdatePatch
    {
        static void Postfix(HudManager __instance)
        {
            if (AmongUsClient.Instance.GameState != InnerNet.InnerNetClient.GameStates.Started) return;

            foreach(var player in Players.playerList)
            {
                player.UpdateButtons();
                if(player.PlayerControl == PlayerControl.LocalPlayer && player.MainRole is MechanicExpert)
                    __instance.ImpostorVentButton.Show();

                ArsonExpertUpdate(player);
                CommonSanityPointUpdate(player);
                SerialKillerSanityPointUpdate(player);
                ImpostorKillButtonUpdate(player, __instance);
            }
        }

        private static void ArsonExpertUpdate(Player player)
        {
            if (player.MainRole is not ArsonExpert) return;

            var role = (ArsonExpert)player.MainRole;
            if (role.oiledButton.OiledTarget == null) return;

            if (role.oiledButton.OiledTarget == role.oiledButton.Target)
            {
                if (role.oiledButton.Timer <= 0)
                {
                    var playerId = role.oiledButton.OiledTarget.PlayerControl.PlayerId;
                    role.oiledButton.OiledPlayerIds.Add(playerId);
                    role.oiledButton.OiledTarget = null;
                    role.oiledButton.Timer = role.oiledButton.CooldownTime;

                    if (PlayerIcons.iconDict.ContainsKey(playerId))
                    {
                        PlayerIcons.iconDict[playerId].setSemiTransparent(false);
                    }
                }
            }
            else
            {
                role.oiledButton.OiledTarget = null;
                role.oiledButton.Timer = 0;
            }

            bool ArsonExpertWin = true;
            foreach (var playerControl in PlayerControl.AllPlayerControls)
            {
                if (playerControl != PlayerControl.LocalPlayer && !playerControl.Data.IsDead && !role.oiledButton.OiledPlayerIds.Contains(playerControl.PlayerId))
                {
                    ArsonExpertWin = false;
                    break;
                }
            }
            if (ArsonExpertWin)
            {
                GameNote.ArsonExpertWin = true;
            }
                
        }

        private static void CommonSanityPointUpdate(Player player)
        {
            if (player.SanityPoint == player.MaxSanityPoint || player.MainRole is SerialKiller) return;

            player.SanityPointTimer -= Time.deltaTime;
            if(player.SanityPointTimer <= 0)
            {
                player.CalcSanityPoint(1);
                player.SanityPointTimer = 2;
            }
        }

        private static void SerialKillerSanityPointUpdate(Player player)
        {
            if(player.MainRole is not SerialKiller) return;

            player.SanityPointTimer -= Time.deltaTime;
            if (player.SanityPointTimer <= 0)
            {
                player.CalcSanityPoint(-1);
                player.SanityPointTimer = 2;
            }
        }

        private static void ImpostorKillButtonUpdate(Player player, HudManager __instance)
        {
            if (player.PlayerControl != PlayerControl.LocalPlayer || !player.PlayerControl.Data.Role.IsImpostor) return;

            if (player.MainRole is SerialKiller)
            {
                __instance.KillButton.Hide();
            }
            else
            {
                __instance.KillButton.Show();
            }
        }
    }
}
