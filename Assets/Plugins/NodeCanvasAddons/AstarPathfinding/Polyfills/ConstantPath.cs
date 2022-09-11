//#define IS_ASTAR_FREE

#if IS_ASTAR_FREE
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
	public class ConstantPath : Path
	{	
		public static ConstantPath Construct (Vector3 start, int maxGScore, OnPathDelegate callback)
        { return null; }

	    public List<GraphNode> allNodes;


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