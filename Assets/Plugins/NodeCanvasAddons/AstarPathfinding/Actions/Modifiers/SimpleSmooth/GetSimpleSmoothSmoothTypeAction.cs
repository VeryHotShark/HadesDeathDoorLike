using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Modifiers/SimpleSmooth")]
    [Name("Get Smooth Type")]
    [Description("Gets the type from the smooth modifier")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class GetSimpleSmoothSmoothTypeAction : GetValueFromModifierAction<SimpleSmoothModifier, SimpleSmoothModifier.SmoothType>
    {
        protected override SimpleSmoothModifier.SmoothType GetValueFrom(SimpleSmoothModifier modifier)
        { return modifier.smoothType; }
    }
}