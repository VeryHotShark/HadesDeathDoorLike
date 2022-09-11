using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;
using UnityEngine;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Paths")]
    [Name("Get Vector From Path By Index")]
    [Description("Gets a vector from a path by index")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class GetVectorFromPathByIndexAction : ActionTask
    {
        [RequiredField]
        [BlackboardOnly]
        public BBParameter<Path> Path;

        [RequiredField]
        public BBParameter<int> Index;

        [BlackboardOnly]
        public BBParameter<Vector3> Vector = new BBParameter<Vector3>();

        protected override void OnExecute()
        {
            if (Path.isNone || Path.isNull)
            { EndAction(false); }

            if (Path.value.path.Count >= Index.value)
            { EndAction(false); }

            Vector.value = Path.value.vectorPath[Index.value];
            EndAction(true);
        }
    }
}