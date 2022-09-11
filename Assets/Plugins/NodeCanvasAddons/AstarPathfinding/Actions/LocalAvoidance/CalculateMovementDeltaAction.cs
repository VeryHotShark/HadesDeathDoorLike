using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding.RVO;
using UnityEngine;

namespace NodeCanvasAddons.AStarPathfinding.LocalAvoidance
{
    [Category("A* Pathfinding Pro/RVOController")]
    [Name("Calculate Movement Delta")]
    [Description("Requests the movement delta from the RVOController")]
    [ParadoxNotion.Design.Icon("PathfindingNWaypoint")]
    public class CalculateMovementDeltaAction : ActionTask<RVOController>
    {
        [RequiredField]
        [BlackboardOnly]
        public BBParameter<Vector3> Delta;

        public BBParameter<Vector3> PositionOverride;

        [GetFromAgent]
        private RVOController _rvoController = default;

        protected override void OnExecute()
        {
            Vector3 delta;

            if(PositionOverride.isNone || PositionOverride.isNull)
            { delta = _rvoController.CalculateMovementDelta(Time.deltaTime); }
            else
            { delta = _rvoController.CalculateMovementDelta(PositionOverride.value, Time.deltaTime); }

            Delta.value = delta;

            EndAction(true);
        }
    }
}