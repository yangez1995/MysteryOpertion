using AgeOfMagic.Roles.CrewmateRoles;
using AgeOfMagic.Roles.ImpostorRoles;
using System;
using System.Collections.Generic;
using System.Text;

namespace AgeOfMagic.Roles
{
    public static class RoleFactory {
        public static Role produce(RoleType roleType)
        {
            switch (roleType)
            {
                case RoleType.Crewmate: return new Crewmate();
                case RoleType.Sheriff: return new Sheriff();
                case RoleType.Impostor: return new Impostor();
                default: return null;
            }
        }
    }
}
