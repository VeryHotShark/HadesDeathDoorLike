using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Modifiers/SimpleSmooth")]
    [Name("Set Simple Smooth Offset")]
    [Description("Sets the offset of the simple smooth modifier")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class SetSimpleSmoothOffsetAction : SetModifierValueAction<SimpleSmoothModifier, float>
    {
        protected override void SetModifierValue(float value)
        { Modifier.value.offset = value; }
    }
}