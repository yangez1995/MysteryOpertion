using MysteryOpertion.Model.Roles;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace MysteryOpertion.Patches
{
    public class GameOverPatch
    {
        [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.OnGameEnd))]
        public class OnGameEndPatch
        {
            public static void Prefix(AmongUsClient __instance, [HarmonyArgument(0)] ref EndGameResult endGameResult)
            {
                if (endGameResult.GameOverReason == 0) endGameResult.GameOverReason = GameOverReason.ImpostorByKill;
            }

            public static void Postfix(AmongUsClient __instance, [HarmonyArgument(0)] ref EndGameResult endGameResult)
            {
                // Arsonist win
                if (GameNote.ArsonExpertWin)
                {
                    TempData.winners = new Il2CppSystem.Collections.Generic.List<WinningPlayerData>();
                    ResetWinnerListByRole(RoleNameDictionary.ArsonExpert);
                }
            }

            private static void ResetWinnerListByRole(string roleName)
            {
                TempData.winners = new Il2CppSystem.Collections.Generic.List<WinningPlayerData>();
                foreach(var player in Players.playerList)
                {
                    if(player.mainRole.GetRoleName() == roleName)
                    {
                        WinningPlayerData wpd = new WinningPlayerData(player.playerControl.Data);
                        TempData.winners.Add(wpd);
                    }
                }
            }
        }
    }
}
