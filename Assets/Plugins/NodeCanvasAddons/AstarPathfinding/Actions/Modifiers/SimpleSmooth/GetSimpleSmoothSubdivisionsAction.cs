using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Modifiers/SimpleSmooth")]
    [Name("Get Smooth Subdivisions")]
    [Description("Gets the subdivisions from the smooth modifier")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class GetSimpleSmoothSubdivisionsAction : GetValueFromModifierAction<SimpleSmoothModifier, int>
    {
        protected override int GetValueFrom(SimpleSmoothModifier modifier)
        { return modifier.subdivisions; }
    }
}