using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Broccoli.Model;

namespace Broccoli.Pipe {
    /// <summary>
    /// Variation group class.
    /// </summary>
    [System.Serializable]
    public class VariationGroup {
        #region Vars
        /// <summary>
        /// Characters used to generate a random name for the framing.
        /// </summary>
        const string glyphs= "abcdefghijklmnopqrstuvwxyz0123456789";
        public int id = 0;
        public string name = "";
        public bool enabled = true;
        public int seed = 0;
        public int minFrequency = 1;
        public int maxFrequency = 4;
        public Vector3 center = Vector3.zero;
        public float minRadius = 0f;
        public float maxRadius = 0f;
        public float centerFactor = 0f;
        public enum OrientationMode {
            CenterToPeriphery,
            PeripheryToCenter,
            clockwise,
            counterClockwise
        }
        public OrientationMode orientation = OrientationMode.CenterToPeriphery;
        public float orientationRandomness = 0f;
        public float minTiltAtCenter = 0f;
        public float maxTiltAtCenter = 0f;
        public float minTiltAtBorder = 0f;
        public float maxTiltAtBorder = 0f;
        public float minScaleAtCenter = 1f;
        public float maxScaleAtCenter = 1f;
        public float minScaleAtBorder = 1f;
        public float maxScaleAtBorder = 1f;
        public enum BendMode {
            CenterToPeriphery,
            PeripheryToCenter,
            clockwise,
            counterClockwise
        }
        public BendMode bendMode = BendMode.CenterToPeriphery;
        public float minBendAtCenter = 0f;
        public float maxBendAtCenter = 0f;
        public float minBendAtBorder = 0f;
        public float maxBendAtBorder = 0f;
        public List<int> snapshotIds = new List<int> ();
        #endregion

        #region Constructor
        public VariationGroup () {}
        #endregion

        #region Clone
        /// <summary>
        /// Clone this instance.
        /// </summary>
        public VariationGroup Clone () {
            VariationGroup clone = new VariationGroup ();
            clone.id = id;
            clone.name = name;
            clone.enabled = enabled;
            clone.seed = seed;
            clone.minFrequency = minFrequency;
            clone.maxFrequency = maxFrequency;
            clone.center = center;
            clone.minRadius = minRadius;
            clone.maxRadius = maxRadius;
            clone.centerFactor = centerFactor;
            clone.orientation = orientation;
            clone.orientationRandomness = orientationRandomness;
            clone.minTiltAtCenter = minTiltAtCenter;
            clone.maxTiltAtCenter = maxTiltAtCenter;
            clone.minTiltAtBorder = minTiltAtBorder;
            clone.maxTiltAtBorder = maxTiltAtBorder;
            clone.minScaleAtCenter = minScaleAtCenter;
            clone.maxScaleAtCenter = maxScaleAtCenter;
            clone.minScaleAtBorder = minScaleAtBorder;
            clone.maxScaleAtBorder = maxScaleAtBorder;
            clone.bendMode = bendMode;
            clone.minBendAtCenter = minBendAtCenter;
            clone.maxBendAtCenter = maxBendAtCenter;
            clone.minBendAtBorder = minBendAtBorder;
            clone.maxBendAtBorder = maxBendAtBorder;
            for (int i = 0; i < snapshotIds.Count; i++) {
                clone.snapshotIds.Add (snapshotIds [i]);
            }
            return clone;
        }
        /// <summary>
		/// Get a random string name.
		/// </summary>
		/// <param name="length">Number of characters.</param>
		/// <returns>Random string name.</returns>
        public static string GetRandomName (int length = 6) {
            string randomName = "";
            Random.InitState ((int)System.DateTime.Now.Ticks);
            for(int i = 0; i < 6; i++) {
                randomName += glyphs [Random.Range (0, glyphs.Length)];
            }
            return randomName;
        }
        #endregion

        #region Snapshot Management
        /// <summary>
        /// Adds a snapshot id to be part of this group.
        /// </summary>
        /// <param name="snapshotId">Id fo the snapshot.</param>
        /// <returns><c>True</c> if the snapshot gets added.</returns>
        public bool AddSnapshot (int snapshotId) {
            if (!snapshotIds.Contains (snapshotId)) {
                snapshotIds.Add (snapshotId);
            }
            return false;
        }
        /// <summary>
        /// Removes a snapshot from this group given its id.
        /// </summary>
        /// <param name="snapshotId">Id of the snapshot.</param>
        /// <returns><c>True</c> if the snapshot was removed.</returns>
        public bool RemoveSnapshot (int snapshotId) {
            int index = snapshotIds.IndexOf (snapshotId);
            if (index >= 0) {
                snapshotIds.RemoveAt (index);
                return true;
            }
            return false;
        }
        #endregion
    }
}