using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvasAddons
{
    [Category("GameObject")]
    [Name("Get Bounds From Game Object")]
    [Description("Get the bounds from a game object, mainly used for isolating graph updates")]
    [ParadoxNotion.Design.Icon("PathfindingGraph")]
    public class GetBoundsFromGameObjectAction : ActionTask<Collider>
    {
        [BlackboardOnly]
        public BBParameter<Bounds> Bounds = new BBParameter<Bounds>();

        [GetFromAgent]
        private Collider _collider = default;

        protected override void OnExecute()
        {
            Bounds.value = _collider.bounds;
            EndAction(true);
        }
    }
}

