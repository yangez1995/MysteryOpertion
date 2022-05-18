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
            this.CooldownTime = 15f;
            this.SanityCost = 0;
            this.text = ButtonTextDictionary.ArsonExpertOiledButtonText;

            this.oiledRequiredTime = 2;
        }

        public override bool IsAvailable()
        {
            return Target is not null && OiledTarget is null && player.PlayerControl.CanMove;
        }

        public override bool IsShow()
        {
            return player?.PlayerControl == PlayerControl.LocalPlayer && !player.PlayerControl.Data.IsDead;
        }

        public override void OnClick()
        {
            Timer = oiledRequiredTime;
            OiledTarget = Target;
        }

        public override void OnMeetingEnd()
        {
            OiledTarget = null;
            Timer = CooldownTime;
        }
    }
}
