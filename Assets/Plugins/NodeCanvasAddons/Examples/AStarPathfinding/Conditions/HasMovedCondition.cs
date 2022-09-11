using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvasAddons.AStarPathfinding.Examples
{
    [Category("GameObject")]
    [Name("Has Moved")]
    public class HasMovedCondition : ConditionTask
    {
        [RequiredField]
        public BBParameter<Vector3> PreviousPosition;

        [RequiredField]
        public BBParameter<Vector3> CurrentPosition;

        protected override bool OnCheck()
        {
            return PreviousPosition.value != CurrentPosition.value;
        }
    }
}