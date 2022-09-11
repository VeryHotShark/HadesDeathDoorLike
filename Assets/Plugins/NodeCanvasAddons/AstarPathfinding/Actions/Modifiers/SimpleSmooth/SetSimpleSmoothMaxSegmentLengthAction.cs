using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Modifiers/SimpleSmooth")]
    [Name("Set Simple Smooth Max Segment Length")]
    [Description("Sets the max segment length of the simple smooth modifier")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class SetSimpleSmoothMaxSegmentLengthAction : SetModifierValueAction<SimpleSmoothModifier, float>
    {
        protected override void SetModifierValue(float value)
        { Modifier.value.maxSegmentLength = value; }
    }
}