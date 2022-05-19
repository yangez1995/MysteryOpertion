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
                    if(player.MainRole is Judge)
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

            SpriteRenderer spriteRenderer = UnityEngine.Object.Instantiate<SpriteRenderer>(__instance.PlayerVotePrefab);
            if (!PlayerControl.GameOptions.AnonymousVotes || PlayerControl.LocalPlayer.Data.IsDead || player.MainRole is Judge)
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
                    ((Coroner)player.MainRole).SendCoronerReport();
                else if (player.MainRole is Eavesdropper)
                    ((Eavesdropper)player.MainRole).SendEavesdropperMessage();
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
                button.OnClick.AddListener((System.Action)(() => guesserOnClick(copiedIndex, __instance)));
            }
        }

        private static GameObject curseUI;
        private static void guesserOnClick(int buttonTarget, MeetingHud __instance)
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
            exitButton.GetComponent<PassiveButton>().OnClick.AddListener((Action)(() => {
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
            roleInfoList.Add(RoleInfoDictionary.Impostor);
            roleInfoList.Add(RoleInfoDictionary.NoneFaceMan);
            roleInfoList.Add(RoleInfoDictionary.SerialKiller);
            roleInfoList.Add(RoleInfoDictionary.CurseWarlock);

            int i = 0;
            foreach(var roleInfo in roleInfoList)
            {
                int row = i / 5, col = i % 5;
                var roleLabelBox = new GameObject();
                roleLabelBox.transform.SetParent(container);
                roleLabelBox.transform.localPosition = new Vector3(-3.47f + 1.75f * col, 1.5f - 0.45f * row, -5);
                roleLabelBox.transform.localScale = new Vector3(0.55f, 0.55f, 1f);

                var roleLabelButton = UnityEngine.Object.Instantiate(buttonOriginal, roleLabelBox.transform);
                roleLabelButton.GetComponent<SpriteRenderer>().sprite = DestroyableSingleton<HatManager>.Instance.GetNamePlateById("nameplate_NoPlate")?.viewData?.viewData?.Image;
                roleLabelButton.GetComponent<PassiveButton>().OnClick.RemoveAllListeners();
                roleLabelButton.GetComponent<PassiveButton>().OnClick.AddListener((System.Action)(() => {
                    var targetPlayer = Players.GetPlayer((byte)__instance.playerStates[buttonTarget].TargetPlayerId);
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
    }
}
