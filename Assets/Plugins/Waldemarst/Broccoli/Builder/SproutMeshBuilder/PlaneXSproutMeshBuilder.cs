using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Broccoli.Builder
{
    public class PlaneXSproutMeshBuilder : BaseSproutMeshBuilder
    {
        #region Vars
        public float width = 1f;
        public float height = 1f;
        public float widthPivot = 0.5f;
        public float heightPivot = 0f;
        public float depth = 0;
        public Dictionary<Hash128, Mesh> _planeMeshes = new Dictionary<Hash128, Mesh> ();
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
            Hash128 hash = GetMeshHash (width, height, widthPivot, heightPivot, depth);
            if (_planeMeshes.ContainsKey (hash)) {
                return _planeMeshes [hash];
            } else {
                Mesh mesh = GetPlaneXMesh (width, height, widthPivot, heightPivot, depth);
                _planeMeshes.Add (hash, mesh);
                return mesh;
            }
        }
        public override void Clear () {
            _planeMeshes.Clear ();
        }
        #endregion

        #region Mesh Processing
        public static Mesh GetPlaneXMesh (
            float width, 
            float height, 
            float widthPivot = 0f, 
            float heightPivot = 0f,
            float depth = 1)
        {
            // Create mesh.
            Mesh mesh = new Mesh ();

            // Validation.
            int planes = 1;

            // Vertices and UV2.
            int numVertices = 5 * planes;
            int numTris = 12 * planes;
            Vector3[] vertices = new Vector3[numVertices];
            Vector3[] normals = new Vector3[numVertices];
            Vector4[] tangents = new Vector4[numVertices];
            Vector4[] uvs = new Vector4[numVertices];
            Vector4[] uv2s = new Vector4[numVertices];
            int[] tris = new int[numTris];

            SetPlaneData (width, height, widthPivot, heightPivot, depth,
                0, 0, Quaternion.identity, 0,
                ref vertices, ref normals, ref tangents, ref uvs, ref uv2s, ref tris);

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
            float depth,
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

            // Normalized pivot (< 0.5f)
            float nWidthPivot = widthPivot;
            if (widthPivot > 0.5f) nWidthPivot = 1f - widthPivot;
            float nHeightPivot = heightPivot;
            if (heightPivot > 0.5f) nHeightPivot = 1f - heightPivot;
            float centerDepth = 0f;
            float borderDepth = 0f;
            if (nWidthPivot > nHeightPivot) {
                // Height based depth.
                borderDepth = nHeightPivot * 2f * depth;
                centerDepth = borderDepth - depth;
            } else {
                // Width based depth.
                borderDepth = nWidthPivot * 2f * depth;
                centerDepth = borderDepth - depth;
            }

            vertices [vOffset] = orientation * new Vector3 (widthOffset, borderDepth, heightOffset);
            vertices [vOffset + 1] = orientation * new Vector3 (width + widthOffset, borderDepth, heightOffset);
            vertices [vOffset + 2] = orientation * new Vector3 (width + widthOffset, borderDepth, height + heightOffset);
            vertices [vOffset + 3] = orientation * new Vector3 (widthOffset, borderDepth, height + heightOffset);
            vertices [vOffset + 4] = orientation * new Vector3 (widthOffset + width * 0.5f, centerDepth, heightOffset + height * 0.5f);
            for (int i = vOffset; i < vOffset + 4; i++) {
                uv2s [i] = new Vector4 (vertices [i].z, vertices [i].x, planeId, _id);
                if (Mathf.Abs (vertices [i].z) > maxLength) {
                    maxLength = Mathf.Abs (vertices [i].z);
                }
                if (Mathf.Abs (vertices [i].x) > maxSide) {
                    maxSide = Mathf.Abs (vertices [i].x);
                }
            }
            uv2s [vOffset + 4] = new Vector4 (vertices [vOffset + 4].magnitude, vertices [vOffset + 4].x, 0f, _id);

            for (int i = vOffset; i < vOffset + 5; i++) {
                uv2s [i].x = Mathf.Abs (uv2s [i].x) / maxLength;
                uv2s [i].y = Mathf.Abs (uv2s [i].y) / maxSide;
            }

            // Normals.
            for (int i = vOffset; i <  vOffset + 5; i++) {
                normals [i] = orientation * Vector3.up;
            }

            // Tangents.
            for (int i = vOffset; i < vOffset + 5; i++) {
                tangents [i] = orientation * _tangent;
                tangents [i].w = -1;
            }

            // UVs.
            if (_uvStep == 0) {
                uvs [vOffset] = new Vector4 (_uvX, _uvY, 0f, 0f);
                uvs [vOffset + 1] = new Vector4 (_uvX + _uvWidth, _uvY, 1f, 0f);
                uvs [vOffset + 2] = new Vector4 (_uvX + _uvWidth, _uvY + _uvHeight, 1f, 1f);
                uvs [vOffset + 3] = new Vector4 (_uvX, _uvY + _uvHeight, 0f, 1f);
                uvs [vOffset + 4] = new Vector4 (_uvX + _uvWidth * 0.5f, _uvY + _uvHeight * 0.5f, 0.5f, 0.5f);
            } else if (_uvStep == 1) {
                uvs [vOffset] = new Vector4 (_uvX + _uvWidth, _uvY, 1f, 0f);
                uvs [vOffset + 1] = new Vector4 (_uvX + _uvWidth, _uvY + _uvHeight, 1f, 1f);
                uvs [vOffset + 2] = new Vector4 (_uvX, _uvY + _uvHeight, 0f, 1f);
                uvs [vOffset + 3] = new Vector4 (_uvX, _uvY, 0f, 0f);
                uvs [vOffset + 4] = new Vector4 (_uvX + _uvWidth * 0.5f, _uvY + _uvHeight * 0.5f, 0.5f, 0.5f);
            } else if (_uvStep == 2) {
                uvs [vOffset] = new Vector4 (_uvX + _uvWidth, _uvY + _uvHeight, 1f, 1f);
                uvs [vOffset + 1] = new Vector4 (_uvX, _uvY + _uvHeight, 0f, 1f);
                uvs [vOffset + 2] = new Vector4 (_uvX, _uvY, 0f, 0f);
                uvs [vOffset + 3] = new Vector4 (_uvX + _uvWidth, _uvY, 1f, 0f);
                uvs [vOffset + 4] = new Vector4 (_uvX + _uvWidth * 0.5f, _uvY + _uvHeight * 0.5f, 0.5f, 0.5f);
            } else {
                uvs [vOffset] = new Vector4 (_uvX, _uvY + _uvHeight, 0f, 1f);
                uvs [vOffset + 1] = new Vector4 (_uvX, _uvY, 0f, 0f);
                uvs [vOffset + 2] = new Vector4 (_uvX + _uvWidth, _uvY, 1f, 0f);
                uvs [vOffset + 3] = new Vector4 (_uvX + _uvWidth, _uvY + _uvHeight, 1f, 1f);
                uvs [vOffset + 4] = new Vector4 (_uvX + _uvWidth * 0.5f, _uvY + _uvHeight * 0.5f, 0.5f, 0.5f);
            }

            // Triangles.
            tris [tOffset] = vOffset + 1;
            tris [tOffset + 1] = vOffset;
            tris [tOffset + 2] = vOffset + 4;

            tris [tOffset + 3] = vOffset + 2;
            tris [tOffset + 4] = vOffset + 1;
            tris [tOffset + 5] = vOffset + 4;

            tris [tOffset + 6] = vOffset + 3;
            tris [tOffset + 7] = vOffset + 2;
            tris [tOffset + 8] = vOffset + 4;

            tris [tOffset + 9] = vOffset + 0;
            tris [tOffset + 10] = vOffset + 3;
            tris [tOffset + 11] = vOffset + 4;
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
        private Hash128 GetMeshHash (float width, float height, float widthPivot, float heightPivot, float depth) {
            string paramsStr = string.Format ("planex_{0:0.###}_{1:0.###}_{2:0.###}_{3:0.###}_{4:0.###}",
                width, height, widthPivot, heightPivot, depth);
            Debug.Log ("paramsStr: " + paramsStr);
            return Hash128.Compute (paramsStr);
        }
        #endregion
    }
}
