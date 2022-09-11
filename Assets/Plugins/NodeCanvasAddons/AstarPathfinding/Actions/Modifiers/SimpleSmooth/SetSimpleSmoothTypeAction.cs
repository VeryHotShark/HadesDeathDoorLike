using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Modifiers/SimpleSmooth")]
    [Name("Set Simple Smooth Type")]
    [Description("Sets the type of the simple smooth modifier")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class SetSimpleSmoothTypeAction : SetModifierValueAction<SimpleSmoothModifier, SimpleSmoothModifier.SmoothType>
    {
        protected override void SetModifierValue(SimpleSmoothModifier.SmoothType value)
        { Modifier.value.smoothType = value; }
    }
}