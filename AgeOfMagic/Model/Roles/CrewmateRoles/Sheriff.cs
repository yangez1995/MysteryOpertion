using MysteryOpertion.Model.Buttons;
using MysteryOpertion.Model.Buttons.Implement;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MysteryOpertion.Model.Roles.CrewmateRoles
{
    public class Sheriff : Crewmate
    {
        public SheriffKillButton sheriffKillButton;

        public Sheriff(Player player) : base(player) 
        {
            this.roleName = RoleNameDictionary.Sheriff;
            this.roleColor = Color.yellow;
            this.roleBlurb = RoleBlurbDictionary.SheriffBlurb;
            this.maxSanityPoint = 75;
            this.initialSanityPoint = 75;

            this.sheriffKillButton = (SheriffKillButton)ButtonFactory.Produce(ButtonType.SheriffKill, player);
            this.buttonDict.Add(ButtonType.SheriffKill, this.sheriffKillButton);
        }
    }
}
