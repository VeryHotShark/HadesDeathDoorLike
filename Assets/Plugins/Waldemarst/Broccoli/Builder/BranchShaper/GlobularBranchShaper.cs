using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Broccoli.Model;

namespace Broccoli.Builder {
    /// <summary>
    /// This class provides methods to build the surface of a branch.
    /// If an instance of an implementation of this class is set on a branch,
    /// methods to get the girth, normal and surface point at at a given position
    /// will be privided by this instance.
    /// </summary>
    [BranchShaper (2, "Globular", 3)]
    public class GlobularBranchShaper : BranchShaper {
        #region Vars
        /// <summary>
        /// Initialization flag.
        /// </summary>
        private bool _isInit = false;
        #endregion

        #region Methods
        /// <summary>
        /// Initializes this branch shaper. Called everytime the shaper gets set in a branch.
        /// </summary>
        public override void Init () {
            if (!_isInit) {
                _isInit = true;
            }
        }
        /// <summary>
        /// Gets the normal at a branch position given a roll angle in radiands.
        /// </summary>
        /// <param name="position">Relative position (0-1) at a branch length.</param>
        /// <param name="rollAngle">Roll angle in radians.</param>
        /// <param name="branch">Branch instance.</param>
        /// <returns>Normal vector at a branch position and roll angle.</returns>
        public override Vector3 GetNormalAt (float position, float rollAngle, BroccoTree.Branch branch) {
            if (branch.curve != null) {
                if (rollAngle == 0f) {
                    return branch.curve.GetPointAt (position).normal;
                } else {
                    CurvePoint point = branch.curve.GetPointAt (position);
                    return Quaternion.AngleAxis (rollAngle * Mathf.Rad2Deg, point.forward) * point.normal;
                }
            }
            return Vector3.forward;
        }
        /// <summary>
        /// Gets the distance from the center of the branch to its mesh surface given a relative position and roll angle.
        /// </summary>
        /// <param name="position">Relative position (0-1) at a branch length.</param>
        /// <param name="rollAngle">Roll angle in radians.</param>
        /// <param name="branch">Branch instance.</param>
        /// <returns>Distance from the center of the branch to is surface.</returns>
        public override float GetSurfaceDistanceAt (float position, float rollAngle, BroccoTree.Branch branch) {
            float radiusA = 1f;
            float radiusB = 1f;
            // Get the base girth of the branch at the given position.
            float baseGirth = branch.GetGirthAtPosition (position);
            // Get the shaper length position.
            float shaperLengthPos = branch.shaperOffset + position * branch.length;
            // If the length is outside the sections range, return default surface point.
            if (shaperLengthPos < 0f || shaperLengthPos > shaperLength) {
                return baseGirth;
            } else {
                // Get the section the point falls into.
                BranchShaper.Section section = GetSection (shaperLengthPos);
                if (section != null) {
                    // Normalize the shaperLengthPos to the local section relative position.
                    float sectionPos = Mathf.InverseLerp (section.fromLength, section.toLength, shaperLengthPos);
                    baseGirth *= section.GetScale (sectionPos, out radiusA, out radiusB);
                }
                //point = GetDefaultSurfacePoint (baseGirth, rollAngle);
                Vector3 point = GetEllipsePoint (rollAngle, baseGirth * radiusA, baseGirth * radiusB , 0f);
                return point.magnitude;
            }
        }
        /// <summary>
        /// Gets the surface branch point given a relative position and roll angle.
        /// </summary>
        /// <param name="position">Relative position (0-1) at a branch length.</param>
        /// <param name="rollAngle">Roll angle in radians.</param>
        /// <param name="branch">Branch instance.</param>
        /// <param name="applyTransforms">If <c>true</c> branch offset, direction and normal rotations are applied.</param>
        /// <returns>Surface point on a branch.</returns>
        public override Vector3 GetSurfacePointAt (float position, float rollAngle, BroccoTree.Branch branch, bool applyTransforms = true) {
            CurvePoint curvePoint = branch.curve.GetPointAt (position);
            Vector3 point;
            float radiusA = 1f;
            float radiusB = 1f;
            // Get the base girth of the branch at the given position.
            float baseGirth = branch.GetGirthAtPosition (position);
            // Get the shaper length position.
            float shaperLengthPos = branch.shaperOffset + position * branch.length;
            // If the length is outside the sections range, return default surface point.
            if (shaperLengthPos < 0f || shaperLengthPos > shaperLength) {
                point = GetDefaultSurfacePoint (baseGirth, rollAngle);
            } else {
                // Get the section the point falls into.
                BranchShaper.Section section = GetSection (shaperLengthPos);
                if (section != null) {
                    // Normalize the shaperLengthPos to the local section relative position.
                    float sectionPos = Mathf.InverseLerp (section.fromLength, section.toLength, shaperLengthPos);
                    baseGirth *= section.GetScale (sectionPos, out radiusA, out radiusB);
                }
                //point = GetDefaultSurfacePoint (baseGirth, rollAngle);
                point = GetEllipsePoint (rollAngle, baseGirth * radiusA, baseGirth * radiusB , 0f);
            }
            if (applyTransforms) {
                Quaternion rotation = Quaternion.LookRotation (
                    curvePoint.forward, 
                    curvePoint.up);
                    /*
                if (branch.parentTree != null) {
                    point = (rotation * point) + curvePoint.position + branch.originOffset - branch.parentTree.obj.transform.position;
                } else {
                    point = (rotation * point) + curvePoint.position + branch.originOffset;
                }
                */
                point = (rotation * point) + curvePoint.position + branch.originOffset;
            }
            return point;
        }
        public Section GetSection (float length) {
            if (sections.Count == 0) return null;
            Section section = sections [0];
            for (int i = 1; i < sections.Count; i++) {
                if (length < sections [i].fromLength) break;
                section = sections [i];
            }
            return section;
        }
        protected Vector3 GetDefaultSurfacePoint (float baseGirth, float rollAngle) {
            return new Vector3 (
                Mathf.Cos (rollAngle) * baseGirth,
                Mathf.Sin (rollAngle) * baseGirth,
                0f);
        }
        static public Vector3 GetEllipsePoint (float angle, float xRadius, float yRadius, float rotationAngle)
        {
            Vector3 point = Vector3.zero;
            point.x = Mathf.Cos (angle) * xRadius;
            point.y = Mathf.Sin (angle) * yRadius;
            float cos = Mathf.Cos (rotationAngle);
            float sin = Mathf.Sin (rotationAngle);
            float pointX = point.x * cos - point.y * sin;
            float pointY = point.x * sin + point.y * cos;
            point.x = pointX;
            point.y = pointY;
            return point;
        }
        protected float GetGirthScale (Section section, float position) {
            if (position < 0f) return section.bottomScale;
            if (position > 1f) return section.topScale;
            if (position < section.bottomCapPos) {
                // Bottom Cap.
                float capPos = Mathf.InverseLerp (0f, section.bottomCapPos, position);
                return Mathf.Lerp (section.bottomScale, section.bottomCapScale, capPos);
            } else if (position > section.topCapPos) {
                // Top Cap.
                float capPos = Mathf.InverseLerp (1f, section.topCapPos, position);
                return Mathf.Lerp (section.topScale, section.topCapScale, capPos);
            } else {
                // Middle Section.
                float midPos = Mathf.InverseLerp (section.bottomCapPos, section.topCapPos, position);
                return Mathf.Lerp (section.bottomCapScale, section.topCapScale, midPos);
            }
        }
        /// <summary>
        /// Get all the relevant positions along the BranchShaper instance, from 0 to 1.
        /// </summary>
        /// <param name="toleranceAngle">Angle tolerance to calculate the relevant positions.</param>
        /// <returns>List of relevant positions from 0 to 1 to the length of the shaper.</returns>
        public override List<float> GetRelevantPositions (float toleranceAngle = 5f) {
            //toleranceAngle = 180 ultra low poly, 32 ultra high poly
            List<float> positions = new List<float> ();
            float length = 0f;
            BranchShaper.Section section;
            if (sections.Count > 0) {
                length = sections [sections.Count - 1].toLength;
                for (int i = 0; i < sections.Count; i++) {
                    section = sections [i];
                    if (section.bottomCapPos > 0f) {
                        // Add inbetween bottom and bottomCap.
                        int capSteps = Mathf.RoundToInt (Mathf.Lerp (2f, 4f, Mathf.InverseLerp (180f, 32f, toleranceAngle)));
                        float capStep = (section.bottomCapPos * section.length) / (float)capSteps;
                        for (int j = 1; j <= capSteps; j++) {
                            positions.Add ((section.fromLength + (capStep * j)) / length);  
                        }
                    }
                    if (section.topCapPos < 1f) {
                        int capSteps = Mathf.RoundToInt (Mathf.Lerp (2f, 4f, Mathf.InverseLerp (180f, 32f, toleranceAngle)));
                        float capStep = ((1f - section.topCapPos) * section.length) / (float)capSteps;
                        for (int j = 0; j < capSteps; j++) {
                            positions.Add ((section.fromLength + section.topCapPos * section.length + (capStep * j)) / length);
                        }
                    }
                    if (i < sections.Count - 1)
                        positions.Add (sections [i].toLength / length);
                }
            }
            return positions;
        }
        #endregion

        #region Exposed Properties
        private static string BOTTOM_CAP_SCALE_NAME = "Bottom Scale";
        private static string TOP_CAP_SCALE_NAME = "Top Scale";
        private static string BOTTOM_CAP_SCALE_GIRTH_NAME = "Bottom Cap Pos";
        private static string TOP_CAP_SCALE_GIRTH_NAME = "Top Cap Pos";
        public override bool bottomCapScaleExposed { get { return true; } }
        public override bool topCapScaleExposed { get { return true; } }
        public override bool bottomCapGirthPosExposed { get { return true; } }
        public override bool topCapGirthPosExposed { get { return true; } }
        public override bool isCapGirthPos { get { return true; } }
        public override string bottomCapScaleName { get { return BOTTOM_CAP_SCALE_NAME; } }
        public override string topCapScaleName { get { return TOP_CAP_SCALE_NAME; } }
        public override string bottomCapGirthPosName { get { return BOTTOM_CAP_SCALE_GIRTH_NAME; } }
        public override string topCapGirthPosName { get { return TOP_CAP_SCALE_GIRTH_NAME; } }
        public override void SetSectionScale (Section section, float bottomScale, float bottomCapScale, float topScale, float topCapScale) {
            section.bottomScale = 1f;
            section.bottomCapScale = bottomCapScale;
            section.topScale = 1f;
            section.topCapScale = topCapScale;
        }
        public override void SetSectionParam1 (Section section, float bottomParam1, float bottomCapParam1, float topParam1, float topCapParam1) {
            section.bottomParam1 = 1f;
            section.bottomCapParam1 = 1f;
            section.topParam1 = 1f;
            section.topCapParam1 = 1f;
        }
        public override void SetSectionParam2 (Section section, float bottomParam2, float bottomCapParam2, float topParam2, float topCapParam2) {
            section.bottomParam2 = 1f;
            section.bottomCapParam2 = 1f;
            section.topParam2 = 1f;
            section.topCapParam2 = 1f;
        }
        public override void SetSectionLengthCapPosition (Section section, float bottomCapPos, int bottomCapFn, float topCapPos, int topCapFn) {
            section.bottomCapFn = bottomCapFn;
            section.topCapFn = topCapFn;
            section.SetLengthCapPosition (bottomCapPos, topCapPos);
        }
        public override void SetSectionGirthCapPosition (
            Section section, 
            float bottomGirth, 
            float bottomCapScale,
            int bottomCapFn,
            float topGirth, 
            float topCapScale,
            int topCapFn)
        {
            section.bottomCapFn = bottomCapFn;
            section.topCapFn = topCapFn;
            section.SetGirthCapPosition (bottomGirth, bottomCapScale, topGirth, topCapScale);
        }
        #endregion
    }
}