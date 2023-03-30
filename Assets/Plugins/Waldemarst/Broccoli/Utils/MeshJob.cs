using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Unity.Jobs;
using Unity.Collections;

namespace Broccoli.Utils
{
    public class MeshJob {
        #region Vars
        public int batchSize = 4;
		public bool applyTranslationAtEnd = true;
		public int applyIdsChannel = 0;
        public List<Vector3> offsets = new List<Vector3> ();
		public List<Vector3> scales = new List<Vector3> ();
        public List<Quaternion> rotations = new List<Quaternion> ();
        public List<float> bendings = new List<float> ();
        public List<int> starts = new List<int> ();
        public List<int> lengths = new List<int> ();
		public List<Vector4> ids = new List<Vector4> ();
		private List<int> _flags = new List<int> ();
        private List<Vector3> vertices = new List<Vector3> ();
        private List<Vector3> normals = new List<Vector3> ();
        private List<Vector4> tangents = new List<Vector4> ();
        private Mesh targetMesh = null;
        #endregion

        #region Job
		/// <summary>
		/// Job structure to process branch skins.
		/// </summary>
		struct MeshJobImpl : IJobParallelFor {
			#region Input
			/// <summary>
			/// If <c>true</c> the translation (offset) if applied at the end of the processing (after scaling and rotation).
			/// </summary>
			public bool applyTranslationAtEnd;
			/// <summary>
			/// Contains the OFFSET (x, y, z).
			/// </summary>
			public NativeArray<Vector3> offset;
			/// <summary>
			/// Contains the SCALE (x, y, z).
			/// </summary>
			public NativeArray<Vector3> scale;
			/// <summary>
			/// Contains the ORIENTATION for the mesh segment.
			/// </summary>
			public NativeArray<Quaternion> orientation;
			/// <summary>
			/// Contains the BENDING for the mesh segment.
			/// </summary>
			public NativeArray<float> bending;
			/// <summary>
			/// Contains the ids to apply to UV2 (ch. 3).
			/// </summary>
			public NativeArray<Vector4> ids;
			/// <summary>
			/// Flags to control the applicacion of transforms.
			/// </summary>
			public NativeArray<int> flags;
			/// <summary>
            /// START for the submesh vertices.
            /// </summary>
            public NativeArray<int> start;
            /// <summary>
            /// LENGTH of the vertices for the submesh
            /// </summary>
            public NativeArray<int> length;
			#endregion

			#region Consts
			public static int APPLY_SCALE = 1;
			public static int APPLY_OFFSET = 2;
			public static int APPLY_ROTATION = 4;
			public static int APPLY_BEND = 8;
			public static int APPLY_IDS = 16;
			#endregion

			#region Mesh Input
			/// <summary>
			/// Vertices for the input mesh.
			/// </summary>
			[NativeDisableParallelForRestriction]
			public NativeArray<Vector3> vertices;
			/// <summary>
			/// Normals for the input mesh.
			/// </summary>
			[NativeDisableParallelForRestriction]
			public NativeArray<Vector3> normals;
			/// <summary>
			/// Tangents for the input mesh.
			/// </summary>
			[NativeDisableParallelForRestriction]
			public NativeArray<Vector4> tangents;
			/// <summary>
			/// Output ids.
			/// </summary>
			[NativeDisableParallelForRestriction]
			public NativeArray<Vector4> uvIds;
			#endregion

			#region Job Methods
			/// <summary>
			/// Executes one per sprout.
			/// </summary>
			/// <param name="i"></param>
			public void Execute (int i) {
				int vertexStart = start [i];
				int vertexEnd = start [i] + length [i];
				int flag = flags [i];

				// Apply the transformations.
				if ((flag & APPLY_BEND) == APPLY_BEND) {
					float spBending = bending [i];
					ApplyBend (vertexStart, vertexEnd, spBending);
				}
				if (!applyTranslationAtEnd) {
					if ((flag & APPLY_OFFSET) == APPLY_OFFSET) {
						Vector3 spOffset = (Vector3)offset [i];
						ApplyOffset (vertexStart, vertexEnd, spOffset);
					}
				}
				if ((flag & APPLY_SCALE) == APPLY_SCALE) {
					Vector3 spScale = scale [i];
					ApplyScale (vertexStart, vertexEnd, spScale);
				}
				if ((flag & APPLY_ROTATION) == APPLY_ROTATION) {
					Quaternion spOrientation = orientation [i];
					ApplyRotation (vertexStart, vertexEnd, spOrientation);
				}
				if (applyTranslationAtEnd) {
					if ((flag & APPLY_OFFSET) == APPLY_OFFSET) {
						Vector3 spOffset = (Vector3)offset [i];
						ApplyOffset (vertexStart, vertexEnd, spOffset);
					}
				}
				if ((flag & APPLY_IDS) == APPLY_IDS) {
					Vector4 spIds = ids [i];
					ApplyIds (vertexStart, vertexEnd, spIds);
				}
			}
			public void ApplyBend (int vertexStart, int vertexEnd, float bending) {
				Vector3 gravityForward = Vector3.forward;
				Vector3 gravityUp = Vector3.up;
				if (bending < 0) {
					gravityForward *= -1;
					gravityUp *= -1;
					bending *= -1;
				}
				Quaternion gravityQuaternion = Quaternion.LookRotation (gravityUp * -1, gravityForward);
				Quaternion bendQuaternion;
				float radialStrength;
				for (int i = 0; i < vertices.Length; i++) {
					radialStrength = bending * vertices[i].magnitude / 1f;
					bendQuaternion = Quaternion.Slerp (Quaternion.identity, gravityQuaternion, radialStrength);
					vertices [i] = bendQuaternion * vertices [i];
					normals [i] = bendQuaternion * normals [i];
					tangents [i] = bendQuaternion * tangents [i];
				}
			}
			public void ApplyScale (int vertexStart, int vertexEnd, Vector3 scale) {
				for (int i = vertexStart; i < vertexEnd; i++) {
					vertices [i] = Vector3.Scale (vertices [i], scale);
				}
			}
			public void ApplyRotation (int vertexStart, int vertexEnd, Quaternion orientation) {
				for (int i = vertexStart; i < vertexEnd; i++) {
					vertices [i] = orientation * vertices [i];
					normals [i] = orientation * normals [i];
					tangents [i] = orientation * tangents [i];
				}
			}
			public void ApplyOffset (int vertexStart, int vertexEnd, Vector3 offset) {
				for (int i = vertexStart; i < vertexEnd; i++) {
					vertices [i] = vertices [i] + offset;
				}
			}
			public void ApplyIds (int vertexStart, int vertexEnd, Vector4 ids) {
				for (int i = vertexStart; i < vertexEnd; i++) {
					uvIds [i] = ids;
				}
			}
			#endregion
		}
		#endregion

		#region Constructor
		public MeshJob (bool applyTranslationAtEnd = true) {
			this.applyTranslationAtEnd = applyTranslationAtEnd;
		}
		#endregion

        #region Processing
		/// <summary>
        /// Clears Job related variables.
        /// </summary>
        public void Clear () {
			_flags.Clear ();
            offsets.Clear ();
			scales.Clear ();
            rotations.Clear ();
            bendings.Clear ();
			ids.Clear ();
            starts.Clear ();
            lengths.Clear ();
			ClearMesh ();
        }
        /// <summary>
        /// Clears Mesh related variables.
        /// </summary>
        public void ClearMesh () {
            vertices.Clear ();
            normals.Clear ();
            tangents.Clear ();
            targetMesh = null;
        }
        public void SetTargetMesh (Mesh mesh) {
            ClearMesh ();
            targetMesh = mesh;
            vertices.AddRange (mesh.vertices);
            normals.AddRange (mesh.normals);
            tangents.AddRange (mesh.tangents);
        }
        public Mesh GetTargetMesh () {
            return targetMesh;
        }
		public void AddTransform (
			int vertexStart, 
			int vertexLength, 
			Vector3 offset, 
			float scale, 
			Quaternion rotation)
		{
			AddTransform (vertexStart, vertexLength, offset, new Vector3(scale, scale, scale), rotation);
		}
        public void AddTransform (
			int vertexStart, 
			int vertexLength, 
			Vector3 offset, 
			Vector3 scale, 
			Quaternion rotation) 
		{
            starts.Add (vertexStart);
            lengths.Add (vertexLength);
            offsets.Add (offset);
			scales.Add (scale);
            rotations.Add (rotation);
            bendings.Add (0f);
			ids.Add (Vector4.zero);
			int flag = MeshJobImpl.APPLY_OFFSET | MeshJobImpl.APPLY_SCALE | MeshJobImpl.APPLY_ROTATION;
			_flags.Add (flag);

        }
		public void IncludeBending (float bending) {
			int lastIndex = _flags.Count - 1;
			bendings [lastIndex] = bending;
			_flags [lastIndex] |= MeshJobImpl.APPLY_BEND;
		}
		public void IncludeIds (int group, int subgroup = -1) {
			int lastIndex = _flags.Count - 1;
			ids [lastIndex] = new Vector4 (group, subgroup, 0f, 0f);
			_flags [lastIndex] |= MeshJobImpl.APPLY_IDS;
		}
        public void ExecuteJob () {
			// Mark the mesh as dynamic.
			targetMesh.MarkDynamic ();
			// Create the job.
			MeshJobImpl _meshJob = new MeshJobImpl () {
				applyTranslationAtEnd = applyTranslationAtEnd,
				offset = new NativeArray<Vector3> (offsets.ToArray (), Allocator.TempJob),
				scale = new NativeArray<Vector3> (scales.ToArray (), Allocator.TempJob),
				orientation = new NativeArray<Quaternion> (rotations.ToArray (), Allocator.TempJob),
				bending = new NativeArray<float> (bendings.ToArray (), Allocator.TempJob),
				ids = new NativeArray<Vector4> (ids.ToArray (), Allocator.TempJob),
				start = new NativeArray<int> (starts.ToArray (), Allocator.TempJob),
				length = new NativeArray<int> (lengths.ToArray (), Allocator.TempJob),
				flags = new NativeArray<int> (_flags.ToArray (), Allocator.TempJob),
				vertices = new NativeArray<Vector3> (vertices.ToArray (), Allocator.TempJob),
				normals = new NativeArray<Vector3> (normals.ToArray (), Allocator.TempJob),
				tangents = new NativeArray<Vector4> (tangents.ToArray (), Allocator.TempJob),
				uvIds = new NativeArray<Vector4> (new Vector4[(applyIdsChannel>0?vertices.Count:0)], Allocator.TempJob)
			};
			// Execute the job .
			JobHandle _meshJobHandle = _meshJob.Schedule (offsets.Count, batchSize);

			// Complete the job.
			_meshJobHandle.Complete();

			targetMesh.SetVertices (_meshJob.vertices);
			targetMesh.SetNormals (_meshJob.normals);
			targetMesh.SetTangents (_meshJob.tangents);
			if (applyIdsChannel > 0) {
				targetMesh.SetUVs (applyIdsChannel, _meshJob.uvIds);
			}
			targetMesh.UploadMeshData (true);

			// Dispose allocated memory.
			_meshJob.offset.Dispose ();
			_meshJob.scale.Dispose ();
			_meshJob.orientation.Dispose ();
			_meshJob.bending.Dispose ();
			_meshJob.ids.Dispose ();
			_meshJob.start.Dispose ();
			_meshJob.length.Dispose ();
			_meshJob.flags.Dispose ();
			_meshJob.vertices.Dispose ();
			_meshJob.normals.Dispose ();
			_meshJob.tangents.Dispose ();
			_meshJob.uvIds.Dispose ();
        }
        #endregion
    }   
}
