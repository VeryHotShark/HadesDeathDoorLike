using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Broccoli.Builder
{
    /// <summary>
    /// Builder for sprout meshes.
    /// This class produce a mesh prototype to be used to create sprouts.
    /// The mesh has this topology:
    /// 1. The XZ plane is used as the horizontal plane, with the X vector for width andZ vector for height.
    /// 2. The Y vector is the up side (normal) for the sprout.
    /// The mesh contains the following data:
    /// 1. Vertices, triangles, normals (up) and tangent (right).
    /// 2. UV mapping 0-1 values on the XY and width/height values on the ZW coordinates.
    /// 3. UV2 mapping with gradient values from center to vertex, length on X and 0-1 on Y coordinate.
    /// </summary>
    public abstract class BaseSproutMeshBuilder
    {
        #region Methods
        /// <summary>
        /// Set the parameters for this builder from a json string.
        /// </summary>
        /// <param name="jsonParams">Parameters in JSON format.</param>
        public abstract void SetParams (string jsonParams);
        /// <summary>
        /// Creates a mesh for this builder.
        /// </summary>
        /// <returns>Mesh.</returns>
        public abstract Mesh GetMesh ();
        /// <summary>
        /// Clear this builder variables.
        /// </summary>
        public abstract void Clear ();
        #endregion
    }
}
