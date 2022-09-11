using ParadoxNotion.Design;
using Pathfinding;
using UnityEngine;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Modifiers/Raycast")]
    [Name("Set Raycast Mask")]
    [Description("Sets the mask for the raycast modifier")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class SetRaycastMaskAction : SetModifierValueAction<RaycastModifier, LayerMask>
    {
        protected override void SetModifierValue(LayerMask value)
        { Modifier.value.mask = value; }
    }
}