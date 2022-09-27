using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using Pathfinding;
using UnityEngine;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Rich AI")]
    [Name("Set Destination")]
    [Description("Sets the destination for the underlying Rich AI agent")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class SetDestinationAction : ActionTask<RichAI>
    {
        [RequiredField]
        [BlackboardOnly]
        public BBParameter<Vector3> Destination;

        [GetFromAgent]
        private RichAI _richAI = default;

        protected override void OnExecute()
        {
            _richAI.destination = Destination.value;
            EndAction(true);
        }
        
        public override void OnDrawGizmosSelected() {
            if ( _richAI != null ) {
                Gizmos.color = Color.red.WithAlpha(0.5f);
                Gizmos.DrawSphere(_richAI.destination, 3.0f);
            }
        }
    }
}