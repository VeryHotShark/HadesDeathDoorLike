using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvasAddons.AStarPathfinding.Examples
{
    [Category("GameObject")]
    [Name("Get Position")]
    public class GetPositionAction : ActionTask<Transform>
    {
        [BlackboardOnly]
        public BBParameter<Vector3> Position;

        protected override void OnExecute()
        {
            Position.value = agent.transform.position;
            EndAction(true);
        }
    }
}