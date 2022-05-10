using System;
using System.Collections.Generic;
using System.Text;

namespace MysteryOpertion.Model.Roles
{
    public class RoleConfiguration
    {
        public int CrewmatePlayerCount { get; set; }
        public int ChaosPlayerCount { get; set; }
        public int ImpostorPlayerCount { get; set; }
        public int LurkerPlayerCount { get; set; }

        public RoleConfiguration(int playerCount)
        {
            switch (playerCount)
            {
                case 4:
                    this.CrewmatePlayerCount = 3;
                    this.ChaosPlayerCount = 0;
                    this.ImpostorPlayerCount = 1;
                    this.LurkerPlayerCount = 0;
                    break;
                case 5:
                    this.CrewmatePlayerCount = 3;
                    this.ChaosPlayerCount = 1;
                    this.ImpostorPlayerCount = 1;
                    this.LurkerPlayerCount = 0;
                    break;
                case 6:
                    this.CrewmatePlayerCount = 3;
                    this.ChaosPlayerCount = 2;
                    this.ImpostorPlayerCount = 1;
                    this.LurkerPlayerCount = 0;
                    break;
                case 7:
                    this.CrewmatePlayerCount = 4;
                    this.ChaosPlayerCount = 1;
                    this.ImpostorPlayerCount = 1;
                    this.LurkerPlayerCount = 1;
                    break;
                case 8:
                    this.CrewmatePlayerCount = 4;
                    this.ChaosPlayerCount = 2;
                    this.ImpostorPlayerCount = 1;
                    this.LurkerPlayerCount = 1;
                    break;
                case 9:
                    this.CrewmatePlayerCount = 5;
                    this.ChaosPlayerCount = 2;
                    this.ImpostorPlayerCount = 2;
                    this.LurkerPlayerCount = 0;
                    break;
                case 10:
                    this.CrewmatePlayerCount = 5;
                    this.ChaosPlayerCount = 3;
                    this.ImpostorPlayerCount = 2;
                    this.LurkerPlayerCount = 0;
                    break;
                case 11:
                    this.CrewmatePlayerCount = 6;
                    this.ChaosPlayerCount = 2;
                    this.ImpostorPlayerCount = 2;
                    this.LurkerPlayerCount = 1;
                    break;
                case 12:
                    this.CrewmatePlayerCount = 6;
                    this.ChaosPlayerCount = 3;
                    this.ImpostorPlayerCount = 2;
                    this.LurkerPlayerCount = 1;
                    break;
                case 13:
                    this.CrewmatePlayerCount = 7;
                    this.ChaosPlayerCount = 3;
                    this.ImpostorPlayerCount = 3;
                    this.LurkerPlayerCount = 0;
                    break;
                case 14:
                    this.CrewmatePlayerCount = 7;
                    this.ChaosPlayerCount = 4;
                    this.ImpostorPlayerCount = 3;
                    this.LurkerPlayerCount = 0;
                    break;
                case 15:
                    this.CrewmatePlayerCount = 8;
                    this.ChaosPlayerCount = 3;
                    this.ImpostorPlayerCount = 3;
                    this.LurkerPlayerCount = 1;
                    break;
                default:
                    break;
            }
        }
    }
}
