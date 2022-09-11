using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Modifiers/SimpleSmooth")]
    [Name("Set Simple Smooth Factor")]
    [Description("Sets the factor of the simple smooth modifier")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class SetSimpleSmoothFactorAction : SetModifierValueAction<SimpleSmoothModifier, float>
    {
        protected override void SetModifierValue(float value)
        { Modifier.value.factor = value; }
    }
}