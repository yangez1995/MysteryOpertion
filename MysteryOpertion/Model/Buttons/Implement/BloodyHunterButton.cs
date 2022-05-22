using Hazel;
using System;
using System.Collections.Generic;
using System.Text;

namespace MysteryOpertion.Model.Buttons.Implement
{
    public class BloodyHunterButton : ButtonBase, TargetedButton
    {
        public Player Target { get; set; }
        public int Level { get; set; }
        public bool CurseFlag { get; set; }

        public BloodyHunterButton(Player player) : base(player)
        {
            this.sprite = HudManager.Instance.KillButton.graphic.sprite;
            this.text = ButtonTextDictionary.KillButtonText;
            this.CooldownTime = 30f;
            this.Level = 0;
            this.CurseFlag = false;
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

            Level++;
            if (Level == 2) CurseFlag = true;
            
            CooldownTime -= 5;
            player.MaxSanityPoint += 10; 
            player.CalcSanityPoint(10);
            
            Timer = CooldownTime;
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
