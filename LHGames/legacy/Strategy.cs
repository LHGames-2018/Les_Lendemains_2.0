
using LHGames.Actions;
using LHGames.Nodes;
using StarterProject.Web.Api;
using System;
using System.Collections.Generic;
using LHGames.Helper;
namespace LHGames
{
    class Strategy
    {
        public HighAction NextAction(LegacyMap map, IPlayer player)
        {
            if(player.CarriedResources < player.CarryingCapacity)
            {
                return collectAction(map, player);
            }
            else
            {
                return returnHomeAction(map, player);
            }
        }

        private HighAction returnHomeAction(LegacyMap map, IPlayer player)
        {
            return new Move(player, map, player.HouseLocation);
        }

        private HighAction DefendHome(LegacyMap map, GameInfo gameInfo)
        {
            return null;
        }

        private HighAction DefendSelf(LegacyMap map, IPlayer player)
        {
            return null;
        }

        private HighAction KillPlayer(LegacyMap map, GameInfo gameInfo)
        {
            return null;
        }

        private HighAction collectAction(LegacyMap map, IPlayer player)
        {
            Point playerPosition = player.Position;
            for (int edge = 1; edge < map.tileTypeMap.GetLength(0); edge++)
            {
                for (int i = playerPosition.X - edge; i <= playerPosition.X + edge && i >= 0 && i < map.tileTypeMap.GetLength(0); i++)
                {
                    for (int j = playerPosition.Y - edge; j <= playerPosition.Y + edge && j >= 0 && j < map.tileTypeMap.GetLength(1); j++)
                    {
                        if (map.tileTypeMap[i, j] == TileContent.Resource)
                        {
                            Point target = new Point(i, j);
                            if (Point.DistanceManhatan(target, playerPosition) <= 1)
                            {
                                return new Collect(target);
                            }
                            else
                            {
                                return MultipleActions.MoveThenCollect(player, map, target);
                            }
                        }
                    }
                }
            }
            return returnHomeAction(map,player);
        }
        private HighAction exploreAction(LegacyMap map, IPlayer gameInfo)
        {
            return null;
        }

    }
}