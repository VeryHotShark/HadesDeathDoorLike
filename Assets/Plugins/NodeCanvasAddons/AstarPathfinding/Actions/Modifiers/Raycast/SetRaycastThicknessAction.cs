using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Modifiers/Raycast")]
    [Name("Set Raycast Thickness")]
    [Description("Sets the raycast thickness/radius for the raycast modifier")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class SetRaycastThicknessAction : SetModifierValueAction<RaycastModifier, float>
    {
        protected override void SetModifierValue(float value)
        { Modifier.value.thickRaycastRadius = value; }
    }
}