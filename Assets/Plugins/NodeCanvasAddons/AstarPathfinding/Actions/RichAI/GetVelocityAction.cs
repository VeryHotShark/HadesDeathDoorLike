using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;
using UnityEngine;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Rich AI")]
    [Name("Get Velocity")]
    [Description("Gets the velocity on the underlying Rich AI agent")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class GetVelocityAction : ActionTask<RichAI>
    {
        [RequiredField]
        [BlackboardOnly]
        public BBParameter<Vector3> Velocity;

        [GetFromAgent]
        private RichAI _richAI = default;

        protected override void OnExecute()
        {
            Velocity.value = _richAI.velocity;
            EndAction(true);
        }
    }
}