using MysteryOpertion.Model.Buttons;
using MysteryOpertion.Model.Buttons.Implement;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MysteryOpertion.Model.Roles.CrewmateRoles
{
    public class Traveller : Crewmate
    {
        public TravelButton travelButton;
        public TravelPlayerButton travelPlayerButton;

        public Traveller(Player player) : base(player)
        {
            this.roleName = RoleNameDictionary.Traveller;
            this.roleColor = Color.cyan;
            this.roleBlurb = RoleBlurbDictionary.TravellerBlurb;
            this.maxSanityPoint = 50;
            this.initialSanityPoint = 50;

            this.travelButton = (TravelButton)ButtonFactory.Produce(ButtonType.Travel, player);
            this.buttonDict.Add(ButtonType.Travel, this.travelButton);

            this.travelPlayerButton = (TravelPlayerButton)ButtonFactory.Produce(ButtonType.TravelPlayer, player);
            this.buttonDict.Add(ButtonType.TravelPlayer, this.travelPlayerButton);
        }
    }
}
