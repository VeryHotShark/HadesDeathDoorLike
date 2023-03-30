using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Broccoli.Builder
{
    public class GridSproutMeshBuilder : BaseSproutMeshBuilder
    {
        #region Vars
        public float width = 1f;
        public float height = 1f;
        public float widthPivot = 0.5f;
        public float heightPivot = 0f;
        public int widthSegments = 1;
        public int heightSegments = 1;
        [Range(1,3)]
        public int planes = 1;
        public Dictionary<Hash128, Mesh> _gridMeshes = new Dictionary<Hash128, Mesh> ();
        private static float _uvX = 0;
        private static float _uvY = 0f;
        private static float _uvWidth = 1f;
        private static float _uvHeight = 1f;
        private static int _uvStep = 0;
        private static int _id = 0;
        private static Vector4 _tangent = new Vector4 (1f, 0f, 0f, -1f);
        #endregion

        #region Abstract
        public override void SetParams (string jsonParams){
            throw new System.NotImplementedException();
        }
        public override Mesh GetMesh () {
            Hash128 hash = GetMeshHash (width, height, widthSegments, heightSegments, widthPivot, heightPivot, planes);
            if (_gridMeshes.ContainsKey (hash)) {
                return _gridMeshes [hash];
            } else {
                Mesh mesh = GetGridMesh (width, height, widthSegments, heightSegments, widthPivot, heightPivot, planes);
                _gridMeshes.Add (hash, mesh);
                return mesh;
            }
        }
        public override void Clear () {
            _gridMeshes.Clear ();
        }
        #endregion

        #region Mesh Processing
        public static Mesh GetGridMesh (
            float width, 
            float height,
            int widthSegments = 1,
            int heightSegments = 1,
            float widthPivot = 0f, 
            float heightPivot = 0f,
            int planes = 1)
        {
            // Create mesh.
            Mesh mesh = new Mesh ();

            // Validation.
            if (widthSegments < 1) widthSegments = 1;
            if (heightSegments < 1) heightSegments = 1;
            if (planes < 1 || planes > 3) planes = 1;

            // Vertices, UV and UV2.
            int numVertices = (widthSegments + 1) * (heightSegments + 1) * planes;
            int numTris = widthSegments * heightSegments * 6 * planes;

            Vector3[] vertices = new Vector3[numVertices];
            Vector3[] normals = new Vector3[numVertices];
            Vector4[] tangents = new Vector4[numVertices];
            Vector4[] uvs = new Vector4[numVertices];
            Vector4[] uv2s = new Vector4[numVertices];
            int[] tris = new int[numTris];

            SetGridData (width, height, widthSegments, heightSegments, widthPivot, heightPivot,
                0, 0, Quaternion.identity, 0,
                ref vertices, ref normals, ref tangents, ref uvs, ref uv2s, ref tris);
            if (planes > 1) {
                SetGridData (width, height, widthSegments, heightSegments, widthPivot, heightPivot,
                    (widthSegments + 1) * (heightSegments + 1), widthSegments * heightSegments * 6,
                    Quaternion.Euler (0f, 0f, 90f), 1,
                    ref vertices, ref normals, ref tangents, ref uvs, ref uv2s, ref tris);
            }
            if (planes > 2) {
                SetGridData (width, height, widthSegments, heightSegments, widthPivot, heightPivot,
                    (widthSegments + 1) * (heightSegments + 1) * 2, widthSegments * heightSegments * 12,
                    Quaternion.Euler (-90f, 0f, 0f), 2,
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
        private static void SetGridData (
            float width, 
            float height,
            int widthSegments,
            int heightSegments,
            float widthPivot, 
            float heightPivot,
            int vOffset,
            int tOffset,
            Quaternion orientation,
            int gridId,
            ref Vector3[] vertices,
            ref Vector3[] normals,
            ref Vector4[] tangents,
            ref Vector4[] uvs,
            ref Vector4[] uv2s,
            ref int[] tris) 
        {
            int numVertices = (widthSegments + 1) * (heightSegments + 1);

            float widthOffset = width * -widthPivot;
            float heightOffset = height * -heightPivot;
            float maxLength = 0f;
            float maxSide = 0f;

            float widthStep = 1f / (float)widthSegments;
            float heightStep = 1f / (float)heightSegments;
            float widthLengthStep = width / (float)widthSegments;
            float heightLengthStep = height / (float)heightSegments;
            float widthUVStep = _uvWidth / (float)widthSegments;
            float heightUVStep = _uvHeight / (float)heightSegments;
            int vCount = 0;
            float uPos = 0;
            float vPos = 0;

            for (int i = 0; i <= heightSegments; i++) {
                for (int j = 0; j <= widthSegments; j++) {
                    vertices [vOffset + vCount] = orientation * new Vector3 (
                        widthOffset + j * widthLengthStep,
                        0f, 
                        heightOffset + i * heightLengthStep);
                    normals [vOffset + vCount] = orientation * Vector3.up;
                    tangents [vOffset + vCount] = orientation * _tangent;
                    tangents [vOffset + vCount].w = -1;
                    
                    if (_uvStep == 0) {
                        uPos = j * widthStep;
                        vPos = i * heightStep;
                    } else if (_uvStep == 1) {
                        uPos = 1f - i * heightStep;
                        vPos = j * widthStep;
                    } else if (_uvStep == 2) {
                        uPos = 1f - j * widthStep;
                        vPos = 1f - i * heightStep;
                    } else if (_uvStep == 3) {
                        uPos = i * heightStep;
                        vPos = 1f - j * widthStep;
                    }
                    uvs [vOffset + vCount] = new Vector4 (
                        _uvX + uPos * _uvWidth, _uvY + vPos * _uvHeight,
                        uPos, 
                        vPos);

                    uv2s [vOffset + vCount] = new Vector4 (
                        vertices [vOffset + vCount].z, // forward length.
                        vertices [vOffset + vCount].x, // side length.
                        gridId, // plane id.
                        _id); // mesh id.

                    if (Mathf.Abs (vertices [vOffset + vCount].z) > maxLength) {
                        maxLength = Mathf.Abs (vertices [vOffset + vCount].z);
                    }
                    if (Mathf.Abs (vertices [vOffset + vCount].x) > maxSide) {
                        maxSide = Mathf.Abs (vertices [vOffset + vCount].x);
                    }
                    vCount++;
                }
            }
            for (int i = vOffset; i < vOffset + numVertices; i++) {
                uv2s [i].x = Mathf.Abs (uv2s [i].x) / maxLength;
                uv2s [i].y = Mathf.Abs (uv2s [i].y) / maxSide;
            }

            // Triangles.
            int trisCount = 0;
            int a, b, c, d;
            for (int i = 0; i < heightSegments; i++) {
                for (int j = 0; j < widthSegments; j++) {
                    a = vOffset + (i * (widthSegments + 1)) + j;
                    b = a + 1;
                    d = a + widthSegments + 1;
                    c = a + widthSegments + 2;

                    tris [tOffset + trisCount] = b;
                    tris [tOffset + trisCount + 1] = a;
                    tris [tOffset + trisCount + 2] = c;

                    tris [tOffset + trisCount + 3] = c;
                    tris [tOffset + trisCount + 4] = a;
                    tris [tOffset + trisCount + 5] = d;

                    trisCount += 6;
                }
            }
        }
        /// <summary>
        /// Get the hash128 for a plane mesh gien its parameters.
        /// </summary>
        /// <param name="width">Width of the plane.</param>
        /// <param name="height">Height of the plane.</param>
        /// <param name="widthSegments">Segment divisions on the width side.</param>
        /// <param name="heightSegments">Segment divisions on the height side.</param>
        /// <param name="widthPivot">Pivot point position on the plane width.</param>
        /// <param name="heightPivot">Pivot point position on the plane height.</param>
        /// <param name="planes">Number of planes.</param>
        /// <returns>Hash for the plane mesh.</returns>
        private Hash128 GetMeshHash (
            float width, float height, 
            int widthSegments, int heightSegments, 
            float widthPivot, float heightPivot,
            int planes)
        {
            string paramsStr = string.Format ("grid_{0:0.###}_{1:0.###}_{2:0.###}_{3:0.###}_{4:0.###}_{5:0.###}_{6}",
                width, height, widthSegments, heightSegments, widthPivot, heightPivot, planes);
            Debug.Log ("paramsStr: " + paramsStr);
            return Hash128.Compute (paramsStr);
        }
        #endregion
    }
}
