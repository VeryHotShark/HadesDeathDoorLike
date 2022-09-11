using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Modifiers/Raycast")]
    [Name("Set Raycast 2d Physics")]
    [Description("Sets if 2d Physics should be used")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class SetRaycastUse2DPhysicsAction : SetModifierValueAction<RaycastModifier, bool>
    {
        protected override void SetModifierValue(bool value)
        { Modifier.value.use2DPhysics = value; }
    }
}