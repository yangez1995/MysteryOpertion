﻿using MysteryOpertion.Model.Buttons;
using MysteryOpertion.Model.Buttons.Implement;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MysteryOpertion.Model.Roles.ChaosRoles
{
    public class ArsonExpert : Chaos
    {
        public ArsonExpertOiledButton oiledButton;
        //public ArsonExpertPlaceDollButton placeDollButton;
        public List<byte> oiledPlayerIds;

        public ArsonExpert(Player player) : base(player)
        {
            this.roleName = RoleNameDictionary.ArsonExpert;
            this.roleColor = new Color32(250, 106, 106, byte.MaxValue);
            this.roleBlurb = RoleBlurbDictionary.ArsonExpertBlurb;
            this.maxSanityPoint = 50;
            this.initialSanityPoint = 50;

            this.oiledButton = (ArsonExpertOiledButton)ButtonFactory.Produce(ButtonType.ArsonExpertOiledButton, player);
            this.buttonDict.Add(ButtonType.ArsonExpertOiledButton, this.oiledButton);

            //this.placeDollButton = (ArsonExpertPlaceDollButton)ButtonFactory.Produce(ButtonType.ArsonExpertPlaceDollButton, player);
            //this.buttonDict.Add(ButtonType.ArsonExpertPlaceDollButton, this.placeDollButton);

            oiledPlayerIds = new List<byte>();
        }
    }
}