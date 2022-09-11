using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;
using UnityEngine;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Rich AI")]
    [Name("Set Target")]
    [Description("Sets the target on the underlying Rich AI agent")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class SetTargetAction : ActionTask<RichAI>
    {
        [RequiredField]
        public BBParameter<Vector3> Target;

        [GetFromAgent]
        private RichAI _richAI = default;

        protected override void OnExecute()
        {
            _richAI.destination = Target.value;
            EndAction(true);
        }
    }
}