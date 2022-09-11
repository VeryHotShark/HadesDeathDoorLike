using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Modifiers/Radius")]
    [Name("Set Radius Detail")]
    [Description("Sets detail of the radius modifier")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class SetRadiusDetailAction : SetModifierValueAction<RadiusModifier, float>
    {
        protected override void SetModifierValue(float value)
        { Modifier.value.detail = value; }
    }
}