using MysteryOpertion.Model.Buttons;
using MysteryOpertion.Model.Buttons.Implement;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MysteryOpertion.Model.Roles.CrewmateRoles
{
    public class Diviner : Crewmate
    {
        public DivineButton divineButton;

        public Diviner(Player player) : base(player)
        {
            this.info = RoleInfoDictionary.Diviner;
            this.maxSanityPoint = 50;
            this.initialSanityPoint = 50;

            this.divineButton = (DivineButton)ButtonFactory.Produce(ButtonType.DivineButton, player);
            this.buttonDict.Add(ButtonType.RepairButton, this.divineButton);
        }
    }
}
