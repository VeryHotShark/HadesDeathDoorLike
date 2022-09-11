using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Modifiers/Raycast")]
    [Name("Get Raycast Is Using Raycasts")]
    [Description("Gets if the raycast is using raycasts or not")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class GetRaycastUseRaycastingAction : GetValueFromModifierAction<RaycastModifier, bool>
    {
        protected override bool GetValueFrom(RaycastModifier modifier)
        { return modifier.useRaycasting; }
    }
}