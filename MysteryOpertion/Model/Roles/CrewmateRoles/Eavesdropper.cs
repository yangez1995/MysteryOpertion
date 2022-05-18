using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MysteryOpertion.Model.Roles.CrewmateRoles
{
    public class Eavesdropper : Crewmate
    {
        public Eavesdropper(Player player) : base(player)
        {
            this.info = RoleInfoDictionary.Eavesdropper;
            this.maxSanityPoint = 50;
            this.initialSanityPoint = 50;
        }
    }
}
