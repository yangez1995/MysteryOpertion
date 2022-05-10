using System;
using System.Collections.Generic;
using System.Text;

namespace MysteryOpertion.Model.Buttons.Implement
{
    public class ArsonExpertOiledButton : ButtonBase
    {
        public float oiledRequiredTime;

        public Player Target { get; set; }
        public Player OiledTarget { get; set; }

        public ArsonExpertOiledButton(Player player) : base(player)
        {
            this.cooldownTime = 15f;
            this.sanityCost = 0;
            this.text = ButtonTextDictionary.ArsonExpertOiledButtonText;

            this.oiledRequiredTime = 2;
        }

        public override bool IsAvailable()
        {
            return Target is not null && OiledTarget is null && player.playerControl.CanMove;
        }

        public override bool IsShow()
        {
            return player?.playerControl == PlayerControl.LocalPlayer && !player.playerControl.Data.IsDead;
        }

        public override void OnClick()
        {
            timer = oiledRequiredTime;
            OiledTarget = Target;
        }

        public override void OnMeetingEnd()
        {
            OiledTarget = null;
            timer = cooldownTime;
        }
    }
}
