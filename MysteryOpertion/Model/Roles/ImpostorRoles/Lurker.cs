using System;
using System.Collections.Generic;
using System.Text;

namespace MysteryOpertion.Model.Roles.ImpostorRoles
{
    public class Lurker : Impostor
    {
        public Lurker(Player player) : base(player)
        {
            this.info = RoleInfoDictionary.Lurker;
            this.maxSanityPoint = 50;
            this.initialSanityPoint = 50;
        }
    }
}
