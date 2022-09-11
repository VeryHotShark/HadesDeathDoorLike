using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Modifiers/Raycast")]
    [Name("Get Raycast Quality")]
    [Description("Gets the quality of the raycast modifier")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class GetRaycastQualityAction : GetValueFromModifierAction<RaycastModifier, RaycastModifier.Quality>
    {
        protected override RaycastModifier.Quality GetValueFrom(RaycastModifier modifier)
        { return modifier.quality; }
    }
}