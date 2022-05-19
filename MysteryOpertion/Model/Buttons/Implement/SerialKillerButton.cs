using Hazel;
using System;
using System.Collections.Generic;
using System.Text;

namespace MysteryOpertion.Model.Buttons.Implement
{
    public class SerialKillerButton : ButtonBase, TargetedButton
    {
        public Player Target { get; set; }

        public SerialKillerButton(Player player) : base(player)
        {
            sprite = HudManager.Instance.KillButton.graphic.sprite;
            text = ButtonTextDictionary.KillButtonText;
            CooldownTime = 20f;
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
            PlayerControl source = player.PlayerControl;
            PlayerControl target = Target.PlayerControl;

            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)RPCFuncType.CustomMurderPlayer, SendOption.Reliable);
            writer.Write(source.PlayerId);
            writer.Write(target.PlayerId);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            RPCFunctions.CustomMurderPlayer(source.PlayerId, target.PlayerId);

            Timer = CooldownTime;
            player.CalcSanityPoint(20);
        }

        public override void OnMeetingEnd()
        {
            Timer = CooldownTime;
        }

        public void UpdateTarget()
        {
            Target = ToolBox.GetTarget();
            ToolBox.SetPlayerOutline(Target?.PlayerControl, RoleInfoDictionary.SerialKiller.Color);
        }
    }
}
