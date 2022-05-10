using MysteryOpertion.Model.Roles.ChaosRoles;
using MysteryOpertion.Model.Roles.CrewmateRoles;
using MysteryOpertion.Model.Roles.ImpostorRoles;
using System;
using System.Collections.Generic;
using System.Text;

namespace MysteryOpertion.Model.Roles
{
    public static class RoleFactory {
        public static Role Produce(RoleType roleType, Player player)
        {
            switch (roleType)
            {
                case RoleType.Crewmate: return new Crewmate(player);
                case RoleType.Sheriff: return new Sheriff(player);
                case RoleType.Traveller: return new Traveller(player);
                case RoleType.MechanicExpert: return new MechanicExpert(player);
                case RoleType.LightPrayer: return new LightPrayer(player);
                case RoleType.Judge: return new Judge(player);
                case RoleType.DoomBait: return new DoomBait(player);

                case RoleType.ArsonExpert: return new ArsonExpert(player);

                case RoleType.Impostor: return new Impostor(player);
                case RoleType.NoneFaceMan: return new NoneFaceMan(player);
                default: return null;
            }
        }
    }
}
