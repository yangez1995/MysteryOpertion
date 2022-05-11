using MysteryOpertion.Model;
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

        public static void ClearGameNote()
        {
            ArsonExpertWin = false;

            DeathRecords = new List<DeathRecord>();
        }
    }
}
