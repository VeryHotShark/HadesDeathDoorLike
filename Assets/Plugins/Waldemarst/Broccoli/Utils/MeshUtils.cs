using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Broccoli.Utils
{
    public class MeshUtils {
        public static Mesh GetPlane (
            float width, 
            float height, 
            int widthCells, 
            int heightCells,
            float pivotW = 0.5f,
            float pivotH = 0f) 
        {
            Vector3[] planeVertices = new Vector3 [(widthCells + 1) * (heightCells + 1)];
			Vector3[] planeNormals = new Vector3 [planeVertices.Length];
			Vector2[] planeUVs = new Vector2 [planeVertices.Length];
            int[] triangles = new int[widthCells * heightCells * 6];

            // Starting position for the grid on the width.
			float posW = -pivotW * width;
			// Starting position for the grid on the height.
			float posH = -pivotH * height;
            // Max diagonal length on the plane.
			float maxLength = 1f;
			float widthFromPivot, heightFromPivot;
			if (pivotW * width > width - (pivotW * width)) {
				widthFromPivot = pivotW * width;
			} else {
				widthFromPivot = width - (pivotW * width);
			}
			if (pivotH * height > height - (pivotH * height)) {
				heightFromPivot = pivotH * height;
			} else {
				heightFromPivot = height - (pivotH * height);
			}
			maxLength = Mathf.Sqrt (Mathf.Pow (widthFromPivot, 2) + Mathf.Pow (heightFromPivot, 2));
			// Value for each segment on the width.
			float segmentW = width / widthCells;
			// Value for each segment on the height.
			float segmentH = height / heightCells;
			// Positions in space for the plane.
			float posX = 0f;
			float posY = posH;
			float posZ = posW;
			float posU = 0f;
			float posV = 0f;
			// Points count.
			int pointCount = 0;

            for (int j = 0; j <= heightCells; j++) {
                for (int i = 0; i <= widthCells; i++) {
                    planeVertices [pointCount] = new Vector3 (posX, posY, posZ);
                    posU = (posZ - posW) / width;
                    posV = (posY - posH) / height;
                    planeNormals [pointCount] = Vector3.right;
                    planeUVs [pointCount] = new Vector4 (1f - posV, posU, 1f - posV, posU);
                    /*
                    } else if (planeDef.uvSteps == 2) {
                        planeUVs [pointCount] = new Vector4 (1f - posU, 1f - posV, 1f - posU, 1f - posV);
                    } else if (planeDef.uvSteps == 3) {
                        planeUVs [pointCount] = new Vector4 (posV, 1f - posU, posV, 1f - posU);
                    } else {
                        planeUVs [pointCount] = new Vector4 (posU, posV, posU, posV);
                    }
                    */
                    posZ += segmentW;
                    pointCount++;
                }
                posZ = posW;
                posY += segmentH;
            }
            
            // Add triangles
            int k = 0;
			for (int j = 0; j < heightCells; j++) {
				for (int i = 0; i < widthCells; i++) {
					pointCount = i + (j * (widthCells + 1));
					triangles [k] = pointCount;
					triangles [k + 1] = pointCount + widthCells + 1;
					triangles [k + 2] = pointCount + widthCells + 2;
					triangles [k + 3] = pointCount;
					triangles [k + 4] = pointCount + widthCells + 2;
					triangles [k + 5] = pointCount + 1;
                    k += 6;
				}
			}

            Mesh mesh = new Mesh ();
            mesh.SetVertices (planeVertices);
            mesh.SetNormals (planeNormals);
            mesh.SetUVs (0, planeUVs);
            mesh.SetTriangles (triangles, 0);
            mesh.RecalculateTangents ();

            return mesh;
        }
        /// <summary>
        /// Combines all the submeshes of an existing mesh.
        /// </summary>
        /// <param name="mesh">Mesh to combine its submeshes.</param>
        /// <returns>Combined mesh.</returns>
        public static Mesh CombineSubMeshes (Mesh mesh) {
            Mesh combined = new Mesh ();
            List<CombineInstance> combines = new List<CombineInstance> ();
            CombineInstance combine;
            for (int i = 0; i < mesh.subMeshCount; i++) {
                combine = new CombineInstance ();
                combine.mesh = mesh;
                combine.transform = Matrix4x4.identity;
                combine.subMeshIndex = i;
                combines.Add (combine);
            }
            combined.CombineMeshes (combines.ToArray (), true, false);
            return combined;
        }
        /// <summary>
        /// Combine all submeshes from a list of meshes into one single mesh.
        /// </summary>
        /// <param name="meshes">List of meshes.</param>
        /// <param name="subMeshIndex">Submesh index.</param>
        /// <returns>Combined mesh for the ibdex submesh.</returns>
        public static Mesh CombineMeshes (List<Mesh> meshes, int subMeshIndex) {
            Mesh mergingMesh = new Mesh ();
			mergingMesh.subMeshCount = meshes.Count;
            List<CombineInstance> combines = new List<CombineInstance> ();
            CombineInstance combine;
            for (int i = 0; i < meshes.Count; i++) {
                combine = new CombineInstance ();
                combine.mesh = meshes [i];
                combine.transform = Matrix4x4.identity;
                combine.subMeshIndex = subMeshIndex;;
                combines.Add (combine);
            }
			mergingMesh.CombineMeshes (combines.ToArray (), true, false);
            return mergingMesh;
        }
        public static Mesh CombineMeshes (List<Mesh> meshes, bool mergeSubmeshes = false) {
            Mesh mergingMesh = new Mesh ();
			mergingMesh.subMeshCount = meshes.Count;
            List<CombineInstance> combines = new List<CombineInstance> ();
            CombineInstance combine;
            for (int i = 0; i < meshes.Count; i++) {
                for (int j = 0; j < meshes [i].subMeshCount; j++) {
                    combine = new CombineInstance ();
                    combine.mesh = meshes [i];
                    combine.transform = Matrix4x4.identity;
                    combine.subMeshIndex = j;
                    combines.Add (combine);
                }
            }
			mergingMesh.CombineMeshes (combines.ToArray (), mergeSubmeshes, false);
            return mergingMesh;
        }
        public static Mesh CombineMeshesAdditive (List<Mesh> meshes) {
            Mesh mergingMesh = new Mesh ();
			mergingMesh.subMeshCount = meshes.Count;
            List<CombineInstance> combines = new List<CombineInstance> ();
            CombineInstance combine;
            for (int i = 0; i < meshes.Count; i++) {
                for (int j = 0; j < meshes [i].subMeshCount; j++) {
                    combine = new CombineInstance ();
                    combine.mesh = meshes [i];
                    combine.transform = Matrix4x4.identity;
                    combine.subMeshIndex = j;
                    combines.Add (combine);
                }
            }
			mergingMesh.CombineMeshes (combines.ToArray (), false, false);
            return mergingMesh;
        }
        public static Mesh CombineMeshesMultiplicative (Mesh srcMesh, int factor, bool mergeSubmeshes = false) {
            if (factor < 0) factor = 0;
            Mesh mergingMesh = new Mesh ();
			mergingMesh.subMeshCount = srcMesh.subMeshCount;
            List<CombineInstance> combines = new List<CombineInstance> ();
            CombineInstance combine;
            for (int i = 0; i < factor; i++) {
                for (int j = 0; j < srcMesh.subMeshCount; j++) {
                    combine = new CombineInstance ();
                    combine.mesh = srcMesh;
                    combine.transform = Matrix4x4.identity;
                    combine.subMeshIndex = j;
                    combines.Add (combine);
                }
            }
			mergingMesh.CombineMeshes (combines.ToArray (), mergeSubmeshes, false);
            return mergingMesh;   
        }
    }
}
