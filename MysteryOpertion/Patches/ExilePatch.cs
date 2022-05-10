using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

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
                WrapUpPostfix();
            }
        }

        [HarmonyPatch(typeof(AirshipExileController), nameof(AirshipExileController.WrapUpAndSpawn))]
        class AirshipExileControllerWrapUpPatch
        {
            public static void Postfix(ExileController __instance)
            {
                WrapUpPostfix();
            }
        }

        private static void WrapUpPostfix()
        {
            foreach (var player in Players.playerList)
            {
                player.UpdateButtonsOnMeetingEnd();
            }
        }
    }
}
