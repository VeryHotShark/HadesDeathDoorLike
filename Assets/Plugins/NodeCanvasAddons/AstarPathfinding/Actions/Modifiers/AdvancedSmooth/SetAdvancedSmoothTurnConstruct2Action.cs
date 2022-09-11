using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Modifiers/AdvancedSmooth")]
    [Name("Set Turn Construct 2")]
    [Description("Sets the turn construct 2 of the advanced smooth modifier")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class SetAdvancedSmoothTurnConstruct2Action : SetModifierValueAction<AdvancedSmooth, AdvancedSmooth.ConstantTurn>
    {
        protected override void SetModifierValue(AdvancedSmooth.ConstantTurn value)
        { Modifier.value.turnConstruct2 = value; }
    }
}