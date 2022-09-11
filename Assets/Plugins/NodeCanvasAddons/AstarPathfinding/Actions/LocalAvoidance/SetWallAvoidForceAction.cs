using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding.RVO;

namespace NodeCanvasAddons.AStarPathfinding.LocalAvoidance
{
    /*
        AStar Library has deprecated this but may re-enable it in future
        So for anyone who needs it, we will leave this action in
        and re-enable it/remove it once a final decision has been
        made by the AStar team.
     */
    
    /*     
    [Category("A* Pathfinding Pro/RVOController")]
    [Name("Set Wall Avoid Force")]
    [Description("Sets the wall avoid force for the RVOController")]
    [ParadoxNotion.Design.Icon("PathfindingNWaypoint")]
    public class SetWallAvoidForceAction : ActionTask<RVOController>
    {
        [RequiredField]
        [BlackboardOnly]
        public BBParameter<float> WallAvoidForce;

        [GetFromAgent]
        private RVOController _rvoController;

        protected override void OnExecute()
        {
            _rvoController.wallAvoidForce = WallAvoidForce.value;
            EndAction(true);
        }
    }*/
}