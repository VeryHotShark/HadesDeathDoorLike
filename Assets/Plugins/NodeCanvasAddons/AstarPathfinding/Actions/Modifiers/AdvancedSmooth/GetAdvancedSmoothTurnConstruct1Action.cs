using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Modifiers/AdvancedSmooth")]
    [Name("Get Turn Construct 1")]
    [Description("Gets the turn construct 1 from the advanced smooth modifier")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class GetAdvancedSmoothTurnConstruct1Action : GetValueFromModifierAction<AdvancedSmooth, AdvancedSmooth.MaxTurn>
    {
        protected override AdvancedSmooth.MaxTurn GetValueFrom(AdvancedSmooth  modifier)
        { return modifier.turnConstruct1; }
    }
}