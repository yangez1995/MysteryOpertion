using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AgeOfMagic.Entities.Roles.CrewmateRoles
{
    public class Sheriff : Crewmate
    {
        public Player SkillTarget { get; set; }

        public Sheriff() : base() { }

        public override string GetRoleName()
        {
            return RoleNameDictionary.Sheriff;
        }

        public override Color GetRoleColor()
        {
            return Color.yellow;
        }

        public override string GetRoleBlurb()
        {
            return RoleBlurbDictionary.SheriffBlurb;
        }
    }
}
