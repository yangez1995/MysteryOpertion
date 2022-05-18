using MysteryOpertion.Model.Roles.ChaosRoles;
using MysteryOpertion.Model.Roles.CrewmateRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MysteryOpertion.Model.Buttons.Implement
{
    public class DivineButton : ButtonBase
    {
        public DivineButton(Player player) : base(player)
        {
            this.CooldownTime = 15f;
            this.SanityCost = 10;
            this.text = ButtonTextDictionary.DivineButtonText;
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
            int totalTask = player.PlayerControl.Data.Tasks.Count;
            int completedTask = ToolBox.CompletedTaskCount(player.PlayerControl.Data);
            float helf = (float)totalTask / 2;

            //根据完成任务数量决定能够占卜到哪些信息
            string message;
            //if(completedTask < helf)
            if (false)
            {
                bool hasDead = GameNote.DeathRecords.Any(it => it.IsDeadInCurrentRound);

                if(GameNote.NonExistingRoles.Count == 0)
                {
                    message = hasDead ? TextDictionary.DivineHasDead : TextDictionary.DivineNonDead;
                }
                else
                {
                    int num1 = hasDead ? 0 : ToolBox.random.Next(0, 2);
                    if (num1 == 0)
                    {
                        int num2 = ToolBox.random.Next(0, GameNote.NonExistingRoles.Count);
                        message = TextDictionary.DivineNonExistent(GameNote.NonExistingRoles[num2]);
                    }
                    else
                    {
                        message = TextDictionary.DivineHasDead;
                    }
                }
            }
            //else if(completedTask >= helf && completedTask < totalTask)
            else if (true)
            {
                Player randomPlayer;
                do
                {
                    randomPlayer = ToolBox.GetRandomPlayer();
                } 
                while (randomPlayer.PlayerControl.Data.IsDead || randomPlayer.PlayerControl.Data.Role.IsImpostor || randomPlayer == player);
                
                message = TextDictionary.DivineExistent(randomPlayer.MainRole.GetRoleName());
            }
            else
            {
                Player randomPlayer;
                do
                {
                    randomPlayer = ToolBox.GetRandomPlayer();
                }
                while (randomPlayer.PlayerControl.Data.IsDead || randomPlayer.MainRole is Chaos || randomPlayer == player);

                if (randomPlayer.PlayerControl.Data.Role.IsImpostor)
                    message = TextDictionary.DivineExistent(randomPlayer.MainRole.GetRoleName());
                else
                    message = TextDictionary.DivineCrewmate(randomPlayer.PlayerControl.Data.PlayerName);
            }


            //var sourceId = player.playerControl.PlayerId;
            //var targetId = Target.playerControl.PlayerId;
            //MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)RPCFuncType.LightPrayerBless, SendOption.Reliable);
            //writer.Write(sourceId);
            //writer.Write(targetId);
            //AmongUsClient.Instance.FinishRpcImmediately(writer);
            RPCFunctions.ShowCenterMessage(message);

            Timer = CooldownTime;
        }

        public override void OnMeetingEnd()
        {
            Timer = CooldownTime;
        }
    }
}
