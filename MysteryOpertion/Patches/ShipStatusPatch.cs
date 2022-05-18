using MysteryOpertion.Model;
using MysteryOpertion.Model.Roles.CrewmateRoles;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MysteryOpertion.Patches
{
    [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.CalculateLightRadius))]
    class ShipStatusCalculateLightRadiusPatch
    {
        public static bool Prefix(ref float __result, ShipStatus __instance, [HarmonyArgument(0)] GameData.PlayerInfo playerInfo)
        {
            ISystemType systemType = __instance.Systems.ContainsKey(SystemTypes.Electrical) ? __instance.Systems[SystemTypes.Electrical] : null;
            if (systemType == null) return true;
            SwitchSystem switchSystem = systemType.TryCast<SwitchSystem>();
            if (switchSystem == null) return true;

            float num = (float)switchSystem.Value / 255f;

            var player = Players.GetPlayer(playerInfo.PlayerId);
            if(playerInfo == null || playerInfo.IsDead)
            {
                __result = __instance.MaxLightRadius;
            }
            else if (playerInfo.Role.IsImpostor)
            {
                __result = __instance.MaxLightRadius;
            }
            else if (player.MainRole is LightPrayer)
            {
                __result = __instance.MaxLightRadius;
            }
            else
            {
                __result = Mathf.Lerp(__instance.MinLightRadius, __instance.MaxLightRadius, num);
            }

            return false;
        }
    }

    [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.CheckEndCriteria))]
    class CheckEndCriteriaPatch
    {
        public static void Prefix(ShipStatus __instance)
        {
            if (!GameData.Instance) return;

            if(GameNote.ArsonExpertWin)
            {
                __instance.enabled = false;
                ShipStatus.RpcEndGame(0, false);
            }

            //CheckAndEndGameForTaskWin(__instance);
        }

        private static bool CheckAndEndGameForTaskWin(ShipStatus __instance)
        {
            bool allTasksCompleted = true;
            foreach (var player in Players.playerList)
            {
                if (player.MainRole is Crewmate && !player.PlayerControl.AllTasksCompleted())
                {
                    allTasksCompleted = false;
                    break;
                }
            }

            if (allTasksCompleted)
            {
                __instance.enabled = false;
                ShipStatus.RpcEndGame(GameOverReason.HumansByTask, false);
                return true;
            }
            return false;
        }
    }
}
