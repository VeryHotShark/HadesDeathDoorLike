using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Broccoli.Base;
using Broccoli.Model;

namespace Broccoli.Builder {
	/// <summary>
	/// Gives methods to help create mesh segments using BranchSkin instances.
	/// </summary>
	public class ShapeMeshBuilder : IBranchMeshBuilder {

		#region Vars
		public float angleTolerance = 200f;
		public ShapeDescriptorCollection shapeCollection;
		public float girthAtHierarchyBase = 1f;
		public float globalScale = 1f;
		
		public enum RangeContext {
			/// <summary>
			/// The custom shape context domain is a single branch.
			/// </summary>
			PerBranch,
			/// <summary>
			/// The custom shape context domain includes parent and followup branches (a branch skin instance).
			/// </summary>
			BranchSequence
		}
		// The selected range context.
		public RangeContext rangeContext = RangeContext.PerBranch;
		/// <summary>
		/// Modes to calculate nodes at a mesh context, by number or length.
		/// </summary>
		public enum NodesMode {
			/// <summary>
			/// The number of nodes is calculated based on minimun and maximum values.
			/// </summary>
			Number,
			/// <summary>
			/// The number of nodes is calculated based on minimum and maximum values relative to
			/// the length of the mesh context.
			/// </summary>
			Length
		}
		/// <summary>
		/// The selected option to calculate the number of nodes in the mesh context.
		/// </summary>
		public NodesMode nodesMode = NodesMode.Number;
		/// <summary>
		/// Minimum number of nodes within the mesh context.
		/// </summary>
		public int minNodes = 2;
		/// <summary>
		/// Maximum number of nodes within the mesh context.
		/// </summary>
		public int maxNodes = 3;
		/// <summary>
		/// Minimum length per nodes.
		/// </summary>
		public float minNodeLength = 0.2f;
		/// <summary>
		/// Maximum length per nodes.
		/// </summary>
		public float maxNodeLength = 0.5f;
		/// <summary>
		/// How much a node length varies in length size from a neighbour node.
		/// </summary>
		public float nodeLengthVariance = 0f;

		public float shapeTopScale = 1f;
		public float shapeTopCapScale = 1f;
		public float shapeBottomScale = 1f;
		public float shapeBottomCapScale = 1f;
		public enum ShapeCapPositioning {
			LengthRelative,
			GirthRelative
		}
		public ShapeCapPositioning shapeCapPositioning = ShapeCapPositioning.LengthRelative;
		public float shapeTopCapPos = 1f;
		public float shapeBottomCapPos = 0f;
		public float shapeTopCapGirthPos = 1f;
		public float shapeBottomCapGirthPos = 1f;
		public int shapeTopCapFn = 0;
		public int shapeBottomCapFn = 0;
		public float shapeTopParam1 = 0f;
		public float shapeTopCapParam1 = 0f;
		public float shapeBottomParam1 = 0f;
		public float shapeBottomCapParam1 = 0f;
		public float shapeTopParam2 = 0f;
		public float shapeTopCapParam2 = 0f;
		public float shapeBottomParam2 = 0f;
		public float shapeBottomCapParam2 = 0f;
		public int shaperId = 0;
		#endregion

		#region Interface
		public virtual void SetAngleTolerance (float angleTolerance) {
			this.angleTolerance = angleTolerance * 4f;
		}
		public virtual float GetAngleTolerance () {
			return angleTolerance;
		}
		public virtual void SetGlobalScale (float globalScale) { this.globalScale = globalScale; }
		public virtual float GetGlobalScale () { return this.globalScale; }
		/// <summary>
		/// Get the branch mesh builder type.
		/// </summary>
		/// <returns>Branch mesh builder type.</returns>
		public virtual BranchMeshBuilder.BuilderType GetBuilderType () {
			return BranchMeshBuilder.BuilderType.Shape;
		}
		/// <summary>
		/// Called right after a BranchSkin is created.
		/// </summary>
		/// <param name="rangeIndex">Index of the branch skin range to process.</param>
		/// <param name="branchSkin">BranchSkin instance to process.</param>
		/// <param name="firstBranch">The first branch instance on the BranchSkin instance.</param>
		/// <param name="parentBranchSkin">Parent BranchSkin instance to process.</param>
		/// <param name="parentBranch">The parent branch of the first branch of the BranchSkin instance.</param>
		/// <returns>True if any processing gets done.</returns>
		public virtual bool PreprocessBranchSkinRange (
			int rangeIndex, 
			BranchMeshBuilder.BranchSkin branchSkin, 
			BroccoTree.Branch firstBranch, 
			BranchMeshBuilder.BranchSkin parentBranchSkin = null, 
			BroccoTree.Branch parentBranch = null)
		{
			bool result = true;
			if (!firstBranch.hasShaper) {
				// Create a branch shaper to be shared by all the branches in the branch skin.
				BranchShaper branchShaper = BranchShaper.GetInstance (shaperId);
				if (branchShaper == null) return false;
				
				float lengthOffset = 0f;
				float girthAtBase = 0f;
				float girthAtTop = 0f;
				BroccoTree.Branch currentBranch = firstBranch;
				do {
					currentBranch.SetShaper (branchShaper, lengthOffset);
					lengthOffset += currentBranch.length;
					currentBranch = currentBranch.followUp;
				} while (currentBranch != null);

				// Add sections to the branch shaper.
				// SECTIONS ON BRANCHSKIN LENGTH.
				if (rangeContext == RangeContext.BranchSequence) {
					girthAtBase = branchSkin.GetGirthAtPosition (0f, firstBranch);
					girthAtTop = branchSkin.GetGirthAtPosition (1f, firstBranch);
					float baseLength = branchSkin.length;
					AddShaperSections (branchShaper, baseLength, 0f, girthAtBase, girthAtTop);
				}
				// SECTIONS ON BRANCH LENGTH.
				else {
					currentBranch = firstBranch;
					lengthOffset = 0f;
					do {
						girthAtBase = currentBranch.GetGirthAtPosition (0f);
						girthAtTop = currentBranch.GetGirthAtPosition (1f);
						AddShaperSections (branchShaper, currentBranch.length, 
							lengthOffset, girthAtBase, girthAtTop);
						lengthOffset += currentBranch.length;
						currentBranch = currentBranch.followUp;
					} while (currentBranch != null);
				}
				
				List<float> relevantShapePositions = branchShaper.GetRelevantPositions (angleTolerance);
				for (int i = 0; i < relevantShapePositions.Count; i++) {
					branchSkin.AddRelevantPosition (relevantShapePositions [i], 0.01f);
				}
			}
			return result;
		}
		/// <summary>
		/// Called per branchskin after the main mesh has been processed. Modifies an additional mesh to merge it with the one processed.
		/// </summary>
		/// <param name="mesh">Mesh to process.</param>
		/// <param name="rangeIndex">Index of the branch skin range to process.</param>
		/// <param name="branchSkin">BranchSkin instance to process.</param>
		/// <param name="firstBranch">The first branch instance on the BranchSkin instance.</param>
		/// <param name="parentBranchSkin">Parent BranchSkin instance to process.</param>
		/// <param name="parentBranch">The parent branch of the first branch of the BranchSkin instance.</param>
		/// <returns>True if any processing gets done.</returns>
		public virtual Mesh PostprocessBranchSkinRange (Mesh mesh, int rangeIndex, BranchMeshBuilder.BranchSkin branchSkin, BroccoTree.Branch firstBranch, BranchMeshBuilder.BranchSkin parentBranchSkin = null, BroccoTree.Branch parentBranch = null) {
			return null;
		}
		private void AddShaperSections (
			BranchShaper branchShaper,
			float baseLength,
			float accumLength,
			float girthAtBase,
			float girthAtTop)
		{
			int nodeCount = Random.Range (minNodes, maxNodes + 1);
			float lengthStep = baseLength / (float)nodeCount;
			float variance = 0f;
			for (int i = 0; i < nodeCount; i++) {
				// Create section.
				BranchShaper.Section section = new BranchShaper.Section ();
				// Set from and to length.
				if (i == 0) {
					section.fromLength = accumLength + lengthStep * i;
				} else {
					section.fromLength = accumLength + lengthStep * i + variance;
				}
				variance = lengthStep * Random.Range (-0.45f, 0.45f) * nodeLengthVariance;
				if (i == nodeCount - 1) {
					section.toLength = accumLength + lengthStep * i + lengthStep;
				} else {
					section.toLength = accumLength + lengthStep * i + lengthStep + variance;
				}
				if (branchShaper != null) {
					branchShaper.SetSectionScale (section, shapeBottomScale, shapeBottomCapScale, shapeTopScale, shapeTopCapScale);
					//if (shapeCapPositioning == ShapeCapPositioning.LengthRelative) {
					if (branchShaper.isCapGirthPos) {
						branchShaper.SetSectionGirthCapPosition (section, girthAtBase, shapeBottomCapGirthPos, shapeBottomCapFn, 
							girthAtTop, shapeTopCapGirthPos, shapeTopCapFn);
					} else {
						branchShaper.SetSectionLengthCapPosition (section, shapeBottomCapPos, shapeBottomCapFn, 
							shapeTopCapPos, shapeTopCapFn);
					}
					branchShaper.SetSectionParam1 (section, shapeBottomParam1, shapeBottomCapParam1, shapeTopParam1, shapeTopCapParam1);
					branchShaper.SetSectionParam2 (section, shapeBottomParam2, shapeBottomCapParam2, shapeTopParam2, shapeTopCapParam2);

					branchShaper.sections.Add (section);
				}
			}
		}
		#endregion|

		#region Vertices
		public virtual Vector3[] GetPolygonAt (
			BranchMeshBuilder.BranchSkin branchSkin,
			int segmentIndex,
			ref List<float> radialPositions,
			float scale,
			float radiusScale = 1f)
		{
			Vector3[] polygon = new Vector3[0];
			return polygon;
		}
		/// <summary>
		/// Gets the number of segments (like polygon sides) as resolution for a branch position.
		/// </summary>
		/// <param name="branch">Branch containing the position and belonging to the BranchSkin instance.</param>
		/// <param name="branchPosition">Branch position.</param>
		/// <param name="branchSkin">BranchSkin instance.</param>
		/// <param name="branchSkinPosition">Position along the BranchSkin instance.</param>
		/// <param name="branchAvgGirth">Branch average girth.</param>
		/// <returns>The number polygon sides.</returns>
		public virtual int GetNumberOfSegments (
			BroccoTree.Branch branch,
			float branchPosition,
			BranchMeshBuilder.BranchSkin branchSkin, 
			float branchSkinPosition, 
			float branchAvgGirth)
		{
			float girthPosition = (branchAvgGirth - branchSkin.minAvgGirth) / (branchSkin.maxAvgGirth - branchSkin.minAvgGirth);
			branchSkin.polygonSides = Mathf.Clamp (
				Mathf.RoundToInt (
					Mathf.Lerp (
						branchSkin.minPolygonSides + 3,
						branchSkin.maxPolygonSides + 2,
						girthPosition)), 
						branchSkin.minPolygonSides + 3,
						branchSkin.maxPolygonSides + 2);
			return branchSkin.polygonSides;
		}
		#endregion

		#region Bezier Curve
		public virtual Vector3 GetBranchSkinPositionOffset (float positionAtBranch, BroccoTree.Branch branch, float rollAngle, Vector3 forward, BranchMeshBuilder.BranchSkin branchSkin) {
			Vector3 positionOffset = Vector3.zero;
			return positionOffset;
		}
		#endregion
	}
}