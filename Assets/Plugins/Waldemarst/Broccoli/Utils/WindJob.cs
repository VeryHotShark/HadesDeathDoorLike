using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Unity.Jobs;
using Unity.Collections;

namespace Broccoli.Utils
{
    public class WindJob {
        #region Vars
		public float windAmplitude = 0.25f;
        public int batchSize = 4;
        public List<Vector2> weightHeights = new List<Vector2> ();
		public List<Vector3> xySwingPhases = new List<Vector3> ();
        public List<Vector3> origins = new List<Vector3> ();
        public List<int> starts = new List<int> ();
        public List<int> lengths = new List<int> ();
        private List<Vector3> vertices = new List<Vector3> ();
        private List<Vector4> uvs = new List<Vector4> ();
        private Mesh targetMesh = null;
        #endregion

        #region Job
		/// <summary>
		/// Job structure to process branch skins.
		/// </summary>
		struct WindJobImpl : IJobParallelFor {
			#region Params
			public float windAmplitude;
			#endregion

			#region Input
			/// <summary>
			/// Contains the wind weight (x), mesh height (y).
			/// </summary>
			public NativeArray<Vector2> weightHeights;
			public NativeArray<Vector3> xySwingPhases;
			/// <summary>
			/// Contains the origin for the mesh unit.
			/// </summary>
			public NativeArray<Vector3> origins;
			/// <summary>
            /// START for the submesh vertices.
            /// </summary>
            public NativeArray<int> start;
            /// <summary>
            /// LENGTH of the vertices for the submesh
            /// </summary>
            public NativeArray<int> length;
			#endregion

			#region Mesh Data
			/// <summary>
			/// Input vertices for the input mesh.
			/// </summary>
			[NativeDisableParallelForRestriction]
			public NativeArray<Vector3> vertices;
            /// <summary>
			/// Output UV information of the mesh.
			/// x: mapping U component.
			/// y: mapping V component.
			/// z: Outputs wind data: gradient from trunk to branch tip.
			/// w: Outputs wind data: branch swing phase. (0-15).
			/// </summary>
            [NativeDisableParallelForRestriction]         
			public NativeArray<Vector4> uvs;
            /// <summary>
			/// Output UV2 information of the mesh.
			/// x: Unalloc: normalized U on leaves.
			/// y: Unalloc: normalized V on leaves.
			/// z: Outputs wind data: x phased values (0-5).
			/// w: Outputs wind data: y phased values (0-14).
			/// </summary>
			[NativeDisableParallelForRestriction]
			public NativeArray<Vector4> uv2s;
            /// <summary>
			/// Output UV3 information of the mesh.
			/// x: vertex x position.
			/// y: vertex y position.
			/// z: vertex z position..
			/// w: vertex z position on the branch origin.
			/// </summary>
			[NativeDisableParallelForRestriction]
			public NativeArray<Vector4> uv3s;
			#endregion

			#region Job Methods
			/// <summary>
			/// Executes one per sprout.
			/// </summary>
			/// <param name="i"></param>
			public void Execute (int i) {
			    float spWeight = weightHeights [i].x;
                float spHeight = weightHeights [i].y;
				Vector3 spOrigin = origins [i];
				float xPhase = xySwingPhases [i].x;
				float yPhase = xySwingPhases [i].y;
				float swingPhase = xySwingPhases [i].z;
				int vertexStart = start [i];
				int vertexEnd = start [i] + length [i];

				// Apply the transformations.
				ApplyUV (vertexStart, vertexEnd, swingPhase, spOrigin);
				ApplyUV2 (vertexStart, vertexEnd, xPhase, yPhase);
				ApplyUV3 (vertexStart, vertexEnd, spOrigin);
			}
			public void ApplyUV (int vertexStart, int vertexEnd, float swingPhase, Vector3 origin) {
				for (int i = vertexStart; i < vertexEnd; i++) {
					uvs [i] = new Vector4 (uvs [i].x, uvs [i].y, Vector3.Distance (origin, vertices [i]) * windAmplitude, swingPhase);
					/// x: mapping U component.
					/// y: mapping V component.
					/// z: Outputs wind data: gradient from trunk to branch tip. (up/down sway?)
					/// w: Outputs wind data: branch swing phase. (0-15).
				}
			}
			public void ApplyUV2 (int vertexStart, int vertexEnd, float xPhase, float yPhase) {
				//float xPhase = Random.Range (0f, 5f);
				//float yPhase = Random.Range (0f, 14f);
				for (int i = vertexStart; i < vertexEnd; i++) {
					uv2s [i] = new Vector4 (uvs [i].z, uvs [i].w, xPhase, yPhase);
					/// z: Adds wind data: x phased values (0-5).
					/// w: Adds wind data: y phased values (0-14).
				}
			}
			public void ApplyUV3 (int vertexStart, int vertexEnd, Vector3 origin) {
				for (int i = vertexStart; i < vertexEnd; i++) {
					uv3s [i] = new Vector4 (vertices [i].x, vertices [i].y, vertices [i].z, vertices [i].z - origin.z);
					/// x: vertex x position.
					/// y: vertex y position.
					/// z: vertex z position..
					/// w: vertex z position on the branch origin.
				}
			}
			#endregion
		}
		#endregion

        #region Processing
		/// <summary>
        /// Clears Job related variables.
        /// </summary>
        public void Clear () {
            weightHeights.Clear ();
            origins.Clear ();
			xySwingPhases.Clear ();
            starts.Clear ();
            lengths.Clear ();
			ClearMesh ();
        }
        /// <summary>
        /// Clears Mesh related variables.
        /// </summary>
        public void ClearMesh () {
            vertices.Clear ();
            uvs.Clear ();
            targetMesh = null;
        }
        public void SetTargetMesh (Mesh mesh) {
            ClearMesh ();
            targetMesh = mesh;
            vertices.AddRange (mesh.vertices);
            uvs.Clear ();
            mesh.GetUVs (0, uvs);
        }
        public Mesh GetTargetMesh () {
            return targetMesh;
        }
        public void AddWindUnit (int vertexStart, int vertexLength, float xPhase, float yPhase, float swingPhase, Vector3 originOffset) {
            starts.Add (vertexStart);
            lengths.Add (vertexLength);
            weightHeights.Add (new Vector2 (1f, 1f));
			xySwingPhases.Add (new Vector3 (xPhase, yPhase, swingPhase));
            origins.Add (originOffset);
        }
        public void ExecuteJob () {
			// Mark the mesh as dynamic.
			targetMesh.MarkDynamic ();
			// Create the job.
			WindJobImpl _meshJob = new WindJobImpl () {
				windAmplitude = windAmplitude,
				weightHeights = new NativeArray<Vector2> (weightHeights.ToArray (), Allocator.TempJob),
				xySwingPhases = new NativeArray<Vector3> (xySwingPhases.ToArray (), Allocator.TempJob),
				origins = new NativeArray<Vector3> (origins.ToArray (), Allocator.TempJob),
				start = new NativeArray<int> (starts.ToArray (), Allocator.TempJob),
				length = new NativeArray<int> (lengths.ToArray (), Allocator.TempJob),
				vertices = new NativeArray<Vector3> (vertices.ToArray (), Allocator.TempJob),
				uvs = new NativeArray<Vector4> (uvs.ToArray (), Allocator.TempJob),
                uv2s = new NativeArray<Vector4> (uvs.Count, Allocator.TempJob),
                uv3s = new NativeArray<Vector4> (uvs.Count, Allocator.TempJob),
			};
			// Execute the job .
			JobHandle _meshJobHandle = _meshJob.Schedule (weightHeights.Count, batchSize);

			// Complete the job.
			_meshJobHandle.Complete();

			targetMesh.SetVertices (_meshJob.vertices);
            targetMesh.SetUVs (0, _meshJob.uvs);
            targetMesh.SetUVs (1, _meshJob.uv2s);
            targetMesh.SetUVs (2, _meshJob.uv3s);
			targetMesh.UploadMeshData (true);

			// Dispose allocated memory.
            _meshJob.weightHeights.Dispose ();
			_meshJob.xySwingPhases.Dispose ();
            _meshJob.origins.Dispose ();
            _meshJob.start.Dispose ();
            _meshJob.length.Dispose ();
            _meshJob.vertices.Dispose ();
            _meshJob.uvs.Dispose ();
            _meshJob.uv2s.Dispose ();
            _meshJob.uv3s.Dispose ();
        }
        #endregion
    }   
}
