using System;
using System.Collections.Generic;
using System.Text;

namespace MysteryOpertion.Model
{
    public class DeathRecord
    {
        public Player deadPlayer { get; set; }
        public Player KillerPlayer { get; set; }
        public DateTime deadTime { get; set; }
        public CauseOfDeath Cause { get; set; }
        public int MurderScenePeopleCount { get; set; }
        public bool IsDeadInCurrentRound { get; set; }

        public DeathRecord()
        {
            this.IsDeadInCurrentRound = true;
            this.deadTime = DateTime.UtcNow;
        }
    }

    public enum CauseOfDeath
    {
        CommonImpostorKill,
        Sheriffkill,
        Suicide
    }
}
