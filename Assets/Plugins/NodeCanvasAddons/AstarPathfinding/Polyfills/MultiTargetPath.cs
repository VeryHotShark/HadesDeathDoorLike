//#define IS_ASTAR_FREE


#if IS_ASTAR_FREE
using System.Collections.Generic;
using UnityEngine;
namespace Pathfinding
{
    public class MultiTargetPath : Path
    {	
	    public static MultiTargetPath Construct(Vector3[] startPoints, Vector3 target, OnPathDelegate[] callbackDelegates, OnPathDelegate callback)
        { return null; }

	    public static MultiTargetPath Construct (Vector3 start, Vector3[] targets, OnPathDelegate[] callbackDelegates, OnPathDelegate callback)
        { return null; }

        protected override void Prepare()
        {
            throw new System.NotImplementedException();
        }

        protected override void Initialize()
        {
            throw new System.NotImplementedException();
        }

        protected override void CalculateStep(long targetTick)
        {
            throw new System.NotImplementedException();
        }
    }
}
#endif