using MysteryOpertion.Model.Buttons;
using MysteryOpertion.Model.Buttons.Implement;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MysteryOpertion.Model.Roles.CrewmateRoles
{
    public class LightPrayer : Crewmate
    {
        public BlessButton blessButton;

        public LightPrayer(Player player) : base(player)
        {
            this.info = RoleInfoDictionary.LightPrayer;
            this.maxSanityPoint = 50;
            this.initialSanityPoint = 50;

            this.blessButton = (BlessButton)ButtonFactory.Produce(ButtonType.BlessButton, player);
            this.buttonDict.Add(ButtonType.RepairButton, this.blessButton);
        }
    }
}
