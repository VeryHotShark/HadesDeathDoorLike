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
    [Name("Get Wall Avoid Falloff")]
    [Description("Gets the wall avoid falloff for the RVOController")]
    [ParadoxNotion.Design.Icon("PathfindingNWaypoint")]
    public class GetWallAvoidFalloffAction : ActionTask<RVOController>
    {
        [RequiredField]
        [BlackboardOnly]
        public BBParameter<float> WallAvoidFalloff;

        [GetFromAgent]
        private RVOController _rvoController;

        protected override void OnExecute()
        {
            WallAvoidFalloff.value = _rvoController.wallAvoidFalloff;
            EndAction(true);
        }
    }*/
}