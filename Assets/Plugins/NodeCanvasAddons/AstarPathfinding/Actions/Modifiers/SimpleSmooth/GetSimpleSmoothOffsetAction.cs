using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Modifiers/SimpleSmooth")]
    [Name("Get Smooth Offset")]
    [Description("Gets the offset from the smooth modifier")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class GetSimpleSmoothOffsetAction : GetValueFromModifierAction<SimpleSmoothModifier, float>
    {
        protected override float GetValueFrom(SimpleSmoothModifier modifier)
        { return modifier.offset; }
    }
}