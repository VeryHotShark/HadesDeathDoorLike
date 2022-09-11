using System.Collections.Generic;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;
using UnityEngine;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Paths")]
    [Name("Get Vectors From Path")]
    [Description("Get vectors from an existing path")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class GetVectorsFromPathAction : ActionTask
    {
        [RequiredField]
        [BlackboardOnly]
        public BBParameter<Path> Path;

        [BlackboardOnly]
        public BBParameter<List<Vector3>> VectorList = new BBParameter<List<Vector3>>();

        protected override void OnExecute()
        {
            if (Path.isNone || Path.isNull)
            { EndAction(false); }

            VectorList.value = Path.value.vectorPath;
            EndAction(true);
        }
    }
}