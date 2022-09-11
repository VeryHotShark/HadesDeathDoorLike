using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Modifiers/SimpleSmooth")]
    [Name("Set Simple Smooth Strength")]
    [Description("Sets the strength of the simple smooth modifier")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class SetSimpleSmoothStrengthAction : SetModifierValueAction<SimpleSmoothModifier, float>
    {
        protected override void SetModifierValue(float value)
        { Modifier.value.strength = value; }
    }
}