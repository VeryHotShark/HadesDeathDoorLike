using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Modifiers/SimpleSmooth")]
    [Name("Get Smooth Factor")]
    [Description("Gets the factor from the smooth modifier")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class GetSimpleSmoothFactorAction : GetValueFromModifierAction<SimpleSmoothModifier, float>
    {
        protected override float GetValueFrom(SimpleSmoothModifier modifier)
        { return modifier.factor; }
    }
}