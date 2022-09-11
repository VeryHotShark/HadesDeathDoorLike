using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Modifiers/Raycast")]
    [Name("Set Raycast Graph Raycasting")]
    [Description("Sets if Graph Raycasting should be used")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class SetRaycastUseGraphRaycastingAction : SetModifierValueAction<RaycastModifier, bool>
    {
        protected override void SetModifierValue(bool value)
        { Modifier.value.useGraphRaycasting = value; }
    }
}