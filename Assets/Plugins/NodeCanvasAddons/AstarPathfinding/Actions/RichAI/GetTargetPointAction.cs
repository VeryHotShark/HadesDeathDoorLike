using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;
using UnityEngine;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Rich AI")]
    [Name("Get Target Point")]
    [Description("Gets the target point from the underlying Rich AI agent")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class GetTargetPointAction : ActionTask<RichAI>
    {
        [RequiredField]
        [BlackboardOnly]
        public BBParameter<Vector3> TargetPoint;

        [GetFromAgent]
        private RichAI _richAI = default;

        protected override void OnExecute()
        {
            TargetPoint.value = _richAI.steeringTarget;
            EndAction(true);
        }
    }
}