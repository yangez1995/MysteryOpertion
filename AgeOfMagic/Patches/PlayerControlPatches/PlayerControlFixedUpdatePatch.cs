using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AgeOfMagic.Patches.PlayerControlPatches
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.FixedUpdate))]
    internal class PlayerControlFixedUpdatePatch
    {
        public static void Postfix(PlayerControl __instance)
        {
            updatePlayerInfo();
        }

        private static void updatePlayerInfo()
        {
            foreach (var player in Players.playerList)
            {
                if(player.playerControl == PlayerControl.LocalPlayer)
                {
                    //player.resetPlayerName();
                    Transform playerInfoTransform = player.playerControl.nameText.transform.parent.FindChild("Info");
                    TMPro.TextMeshPro playerInfo = playerInfoTransform != null ? playerInfoTransform.GetComponent<TMPro.TextMeshPro>() : null;
                    if (playerInfo == null)
                    {
                        playerInfo = UnityEngine.Object.Instantiate(player.playerControl.nameText, player.playerControl.nameText.transform.parent);
                        playerInfo.transform.localPosition += Vector3.up * 0.5f;
                        playerInfo.fontSize *= 0.75f;
                        playerInfo.gameObject.name = "Info";
                    }

                    playerInfo.text = player.GetRoleName();
                    //PlayerVoteArea playerVoteArea = MeetingHud.Instance?.playerStates?.FirstOrDefault(x => x.TargetPlayerId == p.PlayerId);
                    //Transform meetingInfoTransform = playerVoteArea != null ? playerVoteArea.NameText.transform.parent.FindChild("Info") : null;
                    //TMPro.TextMeshPro meetingInfo = meetingInfoTransform != null ? meetingInfoTransform.GetComponent<TMPro.TextMeshPro>() : null;
                    //if (meetingInfo == null && playerVoteArea != null)
                    //{
                    //    meetingInfo = UnityEngine.Object.Instantiate(playerVoteArea.NameText, playerVoteArea.NameText.transform.parent);
                    //    meetingInfo.transform.localPosition += Vector3.down * 0.10f;
                    //    meetingInfo.fontSize *= 0.60f;
                    //    meetingInfo.gameObject.name = "Info";
                    //}

                    //// Set player name higher to align in middle
                    //if (meetingInfo != null && playerVoteArea != null)
                    //{
                    //    var playerName = playerVoteArea.NameText;
                    //    playerName.transform.localPosition = new Vector3(0.3384f, (0.0311f + 0.0683f), -0.1f);
                    //}
                }
            }
        }
    }
}
