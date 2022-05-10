using MysteryOpertion.Model;
using MysteryOpertion.Model.Roles.ChaosRoles;
using HarmonyLib;
using Il2CppSystem.Collections;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MysteryOpertion.Patches
{
    [HarmonyPatch]
    class IntroPatch
    {
        [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.SetUpRoleText))]
        class IntroCutsceneShowRolePatch
        {
            public static void Postfix(IntroCutscene __instance)
            {
                var role = Players.GetLocalPlayer().mainRole;

                HudManager.Instance.StartCoroutine(Effects.Lerp(1f, new Action<float>((p) => {
                    __instance.RoleText.text = role.GetRoleName();
                    __instance.RoleBlurbText.text = role.GetRoleBlurb();

                    __instance.RoleText.color = role.GetRoleColor();
                    __instance.RoleBlurbText.color = role.GetRoleColor();
                    __instance.YouAreText.color = role.GetRoleColor();
                })));
            }
        }

        [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.OnDestroy))]
        class IntroCutsceneOnDestroyPatch
        {
            public static void Prefix(IntroCutscene __instance)
            {
                initPlayerIcons(__instance);

                foreach(var player in Players.playerList)
                {
                    if(player.mainRole is Chaos)
                    {
                        ToolBox.ReSetPlayerTask(player.playerControl.PlayerId, 1, 1, 1);
                    }
                }
            }

            private static void initPlayerIcons(IntroCutscene __instance)
            {
                PlayerIcons.iconDict.Clear();

                Player localPlayer = Players.GetLocalPlayer();
                if (localPlayer.mainRole is not ArsonExpert) return;

                int index = 0;
                if (PlayerControl.LocalPlayer != null && HudManager.Instance != null)
                {
                    Vector3 bottomLeft = new Vector3(-HudManager.Instance.UseButton.transform.localPosition.x, HudManager.Instance.UseButton.transform.localPosition.y, HudManager.Instance.UseButton.transform.localPosition.z);
                    foreach (Player player in Players.playerList)
                    {
                        PlayerControl playerControl = player.playerControl;
                        GameData.PlayerInfo data = playerControl.Data;

                        PoolablePlayer poolablePlayer = UnityEngine.Object.Instantiate<PoolablePlayer>(__instance.PlayerPrefab, HudManager.Instance.transform);
                        PlayerControl.SetPlayerMaterialColors(data.DefaultOutfit.ColorId, poolablePlayer.Body);
                        DestroyableSingleton<HatManager>.Instance.SetSkin(poolablePlayer.Skin.layer, data.DefaultOutfit.SkinId);
                        poolablePlayer.HatSlot.SetHat(data.DefaultOutfit.HatId, data.DefaultOutfit.ColorId);
                        PlayerControl.SetPetImage(data.DefaultOutfit.PetId, data.DefaultOutfit.ColorId, poolablePlayer.PetSlot);
                        poolablePlayer.NameText.text = data.PlayerName;
                        poolablePlayer.SetFlipX(true);
                        PlayerIcons.iconDict[playerControl.PlayerId] = poolablePlayer;

                        if (player.playerControl != PlayerControl.LocalPlayer)
                        {
                            poolablePlayer.transform.localPosition = bottomLeft + new Vector3(-0.25f, -0.25f, 0) + Vector3.right * index++ * 0.35f;
                            poolablePlayer.transform.localScale = Vector3.one * 0.2f;
                            poolablePlayer.setSemiTransparent(true);
                            poolablePlayer.gameObject.SetActive(true);
                        }
                        else
                        {
                            poolablePlayer.gameObject.SetActive(false);
                        }
                    }
                }
            }
        }
    }
}
