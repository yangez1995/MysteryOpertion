using MysteryOpertion.Model.Roles.CrewmateRoles;
using MysteryOpertion.Model.Roles.ImpostorRoles;
using Hazel;
using System;
using System.Collections.Generic;
using System.Text;

namespace MysteryOpertion.Model.Buttons.Implement
{
    public class SheriffKillButton : ButtonBase, TargetedButton
    {
        public Player Target { get; set; }

        public SheriffKillButton(Player player) : base(player)
        {
            sprite = HudManager.Instance.KillButton.graphic.sprite;
            text = ButtonTextDictionary.SheriffKillButtonText;
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
            PlayerControl target;
            if (Target.MainRole is Impostor)
            {
                target = Target.PlayerControl;
            }
            else
            {
                target = player.PlayerControl;
            }

            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)RPCFuncType.CustomMurderPlayer, SendOption.Reliable);
            writer.Write(source.PlayerId);
            writer.Write(target.PlayerId);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            RPCFunctions.CustomMurderPlayer(source.PlayerId, target.PlayerId);

            Timer = CooldownTime;
        }

        public override void OnMeetingEnd()
        {
            Timer = CooldownTime;
        }

        public void UpdateTarget()
        {
            Target = ToolBox.GetTarget();
            ToolBox.SetPlayerOutline(Target?.PlayerControl, RoleInfoDictionary.Sheriff.Color);
        }
    }
}
