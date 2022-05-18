using MysteryOpertion.Model.Roles.CrewmateRoles;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MysteryOpertion.Model.Buttons.Implement
{
    public class TravelButton : ButtonBase
    {
        public Vector2 MarkedPosition { get; set; }

        public TravelButton(Player player) : base(player) 
        {
            this.CooldownTime = 10f;
            this.SanityCost = 10;
            this.text = ButtonTextDictionary.TravelButtonText_Mark;
        }

        public override bool IsAvailable()
        {
            return player.PlayerControl.CanMove;
        }

        public override bool IsShow()
        {
            return player?.PlayerControl == PlayerControl.LocalPlayer && !player.PlayerControl.Data.IsDead;
        }

        public override void OnClick()
        {
            if(MarkedPosition == Vector2.zero)
            {
                MarkedPosition = new Vector2(player.PlayerControl.GetTruePosition().x, player.PlayerControl.GetTruePosition().y + 0.3636f);
                text = ButtonTextDictionary.TravelButtonText_Travel;
            }
            else
            {
                player.PlayerControl.NetTransform.SnapTo(MarkedPosition);
                MarkedPosition = Vector2.zero;
                Timer = CooldownTime;
                text = ButtonTextDictionary.TravelButtonText_Mark;

                CostSanity();
            }
        }

        public override void OnMeetingEnd()
        {
            Timer = CooldownTime;
        }
    }
}
