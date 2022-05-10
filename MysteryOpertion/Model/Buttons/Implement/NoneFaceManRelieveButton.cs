using MysteryOpertion.Model.Roles.ImpostorRoles;
using Hazel;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MysteryOpertion.Model.Buttons.Implement
{
    public class NoneFaceManRelieveButton : ButtonBase
    {
        public NoneFaceManRelieveButton(Player player) : base(player)
        {
            this.cooldownTime = 0f;
            this.sanityCost = 0;
            this.text = ButtonTextDictionary.NoneFaceManButtonText_Relieve;
            positionOffset = new Vector3(-2.7f, 1f, 0f);
        }

        public override bool IsAvailable()
        {
            var role = (NoneFaceMan)player.mainRole;
            return role is not null && role.morphButton.isMorphed && player.playerControl.CanMove;
        }

        public override bool IsShow()
        {
            return player?.playerControl == PlayerControl.LocalPlayer && !player.playerControl.Data.IsDead;
        }

        public override void OnClick()
        {
            var role = (NoneFaceMan)player.mainRole;

            var playerId = player.playerControl.PlayerId;
            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)RPCFuncType.Morph, Hazel.SendOption.Reliable);
            writer.Write(playerId);
            writer.Write(playerId);
            RPCFunctions.Morph(playerId, playerId);

            role.morphButton.isMorphed = false;
        }

        public override void OnMeetingEnd()
        {
            timer = cooldownTime;
        }
    }
}
