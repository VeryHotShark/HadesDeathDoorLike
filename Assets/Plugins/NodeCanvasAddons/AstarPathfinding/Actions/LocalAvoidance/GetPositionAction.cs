using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding.RVO;
using UnityEngine;

namespace NodeCanvasAddons.AStarPathfinding.LocalAvoidance
{
    [Category("A* Pathfinding Pro/RVOController")]
    [Name("Get Position")]
    [Description("Gets the position for the RVOController")]
    [ParadoxNotion.Design.Icon("PathfindingNWaypoint")]
    public class GetPositionAction : ActionTask<RVOController>
    {
        [RequiredField]
        [BlackboardOnly]
        public BBParameter<Vector3> Position;

        [GetFromAgent]
        private RVOController _rvoController = default;

        protected override void OnExecute()
        {
            Position.value = _rvoController.position;
            EndAction(true);
        }
    }
}