using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding.RVO;

namespace NodeCanvasAddons.AStarPathfinding.LocalAvoidance
{
    /*
        Library has deprecated this but may re-enable it in future
        So for anyone who needs it, we will leave this action in
        and re-enable it/remove it once a final decision has been
        made by the AStar team.
    */
    /*
    [Category("A* Pathfinding Pro/RVOController")]
    [Name("Get Wall Avoid Force")]
    [Description("Gets the wall avoid force for the RVOController")]
    [ParadoxNotion.Design.Icon("PathfindingNWaypoint")]
    public class GetWallAvoidForceAction : ActionTask<RVOController>
    {
        [RequiredField]
        [BlackboardOnly]
        public BBParameter<float> WallAvoidForce;

        [GetFromAgent]
        private RVOController _rvoController;

        protected override void OnExecute()
        {
            WallAvoidForce.value = _rvoController.wallAvoidForce;
            EndAction(true);
        }
    }
    */
}