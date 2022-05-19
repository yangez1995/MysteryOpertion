using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MysteryOpertion.Model.Roles.CrewmateRoles
{
    public class Eavesdropper : Crewmate
    {
        private Player player;

        public Eavesdropper(Player player) : base(player)
        {
            this.info = RoleInfoDictionary.Eavesdropper;
            this.maxSanityPoint = 50;
            this.initialSanityPoint = 50;
            this.player = player;
        }

        public void SendEavesdropperMessage()
        {
            if (GameNote.TaskComplete.Count == 0) return;

            for (int i = GameNote.TaskComplete.Count - 1; i >= 0; i--)
            {
                var record = GameNote.TaskComplete[i];
                if (record.Player == player.PlayerControl) continue;

                string msg = TextDictionary.LastTaskComplete(record.TaskName);
                DestroyableSingleton<HudManager>.Instance.Chat.AddChat(player.PlayerControl, msg);
                break;
            }
        }

        public void ShowAllTaskCompleteMessage(PlayerControl playerControl)
        {
            RPCFunctions.ShowCenterMessage(TextDictionary.AllTaskComplete(playerControl.Data.PlayerName));
        }
    }
}
