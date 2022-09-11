using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding.RVO;
using UnityEngine;

namespace NodeCanvasAddons.AStarPathfinding.LocalAvoidance
{
    [Category("A* Pathfinding Pro/RVOController")]
    [Name("Set Target")]
    [Description("Sets the RVOControllers Target")]
    [ParadoxNotion.Design.Icon("PathfindingNWaypoint")]
    public class SetTargetAction : ActionTask<RVOController>
    {
        [RequiredField]
        public BBParameter<Vector3> Target;

        [RequiredField]
        public BBParameter<float> Speed;

        [RequiredField]
        public BBParameter<float> MaxSpeed;

        [GetFromAgent]
        private RVOController _rvoController = default;

        protected override void OnExecute()
        {
            _rvoController.SetTarget(Target.value, Speed.value, MaxSpeed.value, new Vector3(Mathf.Infinity,Mathf.Infinity,Mathf.Infinity));
            EndAction(true);
        }
    }
}