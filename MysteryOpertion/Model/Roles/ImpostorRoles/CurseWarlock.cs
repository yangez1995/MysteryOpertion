using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MysteryOpertion.Model.Roles.ImpostorRoles
{
    public class CurseWarlock : Impostor
    {
        public int CurseTimes { get; set; }
        public CurseWarlock(Player player) : base(player)
        {
            this.info = RoleInfoDictionary.CurseWarlock;
            this.maxSanityPoint = 50;
            this.initialSanityPoint = 50;

            this.CurseTimes = 2;
        }
    }
}
