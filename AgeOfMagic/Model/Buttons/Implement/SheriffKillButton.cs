using MysteryOpertion.Model.Roles.CrewmateRoles;
using MysteryOpertion.Model.Roles.ImpostorRoles;
using Hazel;
using System;
using System.Collections.Generic;
using System.Text;

namespace MysteryOpertion.Model.Buttons.Implement
{
    public class SheriffKillButton : ButtonBase
    {
        public Player Target { get; set; }

        public SheriffKillButton(Player player) : base(player) 
        {
            sprite = HudManager.Instance.KillButton.graphic.sprite;
            text = ButtonTextDictionary.SheriffKillButtonText;
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
            PlayerControl source = player.playerControl;
            PlayerControl target;
            if(Target.mainRole is Impostor)
            {
                target = Target.playerControl;
            }
            else
            {
                target = player.playerControl;
            }

            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)RPCFuncType.CustomMurderPlayer, SendOption.Reliable);
            writer.Write(source.PlayerId);
            writer.Write(target.PlayerId);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            RPCFunctions.CustomMurderPlayer(source.PlayerId, target.PlayerId);

            timer = cooldownTime;
        }

        public override void OnMeetingEnd()
        {
            timer = cooldownTime;
        }
    }
}
