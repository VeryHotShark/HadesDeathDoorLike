using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Modifiers/Raycast")]
    [Name("Get Raycast Thickness")]
    [Description("Gets the thickness/radius of the raycast")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class GetRaycastThicknessAction : GetValueFromModifierAction<RaycastModifier, float>
    {
        protected override float GetValueFrom(RaycastModifier modifier)
        { return modifier.thickRaycastRadius; }
    }
}