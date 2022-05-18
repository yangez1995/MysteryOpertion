using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MysteryOpertion.Model.Roles.CrewmateRoles
{
    public class CurseMage : Crewmate
    {
        public int CurseTimes { get; set; }

        public CurseMage(Player player) : base(player)
        {
            this.info = RoleInfoDictionary.CurseMage;
            this.maxSanityPoint = 50;
            this.initialSanityPoint = 50;

            this.CurseTimes = 2;
        }
    }
}
