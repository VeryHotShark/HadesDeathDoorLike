using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Broccoli.Pipe;
using Broccoli.Manager;

namespace Broccoli.Builder
{
    public class BranchCollectionSproutMeshBuilder : BaseSproutMeshBuilder
    {
        #region Vars
		public BranchDescriptorCollection branchDescriptorCollection = null;
        int snapshot = 0;
        int lod = 0;
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
            Hash128 hash = GetMeshHash (branchDescriptorCollection, snapshot, lod, meshScale, meshPivot, meshOrientation);
            if (_meshes.ContainsKey (hash)) {
                return _meshes [hash];
            } else {
                Mesh mesh = GetMesh (branchDescriptorCollection, snapshot, lod, meshScale, meshPivot, meshOrientation);
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
            BranchDescriptorCollection branchDescriptorCollection,
            int snapshot,
            int lod,
            Vector3 meshScale,
            Vector3 meshPivot,
            Quaternion meshOrientation)
        {
            //Validation,
            if (branchDescriptorCollection == null) return null;if (branchDescriptorCollection == null) return null;

            BranchDescriptor branchDescriptor = branchDescriptorCollection.branchDescriptors [snapshot];
            SproutCompositeManager sproutCompositeManager = SproutCompositeManager.Current ();
            sproutCompositeManager.Clear ();
            for (int j = 0; j < branchDescriptor.polygonAreas.Count; j++) {
                PolygonAreaBuilder.SetPolygonAreaMesh (branchDescriptor.polygonAreas [j]);
                sproutCompositeManager.ManagePolygonArea (branchDescriptor.polygonAreas [j], branchDescriptor);
            }
            Mesh collectionMesh = sproutCompositeManager.GetMesh (branchDescriptor.id, lod);
            NormalizeBranchCollectionTransform (collectionMesh, 1f, Quaternion.Euler (0, 90, 90));

            return collectionMesh;
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
        private Hash128 GetMeshHash (BranchDescriptorCollection branchDescriptorCollection, 
            int snapshot, int lod, Vector3 scale, Vector3 pivot, Quaternion orientation)
        {
            string paramsStr = string.Format ("collection_{0}_{1}_{2}_{3}_{4}_{5}",
                branchDescriptorCollection.ToString (), snapshot, lod, scale.ToString (), meshPivot.ToString (), orientation.ToString ());
            Debug.Log ("paramsStr: " + paramsStr);
            return Hash128.Compute (paramsStr);
        }
        /// <summary>
		/// Applies scale and rotation to meshes coming from SproutLab's branch descriptor collection.
		/// </summary>
		/// <param name="mesh">Mesh to appy the transformation.</param>
		/// <param name="scale">Scale transformation.</param>
		/// <param name="rotation">Rotation transformation.</param>
		private static void NormalizeBranchCollectionTransform (Mesh mesh, float scale, Quaternion rotation) {
			Vector3[] _vertices = mesh.vertices;
			Vector3[] _normals = mesh.normals;
			Vector4[] _tangents = mesh.tangents;
            Vector4[] _uv2s = new Vector4[mesh.vertexCount];
            float maxLength = 0f;
            float maxSide = 0f;
			for (int i = 0; i < _vertices.Length; i++) {
				_vertices [i] = rotation * _vertices [i] * scale;
				_normals [i] = rotation * _normals [i];
				_tangents [i] = rotation * _tangents [i];
                if (Mathf.Abs (_vertices [i].z) > maxLength) {
                    maxLength = Mathf.Abs (_vertices [i].z);
                }
                if (Mathf.Abs (_vertices [i].x) > maxSide) {
                    maxSide = Mathf.Abs (_vertices [i].x);
                }
                _uv2s [i] = new Vector4 (_vertices [i].z, _vertices [i].x, 0f, _id);
			}
            for (int i = 0; i < _uv2s.Length; i++) {
                _uv2s [i].x = Mathf.Abs (_uv2s [i].x) / maxLength;
                _uv2s [i].y = Mathf.Abs (_uv2s [i].y) / maxSide;
            }
			mesh.vertices = _vertices;
			mesh.normals = _normals;
			mesh.tangents = _tangents;
            mesh.SetUVs (1, _uv2s);
			mesh.RecalculateBounds ();
		}
        #endregion
    }
}
