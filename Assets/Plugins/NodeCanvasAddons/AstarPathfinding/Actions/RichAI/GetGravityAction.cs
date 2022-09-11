using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;
using UnityEngine;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Rich AI")]
    [Name("Get Gravity")]
    [Description("Gets the gravity from the underlying Rich AI agent")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class GetGravityAction : ActionTask<RichAI>
    {
        [RequiredField]
        [BlackboardOnly]
        public BBParameter<Vector3> Gravity;

        [GetFromAgent]
        private RichAI _richAI = default;

        protected override void OnExecute()
        {
            Gravity.value = _richAI.gravity;
            EndAction(true);
        }
    }
}