using MysteryOpertion.Model.Buttons;
using MysteryOpertion.Model.Buttons.Implement;
using System;
using System.Collections.Generic;
using System.Text;

namespace MysteryOpertion.Model.Roles.ImpostorRoles
{
    public class BloodyHunter : Impostor
    {
        public BloodyHunterButton BloodyHunterButton { get; set; }

        public BloodyHunter(Player player) : base(player)
        {
            this.info = RoleInfoDictionary.BloodyHunter;
            this.maxSanityPoint = 50;
            this.initialSanityPoint = 50;

            this.BloodyHunterButton = (BloodyHunterButton)ButtonFactory.Produce(ButtonType.BloodyHunterButton, player);
            this.buttonDict.Add(ButtonType.BloodyHunterButton, this.BloodyHunterButton);
        }
    }
}
