using ParadoxNotion.Design;
using Pathfinding;
using UnityEngine;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Modifiers/Raycast")]
    [Name("Get Raycast Mask")]
    [Description("Gets the mask of the raycast modifier")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class GetRaycastMaskAction : GetValueFromModifierAction<RaycastModifier, LayerMask>
    {
        protected override LayerMask GetValueFrom(RaycastModifier modifier)
        { return modifier.mask; }
    }
}