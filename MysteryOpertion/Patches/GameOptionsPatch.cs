using HarmonyLib;
using MysteryOpertion.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

            var template = UnityEngine.Object.FindObjectsOfType<StringOption>().FirstOrDefault();
            if (template == null) return;

            var gameSettings = GameObject.Find("Game Settings");
            var gameSettingMenu = UnityEngine.Object.FindObjectsOfType<GameSettingMenu>().FirstOrDefault();

            var modSettings = UnityEngine.Object.Instantiate(gameSettings, gameSettings.transform.parent);
            var modSettingsMenu = modSettings.transform.FindChild("GameGroup").FindChild("SliderInner").GetComponent<GameOptionsMenu>();
            modSettings.name = ModSettingsName;

            var roleTab = GameObject.Find("RoleTab");
            var gameTab = GameObject.Find("GameTab");
            var modTab = UnityEngine.Object.Instantiate(roleTab, roleTab.transform.parent);

            roleTab.transform.position += Vector3.left * 0.5f;
            gameTab.transform.position += Vector3.left * 0.5f;
            modTab.transform.position += Vector3.right * 0.5f;

            var tabs = new GameObject[] { gameTab, roleTab, modTab };
            for (int i = 0; i < tabs.Length; i++)
            {
                var button = tabs[i].GetComponentInChildren<PassiveButton>();
                if (button is null) continue;
                int copiedIndex = i;
                button.OnClick = new UnityEngine.UI.Button.ButtonClickedEvent();
                button.OnClick.AddListener((UnityEngine.Events.UnityAction)(() => {
                    gameSettingMenu.RegularGameSettings.SetActive(false);
                    gameSettingMenu.RolesSettings.gameObject.SetActive(false);
                    modSettings.gameObject.SetActive(false);
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
                        modSettings.gameObject.SetActive(true);
                    }
                }));
            }

            foreach (OptionBehaviour option in modSettingsMenu.GetComponentsInChildren<OptionBehaviour>())
                UnityEngine.Object.Destroy(option.gameObject);

            List<OptionBehaviour> list = new List<OptionBehaviour>();
            for (int i = 0; i < ConfigSelecters.selecters.Count; i++)
            {
                ConfigSelecter selecter = ConfigSelecters.selecters[i];

                StringOption stringOption = UnityEngine.Object.Instantiate(template, modSettingsMenu.transform);
                list.Add(stringOption);
                stringOption.OnValueChanged = new Action<OptionBehaviour>((o) => { });
                stringOption.TitleText.text = selecter.Name;
                stringOption.Value = selecter.Index;
                stringOption.oldValue = selecter.Index;
                stringOption.ValueText.text = selecter.Options[selecter.Index];

                selecter.Behaviour = stringOption;
                selecter.Behaviour.gameObject.SetActive(true);
            }
            modSettingsMenu.Children = list.ToArray();
            modSettings.gameObject.SetActive(false);
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
            float offset = 2.5f;
            foreach(var selecter in ConfigSelecters.selecters)
            {
                if (selecter.Behaviour is null || selecter.Behaviour.gameObject is null) continue;

                selecter.Behaviour.gameObject.SetActive(true);
                offset -= 0.5f;

                var vector3 = new Vector3(selecter.Behaviour.transform.localPosition.x, offset, selecter.Behaviour.transform.localPosition.z);
                selecter.Behaviour.transform.localPosition = vector3;
            }
        }
    }

    [HarmonyPatch(typeof(StringOption), nameof(StringOption.OnEnable))]
    public class StringOptionEnablePatch
    {
        public static bool Prefix(StringOption __instance)
        {
            ConfigSelecter selecter = ConfigSelecters.selecters.FirstOrDefault(it => it.Behaviour == __instance);
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
            ConfigSelecter selecter = ConfigSelecters.selecters.FirstOrDefault(it => it.Behaviour == __instance);
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
            ConfigSelecter selecter = ConfigSelecters.selecters.FirstOrDefault(it => it.Behaviour == __instance);
            if (selecter is null) return true;

            selecter.Decrease();
            return false;
        }
    }
}
