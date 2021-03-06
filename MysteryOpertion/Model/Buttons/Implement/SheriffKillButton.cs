using MysteryOpertion.Model.Roles.CrewmateRoles;
using MysteryOpertion.Model.Roles.ImpostorRoles;
using Hazel;
using System;
using System.Collections.Generic;
using System.Text;
using MysteryOpertion.Model.Roles.ChaosRoles;

namespace MysteryOpertion.Model.Buttons.Implement
{
    public class SheriffKillButton : ButtonBase, TargetedButton
    {
        public int UsageCount { get; set; }
        public bool CanKillChaos { get; set; }
        public Player Target { get; set; }

        public SheriffKillButton(Player player) : base(player)
        {
            sprite = HudManager.Instance.KillButton.graphic.sprite;
            text = ButtonTextDictionary.SheriffKillButtonText;

            UsageCount = ConfigLoader.selecters[ConfigKeyDictionary.SheriffTotalCount].GetInt32Value();
            CanKillChaos = ConfigLoader.selecters[ConfigKeyDictionary.SheriffCanKillChaos].GetBoolValue();
            CooldownTime = ConfigLoader.selecters[ConfigKeyDictionary.SheriffSkillCD].GetFloatValue();
        }

        public override bool IsAvailable()
        {
            return Target is not null && player.PlayerControl.CanMove;
        }

        public override bool IsShow()
        {
            return player?.PlayerControl == PlayerControl.LocalPlayer && !player.PlayerControl.Data.IsDead && UsageCount > 0;
        }

        public override void OnClick()
        {
            PlayerControl source = player.PlayerControl;
            PlayerControl target = Target.MainRole is Impostor || (Target.MainRole is Chaos && CanKillChaos)
                ? Target.PlayerControl : target = player.PlayerControl;

            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)RPCFuncType.CustomMurderPlayer, SendOption.Reliable);
            writer.Write(source.PlayerId);
            writer.Write(target.PlayerId);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            RPCFunctions.CustomMurderPlayer(source.PlayerId, target.PlayerId);

            Timer = CooldownTime;
            UsageCount--;
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
