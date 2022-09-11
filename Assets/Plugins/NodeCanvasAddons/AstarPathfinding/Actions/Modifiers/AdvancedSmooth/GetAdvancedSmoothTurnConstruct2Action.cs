using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Modifiers/AdvancedSmooth")]
    [Name("Get Turn Construct 2")]
    [Description("Gets the turn construct 2 from the advanced smooth modifier")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class GetAdvancedSmoothTurnConstruct2Action : GetValueFromModifierAction<AdvancedSmooth, AdvancedSmooth.ConstantTurn>
    {
        protected override AdvancedSmooth.ConstantTurn GetValueFrom(AdvancedSmooth  modifier)
        { return modifier.turnConstruct2; }
    }
}