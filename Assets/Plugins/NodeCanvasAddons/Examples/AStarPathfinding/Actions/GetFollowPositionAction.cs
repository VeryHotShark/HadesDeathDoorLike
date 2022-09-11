using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvasAddons.AStarPathfinding.Examples
{
    [Category("Movement")]
    [Name("Get Follow Position")]
    public class GetFollowPositionAction : ActionTask
    {
        [RequiredField]
        public BBParameter<GameObject> FollowTarget;
    
        public BBParameter<float> FollowDistance = new BBParameter<float> {value = 2.0f};

        [BlackboardOnly]
        public BBParameter<Vector3> FollowPosition = new BBParameter<Vector3>();

        protected override void OnExecute()
        {
            var followPosition = FollowTarget.value.transform.position;
            followPosition -= (FollowTarget.value.transform.forward.normalized*FollowDistance.value);
            FollowPosition.value = followPosition;
            EndAction(true);
        }
    }
}