using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace KinematicCharacterController.Walkthrough.ClimbingLadders
{
    public class MyLadder : MonoBehaviour
    {
        // Ladder segment
        [FormerlySerializedAs("LadderSegmentBottom")] public Vector3 _ladderSegmentBottom;
        [FormerlySerializedAs("LadderSegmentLength")] public float _ladderSegmentLength;

        // Points to move to when reaching one of the extremities and moving off of the ladder
        [FormerlySerializedAs("BottomReleasePoint")] public Transform _bottomReleasePoint;
        [FormerlySerializedAs("TopReleasePoint")] public Transform _topReleasePoint;

        // Gets the position of the bottom point of the ladder segment
        public Vector3 BottomAnchorPoint
        {
            get
            {
                return transform.position + transform.TransformVector(_ladderSegmentBottom);
            }
        }

        // Gets the position of the top point of the ladder segment
        public Vector3 TopAnchorPoint
        {
            get
            {
                return transform.position + transform.TransformVector(_ladderSegmentBottom) + (transform.up * _ladderSegmentLength);
            }
        }

        public Vector3 ClosestPointOnLadderSegment(Vector3 fromPoint, out float onSegmentState)
        {
            Vector3 segment = TopAnchorPoint - BottomAnchorPoint;            
            Vector3 segmentPoint1ToPoint = fromPoint - BottomAnchorPoint;
            float pointProjectionLength = Vector3.Dot(segmentPoint1ToPoint, segment.normalized);

            // When higher than bottom point
            if(pointProjectionLength > 0)
            {
                // If we are not higher than top point
                if (pointProjectionLength <= segment.magnitude)
                {
                    onSegmentState = 0;
                    return BottomAnchorPoint + (segment.normalized * pointProjectionLength);
                }
                // If we are higher than top point
                else
                {
                    onSegmentState = pointProjectionLength - segment.magnitude;
                    return TopAnchorPoint;
                }
            }
            // When lower than bottom point
            else
            {
                onSegmentState = pointProjectionLength;
                return BottomAnchorPoint;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(BottomAnchorPoint, TopAnchorPoint);
        }
    }
}