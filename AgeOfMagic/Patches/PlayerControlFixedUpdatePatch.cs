using AgeOfMagic.Entities;
using AgeOfMagic.Entities.Roles.CrewmateRoles;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AgeOfMagic.Patches
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.FixedUpdate))]
    internal class PlayerControlFixedUpdatePatch
    {
        public static void Postfix(PlayerControl __instance)
        {
            if (AmongUsClient.Instance.GameState != InnerNet.InnerNetClient.GameStates.Started) return;

            if (PlayerControl.LocalPlayer != __instance) return;

            updatePlayerOutline();
            updatePlayerInfo();
            updatePlayerTarget();
        }

        //private static void updateRoleDescription(PlayerControl player)
        //{
        //    if(player == null) return;  


        //}

        private static void updatePlayerOutline()
        {
            foreach (var player in PlayerControl.AllPlayerControls)
            {
                if (player == null || player.MyRend == null) 
                    continue;

                player.MyRend.material.SetFloat("_Outline", 0f);
            }
        }

        private static void updatePlayerInfo()
        {
            foreach (var player in Players.playerList)
            {
                if(player.playerControl == PlayerControl.LocalPlayer || true)
                {
                    //变更玩家名显示
                    Transform playerInfoTransform = player?.playerControl?.nameText?.transform?.parent?.FindChild("Info");
                    TMPro.TextMeshPro playerInfo = playerInfoTransform?.GetComponent<TMPro.TextMeshPro>();
                    if (playerInfo == null)
                    {
                        playerInfo = UnityEngine.Object.Instantiate(player.playerControl.nameText, player.playerControl.nameText.transform.parent);
                        playerInfo.transform.localPosition += Vector3.up * 0.5f;
                        playerInfo.fontSize *= 0.75f;
                        playerInfo.gameObject.name = "Info";
                    }

                    playerInfo.text = player.GetRoleName();
                    playerInfo.color = player.GetRoleColor();
                    player.playerControl.nameText.color = player.GetRoleColor();

                    //变更会议时玩家名显示
                    //PlayerVoteArea playerVoteArea = MeetingHud.Instance?.playerStates?.FirstOrDefault(x => x.TargetPlayerId == player.playerControl.PlayerId);
                    //Transform meetingInfoTransform = playerVoteArea != null ? playerVoteArea.NameText.transform.parent.FindChild("Info") : null;
                    //TMPro.TextMeshPro meetingInfo = meetingInfoTransform != null ? meetingInfoTransform.GetComponent<TMPro.TextMeshPro>() : null;
                    //if (meetingInfo == null && playerVoteArea != null)
                    //{
                    //    meetingInfo = UnityEngine.Object.Instantiate(playerVoteArea.NameText, playerVoteArea.NameText.transform.parent);
                    //    meetingInfo.transform.localPosition += Vector3.down * 0.10f;
                    //    meetingInfo.fontSize *= 0.60f;
                    //    meetingInfo.gameObject.name = "Info";
                    //}

                    //// Set player name higher to align in middle
                    //if (meetingInfo != null && playerVoteArea != null)
                    //{
                    //    var playerName = playerVoteArea.NameText;
                    //    playerName.transform.localPosition = new Vector3(0.3384f, (0.0311f + 0.0683f), -0.1f);
                    //}

                    //meetingInfo.text = player.GetRoleName();
                    //meetingInfo.color = player.GetRoleColor();
                }
            }
        }

        private static void updatePlayerTarget()
        {
            if (Players.localPlayer == null) return;

            switch (Players.localPlayer.mainRole.GetRoleName())
            {
                case RoleNameDictionary.Sheriff:
                    var role = (Sheriff)Players.localPlayer.mainRole;
                    role.SkillTarget = getTarget();
                    setTargetOutline(role.SkillTarget?.playerControl, role.GetRoleColor());
                    break;
                default:
                    break;
            }
        }

        private static Player getTarget(bool canTargetPlayerInVents = false)
        {
            PlayerControl target = null;
            float distances = GameOptionsData.KillDistances[Mathf.Clamp(PlayerControl.GameOptions.KillDistance, 0, 2)];

            if (!ShipStatus.Instance) 
                return null;

            var sourcePosition = PlayerControl.LocalPlayer.GetTruePosition();

            var playerInfos = GameData.Instance.AllPlayers;
            foreach(var playerInfo in playerInfos)
            {
                if(playerInfo.Disconnected || playerInfo.IsDead || playerInfo.PlayerId == PlayerControl.LocalPlayer.PlayerId)
                    continue;

                var obj = playerInfo.Object;
                if (obj == null)
                    continue;

                if (!obj.inVent || canTargetPlayerInVents)
                {
                    Vector2 vector = obj.GetTruePosition() - sourcePosition;
                    if (vector.magnitude <= distances && !PhysicsHelpers.AnyNonTriggersBetween(sourcePosition, vector.normalized, vector.magnitude, Constants.ShipAndObjectsMask))
                    {
                        target = obj;
                        distances = vector.magnitude;
                    }
                }
            }

            return target == null ? null : Players.playerList.FirstOrDefault(it => it.playerControl == target);
        }

        private static void setTargetOutline(PlayerControl target, Color color)
        {
            if (target == null || target.MyRend == null) 
                return;

            target.MyRend.material.SetFloat("_Outline", 1f);
            target.MyRend.material.SetColor("_OutlineColor", color);
        }
    }
}
