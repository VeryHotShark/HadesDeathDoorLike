using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Modifiers/SimpleSmooth")]
    [Name("Set Simple Smooth Uniform Length")]
    [Description("Sets the uniform length of the simple smooth modifier")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class SetSimpleSmoothUniformLengthAction : SetModifierValueAction<SimpleSmoothModifier, bool>
    {
        protected override void SetModifierValue(bool value)
        { Modifier.value.uniformLength = value; }
    }
}