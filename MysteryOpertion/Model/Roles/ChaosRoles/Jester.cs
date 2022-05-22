using System;
using System.Collections.Generic;
using System.Text;

namespace MysteryOpertion.Model.Roles.ChaosRoles
{
    public class Jester : Chaos
    {
        public Jester(Player player) : base(player)
        {
            this.info = RoleInfoDictionary.Jester;
            this.maxSanityPoint = 50;
            this.initialSanityPoint = 50;
        }
    }
}
