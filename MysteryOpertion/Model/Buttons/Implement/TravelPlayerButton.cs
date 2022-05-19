using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MysteryOpertion.Model.Buttons.Implement
{
    public class TravelPlayerButton : ButtonBase, TargetedButton
    {
        public Player TargetPlayer { get; set; }
        public Player MarkedPlayer { get; set; }

        public TravelPlayerButton(Player player) : base(player)
        {
            this.CooldownTime = 10f;
            this.SanityCost = 10;
            this.text = ButtonTextDictionary.TravelPlayerButtonText_Mark;
            positionOffset = new Vector3(-0.9f, 1f, 0f);
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
            if (MarkedPlayer == null)
            {
                if (TargetPlayer == null) return;

                MarkedPlayer = TargetPlayer;
                text = ButtonTextDictionary.TravelPlayerButtonText_Travel;
            }
            else
            {
                Vector2 position;
                if (MarkedPlayer.PlayerControl.Data.IsDead)
                {
                    DeadBody targetBody = null;
                    foreach(var body in UnityEngine.Object.FindObjectsOfType<DeadBody>())
                    {
                        if(body.ParentId == MarkedPlayer.PlayerControl.PlayerId)
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
                    position = MarkedPlayer.PlayerControl.GetTruePosition();
                }
                player.PlayerControl.NetTransform.SnapTo(new Vector2(position.x, position.y + 0.3636f));
                MarkedPlayer = null;
                Timer = CooldownTime;
                text = ButtonTextDictionary.TravelPlayerButtonText_Mark;

                CostSanity();
            }
        }

        public override void OnMeetingEnd()
        {
            Timer = CooldownTime;
        }

        public void UpdateTarget()
        {
            if (MarkedPlayer == null)
            {
                TargetPlayer = ToolBox.GetTarget();
                ToolBox.SetPlayerOutline(TargetPlayer?.PlayerControl, RoleInfoDictionary.Traveller.Color);
            }
            else
            {
                ToolBox.SetPlayerOutline(MarkedPlayer?.PlayerControl, RoleInfoDictionary.Traveller.Color);
            }
        }
    }
}
