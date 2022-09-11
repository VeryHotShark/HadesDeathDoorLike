using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvasAddons.AStarPathfinding.Examples
{
    [Category("GameObject")]
    [Name("Parent Game Object")]
    public class ParentGameObjectAction : ActionTask<Transform>
    {
        [RequiredField]
        public BBParameter<GameObject> ChildObject;

        [RequiredField]
        public BBParameter<GameObject> ParentObject;

        protected override void OnExecute()
        {
            ChildObject.value.transform.parent = ParentObject.value.transform;
            EndAction(true);
        }
    }
}