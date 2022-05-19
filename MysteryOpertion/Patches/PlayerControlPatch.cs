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

            if (Players.GetLocalPlayer()?.PlayerControl != __instance) return;

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
                if(player.PlayerControl == PlayerControl.LocalPlayer || true)
                {
                    //变更玩家名显示
                    Transform playerInfoTransform = player?.PlayerControl?.nameText?.transform?.parent?.FindChild("Info");
                    TMPro.TextMeshPro playerInfo = playerInfoTransform?.GetComponent<TMPro.TextMeshPro>();
                    if (playerInfo == null)
                    {
                        playerInfo = UnityEngine.Object.Instantiate(player.PlayerControl.nameText, player.PlayerControl.nameText.transform.parent);
                        playerInfo.transform.localPosition += Vector3.up * 0.5f;
                        playerInfo.fontSize *= 0.75f;
                        playerInfo.gameObject.name = "Info";
                    }

                    var text = $"{player.GetRoleName()} ({player.SanityPoint}/{player.MaxSanityPoint})";
                    playerInfo.text = text;
                    playerInfo.color = player.GetRoleColor();
                    player.PlayerControl.nameText.color = player.GetRoleColor();

                    if(MeetingHud.Instance)
                    {
                        //变更会议时玩家名显示
                        PlayerVoteArea playerVoteArea = MeetingHud.Instance?.playerStates?.FirstOrDefault(x => x.TargetPlayerId == player.PlayerControl.PlayerId);
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
            foreach(var btn in player.MainRole.GetButtons().Values)
            {
                if(btn is TargetedButton)
                    ((TargetedButton)btn).UpdateTarget();
            }
        }

        private static void updatePlayerTaskDescription(Player player)
        {
            if(player.PlayerControl.myTasks.Count > 0)
            {
                var firstTask = player.PlayerControl.myTasks[0];
                var taskText = firstTask.gameObject.GetComponent<ImportantTextTask>()?.Text;
                if(taskText is null)
                {
                    var newTask = new GameObject("RoleTask").AddComponent<ImportantTextTask>();
                    newTask.Text = ToolBox.FormatRoleTaskText(player.MainRole.GetRoleColor(), $"{TextDictionary.Role}-{player.MainRole.GetRoleName()}:\n{player.MainRole.GetRoleBlurb()}");
                    player.PlayerControl.myTasks.Insert(0, newTask);
                }
                else if(taskText.StartsWith(TextDictionary.Role) && !taskText.Contains(player.MainRole.GetRoleName()))
                {
                    var newTask = new GameObject("RoleTask").AddComponent<ImportantTextTask>();
                    newTask.Text = ToolBox.FormatRoleTaskText(player.MainRole.GetRoleColor(), $"{TextDictionary.Role}-{player.MainRole.GetRoleName()}:\n{player.MainRole.GetRoleBlurb()}");
                    player.PlayerControl.myTasks[0] = newTask;
                    firstTask.OnRemove();
                    UnityEngine.Object.Destroy(firstTask.gameObject);
                }
            }
            else
            {
                var newTask = new GameObject("RoleTask").AddComponent<ImportantTextTask>();
                newTask.Text = ToolBox.FormatRoleTaskText(player.MainRole.GetRoleColor(), $"{TextDictionary.Role}-{player.MainRole.GetRoleName()}:\n{player.MainRole.GetRoleBlurb()}");
                player.PlayerControl.myTasks.Insert(0, newTask);
            }
        }

        private static void updateDoomDaitArrow()
        {
            foreach(var player in Players.playerList)
            {
                if (player.MainRole is not DoomBait || !player.PlayerControl.Data.IsDead) continue;


            }
        }
    }

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.MurderPlayer))]
    public static class MurderPlayerPatch
    {
        public static void Postfix(PlayerControl __instance, [HarmonyArgument(0)] PlayerControl target)
        {
            var sourcePlayer = Players.GetPlayer(__instance);
            var targetPlayer = Players.GetPlayer(target);

            //记录死亡报告
            DeathRecord deathRecord = new DeathRecord();
            deathRecord.KillerPlayer = sourcePlayer;
            deathRecord.deadPlayer = targetPlayer;
            Debug.Log(deathRecord.KillerPlayer.MainRole.GetRoleName() + "|" + deathRecord.deadPlayer.MainRole.GetRoleName());

            if (sourcePlayer == targetPlayer) deathRecord.Cause = CauseOfDeath.Suicide;
            else if (sourcePlayer.MainRole is Sheriff) deathRecord.Cause = CauseOfDeath.Sheriffkill;
            else deathRecord.Cause = CauseOfDeath.CommonImpostorKill;

            //计算凶案现场人数
            var sourcePosition = target.GetTruePosition();
            float distances = GameOptionsData.KillDistances[Mathf.Clamp(PlayerControl.GameOptions.KillDistance, 0, 2)] * 1.5f;
            foreach (var playerInfo in GameData.Instance.AllPlayers)
            {
                if (playerInfo.Disconnected || playerInfo.IsDead) continue;

                var obj = playerInfo.Object;
                if (obj == null) continue;

                Vector2 vector = obj.GetTruePosition() - sourcePosition;
                if (vector.magnitude <= distances && !PhysicsHelpers.AnyNonTriggersBetween(sourcePosition, vector.normalized, vector.magnitude, Constants.ShipAndObjectsMask))
                {
                    deathRecord.MurderScenePeopleCount++;
                }
            }
            Debug.Log(deathRecord.MurderScenePeopleCount.ToString());
            GameNote.DeathRecords.Add(deathRecord);

            //厄运诱饵被击杀时事件
            if (targetPlayer.MainRole is DoomBait)
            {
                ToolBox.showFlash(targetPlayer.MainRole.GetRoleColor(), 1);

                if(__instance == PlayerControl.LocalPlayer)
                {
                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)RPCFuncType.CalcSanityPoint, SendOption.Reliable);
                    writer.Write(sourcePlayer.PlayerControl.PlayerId);
                    writer.Write(-20);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    RPCFunctions.CalcSanityPoint(sourcePlayer.PlayerControl.PlayerId, -20);
                }
            }
        }
    }

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.CompleteTask))]
    class PlayerControlCompleteTaskPatch
    {
        public static void Postfix(PlayerControl __instance, [HarmonyArgument(0)] uint idx)
        {
            int offset = 0;
            foreach(var task in __instance.myTasks)
            {
                if (task.gameObject.name == "RoleTask")
                    offset++;
                else
                    break;
            }

            GameNote.TaskComplete.Add(new TaskRecord { Player = __instance, TaskName = __instance.myTasks[(int)idx + offset].TaskType.ToString() });
            var player = Players.GetLocalPlayer();
            if (!__instance.AllTasksCompleted() || player is null || player.MainRole is not Eavesdropper) return;

            ((Eavesdropper)player.MainRole).ShowAllTaskCompleteMessage(__instance);
        }
    }
}
