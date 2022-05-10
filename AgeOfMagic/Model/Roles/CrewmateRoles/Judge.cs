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
            this.roleName = RoleNameDictionary.Judge;
            this.roleColor = new Color32(32, 77, 66, byte.MaxValue);
            this.roleBlurb = RoleBlurbDictionary.JudgeBlurb;
            this.maxSanityPoint = 50;
            this.initialSanityPoint = 50;
        }
    }
}
