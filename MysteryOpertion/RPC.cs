using MysteryOpertion.Model;
using MysteryOpertion.Model.Roles;
using MysteryOpertion.Model.Roles.CrewmateRoles;
using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnhollowerBaseLib;
using UnityEngine;

namespace MysteryOpertion
{
    public enum RPCFuncType
    {
        InitPlayerList = 60,
        AssignRoleToPlayer,
        CustomMurderPlayer,
        CalcSanityPoint,
        ReSetTasks,
        ShowCenterMessage,

        UseRepairButtonFixLights = 91,
        LightPrayerBless,
        Morph,
        CurseKill
    }

    public static class RPCFunctions
    {
        public static void InitPlayerList()
        {
            Players.InitPlayerList();
        }

        public static void AssignRoleToPlayer(byte playerId, RoleType roleType)
        {
            foreach(var player in Players.playerList)
            {
                if(player.PlayerControl.PlayerId == playerId)
                {
                    var role = RoleFactory.Produce(roleType, player);

                    player.MainRole = role;
                    player.MaxSanityPoint = role.GetMaxSanityPoint();
                    player.SanityPoint = role.GetInitialSanityPoint() < player.SanityPoint ? role.GetInitialSanityPoint() : player.SanityPoint;
                    break;
                }
            }
        }

        public static void CustomMurderPlayer(byte sourceId, byte targetId)
        {
            PlayerControl source = ToolBox.GetPlayerControlById(sourceId);
            PlayerControl target = ToolBox.GetPlayerControlById(targetId);
            if (source != null && target != null)
                source.MurderPlayer(target);
        }

        public static void CalcSanityPoint(byte playerId, int value)
        {
            Player player = Players.GetPlayer(playerId);
            player.CalcSanityPoint(value);
        }

        public static void ReSetTasks(byte playerId, byte[] taskTypeIds)
        {
            ToolBox.GetPlayerControlById(playerId).clearAllTasks();
            GameData.Instance.SetTasks(playerId, taskTypeIds);
        }

        public static TMP_Text centerMessage;
        public static void ShowCenterMessage(string message)
        {
            HudManager instance = DestroyableSingleton<HudManager>.Instance;
            var roomTracker = instance?.roomTracker;
            if (roomTracker is null) return;

            var gameObject = UnityEngine.Object.Instantiate<GameObject>(roomTracker.gameObject);
            gameObject.transform.SetParent(DestroyableSingleton<HudManager>.Instance.transform);
            UnityEngine.Object.DestroyImmediate(gameObject.GetComponent<RoomTracker>());
            gameObject.transform.localPosition = new Vector3(0f, -1.8f, gameObject.transform.localPosition.z);
            gameObject.transform.localScale *= 1.5f;

            centerMessage = gameObject.GetComponent<TMP_Text>();
            centerMessage.text = message;

            Action<float> action = (float p) =>
            {
                if (p == 0f)
                {
                    UnityEngine.Object.Destroy(centerMessage.gameObject);
                }
            };
            DestroyableSingleton<HudManager>.Instance.StartCoroutine(Effects.Lerp(3, action));
        }

        public static void UseRepairButtonFixLights()
        {
            SwitchSystem switchSystem = ShipStatus.Instance.Systems[SystemTypes.Electrical].Cast<SwitchSystem>();
            switchSystem.ActualSwitches = switchSystem.ExpectedSwitches;
        }

        public static void LightPrayerBless(byte sourceId, byte targetId)
        {
            Player source = Players.GetPlayer(sourceId);
            if (source.MainRole is not LightPrayer) return;

            Player target = Players.GetPlayer(targetId);
            source.CalcSanityPoint(-5);
            target.CalcSanityPoint(20);
        }

        public static void Morph(byte sourceId, byte targetId)
        {
            PlayerControl source = ToolBox.GetPlayerControlById(sourceId);
            PlayerControl target = ToolBox.GetPlayerControlById(targetId);

            if (source == null || target == null) return;

            source.setLook(target.Data.PlayerName, target.Data.DefaultOutfit.ColorId, target.Data.DefaultOutfit.HatId,
                target.Data.DefaultOutfit.VisorId, target.Data.DefaultOutfit.SkinId, target.Data.DefaultOutfit.PetId);
        }

        public static void CurseKill(byte sourceId, byte targetId)
        {
            if(MeetingHud.Instance is null) return;

            Player source = Players.GetPlayer(sourceId);
            Player target = sourceId == targetId ? source : Players.GetPlayer(targetId);

            target.PlayerControl.Exiled();
            if (Constants.ShouldPlaySfx()) 
                SoundManager.Instance.PlaySound(target.PlayerControl.KillSfx, false, 0.8f);
            
            foreach (var voteArea in MeetingHud.Instance.playerStates)
            {
                if(voteArea.TargetPlayerId == targetId)
                {
                    voteArea.SetDead(voteArea.DidReport, true);
                    voteArea.Overlay.gameObject.SetActive(true);
                }

                if (voteArea.VotedFor == targetId) 
                    voteArea.UnsetVote();
            }

            if (AmongUsClient.Instance.AmHost)
                MeetingHud.Instance.CheckForEndVoting();
            
            if (target.PlayerControl == PlayerControl.LocalPlayer)
                HudManager.Instance.KillOverlay.ShowKillAnimation(source.PlayerControl.Data, target.PlayerControl.Data);
        }
    }

}
