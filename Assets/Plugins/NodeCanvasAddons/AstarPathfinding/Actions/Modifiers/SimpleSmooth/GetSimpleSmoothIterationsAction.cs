using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Modifiers/SimpleSmooth")]
    [Name("Get Smooth Iterations")]
    [Description("Gets the iterations from the smooth modifier")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class GetSimpleSmoothIterationsAction : GetValueFromModifierAction<SimpleSmoothModifier, int>
    {
        protected override int GetValueFrom(SimpleSmoothModifier modifier)
        { return modifier.iterations; }
    }
}