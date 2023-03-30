using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Broccoli.Builder
{
    public class MeshSproutMeshBuilder : BaseSproutMeshBuilder
    {
        #region Vars
        public Mesh srcMesh = null;
        public Vector3 meshScale = Vector3.one;
        public Vector3 meshPivot = Vector3.zero;
        public Quaternion meshOrientation = Quaternion.identity;
        private static int _id = 0;
        public Dictionary<Hash128, Mesh> _meshes = new Dictionary<Hash128, Mesh> ();
        #endregion

        #region Abstract
        public override void SetParams (string jsonParams){
            throw new System.NotImplementedException();
        }
        public override Mesh GetMesh () {
            Hash128 hash = GetMeshHash (srcMesh, meshScale, meshPivot, meshOrientation);
            if (_meshes.ContainsKey (hash)) {
                return _meshes [hash];
            } else {
                Mesh mesh = GetMesh (srcMesh, meshScale, meshPivot, meshOrientation);
                _meshes.Add (hash, mesh);
                return mesh;
            }
        }
        public override void Clear () {
            _meshes.Clear ();
        }
        #endregion

        #region Mesh Processing
        public static Mesh GetMesh (
            Mesh srcMesh,
            Vector3 meshScale,
            Vector3 meshPivot,
            Quaternion meshOrientation)
        {
            //Validation,
            if (srcMesh == null) return null;

            // Create mesh.
            Mesh mesh = Object.Instantiate<Mesh> (srcMesh);

            // Normals.
            if (mesh.normals.Length == 0) mesh.RecalculateNormals ();

            // Tangents.
            if (mesh.tangents.Length == 0) mesh.RecalculateTangents ();

            mesh.RecalculateBounds ();

            // UVs.
            if (mesh.uv.Length == 0) mesh.SetUVs (0, new Vector4[mesh.vertexCount]);

            // Apply scale, pivot and rotation.
            Broccoli.Utils.MeshJob meshJob = new Broccoli.Utils.MeshJob (false);
            Vector3 meshBound = mesh.bounds.size;
            Vector3 offset = new Vector3 (meshBound.x * meshPivot.x, meshBound.y * meshPivot.y, meshBound.z * meshPivot.z);
            meshJob.SetTargetMesh (mesh);
            meshJob.AddTransform (0, mesh.vertexCount, offset, meshScale, meshOrientation);

            meshJob.ExecuteJob ();

            // UV2s.
            Vector4[] uv2s = new Vector4[mesh.vertexCount];
            float maxLength = 0f;
            int vertexCount = mesh.vertexCount;
            Vector3[] vertices = mesh.vertices;
            for (int i = 0; i < vertexCount; i++) {
                if (vertices [i].magnitude > maxLength) maxLength = vertices [i].magnitude;
            }
            for (int i = 0; i < vertexCount; i++) {
                uv2s [i] = new Vector4 (vertices [i].magnitude / maxLength, vertices [i].magnitude / maxLength, 0f, _id);
            }
            mesh.SetUVs (1, uv2s);
            /*

            // Vertices and UV2.
            int numVertices = 4 * planes;
            int numTris = 6 * planes;
            Vector3[] vertices = new Vector3[numVertices];
            Vector3[] normals = new Vector3[numVertices];
            Vector4[] tangents = new Vector4[numVertices];
            Vector4[] uvs = new Vector4[numVertices];
            Vector4[] uv2s = new Vector4[numVertices];
            int[] tris = new int[numTris];

            SetPlaneData (width, height, widthPivot, heightPivot, 
                0, 0, Quaternion.identity,
                ref vertices, ref normals, ref tangents, ref uvs, ref uv2s, ref tris);
            if (planes > 1) {
                SetPlaneData (width, height, widthPivot, heightPivot, 
                    4, 6, Quaternion.LookRotation (Vector3.right),
                    ref vertices, ref normals, ref tangents, ref uvs, ref uv2s, ref tris);
            }
            if (planes > 2) {
                SetPlaneData (width, height, widthPivot, heightPivot, 
                    8, 12, Quaternion.LookRotation (Vector3.down) * Quaternion.LookRotation (Vector3.right),
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
            */

            return mesh;
        }
        public static void SetIdData (int id) {
            _id = id;
        }
        /// <summary>
        /// Get the hash128 for a mesh given its parameters.
        /// </summary>
        /// <param name="mesh">Mesh object.</param>
        /// <param name="scale">Scale to apply to the mesh.</param>
        /// <param name="pivot">Pivot to apply to the mesh.</param>
        /// <param name="orientation">Rotation to apply to the mesh after pivot offset and scaling.</param>
        /// <returns>Hash for the mesh.</returns>
        private Hash128 GetMeshHash (Mesh mesh, Vector3 scale, Vector3 pivot, Quaternion orientation) {
            string paramsStr = string.Format ("mesh_{0}_{1}_{2}_{3}",
                mesh.ToString (), scale.ToString (), meshPivot.ToString (), orientation.ToString ());
            Debug.Log ("paramsStr: " + paramsStr);
            return Hash128.Compute (paramsStr);
        }
        #endregion
    }
}
