using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Modifiers/Funnel")]
    [Name("Get Funnel Unwrap")]
    [Description("Gets if the funnel should unwrap")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class GetFunnelUnwrapAction : GetValueFromModifierAction<FunnelModifier, bool>
    {
        protected override bool GetValueFrom(FunnelModifier modifier)
        { return modifier.unwrap; }
    }
}