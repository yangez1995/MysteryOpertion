using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MysteryOpertion.Model.Roles.CrewmateRoles
{
    public class DoomBait : Crewmate
    {
        public DoomBait(Player player) : base(player)
        {
            this.roleName = RoleNameDictionary.DoomBait;
            this.roleColor = new Color32(205, 133, 0, byte.MaxValue);
            this.roleBlurb = RoleBlurbDictionary.DoomBaitBlurb;
            this.maxSanityPoint = 30;
            this.initialSanityPoint = 30;
        }
    }
}
