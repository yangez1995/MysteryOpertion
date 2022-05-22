using HarmonyLib;
using MysteryOpertion.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;

namespace MysteryOpertion.Patches
{
    [HarmonyPatch(typeof(GameOptionsMenu), nameof(GameOptionsMenu.Start))]
    class GameOptionsMenuStartPatch
    {
        private static string ModSettingsName = "ModSettings";

        public static void Postfix(GameOptionsMenu __instance)
        {
            //if (GameObject.Find(ModSettingsName) != null)
            //{ 
            //    GameObject.Find(ModSettingsName).transform.FindChild("GameGroup").FindChild("Text").GetComponent<TMPro.TextMeshPro>().SetText("The Other Roles Settings");
            //    return;
            //}
            //
            //if (template == null) return;
            var optionOriginal = UnityEngine.Object.FindObjectsOfType<StringOption>().FirstOrDefault();

            var gameSettingsOriginal = GameObject.Find("Game Settings");
            var gameSettingMenu = UnityEngine.Object.FindObjectsOfType<GameSettingMenu>().FirstOrDefault();

            var modRoleSettings = UnityEngine.Object.Instantiate(gameSettingsOriginal, gameSettingsOriginal.transform.parent);
            var modRoleTitle = modRoleSettings.transform.FindChild("GameGroup").FindChild("Text").GetComponent<TextMeshPro>();
            modRoleTitle.SetText("诡秘行动职业设置");
            var modeRoleMenu = modRoleSettings.transform.FindChild("GameGroup").FindChild("SliderInner").GetComponent<GameOptionsMenu>();

            //var modSettings = UnityEngine.Object.Instantiate(gameSettings, gameSettings.transform.parent);
            //var modSettingsMenu = modSettings.transform.FindChild("GameGroup").FindChild("SliderInner").GetComponent<GameOptionsMenu>();
            //modSettings.name = ModSettingsName;

            var roleTab = GameObject.Find("RoleTab");
            var gameTab = GameObject.Find("GameTab");
            var modRoleTab = UnityEngine.Object.Instantiate(roleTab, roleTab.transform.parent);

            roleTab.transform.position += Vector3.left * 0.5f;
            gameTab.transform.position += Vector3.left * 0.5f;
            modRoleTab.transform.position += Vector3.right * 0.5f;

            var tabs = new GameObject[] { gameTab, roleTab, modRoleTab };
            for (int i = 0; i < tabs.Length; i++)
            {
                var button = tabs[i].GetComponentInChildren<PassiveButton>();
                if (button is null) continue;
                int copiedIndex = i;
                button.OnClick = new UnityEngine.UI.Button.ButtonClickedEvent();
                button.OnClick.AddListener((UnityEngine.Events.UnityAction)(() => {
                    gameSettingMenu.RegularGameSettings.SetActive(false);
                    gameSettingMenu.RolesSettings.gameObject.SetActive(false);
                    modRoleSettings.gameObject.SetActive(false);
                    gameSettingMenu.GameSettingsHightlight.enabled = false;
                    gameSettingMenu.RolesSettingsHightlight.enabled = false;

                    if (copiedIndex == 0)
                    {
                        gameSettingMenu.RegularGameSettings.SetActive(true);
                        gameSettingMenu.GameSettingsHightlight.enabled = true;
                    }
                    else if (copiedIndex == 1)
                    {
                        gameSettingMenu.RolesSettings.gameObject.SetActive(true);
                        gameSettingMenu.RolesSettingsHightlight.enabled = true;
                    }
                    else if (copiedIndex == 2)
                    {
                        modRoleSettings.gameObject.SetActive(true);
                    }
                }));
            }

            foreach (OptionBehaviour option in modeRoleMenu.GetComponentsInChildren<OptionBehaviour>())
                UnityEngine.Object.Destroy(option.gameObject);

            
            List<OptionBehaviour> list = new List<OptionBehaviour>();
            foreach(var selecter in ConfigLoader.selecters.Values)
            {
                StringOption stringOption = UnityEngine.Object.Instantiate(optionOriginal, modeRoleMenu.transform);
                list.Add(stringOption);
                stringOption.OnValueChanged = new Action<OptionBehaviour>((o) => { });
                stringOption.TitleText.text = selecter.Name;
                stringOption.Value = selecter.Index;
                stringOption.oldValue = selecter.Index;
                stringOption.ValueText.text = selecter.Options[selecter.Index];

                selecter.Behaviour = stringOption;
                selecter.Behaviour.gameObject.SetActive(true);
            }

            modeRoleMenu.Children = list.ToArray();
            modRoleSettings.gameObject.SetActive(false);
        }
    }

    [HarmonyPatch(typeof(GameSettingMenu), nameof(GameSettingMenu.Start))]
    class GameSettingMenuStartPatch
    {
        public static void Prefix(GameSettingMenu __instance)
        {
            __instance.HideForOnline = new Transform[] { };
        }
    }

    [HarmonyPatch(typeof(GameOptionsMenu), nameof(GameOptionsMenu.Update))]
    class GameOptionsMenuUpdatePatch
    {
        public static void Postfix(GameOptionsMenu __instance)
        {
            __instance.GetComponentInParent<Scroller>().ContentYBounds.max = 55F;

            float offset = 2.5f;
            foreach(var selecter in ConfigLoader.selecters.Values)
            {
                if (selecter.Behaviour is null || selecter.Behaviour.gameObject is null) continue;

                bool isActive = true;
                var parent = selecter.Parent;
                while (parent != null && isActive)
                {
                    isActive = parent.Index != 0;
                    parent = parent.Parent;
                }

                selecter.Behaviour.gameObject.SetActive(isActive);
                if (isActive)
                {
                    offset -= selecter.MarginTop ? 0.75f : 0.5f;
                    var vector3 = new Vector3(selecter.Behaviour.transform.localPosition.x, offset, selecter.Behaviour.transform.localPosition.z);
                    selecter.Behaviour.transform.localPosition = vector3;
                }
            }
        }
    }

    [HarmonyPatch(typeof(StringOption), nameof(StringOption.OnEnable))]
    public class StringOptionEnablePatch
    {
        public static bool Prefix(StringOption __instance)
        {
            ConfigSelecter selecter = ConfigLoader.selecters.Values.FirstOrDefault(it => it.Behaviour == __instance);
            if (selecter is null) return true;
            
            __instance.OnValueChanged = new Action<OptionBehaviour>((o) => { });
            __instance.TitleText.text = selecter.Name;
            __instance.Value = selecter.Index;
            __instance.oldValue = selecter.Index;
            __instance.ValueText.text = selecter.Options[selecter.Index];
            return false;
        }
    }

    [HarmonyPatch(typeof(StringOption), nameof(StringOption.Increase))]
    public class StringOptionIncreasePatch
    {
        public static bool Prefix(StringOption __instance)
        {
            ConfigSelecter selecter = ConfigLoader.selecters.Values.FirstOrDefault(it => it.Behaviour == __instance);
            if (selecter is null) return true;

            selecter.Increase();
            return false;
        }
    }

    [HarmonyPatch(typeof(StringOption), nameof(StringOption.Decrease))]
    public class StringOptionDecreasePatch
    {
        public static bool Prefix(StringOption __instance)
        {
            ConfigSelecter selecter = ConfigLoader.selecters.Values.FirstOrDefault(it => it.Behaviour == __instance);
            if (selecter is null) return true;

            selecter.Decrease();
            return false;
        }
    }
}
