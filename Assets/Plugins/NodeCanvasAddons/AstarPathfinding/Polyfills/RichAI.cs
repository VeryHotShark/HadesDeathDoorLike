//#define IS_ASTAR_FREE

#if IS_ASTAR_FREE
using UnityEngine;

namespace Pathfinding
{
    public class RichAI : MonoBehaviour
    {
        public Transform target;
        public bool repeatedlySearchPaths;
        public float repathRate;
        public float maxSpeed;
        public float acceleration;
        public float slowdownTime;
        public float rotationSpeed;
        public float endReachedDistance;
        public float wallForce;
        public float wallDist;
        public Vector3 gravity;
        public bool raycastingForGroundPlacement;
        public LayerMask groundMask;
        public float centerOffset;
        public bool funnelSimplification;
        public bool reachedEndOfPath;
        public bool reachedDestination;
        public Animation anim;
        public bool preciseSlowdown;
        public bool slowWhenNotFacingTarget;
        public Vector3 velocity;
        public Vector3 destination;

       public bool ApproachingPartEndpoint { get; private set; }
       public bool ApproachingPathEndpoint { get; private set; }
       public float DistanceToNextWaypoint { get; private set; }
       public Vector3 TargetPoint { get; private set; }
       public bool TraversingSpecial { get; private set; }
       public Vector3 Velocity { get; private set; }
       public bool canSearch { get; set; }
       public float height { get; set; }
       public bool approachingPartEndpoint { get; set; }
       public bool traversingOffMeshLink { get; set; }
       public Vector3 steeringTarget { get; set; }
       public Vector3 destination { get; set; }
       public bool approachingPathEndpoint { get; set; }

       public void UpdatePath() { throw new System.NotImplementedException(); }
       public void SearchPath() { throw new System.NotImplementedException(); }
    }

	public class RichFunnel 
    {
		public enum FunnelSimplification {}
    }
}

#endif