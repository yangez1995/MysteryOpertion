using AgeOfMagic.Roles;
using System;
using System.Collections.Generic;
using System.Text;

namespace AgeOfMagic
{
    public static class Players
    {
        public static List<Player> playerList = new List<Player>();

        public static void initPlayerList(List<PlayerControl> allPlayerControls)
        {
            playerList.Clear();
            foreach (PlayerControl playerControl in allPlayerControls)
            {
                playerList.Add(new Player(playerControl));
            }
        }
    }

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
            foreach(var subRole in subRoles)
            {
                builder.Append($"{subRole.GetRoleName()} ");
            }
            builder.Append(mainRole.GetRoleName());

            return builder.ToString();
        }
    }
}
