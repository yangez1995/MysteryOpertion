using MysteryOpertion.Model;
using MysteryOpertion.Model.Roles;
using System;
using System.Collections.Generic;
using System.Text;

namespace MysteryOpertion
{
    public static class GameNote
    {
        //获胜方式记录
        public static bool ArsonExpertWin = false;

        //死亡记录
        public static List<DeathRecord> DeathRecords = new List<DeathRecord>();

        //记录存在的和不存在的职业
        public static List<string> ExistingRoles = new List<string>();
        public static List<string> NonExistingRoles = new List<string>();

        //顺序记录任务完成情况
        public static List<TaskRecord> TaskComplete = new List<TaskRecord>();

        public static void ClearGameNote()
        {
            ArsonExpertWin = false;

            DeathRecords = new List<DeathRecord>();
            ExistingRoles = new List<string>();
            NonExistingRoles = new List<string>();
        }
    }
}
