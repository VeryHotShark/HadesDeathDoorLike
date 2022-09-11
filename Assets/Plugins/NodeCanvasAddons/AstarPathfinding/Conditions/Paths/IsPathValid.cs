using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding")]
    [Name("Is Path Valid")]
    [Description("Checks to see if the path is valid and not null or empty")]
    [ParadoxNotion.Design.Icon("PathfindingWaypoint")]
    public class IsPathValid : ConditionTask
    {
        [RequiredField]
        [BlackboardOnly]
        public BBParameter<Path> Path;
        
        protected override string info
        {
            get { return string.Format("Is {0} valid?", Path); }
        }

        protected override bool OnCheck()
        {
            var isPathInvalid = (Path.isNone || Path.isNull || Path.value == null || Path.value.error || Path.value.vectorPath.Count == 0);
            return !isPathInvalid;
        }
    }
}