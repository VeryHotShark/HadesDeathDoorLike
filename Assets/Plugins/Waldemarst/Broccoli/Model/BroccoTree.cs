using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Broccoli.Base;
using Broccoli.Utils;

/// <summary>
/// Classes for modeling entities composing a tree and the tree itself.
/// </summary>
namespace Broccoli.Model {
	/// <summary>
	/// Class representing a biological tree.
	/// </summary>
	[System.Serializable]
	public class BroccoTree {
		#region Class Sprout
		/// <summary>
		/// Class representing a sprout.
		/// </summary>
		[System.Serializable]
		public class Sprout {
			#region Vars
			/// <summary>
			/// Belonging group.
			/// </summary>
			public int groupId = 0;
			/// <summary>
			/// The index of the snapshot or area map.
			/// </summary>
			[System.NonSerialized]
			public int subgroupId = -1;
			/// <summary>
			/// Relative position on the parent branch. From 0 to 1.
			/// </summary>
			public float position = 1f;
			/// <summary>
			/// Position on the tree hierarchy. From 0 (base) to 1 (top branch tip).
			/// </summary>
			public float hierarchyPosition = 1f;
			/// <summary>
			/// Flag to mark the preference of using hierarchy position instead or in branch positioning.
			/// </summary>
			public bool preferHierarchyPosition = false;
			/// <summary>
			/// Gets the prefered position for the sprout. Either in-branch or in full hierarchy, depending
			/// on the preferHierarchyPosition flag value.
			/// </summary>
			/// <value>The prefered position.</value>
			public float preferedPosition {
				get { 
					if (preferHierarchyPosition)
						return hierarchyPosition;
					else
						return position;
				}
			}
			/// <summary>
			/// Angle around the branch circumference.
			/// </summary>
			public float rollAngle = 0f;
			/// <summary>
			/// Angle between the parent branch and the sprout.
			/// </summary>
			public float branchAlignAngle = 0f;
			/// <summary>
			/// Horizontal alignment relative to gravity.
			/// </summary>
			public float horizontalAlign = 0f;
			/// <summary>
			/// Alignment to look up againts the gravity vector.
			/// </summary>
			public float gravityAlign = 0f;
			/// <summary>
			/// Perpendicular alignment relative to branch direction.
			/// </summary>
			public float perpendicularAlign = 0f;
			/// <summary>
			/// Flip aligment towards a directional vector.
			/// </summary>
			public float flipAlign = 0f;
			/// <summary>
			/// Flip alignment direction.
			/// </summary>
			public Vector3 flipDirection = Vector3.up;
			/// <summary>
			/// Value to randomize the normal vector of a sprout.
			/// </summary>
			public float normalRandomness = 0f;
			/// <summary>
			/// If true the sprout comes from the center of the branch,
			/// instead of the surface of it.
			/// </summary>
			public bool fromBranchCenter = false;
			/// <summary>
			/// Parent branch.
			/// </summary>
			[System.NonSerialized]
			public Branch parentBranch;
			/// <summary>
			/// Position vector on the line of the branch..
			/// </summary>
			[System.NonSerialized]
			public Vector3 inBranchPosition = Vector3.zero;
			/// <summary>
			/// Position vector on the surface of the branch mesh.
			/// </summary>
			[System.NonSerialized]
			public Vector3 inGirthPosition = Vector3.zero;
			/// <summary>
			/// The sprout direction.
			/// </summary>
			[System.NonSerialized]
			public Vector3 sproutDirection = Vector3.zero;
			/// <summary>
			/// The sprout normal.
			/// </summary>
			[System.NonSerialized]
			public Vector3 sproutNormal = Vector3.zero;
			/// <summary>
			/// The sprout forward.
			/// </summary>
			public Vector3 forward = Vector3.zero;
			/// <summary>
			/// Offset of the branch from its center.
			/// </summary>
			public Vector3 positionOffset = Vector3.zero;
			/// <summary>
			/// Saves the id of te structure level that generated this sprout.
			/// </summary>
			[System.NonSerialized]
			public int helperStructureLevelId = -1;
			/// <summary>
			/// Saves the id of the seed that generated this sprout.
			/// </summary>
			[System.NonSerialized]
			public int helperSeedId = -1;
			/// <summary>
			/// Temporal id for the sprout, assigned when requested by process
			/// that need to match each sprout with a particular data structure.
			/// Note this id is non persistent or may not 
			/// be set depending on the context.
			/// </summary>
			[System.NonSerialized]
			public int helperSproutId = -1;
			/// <summary>
			/// The mesh length for this sprout, if it has no mesh then 0.
			/// </summary>
			[System.NonSerialized]
			public float meshHeight = 0f;
			#endregion

			#region Ops
			/// <summary>
			/// Calculates positions relative to the parent branch.
			/// </summary>
			public void CalculateVectors (Branch referenceBranch = null, bool isBranch = false) {
				if (referenceBranch == null)
					referenceBranch = parentBranch;
				if (referenceBranch != null) {
					// Set inBranchPosition.
					inBranchPosition = referenceBranch.GetPointAtPosition (position);

					// Set inGirthPosition.
					if (fromBranchCenter) {
						inGirthPosition = inBranchPosition;
					} else {
						//inGirthPosition = inBranchPosition + positionOffset * 0.8f;
						inGirthPosition =referenceBranch.GetSurfacePointAt (position, rollAngle);
					}

					Vector3 referenceBranchNormal;
					Vector3 referenceBranchDirection;

					//if (isBranch) {
						referenceBranchNormal = referenceBranch.GetNormalAtPosition (position);
						referenceBranchDirection = referenceBranch.GetDirectionAtPosition (position);
					//}

					if (referenceBranch != null && referenceBranch.parentTree != null) {
						hierarchyPosition = (referenceBranch.GetHierarchyLevel () + position) / referenceBranch.parentTree.GetOffspringLevel ();
					}

					// Direction
					Vector3 result;
					if (isBranch) {
						result = Quaternion.AngleAxis(branchAlignAngle * Mathf.Rad2Deg, Vector3.left) * Vector3.forward;
					} else {
						result = Quaternion.AngleAxis(branchAlignAngle * Mathf.Rad2Deg, Vector3.forward) * Vector3.right;	
					}
					//Vector3 result = Quaternion.AngleAxis(branchAlignAngle * Mathf.Rad2Deg, Vector3.forward) * Vector3.right;
					//Vector3 result = Quaternion.AngleAxis(branchAlignAngle * Mathf.Rad2Deg, Vector3.left) * Vector3.forward;
					result = Quaternion.AngleAxis(rollAngle * Mathf.Rad2Deg, GlobalSettings.againstGravityDirection) * result;
					Quaternion rotation;
					if (referenceBranchNormal != Vector3.zero) {
						rotation = Quaternion.LookRotation (referenceBranchNormal, referenceBranchDirection);
					} else {
						rotation = Quaternion.Euler (Vector3.zero);
					}
					sproutDirection = (rotation * result).normalized;

					// Normal
					Vector3 resultN = Quaternion.AngleAxis(branchAlignAngle * Mathf.Rad2Deg, Vector3.forward) * GlobalSettings.againstGravityDirection;
					resultN = Quaternion.AngleAxis (rollAngle * Mathf.Rad2Deg, GlobalSettings.againstGravityDirection) * resultN;
					if (referenceBranchNormal != Vector3.zero) {
						Quaternion rotationN;
						if (referenceBranchNormal != Vector3.zero) {
							rotationN = Quaternion.LookRotation (referenceBranchNormal, referenceBranchDirection);
						} else {
							rotationN = Quaternion.Euler (Vector3.zero);
						}
						sproutNormal = rotationN * resultN;
					}
					sproutNormal = sproutNormal.normalized;

					// Forward
					//sproutForward = Quaternion.AngleAxis (rollAngle, referenceBranchDirection) * referenceBranchNormal;
					forward = Quaternion.AngleAxis (rollAngle * Mathf.Rad2Deg, referenceBranchDirection) * referenceBranchNormal;
					// TODO RE: simplify vector creation.

					// Horizontal align
					if (horizontalAlign > 0) {
						Vector3 horizontalDirection = Vector3.ProjectOnPlane (sproutDirection, GlobalSettings.againstGravityDirection);
						if (horizontalDirection.magnitude == 0) {
							// TODO
						}
						Vector3 newSsproutDirection = Vector3.Lerp (sproutDirection, horizontalDirection, horizontalAlign);
						sproutNormal = Vector3.Lerp (sproutNormal, GlobalSettings.againstGravityDirection, horizontalAlign);
						sproutDirection = newSsproutDirection;
					}

					// Flip sprout align
					if (flipAlign > 0 && !isBranch) {
						Vector3 _flipDirection = Vector3.ProjectOnPlane (sproutDirection, flipDirection);
						if (_flipDirection.magnitude == 0) {
							// TODO
						}
						Vector3 newSsproutDirection = Vector3.Lerp (sproutDirection, _flipDirection, flipAlign);					
						sproutNormal = Vector3.Lerp (sproutNormal, flipDirection, flipAlign);
						if (normalRandomness > 0f) {
							Vector3 rand = Random.onUnitSphere;
							rand.z = Mathf.Abs (rand.z);
							rand = Quaternion.LookRotation (sproutNormal) * rand;
							sproutNormal = Vector3.Lerp (sproutNormal, rand, normalRandomness);
						}
						sproutDirection = newSsproutDirection;
					}

					// Gravity Align
					if (gravityAlign != 0) {
						Vector3 newSproutDirection = Vector3.Lerp (
							sproutDirection, 
							(gravityAlign > 0?GlobalSettings.againstGravityDirection:GlobalSettings.gravityDirection), 
							gravityAlign>0?gravityAlign:-gravityAlign);
						Quaternion gravityRotation = Quaternion.FromToRotation (sproutDirection, newSproutDirection);
						sproutNormal = gravityRotation * sproutNormal;
						sproutDirection = newSproutDirection;
					}
				}
			}
			#endregion

			#region Clone
			/// <summary>
			/// Clone this instance.
			/// </summary>
			public Sprout Clone () {
				Sprout clone = new Sprout ();
				clone.groupId = groupId;
				clone.position = position;
				clone.rollAngle = rollAngle;
				clone.branchAlignAngle = branchAlignAngle;
				clone.horizontalAlign = horizontalAlign;
				clone.gravityAlign = gravityAlign;
				clone.perpendicularAlign = perpendicularAlign;
				clone.flipAlign = flipAlign;
				clone.flipDirection = flipDirection;
				clone.normalRandomness = normalRandomness;
				clone.fromBranchCenter = fromBranchCenter;
				return clone;
			}
			#endregion
		}
		#endregion // END SPROUT CLASS.

		#region Class Branch
		/// <summary>
		/// Class representing a branch.
		/// </summary>
		[System.Serializable]
		public class Branch {
			#region Vars
			/// <summary>
			/// Id for the branch.
			/// </summary>
			public int id = 0;
			/// <summary>
			/// Guid for the branch.
			/// </summary>
			public System.Guid guid {
				get { return curve.guid; }
			}
			/// <summary>
			/// Structural bezier curve for this branch.
			/// </summary>
			/// <returns></returns>
			public BezierCurve curve = new BezierCurve ();
			/// <summary>
			/// Angle in radians to rotate around the parent branch.
			/// </summary>
			public float rollAngle = 0f;
			/// <summary>
			/// True for branches instances representing tree roots.
			/// </summary>
			public bool isRoot = false;
			/// <summary>
			/// Flag to mark that this branch has been manually modified.
			/// </summary>
			public bool isTuned = false;
			/// <summary>
			/// True to branches belonging to the tree trunk.
			/// </summary>
			public bool isTrunk = false;
			/// <summary>
			/// True if this branch has a break point.false The branch does not generate offspring nor it is meshed after this point.
			/// </summary>
			public bool isBroken = false;
			/// <summary>
			/// The position for the break point if this branch is broken.
			/// </summary>
			public float breakPosition = 0.5f;
			/// <summary>
			/// True when new sprouts have been added and recalculation
			/// in needed.
			/// </summary>
			bool _sproutsDirty = false;
			/// <summary>
			/// Levels of offspring after this branch.
			/// </summary>
			int _offspringLevels= 0;
			/// <summary>
			/// Id to the parent branch, if set, used to flat serialization.
			/// </summary>
			public int parentBranchId = -1;
			/// <summary>
			/// Phase value (random 0-1) from the trunk.
			/// </summary>
			public float phase = -1f;
			/// <summary>
			/// Max length for the phase the branch belongs to.
			/// </summary>
			public float phaseLength = 0f;
			/// <summary>
			/// Phase2 value (random 0-1) from the trunk.
			/// </summary>
			public float phase2 = -1f;
			/// <summary>
			/// Length from the tree origin up to the start of the phase.
			/// </summary>
			public float phaseLengthOffset = 0f;
			/// <summary>
			/// Helper id.
			/// </summary>
			public int helperStructureLevelId = -1;
			/// <summary>
			/// Length offset to be passed to the branch shaper.
			/// </summary>
			public float shaperOffset = 0f;
			/// <summary>
			/// Class to define the shape of a branch.
			/// </summary>
			private BranchShaper _shaper = null;
			#endregion

			#region Length Vars
			/// <summary>
			/// Length of the branch at current age.
			/// </summary>
			/// <value>Length of the branch.</value>
			public float length {
				get { return this.curve.length; }
			}
			/// <summary>
			/// Accumulated length for the origin of the tree to this branch origin.
			/// </summary>
			/// <value>Length from the origin of the tree to the origin of this branch.</value>
			public float accumLength {
				get {
					return _accumLength;
				}
			}
			/// <summary>
			/// Accumulated length from the origin of the tree to the origin of this branch.
			/// </summary>
			float _accumLength = 0f;
			#endregion

			#region Position Vars
			/// <summary>
			/// Position within its parent branch, 0 is at root, 1 at the end.
			/// </summary>
			/// <value>The position of this branch relative to its parent.</value>
			public float position {
				get { return this._position; }
				set {
					this._position = value;
					if (_parent != null && _parent.followUp == this) {
						if (_position != 1f) {
							_parent.followUp = null;
							isTrunk = false;
							for (int i = 0; i < parentTree.branches.Count; i++) {
								if (_parent.branches[i].position == 1) {
									_parent.followUp = _parent.branches[i];
									branches[i].isTrunk = _parent.isTrunk;
									break;
								}
							}
						}
					}
					UpdateAccumLength ();
					this._positionDirty = true;
				}
			}
			/// <summary>
			/// World position of the starting point of the branch.
			/// </summary>
			/// <value>The origin.</value>
			public Vector3 origin {
				get {
					return GetPointAtPosition (0);
				}
			}
			/// <summary>
			/// Wold position of the ending point of the branch.
			/// </summary>
			/// <value>The destination.</value>
			public Vector3 destination {
				get {
					return GetPointAtPosition (1.0f);
				}
			}
			/// <summary>
			/// Median direction of the branch.
			/// </summary>
			public Vector3 direction {
				get { return (curve.Last ().position - curve.First ().position).normalized; }
			}
			/// <summary>
			/// Position of the branch relative to its parent length.
			/// When at base position is 0, at tip is 1.
			/// </summary>
			[SerializeField]
			float _position = 0;
			/// <summary>
			/// Adds offset to the branch origin position.
			/// </summary>
			public Vector3 parentOffset = Vector3.zero;
			/// <summary>
			/// Branch origin position in space taking the tree root as reference origin.
			/// </summary>
			public Vector3 originOffset = Vector3.zero;
			/// <summary>
			/// True when the branch requires position update.
			/// </summary>
			bool _positionDirty = false;
			#endregion

			#region Girth Vars
			/// <summary>
			/// Girth expected when branch reaches age 1.
			/// </summary>
			/// <value>The max girth the branch reaches.</value>
			public float maxGirth {
				get { return this._maxGirth; }
				set {
					this._maxGirth = value;
					this._girthDirty = true;
				}
			}
			/// <summary>
			/// Girth expected when branch age is 0.
			/// </summary>
			/// <value>Minimal girth for the branch.</value>
			public float minGirth {
				get { return this._minGirth; }
				set {
					this._minGirth = value;
					this._girthDirty = true;
				}
			}
			/// <summary>
			/// Maximum girth expected at this branch.
			/// </summary>
			[SerializeField]
			float _maxGirth = 0.25f;
			/// <summary>
			/// Minimum girth expected at this branch.
			/// </summary>
			[SerializeField]
			float _minGirth = 0.05f;
			/// <summary>
			/// Multiplies the girth to this factor at the branch base.
			/// </summary>
			float _girthAtBaseFactor = 0;
			/// Multiplies the girth to this factor at the branch tip.
			float _girthAtTopFactor = 0;
			/// <summary>
			/// Curve for values between min and max girth.
			/// </summary>
			//public AnimationCurve girthCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
			public AnimationCurve girthCurve = null;
			/// <summary>
			/// Scale factor to multiply the final girth.
			/// </summary>
			public float girthScale = 1f;
			/// <summary>
			/// True when the branch requires girth update.
			/// </summary>
			bool _girthDirty = false;
			#endregion

			#region Hierarchy Vars
			/// <summary>
			/// Children branches.
			/// </summary>
			/// <value>The branches.</value>
			public List<Branch> branches {
				get { return this._branches; }
			}
			/// <summary>
			/// Children leaves.
			/// </summary>
			/// <value>The leaves.</value>
			public List<Sprout> sprouts {
				get { return this._sprouts; }
			}
			/// <summary>
			/// Parent branch.
			/// </summary>
			/// <value>The parent of this branch.</value>
			public Branch parent {
				get { return _parent; }
			}
			/// <summary>
			/// Number of branch levels after this branch.
			/// </summary>
			/// <value>The offspring levels.</value>
			public int offspringLevels {
				get { return _offspringLevels; }
			}
			/// <summary>
			/// The branch has a position of 1 at its parent and is the continuation of it.
			/// </summary>
			public bool isFollowUp {
				get {
					if (parent != null && parent.followUp == this)
						return true;
					return false;
				}
			}
			/// <summary>
			/// Branch level on the tree offspring.
			/// </summary>
			int _level = 0;
			/// <summary>
			/// Branch hierarchy on the tree offspring.
			/// </summary>
			float _hierarchy = -1;
			/// <summary>
			/// Reference to a branch continuing this one.
			/// </summary>
			[System.NonSerialized]
			public Branch followUp = null;
			/// <summary>
			/// Children branches.
			/// </summary>
			[System.NonSerialized]
			List<Branch> _branches = new List<Branch> ();
			/// <summary>
			/// Children leaves.
			/// </summary>
			List<Sprout> _sprouts = new List<Sprout> ();
			/// <summary>
			/// Parent tree object.
			/// </summary>
			[System.NonSerialized]
			public BroccoTree parentTree = null;
			/// <summary>
			/// Parent branch.
			/// </summary>
			[System.NonSerialized]
			Branch _parent = null;
			#endregion

			#region Shape Vars
			/// <summary>
			/// Checks if there is a branch shaper instance set on this branch.
			/// </summary>
			/// <returns><c>True</c> if a branch shaper has been set.</returns>
			public bool hasShaper {
				get { return _shaper != null; }
			}
			/// <summary>
			/// Gets the shaper assigned to this branch.
			/// </summary>
			/// <value>Branch shaper assigned, otherwise null.</value>
			public BranchShaper shaper {
				get { return _shaper; }
			}
			/// <summary>
			/// Sets the shaper class to define the branch mesh shape.
			/// </summary>
			/// <param name="shaper">Branch shaper class.</param>
			/// <param name="shaperOffset">Length offset for the shaper.</param>
			public void SetShaper (BranchShaper shaper, float shaperOffset) {
				_shaper = shaper;
				this.shaperOffset = shaperOffset;
				if (shaper != null) {
					shaper.Init ();
				}
			}
			/// <summary>
			/// Removes any branch shaper set on this branch instance.
			/// </summary>
			public void RemoveShaper () {
				this._shaper = null;
				this.shaperOffset = 0f;
			}
			#endregion

			#region Constructor
			/// <summary>
			/// Class constructor.
			/// </summary>
			public Branch () {
				curve.resolution = 2;
				BezierNode nodeA = new BezierNode (Vector3.zero);
				BezierNode nodeB = new BezierNode (Vector3.one * 2);
				nodeA.handleStyle = BezierNode.HandleStyle.Auto;
				nodeB.handleStyle = BezierNode.HandleStyle.Auto;
				curve.AddNode (nodeA, false);
				curve.AddNode (nodeB, false);
				curve.onLengthChanged -= InternalOnLengthChanged;
				curve.onLengthChanged += InternalOnLengthChanged;
				curve.Process ();
			}
			#endregion
			
			#region Events
			/// <summary>
			/// Raises the destroy event.
			/// </summary>
			public void OnDestroy () { }
			#endregion

			#region Length Methods
			/// <summary>
			/// Gets the maximum length found on the offspring (upstream) from the position given at this branch.
			/// </summary>
			/// <returns>Max length from this branch to the tip of the hierarchy.</returns>
			public float GetOffspringMaxLength () {
				float maxLength = length;
				for (int i = 0; i < _branches.Count; i++) {
					maxLength = GetOffspringMaxLengthRecursive (maxLength, 0f, _branches[i]);
				}
				return maxLength;
			}
			/// <summary>
			/// Checks recursively for the maximum length in the upstream hierarchy.
			/// </summary>
			/// <param name="maxLength">Max length so far found.</param>
			/// <param name="accumLength">Accumulated length up to the base of the branch to inspect.</param>
			/// <param name="childBranch">Branch to inspect.</param>
			/// <returns>Max length found so far.</returns>
			float GetOffspringMaxLengthRecursive (float maxLength, float accumLength, Branch childBranch) {
				float baseLength = accumLength + childBranch.parent.GetLengthAtPos (childBranch.position);
				if (baseLength + childBranch.length > maxLength) maxLength = baseLength + childBranch.length;
				for (int i = 0; i < childBranch.branches.Count; i++) {
					maxLength = GetOffspringMaxLengthRecursive (maxLength, baseLength, childBranch.branches [i]);
				}
				return maxLength;
			}
			/// <summary>
			/// Updates the length from the origin of the tree to the origin of this branch.
			/// </summary>
			public void UpdateAccumLength () {
				if (_parent != null) {
					_accumLength = _parent.length * position + _parent.accumLength;
				} else {
					_accumLength = 0f;
				}
				for (int i = 0; i < _branches.Count; i++) {
					_branches [i].UpdateAccumLength ();
				}
			}
			/// <summary>
			/// Sets the position of the last node of the branch to a position
			/// according to a directional vector and a length given.
			/// </summary>
			/// <param name="direction"></param>
			/// <param name="directionalLength"></param>
			public void ApplyDirectionalLength (Vector3 direction, float directionalLength) {
				this.curve.Last().position = curve.First().position + (direction.normalized * directionalLength);
				this.curve.Process ();
			}
			/// <summary>
			/// Recalculates the roll angle relative to a parent branch.
			/// </summary>
			/// <param name="direction">Direction of the branch.</param>
			/// <param name="referenceParent">Parent branch.</param>
			public void RecalculateRollAngle (Vector3 direction, Branch referenceParent) {
				// Recalculate roll angle.
				if (referenceParent != null) {
					CurvePoint parentPoint = referenceParent.curve.GetPointAt (position);
					Vector3 directionOnPlane = Vector3.ProjectOnPlane (direction, parentPoint.forward);
					rollAngle = Vector3.SignedAngle (parentPoint.normal, directionOnPlane, parentPoint.forward) * Mathf.Deg2Rad;
				}
			}
			/// <summary>
			/// Called when the length of the curve changes, use to update children branches values.
			/// </summary>
			/// <param name="oldLength">Old length.</param>
			/// <param name="newLength">New length.</param>
			void InternalOnLengthChanged (float oldLength, float newLength) {
				for (int i = 0; i < _branches.Count; i++) {
					_branches [i].UpdateAccumLength ();
					_branches [i].UpdatePosition ();
				}
				if (parentTree != null) parentTree.GetMaxLength (true);
			}
			#endregion

			#region Position Methods
			/// <summary>
			/// Set the transform position for all children branches.
			/// </summary>
			public void UpdatePosition () {
				if (_parent != null) {
					// If followup
					if (isFollowUp) {
						originOffset = _parent.originOffset + _parent.curve.Last ().position; // Use node to skip noise offset.
						parentOffset = Vector3.zero;
					}
					// If not follow up.
					else {
						// this branch is not a follow up.
						if (hasShaper) {
							parentOffset = _parent.GetSurfacePointAt (position, rollAngle) - _parent.GetPointAtPosition (position);
							parentOffset *= 0.8f;
						} else {
							Vector3 noiseAtBase = curve.GetPositionAt (0f);
							float branchToParentAngle = Vector3.Angle (_parent.GetDirectionAtLength (_position), direction);
							if (branchToParentAngle > 90f) {
								branchToParentAngle = 180f - branchToParentAngle;
							}
							float parentOffsetLength = _parent.GetSurfaceDistanceAt (_position, rollAngle) - Mathf.Cos (branchToParentAngle * Mathf.Deg2Rad) * GetSurfaceDistanceAt (0f, 0f);
							parentOffset = _parent.GetNormalAtPosition (_position, rollAngle).normalized * parentOffsetLength * 0.7f - noiseAtBase;
						}
						originOffset = _parent.originOffset + _parent.curve.GetPointAt (_position).position + parentOffset;
					}
				}
				// If trunk.
				else {
					parentOffset = Vector3.zero;
				}
				for (int i = 0; i < _branches.Count; i++) {
					_branches[i].UpdatePosition ();
				}
				_positionDirty = false;
			}
			#endregion

			#region Girth Methods
			/// <summary>
			/// Update the girth factors used to calculate values across the branch.
			/// </summary>
			public void UpdateGirth (bool recursive = false) {
				if (_parent == null) {
					int treeLevels = _level + _offspringLevels + 1;
					_girthAtBaseFactor = 0;
					_girthAtTopFactor = (1 / (float)treeLevels) * (_level + 1);
				} else {
					_girthAtBaseFactor = _parent.GetGirthFactorAt (_position);
					_girthAtTopFactor = ((1 - _girthAtBaseFactor) / (float)(_offspringLevels + 1)) + _girthAtBaseFactor;
					if (isFollowUp) girthScale = parent.girthScale;
				}
				if (recursive) {
					for (int i = 0; i < _branches.Count; i++) {
						_branches[i].UpdateGirth (recursive);
					}
				}
				_girthDirty = false;
			}
			/// <summary>
			/// Get the girth curve used to interpolate between min and max girth.
			/// </summary>
			/// <returns>The girth curve.</returns>
			public AnimationCurve GetGirthCurve () {
				if (girthCurve == null) {
					if (_parent != null) {
						return _parent.GetGirthCurve ();
					} else {
						return AnimationCurve.Linear(0f, 0f, 1f, 1f);
					}
				} else {
					return girthCurve;
				}
			}
			#endregion

			#region Hierarchy Methods
			/// <summary>
			/// Gets the level of the branch at the tree. Zero means the branch has
			/// no parent, 1 means first branch and so on.
			/// </summary>
			/// <returns>The level.</returns>
			/// <param name="recalculate">If set to <c>true</c> recalculate.</param>
			public int GetLevel (bool recalculate = false) {
				if (recalculate || _level < 0) {
					if (_parent == null)
						_level = 0;
					else
						_level = _parent.GetLevel () + 1;
				}
				return _level;
			}
			/// <summary>
			/// Gets the hierarchy level (level + position).
			/// </summary>
			/// <returns>The hierarchy level.</returns>
			/// <param name="recalculate">If set to <c>true</c> recalculate.</param>
			public float GetHierarchyLevel (bool recalculate = false) {
				if (_hierarchy == -1) {
					recalculate = true;
				}
				if (recalculate) {
					if (_parent == null) {
						_hierarchy = 0f;
					} else {
						_hierarchy = _position + _parent.GetHierarchyLevel (recalculate);
					}
				}
				return _hierarchy;
			}
			/// <summary>
			/// Updates the follow up branch.
			/// </summary>
			/// <param name="recursive">If set to <c>true</c> recursive.</param>
			public void UpdateFollowUps (bool recursive = false) {
				followUp = null;
				for (int i = 0; i < _branches.Count; i++) {
					if (_branches[i].position == 1 && 
						(followUp == null || _branches[i].offspringLevels > followUp.offspringLevels)) {
						followUp = _branches[i];
						followUp.rollAngle = rollAngle;
						followUp.isTrunk = isTrunk;
					}
				}
				if (recursive) {
					for (int i = 0; i < _branches.Count; i++) {
						_branches[i].UpdateFollowUps (true);
					}
				}
			}
			/// <summary>
			/// Attaches a branch to the current branch.
			/// </summary>
			/// <param name="branch">Branch to attach.</param>
			public void AddBranch (BroccoTree.Branch branch, bool fullProcess = true) {
				branch.RemoveShaper ();
				parentTree?.onBeforeAddBranch?.Invoke (branch, this, parentTree);
				branch.parentTree = parentTree;
				_branches.Add (branch);
				branch._parent = this;
				branch.UpdatePosition ();
				// Set followUp
				if (this.followUp == null && branch.position == 1) {
					this.followUp = branch;
					branch.isTrunk = isTrunk;
				}
				branch.GetLevel(true);
				_InternalOffspringReceivedBranch (branch, branch.offspringLevels + 1);
				UpdateAccumLength ();
				// Update the max length of the tree and phase lengths.
				if (parentTree != null && fullProcess) parentTree.GetMaxLength (true);
				parentTree?.onAddBranch?.Invoke (branch, this, parentTree);
			}
			public void ClearSprouts () {
				_sprouts.Clear ();
			}
			/// <summary>
			/// Internal function to inform when a new branch is received.
			/// Updates offspringLevels.
			/// </summary>
			/// <param name="branch">Branch.</param>
			/// <param name="offspringLevel">Offspring level.</param>
			public void _InternalOffspringReceivedBranch (Branch branch, int offspringLevels) {
				//GetMaxChildrenLevel (true); //TODO: update offspring level
				if (offspringLevels > this._offspringLevels) {
					this._offspringLevels = offspringLevels;
				}
				if (this._parent != null) {
					this._parent._InternalOffspringReceivedBranch (branch, this._offspringLevels + 1);
				}
			}
			/// <summary>
			/// Return all the branches attached to this particular branch at any level.
			/// </summary>
			/// <returns>The descendant branches.</returns>
			public List<Branch> GetDescendantBranches () {
				List<Branch> children = new List<Branch> (this._branches);
				for (int i = 0; i < _branches.Count; i++) {
					children.AddRange (_branches[i].GetDescendantBranches ());
				}
				return children;
			}
			/// <summary>
			/// Gets a plane at the base of the branch, perpendicular to the parent branch direction.
			/// </summary>
			/// <returns>Plane perpendicular to the parent branch.</returns>
			public Plane GetParentPlane () {
				Plane plane = new Plane ();
				if (_parent != null) {
					Vector3 inNormal = Quaternion.AngleAxis (rollAngle * Mathf.Rad2Deg, _parent.GetDirectionAtPosition (position)) * 
						_parent.GetNormalAtPosition (position);
					plane.SetNormalAndPosition (inNormal, GetPointAtPosition (0f));
				}
				return plane;
			}
			/// <summary>
			/// Attaches a sprout to the current branch.
			/// </summary>
			/// <param name="sprout">Sprout.</param>
			/// <param name="calculateVectors">If set to <c>true</c> calculate vectors for each sprout.</param>
			public void AddSprout (BroccoTree.Sprout sprout, bool calculateVectors = false) {
				_sprouts.Add (sprout);
				sprout.parentBranch = this;
				if (calculateVectors) {
					sprout.CalculateVectors ();
				}
				_sproutsDirty = true;
			}
			/// <summary>
			/// Attaches a list of sprouts to the current branch.
			/// </summary>
			/// <param name="sprouts">Sprouts.</param>
			/// <param name="calculateVectors">If set to <c>true</c> calculate vectors for each sprout.</param>
			public void AddSprouts (List<BroccoTree.Sprout> sprouts, bool calculateVectors = false) {
				for (int i = 0; i < sprouts.Count; i++) {
					AddSprout (sprouts[i], calculateVectors);
				}
			}
			/// <summary>
			/// Calculates the sprouts position and orientation.
			/// </summary>
			/// <param name="recursive">If set to <c>true</c> the calculation is called on all children branches.</param>
			public void UpdateSprouts (bool recursive = true) {
				if (_sproutsDirty) {
					for (int i = 0; i < _sprouts.Count; i++) {
						_sprouts[i].CalculateVectors ();
					}
					_sproutsDirty = false;
				}
				if (recursive) {
					for (int i = 0; i < _branches.Count; i++) {
						_branches[i].UpdateSprouts (recursive);
					}
				}
			}
			#endregion

			#region Structure Methods
			/// <summary>
			/// Structural update this instance.
			/// </summary>
			/// <param name="force">If set to <c>true</c> force the update.</param>
			public void Update (bool force = false) {
				if (_positionDirty || force) {
					UpdatePosition ();
				}
				if (_girthDirty || force) {
					UpdateGirth ();
				}
				for (int i = 0; i < _branches.Count; i++) {
					_branches[i].Update (force);
				}
			}
			public void UpdateResolution (int resolutionSteps, bool recursive = false) {
				curve.resolution = resolutionSteps;
				curve.ComputeSamples ();
				if (recursive) {
					for (int i = 0; i < branches.Count; i++) {
						branches [i].UpdateResolution (resolutionSteps, true);
					}
				}
			}
			/// <summary>
			/// Recalculates the normals.
			/// </summary>
			public void RecalculateNormals () {
				RecalculateNormals (Vector3.zero, Vector3.zero);
			}
			/// <summary>
			/// Recalculates the normals.
			/// </summary>
			/// <param name="prevDirection">Previous direction.</param>
			/// <param name="prevNormal">Previous normal.</param>
			public void RecalculateNormals (Vector3 prevDirection, Vector3 prevNormal) {
				if (isTrunk) {
					curve.normalMode = BezierCurve.NormalMode.ReferenceVector;
					curve.referenceNormal = Vector3.forward;
				} else {
					curve.normalMode = BezierCurve.NormalMode.ReferenceVector;
					if (isFollowUp) {
						CurvePoint parentLastPoint = _parent.curve.GetPointAt (1f, true);
						curve.referenceNormal = parentLastPoint.normal;
						curve.referenceForward = parentLastPoint.forward;
					} else {
						Vector3 forwardAtBase = curve.GetPointAt (0f, true).forward;
						curve.referenceNormal = Vector3.ProjectOnPlane (forwardAtBase, curve.referenceForward).normalized;
					}
					if (curve.referenceNormal == Vector3.zero) {
						curve.referenceNormal = Vector3.forward;
					}
				}
				curve.RecalculateNormals ();
				if (_parent != null) {
					Vector3 dir = curve.Last ().position - curve.First ().position;
					RecalculateRollAngle (dir, _parent);
				}
				
				// ProcessBendPoints.
				for (int i = 0; i < _branches.Count; i++) {
					if (_branches[i] == followUp) {
						_branches[i].RecalculateNormals (prevDirection, prevNormal);
					} else {
						_branches[i].RecalculateNormals (Vector3.zero, Vector3.zero);
					}
				}
			}
			#endregion

			#region Querying
			/// <summary>
			/// Gets the length at a position of the branch.
			/// </summary>
			/// <param name="position">Relative position at the current branch.</param>
			/// <param name="accumulated"><c>True</c> if the length comes from the origin of the tree.</param>
			/// <returns>Length at the branch position.</returns>
			public float GetLengthAtPos (float position, bool accumulated = false) {
				float localLength = curve.length * position;
				if (accumulated) {
					return localLength + _accumLength;
				}
				return localLength;
			}
			/// <summary>
			/// Gets the length from the tree origin to the length of the current branch.
			/// </summary>
			/// <param name="localLength">Length at the current branch.</param>
			/// <returns>Accumulated length from the tree origin up to the length of the current branch.</returns>
			public float GetAccumLengthAtLength (float localLength) {
				return localLength + _accumLength;
			}
			/// <summary>
			/// Gets the phase length for a position on this branch.
			/// </summary>
			/// <param name="position">Position at this branch (0-1)</param>
			/// <returns>Phase length at position,if the branch is a trunk then 0 is returned.</returns>
			public float GetPhaseLength (float position) {
				// If the branch has no parent, then 
				if (_parent == null || phase < 0) return 0f;
				float localAccumLength = GetLengthAtPos (position, true);
				return localAccumLength - phaseLengthOffset;
			}
			/// <summary>
			/// Gets the phase length for a position on this branch, normalized from 0 to 1 according to the max
			/// length of the phase this branch belongs to.
			/// </summary>
			/// <param name="branchPosition">Position on this branch (0-1).</param>
			/// <returns>Phase length at branch position, normalized from the max phase length to a 0-1 range.</returns>
			public float GetPhasePosition (float branchPosition) {
				float localPhaseLength = GetPhaseLength (branchPosition);
				if (localPhaseLength > 0f) return localPhaseLength / phaseLength;
				else return 0f;
			}
			/// <summary>
			/// Gets a vector in that space between origin and destination, in local tree space or absolute space.
			/// </summary>
			/// <returns>Vector at requested position.</returns>
			/// <param name="position">Position between 0 and 1.</param>
			/// <param name="worldPosition">If set to <c>true</c> the position takes in account the tree position in the world.</param>
			/// <summary>
			public Vector3 GetPointAtPosition (float position, bool worldPosition = false) {
				if (worldPosition && parentTree != null) {
					return curve.GetPointAt (position).position + originOffset - parentTree.obj.transform.position;
				} else {
					return curve.GetPointAt (position).position + originOffset;
				}
			}
			/// <summary>
			/// Gets a vector at requested length at origin.
			/// </summary>
			/// <returns>Vector at the requested length.</returns>
			/// <param name="length">Length from origin.</param>
			public Vector3 GetPointAtLength (float length) {
				if (this.length > 0)
					return GetPointAtPosition (length / this.length);
				else
					return this.origin;
			}
			/// <summary>
			/// Gets the normal the branch position.
			/// </summary>
			/// <param name="position">Position from 0 to 1.</param>
			/// <param name="rollAngle">Angle in radians to rotate the normal using the forward vector as axis.</param>
			/// <returns>The normal at position.</returns>
			public Vector3 GetNormalAtPosition (float position, float rollAngle = 0f) {
				if (_shaper != null) {
					return _shaper.GetNormalAt (position, rollAngle, this);
				}
				if (curve != null) {
					if (rollAngle == 0f) {
						return curve.GetPointAt (position).normal;
					} else {
						CurvePoint point = curve.GetPointAt (position);
						return Quaternion.AngleAxis (rollAngle * Mathf.Rad2Deg, point.forward) * point.normal;
					}
				}
				return Vector3.forward;
			}
			/// <summary>
			/// Gets the normal at the branch length.
			/// </summary>
			/// <returns>The normal at length.</returns>
			/// <param name="length">Length.</param>
			public Vector3 GetNormalAtLength (float length) {
				return GetNormalAtPosition (length / this.length);
			}
			public Vector3 GetDirectionAtPosition (float position) {
				CurvePoint p = curve.GetPointAt (position);
				return p.forward;
			}
			public Vector3 GetDirectionAtLength (float length) {
				return GetDirectionAtPosition (length / this.length);
			}
			/// <summary>
			/// Get the factor used to calculate girth.
			/// </summary>
			/// <returns>The <see cref="System.Single"/>Girth factor at position.</returns>
			/// <param name="position">Position on the branch between 0 and 1.</param>
			public float GetGirthFactorAt (float position) {
				return (_girthAtTopFactor - _girthAtBaseFactor) * position + _girthAtBaseFactor;
			}
			/// <summary>
			/// Gets the girth value at a given position.
			/// </summary>
			/// <returns>The girth at position.</returns>
			/// <param name="position">Position between 0 a 1.</param>
			public float GetGirthAtPosition (float position) {
				float girth = (_maxGirth - _minGirth) * GetGirthCurve().Evaluate(1f - GetGirthFactorAt (position)) + _minGirth;
				//float ageFactor = Mathf.Clamp(age, 0, _level + _offspringLevels);
				//return girth * ageFactor;
				return Mathf.Clamp (girth * (isFollowUp?parent.girthScale:girthScale), _minGirth, _maxGirth);
			}
			/// <summary>
			/// Gets the girth value at a given length from branch origin.
			/// </summary>
			/// <returns>The girth of the branch at a given length.</returns>
			/// <param name="length">Length.</param>
			public float GetGirthAtLength (float length) {
				if (this.length > 0)
					return GetGirthAtPosition (length / this.length);
				else
					return Mathf.Clamp (_girthAtBaseFactor * _maxGirth * (isFollowUp?parent.girthScale:girthScale), _minGirth, _maxGirth);
			}
			/// <summary>
			/// Gets the distance from the center of the branch to its mesh surface given a relative position and roll angle.
			/// </summary>
			/// <param name="position">Relative position (0-1) at a branch length.</param>
			/// <param name="rollAngle">Roll angle in radians.</param>
			/// <returns>Distance from the center of the branch to is surface.</returns>
			public float GetSurfaceDistanceAt (float position, float rollAngle) {
				if (_shaper != null) {
					return _shaper.GetSurfaceDistanceAt (position, rollAngle, this);
				}
				return GetGirthAtPosition (position);
			}
			/// <summary>
			/// Gets the surface branch point given a relative position and roll angle in radians.
			/// </summary>
			/// <param name="position">Relative position (0-1) at a branch length.</param>
			/// <param name="rollAngle">Roll angle in radians.</param>
			/// <param name="applyTransforms">If <c>true</c> branch offset, direction and normal rotations are applied.</param>
			/// <returns>Surface point on this branch.</returns>
			public Vector3 GetSurfacePointAt (float position, float rollAngle, bool applyTransforms = true) {
				if (_shaper != null) {
					return _shaper.GetSurfacePointAt (position, rollAngle, this, applyTransforms);
				}
				CurvePoint curvePoint = curve.GetPointAt (position);
				float localGirth = GetGirthAtPosition (position);
				Vector3 point = new Vector3 (
					Mathf.Cos (rollAngle) * localGirth,
					Mathf.Sin (rollAngle) * localGirth,
					0f);
				if (applyTransforms) {
					Quaternion rotation = Quaternion.LookRotation (
						curvePoint.forward, 
						curvePoint.up);
					point = (rotation * point) + curvePoint.position + originOffset;
				}
				return point;
			}
			/// <summary>
			/// Get the point origin of this hierarchy of branches from the tree trunk.
			/// </summary>
			/// <param name="positionAtParent"></param>
			/// <returns></returns>
			public Vector3 GetTrunkPoint (float positionAtParent) {
				if (isTrunk || _parent == null) {
					return GetPointAtPosition (positionAtParent);
				} else {
					return parent.GetTrunkPoint (position);
				}
			}
			#endregion

			#region Clone
			/// <summary>
			/// Clones this instance with no extended objects.
			/// </summary>
			/// <returns>The clone instance.</returns>
			public Branch PlainClone (bool createObj = false) {
				Branch clone = new Branch ();
				clone.id = id;
				clone._accumLength = _accumLength;
				clone._maxGirth = _maxGirth;
				clone._minGirth = _minGirth;
				clone._girthAtBaseFactor = _girthAtBaseFactor;
				clone._girthAtTopFactor = _girthAtTopFactor;
				if (clone.girthCurve != null) {
					clone.girthCurve = new AnimationCurve (girthCurve.keys);
				}
				clone.girthScale = girthScale;
				clone._position = _position;
				clone._level = _level;
				clone._offspringLevels = _offspringLevels;
				clone.originOffset = originOffset;
				clone.parentOffset = parentOffset;
				clone.rollAngle = rollAngle;
				clone.helperStructureLevelId = helperStructureLevelId;
				clone.shaperOffset = shaperOffset;
				clone.curve = curve.Clone ();
				clone.phase = phase;
				clone.phase2 = phase2;
				clone.phaseLength = phaseLength;
				clone.phaseLengthOffset = phaseLengthOffset;
				clone.isTuned = isTuned;
				clone.isTrunk = isTrunk;
				clone.isRoot = isRoot;
				clone.isBroken = isBroken;
				clone.breakPosition = breakPosition;
				return clone;
			}
			#endregion
		}
		#endregion // END OF BRANCH CLASS.

		#region Vars
		/// <summary>
		/// List of branches contained in the tree.
		/// </summary>
		[SerializeField]
		List<Branch> _branches = new List<Branch> ();
		/// <summary>
		/// The branches positions.
		/// </summary>
		[SerializeField]
		List<Vector3> _branchesPositions = new List<Vector3> ();
		/// <summary>
		/// GameObject representing the tree.
		/// </summary>
		public GameObject obj = new GameObject ("root");
		/// <summary>
		/// Minimum girth found on this tree.
		/// </summary>
		public float minGirth = 0.05f; //TODO: get value from traversing branches.
		/// <summary>
		/// Maximum girth found on this tree.
		/// </summary>
		public float maxGirth = 0.25f;  //TODO: get value from traversing branches.
		/// <summary>
		/// Maximum length from the base of the tree to the top branch.
		/// </summary>
		private float _maxLength = -1f;
		/// <summary>
		/// Sets the origin of each branch of this tree hierarchy to the surface of its parent branch.
		/// </summary>
		private bool _autoBranchPositionOffset = true;
		/// <summary>
		/// Sets the origin of each branch of this tree hierarchy to the surface of its parent branch.
		/// </summary>
		public bool autoBranchPositionOffset {
			get { return _autoBranchPositionOffset; }
			set {
				_autoBranchPositionOffset = value;
				UpdatePosition ();
			}
		}
		/// <summary>
		/// Selects one branch for debugging on this tree.
		/// </summary>
		public int debugBranchId = -1;
		#endregion

		#region Getters and Setters
		/// <summary>
		/// Position to spawn the tree.
		/// </summary>
		/// <value>The position.</value>
		public Vector3 position {
			get { return obj.transform.position; }
			set {
				obj.transform.position = value;
			}
		}
		/// <summary>
		/// Branches on this tree.
		/// </summary>
		/// <value>The branches.</value>
		public List<Branch> branches {
			get { return this._branches; }
		}
		/// <summary>
		/// Gets the branches positions.
		/// </summary>
		/// <value>The branches positions.</value>
		public List<Vector3> branchesPositions {
			get { return this._branchesPositions; }
		}
		/// <summary>
		/// Gets the median girth for the branches on the tree.
		/// </summary>
		/// <value>The median girth.</value>
		public float medianGirth {
			get { return (minGirth + maxGirth) / 2f; }
		}
		#endregion

		#region Delegates
		/// <summary>
		/// Delegate for branch hierarchy operations.
		/// </summary>
		/// <param name="targetBranch">Target branch.</param>
		/// <param name="parentBranch">Parent or parent to be branch.</param>
		/// <param name="tree">Tree receiving the branch.</param>
		public delegate void OnBranchHierarchyDelegate (Branch targetBranch, Branch parentBranch, BroccoTree tree);
		/// <summary>
		/// Called before a branch gets added to this tree hierarchy.
		/// </summary>
		public OnBranchHierarchyDelegate onBeforeAddBranch;
		/// <summary>
		/// Called after a branch has beem added to this hierarchy.
		/// </summary>
		public OnBranchHierarchyDelegate onAddBranch;
		#endregion

		#region Constructor
		/// <summary>
		/// Initializes a new instance of the <see cref="Tree"/> class.
		/// </summary>
		public BroccoTree () { }
		#endregion

		#region Structure Methods
		/// <summary>
		/// Adds the branch.
		/// </summary>
		/// <param name="branch">Branch to add.</param>
		public void AddBranch (BroccoTree.Branch branch) {
			AddBranch (branch, Vector3.zero);
		}
		/// <summary>
		/// Adds the branch.
		/// </summary>
		/// <param name="branch">Branch.</param>
		/// <param name="position">Position.</param>
		public void AddBranch (BroccoTree.Branch branch, Vector3 position) {
			branch.RemoveShaper ();
			onBeforeAddBranch?.Invoke (branch, null, this);
			branch.parentTree = this;
			branch.phase = -1f;
			branch.phase2 = -1f;
			branch.phaseLength = 0f;
			branch.phaseLengthOffset = 0f;
			_branches.Add (branch);
			_branchesPositions.Add (position);
			branch.parentOffset = Vector3.zero;
			branch.originOffset = position;
			branch.isTrunk = true;
			int maxIterations = 30;
			Branch followUpBranch = branch.followUp;
			while (followUpBranch != null && maxIterations > 0) {
				followUpBranch.isTrunk = true;
				followUpBranch = followUpBranch.followUp;
				maxIterations--;
				if (maxIterations == 0) {
					Debug.LogWarning ("Max recursion found while processing tree branches.");
				}
			}
			branch.UpdatePosition ();
			branch.UpdateAccumLength ();
			onAddBranch?.Invoke (branch, null, this);
		}
		/// <summary>
		/// Update the structure of the tree.
		/// </summary>
		/// <param name="force">If set to <c>true</c> force the update.</param>
		public void Update (bool force = false) {
			for (int i = 0; i < _branches.Count; i++) {
				_branches[i].Update (force);
			}
		}
		/// <summary>
		/// Updates the girth.
		/// </summary>
		public void UpdateGirth () {
			for (int i = 0; i < _branches.Count; i++) {
				_branches[i].UpdateGirth (true);
			}
		}
		/// <summary>
		/// Updates the position.
		/// </summary>
		public void UpdatePosition () {
			for (int i = 0; i < _branches.Count; i++) {
				_branches[i].UpdatePosition ();
			}
		}
		/// <summary>
		/// Updates the follow up branches on the tree.
		/// </summary>
		public void UpdateFollowUps () {
			for (int i = 0; i < _branches.Count; i++) {
				_branches[i].UpdateFollowUps (true);
			}
		}
		/// <summary>
		/// Updates the curve resolution on every branch of the hierarchy.
		/// </summary>
		public void UpdateResolution (int resolutionSteps) {
			for (int i = 0; i < _branches.Count; i++) {
				_branches[i].UpdateResolution (resolutionSteps, true);
			}
		}
		/// <summary>
		/// Recalculates the normals.
		/// </summary>
		public void RecalculateNormals () {
			for (int i = 0; i < _branches.Count; i++) {
				_branches[i].RecalculateNormals ();
			}
		}
		public void CalculateSprouts () {
			for (int i = 0; i < _branches.Count; i++) {
				_branches[i].UpdateSprouts (true);
			}
		}
		/// <summary>
		/// Set the curve that interpolates between min and max girth of
		/// branches in the tree.
		/// </summary>
		/// <param name="curve">Curve to interpolate values.</param>
		public void SetBranchGirthCurve (AnimationCurve curve) {
			for (int i = 0; i < _branches.Count; i++) {
				_branches[i].girthCurve = curve;
			}
		}
		/// <summary>
		/// Sets a unique and non persistent id to each sprout on the tree.
		/// </summary>
		public void SetHelperSproutIds () {
			List<Branch> branches = GetDescendantBranches ();
			int sproutId = 0;
			for (int i = 0; i < branches.Count; i++) {
				for (int j = 0; j < branches[i].sprouts.Count; j++) {
					branches[i].sprouts[j].helperSproutId = sproutId;
					sproutId++;
				}
			}
			branches.Clear ();
		}
		/// <summary>
		/// Traverses the tree and set follow up branches with those with
		/// the greater offspring level if more than one child branch is at
		/// position 1.
		/// </summary>
		public void SetFollowUpBranchesByWeight () {
			List<Branch> branches = GetDescendantBranches ();
			int followUpOffsetLevels = -1;
			for (int i = 0; i < branches.Count; i++) {
				for (int j = 0; j < branches[i].branches.Count; j++) {
					if (branches[i].branches[j].position == 1 && branches[i].branches[j].offspringLevels > followUpOffsetLevels) {
						branches[i].followUp = branches[i].branches[j];
						branches[i].branches[j].isTrunk = branches[i].isTrunk;
						followUpOffsetLevels = branches[i].branches[j].offspringLevels;
					}
				}
			}
			branches.Clear ();
		}
		/// <summary>
		/// Deletes all objects from this tree.
		/// </summary>
		public void Clear() {
			List<Branch> branches = GetDescendantBranches ();
			for (int i = 0; i < branches.Count; i++) {
				branches[i].OnDestroy ();
			}
			_branches.Clear ();
			_branchesPositions.Clear ();
		}
		#endregion

		#region Traversing methods
		/// <summary>
		/// Traverse the tree and returns all branches.
		/// </summary>
		/// <returns>The descendant branches.</returns>
		public List<Branch> GetDescendantBranches () {
			List<Branch> children = new List<Branch> (this._branches);
			foreach (Branch child in _branches) {
				children.AddRange (child.GetDescendantBranches ());
			}
			return children;
		}
		/// <summary>
		/// Traverse the tree and returns branches corresponding to
		/// the desided generation level.
		/// </summary>
		/// <returns>The descendant branches.</returns>
		/// <param name="level">Level of the branches to return.</param>
		public List<Branch> GetDescendantBranches (int level) {
			List<Branch> allChildren = GetDescendantBranches ();
			List<Branch> levelChildren = new List<Branch> ();
			foreach (Branch child in allChildren) {
				if (child.GetLevel () == level)
					levelChildren.Add (child);
			}
			return levelChildren;
		}
		/// <summary>
		/// Get the max offspring level of branches on the tree.
		/// </summary>
		/// <returns>The offspring level.</returns>
		public int GetOffspringLevel () {
			int childrenBranchLevels = 0;
			if (_branches.Count > 0) {
				for (int i = 0; i < _branches.Count; i++) {
					if (_branches[i].offspringLevels > childrenBranchLevels) {
						childrenBranchLevels = _branches[i].offspringLevels;
					}
				}
				childrenBranchLevels += 1;
			}
			return childrenBranchLevels;
		}
		/// <summary>
		/// Get the maximum length of tree branches from the base of the trunk to the tip of the branches or roots.
		/// </summary>
		/// <param name="recalculate">True to recalculate the max length, false to use cached value.</param>
		/// <returns>Max length found from the base of the tree to the last branch tip.</returns>
		public float GetMaxLength (bool recalculate = false) {
			if (recalculate || _maxLength < 0) {
				_maxLength = 0f;
				for (int i = 0; i < branches.Count; i++) {
					GetMaxLengthRecursive (branches[i], 0f, 0);
				}
			}
			return _maxLength;
		}
		/// <summary>
		/// Recursive helper function to get the maximum distance from the root of the tree to the last branch or root.
		/// </summary>
		/// <param name="branch">Branch to inspect for length.</param>
		/// <param name="accumLength">Accumulated length.</param>
		/// <param name="loop">Recursive loop control.</param>
		void GetMaxLengthRecursive (BroccoTree.Branch branch, float accumLength, int loop) {
			if (accumLength + branch.length > _maxLength) {
				_maxLength = accumLength + branch.length;
			}
			if (branch.parent != null) {
				//if (branch.parent.parent == null) {
				if (branch.parent.isTrunk && (!branch.isTrunk || (branch.isTrunk && branch.followUp == null && branch.branches.Count == 0))) {
					branch.phase = Random.Range (0f, 1f);
					branch.phase2 = Random.Range (0f, 1f);
					branch.phaseLength = branch.GetOffspringMaxLength ();
					branch.phaseLengthOffset = branch.parent.GetLengthAtPos (branch.position, true);
				} else {
					branch.phase = branch.parent.phase;
					branch.phase2 = branch.parent.phase2;
					branch.phaseLength = branch.parent.phaseLength;
					branch.phaseLengthOffset = branch.parent.phaseLengthOffset;
				}
			} else {
				branch.phase = -1f;
				branch.phase2 = -1f;
				branch.phaseLength = 0f;
				branch.phaseLengthOffset = 0f;
			}
			if (loop > 50) {
				Debug.LogWarning ("Probable loop detected with connected branches.");
			}
			for (int i = 0; i < branch.branches.Count; i++) {
				GetMaxLengthRecursive (
					branch.branches [i], 
					accumLength + (branch.branches[i].position * branch.length), 
					loop + 1);
			}
		}
		#endregion
	}
}