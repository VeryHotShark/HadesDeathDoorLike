using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Modifiers")]
    [Name("Get Modifier Order")]
    [Description("Gets the order from the modifier")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class GetModifierOrderAction : ActionTask
    {
        [RequiredField]
        [BlackboardOnly]
        public BBParameter<MonoModifier> ModifierToApply;

        [BlackboardOnly]
        public BBParameter<int> Order;

        protected override string info
        {
            get { return string.Format("Getting Order of {0}", ModifierToApply); }
        }

        protected override void OnExecute()
        {
            Order.value = ModifierToApply.value.Order;
            EndAction(true);
        }
    }
}