using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Modifiers/Raycast")]
    [Name("Set Raycast Quality")]
    [Description("Sets the quality of the raycast modifier")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class SetRaycastQualityAction : SetModifierValueAction<RaycastModifier, RaycastModifier.Quality>
    {
        protected override void SetModifierValue(RaycastModifier.Quality value)
        { Modifier.value.quality = value; }
    }
}