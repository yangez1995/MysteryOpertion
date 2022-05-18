using MysteryOpertion.Model.Roles;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MysteryOpertion.Model
{
    public class Player
    {
        public PlayerControl PlayerControl { get; set; }
        public Role MainRole { get; set; }
        public List<Role> SubRoles { get; set; }
        public int MaxSanityPoint { get; set; }
        public int SanityPoint { get; set; }
        public float SanityPointTimer { get; set; }

        public Player(PlayerControl playerControl)
        {
            this.PlayerControl = playerControl;
            this.SubRoles = new List<Role>();
            this.MaxSanityPoint = 100;
            this.SanityPoint = 100;
            this.SanityPointTimer = 2;
        }

        public string GetRoleName()
        {
            StringBuilder builder = new StringBuilder();
            foreach (var subRole in SubRoles)
            {
                builder.Append($"{subRole.GetRoleName()} ");
            }
            builder.Append(MainRole.GetRoleName());

            return builder.ToString();
        }

        public Color GetRoleColor()
        {
            return MainRole.GetRoleColor();
        }

        public void UpdateButtons()
        {
            MainRole.UpdateButtons();
            foreach (var role in SubRoles)
            {
                role.UpdateButtons();
            }
        }

        public void UpdateButtonsOnMeetingEnd()
        {
            MainRole.UpdateButtonsOnMeetingEnd();
            foreach (var role in SubRoles)
            {
                role.UpdateButtonsOnMeetingEnd();
            }
        }

        public void CalcSanityPoint(int num)
        {
            var sum = SanityPoint + num;
            SanityPoint = sum > MaxSanityPoint ? MaxSanityPoint : sum;     
        }
    }
}
