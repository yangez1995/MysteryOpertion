using HarmonyLib;
using MysteryOpertion.Model.Roles.ChaosRoles;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static GameData;

namespace MysteryOpertion.Patches
{
    [HarmonyPatch]
    class ExilePatch
    {
        [HarmonyPatch(typeof(ExileController), nameof(ExileController.WrapUp))]
        class ExileControllerWrapUpPatch
        {
            public static void Postfix(ExileController __instance)
            {
                WrapUpPostfix(__instance.exiled);
            }
        }

        [HarmonyPatch(typeof(AirshipExileController), nameof(AirshipExileController.WrapUpAndSpawn))]
        class AirshipExileControllerWrapUpPatch
        {
            public static void Postfix(ExileController __instance)
            {
                WrapUpPostfix(__instance.exiled);
            }
        }

        private static void WrapUpPostfix(PlayerInfo exiled)
        {
            foreach (var player in Players.playerList)
            {
                if (exiled is not null && player.PlayerControl.PlayerId == exiled.PlayerId && player.MainRole is Jester)
                {
                    Debug.Log("JesterWinner");
                    GameNote.JesterWinner = player;
                    break;
                }

                player.UpdateButtonsOnMeetingEnd();
            }
        }
    }
}
