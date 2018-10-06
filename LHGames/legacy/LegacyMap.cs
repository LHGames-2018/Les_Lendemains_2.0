using StarterProject.Web.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHGames.Helper;
using StarterProject.Web.Api.Controllers;

namespace LHGames
{
    public class LegacyMap
    {
        public Dictionary<Point, KeyValuePair<string, Player>> playersDictionnary = new Dictionary<Point, KeyValuePair<string, Player>>();
        public TileContent[,] tileTypeMap = new TileContent[1024,1024];
        public LegacyMap()
        {
            for (int i = 0; i < tileTypeMap.GetLength(0); i++)
            {
                for (int j = 0; j < tileTypeMap.GetLength(1); j++)
                {
                    tileTypeMap[i, j] = TileContent.Unknown;
                }
            }
        }
        public void UpdateOtherPLayerMap(List<KeyValuePair<string, Player>> OtherPlayers)
        {
            foreach (KeyValuePair<string, Player> playerInfo in OtherPlayers)
            {
                playersDictionnary[playerInfo.Value.Position] = playerInfo;
            }
        }
        public void UpdateMap(IEnumerable<Tile> visibleTiles)
        {
            foreach(Tile t in visibleTiles)
            {
                int x = t.Position.X;
                int y = t.Position.Y;

                bool online = GameController.playerBot.PlayerInfo.Name != "Player 1";
                if (online)
                {
                    if (x < 0) { x += 66; }
                    if (y < 0) { y += 66; }
                }
                else
                {
                    if (x < 0) { x += 132; }
                    if (y < 0) { y += 198; }
                }
                tileTypeMap[x, y] = t.TileType;
            }
        }
    }
}
