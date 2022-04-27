using AgeOfMagic.Entities.Roles.CrewmateRoles;
using AgeOfMagic.Entities.Roles.ImpostorRoles;
using System;
using System.Collections.Generic;
using System.Text;

namespace AgeOfMagic.Entities.Roles
{
    public static class RoleFactory {
        public static Role Produce(RoleType roleType)
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
