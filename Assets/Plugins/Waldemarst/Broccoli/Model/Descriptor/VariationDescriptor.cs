using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Broccoli.Model;

namespace Broccoli.Pipe {
    /// <summary>
    /// Composite variation container class.
    /// </summary>
    [System.Serializable]
    public class VariationDescriptor {
        #region Variation Group Cluster
        [System.Serializable]
        public class VariationUnit {
            public Vector3 position = Vector3.zero;
            public Quaternion rotation = Quaternion.identity;
            public Vector3 orientation = Vector3.right;
            public float scale = 1f;
            public float bending = 0f;
            public int snapshotIndex = -1;
        }
        [System.Serializable]
        public class VariationGroupCluster {
            #region Vars
            public int groupId = 0;
            public float radius = 0f;
            public float centerFactor = 0f;
            public List<VariationUnit> variationUnits = new List<VariationUnit> ();
            #endregion
        }
        #endregion

        #region Structure Vars
        public int id = 0;
        public int seed = 0;
        public List<VariationGroupCluster> variationGroupCluster = new List<VariationGroupCluster> ();
        public List<VariationGroup> variationGroups = new List<VariationGroup> ();
        /// <summary>
        /// List of snapshot ids used by this variation.
        /// </summary>
        /// <typeparam name="int">Id of a snapshot.</typeparam>
        /// <returns>List of snapshot ids.</returns>
        public List<int> snapshotIds = new List<int> ();
        #endregion

        #region Constructor
        public VariationDescriptor () {}
        #endregion

        #region Clone
        /// <summary>
        /// Clone this instance.
        /// </summary>
        public VariationDescriptor Clone () {
            VariationDescriptor clone = new VariationDescriptor ();
            clone.id = id;
            clone.seed = seed;
            for (int i = 0; i < variationGroups.Count; i++) {
                clone.variationGroups.Add (variationGroups [i].Clone ());
            }
            return clone;
        }
        #endregion

        #region Groups Management
        /// <summary>
        /// Adds a Variation Group to this Variation Descriptor.
        /// </summary>
        /// <param name="groupToAdd"></param>
        public void AddGroup (VariationGroup groupToAdd) {
            groupToAdd.id = GetGroupId ();
            variationGroups.Add (groupToAdd);
        }
        public bool RemoveGroup (int groupIndex) {
            if (groupIndex >= 0 && groupIndex < variationGroups.Count) {
                variationGroups.RemoveAt (groupIndex);
                return true;
            }
            return false;
        }
        int GetGroupId () {
            int id = 0;
            for (int i = 0; i < variationGroups.Count; i++) {
                if (variationGroups [i].id >= id) {
                    id = variationGroups [i].id + 1;
                }
            }
            return id;
        }
        #endregion
    }
}