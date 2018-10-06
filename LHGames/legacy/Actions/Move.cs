
using LHGames.Nodes;
using StarterProject.Web.Api;
using System;
using System.Collections.Generic;
using LHGames.Helper;
namespace LHGames.Actions
{
    class Move : HighAction
    {
        private Point[] path;
        int idx = 0;

        public Move(IPlayer player, LegacyMap map, Point target)
        {
            Node goal = new Node(null, target, null, map.tileTypeMap[target.X, target.Y]);
            Node start = new Node(goal, player.Position, null, map.tileTypeMap[player.Position.X, player.Position.Y]);
            var a = new AStar.AStar(start, goal);
            var status = a.Run();
            if(status != AStar.State.GoalFound)
            {
                path = null;
                Console.WriteLine("Cannot find path");
            }
            else
            {
                var nodes = a.GetPath();
                int size = 0;
                foreach(var _ in nodes) ++size;
                path = new Point[size - 1];

                int idx = 0;
                foreach(var n in nodes)
                {
                    if (idx != 0)
                    {
                        Node node = (Node)n;
                        path[idx - 1] = node.Point;
                    }
                    idx++;
                }
            }
        }

        public string NextAction(LegacyMap map, IPlayer player)
        {
            if (path == null)
            {
                return null;
            }
            else if(idx < path.Length)
            {
                Point p = path[idx];
                var type = map.tileTypeMap[p.X, p.Y];
                if (type == TileContent.Lava || type == TileContent.Resource || type == TileContent.Unknown)
                {
                    return null;
                }
                else if (type == TileContent.Wall)
                {
                    Console.WriteLine("RAWR");
                    //return AIHelper.CreateMeleeAttackAction(p - player.Position); //hotfix degueu
                    return AIHelper.CreateMoveAction(new Point(0, -1));
                }
                idx++;
                return AIHelper.CreateMoveAction(p - player.Position);
            }
            else
            {
                return null;
            }
        }

        public override string ToString()
        {
            if (path.Length > 0)
            {
                return "Move to " + path[path.Length - 1];
            }
            else
            {
                return "";
            }
        }

        public static Move MoveAdjencent(IPlayer player, LegacyMap m, Point target)
        {
            Point diff = target - player.Position;
            if(diff.X >= diff.Y)
            {
                diff = new Point(diff.X > 0 ? 1 : -1, 0);
            }
            else
            {
                diff = new Point(0, diff.Y > 0 ? 1 : -1);
            }
            return new Move(player, m, target - diff);
        }
    }
}