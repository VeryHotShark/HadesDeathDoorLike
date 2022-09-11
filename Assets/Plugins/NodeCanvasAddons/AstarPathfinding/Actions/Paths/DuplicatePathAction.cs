using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Paths")]
    [Name("Duplicate Path")]
    [Description("Duplicates a given path")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class DuplicatePathAction : ActionTask
    {
        [RequiredField]
        [BlackboardOnly]
        public BBParameter<Path> Path;

        [RequiredField]
        [BlackboardOnly]
        public BBParameter<Path> DuplicatePath = new BBParameter<Path>();

        protected override string info
        {
            get { return string.Format("Duplicating path {0} to {1}", Path, DuplicatePath); }
        }

        protected override void OnExecute()
        {
            if (Path.isNone || Path.isNull || DuplicatePath.isNone)
            { EndAction(false); }

            DuplicatePath.value = Path.value.DuplicatePath();
            EndAction(true);
        }
    }
}