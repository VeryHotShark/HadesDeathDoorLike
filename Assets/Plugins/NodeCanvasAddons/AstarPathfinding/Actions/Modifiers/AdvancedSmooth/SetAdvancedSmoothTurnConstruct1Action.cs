using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Modifiers/AdvancedSmooth")]
    [Name("Set Turn Construct 1")]
    [Description("Sets the turn construct 1 of the advanced smooth modifier")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class SetAdvancedSmoothTurnConstruct1Action : SetModifierValueAction<AdvancedSmooth, AdvancedSmooth.MaxTurn>
    {
        protected override void SetModifierValue(AdvancedSmooth.MaxTurn value)
        { Modifier.value.turnConstruct1 = value; }
    }
}