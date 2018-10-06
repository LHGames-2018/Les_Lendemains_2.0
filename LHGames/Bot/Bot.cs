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
        static Strategy strategy = new Strategy();
        static HighAction currentAction = null;
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
            //update map of the world
            worldMap.UpdateMap(map.GetVisibleTiles());
            //worldMap.UpdateOtherPLayerMap(gameInfo.OtherPlayers);

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
            // TODO: Implement your AI here.
            if (map.GetTileAt(PlayerInfo.Position.X + _currentDirection, PlayerInfo.Position.Y) == TileContent.Wall)
            {
                _currentDirection *= -1;
            }

            var data = StorageHelper.Read<TestClass>("Test");
            Console.WriteLine(data?.Test);
            return AIHelper.CreateMoveAction(new Point(_currentDirection, 0));
        }

        /// <summary>
        /// Gets called after ExecuteTurn.
        /// </summary>
        internal void AfterTurn()
        {
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
}

public class MiningStrategy : Strategy
{
    public override string GetNextMove(IPlayer player, IEnumerable<IPlayer> visiblePlayers, LegacyMap map)
    {
        // Dropper nos ressoruces si on est colles a la maison
        if (Point.DistanceManhatan(player.HouseLocation, player.Position) == 0)
        {
            return AIHelper.CreateEmptyAction();
        }

        // Verifier si on doit rentrer pour drop nos ressources
        if (player.CarryingCapacity - player.CarriedResources < 100)
        {
            
        }

        // Trouver le filon le plus proche
        Point closestMineralPosition = GetClosestMineralPosition(player, map);

        // Si on est colles au filon, le miner
        if (Point.DistanceManhatan(closestMineralPosition, player.Position) <= 1)
        {
            return AIHelper.CreateCollectAction(new Point(closestMineralPosition.X - player.Position.X,
                closestMineralPosition.Y - player.Position.Y));
        }

        // Si on est pas colles, quon rentre pas, aller vers le filon
       //MultipleActions.MoveThenCollect(player, map, closestMineralPosition);

        return AIHelper.CreateMoveAction(new Point(1, 0));


    }

    public Point GetClosestMineralPosition(IPlayer player, LegacyMap map)
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