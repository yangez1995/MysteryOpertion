using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MysteryOpertion.Model.Roles.CrewmateRoles
{
    public class Crewmate : RoleBase
    {
        public Crewmate(Player player) : base(player) 
        { 
            this.roleName = RoleNameDictionary.Crewmate;
            this.roleColor = Color.white;
            this.roleBlurb = RoleBlurbDictionary.CrewmateBlurb;
            this.maxSanityPoint = 50;
            this.initialSanityPoint = 50;
        }
    }
}
