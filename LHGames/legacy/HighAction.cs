
using StarterProject.Web.Api;
using LHGames.Helper;
namespace LHGames
{
    interface HighAction
    {
        // returns null when done
        string NextAction(LegacyMap map,  IPlayer player);
    }
}