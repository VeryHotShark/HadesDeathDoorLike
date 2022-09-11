using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Paths")]
    [Name("Free Path In Pool")]
    [Description("Frees the path from the pathing pool")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class FreePathInPoolAction : ActionTask
    {
        [RequiredField]
        [BlackboardOnly]
        public BBParameter<Path> Path;

        protected override string info
        {
            get { return string.Format("Freeing path {0} \nin pool by {1}", Path, agent.name); }
        }

        protected override void OnExecute()
        {
            if (Path.isNone || Path.isNull)
            { EndAction(false); }

            Path.value.Release(agent.gameObject);
            EndAction(true);
        }
    }
}