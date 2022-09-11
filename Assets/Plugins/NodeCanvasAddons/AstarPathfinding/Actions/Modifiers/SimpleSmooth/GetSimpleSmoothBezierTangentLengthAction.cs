using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Modifiers/SimpleSmooth")]
    [Name("Get Smooth Bezier Tangent Length")]
    [Description("Gets the bezier tangent length from the smooth modifier")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class GetSimpleSmoothBezierTangentLengthAction : GetValueFromModifierAction<SimpleSmoothModifier, float>
    {
        protected override float GetValueFrom(SimpleSmoothModifier modifier)
        { return modifier.bezierTangentLength; }
    }
}