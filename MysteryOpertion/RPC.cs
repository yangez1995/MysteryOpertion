using MysteryOpertion.Model;
using MysteryOpertion.Model.Roles;
using MysteryOpertion.Model.Roles.CrewmateRoles;
using System;
using System.Collections.Generic;
using System.Text;
using UnhollowerBaseLib;

namespace MysteryOpertion
{
    public enum RPCFuncType
    {
        InitPlayerList = 60,
        AssignRoleToPlayer,
        CustomMurderPlayer,
        CalcSanityPoint,
        ReSetTasks,

        UseRepairButtonFixLights = 91,
        LightPrayerBless,
        Morph
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
                if(player.playerControl.PlayerId == playerId)
                {
                    var role = RoleFactory.Produce(roleType, player);

                    player.mainRole = role;
                    player.maxSanityPoint = role.GetMaxSanityPoint();
                    player.sanityPoint = role.GetInitialSanityPoint() < player.sanityPoint ? role.GetInitialSanityPoint() : player.sanityPoint;
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

        public static void UseRepairButtonFixLights()
        {
            SwitchSystem switchSystem = ShipStatus.Instance.Systems[SystemTypes.Electrical].Cast<SwitchSystem>();
            switchSystem.ActualSwitches = switchSystem.ExpectedSwitches;
        }

        public static void LightPrayerBless(byte sourceId, byte targetId)
        {
            Player source = Players.GetPlayer(sourceId);
            if (source.mainRole is not LightPrayer) return;

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

        
    }
}
