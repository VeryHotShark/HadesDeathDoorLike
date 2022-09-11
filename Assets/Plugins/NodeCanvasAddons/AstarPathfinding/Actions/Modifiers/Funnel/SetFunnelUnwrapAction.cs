using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Modifiers/Funnel")]
    [Name("Set Funnel Unwrap")]
    [Description("Sets the unwrap for the funnel modifier")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class SetFunnelUnwrapAction : SetModifierValueAction<FunnelModifier, bool>
    {
        protected override void SetModifierValue(bool value)
        { Modifier.value.unwrap = value; }
    }
}