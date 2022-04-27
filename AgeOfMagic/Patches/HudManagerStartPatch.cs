using AgeOfMagic.Entities;
using AgeOfMagic.Entities.Buttons;
using AgeOfMagic.Entities.Roles.CrewmateRoles;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AgeOfMagic.Patches
{
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Start))]
    public static class HudManagerStartPatch
    {
        public static void Postfix(HudManager __instance)
        {
            initButtons(__instance);
        }

        private static void initButtons(HudManager __instance)
        {
            ButtonRespository.buttonDict.Clear();
            ButtonRespository.buttonDict.Add(ButtonType.SheriffKill, ButtonFactory.Produce(ButtonType.SheriffKill, __instance));
        }
    }
}
