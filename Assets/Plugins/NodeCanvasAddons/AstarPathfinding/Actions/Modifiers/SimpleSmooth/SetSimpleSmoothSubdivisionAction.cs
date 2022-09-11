using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Modifiers/SimpleSmooth")]
    [Name("Set Simple Smooth Subdivisions")]
    [Description("Sets the subdivisions of the simple smooth modifier")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class SetSimpleSmoothSubdivisionAction : SetModifierValueAction<SimpleSmoothModifier, int>
    {
        protected override void SetModifierValue(int value)
        { Modifier.value.subdivisions = value; }
    }
}