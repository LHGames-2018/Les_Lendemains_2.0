using System;
using System.Collections.Generic;
using System.Linq;
using LHGames;
using LHGames.Actions;
using LHGames.Helper;
using Microsoft.AspNetCore.Razor.Language;
using StarterProject.Web.Api.Controllers;

namespace LHGames.Bot
{
    internal class Bot
    {
        public int x_max
        {
            get
            {
                bool online = GameController.playerBot.PlayerInfo.Name != "Player 1";
                if (!online)
                {
                    return 66;
                }
                else
                {
                    return 132;
                }
            }
        }
        public int y_max
        {
            get
            {
                bool online = GameController.playerBot.PlayerInfo.Name != "Player 1";
                if (!online)
                {
                    return 66;
                }
                else
                {
                    return 198;
                }
            }
        }
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
            Console.WriteLine("Atk    = " + PlayerInfo.AttackPower.ToString());
            Console.WriteLine("Def    = " + PlayerInfo.Defence.ToString());
            Console.WriteLine("Health = " + PlayerInfo.Health.ToString());
            Console.WriteLine("Max Hea= " + PlayerInfo.MaxHealth.ToString());
            Console.WriteLine("C1     = " + (PlayerInfo.Position == PlayerInfo.HouseLocation &&
                PlayerInfo.TotalResources >= 10000 &&
                PlayerInfo.CollectingSpeed == 1).ToString());
            Console.WriteLine("C2     = " + (PlayerInfo.Position == PlayerInfo.HouseLocation &&
                PlayerInfo.TotalResources >= 10000 &&
                PlayerInfo.CarryingCapacity == 1000).ToString());


            //Bypass upgrade
            if (PlayerInfo.Position == PlayerInfo.HouseLocation &&
                PlayerInfo.TotalResources >= 10000 &&
                PlayerInfo.CarryingCapacity == 1000)
            {
                Console.WriteLine("Buying a Carrying Capacity");
                return AIHelper.CreateUpgradeAction(UpgradeType.CarryingCapacity);
            }
            if (PlayerInfo.Position == PlayerInfo.HouseLocation &&
                PlayerInfo.TotalResources >= 10000 &&
                PlayerInfo.CarryingCapacity == 1250)
            {
                Console.WriteLine("Buying a Carrying Capacity");
                return AIHelper.CreateUpgradeAction(UpgradeType.CarryingCapacity);
            }
            if (PlayerInfo.Position == PlayerInfo.HouseLocation &&
                PlayerInfo.TotalResources >= 10000 &&
                PlayerInfo.AttackPower == 1)
            {
                Console.WriteLine("Buying a Attack");
                return AIHelper.CreateUpgradeAction(UpgradeType.AttackPower);
            }
            if (PlayerInfo.Position == PlayerInfo.HouseLocation &&
                PlayerInfo.TotalResources >= 10000 &&
                PlayerInfo.Defence == 1)
            {
                Console.WriteLine("Buying a Defence");
                return AIHelper.CreateUpgradeAction(UpgradeType.Defence);
            }
            if (PlayerInfo.Position == PlayerInfo.HouseLocation &&
                PlayerInfo.TotalResources >= 15000 &&
                PlayerInfo.AttackPower == 2)
            {
                Console.WriteLine("Buying a Attack");
                return AIHelper.CreateUpgradeAction(UpgradeType.AttackPower);
            }
            if (PlayerInfo.Position == PlayerInfo.HouseLocation &&
                PlayerInfo.TotalResources >= 15000 &&
                PlayerInfo.Defence == 2)
            {
                Console.WriteLine("Buying a Defence");
                return AIHelper.CreateUpgradeAction(UpgradeType.Defence);
            }
            if (PlayerInfo.Position == PlayerInfo.HouseLocation &&
                PlayerInfo.TotalResources >= 25000 &&
                PlayerInfo.AttackPower == 4)
            {
                Console.WriteLine("Buying a Attack");
                return AIHelper.CreateUpgradeAction(UpgradeType.AttackPower);
            }

            //fin des upgrades


            if (PlayerInfo.Position.Y == 0)
            {
                Console.WriteLine("Stuck at y 0, going up");
                return AIHelper.CreateMoveAction(new Point(0, -1));
            }
            if (PlayerInfo.Position.Y > 194)
            {
                Console.WriteLine("Stuck at high y, going up");
                return AIHelper.CreateMoveAction(new Point(0, -1));
            }


            //update map of the world
            worldMap.UpdateMap(map.GetVisibleTiles());
            //worldMap.UpdateOtherPLayerMap(gameInfo.OtherPlayers);
            StrategyManager.PickStrategy(PlayerInfo,visiblePlayers,worldMap);
            return StrategyManager.currentStrategy.GetNextMove(PlayerInfo, visiblePlayers, worldMap);
            /*string action = null;

            Console.WriteLine("Collect= " + PlayerInfo.CollectingSpeed.ToString());
            Console.WriteLine("Carryin= " + PlayerInfo.CarryingCapacity.ToString());
=======


            //Bypass upgrade
            if (PlayerInfo.Position == PlayerInfo.HouseLocation && !alreadyTriedToBuy)
            {
                Console.WriteLine("Buying a Carrying Capacity");
                alreadyTriedToBuy = true;
                return AIHelper.CreateUpgradeAction(UpgradeType.CarryingCapacity);
            }
            if (PlayerInfo.Position != PlayerInfo.HouseLocation)
            {
                alreadyTriedToBuy = false;
            }



            //update map of the world
            worldMap.UpdateMap(map.GetVisibleTiles());
            //worldMap.UpdateOtherPLayerMap(gameInfo.OtherPlayers);
            StrategyManager.PickStrategy();
            return StrategyManager.currentStrategy.GetNextMove(PlayerInfo, visiblePlayers, worldMap);
            
            /*string action = null;
            
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

    public Point GetClosestFreeAdjacentPosition(IPlayer player, Point position, LegacyMap map)
    {
        List<Point> freeAdjacentPositions = GetFreeAdjacentPositions(position, map);
        return freeAdjacentPositions.OrderByDescending(p => Point.DistanceManhatan(p, player.Position)).LastOrDefault();
    }

    public List<Point> GetFreeAdjacentPositions(Point position, LegacyMap map)
    {
        List<Point> adjacentPositions = new List<Point>();
        if (position.X + 1 < GameController.playerBot.x_max && map.tileTypeMap[position.X+1, position.Y] == TileContent.Empty)
            adjacentPositions.Add(new Point(position.X + 1, position.Y));
        if (position.X - 1 >=0 && map.tileTypeMap[position.X - 1, position.Y] == TileContent.Empty)
            adjacentPositions.Add(new Point(position.X - 1, position.Y));
        if (position.Y + 1 < GameController.playerBot.y_max && map.tileTypeMap[position.X, position.Y + 1] == TileContent.Empty)
            adjacentPositions.Add(new Point(position.X, position.Y + 1));
        if (position.Y - 1 >= 0 && map.tileTypeMap[position.X, position.Y - 1] == TileContent.Empty)
            adjacentPositions.Add(new Point(position.X, position.Y - 1));
        return adjacentPositions;
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

        // Si le filon le plus proche renvoit la maison, ca veut dire quon ne truove rien proche de nous. Nous allons donc aller explorer.
        //if (closestMineralPosition.X == player.HouseLocation.X && closestMineralPosition.Y == player.HouseLocation.Y)
        //{
        //    Random random = new Random();
        //    int randomlyGeneratedNumber = random.Next(1, 3);
        //    if (randomlyGeneratedNumber == 1)
        //    {
        //        Move moveTowardsHome = new Move(player, map, new Point(player.Position.X + 1, player.Position.Y));
        //        return moveTowardsHome.NextAction(map, player);
        //    }
        //    else
        //    {
        //        Move moveTowardsHome = new Move(player, map, new Point(player.Position.X, player.Position.Y -1));
        //        return moveTowardsHome.NextAction(map, player);
        //    }
        //}
        Console.WriteLine("");

        // Sinon, good, on a qqch a miner. On trouve la case a partir de laquelle on va miner
        Point closestMineralAdjacentPosition = GetClosestFreeAdjacentPosition(player, closestMineralPosition, map);

        // Si on est colles au filon, le miner
        if (Point.DistanceManhatan(closestMineralPosition, player.Position) <= 1)
        {
            return AIHelper.CreateCollectAction(new Point(closestMineralPosition.X - player.Position.X,
                closestMineralPosition.Y - player.Position.Y));
        }

        // Si aller passer par la maison avant daller au filon ne nous ralenti pas, on va aller a la maison tds 
        // Verifier si on doit rentrer pour drop nos ressources
        if ((player.CarriedResources >= 500) &&
            ((player.Position.X <= player.HouseLocation.X && closestMineralAdjacentPosition.X >= player.HouseLocation.X
            && player.Position.Y <= player.HouseLocation.Y && closestMineralAdjacentPosition.Y >= player.HouseLocation.Y)

            || (player.Position.X <= player.HouseLocation.X && closestMineralAdjacentPosition.X >= player.HouseLocation.X
            && player.Position.Y >= player.HouseLocation.Y && closestMineralAdjacentPosition.Y <= player.HouseLocation.Y)

            || (player.Position.X >= player.HouseLocation.X && closestMineralAdjacentPosition.X <= player.HouseLocation.X
            && player.Position.Y >= player.HouseLocation.Y && closestMineralAdjacentPosition.Y <= player.HouseLocation.Y)

            || (player.Position.X >= player.HouseLocation.X && closestMineralAdjacentPosition.X <= player.HouseLocation.X
            && player.Position.Y <= player.HouseLocation.Y && closestMineralAdjacentPosition.Y >= player.HouseLocation.Y)))
        {
            Move moveTowardsHome = new Move(player, map, player.HouseLocation);
            return moveTowardsHome.NextAction(map, player);
        }

        // Si on est pas colles, quon rentre pas, aller vers le filon
        Move moveTowardsMineral = new Move(player, map, closestMineralAdjacentPosition);
        return moveTowardsMineral.NextAction(map, player);
    }

    public Point GetClosestMineralPosition(IPlayer player, LegacyMap map)
    {
        int max_x = 0;
        int max_y = 0;
        bool online = GameController.playerBot.PlayerInfo.Name != "Player 1";
        if (!online)
        {
            max_x = 66;
            max_y = 66;
        }
        else
        {
            max_x = 132;
            max_y = 198;
        }

        Point centerOfSearch = player.Position;
        
        for (int edge = 1; edge < max_x  && edge < max_y; edge++)
        {
            for (int i = centerOfSearch.X - edge; i < max_x && i < centerOfSearch.X + edge; i++)
            {
                if (i >= 0)
                {
                    for (int j = centerOfSearch.Y - edge; j < max_y && j < centerOfSearch.Y + edge; j++)
                    {
                        if (j >= 0)
                        {
                            if (map.tileTypeMap[i, j] == TileContent.Resource)
                            {

                                return new Point(i, j);

                            }
                        }
                    }
                }

            }
        }

        // Retourner a la maison si jamais on trouve rien
        return player.HouseLocation;
    }

}
public class QuickAttackStrategy : Strategy
{
    public IPlayer ennemy { get; set; }

    public override string GetNextMove(IPlayer player, IEnumerable<IPlayer> visiblePlayers, LegacyMap map)
    {

        Point direction = new Point(ennemy.Position.X - player.Position.X, ennemy.Position.Y - player.Position.Y);

        return AIHelper.CreateMeleeAttackAction(direction);
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

    public static void PickStrategy(IPlayer player, IEnumerable<IPlayer> visiblePlayers, LegacyMap map)
    {
    

        if(!IsFighting(player, visiblePlayers))
        {
            currentStrategy = new MiningStrategy();

        }
    }


    public static bool IsFighting(IPlayer player, IEnumerable<IPlayer> visiblePlayers)
    {
        bool isFighting = false;
        if (visiblePlayers.Count() > 0)
        {
            int acceptableDistance = 1;
            foreach (var visiblePlayer in visiblePlayers)
            {
                if (Point.DistanceManhatan(visiblePlayer.Position, player.Position) <= acceptableDistance)
                {
                   
                    currentStrategy = new QuickAttackStrategy();
                    (currentStrategy as QuickAttackStrategy).ennemy = visiblePlayer;
                    isFighting = true;
                }
            }
        }

        return isFighting;
    }

}



class TestClass
{
    public string Test { get; set; }
}