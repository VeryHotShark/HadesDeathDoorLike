using ParadoxNotion.Design;
using Pathfinding;
using UnityEngine;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Modifiers/Raycast")]
    [Name("Get Raycast Offset")]
    [Description("Gets the offset of the raycast modifier")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class GetRaycastOffsetAction : GetValueFromModifierAction<RaycastModifier, Vector3>
    {
        protected override Vector3 GetValueFrom(RaycastModifier modifier)
        { return modifier.raycastOffset; }
    }
}