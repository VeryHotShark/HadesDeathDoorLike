//#define IS_ASTAR_FREE

#if IS_ASTAR_FREE
using UnityEngine;

namespace Pathfinding.RVO
{
    public enum RVOLayer {}

    public class RVOController : MonoBehaviour
    {
        public float radius;
        public float maxSpeed;
        public float height;
        public bool locked;
        public bool lockWhenNotMoving;
        public float agentTimeHorizon;
        public float obstacleTimeHorizon;
        public float neighbourDist;
        public int maxNeighbours;
        public LayerMask mask;
        public RVOLayer layer;
        public RVOLayer collidesWith;
        public float wallAvoidForce;
        public float wallAvoidFalloff;
        public float center;
        public bool enableRotation;
        public float rotationSpeed;
        public bool debug;
        public MovementPlane movementPlane;
        private Vector3 lastPosition;
        public Vector3 position;
        public Vector3 velocity;
        public void OnDisable() {}
        public void Awake() {}
        public void OnEnable() {}
        public void Teleport(Vector3 pos) {}
        public void Move(Vector3 pos) {}
        public Vector3 CalculateMovementDelta(params object[] data) { return Vector3.zero; }
        public void SetTarget(params object[] data) {}
        public void Update() {}
        public void OnDrawGizmos() {}
    }

    public class MovementPlane
    {
        
    }
}
#endif