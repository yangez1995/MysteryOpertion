using MysteryOpertion.Model.Buttons;
using MysteryOpertion.Model.Buttons.Implement;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MysteryOpertion.Model.Roles.CrewmateRoles
{
    public class MechanicExpert : Crewmate
    {
        public RepairButton repairButton;

        public MechanicExpert(Player player) : base(player)
        {
            this.info = RoleInfoDictionary.MechanicExpert;
            this.maxSanityPoint = 50;
            this.initialSanityPoint = 50;

            this.repairButton = (RepairButton)ButtonFactory.Produce(ButtonType.RepairButton, player);
            this.buttonDict.Add(ButtonType.RepairButton, this.repairButton);
        }
    }
}
