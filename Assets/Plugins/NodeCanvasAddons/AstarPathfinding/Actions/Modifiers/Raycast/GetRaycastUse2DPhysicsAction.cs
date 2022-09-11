using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Modifiers/Raycast")]
    [Name("Get Raycast Is Using 2d Physics")]
    [Description("Gets if the raycast is using 2d Physics or not")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class GetRaycastUse2DPhysicsAction : GetValueFromModifierAction<RaycastModifier, bool>
    {
        protected override bool GetValueFrom(RaycastModifier modifier)
        { return modifier.use2DPhysics; }
    }
}