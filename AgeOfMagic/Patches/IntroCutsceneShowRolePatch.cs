using HarmonyLib;
using Il2CppSystem.Collections;
using System;
using System.Collections.Generic;
using System.Text;

namespace AgeOfMagic.Patches
{
    [HarmonyPatch]
    class IntroPatch
    {
        [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.SetUpRoleText))]
        class IntroCutsceneShowRolePatch
        {
            public static void Postfix(IntroCutscene __instance)
            {
                var role = Players.localPlayer.mainRole;

                HudManager.Instance.StartCoroutine(Effects.Lerp(1f, new Action<float>((p) => {
                    __instance.RoleText.text = role.GetRoleName();
                    __instance.RoleBlurbText.text = role.GetRoleBlurb();

                    __instance.RoleText.color = role.GetRoleColor();
                    __instance.RoleBlurbText.color = role.GetRoleColor();
                    __instance.YouAreText.color = role.GetRoleColor();
                })));
            }
        }
    }
}
