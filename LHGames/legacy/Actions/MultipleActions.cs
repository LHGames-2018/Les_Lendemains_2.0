
using StarterProject.Web.Api;
using System;
using LHGames.Helper;
namespace LHGames.Actions
{
    class MultipleActions : HighAction
    {
        private HighAction[] actions;
        int idx = 0;

        MultipleActions(HighAction[] actions)
        {
            if (actions.Length == 0)
                throw new ArgumentException("Length 0 argument");
            this.actions = actions;
        }

        public string NextAction(LegacyMap map, IPlayer player)
        {
            string next = null;
            while(next == null)
            {
                next = actions[idx]?.NextAction(map, player);
                if(next == null)
                {
                    ++idx;
                }
                if (idx >= actions.Length)
                    break;
            }
            return next;
        }
        
        public static MultipleActions MoveThenCollect(IPlayer player, LegacyMap map, Point target)
        {
            Move move = Move.MoveAdjencent(player, map, target);
            Collect collect = new Collect(target);
            return new MultipleActions(new HighAction[] { move, collect });
        }

        public override string ToString()
        {
            string res = "=== Multiple Actions\n";
            foreach (var a in actions)
            {
                res += "   " + a.ToString() + "\n";
            }
            res += "===";
            return res;
        }
    }
}