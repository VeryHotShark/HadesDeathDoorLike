using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding.RVO;
using UnityEngine;

namespace NodeCanvasAddons.AStarPathfinding.LocalAvoidance
{
    [Category("A* Pathfinding Pro/RVOController")]
    [Name("Teleport")]
    [Description("Teleports the RVOController")]
    [ParadoxNotion.Design.Icon("PathfindingNWaypoint")]
    public class TeleportAction : ActionTask<RVOController>
    {
        [RequiredField]
        public BBParameter<Vector3> Position;

        [GetFromAgent]
        private RVOController _rvoController = default;

        protected override void OnExecute()
        {
            _rvoController.transform.position =  Position.value;
            EndAction(true);
        }
    }
}