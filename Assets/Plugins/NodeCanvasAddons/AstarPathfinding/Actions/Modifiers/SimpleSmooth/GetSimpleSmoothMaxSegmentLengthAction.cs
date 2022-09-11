using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Modifiers/SimpleSmooth")]
    [Name("Get Smooth Max Segment Length")]
    [Description("Gets the max segment length from the smooth modifier")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class GetSimpleSmoothMaxSegmentLengthAction : GetValueFromModifierAction<SimpleSmoothModifier, float>
    {
        protected override float GetValueFrom(SimpleSmoothModifier modifier)
        { return modifier.maxSegmentLength; }
    }
}