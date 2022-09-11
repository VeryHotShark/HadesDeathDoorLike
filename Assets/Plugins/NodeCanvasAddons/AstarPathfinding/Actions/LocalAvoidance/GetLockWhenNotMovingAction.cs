using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding.RVO;

namespace NodeCanvasAddons.AStarPathfinding.LocalAvoidance
{
    [Category("A* Pathfinding Pro/RVOController")]
    [Name("Get Lock When Not Moving")]
    [Description("Gets the lock when not moving for the RVOController")]
    [ParadoxNotion.Design.Icon("PathfindingNWaypoint")]
    public class GetLockWhenNotMovingAction : ActionTask<RVOController>
    {
        [RequiredField]
        [BlackboardOnly]
        public BBParameter<bool> LockWhenNotMoving;

        [GetFromAgent]
        private RVOController _rvoController = default;

        protected override void OnExecute()
        {
            LockWhenNotMoving.value = _rvoController.lockWhenNotMoving;
            EndAction(true);
        }
    }
}