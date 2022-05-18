using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MysteryOpertion.Model.Roles.CrewmateRoles
{
    public class Judge : Crewmate
    {
        public Judge(Player player) : base(player)
        {
            this.info = RoleInfoDictionary.Judge;
            this.maxSanityPoint = 50;
            this.initialSanityPoint = 50;
        }
    }
}
