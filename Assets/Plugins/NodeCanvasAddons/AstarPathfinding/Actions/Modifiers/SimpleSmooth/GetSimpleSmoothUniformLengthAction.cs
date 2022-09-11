using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Modifiers/SimpleSmooth")]
    [Name("Get Smooth Uniform Length")]
    [Description("Gets the uniform length from the smooth modifier")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class GetSimpleSmoothUniformLengthAction : GetValueFromModifierAction<SimpleSmoothModifier, bool>
    {
        protected override bool GetValueFrom(SimpleSmoothModifier modifier)
        { return modifier.uniformLength; }
    }
}