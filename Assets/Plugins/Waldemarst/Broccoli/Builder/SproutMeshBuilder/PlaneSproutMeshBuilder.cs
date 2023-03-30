using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Broccoli.Builder
{
    public class PlaneSproutMeshBuilder : BaseSproutMeshBuilder
    {
        #region Vars
        public float width = 1f;
        public float height = 1f;
        public float widthPivot = 0.5f;
        public float heightPivot = 0f;
        [Range(1,3)]
        public int planes = 1;
        public Dictionary<Hash128, Mesh> _planeMeshes = new Dictionary<Hash128, Mesh> ();
        private static float _uvX = 0;
        private static float _uvY = 0f;
        private static float _uvWidth = 1f;
        private static float _uvHeight = 1f;
        private static int _uvStep = 0;
        private static Vector4 _tangent = new Vector4 (1f, 0f, 0f, -1f);
        private static int _id = 0;
        private static float _pivotAdaptativeThreshold = 0.1f;
        private static int PIVOT_MODE_NONE = 0;
        private static int PIVOT_MODE_CORNER = 1;
        private static int PIVOT_MODE_SIDE = 2;
        private static int PIVOT_MODE_CENTER = 3;
        
        #endregion

        #region Abstract
        public override void SetParams (string jsonParams){
            throw new System.NotImplementedException();
        }
        public override Mesh GetMesh () {
            Hash128 hash = GetMeshHash (width, height, widthPivot, heightPivot, planes);
            if (_planeMeshes.ContainsKey (hash)) {
                return _planeMeshes [hash];
            } else {
                Mesh mesh = GetPlaneMesh (width, height, widthPivot, heightPivot, planes);
                _planeMeshes.Add (hash, mesh);
                return mesh;
            }
        }
        public override void Clear () {
            _planeMeshes.Clear ();
        }
        #endregion

        #region Mesh Processing
        /// <summary>
        /// Get a plane mesh on the XY plane.
        /// </summary>
        /// <param name="width">Width of the plane.</param>
        /// <param name="height">Height of the plane.</param>
        /// <param name="widthPivot">Pivot for the center of the plane (at 0,0,0) on the width side.</param>
        /// <param name="heightPivot">Pivot for the center of the plane (at 0,0,0) on the height side.</param>
        /// <param name="planes">Numbers of planes (cross or tricross).</param>
        /// <param name="pivotAdaptative">If <c>true</c> planes with a pivot at the center of the plane get an
        /// additional vertex to better set the gradient values.</param>
        /// <returns>Plane mesh.</returns>
        public static Mesh GetPlaneMesh (
            float width, 
            float height, 
            float widthPivot = 0f, 
            float heightPivot = 0f,
            int planes = 1,
            bool pivotAdaptative = true)
        {
            // Create mesh.
            Mesh mesh = new Mesh ();

            // Validation.
            if (planes < 1 || planes > 3) planes = 1;

            // Check if a pivot adaptative should be build.
            int pivotAdaptativeMode = PIVOT_MODE_NONE;
            if (pivotAdaptative) {
                if (widthPivot < _pivotAdaptativeThreshold || widthPivot > 1f - _pivotAdaptativeThreshold) {
                    if (heightPivot < _pivotAdaptativeThreshold || heightPivot > 1f - _pivotAdaptativeThreshold) {
                        // Corner pivot.
                        pivotAdaptativeMode = PIVOT_MODE_CORNER;
                    } else {
                        // Side pivot.
                        pivotAdaptativeMode = PIVOT_MODE_SIDE;
                    }
                }
                else {
                    if (heightPivot < _pivotAdaptativeThreshold || heightPivot > 1f - _pivotAdaptativeThreshold) {
                        // Side pivot.
                        pivotAdaptativeMode = PIVOT_MODE_SIDE;
                    } else {
                        // Center pivot.
                        pivotAdaptativeMode = PIVOT_MODE_CENTER;
                    }
                }
            }

            // Vertices and UV2.
            int numVertices;
            int numTris;
            if (pivotAdaptativeMode == PIVOT_MODE_CENTER) {
                numVertices = 5 * planes;
                numTris = 12 * planes;
            } else {
                numVertices = 4 * planes;
                numTris = 6 * planes;
            }
            Vector3[] vertices = new Vector3[numVertices];
            Vector3[] normals = new Vector3[numVertices];
            Vector4[] tangents = new Vector4[numVertices];
            Vector4[] uvs = new Vector4[numVertices];
            Vector4[] uv2s = new Vector4[numVertices];
            int[] tris = new int[numTris];

            SetPlaneData (width, height, widthPivot, heightPivot, pivotAdaptativeMode,
                0, 0, Quaternion.identity, 0,
                ref vertices, ref normals, ref tangents, ref uvs, ref uv2s, ref tris);
            if (planes > 1) {
                SetPlaneData (width, height, widthPivot, heightPivot, pivotAdaptativeMode,
                    (pivotAdaptativeMode==PIVOT_MODE_CENTER?5:4), (pivotAdaptativeMode==PIVOT_MODE_CENTER?12:6), Quaternion.Euler (0f, 0f, 90f), 1,
                    ref vertices, ref normals, ref tangents, ref uvs, ref uv2s, ref tris);
            }
            if (planes > 2) {
                SetPlaneData (width, height, widthPivot, heightPivot, pivotAdaptativeMode,
                    (pivotAdaptativeMode==PIVOT_MODE_CENTER?10:8), (pivotAdaptativeMode==PIVOT_MODE_CENTER?24:12), Quaternion.Euler (-90f, 0f, 0f), 2,
                    ref vertices, ref normals, ref tangents, ref uvs, ref uv2s, ref tris);
            }

            // Set vertices, normals, tangents, UVs, UV2s and triangles.
            mesh.SetVertices (vertices);
            mesh.SetTriangles (tris, 0);
            mesh.SetNormals (normals);
            mesh.SetTangents (tangents);
            mesh.SetUVs (0, uvs);
            mesh.SetUVs (1, uv2s);
            mesh.RecalculateBounds ();

            return mesh;
        }
        public static void SetUVData (float uvX, float uvY, float uvWidth, float uvHeight, int uvStep) {
            _uvX = uvX;
            _uvY = uvY;
            _uvWidth = uvWidth;
            _uvHeight = uvHeight;
            _uvStep = uvStep;
            if (_uvStep == 1)
                _tangent = new Vector4 (0f, 0f, -1f, -1f);
            else if (_uvStep == 2)
                _tangent = new Vector4 (-1f, 0f, 0f, -1f);
            else if (_uvStep == 3)
                _tangent = new Vector4 (0f, 0f, 1f, -1f);
            else
                _tangent = new Vector4 (1f, 0f, 0f, -1f);
        }
        public static void SetIdData (int id) {
            _id = id;
        }
        private static void SetPlaneData (
            float width, 
            float height, 
            float widthPivot, 
            float heightPivot,
            int pivotAdaptativeMode,
            int vOffset,
            int tOffset,
            Quaternion orientation,
            int planeId,
            ref Vector3[] vertices,
            ref Vector3[] normals,
            ref Vector4[] tangents,
            ref Vector4[] uvs,
            ref Vector4[] uv2s,
            ref int[] tris)
        {
            float widthOffset = width * -widthPivot;
            float heightOffset = height * -heightPivot;
            float maxLength = 0f;
            float maxSide = 0f;

            // Set VERTICES-
            vertices [vOffset] = new Vector3 (widthOffset, 0f, heightOffset);
            vertices [vOffset + 1] = new Vector3 (width + widthOffset, 0f, heightOffset);
            vertices [vOffset + 2] = new Vector3 (width + widthOffset, 0f, height + heightOffset);
            vertices [vOffset + 3] = new Vector3 (widthOffset, 0f, height + heightOffset);
            if (pivotAdaptativeMode == PIVOT_MODE_CENTER) {
                vertices [vOffset + 4] = Vector3.zero;
            }

            // Set NORMALS.
            for (int i = vOffset; i <  vOffset + 4; i++) {
                normals [i] = orientation * Vector3.up;
            }
            if (pivotAdaptativeMode == PIVOT_MODE_CENTER) {
                normals [vOffset + 4] = orientation * Vector3.up;
            }

            // Set TANGENTS.
            for (int i = vOffset; i < vOffset + 4; i++) {
                tangents [i] = orientation * _tangent;
                tangents [i].w = -1;
            }
            if (pivotAdaptativeMode == PIVOT_MODE_CENTER) {
                tangents [vOffset + 4] = orientation * _tangent;
                tangents [vOffset + 4].w = -1;
            }

            // Set UVs.
            if (_uvStep == 0) {
                uvs [vOffset] = new Vector4 (_uvX, _uvY, 0f, 0f);
                uvs [vOffset + 1] = new Vector4 (_uvX + _uvWidth, _uvY, 1f, 0f); 
                uvs [vOffset + 2] = new Vector4 (_uvX + _uvWidth, _uvY + _uvHeight, 1f, 1f);
                uvs [vOffset + 3] = new Vector4 (_uvX, _uvY + _uvHeight, 0f, 1f);
            } else if (_uvStep == 1) {
                uvs [vOffset] = new Vector4 (_uvX + _uvWidth, _uvY, 1f, 0f); 
                uvs [vOffset + 1] = new Vector4 (_uvX + _uvWidth, _uvY + _uvHeight, 1f, 1f);
                uvs [vOffset + 2] = new Vector4 (_uvX, _uvY + _uvHeight, 0f, 1f);
                uvs [vOffset + 3] = new Vector4 (_uvX, _uvY, 0f, 0f);
            } else if (_uvStep == 2) {
                uvs [vOffset] = new Vector4 (_uvX + _uvWidth, _uvY + _uvHeight, 1f, 1f);
                uvs [vOffset + 1] = new Vector4 (_uvX, _uvY + _uvHeight, 0f, 1f);
                uvs [vOffset + 2] = new Vector4 (_uvX, _uvY, 0f, 0f);
                uvs [vOffset + 3] = new Vector4 (_uvX + _uvWidth, _uvY, 1f, 0f); 
            } else {
                uvs [vOffset] = new Vector4 (_uvX, _uvY + _uvHeight, 0f, 1f);
                uvs [vOffset + 1] = new Vector4 (_uvX, _uvY, 0f, 0f);
                uvs [vOffset + 2] = new Vector4 (_uvX + _uvWidth, _uvY, 1f, 0f); 
                uvs [vOffset + 3] = new Vector4 (_uvX + _uvWidth, _uvY + _uvHeight, 1f, 1f);
            }
            if (pivotAdaptativeMode == PIVOT_MODE_CENTER) {
                uvs [vOffset + 4] = new Vector4 (_uvX + _uvWidth * widthPivot, _uvY + _uvHeight * heightPivot, widthPivot, heightPivot);
            }

            // Set UV2s.
            float widthPAOffset = 0f;
            float heightPAOffset= 0f;
            int pivotAdaptiveSide = -1;
            if (pivotAdaptativeMode == PIVOT_MODE_SIDE || pivotAdaptativeMode == PIVOT_MODE_CORNER) {
                if (heightPivot < _pivotAdaptativeThreshold) {
                    heightPAOffset = heightPivot * height;
                    pivotAdaptiveSide = 0;
                }
                if (widthPivot > 1f - _pivotAdaptativeThreshold) {
                    widthPAOffset = -(width - widthPivot * width);
                    pivotAdaptiveSide = 1;
                }
                if (heightPivot > 1f - _pivotAdaptativeThreshold) {
                    heightPAOffset = -(height - heightPivot * height);
                    pivotAdaptiveSide = 2;
                }
                if (widthPivot < _pivotAdaptativeThreshold) {
                    widthPAOffset = widthPivot * width;
                    pivotAdaptiveSide = 3;
                }
            }
            for (int i = vOffset; i < vOffset + 4; i++) {
                uv2s [i] = new Vector4 (vertices [i].z + heightPAOffset, vertices [i].x + widthPAOffset, planeId, _id);
                if (Mathf.Abs (vertices [i].z + heightPAOffset) > maxLength) {
                    maxLength = Mathf.Abs (vertices [i].z + heightPAOffset);
                }
                if (Mathf.Abs (vertices [i].x + widthPAOffset) > maxSide) {
                    maxSide = Mathf.Abs (vertices [i].x + widthPAOffset);
                }
            }
            for (int i = vOffset; i < vOffset + 4; i++) {
                uv2s [i].x = Mathf.Abs (uv2s [i].x) / maxLength;
                uv2s [i].y = Mathf.Abs (uv2s [i].y) / maxSide;
                if (pivotAdaptativeMode == PIVOT_MODE_SIDE) {
                    if (pivotAdaptiveSide == 0 || pivotAdaptiveSide == 2) uv2s [i].y = uv2s [i].x;
                    else uv2s [i].x = uv2s [i].y;
                }
            }
            if (pivotAdaptativeMode == PIVOT_MODE_CENTER) {
                uv2s [vOffset + 4] = new Vector4 (0.001f, 0.001f, planeId, _id); // Near zero to avoid division by 0.
            }

            // Rotate VERTICES-
            vertices [vOffset] = orientation * vertices [vOffset];
            vertices [vOffset + 1] = orientation * vertices [vOffset + 1];
            vertices [vOffset + 2] = orientation * vertices [vOffset + 2];
            vertices [vOffset + 3] = orientation * vertices [vOffset + 3];

            // Triangles.
            if (pivotAdaptativeMode == PIVOT_MODE_CENTER) {
                tris [tOffset] = vOffset;
                tris [tOffset + 1] = vOffset + 4;
                tris [tOffset + 2] = vOffset + 1;

                tris [tOffset + 3] = vOffset + 1;
                tris [tOffset + 4] = vOffset + 4;
                tris [tOffset + 5] = vOffset + 2;

                tris [tOffset + 6] = vOffset + 2;
                tris [tOffset + 7] = vOffset + 4;
                tris [tOffset + 8] = vOffset + 3;

                tris [tOffset + 9]  = vOffset + 3;
                tris [tOffset + 10] = vOffset + 4;
                tris [tOffset + 11] = vOffset;
            } else {
                tris [tOffset] = vOffset;
                tris [tOffset + 1] = vOffset + 2;
                tris [tOffset + 2] = vOffset + 1;
                
                tris [tOffset + 3] = vOffset;
                tris [tOffset + 4] = vOffset + 3;
                tris [tOffset + 5] = vOffset + 2;
            }
        }
        /// <summary>
        /// Get the hash128 for a plane mesh gien its parameters.
        /// </summary>
        /// <param name="width">Width of the plane.</param>
        /// <param name="height">Height of the plane.</param>
        /// <param name="widthPivot">Pivot point position on the plane width.</param>
        /// <param name="heightPivot">Pivot point position on the plane height.</param>
        /// <param name="planes">Number of planes.</param>
        /// <returns>Hash for the plane mesh.</returns>
        private Hash128 GetMeshHash (float width, float height, float widthPivot, float heightPivot, int planes) {
            string paramsStr = string.Format ("plane_{0:0.###}_{1:0.###}_{2:0.###}_{3:0.###}_{4}",
                width, height, widthPivot, heightPivot, planes);
            Debug.Log ("paramsStr: " + paramsStr);
            return Hash128.Compute (paramsStr);
        }
        #endregion
    }
}
