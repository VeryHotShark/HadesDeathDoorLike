using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;
using UnityEngine;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Rich AI")]
    [Name("Set Ground Mask")]
    [Description("Sets the ground mask from the underlying Rich AI agent")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class SetGroundMaskAction : ActionTask<RichAI>
    {
        [RequiredField]
        [BlackboardOnly]
        public BBParameter<LayerMask> LayerMask;

        [GetFromAgent]
        private RichAI _richAI = default;

        protected override void OnExecute()
        {
            _richAI.groundMask = LayerMask.value;
            EndAction(true);
        }
    }
}