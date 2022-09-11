using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Modifiers/SimpleSmooth")]
    [Name("Get Smooth Strength")]
    [Description("Gets the strength from the smooth modifier")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class GetSimpleSmoothStrengthAction : GetValueFromModifierAction<SimpleSmoothModifier, float>
    {
        protected override float GetValueFrom(SimpleSmoothModifier modifier)
        { return modifier.strength; }
    }
}