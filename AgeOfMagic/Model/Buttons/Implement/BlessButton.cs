using Hazel;
using System;
using System.Collections.Generic;
using System.Text;

namespace MysteryOpertion.Model.Buttons.Implement
{
    public class BlessButton : ButtonBase
    {
        public Player Target { get; set; }

        public BlessButton(Player player) : base(player)
        {
            this.cooldownTime = 20f;
            this.sanityCost = 5;
            this.text = ButtonTextDictionary.BlessButtonText;
        }

        public override bool IsAvailable()
        {
            return Target is not null && player.playerControl.CanMove;
        }

        public override bool IsShow()
        {
            return player?.playerControl == PlayerControl.LocalPlayer && !player.playerControl.Data.IsDead;
        }

        public override void OnClick()
        {
            var sourceId = player.playerControl.PlayerId;
            var targetId = Target.playerControl.PlayerId;
            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)RPCFuncType.LightPrayerBless, SendOption.Reliable);
            writer.Write(sourceId);
            writer.Write(targetId);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            RPCFunctions.LightPrayerBless(sourceId, targetId);

            timer = cooldownTime;
        }

        public override void OnMeetingEnd()
        {
            timer = cooldownTime;
        }
    }
}
