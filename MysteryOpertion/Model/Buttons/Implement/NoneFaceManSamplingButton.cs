using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MysteryOpertion.Model.Buttons.Implement
{
    public class NoneFaceManSamplingButton : ButtonBase, TargetedButton
    {
        public Player Target { get; set; }
        public Player SamplingTarget { get; set; }

        public NoneFaceManSamplingButton(Player player) : base(player)
        {
            this.CooldownTime = 10f;
            this.SanityCost = 0;
            this.text = ButtonTextDictionary.NoneFaceManButtonText_Sampling;
            positionOffset = new Vector3(-2.7f, 0f, 0f);
        }

        public override bool IsAvailable()
        {
            return Target is not null && SamplingTarget is null && player.PlayerControl.CanMove;
        }

        public override bool IsShow()
        {
            return player?.PlayerControl == PlayerControl.LocalPlayer && !player.PlayerControl.Data.IsDead;
        }

        public override void OnClick()
        {
            SamplingTarget = Target;
            Timer = CooldownTime;
        }

        public override void OnMeetingEnd()
        {
            Timer = CooldownTime;
        }

        public void UpdateTarget()
        {
            Target = ToolBox.GetTarget();
            ToolBox.SetPlayerOutline(Target?.PlayerControl, RoleInfoDictionary.NoneFaceMan.Color);
        }
    }
}
