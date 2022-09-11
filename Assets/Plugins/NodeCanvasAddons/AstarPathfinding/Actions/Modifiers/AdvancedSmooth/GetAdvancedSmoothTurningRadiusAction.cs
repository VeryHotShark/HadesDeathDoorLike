using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Modifiers/AdvancedSmooth")]
    [Name("Get Turning Radius")]
    [Description("Gets the turning radius from the advanced smooth modifier")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class GetAdvancedSmoothTurningRadiusAction : GetValueFromModifierAction<AdvancedSmooth, float>
    {
        protected override float GetValueFrom(AdvancedSmooth  modifier)
        { return modifier.turningRadius; }
    }
}