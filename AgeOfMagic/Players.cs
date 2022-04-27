using AgeOfMagic.Entities;
using System.Collections.Generic;

namespace AgeOfMagic
{
    public static class Players
    {
        public static List<Player> playerList = new List<Player>();
        public static Player localPlayer = null;

        public static void initPlayerList(List<PlayerControl> allPlayerControls)
        {
            playerList.Clear();
            foreach (PlayerControl playerControl in allPlayerControls)
            {
                var player = new Player(playerControl);
                playerList.Add(player);

                if(playerControl == PlayerControl.LocalPlayer)
                    localPlayer = player;
            }
        }
    }
}
