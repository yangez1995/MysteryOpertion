﻿using AgeOfMagic.Entities;
using AgeOfMagic.Entities.Roles;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AgeOfMagic.Patches
{
    [HarmonyPatch(typeof(RoleManager), nameof(RoleManager.SelectRoles))]
    public static class RoleManagerSelectRolesPatch
    {
        public static Random random = new Random((int)DateTime.Now.Ticks);

        public static void Postfix()
        {
            Players.initPlayerList(PlayerControl.AllPlayerControls.ToArray().ToList());

            var waitAssignCrewmateList = new List<Player>();
            var waitAssignImpostorList = new List<Player>();
            foreach(var player in Players.playerList)
            {
                if (player.playerControl.Data.Role.IsImpostor)
                    waitAssignImpostorList.Add(player);
                else
                    waitAssignCrewmateList.Add(player);
            }

            RolePool rolePool = loadRolePool();
            foreach(var roleType in rolePool.crewmateEnsuredPool)
            {
                var index = random.Next(0, waitAssignCrewmateList.Count);
                var player = waitAssignCrewmateList[index];
                waitAssignCrewmateList.RemoveAt(index);

                player.mainRole = RoleFactory.Produce(roleType);
            }

            //设置未分配船员为白板
            foreach (var player in waitAssignCrewmateList)
            {
                player.mainRole = RoleFactory.Produce(RoleType.Crewmate);
            }

            //设置未分配内鬼为白板
            foreach (var player in waitAssignImpostorList)
            {
                player.mainRole = RoleFactory.Produce(RoleType.Impostor);
            }
        }

        private static RolePool loadRolePool()
        {
            RolePool pool = new RolePool();
            pool.crewmateEnsuredPool.Add(RoleType.Sheriff);
            pool.crewmateEnsuredPool.Add(RoleType.Sheriff);

            return pool;
        }

        private class RolePool
        {
            public List<RoleType> crewmateEnsuredPool;
            public List<RoleType> crewmateChancePool;
            public List<RoleType> impostorEnsuredPool;
            public List<RoleType> impostorChancePool;

            public RolePool()
            {
                crewmateEnsuredPool = new List<RoleType>();
                crewmateChancePool = new List<RoleType>();
                impostorEnsuredPool = new List<RoleType>();
                impostorChancePool = new List<RoleType>();            
            }
        }
    }
}
