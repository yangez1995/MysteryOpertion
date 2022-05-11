using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MysteryOpertion.Model.Roles.CrewmateRoles
{
    public class Coroner : Crewmate
    {
        public Coroner(Player player) : base(player)
        {
            this.roleName = RoleNameDictionary.Coroner;
            this.roleColor = new Color32(45, 106, 165, byte.MaxValue);
            this.roleBlurb = RoleBlurbDictionary.CoronerBlurb;
            this.maxSanityPoint = 50;
            this.initialSanityPoint = 50;
        }
    }
}
