using AgeOfMagic.Entities.Roles.CrewmateRoles;
using AgeOfMagic.Entities.Roles.ImpostorRoles;
using System;
using System.Collections.Generic;
using System.Text;

namespace AgeOfMagic.Entities.Buttons.Implement
{
    public class SheriffKillButton : ButtonBase
    {
        public SheriffKillButton(HudManager hudManager) : base(hudManager) {}

        public override bool IsAvailable()
        {
            if(Players.localPlayer.mainRole is not Sheriff)
                return false;

            var role = (Sheriff)Players.localPlayer.mainRole;
            return role.SkillTarget is not null && PlayerControl.LocalPlayer.CanMove;
        }

        public override bool IsShow()
        {
            return Players.localPlayer is not null && Players.localPlayer.mainRole is Sheriff && !Players.localPlayer.playerControl.Data.IsDead;
        }

        public override void OnClick()
        {
            if (Players.localPlayer.mainRole is not Sheriff)
                return;

            var role = (Sheriff)Players.localPlayer.mainRole;

            PlayerControl source = PlayerControl.LocalPlayer;
            PlayerControl target;
            if(role.SkillTarget.mainRole is Impostor)
            {
                target = role.SkillTarget.playerControl;
            }
            else
            {
                target = PlayerControl.LocalPlayer;
            }

            source.MurderPlayer(target);
        }

        public override void OnMeetingEnd()
        {
            timer = cooldownTime;
        }
    }
}
