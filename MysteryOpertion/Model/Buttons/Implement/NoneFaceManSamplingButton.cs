using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MysteryOpertion.Model.Buttons.Implement
{
    public class NoneFaceManSamplingButton : ButtonBase
    {
        public Player Target { get; set; }
        public Player SamplingTarget { get; set; }

        public NoneFaceManSamplingButton(Player player) : base(player)
        {
            this.cooldownTime = 10f;
            this.sanityCost = 0;
            this.text = ButtonTextDictionary.NoneFaceManButtonText_Sampling;
            positionOffset = new Vector3(-2.7f, 0f, 0f);
        }

        public override bool IsAvailable()
        {
            return Target is not null && SamplingTarget is null && player.playerControl.CanMove;
        }

        public override bool IsShow()
        {
            return player?.playerControl == PlayerControl.LocalPlayer && !player.playerControl.Data.IsDead;
        }

        public override void OnClick()
        {
            SamplingTarget = Target;
            timer = cooldownTime;
        }

        public override void OnMeetingEnd()
        {
            timer = cooldownTime;
        }
    }
}
