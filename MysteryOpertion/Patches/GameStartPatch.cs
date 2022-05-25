using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace MysteryOpertion.Patches
{
    [HarmonyPatch(typeof(GameStartManager), nameof(GameStartManager.BeginGame))]
    class GameStartManagerBeginGame
    {
        public static void Prefix(GameStartManager __instance)
        {

        }
    }
}
