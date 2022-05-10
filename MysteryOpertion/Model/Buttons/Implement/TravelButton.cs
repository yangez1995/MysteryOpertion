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
            this.cooldownTime = 10f;
            this.sanityCost = 10;
            this.text = ButtonTextDictionary.TravelButtonText_Mark;
        }

        public override bool IsAvailable()
        {
            return player.playerControl.CanMove;
        }

        public override bool IsShow()
        {
            return player?.playerControl == PlayerControl.LocalPlayer && !player.playerControl.Data.IsDead;
        }

        public override void OnClick()
        {
            if(MarkedPosition == Vector2.zero)
            {
                MarkedPosition = new Vector2(player.playerControl.GetTruePosition().x, player.playerControl.GetTruePosition().y + 0.3636f);
                text = ButtonTextDictionary.TravelButtonText_Travel;
            }
            else
            {
                player.playerControl.NetTransform.SnapTo(MarkedPosition);
                MarkedPosition = Vector2.zero;
                timer = cooldownTime;
                text = ButtonTextDictionary.TravelButtonText_Mark;

                CostSanity();
            }
        }

        public override void OnMeetingEnd()
        {
            timer = cooldownTime;
        }
    }
}
