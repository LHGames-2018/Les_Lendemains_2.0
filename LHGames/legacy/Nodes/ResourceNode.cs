
using AStar;
using StarterProject.Web.Api;
using LHGames.Helper;
namespace LHGames.Nodes
{
    class ResourceNode : Node
    {
        public ResourceNode(Node goalNode, Point point, Node parent, TileContent tileType) : base(goalNode, point, parent, tileType)
        {
        }

        public override void SetEstimatedCost(INode goal)
        {
            estimatedCost = 0;
        }

        protected override bool filterType(TileContent type)
        {
            return type == TileContent.Resource || base.filterType(type);
        }

        public override bool IsGoal(INode goal)
        {
            return tileContent == TileContent.Resource;
        }

        protected override Node create(Node goalNode, Point point, Node parent, TileContent tileType)
        {
            return new ResourceNode(goalNode, point, parent, tileType);
        }

    }
}