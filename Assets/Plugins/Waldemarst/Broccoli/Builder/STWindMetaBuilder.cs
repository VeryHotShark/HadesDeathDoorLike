using System.Collections.Generic;

using UnityEngine;
using Unity.Jobs;
using Unity.Collections;

using Broccoli.Model;
using Broccoli.Manager;

namespace Broccoli.Builder
{
	/// <summary>
	/// Wind meta builder.
	/// Analyzes trees to provide wind weight based on UV2 and colors channels.
	/// </summary>
	/// Leaves UV ch0
	/// z: Up/down branch swing
	/// Leaves UV4 ch.3 
	/// x: leaf sway factor with gradient from leaves origin.
	public class STWindMetaBuilder {
		#region Wind Jobs
		struct BranchWindJob : IJobParallelFor {
			/// <summary>
			/// Vertices for the mesh.
			/// </summary>
			public NativeArray<Vector3> vertices;

			/// <summary>
			/// UV information of the mesh.
			/// INPUT:
			/// x: mapping U component.
			/// y: mapping V component.
			/// z: radial position.
			/// w: girth.
			/// 
			/// OUTPUT:
			/// x: mapping U component.
			/// y: mapping V component.
			/// z: Adds wind data: gradient from trunk to branchskin tip.
			/// w: Adds wind data: branch swing phase. (0-15).
			/// </summary>
			public NativeArray<Vector4> uvs;

			/// <summary>
			/// UV2 information of the mesh.
			/// INPUT
			/// x: global length position.
			/// y: phase.
			/// z: phase position.
			/// w: accum length.
			/// 
			/// OUTPUT:
			/// x: phase 1 value (0-1).
			/// y: phase 2 value (0-1).
			/// z: Adds wind data: x phased values (0-5).
			/// w: Adds wind data: y phased values (0-14).
			/// </summary>
			public NativeArray<Vector4> uv2s;

			/// <summary>
			/// UV3 information of the mesh.
			/// x, y, z: vertex position.
			/// w: unallocated.
			/// </summary>
			[NativeDisableParallelForRestriction]
			public NativeArray<Vector4> uv3s;

			/* FORMERLY
			/// UV5 information of the mesh.
			/// x: radial position.
			/// y: global length position.
			/// z: girth.
			/// w: unallocated.
			/// 
			/// UV6 information of the mesh.
			/// x: id of the branch.
			/// y: id of the branch skin.
			/// z: id of the struct.
			/// w: tuned.
			/// 
			/// UV7 information of the mesh.
			/// x, y, z: center.
			/// w: unallocated.
			/// 
			/// branchPhases Saves the branch phase for each vertex.
			/// x: phase value.
			/// y: phase min length.
			/// z: phase max length.
			*/

			public bool isST7;
			public float windAmplitude;
			public float branchSway;
			public void Execute(int i) {
				// Get channel values.
				Vector3 vertex = vertices[i];
				Vector4 uv = uvs[i];
				Vector4 uv2 = uv2s[i];
				Vector4 uv3 = uv3s[i];

				// Set UV if branch.
				if (uv2.w < 1f) {
					uv.z = uv2.z * branchSway; // Length from tree origin (0-1)
					uv.w = uv2.y * 15f;
				} else {
					uv.z = 0f;
					uv.w = 0f;
				}
				

				if (isST7) {
					// Set UV2, holds vertex position.
					uv2 = vertex;
					uv2.y *= windAmplitude;
					uv2.w = 0;
				} else {
					// Set UV2.
					float phase = uv2.y;
					uv2.x = 0.5f + phase * 0.5f;
					uv2.y = 0.5f + (1f - phase) * 0.5f;
					uv2.z = vertex.x;
					uv2.w = vertex.y;
				}

				uv3 = vertex;

				uvs [i] = uv;
				uv2s [i] = uv2;
				uv3s [i] = vertex;
			}
		}
		struct SproutWindJob : IJobParallelFor {
			/// <summary>
			/// Vertices for the mesh.
			/// </summary>
			public NativeArray<Vector3> vertices;

			/// <summary>
			/// UV information of the mesh.
			/// INPUT:
			/// x: mapping U component.
			/// y: mapping V component.
			/// x: mapping U component (normalized 0-1).
			/// y: mapping V component (normalized 0-1).
			/// 
			/// OUTPUT:
			/// x: mapping U component.
			/// y: mapping V component.
			/// z: Adds wind data: gradient from trunk to branchskin tip.
			/// w: Adds wind data: branch swing phase. (0-15).
			/// </summary>
			public NativeArray<Vector4> uvs;

			/// <summary>
			/// UV2s ch1 for the input mesh.
			/// INPUT
			/// x: forward gradient (0-1) from sprout origin.
			/// y: side gradient (0-1) from sprout forward middle line to the sides.
			/// z: random value (0-1).
			/// w: mesh id.
			/// 
			/// OUTPUT
			/// x: Random Phase A (0.5-1).
			/// y: Random Phase B (0.5-1).
			/// z: Sprout anchor X value.
			/// w: Sprout anchor Y value.
			/// </summary>
			[NativeDisableParallelForRestriction]
			public NativeArray<Vector4> uv2s;

			/// <summary>
			/// UV3s ch2 for the output mesh.
			/// INPUT
			/// xyz: sprout anchor point.
			/// w: sprout id.
			/// 
			/// OUTPUT
			/// x: X vertex position.
			/// y: Y vertex position.
			/// z: Z vertex position.
			/// w: Sprout anchor Z value.
			/// </summary>
			[NativeDisableParallelForRestriction]
			public NativeArray<Vector4> uv3s;

			/// <summary>
			/// UV4s for the output mesh.
			/// INPUT
			/// x: branch id.
			/// y: accumulated length from the trunk.
			/// z: branch phase.
			/// w: branch phase position.
			/// 
			/// OUTPUT
			/// x: Gradient from anchor point to borders.
			/// y: Random Phase C (0-15).
			/// z: Random Phase D (0-15)
			/// w: sprout = 2.
			/// </summary>
			[NativeDisableParallelForRestriction]
			public NativeArray<Vector4> uv4s;
			public bool isST7;
			public float windAmplitude;
			public float branchSway;
			public float sproutTurbulence;
			public float sproutSway;
			public void Execute(int i) {
				// Get channel values.
				Vector3 vertex = vertices[i];
				Vector4 uv = uvs[i];
				Vector4 uv2 = uv2s[i];
				Vector4 uv3 = uv3s[i];
				Vector4 uv4 = uv4s[i];

				/*
				UV
				INPUT:
				x: mapping U component.
				y: mapping V component.
				x: mapping U component (normalized 0-1).
				y: mapping V component (normalized 0-1).
				OUTPUT:
				x: mapping U component.
				y: mapping V component.
				z: Adds wind data: gradient from trunk to branchskin tip.
				w: Adds wind data: branch swing phase. (0-15).
				
				UV2s ch1 for the input mesh.
				INPUT
				x: forward gradient (0-1) from sprout origin.
				y: side gradient (0-1) from sprout forward middle line to the sides.
				z: random value (0-1).
				w: mesh id.
				OUTPUT
				x: Random Phase A (0.5-1).
				y: Random Phase B (0.5-1).
				z: Sprout anchor X value.
				w: Sprout anchor Y value.
				
				UV3s ch2 for the output mesh.
				INPUT
				xyz: sprout anchor point.
				w: sprout id.
				OUTPUT
				x: X vertex position.
				y: Y vertex position.
				z: Z vertex position.
				w: Sprout anchor Z value.
				
				UV4s for the output mesh.
				INPUT
				x: branch id.
				y: accumulated length from the trunk.
				z: branch phase.
				w: branch phase position.
				OUTPUT
				x: Gradient from anchor point to borders.
				y: Random Phase C (0-15).
				z: Random Phase D (0-15)
				w: sprout = 2.
				*/

				// Set UV.
				uv = new Vector4(uv.x, uv.y, uv4.w * branchSway, uv4.z * 15f);

				float random = uv2.z;
				Vector3 anchor = uv3;
				float gradient = Mathf.Sqrt (uv2.x * uv2.x + uv2.y * uv2.y) / 1.4142f;
				if (isST7) {
					// Set UV2, holds vertex position.
					uv2 = new Vector4 (vertex.x, vertex.y, vertex.z, anchor.z);
					uv3 = new Vector4 (0.5f + random * 0.5f * sproutTurbulence, 0.5f + (1f - random) * 0.5f * sproutSway, random * 15f, 0f);
					uv4 = new Vector4 (gradient, random, 0f, 1f);
				} else {
					// Set UV2.
					uv2 = new Vector4 (0.5f + random * 0.5f * sproutTurbulence, 0.5f + (1f - random) * 0.5f * sproutSway, anchor.x, anchor.y);
					uv3 = new Vector4 (vertex.x, vertex.y, vertex.z, anchor.z);
					uv4 = new Vector4 (gradient, random * 15f, (1f - random) * 15f, 2f);
				}

				uvs [i] = uv;
				uv2s [i] = uv2;
				uv3s [i] = uv3;
				uv4s [i] = uv4;
			}
		}
		#endregion

		#region Vars
		/// <summary>
		/// The branches on the analyzed tree.
		/// </summary>
		/// <typeparam name="int">Id of the branch.</typeparam>
		/// <typeparam name="BroccoTree.Branch">Branch.</typeparam>
		/// <returns>Branch given its id.</returns>
		public Dictionary<int, BroccoTree.Branch> branches = new Dictionary<int, BroccoTree.Branch> ();
		/// <summary>
		/// The wind amplitude.
		/// </summary>
		float _windAmplitude = 0f;
		/// <summary>
		/// Gets or sets the wind resistance.
		/// </summary>
		/// <value>The wind resistance.</value>
		public float windAmplitude {
			get { return _windAmplitude; }
			set {
				weightCurve = AnimationCurve.EaseInOut (value, 0f, 1f, 1f);
				_windAmplitude = value;
			}
		}
		public float sproutTurbulence = 1f;
		public float sproutSway = 1f;
		/// <summary>
		/// The weight curve used to get the UV2 values for wind.
		/// </summary>
		public AnimationCurve weightCurve;
		public AnimationCurve weightSensibilityCurve = null;
		public AnimationCurve weightAngleCurve = null;
		public bool isST7 = false;
		public float branchSway = 1f;
		/// <summary>
		/// True to apply wind mapping to roots.
		/// </summary>
		public bool applyToRoots = false;
		#endregion

		#region Constructor
		/// <summary>
		/// Initializes a new instance of the <see cref="Broccoli.Builder.STWindMetaBuilder"/> class.
		/// </summary>
		public STWindMetaBuilder () {
			weightSensibilityCurve = new AnimationCurve ();
			weightSensibilityCurve.AddKey (new Keyframe ());
			weightSensibilityCurve.AddKey (new Keyframe (1, 1, 2, 2));
		}
		#endregion

		#region Analyze
		/// <summary>
		/// Analyze the tree and its branches to apply wind on this data.
		/// </summary>
		/// <param name="tree">Broccoli tree instance to analyze.</param>
		/// <param name="branchSkins">List of branch skin instances from this tree.</param>
		public void AnalyzeTree (BroccoTree tree, List<BranchMeshBuilder.BranchSkin> branchSkins) {
			// Prepare for analysis.
			Clear ();
			// Analyze branches.
			for (int i = 0; i < tree.branches.Count; i++) {
				AnalyzeBranch (tree.branches[i]);
			}
		}
		void AnalyzeBranch (BroccoTree.Branch branch, int hierarchyLevel = 0, float lengthOffset = 0, float phase = -1f, float minPhaseLength = -1f, float maxPhaseLength = -1f) {
			// Index branch.
			if (!branches.ContainsKey (branch.id)) {
				branches.Add (branch.id, branch);
			}
			// Run analysis on children branches.
			for (int i = 0; i < branch.branches.Count; i++) {
				AnalyzeBranch (branch.branches [i], hierarchyLevel + 1, lengthOffset + (branch.length * branch.branches [i].position), phase);
			}
		}
		float GetMaxPhaseLength (BroccoTree.Branch branch, float lengthOffset = 0f) {
			float maxPhaseLength = branch.length;
			for (int i = 0; i < branch.branches.Count; i++) {
				float childMaxPhaseLength = GetMaxPhaseLength (branch.branches[i], lengthOffset + (branch.length * branch.branches[i].position));
				if (childMaxPhaseLength > maxPhaseLength) maxPhaseLength = childMaxPhaseLength;
			}
			return maxPhaseLength;
		}
		/// <summary>
		/// Clear this instance.
		/// </summary>
		public void Clear () {
			branches.Clear ();
		}
		#endregion

		#region Branch Mesh
		public void SetBranchesWindDataJobs (Mesh mesh) {
			// Mark mesh as dynamic.
			mesh.MarkDynamic ();

			// Create job and set variables.
			BranchWindJob branchWindJob = new BranchWindJob ();
			branchWindJob.windAmplitude = windAmplitude;
			branchWindJob.isST7 = isST7;
			branchWindJob.branchSway = branchSway;

			// Set job vertices.
			branchWindJob.vertices = new NativeArray<Vector3> (mesh.vertices, Allocator.TempJob);
			// Set UVs
			List<Vector4> uvs = new List<Vector4> ();
			List<Vector4> uv2s = new List<Vector4> ();
			//List<Vector4> uv3s = new List<Vector4> ();
			mesh.GetUVs (0, uvs);
			mesh.GetUVs (1, uv2s);
			//mesh.GetUVs (2, uv3s);
			branchWindJob.uvs = new NativeArray<Vector4> (uvs.ToArray (), Allocator.TempJob);
			branchWindJob.uv2s = new NativeArray<Vector4> (uv2s.ToArray (), Allocator.TempJob);
			branchWindJob.uv3s = new NativeArray<Vector4> (uvs.Count, Allocator.TempJob);

			// Execute job.
			int totalVertices = uvs.Count;
			JobHandle branchWindJobHandle = branchWindJob.Schedule (totalVertices, 16);

			// Complete job.
			branchWindJobHandle.Complete ();

			mesh.SetUVs (0, branchWindJob.uvs);
			mesh.SetUVs (1, branchWindJob.uv2s);
			mesh.SetUVs (2, branchWindJob.uv3s);

			// Dispose.
			branchWindJob.vertices.Dispose ();
			branchWindJob.uvs.Dispose ();
			branchWindJob.uv2s.Dispose ();
			branchWindJob.uv3s.Dispose ();
		}
		#endregion

		#region Sprout Mesh
		/// <summary>
		/// Bakes wind data on the sprout mesh UV channels.
		/// It takes values from the base mesh UV channels as parameters.
		/// </summary>
		/// <param name="sproutMeshId">Id of the sprout mesh.</param>
		/// <param name="mesh">Sprout mesh.</param>
		public void SetAdvancedSproutsWindData (
			int sproutMeshId,
			Mesh mesh)
		{
			// Mark mesh as dynamic.
			mesh.MarkDynamic ();

			// Create job and set variables.
			SproutWindJob sproutWindJob = new SproutWindJob ();
			sproutWindJob.branchSway = branchSway;
			sproutWindJob.windAmplitude = windAmplitude;
			sproutWindJob.sproutSway = sproutSway;
			sproutWindJob.sproutTurbulence = sproutTurbulence;
			sproutWindJob.isST7 = isST7;

			// Set job vertices.
			sproutWindJob.vertices = new NativeArray<Vector3> (mesh.vertices, Allocator.TempJob);
			// Set UVs
			List<Vector4> uvs = new List<Vector4> ();
			List<Vector4> uv2s = new List<Vector4> ();
			List<Vector4> uv3s = new List<Vector4> ();
			List<Vector4> uv4s = new List<Vector4> ();
			mesh.GetUVs (0, uvs);
			mesh.GetUVs (1, uv2s);
			mesh.GetUVs (2, uv3s);
			mesh.GetUVs (3, uv4s);
			sproutWindJob.uvs = new NativeArray<Vector4> (uvs.ToArray (), Allocator.TempJob);
			sproutWindJob.uv2s = new NativeArray<Vector4> (uv2s.ToArray (), Allocator.TempJob);
			
			if (uv3s.Count > 0)
				sproutWindJob.uv3s = new NativeArray<Vector4> (uv3s.ToArray (), Allocator.TempJob);
			else
				sproutWindJob.uv3s = new NativeArray<Vector4> (mesh.vertexCount, Allocator.TempJob);

			if (uv4s.Count > 0)
				sproutWindJob.uv4s = new NativeArray<Vector4> (uv4s.ToArray (), Allocator.TempJob);
			else 
				sproutWindJob.uv4s = new NativeArray<Vector4> (mesh.vertexCount, Allocator.TempJob);
			// Execute job.
			JobHandle sproutWindJobHandle = sproutWindJob.Schedule (mesh.vertexCount, 16);

			// Complete job.
			sproutWindJobHandle.Complete ();

			mesh.SetUVs (0, sproutWindJob.uvs);
			mesh.SetUVs (1, sproutWindJob.uv2s);
			mesh.SetUVs (2, sproutWindJob.uv3s);
			mesh.SetUVs (3, sproutWindJob.uv4s);

			// Dispose.
			sproutWindJob.vertices.Dispose ();
			sproutWindJob.uvs.Dispose ();
			sproutWindJob.uv2s.Dispose ();
			sproutWindJob.uv3s.Dispose ();
			sproutWindJob.uv4s.Dispose ();
		}
		#endregion
	}
}
