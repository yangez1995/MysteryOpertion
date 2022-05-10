using MysteryOpertion.Model.Roles.ImpostorRoles;
using Hazel;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MysteryOpertion.Model.Buttons.Implement
{
    public class NoneFaceManMorphButton : ButtonBase
    {
        public bool isMorphed;

        public NoneFaceManMorphButton(Player player) : base(player)
        {
            this.cooldownTime = 0f;
            this.sanityCost = 15;
            this.text = ButtonTextDictionary.NoneFaceManButtonText_Morph;
            positionOffset = new Vector3(-1.8f, 0f, 0f);

            this.isMorphed = false;
        }

        public override bool IsAvailable()
        {
            var role = (NoneFaceMan)player.mainRole;
            return role is not null && role.samplingButton.SamplingTarget is not null && player.playerControl.CanMove;
        }

        public override bool IsShow()
        {
            return player?.playerControl == PlayerControl.LocalPlayer && !player.playerControl.Data.IsDead;
        }

        public override void OnClick()
        {
            var role = (NoneFaceMan)player.mainRole;

            var sourceId = player.playerControl.PlayerId;
            var targetId = role.samplingButton.SamplingTarget.playerControl.PlayerId;
            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)RPCFuncType.Morph, Hazel.SendOption.Reliable);
            writer.Write(sourceId);
            writer.Write(targetId);
            RPCFunctions.Morph(sourceId, targetId);
            role.samplingButton.SamplingTarget = null;
            isMorphed = true;
        }

        public override void OnMeetingEnd()
        {
            var playerId = player.playerControl.PlayerId;
            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)RPCFuncType.Morph, Hazel.SendOption.Reliable);
            writer.Write(playerId);
            writer.Write(playerId);
            RPCFunctions.Morph(playerId, playerId);

            timer = cooldownTime;
            isMorphed = false;
        }
    }
}
