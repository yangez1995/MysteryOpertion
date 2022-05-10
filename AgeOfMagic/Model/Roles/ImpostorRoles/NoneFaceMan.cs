using MysteryOpertion.Model.Buttons;
using MysteryOpertion.Model.Buttons.Implement;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MysteryOpertion.Model.Roles.ImpostorRoles
{
    public class NoneFaceMan : Impostor
    {
        public NoneFaceManSamplingButton samplingButton;
        public NoneFaceManMorphButton morphButton;
        public NoneFaceManRelieveButton relieveButton;

        public NoneFaceMan(Player player) : base(player)
        {
            this.roleName = RoleNameDictionary.NoneFaceMan;
            this.roleColor = Color.red;
            this.roleBlurb = RoleBlurbDictionary.NoneFaceManBlurb;
            this.maxSanityPoint = 50;
            this.initialSanityPoint = 50;

            this.samplingButton = (NoneFaceManSamplingButton)ButtonFactory.Produce(ButtonType.NoneFaceManSamplingButton, player);
            this.buttonDict.Add(ButtonType.NoneFaceManSamplingButton, this.samplingButton);

            this.morphButton = (NoneFaceManMorphButton)ButtonFactory.Produce(ButtonType.NoneFaceManMorphButton, player);
            this.buttonDict.Add(ButtonType.NoneFaceManMorphButton, this.morphButton);

            this.relieveButton = (NoneFaceManRelieveButton)ButtonFactory.Produce(ButtonType.NoneFaceManRelieveButton, player);
            this.buttonDict.Add(ButtonType.NoneFaceManRelieveButton, this.relieveButton);
        }
    }
}
