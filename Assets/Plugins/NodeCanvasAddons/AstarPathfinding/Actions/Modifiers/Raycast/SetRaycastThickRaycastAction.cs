using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Modifiers/Raycast")]
    [Name("Set Raycast Tick Raycast")]
    [Description("Sets if thick raycasts should be used")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class SetRaycastThickRaycastAction : SetModifierValueAction<RaycastModifier, bool>
    {
        protected override void SetModifierValue(bool value)
        { Modifier.value.thickRaycast = value; }
    }
}