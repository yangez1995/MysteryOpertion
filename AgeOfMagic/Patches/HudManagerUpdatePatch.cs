using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AgeOfMagic.Patches
{
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    class HudManagerUpdatePatch
    {
        static void Postfix(HudManager __instance)
        {
            if (AmongUsClient.Instance.GameState != InnerNet.InnerNetClient.GameStates.Started) return;

            //foreach (PlayerControl player in PlayerControl.AllPlayerControls)
            //{
            //    if (player != PlayerControl.LocalPlayer)
            //        continue;

            //    if (!player.Data.Role.IsImpostor)
            //    {
            //        player.nameText.text = "监听者 \n yang";
            //        player.nameText.color = Color.blue;
            //    }
            //}
        }
    }
}
