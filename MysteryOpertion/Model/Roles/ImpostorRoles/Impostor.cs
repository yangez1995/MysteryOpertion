using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MysteryOpertion.Model.Roles.ImpostorRoles
{
    public class Impostor : RoleBase
    {
        public Impostor(Player player) : base(player) 
        {
            this.info = RoleInfoDictionary.Impostor;
            this.maxSanityPoint = 75;
            this.initialSanityPoint = 75;
        }
    }
}
