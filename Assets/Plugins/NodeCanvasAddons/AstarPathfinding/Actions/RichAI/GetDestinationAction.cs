using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;
using UnityEngine;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Rich AI")]
    [Name("Get Destination")]
    [Description("Gets the destination from the underlying Rich AI agent")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class GetDestinationAction : ActionTask<RichAI>
    {
        [RequiredField]
        [BlackboardOnly]
        public BBParameter<Vector3> Destination;

        [GetFromAgent]
        private RichAI _richAI = default;

        protected override void OnExecute()
        {
            Destination.value = _richAI.destination;
            EndAction(true);
        }
    }
}