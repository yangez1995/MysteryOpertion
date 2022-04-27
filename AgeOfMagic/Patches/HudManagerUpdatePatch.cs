using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AgeOfMagic.Patches
{
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public static class HudManagerUpdatePatch
    {
        static void Postfix(HudManager __instance)
        {
            if (AmongUsClient.Instance.GameState != InnerNet.InnerNetClient.GameStates.Started) return;

            foreach(var button in ButtonRespository.buttonDict)
            {
                button.Value.OnHudManagerUpdate();
            }
        }
    }
}
