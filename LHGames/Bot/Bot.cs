﻿using System;
using System.Collections.Generic;
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
          
            return action;

        }

        /// <summary>
        /// Gets called after ExecuteTurn.
        /// </summary>
        internal void AfterTurn()
        {
        }
    }
}

class TestClass
{
    public string Test { get; set; }
}