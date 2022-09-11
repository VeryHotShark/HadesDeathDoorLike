using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Modifiers/SimpleSmooth")]
    [Name("Set Simple Smooth Iteration")]
    [Description("Sets the iteration of the simple smooth modifier")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class SetSimpleSmoothIterationAction : SetModifierValueAction<SimpleSmoothModifier, int>
    {
        protected override void SetModifierValue(int value)
        { Modifier.value.iterations = value; }
    }
}