using MysteryOpertion.Model.Roles.CrewmateRoles;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnhollowerBaseLib;
using UnityEngine;
using MysteryOpertion.Model.Roles.ImpostorRoles;
using System.Linq;
using MysteryOpertion.Model.Roles;
using Hazel;
using MysteryOpertion.Model.Roles.ChaosRoles;
using TMPro;
using MysteryOpertion.Model;
using MysteryOpertion.Model.MysteryItems;

namespace MysteryOpertion.Patches
{
    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.PopulateResults))]
    class MeetingHudPopulateVotesPatch
    {
        static bool Prefix(MeetingHud __instance, Il2CppStructArray<MeetingHud.VoterState> states)
        {
            int SkipCounter = 0;
            bool mayorFirstVoteDisplayed = false;
            for (int i = 0; i < __instance.playerStates.Length; i++)
            {
                var playerVoteArea = __instance.playerStates[i];
                byte targetPlayerId = playerVoteArea.TargetPlayerId;
                playerVoteArea.ClearForResults();

                int voteCounter = 0;
                for (int j = 0; j < states.Length; j++)
                {
                    MeetingHud.VoterState voterState = states[j];
                    var player = Players.GetPlayer(voterState.VoterId);

                    if (i == 0 && voterState.SkippedVote && !player.PlayerControl.Data.IsDead) //弃权票
                    {
                        __instance.BloopAVoteIcon(player.PlayerControl.Data, SkipCounter, __instance.SkippedVoting.transform);
                        SkipCounter++;
                    }
                    else if (voterState.VotedForId == targetPlayerId && !player.PlayerControl.Data.IsDead)
                    {
                        __instance.BloopAVoteIcon(player.PlayerControl.Data, voteCounter, playerVoteArea.transform);
                        voteCounter++;
                    }

                    //法官重复投票操作一次
                    if (player.MainRole is Judge)
                    {
                        if (mayorFirstVoteDisplayed)
                        {
                            mayorFirstVoteDisplayed = false;
                        }
                        else
                        {
                            mayorFirstVoteDisplayed = true;
                            j--;
                        }
                    }
                }
            }

            return false;
        }
    }

    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.BloopAVoteIcon))]
    class MeetingHudBloopAVoteIconPatch
    {
        // Token: 0x06000624 RID: 1572 RVA: 0x0002C0C8 File Offset: 0x0002A2C8
        public static bool Prefix(MeetingHud __instance, [HarmonyArgument(0)] GameData.PlayerInfo voterPlayer, [HarmonyArgument(1)] int index, [HarmonyArgument(2)] Transform parent)
        {
            var player = Players.GetLocalPlayer();

            bool JudgeSeeVoteColor = ConfigLoader.selecters[ConfigKeyDictionary.JudgeSeeVoteColor].GetBoolValue();
            SpriteRenderer spriteRenderer = UnityEngine.Object.Instantiate<SpriteRenderer>(__instance.PlayerVotePrefab);
            if (!PlayerControl.GameOptions.AnonymousVotes || PlayerControl.LocalPlayer.Data.IsDead || (player.MainRole is Judge && JudgeSeeVoteColor))
                PlayerControl.SetPlayerMaterialColors(voterPlayer.DefaultOutfit.ColorId, spriteRenderer);
            else
                PlayerControl.SetPlayerMaterialColors(Palette.DisabledGrey, spriteRenderer);

            spriteRenderer.transform.SetParent(parent);
            spriteRenderer.transform.localScale = Vector3.zero;
            __instance.StartCoroutine(Effects.Bloop((float)index * 0.3f, spriteRenderer.transform, 1f, 0.5f));
            parent.GetComponent<VoteSpreader>().AddVote(spriteRenderer);
            return false;
        }
    }

    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.ServerStart))]
    class MeetingServerStartPatch
    {
        public static void Postfix(MeetingHud __instance)
        {
            SendChatMessage();
            AddCurseButtons(__instance);
        }

        private static void SendChatMessage()
        {
            foreach (var player in Players.playerList)
            {
                if (player.MainRole is Coroner)
                {
                    SendCoronerReport(player);
                }
                else if (player.ItemBag.ContainsKey(ItemType.CoronerScalpel))
                {
                    SendCoronerReport(player);
                    player.ItemBag[ItemType.CoronerScalpel].OnUse();
                }
                else if (player.MainRole is Eavesdropper)
                {
                    ((Eavesdropper)player.MainRole).SendEavesdropperMessage();
                }
            }
        }

        private static void SendCoronerReport(Player player)
        {
            foreach (var record in GameNote.DeathRecords)
            {
                if (record.IsDeadInCurrentRound)
                {
                    string msg = TextDictionary.GenerateCoronerReport(record);
                    DestroyableSingleton<HudManager>.Instance.Chat.AddChat(player.PlayerControl, msg);
                }
            }
        }

        private static void AddCurseButtons(MeetingHud __instance)
        {
            var player = Players.GetLocalPlayer();
            if (player.PlayerControl.Data.IsDead) return;

            if (player.MainRole is CurseMage)
            {
                var role = (CurseMage)player.MainRole;
                if (role.CurseTimes <= 0) return;

                AddCurseButton(__instance);
                return;
            }

            if (player.MainRole is CurseWarlock)
            {
                var role = (CurseWarlock)player.MainRole;
                if (role.CurseTimes <= 0) return;

                AddCurseButton(__instance);
                return;
            }

            if (player.MainRole is BloodyHunter)
            {
                var role = (BloodyHunter)player.MainRole;
                if (!role.BloodyHunterButton.CurseFlag) return;

                AddCurseButton(__instance);
                return;
            }

            if (player.MainRole is Spectator)
            {
                AddGuessButton(__instance);
                return;
            }
        }

        private static void AddCurseButton(MeetingHud __instance)
        {
            for (int i = 0; i < __instance.playerStates.Length; i++)
            {
                PlayerVoteArea playerVoteArea = __instance.playerStates[i];
                if (playerVoteArea.AmDead || playerVoteArea.TargetPlayerId == PlayerControl.LocalPlayer.PlayerId) continue;

                GameObject original = playerVoteArea.Buttons.transform.Find("CancelButton").gameObject;
                GameObject targetBox = UnityEngine.Object.Instantiate(original, playerVoteArea.transform);
                targetBox.name = "CurseButton";
                targetBox.transform.localPosition = new Vector3(-0.95f, 0.03f, -1.3f);
                SpriteRenderer renderer = targetBox.GetComponent<SpriteRenderer>();
                renderer.sprite = ToolBox.loadSpriteFromResources("MysteryOpertion.Resources.TargetIcon.png", 150f);
                PassiveButton button = targetBox.GetComponent<PassiveButton>();
                button.OnClick.RemoveAllListeners();
                int copiedIndex = i;
                button.OnClick.AddListener((System.Action)(() => CurseOnClick(copiedIndex, __instance)));
            }
        }

        public static GameObject curseUI;
        private static void CurseOnClick(int buttonTarget, MeetingHud __instance)
        {
            if (curseUI != null || !(__instance.state == MeetingHud.VoteStates.Voted || __instance.state == MeetingHud.VoteStates.NotVoted)) return;
            __instance.playerStates.ToList().ForEach(x => x.gameObject.SetActive(false));

            var container = UnityEngine.Object.Instantiate(__instance.transform.FindChild("PhoneUI"), __instance.transform);
            container.transform.localPosition = new Vector3(0, 0, -5f);
            container.transform.localScale *= 0.8f;
            curseUI = container.gameObject;

            var exitButtonBox = new GameObject();
            exitButtonBox.transform.SetParent(container);
            exitButtonBox.transform.transform.localPosition = new Vector3(2.725f, 2.1f, -5);
            exitButtonBox.transform.transform.localScale = new Vector3(0.217f, 0.9f, 1);

            var buttonOriginal = __instance.playerStates[0].transform.FindChild("votePlayerBase");
            var cancelButtonOriginal = __instance.playerStates[0].Buttons.transform.Find("CancelButton");

            var exitButton = UnityEngine.Object.Instantiate(buttonOriginal.transform, exitButtonBox.transform);
            exitButton.gameObject.GetComponent<SpriteRenderer>().sprite = cancelButtonOriginal.GetComponent<SpriteRenderer>().sprite;
            exitButton.GetComponent<PassiveButton>().OnClick.RemoveAllListeners();
            exitButton.GetComponent<PassiveButton>().OnClick.AddListener((Action)(() =>
            {
                __instance.playerStates.ToList().ForEach(x => x.gameObject.SetActive(true));
                UnityEngine.Object.Destroy(container.gameObject);
            }));

            var maskAreaOriginal = __instance.playerStates[0].transform.FindChild("MaskArea");
            var exitButtonMaskArea = UnityEngine.Object.Instantiate(maskAreaOriginal, exitButtonBox.transform);

            var textOriginal = __instance.playerStates[0].NameText;
            List<RoleInfo> roleInfoList = new List<RoleInfo>();
            roleInfoList.Add(RoleInfoDictionary.Crewmate);
            roleInfoList.Add(RoleInfoDictionary.Sheriff);
            roleInfoList.Add(RoleInfoDictionary.Traveller);
            roleInfoList.Add(RoleInfoDictionary.MechanicExpert);
            roleInfoList.Add(RoleInfoDictionary.LightPrayer);
            roleInfoList.Add(RoleInfoDictionary.Judge);
            roleInfoList.Add(RoleInfoDictionary.DoomBait);
            roleInfoList.Add(RoleInfoDictionary.Coroner);
            roleInfoList.Add(RoleInfoDictionary.Diviner);
            roleInfoList.Add(RoleInfoDictionary.Eavesdropper);
            roleInfoList.Add(RoleInfoDictionary.CurseMage);
            roleInfoList.Add(RoleInfoDictionary.ArsonExpert);
            roleInfoList.Add(RoleInfoDictionary.Spectator);
            roleInfoList.Add(RoleInfoDictionary.Impostor);
            roleInfoList.Add(RoleInfoDictionary.NoneFaceMan);
            roleInfoList.Add(RoleInfoDictionary.SerialKiller);
            roleInfoList.Add(RoleInfoDictionary.CurseWarlock);

            int i = 0;
            var sourcePlayer = Players.GetLocalPlayer();
            foreach (var roleInfo in roleInfoList)
            {
                int row = i / 5, col = i % 5;
                var roleLabelBox = new GameObject();
                roleLabelBox.transform.SetParent(container);
                roleLabelBox.transform.localPosition = new Vector3(-3.47f + 1.75f * col, 1.5f - 0.45f * row, -5);
                roleLabelBox.transform.localScale = new Vector3(0.55f, 0.55f, 1f);

                var roleLabelButton = UnityEngine.Object.Instantiate(buttonOriginal, roleLabelBox.transform);
                roleLabelButton.GetComponent<SpriteRenderer>().sprite = DestroyableSingleton<HatManager>.Instance.GetNamePlateById("nameplate_NoPlate")?.viewData?.viewData?.Image;
                roleLabelButton.GetComponent<PassiveButton>().OnClick.RemoveAllListeners();
                roleLabelButton.GetComponent<PassiveButton>().OnClick.AddListener((System.Action)(() =>
                {
                    var targetPlayer = Players.GetPlayer((byte)__instance.playerStates[buttonTarget].TargetPlayerId);
                    if (targetPlayer.MainRole.GetRoleName() != roleInfo.Name)
                        targetPlayer = sourcePlayer;

                    __instance.playerStates.ToList().ForEach(x => x.gameObject.SetActive(true));
                    UnityEngine.Object.Destroy(container.gameObject);
                    __instance.playerStates.ToList().ForEach(x =>
                    {
                        if (x.TargetPlayerId == targetPlayer.PlayerControl.PlayerId && x.transform.FindChild("CurseButton") != null)
                            UnityEngine.Object.Destroy(x.transform.FindChild("CurseButton").gameObject);
                    });

                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)RPCFuncType.CurseKill, Hazel.SendOption.Reliable);
                    writer.Write(sourcePlayer.PlayerControl.PlayerId);
                    writer.Write(targetPlayer.PlayerControl.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    RPCFunctions.CurseKill(sourcePlayer.PlayerControl.PlayerId, targetPlayer.PlayerControl.PlayerId);
                }));

                var roleLabelMask = UnityEngine.Object.Instantiate(maskAreaOriginal, roleLabelBox.transform);

                var roleLabel = UnityEngine.Object.Instantiate(textOriginal, roleLabelButton);
                roleLabel.text = ToolBox.FormatRoleTaskText(roleInfo.Color, roleInfo.Name);
                roleLabel.alignment = TMPro.TextAlignmentOptions.Center;
                roleLabel.transform.localPosition = new Vector3(0, 0, roleLabel.transform.localPosition.z);
                roleLabel.transform.localScale *= 1.7f;

                i++;
            }
        }

        private static Dictionary<int, Guess> guesses;
        private static GameObject confirmButtonBox;
        private static void AddGuessButton(MeetingHud __instance)
        {
            var buttonOriginal = __instance.playerStates[0].transform.FindChild("votePlayerBase");
            var maskAreaOriginal = __instance.playerStates[0].transform.FindChild("MaskArea");
            var textOriginal = __instance.playerStates[0].NameText;

            guesses = new Dictionary<int, Guess>();
            for (int i = 0; i < __instance.playerStates.Length; i++)
            {
                PlayerVoteArea playerVoteArea = __instance.playerStates[i];
                if (playerVoteArea.TargetPlayerId == PlayerControl.LocalPlayer.PlayerId) continue;

                var guessButtonBox = new GameObject();
                guessButtonBox.transform.SetParent(playerVoteArea.transform);
                guessButtonBox.transform.transform.localPosition = new Vector3(0.35f, -0.08f, -2f);
                guessButtonBox.transform.transform.localScale = new Vector3(0.35f, 0.35f, 1f);

                var guessButton = UnityEngine.Object.Instantiate(buttonOriginal, guessButtonBox.transform);
                guessButton.GetComponent<SpriteRenderer>().sprite = DestroyableSingleton<HatManager>.Instance.GetNamePlateById("nameplate_NoPlate")?.viewData?.viewData?.Image;
                guessButton.GetComponent<PassiveButton>().OnClick.RemoveAllListeners();

                var guessButtonMask = UnityEngine.Object.Instantiate(maskAreaOriginal, guessButtonBox.transform);

                var buttonText = UnityEngine.Object.Instantiate(textOriginal, guessButton);
                buttonText.text = ToolBox.FormatRoleTaskText(Color.gray, "猜测");
                buttonText.alignment = TMPro.TextAlignmentOptions.Center;
                buttonText.transform.localPosition = new Vector3(0, 0, guessButton.transform.localPosition.z);
                buttonText.transform.localScale *= 1.7f;

                int index = i;
                guessButton.GetComponent<PassiveButton>().OnClick.AddListener((System.Action)(() => GuessOnClick(index, __instance, buttonText)));
                guesses.Add(index, new Guess { Player = Players.GetPlayer(playerVoteArea.TargetPlayerId), RoleName = String.Empty });
            }

            confirmButtonBox = new GameObject();
            confirmButtonBox.name = "ConfirmButton";
            confirmButtonBox.transform.SetParent(__instance.transform);
            confirmButtonBox.transform.transform.localPosition = new Vector3(0f, -2.2f, -5f);
            confirmButtonBox.transform.transform.localScale = new Vector3(0.35f, 0.35f, 1f);

            var confirmButton = UnityEngine.Object.Instantiate(buttonOriginal, confirmButtonBox.transform);
            confirmButton.GetComponent<SpriteRenderer>().sprite = DestroyableSingleton<HatManager>.Instance.GetNamePlateById("nameplate_NoPlate")?.viewData?.viewData?.Image;
            confirmButton.GetComponent<PassiveButton>().OnClick.RemoveAllListeners();
            confirmButton.GetComponent<PassiveButton>().OnClick.AddListener((System.Action)(() => {
                int correctGuesses = guesses.Values.Where(it => it.Player.MainRole.GetRoleName() == it.RoleName).Count();
                if (correctGuesses < 3)
                {
                    confirmButtonBox.SetActive(false);

                    var resultText = UnityEngine.Object.Instantiate(textOriginal, __instance.transform);
                    resultText.text = ToolBox.FormatRoleTaskText(Color.white, $"猜对了{correctGuesses}人的职业");
                    resultText.alignment = TMPro.TextAlignmentOptions.Center;
                    resultText.transform.localPosition = new Vector3(0f, -2.2f, -5f);
                }
                else
                {
                    GameNote.SpectatorWinner = Players.GetLocalPlayer();
                }
            }));

            var confirmButtonMask = UnityEngine.Object.Instantiate(maskAreaOriginal, confirmButtonBox.transform);

            var confirmButtonText = UnityEngine.Object.Instantiate(textOriginal, confirmButton);
            confirmButtonText.text = ToolBox.FormatRoleTaskText(Color.gray, "确认猜测");
            confirmButtonText.alignment = TMPro.TextAlignmentOptions.Center;
            confirmButtonText.transform.localPosition = new Vector3(0, 0, confirmButton.transform.localPosition.z - 5);
            confirmButtonText.transform.localScale *= 1.7f;

            confirmButtonBox.SetActive(false);
        }

        public static GameObject guessUI;
        private static void GuessOnClick(int buttonTarget, MeetingHud __instance, TextMeshPro currentText)
        {
            if (guessUI != null || !(__instance.state == MeetingHud.VoteStates.Voted || __instance.state == MeetingHud.VoteStates.NotVoted)) return;
            __instance.playerStates.ToList().ForEach(x => x.gameObject.SetActive(false));

            var container = UnityEngine.Object.Instantiate(__instance.transform.FindChild("PhoneUI"), __instance.transform);
            container.transform.localPosition = new Vector3(0, 0, -5f);
            container.transform.localScale *= 0.8f;
            guessUI = container.gameObject;

            var exitButtonBox = new GameObject();
            exitButtonBox.transform.SetParent(container);
            exitButtonBox.transform.transform.localPosition = new Vector3(2.725f, 2.1f, -5);
            exitButtonBox.transform.transform.localScale = new Vector3(0.217f, 0.9f, 1);

            var buttonOriginal = __instance.playerStates[0].transform.FindChild("votePlayerBase");
            var cancelButtonOriginal = __instance.playerStates[0].Buttons.transform.Find("CancelButton");

            var exitButton = UnityEngine.Object.Instantiate(buttonOriginal.transform, exitButtonBox.transform);
            exitButton.gameObject.GetComponent<SpriteRenderer>().sprite = cancelButtonOriginal.GetComponent<SpriteRenderer>().sprite;
            exitButton.GetComponent<PassiveButton>().OnClick.RemoveAllListeners();
            exitButton.GetComponent<PassiveButton>().OnClick.AddListener((Action)(() =>
            {
                __instance.playerStates.ToList().ForEach(x => x.gameObject.SetActive(true));
                UnityEngine.Object.Destroy(container.gameObject);
            }));

            var maskAreaOriginal = __instance.playerStates[0].transform.FindChild("MaskArea");
            var exitButtonMaskArea = UnityEngine.Object.Instantiate(maskAreaOriginal, exitButtonBox.transform);

            var textOriginal = __instance.playerStates[0].NameText;
            List<RoleInfo> roleInfoList = new List<RoleInfo>();
            roleInfoList.Add(RoleInfoDictionary.Crewmate);
            roleInfoList.Add(RoleInfoDictionary.Sheriff);
            roleInfoList.Add(RoleInfoDictionary.Traveller);
            roleInfoList.Add(RoleInfoDictionary.MechanicExpert);
            roleInfoList.Add(RoleInfoDictionary.LightPrayer);
            roleInfoList.Add(RoleInfoDictionary.Judge);
            roleInfoList.Add(RoleInfoDictionary.DoomBait);
            roleInfoList.Add(RoleInfoDictionary.Coroner);
            roleInfoList.Add(RoleInfoDictionary.Diviner);
            roleInfoList.Add(RoleInfoDictionary.Eavesdropper);
            roleInfoList.Add(RoleInfoDictionary.CurseMage);
            roleInfoList.Add(RoleInfoDictionary.ArsonExpert);
            roleInfoList.Add(RoleInfoDictionary.Spectator);
            roleInfoList.Add(RoleInfoDictionary.Impostor);
            roleInfoList.Add(RoleInfoDictionary.NoneFaceMan);
            roleInfoList.Add(RoleInfoDictionary.SerialKiller);
            roleInfoList.Add(RoleInfoDictionary.CurseWarlock);

            int i = 0;
            var sourcePlayer = Players.GetLocalPlayer();
            foreach (var roleInfo in roleInfoList)
            {
                int row = i / 5, col = i % 5;
                var roleLabelBox = new GameObject();
                roleLabelBox.transform.SetParent(container);
                roleLabelBox.transform.localPosition = new Vector3(-3.47f + 1.75f * col, 1.5f - 0.45f * row, -5);
                roleLabelBox.transform.localScale = new Vector3(0.55f, 0.55f, 1f);

                var roleLabelButton = UnityEngine.Object.Instantiate(buttonOriginal, roleLabelBox.transform);
                roleLabelButton.GetComponent<SpriteRenderer>().sprite = DestroyableSingleton<HatManager>.Instance.GetNamePlateById("nameplate_NoPlate")?.viewData?.viewData?.Image;
                roleLabelButton.GetComponent<PassiveButton>().OnClick.RemoveAllListeners();
                roleLabelButton.GetComponent<PassiveButton>().OnClick.AddListener((System.Action)(() =>
                {
                    __instance.playerStates.ToList().ForEach(x => x.gameObject.SetActive(true));
                    UnityEngine.Object.Destroy(container.gameObject);

                    currentText.text = ToolBox.FormatRoleTaskText(roleInfo.Color, roleInfo.Name);
                    guesses[buttonTarget].RoleName = roleInfo.Name;

                    int guessedCount = guesses.Values.Where(it => !string.IsNullOrEmpty(it.RoleName)).Count();
                    if (guessedCount >= 3)
                        confirmButtonBox.SetActive(true);
                }));

                var roleLabelMask = UnityEngine.Object.Instantiate(maskAreaOriginal, roleLabelBox.transform);

                var roleLabel = UnityEngine.Object.Instantiate(textOriginal, roleLabelButton);
                roleLabel.text = ToolBox.FormatRoleTaskText(roleInfo.Color, roleInfo.Name);
                roleLabel.alignment = TMPro.TextAlignmentOptions.Center;
                roleLabel.transform.localPosition = new Vector3(0, 0, roleLabel.transform.localPosition.z);
                roleLabel.transform.localScale *= 1.7f;

                i++;
            }
        }

        private class Guess
        {
            public Player Player { get; set; }
            public string RoleName { get; set; }
        }
    }

    [HarmonyPatch(typeof(PlayerVoteArea), nameof(PlayerVoteArea.Select))]
    class PlayerVoteAreaSelectPatch
    {
        static bool Prefix(PlayerVoteArea __instance)
        {
            //if (__instance.transform.parent.name == "ConfirmButton") return false;
            //Debug.Log("ShowName======" + __instance.transform.parent.name);
            //Debug.Log("ShowCount======" + __instance.transform.childCount);

            var player = Players.GetLocalPlayer();
            var flag1 = !((player.MainRole is CurseMage || player.MainRole is CurseWarlock) && MeetingServerStartPatch.curseUI != null);
            var flag2 = !(player.MainRole is Spectator && MeetingServerStartPatch.guessUI != null);
            return flag1 && flag2;
        }
    }
}
