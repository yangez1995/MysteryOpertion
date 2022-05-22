using MysteryOpertion.Model.Roles;
using HarmonyLib;
using Hazel;
using System;
using System.Collections.Generic;
using System.Text;

namespace MysteryOpertion.Patches
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.HandleRpc))]
    class RPCHandlerPatch
    {
        static void Postfix([HarmonyArgument(0)] byte callId, [HarmonyArgument(1)] MessageReader reader)
        {
            byte playerId;
            byte sourceId;
            byte targetId;
            switch (callId)
            {
                case (byte)RPCFuncType.InitPlayerList:
                    RPCFunctions.InitPlayerList();
                    break;
                case (byte)RPCFuncType.AssignRoleToPlayer:
                    playerId = reader.ReadByte();
                    RoleType roleType = (RoleType)reader.ReadByte();
                    RPCFunctions.AssignRoleToPlayer(playerId, roleType);
                    break;
                case (byte)RPCFuncType.CustomMurderPlayer:
                    sourceId = reader.ReadByte();
                    targetId = reader.ReadByte();
                    RPCFunctions.CustomMurderPlayer(sourceId, targetId);
                    break;
                case (byte)RPCFuncType.CalcSanityPoint:
                    playerId = reader.ReadByte();
                    int value = reader.ReadInt32();
                    RPCFunctions.CalcSanityPoint(playerId, value);
                    break;
                case (byte)RPCFuncType.ReSetTasks:
                    playerId = reader.ReadByte();
                    var taskTypeIds = reader.ReadBytesAndSize();
                    RPCFunctions.ReSetTasks(playerId, taskTypeIds);
                    break;
                case (byte)RPCFuncType.ShowCenterMessage:
                    var message = reader.ReadString();
                    RPCFunctions.ShowCenterMessage(message);
                    break;
                case (byte)RPCFuncType.UseRepairButtonFixLights:
                    RPCFunctions.UseRepairButtonFixLights();
                    break;
                case (byte)RPCFuncType.LightPrayerBless:
                    sourceId = reader.ReadByte();
                    targetId = reader.ReadByte();
                    RPCFunctions.LightPrayerBless(sourceId, targetId);
                    break;
                case (byte)RPCFuncType.Morph:
                    sourceId = reader.ReadByte();
                    targetId = reader.ReadByte();
                    RPCFunctions.Morph(sourceId, targetId);
                    break;
                case (byte)RPCFuncType.CurseKill:
                    sourceId = reader.ReadByte();
                    targetId = reader.ReadByte();
                    RPCFunctions.CurseKill(sourceId, targetId);
                    break;
                default:
                    break;
            }
        }
    }
}
