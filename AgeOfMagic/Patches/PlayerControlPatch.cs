using MysteryOpertion.Model;
using MysteryOpertion.Model.Buttons;
using MysteryOpertion.Model.Buttons.Implement;
using MysteryOpertion.Model.Roles.ChaosRoles;
using MysteryOpertion.Model.Roles.CrewmateRoles;
using MysteryOpertion.Model.Roles.ImpostorRoles;
using HarmonyLib;
using Hazel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static GameData;

namespace MysteryOpertion.Patches
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.FixedUpdate))]
    class PlayerControlFixedUpdatePatch
    {
        public static void Postfix(PlayerControl __instance)
        {
            if (AmongUsClient.Instance.GameState != InnerNet.InnerNetClient.GameStates.Started) return;

            if (Players.GetLocalPlayer()?.playerControl != __instance) return;

            var player = Players.GetLocalPlayer();
            if(player is null) return;

            updatePlayerOutline();
            updatePlayerInfo();
            updatePlayerTarget(player);
            updatePlayerTaskDescription(player);

            updateDoomDaitArrow();
        }

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

                    var text = $"{player.GetRoleName()} ({player.sanityPoint}/{player.maxSanityPoint})";
                    playerInfo.text = text;
                    playerInfo.color = player.GetRoleColor();
                    player.playerControl.nameText.color = player.GetRoleColor();

                    if(MeetingHud.Instance)
                    {
                        //变更会议时玩家名显示
                        PlayerVoteArea playerVoteArea = MeetingHud.Instance?.playerStates?.FirstOrDefault(x => x.TargetPlayerId == player.playerControl.PlayerId);
                        Transform meetingInfoTransform = playerVoteArea?.NameText?.transform?.parent?.FindChild("Info");
                        TMPro.TextMeshPro meetingInfo = meetingInfoTransform?.GetComponent<TMPro.TextMeshPro>();
                        if (meetingInfo == null && playerVoteArea != null)
                        {
                            meetingInfo = UnityEngine.Object.Instantiate(playerVoteArea?.NameText, playerVoteArea?.NameText?.transform?.parent);
                            meetingInfo.transform.localPosition += Vector3.down * 0.10f;
                            meetingInfo.fontSize *= 0.60f;
                            meetingInfo.gameObject.name = "Info";
                        }

                        // Set player name higher to align in middle
                        if (meetingInfo != null && playerVoteArea != null)
                        {
                            var playerName = playerVoteArea.NameText;
                            playerName.transform.localPosition = new Vector3(0.3384f, (0.0311f + 0.0683f), -0.1f);

                            meetingInfo.text = text;
                            meetingInfo.color = player.GetRoleColor();
                        }
                    }
                }
            }
        }

        private static void updatePlayerTarget(Player player)
        {
            switch (player.mainRole.GetRoleName())
            {
                case RoleNameDictionary.Sheriff:
                    var sheriff = (Sheriff)player.mainRole;
                    sheriff.sheriffKillButton.Target = getTarget();
                    setTargetOutline(sheriff.sheriffKillButton.Target?.playerControl, sheriff.GetRoleColor());
                    break;
                case RoleNameDictionary.Traveller:
                    var traveller = (Traveller)player.mainRole;
                    if (traveller.travelPlayerButton.MarkedPlayer == null)
                    {
                        traveller.travelPlayerButton.TargetPlayer = getTarget();
                        setTargetOutline(traveller.travelPlayerButton.TargetPlayer?.playerControl, traveller.GetRoleColor());
                    }
                    else
                    {
                        setTargetOutline(traveller.travelPlayerButton.MarkedPlayer?.playerControl, traveller.GetRoleColor());
                    }
                    break;
                case RoleNameDictionary.LightPrayer:
                    var lightPrayer = (LightPrayer)player.mainRole;
                    lightPrayer.blessButton.Target = getTarget();
                    setTargetOutline(lightPrayer.blessButton.Target?.playerControl, lightPrayer.GetRoleColor());
                    break;
                case RoleNameDictionary.NoneFaceMan:
                    var noneFaceMan = (NoneFaceMan)player.mainRole;
                    noneFaceMan.samplingButton.Target = getTarget();
                    setTargetOutline(noneFaceMan.samplingButton.Target?.playerControl, noneFaceMan.GetRoleColor());
                    break;
                case RoleNameDictionary.ArsonExpert:
                    var arsonExpert = (ArsonExpert)player.mainRole;
                    arsonExpert.oiledButton.Target = getTarget(hodePlayer: arsonExpert.oiledButton.OiledTarget?.playerControl.Data, excludeIdList: arsonExpert.oiledPlayerIds);
                    setTargetOutline(arsonExpert.oiledButton.Target?.playerControl, arsonExpert.GetRoleColor());
                    break;
                default:
                    break;
            }
        }

        private static void updatePlayerTaskDescription(Player player)
        {
            if(player.playerControl.myTasks.Count > 0)
            {
                var firstTask = player.playerControl.myTasks[0];
                var taskText = firstTask.gameObject.GetComponent<ImportantTextTask>()?.Text;
                if(taskText is null)
                {
                    var newTask = new GameObject("RoleTask").AddComponent<ImportantTextTask>();
                    newTask.Text = ToolBox.FormatRoleTaskText(player.mainRole.GetRoleColor(), $"{TextDictionary.Role}-{player.mainRole.GetRoleName()}:\n{player.mainRole.GetRoleBlurb()}");
                    player.playerControl.myTasks.Insert(0, newTask);
                }
                else if(taskText.StartsWith(TextDictionary.Role) && !taskText.Contains(player.mainRole.GetRoleName()))
                {
                    var newTask = new GameObject("RoleTask").AddComponent<ImportantTextTask>();
                    newTask.Text = ToolBox.FormatRoleTaskText(player.mainRole.GetRoleColor(), $"{TextDictionary.Role}-{player.mainRole.GetRoleName()}:\n{player.mainRole.GetRoleBlurb()}");
                    player.playerControl.myTasks[0] = newTask;
                    firstTask.OnRemove();
                    UnityEngine.Object.Destroy(firstTask.gameObject);
                }
            }
            else
            {
                var newTask = new GameObject("RoleTask").AddComponent<ImportantTextTask>();
                newTask.Text = ToolBox.FormatRoleTaskText(player.mainRole.GetRoleColor(), $"{TextDictionary.Role}-{player.mainRole.GetRoleName()}:\n{player.mainRole.GetRoleBlurb()}");
                player.playerControl.myTasks.Insert(0, newTask);
            }
        }

        private static void updateDoomDaitArrow()
        {
            foreach(var player in Players.playerList)
            {
                if (player.mainRole is not DoomBait || !player.playerControl.Data.IsDead) continue;


            }
        }

        private static Player getTarget(bool canTargetPlayerInVents = false, PlayerInfo hodePlayer = null, List<byte> excludeIdList = null)
        {
            PlayerControl target = null;
            float distances = GameOptionsData.KillDistances[Mathf.Clamp(PlayerControl.GameOptions.KillDistance, 0, 2)];

            if (!ShipStatus.Instance) 
                return null;

            var sourcePosition = PlayerControl.LocalPlayer.GetTruePosition();

            //判断持续目标是否脱离范围
            if(hodePlayer is not null)
            {
                Vector2 vector = hodePlayer.Object.GetTruePosition() - sourcePosition;
                if (vector.magnitude <= distances && !PhysicsHelpers.AnyNonTriggersBetween(sourcePosition, vector.normalized, vector.magnitude, Constants.ShipAndObjectsMask))
                {
                    return Players.GetPlayer(hodePlayer.Object);
                }
            }

            var playerInfos = GameData.Instance.AllPlayers;
            foreach(var playerInfo in playerInfos)
            {
                if(playerInfo.Disconnected || playerInfo.IsDead || playerInfo.PlayerId == PlayerControl.LocalPlayer.PlayerId) continue;

                if (excludeIdList != null && excludeIdList.Contains(playerInfo.PlayerId)) continue;

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

            return target == null ? null : Players.GetPlayer(target);
        }

        private static void setTargetOutline(PlayerControl target, Color color)
        {
            if (target == null || target.MyRend == null) 
                return;

            target.MyRend.material.SetFloat("_Outline", 1f);
            target.MyRend.material.SetColor("_OutlineColor", color);
        }
    }

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.MurderPlayer))]
    public static class MurderPlayerPatch
    {
        public static void Postfix(PlayerControl __instance, [HarmonyArgument(0)] PlayerControl target)
        {
            var sourcePlayer = Players.GetPlayer(__instance);
            var targetPlayer = Players.GetPlayer(target);
            if (targetPlayer.mainRole is DoomBait)
            {
                ToolBox.showFlash(targetPlayer.mainRole.GetRoleColor(), 1);

                if(__instance == PlayerControl.LocalPlayer)
                {
                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)RPCFuncType.CalcSanityPoint, SendOption.Reliable);
                    writer.Write(sourcePlayer.playerControl.PlayerId);
                    writer.Write(-20);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    RPCFunctions.CalcSanityPoint(sourcePlayer.playerControl.PlayerId, -20);
                }
            }
        }
    }
}
