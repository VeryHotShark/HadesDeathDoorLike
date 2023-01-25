using System;
using UnityEngine;

namespace VHS {
    public static class Extension_Vector {
        /// <summary>
        /// returns random between given range
        /// </summary>
        /// <param name="range">range used for Min Max</param>
        /// <returns></returns>
        public static float Random(this Vector2 range) {
            return UnityEngine.Random.Range(range.x, range.y);
        }
        
        public static Vector3 SetX(this Vector3 vector3, float newX) {
            vector3.x = newX;
            return vector3;
        }

        public static Vector3 SetY(this Vector3 vector3, float newY) {
            vector3.y = newY;
            return vector3;
        }

        public static Vector3 SetZ(this Vector3 vector3, float newZ) {
            vector3.z = newZ;
            return vector3;
        }

        public static Vector3 With(this Vector3 vector3, float? x = null, float? y = null, float? z = null) => new Vector3(x ?? vector3.x, y ?? vector3.y, z ?? vector3.z);

        /// <summary>
        /// Set Y to 0.0f and normalize
        /// </summary>
        public static Vector3 Flatten(this Vector3 vector3) {
            return vector3.SetY(0.0f).normalized;
        }

        public static Vector3 DirectionTo(this Vector3 origin, Vector3 destination) {
            return (destination - origin).normalized;
        }

        public static Vector3 QuadraticLerp(Vector3 a, Vector3 b, Vector3 c, float t) {
            Vector3 ab = Vector3.Lerp(a, b, t);
            Vector3 bc = Vector3.Lerp(b, c, t);

            return Vector3.Lerp(ab, bc, t);
        }

        public static Vector3 CubicLerp(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t) {
            Vector3 ab_bc = QuadraticLerp(a, b, c, t);
            Vector3 bc_cd = QuadraticLerp(b, c, d, t);

            return Vector3.Lerp(ab_bc, bc_cd, t);
        }

        public static float DistanceSquaredTo(this Vector3 a, Vector3 b) {
            return (a - b).sqrMagnitude;
        }

        /// <summary>
        /// Returns -1 if point is to the left of forward direction, 1 if it's on the right, 0 if it's on the forward/backward direction
        /// </summary>
        public static float DirectionRelativeSideTo(this Vector3 direction, Vector3 forward, Vector3 up) {
            Vector3 cross = Vector3.Cross(forward, direction);
            float dot = Vector3.Dot(cross, up);
            return dot != 0f ? Mathf.Sign(dot) : 0f;
        }

        public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles) {
            return Quaternion.Euler(angles) * (point - pivot) + pivot;
        }

        public static Vector3 GetClosestPointOnFiniteLine(Vector3 point, Vector3 line_start, Vector3 line_end) {
            Vector3 line_direction = line_end - line_start;
            float line_length = line_direction.magnitude;
            line_direction.Normalize();
            float project_length = Mathf.Clamp(Vector3.Dot(point - line_start, line_direction), 0f, line_length);
            return line_start + line_direction * project_length;
        }

        public static Vector3 GetClosestPointOnInfiniteLine(Vector3 point, Vector3 line_start, Vector3 line_end) {
            return line_start + Vector3.Project(point - line_start, line_end - line_start);
        }

        public static Vector3 GetMaxVector(this Vector3 vector3) {
            if (vector3.x >= vector3.y && vector3.x >= vector3.z)
                return new Vector3(vector3.x, 0f, 0f);
            else if (vector3.y >= vector3.x && vector3.y >= vector3.z)
                return new Vector3(0f, vector3.y, 0f);
            else
                return new Vector3(0f, 0f, vector3.z);
        }

        public static Vector3 Multiply(this Vector3 v1, Vector3 v2) {
            return new Vector3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
        }

        public static Vector3 Absolute(this Vector3 v1) {
            return new Vector3(Mathf.Abs(v1.x), Mathf.Abs(v1.y), Mathf.Abs(v1.z));
        }

        public static float Min(this Vector3 vector3) {
            return Mathf.Min(Mathf.Min(vector3.x, vector3.y), vector3.z);
        }

        public static float Mid(this Vector3 vector3) {
            float[] values = { vector3.x, vector3.y, vector3.z };
            Array.Sort(values);
            return values[1];
        }

        public static float Max(this Vector3 vector3) {
            return Mathf.Max(Mathf.Max(vector3.x, vector3.y), vector3.z);
        }

        public static float MaxAbsolute(this Vector3 vector3) {
            float[] values = { vector3.x, vector3.y, vector3.z };
            float maxValue = float.MinValue;
            float currentValue = 0.0f;

            foreach (float value in values)
                if (Mathf.Abs(value) > maxValue) {
                    maxValue = Mathf.Abs(value);
                    currentValue = value;
                }

            return currentValue;
        }

        /// <summary>
        /// Quaternion.LookRotation but with 0,0,0 vector checking
        /// </summary>
        public static Quaternion Rotation(this Vector3 vector3) {
            if (vector3.sqrMagnitude > 0.0f)
                return Quaternion.LookRotation(vector3);

            return Quaternion.identity;
        }
    }
}