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
            this.info = RoleInfoDictionary.Coroner;
            this.maxSanityPoint = 50;
            this.initialSanityPoint = 50;
        }
    }
}
