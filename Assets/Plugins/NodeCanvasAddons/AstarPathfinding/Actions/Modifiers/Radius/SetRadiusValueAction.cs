using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Modifiers/Radius")]
    [Name("Set Radius Value")]
    [Description("Sets value of the radius modifier")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class SetRadiusValueAction : SetModifierValueAction<RadiusModifier, float>
    {
        protected override void SetModifierValue(float value)
        { Modifier.value.radius = value; }
    }
}