using System;
using System.Collections.Generic;
using System.Text;

namespace AgeOfMagic.Roles.CrewmateRoles
{
    public class Crewmate : RoleBase
    {
        public Crewmate() : base() { }

        public override string GetRoleName()
        {
            return RoleNameDictionary.Crewmate;
        }
    }
}
