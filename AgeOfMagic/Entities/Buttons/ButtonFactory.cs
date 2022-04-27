using AgeOfMagic.Entities.Buttons.Implement;
using System;
using System.Collections.Generic;
using System.Text;

namespace AgeOfMagic.Entities.Buttons
{
    public static class ButtonFactory
    {
        public static Button Produce(ButtonType buttonType, HudManager hudManager)
        {
            switch (buttonType)
            {
                case ButtonType.SheriffKill: return new SheriffKillButton(hudManager);
                default: return null;
            }
        }
    }
}
