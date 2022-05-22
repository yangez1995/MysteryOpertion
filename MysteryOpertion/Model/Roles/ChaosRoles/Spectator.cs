using System;
using System.Collections.Generic;
using System.Text;

namespace MysteryOpertion.Model.Roles.ChaosRoles
{
    public class Spectator : Chaos
    {
        public Spectator(Player player) : base(player)
        {
            this.info = RoleInfoDictionary.Spectator;
            this.maxSanityPoint = 50;
            this.initialSanityPoint = 50;
        }
    }
}
