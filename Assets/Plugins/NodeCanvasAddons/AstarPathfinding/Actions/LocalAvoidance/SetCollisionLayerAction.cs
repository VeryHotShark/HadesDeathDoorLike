using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding.RVO;

namespace NodeCanvasAddons.AStarPathfinding.LocalAvoidance
{
    [Category("A* Pathfinding Pro/RVOController")]
    [Name("Set Collision Layer")]
    [Description("Sets the collision layer for the RVOController")]
    [ParadoxNotion.Design.Icon("PathfindingNWaypoint")]
    public class SetCollisionLayerAction : ActionTask<RVOController>
    {
        [RequiredField]
        [BlackboardOnly]
        public BBParameter<RVOLayer> Layer;

        [GetFromAgent]
        private RVOController _rvoController = default;

        protected override void OnExecute()
        {
            _rvoController.collidesWith = Layer.value;
            EndAction(true);
        }
    }
}