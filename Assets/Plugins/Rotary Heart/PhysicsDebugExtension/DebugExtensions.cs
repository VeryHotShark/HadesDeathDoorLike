using RotaryHeart.Lib.UnityGLDebug;
using UnityEngine;

namespace RotaryHeart.Lib.PhysicsExtension
{
    /// <summary>
    /// Class used to draw additional debugs, this was based of the Debug Drawing Extension from the asset store (https://www.assetstore.unity3d.com/en/#!/content/11396)
    /// </summary>
    internal static partial class DebugExtensions
    {
        internal static void DebugSquare(Vector3 origin, Vector3 halfExtents, Color color, Quaternion orientation,
            float drawDuration = 0, PreviewCondition preview = PreviewCondition.Editor, bool drawDepth = false)
        {
            Vector3 forward = orientation * Vector3.forward;
            Vector3 up = orientation * Vector3.up;
            Vector3 right = orientation * Vector3.right;

            Vector3 topMinY1 = origin + (right * halfExtents.x) + (up * halfExtents.y) + (forward * halfExtents.z);
            Vector3 topMaxY1 = origin - (right * halfExtents.x) + (up * halfExtents.y) + (forward * halfExtents.z);
            Vector3 botMinY1 = origin + (right * halfExtents.x) - (up * halfExtents.y) + (forward * halfExtents.z);
            Vector3 botMaxY1 = origin - (right * halfExtents.x) - (up * halfExtents.y) + (forward * halfExtents.z);

            bool drawEditor = false;
            bool drawGame = false;

            switch (preview)
            {
                case PreviewCondition.Editor:
                    drawEditor = true;
                    break;

                case PreviewCondition.Game:
                    drawGame = true;
                    break;

                case PreviewCondition.Both:
                    drawEditor = true;
                    drawGame = true;
                    break;
            }

            if (drawEditor)
            {
                Debug.DrawLine(topMinY1, botMinY1, color, drawDuration, drawDepth);
                Debug.DrawLine(topMaxY1, botMaxY1, color, drawDuration, drawDepth);
                Debug.DrawLine(topMinY1, topMaxY1, color, drawDuration, drawDepth);
                Debug.DrawLine(botMinY1, botMaxY1, color, drawDuration, drawDepth);
            }

            if (drawGame)
            {
                GLDebug.DrawLine(topMinY1, botMinY1, color, drawDuration, drawDepth);
                GLDebug.DrawLine(topMaxY1, botMaxY1, color, drawDuration, drawDepth);
                GLDebug.DrawLine(topMinY1, topMaxY1, color, drawDuration, drawDepth);
                GLDebug.DrawLine(botMinY1, botMaxY1, color, drawDuration, drawDepth);
            }
        }

        internal static void DebugBox(Vector3 origin, Vector3 halfExtents, Vector3 direction, float maxDistance, Color color,
            Quaternion orientation, Color endColor, bool drawBase = true, float drawDuration = 0,
            PreviewCondition preview = PreviewCondition.Editor, bool drawDepth = false)
        {
            Vector3 end = origin + direction * (float.IsPositiveInfinity(maxDistance) ? 1000 * 1000 : maxDistance);

            Vector3 forward = orientation * Vector3.forward;
            Vector3 up = orientation * Vector3.up;
            Vector3 right = orientation * Vector3.right;

            #region Coords

            #region End coords

            //trans.position = end;
            Vector3 topMinX0 = end + (right * halfExtents.x) + (up * halfExtents.y) - (forward * halfExtents.z);
            Vector3 topMaxX0 = end - (right * halfExtents.x) + (up * halfExtents.y) - (forward * halfExtents.z);
            Vector3 topMinY0 = end + (right * halfExtents.x) + (up * halfExtents.y) + (forward * halfExtents.z);
            Vector3 topMaxY0 = end - (right * halfExtents.x) + (up * halfExtents.y) + (forward * halfExtents.z);

            Vector3 botMinX0 = end + (right * halfExtents.x) - (up * halfExtents.y) - (forward * halfExtents.z);
            Vector3 botMaxX0 = end - (right * halfExtents.x) - (up * halfExtents.y) - (forward * halfExtents.z);
            Vector3 botMinY0 = end + (right * halfExtents.x) - (up * halfExtents.y) + (forward * halfExtents.z);
            Vector3 botMaxY0 = end - (right * halfExtents.x) - (up * halfExtents.y) + (forward * halfExtents.z);

            #endregion

            #region Origin coords

            //trans.position = origin;
            Vector3 topMinX1 = origin + (right * halfExtents.x) + (up * halfExtents.y) - (forward * halfExtents.z);
            Vector3 topMaxX1 = origin - (right * halfExtents.x) + (up * halfExtents.y) - (forward * halfExtents.z);
            Vector3 topMinY1 = origin + (right * halfExtents.x) + (up * halfExtents.y) + (forward * halfExtents.z);
            Vector3 topMaxY1 = origin - (right * halfExtents.x) + (up * halfExtents.y) + (forward * halfExtents.z);

            Vector3 botMinX1 = origin + (right * halfExtents.x) - (up * halfExtents.y) - (forward * halfExtents.z);
            Vector3 botMaxX1 = origin - (right * halfExtents.x) - (up * halfExtents.y) - (forward * halfExtents.z);
            Vector3 botMinY1 = origin + (right * halfExtents.x) - (up * halfExtents.y) + (forward * halfExtents.z);
            Vector3 botMaxY1 = origin - (right * halfExtents.x) - (up * halfExtents.y) + (forward * halfExtents.z);

            #endregion

            #endregion

            #region Draw lines

            bool drawEditor = false;
            bool drawGame = false;

            switch (preview)
            {
                case PreviewCondition.Editor:
                    drawEditor = true;
                    break;

                case PreviewCondition.Game:
                    drawGame = true;
                    break;

                case PreviewCondition.Both:
                    drawEditor = true;
                    drawGame = true;
                    break;
            }

            if (drawEditor)
            {
                #region Origin box

                if (drawBase)
                {
                    Debug.DrawLine(topMinX1, botMinX1, color, drawDuration, drawDepth);
                    Debug.DrawLine(topMaxX1, botMaxX1, color, drawDuration, drawDepth);
                    Debug.DrawLine(topMinY1, botMinY1, color, drawDuration, drawDepth);
                    Debug.DrawLine(topMaxY1, botMaxY1, color, drawDuration, drawDepth);

                    Debug.DrawLine(topMinX1, topMaxX1, color, drawDuration, drawDepth);
                    Debug.DrawLine(topMinX1, topMinY1, color, drawDuration, drawDepth);
                    Debug.DrawLine(topMinY1, topMaxY1, color, drawDuration, drawDepth);
                    Debug.DrawLine(topMaxY1, topMaxX1, color, drawDuration, drawDepth);

                    Debug.DrawLine(botMinX1, botMaxX1, color, drawDuration, drawDepth);
                    Debug.DrawLine(botMinX1, botMinY1, color, drawDuration, drawDepth);
                    Debug.DrawLine(botMinY1, botMaxY1, color, drawDuration, drawDepth);
                    Debug.DrawLine(botMaxY1, botMaxX1, color, drawDuration, drawDepth);
                }

                #endregion

                #region Connection between boxes

                Debug.DrawLine(topMinX0, topMinX1, color, drawDuration, drawDepth);
                Debug.DrawLine(topMaxX0, topMaxX1, color, drawDuration, drawDepth);
                Debug.DrawLine(topMinY0, topMinY1, color, drawDuration, drawDepth);
                Debug.DrawLine(topMaxY0, topMaxY1, color, drawDuration, drawDepth);

                Debug.DrawLine(botMinX0, botMinX1, color, drawDuration, drawDepth);
                Debug.DrawLine(botMinX0, botMinX1, color, drawDuration, drawDepth);
                Debug.DrawLine(botMinY0, botMinY1, color, drawDuration, drawDepth);
                Debug.DrawLine(botMaxY0, botMaxY1, color, drawDuration, drawDepth);

                #endregion

                #region End box

                color = endColor;

                Debug.DrawLine(topMinX0, botMinX0, color, drawDuration, drawDepth);
                Debug.DrawLine(topMaxX0, botMaxX0, color, drawDuration, drawDepth);
                Debug.DrawLine(topMinY0, botMinY0, color, drawDuration, drawDepth);
                Debug.DrawLine(topMaxY0, botMaxY0, color, drawDuration, drawDepth);

                Debug.DrawLine(topMinX0, topMaxX0, color, drawDuration, drawDepth);
                Debug.DrawLine(topMinX0, topMinY0, color, drawDuration, drawDepth);
                Debug.DrawLine(topMinY0, topMaxY0, color, drawDuration, drawDepth);
                Debug.DrawLine(topMaxY0, topMaxX0, color, drawDuration, drawDepth);

                Debug.DrawLine(botMinX0, botMaxX0, color, drawDuration, drawDepth);
                Debug.DrawLine(botMinX0, botMinY0, color, drawDuration, drawDepth);
                Debug.DrawLine(botMinY0, botMaxY0, color, drawDuration, drawDepth);
                Debug.DrawLine(botMaxY0, botMaxX0, color, drawDuration, drawDepth);

                #endregion
            }

            if (drawGame)
            {
                #region Origin box

                if (drawBase)
                {
                    GLDebug.DrawLine(topMinX1, botMinX1, color, drawDuration, drawDepth);
                    GLDebug.DrawLine(topMaxX1, botMaxX1, color, drawDuration, drawDepth);
                    GLDebug.DrawLine(topMinY1, botMinY1, color, drawDuration, drawDepth);
                    GLDebug.DrawLine(topMaxY1, botMaxY1, color, drawDuration, drawDepth);

                    GLDebug.DrawLine(topMinX1, topMaxX1, color, drawDuration, drawDepth);
                    GLDebug.DrawLine(topMinX1, topMinY1, color, drawDuration, drawDepth);
                    GLDebug.DrawLine(topMinY1, topMaxY1, color, drawDuration, drawDepth);
                    GLDebug.DrawLine(topMaxY1, topMaxX1, color, drawDuration, drawDepth);

                    GLDebug.DrawLine(botMinX1, botMaxX1, color, drawDuration, drawDepth);
                    GLDebug.DrawLine(botMinX1, botMinY1, color, drawDuration, drawDepth);
                    GLDebug.DrawLine(botMinY1, botMaxY1, color, drawDuration, drawDepth);
                    GLDebug.DrawLine(botMaxY1, botMaxX1, color, drawDuration, drawDepth);
                }

                #endregion

                #region Connection between boxes

                GLDebug.DrawLine(topMinX0, topMinX1, color, drawDuration, drawDepth);
                GLDebug.DrawLine(topMaxX0, topMaxX1, color, drawDuration, drawDepth);
                GLDebug.DrawLine(topMinY0, topMinY1, color, drawDuration, drawDepth);
                GLDebug.DrawLine(topMaxY0, topMaxY1, color, drawDuration, drawDepth);

                GLDebug.DrawLine(botMinX0, botMinX1, color, drawDuration, drawDepth);
                GLDebug.DrawLine(botMinX0, botMinX1, color, drawDuration, drawDepth);
                GLDebug.DrawLine(botMinY0, botMinY1, color, drawDuration, drawDepth);
                GLDebug.DrawLine(botMaxY0, botMaxY1, color, drawDuration, drawDepth);

                #endregion

                #region End box

                color = endColor;

                GLDebug.DrawLine(topMinX0, botMinX0, color, drawDuration, drawDepth);
                GLDebug.DrawLine(topMaxX0, botMaxX0, color, drawDuration, drawDepth);
                GLDebug.DrawLine(topMinY0, botMinY0, color, drawDuration, drawDepth);
                GLDebug.DrawLine(topMaxY0, botMaxY0, color, drawDuration, drawDepth);

                GLDebug.DrawLine(topMinX0, topMaxX0, color, drawDuration, drawDepth);
                GLDebug.DrawLine(topMinX0, topMinY0, color, drawDuration, drawDepth);
                GLDebug.DrawLine(topMinY0, topMaxY0, color, drawDuration, drawDepth);
                GLDebug.DrawLine(topMaxY0, topMaxX0, color, drawDuration, drawDepth);

                GLDebug.DrawLine(botMinX0, botMaxX0, color, drawDuration, drawDepth);
                GLDebug.DrawLine(botMinX0, botMinY0, color, drawDuration, drawDepth);
                GLDebug.DrawLine(botMinY0, botMaxY0, color, drawDuration, drawDepth);
                GLDebug.DrawLine(botMaxY0, botMaxX0, color, drawDuration, drawDepth);

                #endregion
            }

            #endregion
        }

        internal static void DebugBoxCast(Vector3 origin, Vector3 halfExtents, Vector3 direction, float maxDistance, Color color, Quaternion orientation,
            float drawDuration = 0, CastDrawType drawType = CastDrawType.Minimal, PreviewCondition preview = PreviewCondition.Editor, bool drawDepth = false)
        {
            bool drawEditor = false;
            bool drawGame = false;

            switch (preview)
            {
                case PreviewCondition.Editor:
                    drawEditor = true;
                    break;

                case PreviewCondition.Game:
                    drawGame = true;
                    break;

                case PreviewCondition.Both:
                    drawEditor = true;
                    drawGame = true;
                    break;
            }

            if (drawEditor)
            {
                if (drawType == CastDrawType.Minimal)
                {
                    Debug.DrawLine(origin, origin + direction * maxDistance, color, drawDuration);
                }
                else
                {
                    Vector3 forward = orientation * Vector3.forward;
                    Vector3 up = orientation * Vector3.up;
                    Vector3 right = orientation * Vector3.right;

                    Vector3 topMinX1 = origin + (right * halfExtents.x) + (up * halfExtents.y) - (forward * halfExtents.z);
                    Vector3 topMaxX1 = origin - (right * halfExtents.x) + (up * halfExtents.y) - (forward * halfExtents.z);
                    Vector3 topMinY1 = origin + (right * halfExtents.x) + (up * halfExtents.y) + (forward * halfExtents.z);
                    Vector3 topMaxY1 = origin - (right * halfExtents.x) + (up * halfExtents.y) + (forward * halfExtents.z);

                    Vector3 botMinX1 = origin + (right * halfExtents.x) - (up * halfExtents.y) - (forward * halfExtents.z);
                    Vector3 botMaxX1 = origin - (right * halfExtents.x) - (up * halfExtents.y) - (forward * halfExtents.z);
                    Vector3 botMinY1 = origin + (right * halfExtents.x) - (up * halfExtents.y) + (forward * halfExtents.z);
                    Vector3 botMaxY1 = origin - (right * halfExtents.x) - (up * halfExtents.y) + (forward * halfExtents.z);

                    Debug.DrawLine(topMinX1, topMinX1 + direction * maxDistance, color, drawDuration);
                    Debug.DrawLine(topMaxX1, topMaxX1 + direction * maxDistance, color, drawDuration);
                    Debug.DrawLine(topMinY1, topMinY1 + direction * maxDistance, color, drawDuration);
                    Debug.DrawLine(topMaxY1, topMaxY1 + direction * maxDistance, color, drawDuration);
                    Debug.DrawLine(botMinX1, botMinX1 + direction * maxDistance, color, drawDuration);
                    Debug.DrawLine(botMaxX1, botMaxX1 + direction * maxDistance, color, drawDuration);
                    Debug.DrawLine(botMinY1, botMinY1 + direction * maxDistance, color, drawDuration);
                    Debug.DrawLine(botMaxY1, botMaxY1 + direction * maxDistance, color, drawDuration);
                }
            }

            if (drawGame)
            {
                if (drawType == CastDrawType.Minimal)
                {
                    GLDebug.DrawLine(origin, origin + direction * maxDistance, color, drawDuration, true);
                }
                else
                {
                    Vector3 forward = orientation * Vector3.forward;
                    Vector3 up = orientation * Vector3.up;
                    Vector3 right = orientation * Vector3.right;

                    Vector3 topMinX1 = origin + (right * halfExtents.x) + (up * halfExtents.y) - (forward * halfExtents.z);
                    Vector3 topMaxX1 = origin - (right * halfExtents.x) + (up * halfExtents.y) - (forward * halfExtents.z);
                    Vector3 topMinY1 = origin + (right * halfExtents.x) + (up * halfExtents.y) + (forward * halfExtents.z);
                    Vector3 topMaxY1 = origin - (right * halfExtents.x) + (up * halfExtents.y) + (forward * halfExtents.z);

                    Vector3 botMinX1 = origin + (right * halfExtents.x) - (up * halfExtents.y) - (forward * halfExtents.z);
                    Vector3 botMaxX1 = origin - (right * halfExtents.x) - (up * halfExtents.y) - (forward * halfExtents.z);
                    Vector3 botMinY1 = origin + (right * halfExtents.x) - (up * halfExtents.y) + (forward * halfExtents.z);
                    Vector3 botMaxY1 = origin - (right * halfExtents.x) - (up * halfExtents.y) + (forward * halfExtents.z);

                    GLDebug.DrawLine(topMinX1, topMinX1 + direction * maxDistance, color, drawDuration);
                    GLDebug.DrawLine(topMaxX1, topMaxX1 + direction * maxDistance, color, drawDuration);
                    GLDebug.DrawLine(topMinY1, topMinY1 + direction * maxDistance, color, drawDuration);
                    GLDebug.DrawLine(topMaxY1, topMaxY1 + direction * maxDistance, color, drawDuration);
                    GLDebug.DrawLine(botMinX1, botMinX1 + direction * maxDistance, color, drawDuration);
                    GLDebug.DrawLine(botMaxX1, botMaxX1 + direction * maxDistance, color, drawDuration);
                    GLDebug.DrawLine(botMinY1, botMinY1 + direction * maxDistance, color, drawDuration);
                    GLDebug.DrawLine(botMaxY1, botMaxY1 + direction * maxDistance, color, drawDuration);
                }
            }

            DebugBox(origin, halfExtents, Physics.M_castColor, orientation, drawDuration, preview, drawDepth);
            DebugBox(origin + direction * maxDistance, halfExtents, color, orientation, drawDuration, preview, drawDepth);
        }

        internal static void DebugBox(Vector3 origin, Vector3 halfExtents, Color color, Quaternion orientation,
            float drawDuration = 0, PreviewCondition preview = PreviewCondition.Editor, bool drawDepth = false)
        {
            Vector3 forward = orientation * Vector3.forward;
            Vector3 up = orientation * Vector3.up;
            Vector3 right = orientation * Vector3.right;

            Vector3 topMinX1 = origin + (right * halfExtents.x) + (up * halfExtents.y) - (forward * halfExtents.z);
            Vector3 topMaxX1 = origin - (right * halfExtents.x) + (up * halfExtents.y) - (forward * halfExtents.z);
            Vector3 topMinY1 = origin + (right * halfExtents.x) + (up * halfExtents.y) + (forward * halfExtents.z);
            Vector3 topMaxY1 = origin - (right * halfExtents.x) + (up * halfExtents.y) + (forward * halfExtents.z);

            Vector3 botMinX1 = origin + (right * halfExtents.x) - (up * halfExtents.y) - (forward * halfExtents.z);
            Vector3 botMaxX1 = origin - (right * halfExtents.x) - (up * halfExtents.y) - (forward * halfExtents.z);
            Vector3 botMinY1 = origin + (right * halfExtents.x) - (up * halfExtents.y) + (forward * halfExtents.z);
            Vector3 botMaxY1 = origin - (right * halfExtents.x) - (up * halfExtents.y) + (forward * halfExtents.z);

            bool drawEditor = false;
            bool drawGame = false;

            switch (preview)
            {
                case PreviewCondition.Editor:
                    drawEditor = true;
                    break;

                case PreviewCondition.Game:
                    drawGame = true;
                    break;

                case PreviewCondition.Both:
                    drawEditor = true;
                    drawGame = true;
                    break;
            }

            if (drawEditor)
            {
                Debug.DrawLine(topMinX1, botMinX1, color, drawDuration, drawDepth);
                Debug.DrawLine(topMaxX1, botMaxX1, color, drawDuration, drawDepth);
                Debug.DrawLine(topMinY1, botMinY1, color, drawDuration, drawDepth);
                Debug.DrawLine(topMaxY1, botMaxY1, color, drawDuration, drawDepth);

                Debug.DrawLine(topMinX1, topMaxX1, color, drawDuration, drawDepth);
                Debug.DrawLine(topMinX1, topMinY1, color, drawDuration, drawDepth);
                Debug.DrawLine(topMinY1, topMaxY1, color, drawDuration, drawDepth);
                Debug.DrawLine(topMaxY1, topMaxX1, color, drawDuration, drawDepth);

                Debug.DrawLine(botMinX1, botMaxX1, color, drawDuration, drawDepth);
                Debug.DrawLine(botMinX1, botMinY1, color, drawDuration, drawDepth);
                Debug.DrawLine(botMinY1, botMaxY1, color, drawDuration, drawDepth);
                Debug.DrawLine(botMaxY1, botMaxX1, color, drawDuration, drawDepth);
            }

            if (drawGame)
            {
                GLDebug.DrawLine(topMinX1, botMinX1, color, drawDuration, drawDepth);
                GLDebug.DrawLine(topMaxX1, botMaxX1, color, drawDuration, drawDepth);
                GLDebug.DrawLine(topMinY1, botMinY1, color, drawDuration, drawDepth);
                GLDebug.DrawLine(topMaxY1, botMaxY1, color, drawDuration, drawDepth);

                GLDebug.DrawLine(topMinX1, topMaxX1, color, drawDuration, drawDepth);
                GLDebug.DrawLine(topMinX1, topMinY1, color, drawDuration, drawDepth);
                GLDebug.DrawLine(topMinY1, topMaxY1, color, drawDuration, drawDepth);
                GLDebug.DrawLine(topMaxY1, topMaxX1, color, drawDuration, drawDepth);

                GLDebug.DrawLine(botMinX1, botMaxX1, color, drawDuration, drawDepth);
                GLDebug.DrawLine(botMinX1, botMinY1, color, drawDuration, drawDepth);
                GLDebug.DrawLine(botMinY1, botMaxY1, color, drawDuration, drawDepth);
                GLDebug.DrawLine(botMaxY1, botMaxX1, color, drawDuration, drawDepth);
            }
        }

        internal static void DebugOneSidedCapsuleCast(Vector3 baseSphere, Vector3 endSphere, Vector3 direction, float maxDistance,
            Color color, float radius = 1.0f, float drawDuration = 0, CastDrawType drawType = CastDrawType.Minimal,
            PreviewCondition preview = PreviewCondition.Editor, bool drawDepth = false)
        {
            bool drawEditor = false;
            bool drawGame = false;

            switch (preview)
            {
                case PreviewCondition.Editor:
                    drawEditor = true;
                    break;

                case PreviewCondition.Game:
                    drawGame = true;
                    break;

                case PreviewCondition.Both:
                    drawEditor = true;
                    drawGame = true;
                    break;
            }

            Vector3 midPoint = (baseSphere + endSphere) / 2;

            DebugOneSidedCapsule(baseSphere, endSphere, Physics.M_castColor, radius, true, drawDuration, preview, drawDepth);

            if (drawEditor)
            {
                if (drawType == CastDrawType.Minimal)
                {
                    Debug.DrawLine(midPoint, midPoint + direction * maxDistance, color,
                        drawDuration);
                }
                else
                {
                    Vector3 up = (endSphere - baseSphere).normalized;
                    if (up == Vector3.zero)
                        up = Vector3.up;
                    Vector3 forward = Vector3.Slerp(up, -up, 0.5f);
                    Vector3 right = Vector3.Cross(up, forward).normalized;

                    Debug.DrawLine(baseSphere + right * radius, baseSphere + right * radius + direction * maxDistance, color, drawDuration);
                    Debug.DrawLine(endSphere + right * radius, endSphere + right * radius + direction * maxDistance, color, drawDuration);

                    Debug.DrawLine(baseSphere - right * radius, baseSphere - right * radius + direction * maxDistance, color, drawDuration);
                    Debug.DrawLine(endSphere - right * radius, endSphere - right * radius + direction * maxDistance, color, drawDuration);

                    Debug.DrawLine(endSphere + up * radius, endSphere + up * radius + direction * maxDistance, color, drawDuration);
                    Debug.DrawLine(baseSphere - up * radius, baseSphere - up * radius + direction * maxDistance, color, drawDuration);
                }
            }

            if (drawGame)
            {
                if (drawType == CastDrawType.Minimal)
                {
                    GLDebug.DrawLine(midPoint, midPoint + direction * maxDistance, color,
                        drawDuration, true);
                }
                else
                {
                    Vector3 up = (endSphere - baseSphere).normalized;
                    if (up == Vector3.zero)
                        up = Vector3.up;
                    Vector3 forward = Vector3.Slerp(up, -up, 0.5f);
                    Vector3 right = Vector3.Cross(up, forward).normalized;

                    GLDebug.DrawLine(baseSphere + right * radius, baseSphere + right * radius + direction * maxDistance, color, drawDuration);
                    GLDebug.DrawLine(endSphere + right * radius, endSphere + right * radius + direction * maxDistance, color, drawDuration);

                    GLDebug.DrawLine(baseSphere - right * radius, baseSphere - right * radius + direction * maxDistance, color, drawDuration);
                    GLDebug.DrawLine(endSphere - right * radius, endSphere - right * radius + direction * maxDistance, color, drawDuration);

                    GLDebug.DrawLine(endSphere + up * radius, endSphere + up * radius + direction * maxDistance, color, drawDuration);
                    GLDebug.DrawLine(baseSphere - up * radius, baseSphere - up * radius + direction * maxDistance, color, drawDuration);
                }
            }

            DebugOneSidedCapsule(baseSphere + direction * maxDistance, endSphere + direction * maxDistance, color, radius, true, drawDuration, preview,
                drawDepth);
        }

        internal static void DebugOneSidedCapsule(Vector3 baseSphere, Vector3 endSphere, Color color, float radius = 1,
            bool colorizeBase = false, float drawDuration = 0,
            PreviewCondition preview = PreviewCondition.Editor, bool drawDepth = false)
        {
            Vector3 up = (endSphere - baseSphere).normalized * radius;
            if (up == Vector3.zero)
                up = Vector3.up;
            Vector3 forward = Vector3.Slerp(up, -up, 0.5f);
            Vector3 right = Vector3.Cross(up, forward).normalized * radius;

            bool drawEditor = false;
            bool drawGame = false;

            switch (preview)
            {
                case PreviewCondition.Editor:
                    drawEditor = true;
                    break;

                case PreviewCondition.Game:
                    drawGame = true;
                    break;

                case PreviewCondition.Both:
                    drawEditor = true;
                    drawGame = true;
                    break;
            }

            if (drawEditor)
            {
                //Side lines
                Debug.DrawLine(baseSphere + right, endSphere + right, color, drawDuration, drawDepth);
                Debug.DrawLine(baseSphere - right, endSphere - right, color, drawDuration, drawDepth);

                //Draw end caps
                for (int i = 1; i < 26; i++)
                {
                    //Start endcap
                    Debug.DrawLine(Vector3.Slerp(right, -up, i / 25.0f) + baseSphere, Vector3.Slerp(right, -up, (i - 1) / 25.0f) + baseSphere,
                        colorizeBase ? color : Color.red, drawDuration, drawDepth);
                    Debug.DrawLine(Vector3.Slerp(-right, -up, i / 25.0f) + baseSphere, Vector3.Slerp(-right, -up, (i - 1) / 25.0f) + baseSphere,
                        colorizeBase ? color : Color.red, drawDuration, drawDepth);

                    //End endcap
                    Debug.DrawLine(Vector3.Slerp(right, up, i / 25.0f) + endSphere, Vector3.Slerp(right, up, (i - 1) / 25.0f) + endSphere, color, drawDuration,
                        drawDepth);
                    Debug.DrawLine(Vector3.Slerp(-right, up, i / 25.0f) + endSphere, Vector3.Slerp(-right, up, (i - 1) / 25.0f) + endSphere, color,
                        drawDuration, drawDepth);
                }
            }

            if (drawGame)
            {
                //Side lines
                GLDebug.DrawLine(baseSphere + right, endSphere + right, color, drawDuration, drawDepth);
                GLDebug.DrawLine(baseSphere - right, endSphere - right, color, drawDuration, drawDepth);

                //Draw end caps
                for (int i = 1; i < 26; i++)
                {
                    //Start endcap
                    GLDebug.DrawLine(Vector3.Slerp(right, -up, i / 25.0f) + baseSphere, Vector3.Slerp(right, -up, (i - 1) / 25.0f) + baseSphere,
                        colorizeBase ? color : Color.red, drawDuration, drawDepth);
                    GLDebug.DrawLine(Vector3.Slerp(-right, -up, i / 25.0f) + baseSphere, Vector3.Slerp(-right, -up, (i - 1) / 25.0f) + baseSphere,
                        colorizeBase ? color : Color.red, drawDuration, drawDepth);

                    //End endcap
                    GLDebug.DrawLine(Vector3.Slerp(right, up, i / 25.0f) + endSphere, Vector3.Slerp(right, up, (i - 1) / 25.0f) + endSphere, color,
                        drawDuration, drawDepth);
                    GLDebug.DrawLine(Vector3.Slerp(-right, up, i / 25.0f) + endSphere, Vector3.Slerp(-right, up, (i - 1) / 25.0f) + endSphere, color,
                        drawDuration, drawDepth);
                }
            }
        }

        internal static void DebugCapsuleCast(Vector3 baseSphere, Vector3 endSphere, Vector3 direction, float maxDistance,
            Color color, float radius = 1.0f, float drawDuration = 0, CastDrawType drawType = CastDrawType.Minimal,
            PreviewCondition preview = PreviewCondition.Editor, bool drawDepth = false)
        {
            bool drawEditor = false;
            bool drawGame = false;

            switch (preview)
            {
                case PreviewCondition.Editor:
                    drawEditor = true;
                    break;

                case PreviewCondition.Game:
                    drawGame = true;
                    break;

                case PreviewCondition.Both:
                    drawEditor = true;
                    drawGame = true;
                    break;
            }

            Vector3 midPoint = (baseSphere + endSphere) / 2;

            DebugCapsule(baseSphere, endSphere, Physics.M_castColor, radius, true, drawDuration, preview, drawDepth);

            if (drawEditor)
            {
                if (drawType == CastDrawType.Minimal)
                {
                    Debug.DrawLine(midPoint, midPoint + direction * maxDistance, color,
                        drawDuration);
                }
                else
                {
                    Vector3 up = (endSphere - baseSphere).normalized;
                    if (up == Vector3.zero)
                        up = Vector3.up;
                    Vector3 forward = Vector3.Slerp(up, -up, 0.5f);
                    Vector3 right = Vector3.Cross(up, forward).normalized;

                    Debug.DrawLine(baseSphere + right * radius, baseSphere + right * radius + direction * maxDistance, color, drawDuration);
                    Debug.DrawLine(endSphere + right * radius, endSphere + right * radius + direction * maxDistance, color, drawDuration);

                    Debug.DrawLine(baseSphere - right * radius, baseSphere - right * radius + direction * maxDistance, color, drawDuration);
                    Debug.DrawLine(endSphere - right * radius, endSphere - right * radius + direction * maxDistance, color, drawDuration);

                    Debug.DrawLine(baseSphere + forward * radius, baseSphere + forward * radius + direction * maxDistance, color, drawDuration);
                    Debug.DrawLine(endSphere + forward * radius, endSphere + forward * radius + direction * maxDistance, color, drawDuration);

                    Debug.DrawLine(baseSphere - forward * radius, baseSphere - forward * radius + direction * maxDistance, color, drawDuration);
                    Debug.DrawLine(endSphere - forward * radius, endSphere - forward * radius + direction * maxDistance, color, drawDuration);

                    Debug.DrawLine(endSphere + up * radius, endSphere + up * radius + direction * maxDistance, color, drawDuration);
                    Debug.DrawLine(baseSphere - up * radius, baseSphere - up * radius + direction * maxDistance, color, drawDuration);
                }
            }

            if (drawGame)
            {
                if (drawType == CastDrawType.Minimal)
                {
                    GLDebug.DrawLine(midPoint, midPoint + direction * maxDistance, color,
                        drawDuration, true);
                }
                else
                {
                    Vector3 up = (endSphere - baseSphere).normalized;
                    if (up == Vector3.zero)
                        up = Vector3.up;
                    Vector3 forward = Vector3.Slerp(up, -up, 0.5f);
                    Vector3 right = Vector3.Cross(up, forward).normalized;

                    GLDebug.DrawLine(baseSphere + right * radius, baseSphere + right * radius + direction * maxDistance, color, drawDuration);
                    GLDebug.DrawLine(endSphere + right * radius, endSphere + right * radius + direction * maxDistance, color, drawDuration);

                    GLDebug.DrawLine(baseSphere - right * radius, baseSphere - right * radius + direction * maxDistance, color, drawDuration);
                    GLDebug.DrawLine(endSphere - right * radius, endSphere - right * radius + direction * maxDistance, color, drawDuration);

                    GLDebug.DrawLine(baseSphere + forward * radius, baseSphere + forward * radius + direction * maxDistance, color, drawDuration);
                    GLDebug.DrawLine(endSphere + forward * radius, endSphere + forward * radius + direction * maxDistance, color, drawDuration);

                    GLDebug.DrawLine(baseSphere - forward * radius, baseSphere - forward * radius + direction * maxDistance, color, drawDuration);
                    GLDebug.DrawLine(endSphere - forward * radius, endSphere - forward * radius + direction * maxDistance, color, drawDuration);

                    GLDebug.DrawLine(endSphere + up * radius, endSphere + up * radius + direction * maxDistance, color, drawDuration);
                    GLDebug.DrawLine(baseSphere - up * radius, baseSphere - up * radius + direction * maxDistance, color, drawDuration);
                }
            }

            DebugCapsule(baseSphere + direction * maxDistance, endSphere + direction * maxDistance, color, radius, true, drawDuration, preview, drawDepth);
        }

        internal static void DebugCapsule(Vector3 baseSphere, Vector3 endSphere, Color color, float radius = 1,
            bool colorizeBase = true, float drawDuration = 0,
            PreviewCondition preview = PreviewCondition.Editor, bool drawDepth = false)
        {
            Vector3 up = (endSphere - baseSphere).normalized * radius;
            if (up == Vector3.zero)
                up = Vector3.up;
            Vector3 forward = Vector3.Slerp(up, -up, 0.5f);
            Vector3 right = Vector3.Cross(up, forward).normalized * radius;

            //Radial circles
            DebugCircle(baseSphere, up, colorizeBase ? color : Color.red, radius, drawDuration, preview, drawDepth);
            DebugCircle(endSphere, -up, color, radius, drawDuration, preview, drawDepth);

            bool drawEditor = false;
            bool drawGame = false;

            switch (preview)
            {
                case PreviewCondition.Editor:
                    drawEditor = true;
                    break;

                case PreviewCondition.Game:
                    drawGame = true;
                    break;

                case PreviewCondition.Both:
                    drawEditor = true;
                    drawGame = true;
                    break;
            }

            if (drawEditor)
            {
                //Side lines
                Debug.DrawLine(baseSphere + right, endSphere + right, color, drawDuration, drawDepth);
                Debug.DrawLine(baseSphere - right, endSphere - right, color, drawDuration, drawDepth);

                Debug.DrawLine(baseSphere + forward, endSphere + forward, color, drawDuration, drawDepth);
                Debug.DrawLine(baseSphere - forward, endSphere - forward, color, drawDuration, drawDepth);

                //Draw end caps
                for (int i = 1; i < 26; i++)
                {
                    //End endcap
                    Debug.DrawLine(Vector3.Slerp(right, up, i / 25.0f) + endSphere, Vector3.Slerp(right, up, (i - 1) / 25.0f) + endSphere, color, drawDuration,
                        drawDepth);
                    Debug.DrawLine(Vector3.Slerp(-right, up, i / 25.0f) + endSphere, Vector3.Slerp(-right, up, (i - 1) / 25.0f) + endSphere, color,
                        drawDuration, drawDepth);
                    Debug.DrawLine(Vector3.Slerp(forward, up, i / 25.0f) + endSphere, Vector3.Slerp(forward, up, (i - 1) / 25.0f) + endSphere, color,
                        drawDuration, drawDepth);
                    Debug.DrawLine(Vector3.Slerp(-forward, up, i / 25.0f) + endSphere, Vector3.Slerp(-forward, up, (i - 1) / 25.0f) + endSphere, color,
                        drawDuration, drawDepth);

                    //Start endcap
                    Debug.DrawLine(Vector3.Slerp(right, -up, i / 25.0f) + baseSphere, Vector3.Slerp(right, -up, (i - 1) / 25.0f) + baseSphere,
                        colorizeBase ? color : Color.red, drawDuration, drawDepth);
                    Debug.DrawLine(Vector3.Slerp(-right, -up, i / 25.0f) + baseSphere, Vector3.Slerp(-right, -up, (i - 1) / 25.0f) + baseSphere,
                        colorizeBase ? color : Color.red, drawDuration, drawDepth);
                    Debug.DrawLine(Vector3.Slerp(forward, -up, i / 25.0f) + baseSphere, Vector3.Slerp(forward, -up, (i - 1) / 25.0f) + baseSphere,
                        colorizeBase ? color : Color.red, drawDuration, drawDepth);
                    Debug.DrawLine(Vector3.Slerp(-forward, -up, i / 25.0f) + baseSphere, Vector3.Slerp(-forward, -up, (i - 1) / 25.0f) + baseSphere,
                        colorizeBase ? color : Color.red, drawDuration, drawDepth);
                }
            }

            if (drawGame)
            {
                //Side lines
                GLDebug.DrawLine(baseSphere + right, endSphere + right, color, drawDuration, drawDepth);
                GLDebug.DrawLine(baseSphere - right, endSphere - right, color, drawDuration, drawDepth);

                GLDebug.DrawLine(baseSphere + forward, endSphere + forward, color, drawDuration, drawDepth);
                GLDebug.DrawLine(baseSphere - forward, endSphere - forward, color, drawDuration, drawDepth);

                //Draw end caps
                for (int i = 1; i < 26; i++)
                {
                    //End endcap
                    GLDebug.DrawLine(Vector3.Slerp(right, up, i / 25.0f) + endSphere, Vector3.Slerp(right, up, (i - 1) / 25.0f) + endSphere, color,
                        drawDuration, drawDepth);
                    GLDebug.DrawLine(Vector3.Slerp(-right, up, i / 25.0f) + endSphere, Vector3.Slerp(-right, up, (i - 1) / 25.0f) + endSphere, color,
                        drawDuration, drawDepth);
                    GLDebug.DrawLine(Vector3.Slerp(forward, up, i / 25.0f) + endSphere, Vector3.Slerp(forward, up, (i - 1) / 25.0f) + endSphere, color,
                        drawDuration, drawDepth);
                    GLDebug.DrawLine(Vector3.Slerp(-forward, up, i / 25.0f) + endSphere, Vector3.Slerp(-forward, up, (i - 1) / 25.0f) + endSphere, color,
                        drawDuration, drawDepth);

                    //Start endcap
                    GLDebug.DrawLine(Vector3.Slerp(right, -up, i / 25.0f) + baseSphere, Vector3.Slerp(right, -up, (i - 1) / 25.0f) + baseSphere,
                        colorizeBase ? color : Color.red, drawDuration, drawDepth);
                    GLDebug.DrawLine(Vector3.Slerp(-right, -up, i / 25.0f) + baseSphere, Vector3.Slerp(-right, -up, (i - 1) / 25.0f) + baseSphere,
                        colorizeBase ? color : Color.red, drawDuration, drawDepth);
                    GLDebug.DrawLine(Vector3.Slerp(forward, -up, i / 25.0f) + baseSphere, Vector3.Slerp(forward, -up, (i - 1) / 25.0f) + baseSphere,
                        colorizeBase ? color : Color.red, drawDuration, drawDepth);
                    GLDebug.DrawLine(Vector3.Slerp(-forward, -up, i / 25.0f) + baseSphere, Vector3.Slerp(-forward, -up, (i - 1) / 25.0f) + baseSphere,
                        colorizeBase ? color : Color.red, drawDuration, drawDepth);
                }
            }
        }

        internal static void DebugCircleCast(Vector3 origin, Vector3 direction, float maxDistance, Color color, float radius, float drawDuration,
            CastDrawType drawType, PreviewCondition preview, bool drawDepth)
        {
            bool drawEditor = false;
            bool drawGame = false;

            switch (preview)
            {
                case PreviewCondition.Editor:
                    drawEditor = true;
                    break;

                case PreviewCondition.Game:
                    drawGame = true;
                    break;

                case PreviewCondition.Both:
                    drawEditor = true;
                    drawGame = true;
                    break;
            }

            DebugCircle(origin, Vector3.forward, Physics.M_castColor, radius, drawDuration, preview, drawDepth);

            if (drawEditor)
            {
                if (drawType == CastDrawType.Minimal)
                {
                    Debug.DrawLine(origin, origin + direction * maxDistance, color, drawDuration);
                }
                else
                {
                    Vector3 up = origin.normalized * radius;
                    if (up == Vector3.zero)
                        up = Vector3.up;
                    Vector3 forward = Vector3.Slerp(up, -up, 0.5f);
                    Vector3 right = Vector3.Cross(up, forward).normalized * radius;

                    Debug.DrawLine(origin + right * radius, origin + right * radius + direction * maxDistance, color, drawDuration);
                    Debug.DrawLine(origin - right * radius, origin - right * radius + direction * maxDistance, color, drawDuration);
                    Debug.DrawLine(origin + right * radius, origin + right * radius + direction * maxDistance, color, drawDuration);
                    Debug.DrawLine(origin + up * radius, origin + up * radius + direction * maxDistance, color, drawDuration);
                    Debug.DrawLine(origin - up * radius, origin - up * radius + direction * maxDistance, color, drawDuration);
                }
            }

            if (drawGame)
            {
                if (drawType == CastDrawType.Minimal)
                {
                    GLDebug.DrawLine(origin, origin + direction * maxDistance, color, drawDuration, true);
                }
                else
                {
                    Vector3 up = origin.normalized * radius;
                    if (up == Vector3.zero)
                        up = Vector3.up;
                    Vector3 forward = Vector3.Slerp(up, -up, 0.5f);
                    Vector3 right = Vector3.Cross(up, forward).normalized * radius;

                    GLDebug.DrawLine(origin + right * radius, origin + right * radius + direction * maxDistance, color, drawDuration);
                    GLDebug.DrawLine(origin - right * radius, origin - right * radius + direction * maxDistance, color, drawDuration);
                    Debug.DrawLine(origin + right * radius, origin + right * radius + direction * maxDistance, color, drawDuration);
                    GLDebug.DrawLine(origin + up * radius, origin + up * radius + direction * maxDistance, color, drawDuration);
                    GLDebug.DrawLine(origin - up * radius, origin - up * radius + direction * maxDistance, color, drawDuration);
                }
            }

            DebugCircle(origin + direction * maxDistance, Vector3.forward, color, radius, drawDuration, preview, drawDepth);
        }

        internal static void DebugCircle(Vector3 position, Vector3 up, Color color, float radius = 1.0f,
            float drawDuration = 0, PreviewCondition preview = PreviewCondition.Editor, bool drawDepth = false)
        {
            Vector3 upDir = up.normalized * radius;
            Vector3 forwardDir = Vector3.Slerp(upDir, -upDir, 0.5f);
            Vector3 rightDir = Vector3.Cross(upDir, forwardDir).normalized * radius;

            Matrix4x4 matrix = new Matrix4x4()
            {
                [0] = rightDir.x,
                [1] = rightDir.y,
                [2] = rightDir.z,

                [4] = upDir.x,
                [5] = upDir.y,
                [6] = upDir.z,

                [8] = forwardDir.x,
                [9] = forwardDir.y,
                [10] = forwardDir.z
            };

            Vector3 lastPoint = position + matrix.MultiplyPoint3x4(new Vector3(Mathf.Cos(0), 0, Mathf.Sin(0)));
            Vector3 nextPoint = Vector3.zero;

            color = (color == default(Color)) ? Color.white : color;

            bool drawEditor = false;
            bool drawGame = false;

            switch (preview)
            {
                case PreviewCondition.Editor:
                    drawEditor = true;
                    break;

                case PreviewCondition.Game:
                    drawGame = true;
                    break;

                case PreviewCondition.Both:
                    drawEditor = true;
                    drawGame = true;
                    break;
            }

            for (var i = 0; i < 91; i++)
            {
                nextPoint.x = Mathf.Cos((i * 4) * Mathf.Deg2Rad);
                nextPoint.z = Mathf.Sin((i * 4) * Mathf.Deg2Rad);
                nextPoint.y = 0;

                nextPoint = position + matrix.MultiplyPoint3x4(nextPoint);

                if (drawEditor)
                    Debug.DrawLine(lastPoint, nextPoint, color, drawDuration, drawDepth);

                if (drawGame)
                    GLDebug.DrawLine(lastPoint, nextPoint, color, drawDuration, drawDepth);

                lastPoint = nextPoint;
            }
        }

        internal static void DebugPoint(Vector3 position, Color color, float scale = 0.5f, float drawDuration = 0,
            PreviewCondition preview = PreviewCondition.Editor, bool drawDepth = false)
        {
            color = (color == default(Color)) ? Color.white : color;

            bool drawEditor = false;
            bool drawGame = false;

            switch (preview)
            {
                case PreviewCondition.Editor:
                    drawEditor = true;
                    break;

                case PreviewCondition.Game:
                    drawGame = true;
                    break;

                case PreviewCondition.Both:
                    drawEditor = true;
                    drawGame = true;
                    break;
            }

            if (drawEditor)
            {
                Debug.DrawRay(position + (Vector3.up * (scale * 0.5f)), -Vector3.up * scale, color, drawDuration, drawDepth);
                Debug.DrawRay(position + (Vector3.right * (scale * 0.5f)), -Vector3.right * scale, color, drawDuration, drawDepth);
                Debug.DrawRay(position + (Vector3.forward * (scale * 0.5f)), -Vector3.forward * scale, color, drawDuration, drawDepth);
            }

            if (drawGame)
            {
                GLDebug.DrawRay(position + (Vector3.up * (scale * 0.5f)), -Vector3.up * scale, color, drawDuration, drawDepth);
                GLDebug.DrawRay(position + (Vector3.right * (scale * 0.5f)), -Vector3.right * scale, color, drawDuration, drawDepth);
                GLDebug.DrawRay(position + (Vector3.forward * (scale * 0.5f)), -Vector3.forward * scale, color, drawDuration, drawDepth);
            }
        }

        internal static void DebugSphereCast(Vector3 origin, Vector3 direction, float maxDistance, Color color, float radius, float drawDuration,
            CastDrawType drawType, PreviewCondition preview, bool drawDepth)
        {
            bool drawEditor = false;
            bool drawGame = false;

            switch (preview)
            {
                case PreviewCondition.Editor:
                    drawEditor = true;
                    break;

                case PreviewCondition.Game:
                    drawGame = true;
                    break;

                case PreviewCondition.Both:
                    drawEditor = true;
                    drawGame = true;
                    break;
            }

            DebugWireSphere(origin, Physics.M_castColor, radius, drawDuration, preview, drawDepth);

            if (drawEditor)
            {
                if (drawType == CastDrawType.Minimal)
                {
                    Debug.DrawLine(origin, origin + direction * maxDistance, color, drawDuration);
                }
                else
                {
                    Vector3 up = origin.normalized * radius;
                    if (up == Vector3.zero)
                        up = Vector3.up;
                    Vector3 forward = Vector3.Slerp(up, -up, 0.5f);
                    Vector3 right = Vector3.Cross(up, forward).normalized * radius;

                    Debug.DrawLine(origin + right * radius, origin + right * radius + direction * maxDistance, color, drawDuration);
                    Debug.DrawLine(origin - right * radius, origin - right * radius + direction * maxDistance, color, drawDuration);
                    Debug.DrawLine(origin + right * radius, origin + right * radius + direction * maxDistance, color, drawDuration);
                    Debug.DrawLine(origin + up * radius, origin + up * radius + direction * maxDistance, color, drawDuration);
                    Debug.DrawLine(origin - up * radius, origin - up * radius + direction * maxDistance, color, drawDuration);
                }
            }

            if (drawGame)
            {
                if (drawType == CastDrawType.Minimal)
                {
                    GLDebug.DrawLine(origin, origin + direction * maxDistance, color, drawDuration, true);
                }
                else
                {
                    Vector3 up = origin.normalized * radius;
                    if (up == Vector3.zero)
                        up = Vector3.up;
                    Vector3 forward = Vector3.Slerp(up, -up, 0.5f);
                    Vector3 right = Vector3.Cross(up, forward).normalized * radius;

                    GLDebug.DrawLine(origin + right * radius, origin + right * radius + direction * maxDistance, color, drawDuration);
                    GLDebug.DrawLine(origin - right * radius, origin - right * radius + direction * maxDistance, color, drawDuration);
                    Debug.DrawLine(origin + right * radius, origin + right * radius + direction * maxDistance, color, drawDuration);
                    GLDebug.DrawLine(origin + up * radius, origin + up * radius + direction * maxDistance, color, drawDuration);
                    GLDebug.DrawLine(origin - up * radius, origin - up * radius + direction * maxDistance, color, drawDuration);
                }
            }

            DebugWireSphere(origin + direction * maxDistance, color, radius, drawDuration, preview, drawDepth);
        }

        internal static void DebugWireSphere(Vector3 position, Color color, float radius = 1.0f, float drawDuration = 0,
            PreviewCondition preview = PreviewCondition.Editor, bool drawDepth = false)
        {
            float angle = 10.0f;

            Vector3 x = new Vector3(position.x, position.y + radius * Mathf.Sin(0), position.z + radius * Mathf.Cos(0));
            Vector3 y = new Vector3(position.x + radius * Mathf.Cos(0), position.y, position.z + radius * Mathf.Sin(0));
            Vector3 z = new Vector3(position.x + radius * Mathf.Cos(0), position.y + radius * Mathf.Sin(0), position.z);

            bool drawEditor = false;
            bool drawGame = false;

            switch (preview)
            {
                case PreviewCondition.Editor:
                    drawEditor = true;
                    break;

                case PreviewCondition.Game:
                    drawGame = true;
                    break;

                case PreviewCondition.Both:
                    drawEditor = true;
                    drawGame = true;
                    break;
            }

            for (int i = 1; i < 37; i++)
            {
                Vector3 new_x = new Vector3(position.x, position.y + radius * Mathf.Sin(angle * i * Mathf.Deg2Rad),
                    position.z + radius * Mathf.Cos(angle * i * Mathf.Deg2Rad));
                Vector3 new_y = new Vector3(position.x + radius * Mathf.Cos(angle * i * Mathf.Deg2Rad), position.y,
                    position.z + radius * Mathf.Sin(angle * i * Mathf.Deg2Rad));
                Vector3 new_z = new Vector3(position.x + radius * Mathf.Cos(angle * i * Mathf.Deg2Rad),
                    position.y + radius * Mathf.Sin(angle * i * Mathf.Deg2Rad), position.z);

                if (drawEditor)
                {
                    Debug.DrawLine(x, new_x, color, drawDuration, drawDepth);
                    Debug.DrawLine(y, new_y, color, drawDuration, drawDepth);
                    Debug.DrawLine(z, new_z, color, drawDuration, drawDepth);
                }

                if (drawGame)
                {
                    GLDebug.DrawLine(x, new_x, color, drawDuration, drawDepth);
                    GLDebug.DrawLine(y, new_y, color, drawDuration, drawDepth);
                    GLDebug.DrawLine(z, new_z, color, drawDuration, drawDepth);
                }

                x = new_x;
                y = new_y;
                z = new_z;
            }
        }
    }
}