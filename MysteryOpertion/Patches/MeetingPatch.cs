using MysteryOpertion.Model.Roles.CrewmateRoles;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnhollowerBaseLib;
using UnityEngine;

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

                    if (i == 0 && voterState.SkippedVote && !player.playerControl.Data.IsDead) //弃权票
                    {
                        __instance.BloopAVoteIcon(player.playerControl.Data, SkipCounter, __instance.SkippedVoting.transform);
                        SkipCounter++;
                    }
                    else if (voterState.VotedForId == targetPlayerId && !player.playerControl.Data.IsDead)
                    {
                        __instance.BloopAVoteIcon(player.playerControl.Data, voteCounter, playerVoteArea.transform);
                        voteCounter++;
                    }

                    //法官重复投票操作一次
                    if(player.mainRole is Judge)
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

    [HarmonyPatch(typeof(MeetingHud), "BloopAVoteIcon")]
    class MeetingHudBloopAVoteIconPatch
    {
        // Token: 0x06000624 RID: 1572 RVA: 0x0002C0C8 File Offset: 0x0002A2C8
        public static bool Prefix(MeetingHud __instance, [HarmonyArgument(0)] GameData.PlayerInfo voterPlayer, [HarmonyArgument(1)] int index, [HarmonyArgument(2)] Transform parent)
        {
            var player = Players.GetLocalPlayer();

            SpriteRenderer spriteRenderer = UnityEngine.Object.Instantiate<SpriteRenderer>(__instance.PlayerVotePrefab);
            if (!PlayerControl.GameOptions.AnonymousVotes || PlayerControl.LocalPlayer.Data.IsDead || player.mainRole is Judge)
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
            SendCoronerChatMessage();
        }

        private static void SendCoronerChatMessage()
        {
            foreach(var record in GameNote.DeathRecords)
            {
                if(record.IsDeadInCurrentRound)
                {
                    foreach (var player in Players.playerList)
                    {
                        if (player.mainRole is not Coroner) continue;

                        string msg = TextDictionary.GenerateCoronerReport(record);
                        DestroyableSingleton<HudManager>.Instance.Chat.AddChat(player.playerControl, msg);
                    }
                }
            }
        }
    }
}
