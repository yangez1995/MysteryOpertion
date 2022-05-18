using MysteryOpertion.Model.Buttons;
using MysteryOpertion.Model.Buttons.Implement;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MysteryOpertion.Model.Roles.ImpostorRoles
{
    public class SerialKiller : Impostor
    {
        public SerialKillerButton serialKillerButton;

        public SerialKiller(Player player) : base(player)
        {
            this.info = RoleInfoDictionary.SerialKiller;
            this.maxSanityPoint = 50;
            this.initialSanityPoint = 50;
            player.SanityPointTimer = 30;

            this.serialKillerButton = (SerialKillerButton)ButtonFactory.Produce(ButtonType.SerialKillerButton, player);
            this.buttonDict.Add(ButtonType.SerialKillerButton, this.serialKillerButton);
        }
    }
}
