using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Modifiers/Raycast")]
    [Name("Get Raycast Is Thick")]
    [Description("Gets if the raycast is using thick raycasts or not")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class GetRaycastThickAction : GetValueFromModifierAction<RaycastModifier, bool>
    {
        protected override bool GetValueFrom(RaycastModifier modifier)
        { return modifier.thickRaycast; }
    }
}