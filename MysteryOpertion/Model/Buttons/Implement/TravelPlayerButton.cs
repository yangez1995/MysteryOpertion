using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MysteryOpertion.Model.Buttons.Implement
{
    public class TravelPlayerButton : ButtonBase
    {
        public Player TargetPlayer { get; set; }
        public Player MarkedPlayer { get; set; }

        public TravelPlayerButton(Player player) : base(player)
        {
            this.cooldownTime = 10f;
            this.sanityCost = 10;
            this.text = ButtonTextDictionary.TravelPlayerButtonText_Mark;
            positionOffset = new Vector3(-0.9f, 1f, 0f);
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
            if (MarkedPlayer == null)
            {
                if (TargetPlayer == null) return;

                MarkedPlayer = TargetPlayer;
                text = ButtonTextDictionary.TravelPlayerButtonText_Travel;
            }
            else
            {
                Vector2 position;
                if (MarkedPlayer.playerControl.Data.IsDead)
                {
                    DeadBody targetBody = null;
                    foreach(var body in UnityEngine.Object.FindObjectsOfType<DeadBody>())
                    {
                        if(body.ParentId == MarkedPlayer.playerControl.PlayerId)
                        {
                            targetBody = body;
                            break;
                        }
                    }
                    if (targetBody == null) return;

                    position = targetBody.TruePosition;
                } 
                else
                {
                    position = MarkedPlayer.playerControl.GetTruePosition();
                }
                player.playerControl.NetTransform.SnapTo(new Vector2(position.x, position.y + 0.3636f));
                MarkedPlayer = null;
                timer = cooldownTime;
                text = ButtonTextDictionary.TravelPlayerButtonText_Mark;

                CostSanity();
            }
        }

        public override void OnMeetingEnd()
        {
            timer = cooldownTime;
        }
    }
}
