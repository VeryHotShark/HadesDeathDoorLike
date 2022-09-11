using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Modifiers/Funnel")]
    [Name("Get Funnel Should Split At Every Portal")]
    [Description("Gets if the funnel should split at every portal")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class GetFunnelSplitAtEveryPortalAction : GetValueFromModifierAction<FunnelModifier, bool>
    {
        protected override bool GetValueFrom(FunnelModifier modifier)
        { return modifier.splitAtEveryPortal; }
    }
}