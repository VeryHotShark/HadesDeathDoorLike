//#define IS_ASTAR_FREE


#if IS_ASTAR_FREE
using UnityEngine;
namespace Pathfinding
{
    public class RandomPath : Path
    {
        public static RandomPath Construct(Vector3 position, int length, OnPathDelegate callback)
        {return null;}

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