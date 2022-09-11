using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Modifiers/Radius")]
    [Name("Get Radius Value")]
    [Description("Gets the value of the radius modifier")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class GetRadiusValueAction : GetValueFromModifierAction<RadiusModifier, float>
    {
        protected override float GetValueFrom(RadiusModifier modifier)
        { return modifier.radius; }
    }
}