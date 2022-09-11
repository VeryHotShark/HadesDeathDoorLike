using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Modifiers/Funnel")]
    [Name("Set Funnel Should Split At Every Portal")]
    [Description("Sets if the funnel should split at every portal")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class SetFunnelSplitAtEveryPortalAction : SetModifierValueAction<FunnelModifier, bool>
    {
        protected override void SetModifierValue(bool value)
        { Modifier.value.splitAtEveryPortal = value; }
    }
}