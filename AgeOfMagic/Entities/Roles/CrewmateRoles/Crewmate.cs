using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AgeOfMagic.Entities.Roles.CrewmateRoles
{
    public class Crewmate : RoleBase
    {
        public Crewmate() : base() { }

        public override string GetRoleName()
        {
            return RoleNameDictionary.Crewmate;
        }

        public override Color GetRoleColor()
        {
            return Color.white;
        }

        public override string GetRoleBlurb()
        {
            return RoleBlurbDictionary.CrewmateBlurb;
        }
    }
}
