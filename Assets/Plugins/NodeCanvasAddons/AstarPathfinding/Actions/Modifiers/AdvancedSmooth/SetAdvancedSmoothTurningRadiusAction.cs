using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Modifiers/AdvancedSmooth")]
    [Name("Set Turning Radius")]
    [Description("Sets the turning radius of the advanced smooth modifier")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class SetAdvancedSmoothTurningRadiusAction : SetModifierValueAction<AdvancedSmooth, float>
    {
        protected override void SetModifierValue(float value)
        { Modifier.value.turningRadius = value; }
    }
}