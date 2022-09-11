using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding.RVO;
using UnityEngine;

namespace NodeCanvasAddons.AStarPathfinding.LocalAvoidance
{
    [Category("A* Pathfinding Pro/RVOController")]
    [Name("Move")]
    [Description("Moves the RVOController")]
    [ParadoxNotion.Design.Icon("PathfindingNWaypoint")]
    public class MoveAction : ActionTask<RVOController>
    {
        [RequiredField]
        public BBParameter<Vector3> Velocity;

        [GetFromAgent]
        private RVOController _rvoController = default;

        protected override void OnExecute()
        {
            _rvoController.Move(Velocity.value);
            EndAction(true);
        }
    }
}