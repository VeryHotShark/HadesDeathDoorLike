using ParadoxNotion.Design;
using Pathfinding;
using UnityEngine;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Modifiers/Radius")]
    [Name("Get Radius Detail")]
    [Description("Gets the detail of the radius modifier")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class GetRadiusDetailAction : GetValueFromModifierAction<RadiusModifier, float>
    {
        protected override float GetValueFrom(RadiusModifier modifier)
        { return modifier.detail; }
    }
}