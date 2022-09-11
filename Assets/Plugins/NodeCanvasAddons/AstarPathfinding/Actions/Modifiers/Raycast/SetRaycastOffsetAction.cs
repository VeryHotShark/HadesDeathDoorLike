using ParadoxNotion.Design;
using Pathfinding;
using UnityEngine;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Modifiers/Raycast")]
    [Name("Set Raycast Offset")]
    [Description("Sets the offset for the raycast modifier")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class SetRaycastOffsetAction : SetModifierValueAction<RaycastModifier, Vector3>
    {
        protected override void SetModifierValue(Vector3 value)
        { Modifier.value.raycastOffset = value; }
    }
}