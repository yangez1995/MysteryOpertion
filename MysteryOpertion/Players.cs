using MysteryOpertion.Model;
using System.Collections.Generic;

namespace MysteryOpertion
{
    public static class Players
    {
        public static List<Player> playerList = new List<Player>();

        public static void InitPlayerList()
        {
            playerList.Clear();
            foreach (PlayerControl playerControl in PlayerControl.AllPlayerControls)
            {
                var player = new Player(playerControl);
                playerList.Add(player);
            }
        }

        public static Player GetLocalPlayer()
        {
            return GetPlayer(PlayerControl.LocalPlayer);
        }

        public static Player GetPlayer(PlayerControl playerControl)
        {
            return GetPlayer(playerControl.PlayerId);
        }

        public static Player GetPlayer(byte playerId)
        {
            foreach (var player in Players.playerList)
            {
                if (player.playerControl.PlayerId == playerId)
                    return player;
            }
            return null;
        }
    }
}
