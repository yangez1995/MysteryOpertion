using Hazel;
using System;
using System.Collections.Generic;
using System.Text;

namespace MysteryOpertion.Model.Buttons.Implement
{
    public class RepairButton : ButtonBase
    {
        public RepairButton(Player player) : base(player)
        {
            this.cooldownTime = 0f;
            this.sanityCost = 20;
            this.text = ButtonTextDictionary.RepairButtonText;
        }

        public override bool IsAvailable()
        {
            bool SpecialTaskActive = false;
            foreach(var task in PlayerControl.LocalPlayer.myTasks)
            {
                if (task.TaskType == TaskTypes.FixLights || task.TaskType == TaskTypes.RestoreOxy || task.TaskType == TaskTypes.ResetReactor || task.TaskType == TaskTypes.ResetSeismic || task.TaskType == TaskTypes.FixComms || task.TaskType == TaskTypes.StopCharles)
                {
                    SpecialTaskActive = true;
                    break;
                }
            }
                
            return player.playerControl.CanMove && SpecialTaskActive;
        }

        public override bool IsShow()
        {
            return player?.playerControl == PlayerControl.LocalPlayer && !player.playerControl.Data.IsDead;
        }

        public override void OnClick()
        {
            foreach (PlayerTask task in PlayerControl.LocalPlayer.myTasks)
            {
                if (task.TaskType == TaskTypes.FixLights)
                {
                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)RPCFuncType.UseRepairButtonFixLights, SendOption.Reliable);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    RPCFunctions.UseRepairButtonFixLights();
                }
                else if (task.TaskType == TaskTypes.RestoreOxy)
                {
                    ShipStatus.Instance.RpcRepairSystem(SystemTypes.LifeSupp, 0 | 64);
                    ShipStatus.Instance.RpcRepairSystem(SystemTypes.LifeSupp, 1 | 64);
                }
                else if (task.TaskType == TaskTypes.ResetReactor)
                {
                    ShipStatus.Instance.RpcRepairSystem(SystemTypes.Reactor, 16);
                }
                else if (task.TaskType == TaskTypes.ResetSeismic)
                {
                    ShipStatus.Instance.RpcRepairSystem(SystemTypes.Laboratory, 16);
                }
                else if (task.TaskType == TaskTypes.FixComms)
                {
                    ShipStatus.Instance.RpcRepairSystem(SystemTypes.Comms, 16 | 0);
                    ShipStatus.Instance.RpcRepairSystem(SystemTypes.Comms, 16 | 1);
                }
                else if (task.TaskType == TaskTypes.StopCharles)
                {
                    ShipStatus.Instance.RpcRepairSystem(SystemTypes.Reactor, 0 | 16);
                    ShipStatus.Instance.RpcRepairSystem(SystemTypes.Reactor, 1 | 16);
                }
            }

            CostSanity();
        }

        public override void OnMeetingEnd()
        {
            timer = cooldownTime;
        }
    }
}
