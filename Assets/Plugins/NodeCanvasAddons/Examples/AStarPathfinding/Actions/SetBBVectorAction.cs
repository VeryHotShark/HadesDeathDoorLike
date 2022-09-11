using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvasAddons.AStarPathfinding.Examples
{
    [Category("Physics")]
    [Name("Set Vector")]
    public class SetVectorAction : ActionTask
    {
        [RequiredField]
        public BBParameter<Vector3> VectorToSet;

        [RequiredField]
        public BBParameter<Vector3> VectorValue;

        protected override void OnExecute()
        {
            VectorToSet.value = VectorValue.value;
            EndAction(true);
        }
    }
}