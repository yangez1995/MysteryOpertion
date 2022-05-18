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
            this.info = RoleInfoDictionary.DoomBait;
            this.maxSanityPoint = 30;
            this.initialSanityPoint = 30;
        }
    }
}
