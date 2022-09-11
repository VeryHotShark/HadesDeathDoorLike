using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Graphs")]
    [Name("Update Graphs")]
    [Description("Updates graphs nodes within a given boundry")]
    [ParadoxNotion.Design.Icon("PathfindingUpdate")]
    public class UpdateGraphsAction : ActionTask
    {
        [RequiredField]
        public BBParameter<Bounds> UpdateBoundry;

        protected override string info
        {
            get { return string.Format("Update Graph Boundry {0}", UpdateBoundry); }
        }

        protected override void OnExecute()
        {
            AstarPath.active.UpdateGraphs(UpdateBoundry.value);
            EndAction(true);
        }
    }
}