using System;
using System.Reflection;
using System.Collections.Generic;

using UnityEngine;

namespace Broccoli.Model {
    /// <summary>
    /// This class provides methods to build the surface of a branch.
    /// If an instance of an implementation of this class is set on a branch,
    /// methods to get the girth, normal and surface point at at a given position
    /// will be privided by this instance.
    /// </summary>
    public abstract class BranchShaper {
        #region Section Class
        /// <summary>
        /// Class to defines sections when dividing the shaper range.
        /// </summary>
        public class Section {
            public const int EASE_LINEAR = 0;
            public const int EASE_OUT_SINE = 1;
            public const int EASE_OUT_CUBIC = 2;
            public const int EASE_OUT_QUINT = 3;
            public const int EASE_OUT_CIRC = 4;
            public const int EASE_IN_SINE = 5;
            public const int EASE_IN_CUBIC = 6;
            public const int EASE_IN_QUINT = 7;
            public const int EASE_IN_CIRC = 8;
            public float fromLength = 0f;
            public float toLength = 1f;
            public float length {
                get { return toLength - fromLength; }
            }
            public float topScale = 1f;
            public float topCapPos = 1f;
            public float topCapScale = 1f;
            public int topCapFn = 0;
            public float topParam1 = 0f;
            public float topParam2 = 0f;
            public float topCapParam1 = 0f;
            public float topCapParam2 = 0f;
            public float bottomCapScale = 1f;
            public float bottomCapPos = 0f;
            public float bottomScale = 1f;
            public int bottomCapFn = 0;
            public float bottomParam1 = 0f;
            public float bottomParam2 = 0f;
            public float bottomCapParam1 = 0f;
            public float bottomCapParam2 = 0f;
            public void SetScale (float bottomScale, float bottomCapScale, float topScale, float topCapScale) {
                this.bottomScale = bottomScale;
                this.bottomCapScale = bottomCapScale;
                this.topScale = topScale;
                this.topCapScale = topCapScale;
            }
            public void SetLengthCapPosition (float bottomCapPos, float topCapPos) {
                this.bottomCapPos = bottomCapPos;
                this.topCapPos = topCapPos;
                if (topCapPos < bottomCapPos) topCapPos = bottomCapPos;
            }
            public void SetGirthCapPosition (float bottomGirth, float bottomCapScale, float topGirth, float topCapScale) {
                float _bottomCapPos = bottomGirth * bottomCapScale / length;
                float _topCapPos = 1f - (topGirth * topCapScale / length);
                if (_bottomCapPos > 0.5f) _bottomCapPos = 0.5f;
                if (_topCapPos < 0.5f) _topCapPos = 0.5f;
                SetLengthCapPosition (_bottomCapPos, _topCapPos);
            }
            public void SetParam1 (float bottomParam1, float bottomCapParam1, float topParam1, float topCapParam1) {
                this.bottomParam1 = bottomParam1;
                this.bottomCapParam1 = bottomCapParam1;
                this.topParam1 = topParam1;
                this.topCapParam1 = topCapParam1;
            }
            public void SetParam2 (float bottomParam2, float bottomCapParam2, float topParam2, float topCapParam2) {
                this.bottomParam2 = bottomParam2;
                this.bottomCapParam2 = bottomCapParam2;
                this.topParam2 = topParam2;
                this.topCapParam2 = topCapParam2;
            }
            public float GetScale (float position, out float param1, out float param2) {
                if (position < 0f) {
                    param1 = bottomParam1;
                    param2 = bottomParam2;
                    return bottomScale;
                } 
                if (position > 1f) {
                    param1 = topParam1;
                    param2 = topParam2;
                    return topScale;
                }
                if (position < bottomCapPos) {
                    // Bottom Cap.
                    float capPos = Mathf.InverseLerp (0f, bottomCapPos, position);
                    capPos = Easing (bottomCapFn, capPos);
                    param1 = Mathf.Lerp (bottomParam1, bottomCapParam1, capPos);
                    param2 = Mathf.Lerp (bottomParam2, bottomCapParam2, capPos);
                    return Mathf.Lerp (bottomScale, bottomCapScale, capPos);
                } else if (position > topCapPos) {
                    // Top Cap.
                    float capPos = Mathf.InverseLerp (1f, topCapPos, position);
                    capPos = Easing (topCapFn, capPos);
                    param1 = Mathf.Lerp (topParam1, topCapParam1, capPos);
                    param2 = Mathf.Lerp (topParam2, topCapParam2, capPos);
                    return Mathf.Lerp (topScale, topCapScale, capPos);
                } else {
                    // Middle Section.
                    float midPos = Mathf.InverseLerp (bottomCapPos, topCapPos, position);
                    param1 = Mathf.Lerp (bottomCapParam1, topCapParam1, midPos);
                    param2 = Mathf.Lerp (bottomCapParam2, topCapParam2, midPos);
                    return Mathf.Lerp (bottomCapScale, topCapScale, midPos);
                }
            }
            private float Easing (int fn, float x) {
                switch (fn) {
                    // Linear.
                    case EASE_LINEAR: return x;
                    // easeOutSine
                    case EASE_OUT_SINE: return Mathf.Sin ((x * Mathf.PI) / 2f);
                    // easeOutCubic
                    case EASE_OUT_CUBIC: return 1f - Mathf.Pow (1f - x, 3f);
                    // easeOutQuint
                    case EASE_OUT_QUINT: return 1f - Mathf.Pow( 1f - x, 5f);
                    // easeOutCirc
                    case EASE_OUT_CIRC: return Mathf.Sqrt (1f - Mathf.Pow (x - 1f, 2f));
                    // easeInSine
                    case EASE_IN_SINE: return 1f - Mathf.Cos ((x * Mathf.PI) / 2f);
                    // easeInCubic
                    case EASE_IN_CUBIC: return x * x * x;
                    // easeInQuint
                    case EASE_IN_QUINT: return x * x * x * x * x;
                    // easeInCirc
                    case EASE_IN_CIRC: return 1f - Mathf.Sqrt (1f - Mathf.Pow (x, 2f));
                }
                return x;
            }
        }
        #endregion

        #region Vars
        /// <summary>
        /// Length offset to calculate the shaper operations.
        /// </summary>
        public float lengthOffset = 0f;
        /// <summary>
        /// Sections to divide the shaper range.
        /// </summary>
        public List<Section> sections = new List<Section> ();
        /// <summary>
        /// Gets the length compromising the sections contained in this shaper.
        /// </summary>
        /// <value></value>
        public float shaperLength {
            get { 
                if (sections.Count > 0) return sections [sections.Count - 1].toLength;
                else return 0f;
            }
        }
        private static Dictionary<int, Type> _idToBranchShaperType = new Dictionary<int, Type>();
        private static Dictionary<int, BranchShaper> _idToBranchShaper = new Dictionary<int, BranchShaper>();
        private static List<int> _branchShaperIds = new List<int> ();
        private static List<string> _branchShaperNames = new List<string> ();
        public static string[] shaperNames;
        #endregion

        #region Constructor and Initialization
        /// <summary>
		/// Static constructor. Registers branch shapers.
		/// </summary>
		static BranchShaper () {
			_idToBranchShaperType.Clear ();
            _branchShaperIds.Clear ();
            _branchShaperNames.Clear ();
            _idToBranchShaper.Clear ();
            List<int> orders = new List<int> ();
			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies()) {
				foreach (Type type in assembly.GetTypes()) {
                    BranchShaperAttribute branchShaperAttr = type.GetCustomAttribute<BranchShaperAttribute> ();
					if (branchShaperAttr != null && branchShaperAttr.enabled) {
                        if (!_idToBranchShaperType.ContainsKey (branchShaperAttr.id)) {
                            _idToBranchShaperType.Add (branchShaperAttr.id, type);
                            bool added = false;
                            for (int i = 0; i < orders.Count; i++) {
                                if (branchShaperAttr.order <= orders [i]) {
                                    orders.Insert (i, branchShaperAttr.order);
                                    _branchShaperIds.Insert (i, branchShaperAttr.id);
                                    _branchShaperNames.Insert (i, branchShaperAttr.name);
                                    added = true;
                                    break;
                                }
                            }
                            if (!added) {
                                orders.Add (branchShaperAttr.order);
                                _branchShaperIds.Add (branchShaperAttr.id);
                                _branchShaperNames.Add (branchShaperAttr.name);
                            }
                        } else {
                            Debug.LogWarning ("Registering duplicated BranchShaper implementation with id " + branchShaperAttr.id);
                        }
					}
				}
			}
            shaperNames = new string[_branchShaperNames.Count];
            for (int i = 0; i < _branchShaperNames.Count; i++) {
                shaperNames [i] = _branchShaperNames [i];
            }
		}
        #endregion

        #region Management
        public static int GetShaperIndex (int shaperId) {
            int index = -1;
            for (int i = 0; i < _branchShaperIds.Count; i++) {
                if (_branchShaperIds [i] == shaperId) {
                    index = i;
                    break;
                }
            }
            return index;
        }
        public static int GetShaperId (int shaperIndex) {
            int id = 0;
            if (shaperIndex >= 0 && shaperIndex < _branchShaperIds.Count) {
                id = _branchShaperIds [shaperIndex];
            }
            return id;
        }
        public static BranchShaper GetInstance (int shaperId) {
            if (_idToBranchShaperType.ContainsKey (shaperId)) {
                return (BranchShaper)Activator.CreateInstance (_idToBranchShaperType [shaperId]);
            }
            return null;
        }
        public static BranchShaper GetSingleton (int shaperId) {
            if (_idToBranchShaper.ContainsKey (shaperId)) {
                return _idToBranchShaper [shaperId];
            } else {
                BranchShaper instance = GetInstance (shaperId);
                if (instance != null) _idToBranchShaper.Add (shaperId, instance);
                return instance;
            }
        }
        #endregion

        #region Abstract Methods
        /// <summary>
        /// Initializes this branch shaper. Called everytime the shaper gets set in a branch.
        /// </summary>
        public abstract void Init ();
        /// <summary>
        /// Gets the normal at a branch position given a roll angle in radiands.
        /// </summary>
        /// <param name="position">Relative position (0-1) at a branch length.</param>
        /// <param name="rollAngle">Roll angle in radians.</param>
        /// <param name="branch">Branch instance.</param>
        /// <returns>Normal vector at a branch position and roll angle.</returns>
        public abstract Vector3 GetNormalAt (float position, float rollAngle, BroccoTree.Branch branch);
        /// <summary>
        /// Gets the distance from the center of the branch to its mesh surface given a relative position and roll angle.
        /// </summary>
        /// <param name="position">Relative position (0-1) at a branch length.</param>
        /// <param name="rollAngle">Roll angle in radians.</param>
        /// <param name="branch">Branch instance.</param>
        /// <returns>Distance from the center of the branch to is surface.</returns>
        public abstract float GetSurfaceDistanceAt (float position, float rollAngle, BroccoTree.Branch branch);
        /// <summary>
        /// Gets the surface branch point given a relative position and roll angle.
        /// </summary>
        /// <param name="position">Relative position (0-1) at a branch length.</param>
        /// <param name="rollAngle">Roll angle in radians.</param>
        /// <param name="branch">Branch instance.</param>
        /// <param name="applyTransforms">If <c>true</c> branch offset, direction and normal rotations are applied.</param>
        /// <returns>Surface point on a branch.</returns>
        public abstract Vector3 GetSurfacePointAt (float position, float rollAngle, BroccoTree.Branch branch, bool applyTransforms = true);
        /// <summary>
        /// Get all the relevant positions along the BranchShaper instance, from 0 to 1.
        /// </summary>
        /// <param name="toleranceAngle">Angle tolerance to calculate the relevant positions.</param>
        /// <returns>List of relevant positions from 0 to 1 to the length of the shaper.</returns>
        public abstract List<float> GetRelevantPositions (float toleranceAngle = 5f);
        #endregion

        #region Exposed Properties
        public virtual bool bottomScaleExposed { get { return false; } }
        public virtual bool bottomCapScaleExposed { get { return false; } }
        public virtual bool topScaleExposed { get { return false; } }
        public virtual bool topCapScaleExposed { get { return false; } }
        public virtual bool bottomParam1Exposed { get { return false; } }
        public virtual bool bottomCapParam1Exposed { get { return false; } }
        public virtual bool topParam1Exposed { get { return false; } }
        public virtual bool topCapParam1Exposed { get { return false; } }
        public virtual bool bottomParam2Exposed { get { return false; } }
        public virtual bool bottomCapParam2Exposed { get { return false; } }
        public virtual bool topParam2Exposed { get { return false; } }
        public virtual bool topCapParam2Exposed { get { return false; } }
        public virtual bool bottomCapFnExposed { get { return false; } }
        public virtual bool topCapFnExposed { get { return false; } }
        public virtual bool bottomCapGirthPosExposed { get { return false; } }
        public virtual bool topCapGirthPosExposed { get { return false; } }
        public virtual bool isCapGirthPos { get { return false; } }
        public virtual string bottomScaleName { get { return string.Empty; } }
        public virtual string bottomCapScaleName { get { return string.Empty; } }
        public virtual string topScaleName { get { return string.Empty; } }
        public virtual string topCapScaleName { get { return string.Empty; } }
        public virtual string bottomParam1Name { get { return string.Empty; } }
        public virtual string bottomCapParam1Name { get { return string.Empty; } }
        public virtual string topParam1Name { get { return string.Empty; } }
        public virtual string topCapParam1Name { get { return string.Empty; } }
        public virtual string bottomParam2Name { get { return string.Empty; } }
        public virtual string bottomCapParam2Name { get { return string.Empty; } }
        public virtual string topParam2Name { get { return string.Empty; } }
        public virtual string topCapParam2Name { get { return string.Empty; } }
        public virtual string bottomCapFnName { get { return string.Empty; } }
        public virtual string topCapFnName { get { return string.Empty; } }
        public virtual string bottomCapGirthPosName { get { return string.Empty; } }
        public virtual string topCapGirthPosName { get { return string.Empty; } }
        public virtual float minScale { get { return 1f; } }
        public virtual float maxScale { get { return 4f; } }
        public virtual float minParam1 { get { return 1f; } }
        public virtual float maxParam1 { get {return 4f; } }
        public virtual float minParam2 { get { return 1f; } }
        public virtual float maxParam2 { get {return 4f; } }
        public virtual float minCapGirthPos { get { return 1f; } }
        public virtual float maxCapGirthPos { get { return 4f; } }
        public virtual void SetSectionScale (Section section, float bottomScale, float bottomCapScale, float topScale, float topCapScale) {
            section.bottomScale = bottomScale;
            section.bottomCapScale = bottomCapScale;
            section.topScale = topScale;
            section.topCapScale = topCapScale;
        }
        public virtual void SetSectionParam1 (Section section, float bottomParam1, float bottomCapParam1, float topParam1, float topCapParam1) {
            section.bottomParam1 = bottomParam1;
            section.bottomCapParam1 = bottomCapParam1;
            section.topParam1 = topParam1;
            section.topCapParam1 = topCapParam1;
        }
        public virtual void SetSectionParam2 (Section section, float bottomParam2, float bottomCapParam2, float topParam2, float topCapParam2) {
            section.bottomParam2 = bottomParam2;
            section.bottomCapParam2 = bottomCapParam2;
            section.topParam2 = topParam2;
            section.topCapParam2 = topCapParam2;
        }
        public virtual void SetSectionLengthCapPosition (Section section, float bottomCapPos, int bottomCapFn, float topCapPos, int topCapFn) {
            section.bottomCapFn = bottomCapFn;
            section.topCapFn = topCapFn;
            section.SetLengthCapPosition (bottomCapPos, topCapPos);
        }
        public virtual void SetSectionGirthCapPosition (
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