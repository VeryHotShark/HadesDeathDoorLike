using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Paths")]
    [Name("Reverse Path")]
    [Description("Reverses a given path")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class ReversePathAction : ActionTask
    {
        [RequiredField]
        [BlackboardOnly]
        public BBParameter<Path> Path;

        [BlackboardOnly]
        public BBParameter<Path> ReversedPath = new BBParameter<Path>();

        protected override string info
        {
            get { return string.Format("Reversing path {0} to {1}", Path, ReversedPath); }
        }

        protected override void OnExecute()
        {
            if (Path.isNone || Path.isNull || ReversedPath.isNone)
            { EndAction(false); }

            ReversedPath.value = Path.value.ReversePath();
            EndAction(true);
        }
    }
}