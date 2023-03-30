using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Unity.Jobs;
using Unity.Collections;

using Broccoli.Base;
using Broccoli.Model;
using Broccoli.Utils;
using Broccoli.Pipe;

namespace Broccoli.Builder
{
	/// <summary>
	/// Mesh building for sprouts.
	/// </summary>
	public class AdvancedSproutMeshBuilder {
		#region Jobs
		/// <summary>
		/// Job structure to process branch skins.
		/// </summary>
		struct SproutJob : IJobParallelFor {
			#region Globals
			public Vector3 gravityForward;
			public Vector3 gravityRight;
			public Vector3 gravityUp;
			/// <summary>
			/// Bend mode to combine both forward and side bending.
			/// 0 = add, side bending is applied, then forward bending.
			/// 1 = multiply, forward and side quaternion are multiplied.
			/// 2 = stylized, forward and side blending are lerped.
			/// </summary>
			public int bendMode;
			#endregion

			#region Input
			/// <summary>
			/// START for the group of vertices.
			/// </summary>
			public NativeArray<int> start;
			/// <summary>
			/// LENGTH for the group of vertices.
			/// </summary>
			public NativeArray<int> length;
			/// <summary>
			/// Contains the POSITION (x, y, z) and SCALE (w) of the sprout.
			/// </summary>
			public NativeArray<Vector4> sPosition_sScale;
			/// <summary>
			/// Contains the DIRECTION and NORMAL as a quaternion of the sprout.
			/// </summary>
			public NativeArray<Quaternion> sRotation;
			/// <summary>
			/// Contains the BRANCH_ID, BRANCH_POS, SPROUT_FORWARD_BEND, SPROUT_SIDE_BEND of the sprout.
			/// </summary>
			public NativeArray<Vector4> bId_bPos_sFBend_sSBend;
			/// <summary>
			/// Contains the SPROUT_ANCHOR, SPROUT_ID.
			/// </summary>
			public NativeArray<Vector4> sAnchor_sId;
			/// <summary>
			/// Contains the BRANCH_PHASE, BRANCH_PHASE_POS, ACCUM_LENGTH, RANDOM (0-1).
			/// </summary>
			public NativeArray<Vector4> bPhase_bPhasePos_accumLength_rand;
			/// <summary>
			/// Table of random values provided to assign to the sprouts/planes.
			/// </summary>
			[NativeDisableParallelForRestriction]
			public NativeArray<float> randomTable;
			#endregion

			#region Mesh Data
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
			/// UV2s ch1 for the input mesh.
			/// INPUT
			/// x: forward gradient (0-1) from sprout origin.
			/// y: side gradient (0-1) from sprout forward middle line to the sides.
			/// z: plane id.
			/// w: mesh id.
			/// 
			/// OUTPUT
			/// x: forward gradient (0-1) from sprout origin.
			/// y: side gradient (0-1) from sprout forward middle line to the sides.
			/// z: random value (0-1).
			/// w: mesh id.
			/// </summary>
			[NativeDisableParallelForRestriction]
			public NativeArray<Vector4> uv2s;

			/// <summary>
			/// UV3s ch4 for the output mesh.
			/// xyz: sprout anchor point.
			/// w: sprout id.
			/// </summary>
			[NativeDisableParallelForRestriction]
			public NativeArray<Vector4> uv3s;

			/// <summary>
			/// UV4s for the output mesh.
			/// x: branch id.
			/// y: accumulated length from the trunk.
			/// z: branch phase.
			/// w: branch phase position.
			/// </summary>
			[NativeDisableParallelForRestriction]
			public NativeArray<Vector4> uv4s;

			/// <summary>
			/// UV5s for the output mesh.
			/// x: id of the branch.
			/// y: id of sprout.
			/// z: unallocated.
			/// w: unallocated.
			/// </summary>
			[NativeDisableParallelForRestriction]
			public NativeArray<Vector4> uv5s;
			#endregion

			#region Job Methods
			/// <summary>
			/// Executes one per sprout.
			/// </summary>
			/// <param name="i"></param>
			public void Execute (int i) {
				int vertexStart = start [i];
				int vertexEnd = start [i] + length [i];

				Vector3 spOffset = (Vector3)sPosition_sScale [i];
				Quaternion spRotation = sRotation [i];
				float spScale = sPosition_sScale [i].w;
				
				int bId = (int)bId_bPos_sFBend_sSBend [i].x;
				float spPos = bId_bPos_sFBend_sSBend [i].y;
				float spFBend = bId_bPos_sFBend_sSBend [i].z;
				float spSBend = bId_bPos_sFBend_sSBend [i].w;

				Vector3 spAnchor = sAnchor_sId [i];
				int spId = (int)sAnchor_sId [i].w;
				float bPhase = bPhase_bPhasePos_accumLength_rand [i].x;
				float bPhasePos = bPhase_bPhasePos_accumLength_rand [i].y;
				float accumLength = bPhase_bPhasePos_accumLength_rand [i].z;
				float spRand = bPhase_bPhasePos_accumLength_rand [i].w;

				if (spFBend != 0 || spSBend != 0)
					ApplyBend (vertexStart, vertexEnd, spFBend, spSBend);
				ApplyScale (vertexStart, vertexEnd, spScale);
				ApplyRotation (vertexStart, vertexEnd, spRotation);
				ApplyOffset (vertexStart, vertexEnd, spOffset);
				ApplyUV (vertexStart, vertexEnd, spId, accumLength, bPhasePos, bId, bPhase, spAnchor, spRand);
			}
			public static float[] GetRandomTable (float minRange, float maxRange, int count = 25) {
				if (count < 0) count = 1;
				float[] randomTable = new float[count];
				for (int i = 0; i < count; i++) {
					randomTable [i] = Random.Range (minRange, maxRange);
				}
				return randomTable;
			}
			private void ApplyBend (int vertexStart, int vertexEnd, float fBend, float sBend) {
				/*
				if (fBend < 0) {
					gravityForward *= -1;
					gravityUp *= -1;
					fBend *= -1;
					sBend *= -1;
				}
				*/
				/*
				Quaternion fGravityQuaternion = Quaternion.LookRotation (gravityUp * -1, gravityForward);
				Quaternion fAntigravityQuaternion = Quaternion.LookRotation (gravityUp, -gravityForward);
				*/
				Quaternion fGravityQuaternion = Quaternion.FromToRotation (gravityUp * -1, gravityForward);
				Quaternion fAntigravityQuaternion = Quaternion.FromToRotation (gravityUp, gravityForward);
				Quaternion fBendQuaternion;
				Quaternion sGravityQuaternion = Quaternion.FromToRotation (gravityUp * -1, gravityRight);
				Quaternion sAntigravityQuaternion = Quaternion.FromToRotation (gravityUp, gravityRight);
				Quaternion sBendQuaternion;
				Quaternion bendQuaternion;
				float forwardStrength;
				float sideStrength;
				float bitan = 0;
				Vector4 tangent;
				for (int i = vertexStart; i < vertexEnd; i++) {
					//Quaternion gravityQuaternion = Quaternion.FromToRotation (vertices [i].normalized, gravityUp * -1);
					forwardStrength = fBend * uv2s[i].x;
					// EaseOutCirc
					//radialStrength = Mathf.Sqrt (1f - Mathf.Pow (radialStrength - 1, 2));
					// EaseInCirc
					//radialStrength = 1f - Mathf.Sqrt (1f - Mathf.Pow (radialStrength, 2));
					// EaseInSine
					//radialStrength = 1f - Mathf.Cos ((radialStrength * Mathf.PI) / 2f);
					// EaseOutSine
					// radialStrength = Mathf.Sin((radialStrength * Mathf.PI) / 2f);
					// EaseInOutCubiv
					//radialStrength = (radialStrength < 0.5f ? 4f * radialStrength * radialStrength * radialStrength : 1f - Mathf.Pow(-2f * radialStrength + 2f, 3f) / 2f);
					//forwardStrength *= 0.6f;
					//bendQuaternion = Quaternion.Slerp (Quaternion.identity, gravityQuaternion, forwardStrength);
					if (forwardStrength > 0f)
						fBendQuaternion = Quaternion.Slerp (Quaternion.identity, (vertices [i].z < 0 ? fGravityQuaternion : fAntigravityQuaternion), forwardStrength);
					else
						fBendQuaternion = Quaternion.Slerp (Quaternion.identity, (vertices [i].z < 0 ? fAntigravityQuaternion : fGravityQuaternion), -forwardStrength); 
						
					//fBendQuaternion = Quaternion.Slerp (Quaternion.identity, fGravityQuaternion, forwardStrength);
					sideStrength = sBend * uv2s[i].y;
					//sideStrength *= 0.6f;
					if (sideStrength > 0f)
						sBendQuaternion = Quaternion.Slerp (Quaternion.identity, (vertices [i].x < 0 ? sGravityQuaternion : sAntigravityQuaternion), sideStrength);
					else
						sBendQuaternion = Quaternion.Slerp (Quaternion.identity, (vertices [i].x < 0 ? sAntigravityQuaternion: sGravityQuaternion), -sideStrength);
					//sBendQuaternion = Quaternion.Slerp (Quaternion.identity, sGravityQuaternion, sideStrength);
					//sBendQuaternion = Quaternion.Slerp (Quaternion.identity, sGravityQuaternion, sideStrength);

					if (bendMode > 0) {
						if (bendMode == 2) {
							// STYLIZED bending mode.
							float lerpBend = Mathf.Atan2 (uv2s[i].y, uv2s[i].x) / (Mathf.PI * 0.5f);
							bendQuaternion = Quaternion.Lerp (fBendQuaternion, sBendQuaternion, lerpBend);		
						} else {
							// MULTIPLY bending mode.
							bendQuaternion = sBendQuaternion * fBendQuaternion;
						}
						vertices [i] = bendQuaternion * vertices [i];
						normals [i] = bendQuaternion * normals [i];
						bitan = tangents [i].w;
						tangent = bendQuaternion * tangents [i];
						tangent.w = bitan;
						tangents [i] = tangent;
					} else {
						// ADDITIVE bending mode.
						vertices [i] = sBendQuaternion * vertices [i];
						normals [i] = sBendQuaternion * normals [i];
						bitan = tangents [i].w;
						tangent = sBendQuaternion * tangents [i];
						tangent.w = bitan;
						tangents [i] = tangent;

						vertices [i] = fBendQuaternion * vertices [i];
						normals [i] = fBendQuaternion * normals [i];
						bitan = tangents [i].w;
						tangent = fBendQuaternion * tangents [i];
						tangent.w = bitan;
						tangents [i] = tangent;
					}
				}
			}
			public void ApplyScale (int vertexStart, int vertexEnd, float scale) {
				for (int i = vertexStart; i < vertexEnd; i++) {
					vertices [i] = vertices [i] * scale;
				}
			}
			public void ApplyRotation (int vertexStart, int vertexEnd, Quaternion orientation) {
				float bitan = 0;
				Vector4 tangent;
				for (int i = vertexStart; i < vertexEnd; i++) {
					vertices [i] = orientation * vertices [i];
					normals [i] = orientation * normals [i];
					bitan = tangents [i].w;
					tangent = orientation * tangents [i];
					tangent.w = bitan;
					tangents [i] = tangent;
				}
			}
			public void ApplyOffset (int vertexStart, int vertexEnd, Vector3 offset) {
				for (int i = vertexStart; i < vertexEnd; i++) {
					vertices [i] = vertices [i] + offset;
				}
			}
			public void ApplyUV (
				int vertexStart, 
				int vertexEnd, 
				int sproutId, 
				float accumLength,
				float branchPhasePos,
				int branchId,
				float branchPhase,
				Vector3 sproutAnchor,
				float randomValue)
			{
				Vector4 uv3;
				Vector4 uv4;
				Vector4 uv2;
				for (int i = vertexStart; i < vertexEnd; i++) {
					uv2 = uv2s [i];
					uv2.z = GetRandomValue (randomValue, uv2.z);
					uv2s [i] = uv2;

					uv3 = sproutAnchor;
					uv3.w = sproutId;
					uv3s [i] = uv3;

					uv4 = uv4s [i];
					uv4.x = branchId;
					uv4.y = accumLength;
					uv4.z = branchPhase;
					uv4.w = branchPhasePos;
					uv4s [i] = uv4;

					uv5s [i] = new Vector4 (branchId, sproutId, 0f, 0f);
				}
			}
			private float GetRandomValue (float baseRand, float offset) {
				float randomIndex = baseRand * randomTable.Length + offset;
				return randomTable [(int)(randomIndex % randomTable.Length)];
			}
			#endregion
		}
		#endregion

		#region Mesh&Jobs Vars
		/// <summary>
		/// Scale for all the meshes generated.
		/// </summary>
		public float globalScale = 1.0f;
		/// <summary>
		/// Relationship between registered meshes and their ids.
		/// </summary>
		/// <typeparam name="int">Id of the mesh, compound for groupId * 10000 + subgroupId.</typeparam>
		/// <typeparam name="Mesh">Mesh.</typeparam>
		/// <returns>Relationship between meshes and their ids.</returns>
		Dictionary<int, Mesh> _idToMesh = new Dictionary<int, Mesh> ();
		/// <summary>
		/// Contains the START of a group of vertices.
		/// </summary>
		private List<int> _start = new List<int> ();
		/// <summary>
		/// Contains the LENGTH of a group of vertices.
		/// </summary>
		private List<int> _length = new List<int> ();
		/// <summary>
		/// Contains the POSITION (x, y, z) and SCALE (w) of the sprout.
		/// </summary>
		private List<Vector4> _sproutPositionScale = new List<Vector4> ();
		/// <summary>
		/// Contains the DIRECTION and NORMAL of the sprout as a quaternion.
		/// </summary>
		private List<Quaternion> _sproutRotation = new List<Quaternion> ();
		/// <summary>
		/// Contains the BRANCH_ID, BRANCH_POS, SPROUT_FORWARD_BEND, SPROUT_SIDE_BEND of the sprout.
		/// </summary>
		private List<Vector4> _branchIdBranchPosSproutFBendSproutSBend = new List<Vector4> ();
		/// <summary>
		/// Contains the SPROUT_ANCHOR and SPROUT_ID.
		/// </summary>
		private List<Vector4> _branchAnchorSproutId = new List<Vector4> ();
		/// <summary>
		/// Contains the BRANCH_PHASE, BRANCH_PHASE_POS, ACCUM_LENGTH and RANDOM (0-1).
		/// </summary>
		public List<Vector4> _phasePhasePosAccumLengthRand = new List<Vector4> ();
		#endregion

		#region Singleton
		/// <summary>
		/// Singleton for this class.
		/// </summary>
		static AdvancedSproutMeshBuilder _sproutMeshBuilder = null;
		/// <summary>
		/// Gets the singleton instance for this class.
		/// </summary>
		/// <returns>The instance.</returns>
		public static AdvancedSproutMeshBuilder GetInstance() {
			if (_sproutMeshBuilder == null) {
				_sproutMeshBuilder = new AdvancedSproutMeshBuilder ();
			}
			return _sproutMeshBuilder;
		}
		#endregion

		#region Meshes Management
		/// <summary>
		/// Bounds a mesh to  group id and subgroup id.
		/// </summary>
		/// <param name="mesh">Mesh to register.</param>
		/// <param name="groupId">Group id.</param>
		/// <param name="subgroupId">Subgroup id.</param>
		public void RegisterMesh (Mesh mesh, int groupId, int subgroupId = -1) {
			int meshId = GetGroupSubgroupId (groupId, subgroupId);
			if (_idToMesh.ContainsKey (meshId)) {
				UnityEngine.Object.DestroyImmediate (_idToMesh [meshId]);
				_idToMesh.Remove (meshId);
			}
			_idToMesh.Add (meshId, UnityEngine.Object.Instantiate (mesh));
		}
		/// <summary>
		/// Remove all registered meshes.
		/// </summary>
		public void RemoveRegisteredMeshes () {
			var meshEnum = _idToMesh.GetEnumerator ();
			while (meshEnum.MoveNext ()) {
				UnityEngine.Object.DestroyImmediate (meshEnum.Current.Value);
			}
			_idToMesh.Clear ();
		}
		/// <summary>
		/// Builds the compound id between groupId/subgroupId.
		/// </summary>
		/// <param name="mesh">Mesh to register.</param>
		/// <param name="groupId">Group id.</param>
		/// <returns>Compound id.</returns>
		private int GetGroupSubgroupId (int groupId, int subgroupId) {
			if (subgroupId < 0) subgroupId = -1;
			int meshId =  groupId * 10000 + (subgroupId + 1);
			return meshId;
		}
		#endregion

		#region Initialization
		/// <summary>
		/// Clear local variables.
		/// </summary>
		public void Clear () {
			RemoveRegisteredMeshes ();
			ClearSproutParams ();
		}
		/// <summary>
		/// Clear the sprout params lists.
		/// </summary>
		private void ClearSproutParams () {
			_start.Clear ();
			_length.Clear ();
			_sproutPositionScale.Clear ();
			_sproutRotation.Clear ();
			_branchIdBranchPosSproutFBendSproutSBend.Clear ();
			_branchAnchorSproutId.Clear ();
			_phasePhasePosAccumLengthRand.Clear ();
		}
		#endregion

		#region Processing
		/// <summary>
		/// Creates the mesh coming from a sprout group on a tree instance.
		/// </summary>
		/// <returns>The mesh object for the sprouts.</returns>
		/// <param name="tree">Tree object.</param>
		/// <param name="subgroupId">Sprout mesh instance.</param>
		/// <param name="groupId">Sprout group id.</param>
		public Mesh MeshSprouts (
			BroccoTree tree, 
			SproutMesh sproutMesh, 
			int groupId, 
			int subgroupId = -1)
		{
			// Mesh to build.
			Mesh groupMesh = new Mesh ();

			// Generate mesh if a registered mesh is available to the groupId/subgroupId.
			int meshId = GetGroupSubgroupId (groupId, subgroupId);
			if (_idToMesh.ContainsKey (meshId)) {
				// Get the mesh to use as source for the group/subgroup.
				Mesh srcMesh = _idToMesh [meshId];

				// Clear the list to feed the job.
				ClearSproutParams ();

				// Get all branches/roots in the tree to populate sprouts with groupId/subgroupId duple.
				int vertexIndex = 0;
				int vertexCount = srcMesh.vertexCount;
				int sproutCount = 0;
				List<BroccoTree.Branch> branches = tree.GetDescendantBranches ();
				for (int i = 0; i < branches.Count; i++) {
					for (int j = 0; j < branches[i].sprouts.Count; j++) {
						if (branches[i].sprouts[j].groupId == groupId && 
							(subgroupId>=0?branches[i].sprouts[j].subgroupId == subgroupId:true))
						{
							// Set the sprout horizontal alignment.
							branches[i].sprouts[j].horizontalAlign = 
								Mathf.Lerp (sproutMesh.horizontalAlignAtBase, 
									sproutMesh.horizontalAlignAtTop, 
									branches[i].sprouts[j].position);
							// Recalculate sprouts direction and normal.
							branches[i].sprouts[j].CalculateVectors ();
							// Add sprout bound to the mesh to the job system.
							AddSprout (vertexIndex, vertexCount, branches[i].sprouts[j], branches[i], sproutMesh);
							vertexIndex += vertexCount;
							sproutCount++;
						}
					}
				}

				// Merge the sprouts into the the group mesh.
				groupMesh = MeshUtils.CombineMeshesMultiplicative (srcMesh, sproutCount, true);
				groupMesh.MarkDynamic ();

				// Create Job.
				//List<Vector4> groupMeshUVs = new List<Vector4> ();
				List<Vector4> groupMeshUV2s = new List<Vector4> ();
				//groupMesh.GetUVs (0, groupMeshUVs);
				groupMesh.GetUVs (1, groupMeshUV2s);
				int totalVertices = groupMesh.vertexCount;

				SproutJob _sproutJob = new SproutJob () {
					// Global.
					gravityForward = Vector3.forward,
					gravityRight = Vector3.right,
					gravityUp = Vector3.up,
					bendMode = (int)sproutMesh.gravityBendMode,
					randomTable = new NativeArray<float> (SproutJob.GetRandomTable (0f, 1f), Allocator.TempJob),

					// Input.
					start = new NativeArray<int> (_start.ToArray (), Allocator.TempJob),
					length = new NativeArray<int> (_length.ToArray (), Allocator.TempJob),
					sPosition_sScale = new NativeArray<Vector4> (_sproutPositionScale.ToArray (), Allocator.TempJob),
					sRotation = new NativeArray<Quaternion> (_sproutRotation.ToArray (), Allocator.TempJob),
					bId_bPos_sFBend_sSBend = new NativeArray<Vector4> (_branchIdBranchPosSproutFBendSproutSBend.ToArray (), Allocator.TempJob),
					sAnchor_sId = new NativeArray<Vector4> (_branchAnchorSproutId.ToArray (), Allocator.TempJob),
					bPhase_bPhasePos_accumLength_rand = new NativeArray<Vector4> (_phasePhasePosAccumLengthRand.ToArray (), Allocator.TempJob),

					// Mesh Data.
					vertices = new NativeArray<Vector3> (groupMesh.vertices, Allocator.TempJob),
					normals = new NativeArray<Vector3> (groupMesh.normals, Allocator.TempJob),
					tangents = new NativeArray<Vector4> (groupMesh.tangents, Allocator.TempJob),
					uv2s = new NativeArray<Vector4> (groupMeshUV2s.ToArray (), Allocator.TempJob),
					uv3s = new NativeArray<Vector4> (totalVertices, Allocator.TempJob),
					uv4s = new NativeArray<Vector4> (totalVertices, Allocator.TempJob),
					uv5s = new NativeArray<Vector4> (totalVertices, Allocator.TempJob)
				};

				// Execute the branch jobs.
				JobHandle _sproutJobHandle = _sproutJob.Schedule (_sproutPositionScale.Count, 8);

				// Complete the job.
				_sproutJobHandle.Complete();

				groupMesh.SetVertices (_sproutJob.vertices);
				groupMesh.SetNormals (_sproutJob.normals);
				groupMesh.SetTangents (_sproutJob.tangents);
				groupMesh.SetUVs (1, _sproutJob.uv2s);
				groupMesh.SetUVs (2, _sproutJob.uv3s);
				groupMesh.SetUVs (3, _sproutJob.uv4s);
				groupMesh.SetUVs (4, _sproutJob.uv5s);

				groupMesh.UploadMeshData (false);

				// Dispose allocated memory.
				_sproutJob.start.Dispose ();
				_sproutJob.length.Dispose ();
				_sproutJob.sPosition_sScale.Dispose ();
				_sproutJob.sRotation.Dispose ();
				_sproutJob.bId_bPos_sFBend_sSBend.Dispose ();
				_sproutJob.sAnchor_sId.Dispose ();
				_sproutJob.bPhase_bPhasePos_accumLength_rand.Dispose ();
				_sproutJob.vertices.Dispose ();
				_sproutJob.normals.Dispose ();
				_sproutJob.tangents.Dispose ();
				_sproutJob.uv2s.Dispose ();
				_sproutJob.uv3s.Dispose ();
				_sproutJob.uv4s.Dispose ();
				_sproutJob.uv5s.Dispose ();
				_sproutJob.randomTable.Dispose ();
			}

			// Return the mesh.
			return groupMesh;
		}
		/// <summary>
		/// Adds information on the mesh creation for a single sprout.
		/// </summary>
		/// <param name="sprout">Sprout instance.</param>
		/// <param name="branch">Parent branch.</param>
		/// <param name="sproutMesh">Sprout mesh instance.</param>
		private void AddSprout (int vertexStart, int vertexLength, BroccoTree.Sprout sprout, BroccoTree.Branch branch, SproutMesh sproutMesh) {
			_start.Add (vertexStart);
			_length.Add (vertexLength);

			float scale = Mathf.Lerp (sproutMesh.scaleAtBase, sproutMesh.scaleAtTop, 
				Mathf.Clamp (sproutMesh.scaleCurve.Evaluate(sprout.preferedPosition), 0f, 1f));

			sprout.meshHeight = globalScale * scale * sproutMesh.overridedHeight * (1f - sproutMesh.pivotY);


			Vector3 pos = sprout.inGirthPosition * globalScale;
			_sproutPositionScale.Add (new Vector4 (pos.x, pos.y, pos.z, scale * globalScale));

			_sproutRotation.Add (Quaternion.LookRotation (sprout.sproutDirection, sprout.sproutNormal));

			Vector4 sproutAnchorSproutId = branch.GetPointAtPosition (sprout.position) * globalScale;
			sproutAnchorSproutId.w = sprout.helperSproutId;
			_branchAnchorSproutId.Add (sproutAnchorSproutId);

			float sproutFBend = 0f;
			float sproutSBend = 0f;
			if (sproutMesh.hasBending) {
				sproutFBend = Mathf.Lerp (sproutMesh.gravityBendingAtBase, 
					sproutMesh.gravityBendingAtTop, sprout.hierarchyPosition);
				sproutFBend *= sproutMesh.gravityBendingCurve.Evaluate (sprout.hierarchyPosition);
				sproutSBend = Mathf.Lerp (sproutMesh.sideGravityBendingAtBase, 
					sproutMesh.sideGravityBendingAtTop, sprout.hierarchyPosition);
				sproutSBend *= sproutMesh.sideGravityBendingShape.Evaluate (sprout.hierarchyPosition);
			}
			_branchIdBranchPosSproutFBendSproutSBend.Add (new Vector4 (branch.id, sprout.position, sproutFBend, sproutSBend));
			_phasePhasePosAccumLengthRand.Add (
				new Vector4 (
					branch.phase, 
					branch.GetPhasePosition (sprout.position), 
					branch.GetLengthAtPos (sprout.position, true),
					Random.Range (0f, 1f)));
		}
		#endregion

		#region Subgroups
		/// <summary>
		/// Assign Sprouts belonging to a sprout group to their subgroup (thus with a mapping area).
		/// </summary>
		/// <param name="tree">Tree object to traverse the Sprout objects.</param>
		/// <param name="sproutGroupId">Sprout group id.</param>
		/// <param name="sproutMap">Sprout map instance.</param>
		public void AssignSproutSubgroups (BroccoTree tree, int sproutGroupId, SproutMap sproutMap) {
			sproutMap.NormalizeAreas ();
			List<int> enabledSubgroups = new List<int> ();
			for (int i = 0; i < sproutMap.sproutAreas.Count; i++) {
				if (sproutMap.sproutAreas[i].enabled && 
					sproutMap.sproutAreas[i].texture != null) {
					enabledSubgroups.Add (i);
				}
			}
			int maxAreaIndex = enabledSubgroups.Count;
			List<BroccoTree.Branch> branches = tree.GetDescendantBranches ();
			for (int i = 0; i < branches.Count; i++) {
				for (int j = 0; j < branches[i].sprouts.Count; j++) {
					if (branches[i].sprouts[j].groupId == sproutGroupId) {
						if (maxAreaIndex > 0) {
							branches[i].sprouts[j].subgroupId = 
								enabledSubgroups [Random.Range (0, maxAreaIndex)];
						} else {
							branches[i].sprouts[j].subgroupId = -1;
						}
					}
				}
			}
		}
		/// <summary>
		/// Assign Sprouts belonging to a sprout group to their subgroup (thus with a collection snapshot).
		/// </summary>
		/// <param name="tree">Tree object to traverse the Sprout objects.</param>
		/// <param name="sproutGroupId">Sprout group id.</param>
		/// <param name="branchCollection">Branch collection containing the snapshots.</param>
		public void AssignSproutSubgroups (BroccoTree tree, int sproutGroupId, BranchDescriptorCollection branchCollection, SproutMesh sproutMesh) {
			List<int> enabledSubgroups = new List<int> ();
			for (int i = 0; i < branchCollection.branchDescriptors.Count; i++) {
				enabledSubgroups.Add (i);
			}
			sproutMesh.subgroups = enabledSubgroups.ToArray ();
			
			int maxSnapshotIndex = enabledSubgroups.Count;
			List<BroccoTree.Branch> branches = tree.GetDescendantBranches ();
			for (int i = 0; i < branches.Count; i++) {
				for (int j = 0; j < branches[i].sprouts.Count; j++) {
					if (branches[i].sprouts[j].groupId == sproutGroupId) {
						if (maxSnapshotIndex > 0) {
							branches[i].sprouts[j].subgroupId = enabledSubgroups [Random.Range (0, maxSnapshotIndex)];
						} else {
							branches[i].sprouts[j].subgroupId = -1;
						}
					}
				}
			}
		}
		public void SetMapArea (SproutMap.SproutMapArea sproutArea) {
			
		}
		#endregion
	}
}