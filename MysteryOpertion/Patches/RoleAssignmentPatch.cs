using MysteryOpertion.Model;
using MysteryOpertion.Model.Roles;
using HarmonyLib;
using Hazel;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
                if (player.PlayerControl == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.Role.IsImpostor)
                {
                    AssignRole(player.PlayerControl.PlayerId, RoleType.Traveller);
                    continue;
                }

                if (player.PlayerControl.Data.Role.IsImpostor)
                    waitAssignImpostorList.Add(player);
                else
                    waitAssignCrewmateList.Add(player);
            }

            RolePool pool = LoadRolePool();
            
            pool.LurkerCount = pool.LurkerCount <= waitAssignCrewmateList.Count ? pool.LurkerCount : 0;
            //计算实际船员阵营人数
            int trueCrewmateCount = waitAssignCrewmateList.Count > (pool.CrewmateCount + pool.LurkerCount)
                ? pool.CrewmateCount : waitAssignCrewmateList.Count - pool.LurkerCount;

            Debug.Log(trueCrewmateCount + "");
            //分配优先分配船员职业
            int crewmatePriorityCount = trueCrewmateCount > pool.CrewmatePriorityPool.Count 
                ? pool.CrewmatePriorityPool.Count : trueCrewmateCount;
            for (int i = 0; i < crewmatePriorityCount; i++)
            {
                int playerIndex = ToolBox.random.Next(0, waitAssignCrewmateList.Count);
                int roleIndex = ToolBox.random.Next(0, pool.CrewmatePriorityPool.Count);
                AssignRole(waitAssignCrewmateList[playerIndex].PlayerControl.PlayerId, pool.CrewmatePriorityPool[roleIndex]);

                waitAssignCrewmateList.RemoveAt(playerIndex);
                pool.CrewmatePriorityPool.RemoveAt(roleIndex);
            }

            //分配概率分配船员职业
            int crewmateProbabilityCount = trueCrewmateCount - crewmatePriorityCount;
            for (int i = 0; i < crewmateProbabilityCount; i++)
            {
                if (pool.CrewmateProbabilityPool.Count == 0) break;

                int playerIndex = ToolBox.random.Next(0, waitAssignCrewmateList.Count);
                int roleIndex = ToolBox.random.Next(0, pool.CrewmateProbabilityPool.Count);
                var roleType = pool.CrewmateProbabilityPool[roleIndex];
                AssignRole(waitAssignCrewmateList[playerIndex].PlayerControl.PlayerId, roleType);

                waitAssignCrewmateList.RemoveAt(playerIndex);
                pool.CrewmateProbabilityPool.RemoveAll(it => it == roleType);
            }

            //计算实际混沌阵营人数
            int trueChaosCount = waitAssignCrewmateList.Count > (pool.ChaosCount + pool.LurkerCount)
                ? pool.ChaosCount : waitAssignCrewmateList.Count - pool.LurkerCount;

            //分配优先分配混沌职业
            int chaosPriorityCount = trueChaosCount > pool.ChaosPriorityPool.Count
                ? pool.ChaosPriorityPool.Count : trueChaosCount;
            for (int i = 0; i < chaosPriorityCount; i++)
            {
                int playerIndex = ToolBox.random.Next(0, waitAssignCrewmateList.Count);
                int roleIndex = ToolBox.random.Next(0, pool.ChaosPriorityPool.Count);
                AssignRole(waitAssignCrewmateList[playerIndex].PlayerControl.PlayerId, pool.ChaosPriorityPool[roleIndex]);

                waitAssignCrewmateList.RemoveAt(playerIndex);
                pool.ChaosPriorityPool.RemoveAt(roleIndex);
            }

            //分配概率分配混沌职业
            int chaosProbabilityCount = trueChaosCount - chaosPriorityCount;
            for (int i = 0; i < chaosProbabilityCount; i++)
            {
                if (pool.ChaosProbabilityPool.Count == 0) break;

                int playerIndex = ToolBox.random.Next(0, waitAssignCrewmateList.Count);
                int roleIndex = ToolBox.random.Next(0, pool.ChaosProbabilityPool.Count);
                var roleType = pool.ChaosProbabilityPool[roleIndex];
                AssignRole(waitAssignCrewmateList[playerIndex].PlayerControl.PlayerId, roleType);

                waitAssignCrewmateList.RemoveAt(playerIndex);
                pool.ChaosProbabilityPool.RemoveAll(it => it == roleType);
            }

            //分配潜伏者
            for (int i = 0; i < pool.LurkerCount; i++)
            {
                int playerIndex = ToolBox.random.Next(0, waitAssignCrewmateList.Count);
                AssignRole(waitAssignCrewmateList[playerIndex].PlayerControl.PlayerId, RoleType.Lurker);
                waitAssignCrewmateList.RemoveAt(playerIndex);
            }

            //设置未分配船员为白板
            foreach (var player in waitAssignCrewmateList)
            {
                AssignRole(player.PlayerControl.PlayerId, RoleType.Crewmate);
            }

            //分配优先分配伪装者职业
            int impostorPriorityCount = pool.ImpostorCount > pool.ChaosPriorityPool.Count
                ? pool.ChaosPriorityPool.Count : pool.ImpostorCount;
            for (int i = 0; i < impostorPriorityCount; i++)
            {
                int playerIndex = ToolBox.random.Next(0, waitAssignImpostorList.Count);
                int roleIndex = ToolBox.random.Next(0, pool.ImpostorPriorityPool.Count);
                AssignRole(waitAssignImpostorList[playerIndex].PlayerControl.PlayerId, pool.ImpostorPriorityPool[roleIndex]);

                waitAssignImpostorList.RemoveAt(playerIndex);
                pool.ImpostorPriorityPool.RemoveAt(roleIndex);
            }

            //分配概率分配混沌职业
            int impostorProbabilityCount = pool.ImpostorCount - impostorPriorityCount;
            for (int i = 0; i < impostorProbabilityCount; i++)
            {
                if (pool.ImpostorProbabilityPool.Count == 0) break;

                int playerIndex = ToolBox.random.Next(0, waitAssignImpostorList.Count);
                int roleIndex = ToolBox.random.Next(0, pool.ImpostorProbabilityPool.Count);
                var roleType = pool.ImpostorProbabilityPool[roleIndex];
                AssignRole(waitAssignImpostorList[playerIndex].PlayerControl.PlayerId, roleType);

                waitAssignImpostorList.RemoveAt(playerIndex);
                pool.ImpostorProbabilityPool.RemoveAll(it => it == roleType);
            }

            //设置未分配伪装者为白板
            foreach (var player in waitAssignImpostorList)
            {
                AssignRole(player.PlayerControl.PlayerId, RoleType.Impostor);
            }

            //foreach (var roleType in rolePool.CrewmatePriorityPool)
            //{
            //    var index = ToolBox.random.Next(0, waitAssignCrewmateList.Count);
            //    var player = waitAssignCrewmateList[index];
            //    waitAssignCrewmateList.RemoveAt(index);

            //    AssignRole(player.PlayerControl.PlayerId, roleType);
            //}

            //foreach (var roleType in rolePool.ImpostorPriorityPool)
            //{
            //    var index = ToolBox.random.Next(0, waitAssignImpostorList.Count);
            //    var player = waitAssignImpostorList[index];
            //    waitAssignImpostorList.RemoveAt(index);

            //    AssignRole(player.PlayerControl.PlayerId, roleType);
            //}
        }

        private static void AssignRole(byte playerId, RoleType roleType)
        {
            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)RPCFuncType.AssignRoleToPlayer, SendOption.Reliable);
            writer.Write(playerId);
            writer.Write((byte)roleType);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            RPCFunctions.AssignRoleToPlayer(playerId, roleType);
        }

        private static RolePool TestRolePool()
        {
            RolePool pool = new RolePool();

            

            //pool.crewmatePriorityPool.Add(RoleType.Spectator);
            //pool.crewmatePriorityPool.Add(RoleType.Spectator);
            //pool.crewmatePriorityPool.Add(RoleType.Spectator);
            ////pool.crewmatePriorityPool.Add(RoleType.LightPrayer);
            ////pool.crewmatePriorityPool.Add(RoleType.Judge);
            ////pool.crewmatePriorityPool.Add(RoleType.DoomBait);
            ////pool.crewmatePriorityPool.Add(RoleType.Coroner);
            ////pool.crewmatePriorityPool.Add(RoleType.Diviner);
            ////pool.crewmatePriorityPool.Add(RoleType.Eavesdropper);
            ////pool.crewmatePriorityPool.Add(RoleType.CurseMage);
            ////pool.crewmatePriorityPool.Add(RoleType.ArsonExpert);
            ////pool.crewmatePriorityPool.Add(RoleType.Jester);
            ////pool.crewmatePriorityPool.Add(RoleType.Spectator);
            
            ////pool.impostorPriorityPool.Add(RoleType.NoneFaceMan);
            //pool.impostorPriorityPool.Add(RoleType.CurseWarlock);
            return pool;
        }

        private static RolePool LoadRolePool()
        {
            RolePool pool = new RolePool();

            pool.CrewmateCount = ConfigLoader.selecters[ConfigKeyDictionary.CrewmateCount].Index + 2;
            pool.ImpostorCount = ConfigLoader.selecters[ConfigKeyDictionary.ImpostorCount].Index + 1;
            pool.ChaosCount = ConfigLoader.selecters[ConfigKeyDictionary.ChaosCount].Index;
            pool.LurkerCount = ConfigLoader.selecters[ConfigKeyDictionary.LurkerCount].Index;

            pool.LoadRole(ConfigKeyDictionary.CoronerPriority, RoleType.Coroner, 0);
            pool.LoadRole(ConfigKeyDictionary.CurseMagePriority, RoleType.CurseMage, 0);
            pool.LoadRole(ConfigKeyDictionary.DivinerPriority, RoleType.Diviner, 0);
            pool.LoadRole(ConfigKeyDictionary.DoomBaitPriority, RoleType.DoomBait, 0);
            pool.LoadRole(ConfigKeyDictionary.EavesdropperPriority, RoleType.Eavesdropper, 0);
            pool.LoadRole(ConfigKeyDictionary.JudgePriority, RoleType.Judge, 0);
            pool.LoadRole(ConfigKeyDictionary.LightPrayerPriority, RoleType.LightPrayer, 0);
            pool.LoadRole(ConfigKeyDictionary.MechanicExpertPriority, RoleType.MechanicExpert, 0);
            pool.LoadRole(ConfigKeyDictionary.SheriffPriority, RoleType.Sheriff, 0);
            pool.LoadRole(ConfigKeyDictionary.TravellerPriority, RoleType.Traveller, 0);
            
            pool.LoadRole(ConfigKeyDictionary.BloodyHunterPriority, RoleType.BloodyHunter, 1);
            pool.LoadRole(ConfigKeyDictionary.CurseWarlockPriority, RoleType.CurseWarlock, 1);
            pool.LoadRole(ConfigKeyDictionary.NoneFaceManPriority, RoleType.NoneFaceMan, 1);
            pool.LoadRole(ConfigKeyDictionary.SerialKillerPriority, RoleType.SerialKiller, 1);

            pool.LoadRole(ConfigKeyDictionary.ArsonExpertPriority, RoleType.ArsonExpert, 2);
            pool.LoadRole(ConfigKeyDictionary.JesterPriority, RoleType.Jester, 2);
            pool.LoadRole(ConfigKeyDictionary.SpectatorPriority, RoleType.Spectator, 2);
            return pool;
        }

        private class RolePool
        {
            public int CrewmateCount { get; set; }
            public int ImpostorCount { get; set; }
            public int ChaosCount { get; set; }
            public int LurkerCount { get; set; }

            public List<RoleType> CrewmatePriorityPool { get; set; }
            public List<RoleType> CrewmateProbabilityPool { get; set; }
            public List<RoleType> ImpostorPriorityPool { get; set; }
            public List<RoleType> ImpostorProbabilityPool { get; set; }
            public List<RoleType> ChaosPriorityPool { get; set; }
            public List<RoleType> ChaosProbabilityPool { get; set; }

            public RolePool()
            {
                CrewmatePriorityPool = new List<RoleType>();
                CrewmateProbabilityPool = new List<RoleType>();
                ImpostorPriorityPool = new List<RoleType>();
                ImpostorProbabilityPool = new List<RoleType>();
                ChaosPriorityPool = new List<RoleType>();
                ChaosProbabilityPool = new List<RoleType>();
            }

            public void LoadRole(string key, RoleType roleType, int camp)
            {
                int index = ConfigLoader.selecters[key].Index;
                switch (index)
                {
                    case 0:
                        break;
                    case 1:
                    case 2:
                    case 3:
                        for(int i = 0; i < index; i++)
                        {
                            if(camp == 0) CrewmateProbabilityPool.Add(roleType);
                            else if(camp == 1) ImpostorProbabilityPool.Add(roleType);
                            else ChaosProbabilityPool.Add(roleType);
                        }
                        break;
                    case 4:
                        if (camp == 0) CrewmatePriorityPool.Add(roleType);
                        else if (camp == 1) ImpostorPriorityPool.Add(roleType);
                        else ChaosPriorityPool.Add(roleType);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
