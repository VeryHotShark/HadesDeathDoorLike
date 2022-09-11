using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;
using UnityEngine;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Rich AI")]
    [Name("Get Ground Mask")]
    [Description("Gets the ground mask from the underlying Rich AI agent")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class GetGroundMaskAction : ActionTask<RichAI>
    {
        [RequiredField]
        [BlackboardOnly]
        public BBParameter<LayerMask> LayerMask;

        [GetFromAgent]
        private RichAI _richAI = default;

        protected override void OnExecute()
        {
            LayerMask.value = _richAI.groundMask;
            EndAction(true);
        }
    }
}