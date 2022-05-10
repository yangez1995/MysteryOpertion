using MysteryOpertion.Model;
using MysteryOpertion.Model.Roles.ChaosRoles;
using MysteryOpertion.Model.Roles.CrewmateRoles;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

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
                if(player.playerControl == PlayerControl.LocalPlayer && player.mainRole is MechanicExpert)
                    __instance.ImpostorVentButton.Show();

                ArsonExpertUpdate(player);
            }

            //foreach(var button in ButtonRespository.buttonDict)
            //{
            //    button.Value.OnHudManagerUpdate();
            //}
        }

        private static void ArsonExpertUpdate(Player player)
        {
            if (player.mainRole is not ArsonExpert) return;

            var role = (ArsonExpert)player.mainRole;
            if (role.oiledButton.OiledTarget == null) return;

            if (role.oiledButton.OiledTarget == role.oiledButton.Target)
            {
                if (role.oiledButton.timer <= 0)
                {
                    var playerId = role.oiledButton.OiledTarget.playerControl.PlayerId;
                    role.oiledPlayerIds.Add(playerId);
                    role.oiledButton.OiledTarget = null;
                    role.oiledButton.timer = role.oiledButton.cooldownTime;

                    if (PlayerIcons.iconDict.ContainsKey(playerId))
                    {
                        PlayerIcons.iconDict[playerId].setSemiTransparent(false);
                    }
                }
            }
            else
            {
                role.oiledButton.OiledTarget = null;
                role.oiledButton.timer = 0;
            }

            bool ArsonExpertWin = true;
            foreach (var playerControl in PlayerControl.AllPlayerControls)
            {
                if (playerControl != PlayerControl.LocalPlayer && !playerControl.Data.IsDead && !role.oiledPlayerIds.Contains(playerControl.PlayerId))
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
    }
}
