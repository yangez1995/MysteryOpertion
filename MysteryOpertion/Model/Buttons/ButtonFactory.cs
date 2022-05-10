using MysteryOpertion.Model.Buttons.Implement;
using System;
using System.Collections.Generic;
using System.Text;

namespace MysteryOpertion.Model.Buttons
{
    public static class ButtonFactory
    {
        public static Button Produce(ButtonType buttonType, Player player)
        {
            switch (buttonType)
            {
                case ButtonType.SheriffKill: return new SheriffKillButton(player);
                case ButtonType.Travel: return new TravelButton(player);
                case ButtonType.TravelPlayer: return new TravelPlayerButton(player);
                case ButtonType.RepairButton: return new RepairButton(player);
                case ButtonType.BlessButton: return new BlessButton(player);
                case ButtonType.NoneFaceManMorphButton: return new NoneFaceManMorphButton(player);
                case ButtonType.NoneFaceManSamplingButton: return new NoneFaceManSamplingButton(player);
                case ButtonType.NoneFaceManRelieveButton: return new NoneFaceManRelieveButton(player);
                case ButtonType.ArsonExpertOiledButton: return new ArsonExpertOiledButton(player);
                case ButtonType.ArsonExpertPlaceDollButton: return new ArsonExpertPlaceDollButton(player);
                default: return null;
            }
        }
    }
}
