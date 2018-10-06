using System;
using System.Collections.Generic;
using LHGames;
using LHGames.Actions;
using LHGames.Helper;

namespace LHGames.Bot
{
    internal class Bot
    {
        //legacy variable
        //static Strategy strategy = new Strategy();
        //static HighAction currentAction = null;
        public LegacyMap worldMap = new LegacyMap();
        // already there variable
        internal IPlayer PlayerInfo { get; set; }
        private int _currentDirection = 1;

        internal Bot() { }

        /// <summary>
        /// Gets called before ExecuteTurn. This is where you get your bot's state.
        /// </summary>
        /// <param name="playerInfo">Your bot's current state.</param>
        internal void BeforeTurn(IPlayer playerInfo)
        {
            PlayerInfo = playerInfo;
        }

        /// <summary>
        /// Implement your bot here.
        /// </summary>
        /// <param name="map">The gamemap.</param>
        /// <param name="visiblePlayers">Players that are visible to your bot.</param>
        /// <returns>The action you wish to execute.</returns>
        internal string ExecuteTurn(Map map, IEnumerable<IPlayer> visiblePlayers)
        {
            Console.WriteLine("My pos = " + PlayerInfo.Position.ToString());
            Console.WriteLine("House  = " + PlayerInfo.HouseLocation.ToString());
            Console.WriteLine("Total  = " + PlayerInfo.TotalResources.ToString());
            Console.WriteLine("Collect= " + PlayerInfo.CollectingSpeed.ToString());
            Console.WriteLine("Carryin= " + PlayerInfo.CarryingCapacity.ToString());
            Console.WriteLine("C1     = " + (PlayerInfo.Position == PlayerInfo.HouseLocation &&
                PlayerInfo.TotalResources >= 10000 &&
                PlayerInfo.CollectingSpeed == 1).ToString());
            Console.WriteLine("C2     = " + (PlayerInfo.Position == PlayerInfo.HouseLocation &&
                PlayerInfo.TotalResources >= 10000 &&
                PlayerInfo.CarryingCapacity == 1000).ToString());

            //update map of the world
            worldMap.UpdateMap(map.GetVisibleTiles());
            //worldMap.UpdateOtherPLayerMap(gameInfo.OtherPlayers);
            StrategyManager.PickStrategy();
            return StrategyManager.currentStrategy.GetNextMove(PlayerInfo, visiblePlayers, worldMap);
            /*string action = null;

            Console.WriteLine("Collect= " + PlayerInfo.CollectingSpeed.ToString());
            Console.WriteLine("Carryin= " + PlayerInfo.CarryingCapacity.ToString());

            if (PlayerInfo.Position == PlayerInfo.HouseLocation &&
                PlayerInfo.TotalResources >= 10000 &&
                PlayerInfo.CollectingSpeed == 1)
            {
                Console.WriteLine("Buying a collecting speed");
                return AIHelper.CreateUpgradeAction(UpgradeType.CollectingSpeed);
            }
            if (PlayerInfo.Position == PlayerInfo.HouseLocation &&
                PlayerInfo.TotalResources >= 10000 &&
                PlayerInfo.CarryingCapacity == 1000)
            {
                Console.WriteLine("Buying a Carrying Capacity");
                return AIHelper.CreateUpgradeAction(UpgradeType.CarryingCapacity);
            }


            string action = null;
            while (action == null)
            {
                if (currentAction == null)
                {
                    currentAction = strategy.NextAction(worldMap, PlayerInfo);
                    if (currentAction == null)
                    {
                        break;
                    }
                    //log(currentAction.ToString());
                }
                action = currentAction.NextAction(worldMap, PlayerInfo);
                if (action == null)
                {
                    currentAction = null;
                }
            }
          
            return action;*/
            
        }

        /// <summary>
        /// Gets called after ExecuteTurn.
        /// </summary>
        internal void AfterTurn()
        {
        }
        
        Point GetStorePosition(Point housePosition)
        {
            Point storePosition = new Point(housePosition.X + 27, housePosition.Y);
            
            return storePosition;
        }

        internal void Scout(IEnumerable<IPlayer> visiblePlayers)
        {
            foreach (var visiblePlayer in visiblePlayers)
            {
                if (visiblePlayer.Position.X - PlayerInfo.Position.X < 2 &&
                    visiblePlayer.Position.Y - PlayerInfo.Position.Y < 2)
                {

                }
            }
        }
    }

    
   


}



public class Strategy
{

    public virtual string GetNextMove(IPlayer player, IEnumerable<IPlayer> visiblePlayers, LegacyMap map)
    {
        return "";
    }

    public Point GetFreeAdjacentPosition(Point position, LegacyMap map)
    {
        Point adjacentPoint;
        if (map.tileTypeMap[position.X+1, position.Y] == TileContent.Empty)
            return new Point(position.X + 1, position.Y);
        else if (map.tileTypeMap[position.X - 1, position.Y] == TileContent.Empty)
            return new Point(position.X - 1, position.Y);
        else if (map.tileTypeMap[position.X, position.Y + 1] == TileContent.Empty)
            return new Point(position.X, position.Y + 1);
        else if (map.tileTypeMap[position.X, position.Y - 1] == TileContent.Empty)
            return new Point(position.X, position.Y - 1);
        return null;
    }

}

public class MiningStrategy : Strategy
{
    public override string GetNextMove(IPlayer player, IEnumerable<IPlayer> visiblePlayers, LegacyMap map)
    {
        // Dropper nos ressoruces si on est colles a la maison
        if (Point.DistanceManhatan(player.HouseLocation, player.Position) == 0 && player.CarriedResources > 0)
        {
            return AIHelper.CreateEmptyAction();
        }

        // Verifier si on doit rentrer pour drop nos ressources
        if (player.CarryingCapacity - player.CarriedResources < 100)
        {
            Move moveTowardsHome = new Move(player, map, player.HouseLocation);
            return moveTowardsHome.NextAction(map, player);
        }

        // Trouver le filon le plus proche
        Point closestMineralPosition = GetClosestMineralPosition(player, map);
        Point closestMineralAdjacentPosition = GetFreeAdjacentPosition(closestMineralPosition, map);

        // Si on est colles au filon, le miner
        if (Point.DistanceManhatan(closestMineralPosition, player.Position) <= 1)
        {
            return AIHelper.CreateCollectAction(new Point(closestMineralPosition.X - player.Position.X,
                closestMineralPosition.Y - player.Position.Y));
        }

        // Si on est pas colles, quon rentre pas, aller vers le filon
        Move moveTowardsMineral = new Move(player, map, closestMineralAdjacentPosition);
        return moveTowardsMineral.NextAction(map, player);
    }

    public Point GetClosestMineralPosition(IPlayer player, LegacyMap map)
    {
        Point houseLocaltion = player.HouseLocation;
        for (int edge = 1; edge < map.tileTypeMap.GetLength(0); edge++)
        {
            for (int i = houseLocaltion.X - edge; i <= houseLocaltion.X + edge && i >= 0 && i < map.tileTypeMap.GetLength(0); i++)
            {
                for (int j = houseLocaltion.Y - edge; j <= houseLocaltion.Y + edge && j >= 0 && j < map.tileTypeMap.GetLength(1); j++)
                {
                    if (map.tileTypeMap[i, j] == TileContent.Resource)
                    {
                        return new Point(i, j);
                        
                    }
                }
            }
        }

        return player.Position;
    }

}
public class QuickAttackStrategy : Strategy
{
    public override string GetNextMove(IPlayer player, IEnumerable<IPlayer> visiblePlayers, LegacyMap map)
    {
        throw new NotImplementedException();
    }
}

public class upgradeStrategy : Strategy
{
    public override string GetNextMove(IPlayer player, IEnumerable<IPlayer> visiblePlayers, LegacyMap map)
    {
        throw new NotImplementedException();
    }
}

public class buyItemStrategy : Strategy
{
    public override string GetNextMove(IPlayer player, IEnumerable<IPlayer> visiblePlayers, LegacyMap map)
    {
        throw new NotImplementedException();
    }
}
public class defendHomeStrategy : Strategy
{
    public override string GetNextMove(IPlayer player, IEnumerable<IPlayer> visiblePlayers, LegacyMap map)
    {
        throw new NotImplementedException();
    }
}




public static class StrategyManager
{
    public static Strategy currentStrategy { get; set; }

    public static void PickStrategy()
    {
        currentStrategy = new MiningStrategy();


    }




}


class TestClass
{
    public string Test { get; set; }
}