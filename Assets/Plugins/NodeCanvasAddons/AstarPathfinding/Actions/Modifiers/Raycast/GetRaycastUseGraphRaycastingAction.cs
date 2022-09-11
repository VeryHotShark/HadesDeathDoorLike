using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Modifiers/Raycast")]
    [Name("Get Raycast Is Using Graph Raycasting")]
    [Description("Gets if the raycast is using graph raycasting or not")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class GetRaycastUseGraphRaycastingAction : GetValueFromModifierAction<RaycastModifier, bool>
    {
        protected override bool GetValueFrom(RaycastModifier modifier)
        { return modifier.useGraphRaycasting; }
    }
}