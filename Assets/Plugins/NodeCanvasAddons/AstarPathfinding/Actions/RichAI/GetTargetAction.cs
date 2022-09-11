using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;
using UnityEngine;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Rich AI")]
    [Name("Get Target")]
    [Description("Gets the target on the underlying Rich AI agent")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class GetTargetAction : ActionTask<RichAI>
    {
        [RequiredField]
        [BlackboardOnly]
        public BBParameter<Vector3> Target;

        [GetFromAgent]
        private RichAI _richAI = default;

        protected override void OnExecute()
        {
            Target.value = _richAI.destination;
            EndAction(true);
        }
    }
}