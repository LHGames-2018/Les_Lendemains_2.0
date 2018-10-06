
using StarterProject.Web.Api;
using LHGames.Helper;
using System;

namespace LHGames.Actions
{
    class Collect : HighAction
    {
        private Point target;
        private bool done = false;

        public Collect(Point target)
        {
            this.target = target;
        }

        public string NextAction(LegacyMap map, IPlayer player)
        {
            if (!done)
            {
                done = true;
                Console.WriteLine(target - player.Position);
                return AIHelper.CreateCollectAction(target - player.Position);
            }
            else
            {
                return null;
            }
        }

        public override string ToString()
        {
            return "Collect at " + target.ToString();
        }
    }
}