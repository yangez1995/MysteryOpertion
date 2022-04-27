using AgeOfMagic.Entities.Roles;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AgeOfMagic.Entities
{
    public class Player
    {
        public PlayerControl playerControl { get; }
        public Role mainRole { get; set; }
        public List<Role> subRoles { get; set; }

        public Player(PlayerControl playerControl)
        {
            this.playerControl = playerControl;
            this.subRoles = new List<Role>();
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
    }
}
