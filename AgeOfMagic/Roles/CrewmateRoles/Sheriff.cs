using System;
using System.Collections.Generic;
using System.Text;

namespace AgeOfMagic.Roles.CrewmateRoles
{
    public class Sheriff : Crewmate
    {
        private const string RoleName = RoleNameDictionary.Sheriff;

        public Sheriff() : base() { }

        public override string GetRoleName()
        {
            return RoleNameDictionary.Sheriff;
        }
    }
}
