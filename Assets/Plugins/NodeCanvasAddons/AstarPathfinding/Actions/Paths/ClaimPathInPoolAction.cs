using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Paths")]
    [Name("Claim Path In Pool")]
    [Description("Claims the path in the pathing pool")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class ClaimPathInPoolAction : ActionTask
    {
        [RequiredField]
        [BlackboardOnly]
        public BBParameter<Path> Path;

        protected override string info
        {
            get { return string.Format("Claiming path {0} \nin pool by {1}", Path, agent.name); }
        }

        protected override void OnExecute()
        {
            if (Path.isNone || Path.isNull)
            { EndAction(false); }

            Path.value.Claim(agent.gameObject);
            EndAction(true);
        }
    }
}