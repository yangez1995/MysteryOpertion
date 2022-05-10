using MysteryOpertion.Model;
using MysteryOpertion.Model.Roles;
using HarmonyLib;
using Hazel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MysteryOpertion.Patches
{
    [HarmonyPatch(typeof(RoleManager), nameof(RoleManager.SelectRoles))]
    public static class RoleManagerSelectRolesPatch
    {
        

        public static void Postfix()
        {
            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)RPCFuncType.InitPlayerList, SendOption.Reliable);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            RPCFunctions.InitPlayerList();

            var waitAssignCrewmateList = new List<Player>();
            var waitAssignImpostorList = new List<Player>();
            foreach (var player in Players.playerList)
            {
                if (player.playerControl.Data.Role.IsImpostor)
                    waitAssignImpostorList.Add(player);
                else
                    waitAssignCrewmateList.Add(player);
            }

            RolePool rolePool = loadRolePool();
            foreach (var roleType in rolePool.crewmatePriorityPool)
            {
                var index = ToolBox.random.Next(0, waitAssignCrewmateList.Count);
                var player = waitAssignCrewmateList[index];
                waitAssignCrewmateList.RemoveAt(index);

                AssignRole(player.playerControl.PlayerId, roleType);
            }

            foreach (var roleType in rolePool.impostorPriorityPool)
            {
                var index = ToolBox.random.Next(0, waitAssignImpostorList.Count);
                var player = waitAssignImpostorList[index];
                waitAssignImpostorList.RemoveAt(index);

                AssignRole(player.playerControl.PlayerId, roleType);
            }

            //设置未分配船员为白板
            foreach (var player in waitAssignCrewmateList)
            {
                AssignRole(player.playerControl.PlayerId, RoleType.Crewmate);
            }

            //设置未分配内鬼为白板
            foreach (var player in waitAssignImpostorList)
            {
                AssignRole(player.playerControl.PlayerId, RoleType.Impostor);
            }
        }

        private static void AssignRole(byte playerId, RoleType roleType)
        {
            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)RPCFuncType.AssignRoleToPlayer, SendOption.Reliable);
            writer.Write(playerId);
            writer.Write((byte)roleType);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            RPCFunctions.AssignRoleToPlayer(playerId, roleType);
        }

        private static RolePool loadRolePool()
        {
            RolePool pool = new RolePool();
            pool.crewmatePriorityPool.Add(RoleType.Traveller);
            pool.crewmatePriorityPool.Add(RoleType.DoomBait);
            pool.crewmatePriorityPool.Add(RoleType.DoomBait);

            pool.impostorPriorityPool.Add(RoleType.NoneFaceMan);
            return pool;
        }

        private class RolePool
        {
            public List<RoleType> crewmatePriorityPool;
            public List<RoleType> crewmateProbabilityPool;
            public List<RoleType> impostorPriorityPool;
            public List<RoleType> impostorProbabilityPool;
            public List<RoleType> chaosPriorityPool;
            public List<RoleType> chaosProbabilityPool;

            public RolePool()
            {
                crewmatePriorityPool = new List<RoleType>();
                crewmateProbabilityPool = new List<RoleType>();
                impostorPriorityPool = new List<RoleType>();
                impostorProbabilityPool = new List<RoleType>();
                chaosPriorityPool = new List<RoleType>();
                chaosProbabilityPool = new List<RoleType>();
            }
        }
    }
}
