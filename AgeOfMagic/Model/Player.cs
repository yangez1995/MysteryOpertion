using MysteryOpertion.Model.Roles;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MysteryOpertion.Model
{
    public class Player
    {
        public PlayerControl playerControl;
        public Role mainRole;
        public List<Role> subRoles;
        public int maxSanityPoint;
        public int sanityPoint;

        public Player(PlayerControl playerControl)
        {
            this.playerControl = playerControl;
            this.subRoles = new List<Role>();
            this.maxSanityPoint = 100;
            this.sanityPoint = 100;
        }

        public string GetRoleName()
        {
            StringBuilder builder = new StringBuilder();
            foreach (var subRole in subRoles)
            {
                builder.Append($"{subRole.GetRoleName()} ");
            }
            builder.Append(mainRole.GetRoleName());

            return builder.ToString();
        }

        public Color GetRoleColor()
        {
            return mainRole.GetRoleColor();
        }

        public void UpdateButtons()
        {
            mainRole.UpdateButtons();
            foreach (var role in subRoles)
            {
                role.UpdateButtons();
            }
        }

        public void UpdateButtonsOnMeetingEnd()
        {
            mainRole.UpdateButtonsOnMeetingEnd();
            foreach (var role in subRoles)
            {
                role.UpdateButtonsOnMeetingEnd();
            }
        }

        public void CalcSanityPoint(int num)
        {
            var sum = sanityPoint + num;
            sanityPoint = sum > maxSanityPoint ? maxSanityPoint: sum;     
        }
    }
}
