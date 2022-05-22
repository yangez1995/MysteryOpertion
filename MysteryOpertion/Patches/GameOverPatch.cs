using MysteryOpertion.Model.Roles;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using MysteryOpertion.Model;

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
                if (GameNote.ArsonExpertWinner is not null)
                    ResetWinnerListByRole(GameNote.ArsonExpertWinner);
                if (GameNote.JesterWinner is not null)
                    ResetWinnerListByRole(GameNote.JesterWinner);
                if (GameNote.SpectatorWinner is not null)
                    ResetWinnerListByRole(GameNote.SpectatorWinner);
            }

            private static void ResetWinnerListByRole(Player winner)
            {
                TempData.winners = new Il2CppSystem.Collections.Generic.List<WinningPlayerData>();
                WinningPlayerData wpd = new WinningPlayerData(winner.PlayerControl.Data);
                TempData.winners.Add(wpd);
            }
        }
    }
}
