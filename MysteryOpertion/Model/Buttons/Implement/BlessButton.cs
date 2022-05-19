using Hazel;
using System;
using System.Collections.Generic;
using System.Text;

namespace MysteryOpertion.Model.Buttons.Implement
{
    public class BlessButton : ButtonBase, TargetedButton
    {
        public Player Target { get; set; }

        public BlessButton(Player player) : base(player)
        {
            this.CooldownTime = 20f;
            this.SanityCost = 5;
            this.text = ButtonTextDictionary.BlessButtonText;
        }

        public override bool IsAvailable()
        {
            return Target is not null && player.PlayerControl.CanMove;
        }

        public override bool IsShow()
        {
            return player?.PlayerControl == PlayerControl.LocalPlayer && !player.PlayerControl.Data.IsDead;
        }

        public override void OnClick()
        {
            var sourceId = player.PlayerControl.PlayerId;
            var targetId = Target.PlayerControl.PlayerId;
            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)RPCFuncType.LightPrayerBless, SendOption.Reliable);
            writer.Write(sourceId);
            writer.Write(targetId);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            RPCFunctions.LightPrayerBless(sourceId, targetId);

            Timer = CooldownTime;
        }

        public override void OnMeetingEnd()
        {
            Timer = CooldownTime;
        }

        public void UpdateTarget()
        {
            Target = ToolBox.GetTarget();
            ToolBox.SetPlayerOutline(Target?.PlayerControl, RoleInfoDictionary.LightPrayer.Color);
        }
    }
}
