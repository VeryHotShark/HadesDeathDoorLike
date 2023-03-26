#if UNITY_EDITOR

using System.Collections.Generic;
using System.IO;
using ExternalPropertyAttributes;
using UnityEditor;
using UnityEngine;

namespace Dustyroom {
[System.Serializable]
public class LODTopologyOptions {
    [Space] public int planeCount = 4;

    [Space] public int verticesX = 1;

    public int verticesY = 4;
}

[ExecuteInEditMode]
public class GrassMeshGenerator : MonoBehaviour {
    [Tooltip("The material applied to the generated grass patch. Please use 'Quibli/Grass' shader on the material.")]
    public Material material;

    [Tooltip("The width (X) and height (Y) of the grass patch in meters.")]
    public Vector2 patchSize = Vector2.one * 0.5f;

    [BoxGroup("LOD: 0"), Label("Options")] public LODTopologyOptions lod0;

    [BoxGroup("LOD: 1"), Label("Enabled")] public bool enableLod1 = true;

    [ShowIf(nameof(enableLod1)), BoxGroup("LOD: 1"), Label("Options")]
    public LODTopologyOptions lod1;

    [BoxGroup("LOD: 2"), Label("Enabled")] public bool enableLod2 = false;

    [ShowIf(nameof(enableLod2)), BoxGroup("LOD: 2"), Label("Options")]
    public LODTopologyOptions lod2;

    [SerializeField, HideInInspector] private string previousPath = "Assets/";

    [Button("Add to scene")]
    void AddToScene() {
        GeneratePatch();
    }

    [Button("Save prefab")]
    void Save() {
        var patch = GeneratePatch();
        var path = string.IsNullOrEmpty(previousPath) ? "Grass Patch.prefab" : previousPath;
        ExportAssets(patch, path);
    }

    [Button("Save prefab as...")]
    void SaveAs() {
        var patch = GeneratePatch();

        string path = EditorUtility.SaveFilePanel("Save the mesh as file", "Assets/", "Grass Patch", "prefab");
        if (string.IsNullOrEmpty(path)) {
            return;
        }

        path = FileUtil.GetProjectRelativePath(path);
        ExportAssets(patch, path);
        previousPath = path;
    }

    private void ExportAssets(GameObject patch, string path) {
        var meshes = patch.GetComponentsInChildren<MeshFilter>();
        var baseMeshPath =
            $"{Path.GetDirectoryName(path)}{Path.DirectorySeparatorChar}{Path.GetFileNameWithoutExtension(path)}-";

        foreach (var meshFilter in meshes) {
            var meshPath = Path.ChangeExtension($"{baseMeshPath}{meshFilter.name}", "asset");
            AssetDatabase.CreateAsset(meshFilter.sharedMesh, meshPath);
        }

        PrefabUtility.SaveAsPrefabAssetAndConnect(patch, path, InteractionMode.UserAction);
        AssetDatabase.SaveAssets();
    }

    private GameObject GeneratePatch() {
        var mesh0 = GenerateLod(lod0, 0);
        var mesh1 = GenerateLod(lod1, 1);
        var mesh2 = GenerateLod(lod2, 2);

        var go = new GameObject("Grass Patch");
        var go0 = new GameObject("Grass LOD 0");
        var meshFilter0 = go0.AddComponent<MeshFilter>();
        meshFilter0.mesh = mesh0;
        var renderer0 = go0.AddComponent<MeshRenderer>();
        renderer0.sharedMaterial = material;
        go0.transform.parent = go.transform;

        var lodGroup = go.AddComponent<LODGroup>();
        List<LOD> lods = new List<LOD>();
        lods.Add(new LOD(0.3f, new Renderer[] {renderer0}));

        if (enableLod1) {
            var go1 = new GameObject("Grass LOD 1");
            var meshFilter1 = go1.AddComponent<MeshFilter>();
            meshFilter1.mesh = mesh1;
            var renderer1 = go1.AddComponent<MeshRenderer>();
            renderer1.sharedMaterial = material;

            go1.transform.parent = go.transform;

            lods.Add(new LOD(enableLod2 ? 0.1f : 0.05f, new Renderer[] {renderer1}));
        }

        if (enableLod2) {
            var go2 = new GameObject("Grass LOD 2");
            var meshFilter2 = go2.AddComponent<MeshFilter>();
            meshFilter2.mesh = mesh2;
            var renderer2 = go2.AddComponent<MeshRenderer>();
            renderer2.sharedMaterial = material;

            go2.transform.parent = go.transform;

            lods.Add(new LOD(0.05f, new Renderer[] {renderer2}));
        }

        lodGroup.SetLODs(lods.ToArray());
        lodGroup.RecalculateBounds();

        return go;
    }

    private Mesh GenerateLod(LODTopologyOptions lod, int index) {
        var mesh = new Mesh {name = "Procedural Grass " + index};

        int numVerticesPerPlane = (lod.verticesX + 1) * (lod.verticesY + 1);
        Vector3[] vertices = new Vector3[numVerticesPerPlane * lod.planeCount];
        Vector3[] normals = new Vector3[vertices.Length];
        Vector2[] uv = new Vector2[vertices.Length];
        Vector4[] tangents = new Vector4[vertices.Length];
        int[] triangles = new int[lod.verticesX * lod.verticesY * lod.planeCount * 6];

        for (int planeIndex = 0; planeIndex < lod.planeCount; ++planeIndex) {
            float planeRotation = Mathf.PI / lod.planeCount * planeIndex;
            GeneratePlane(lod, vertices, normals, uv, tangents, triangles, planeIndex, planeRotation);
        }

        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.uv = uv;
        mesh.tangents = tangents;
        mesh.triangles = triangles;

        return mesh;
    }

    private void GeneratePlane(LODTopologyOptions lod, Vector3[] vertices, Vector3[] normals, Vector2[] uv,
                               Vector4[] tangents, int[] triangles, int planeIndex, float planeRotation) {
        int vertexStartIndex = (lod.verticesX + 1) * (lod.verticesY + 1) * planeIndex;
        for (int i = vertexStartIndex, y = 0; y <= lod.verticesY; y++) {
            for (int x = 0; x <= lod.verticesX; x++, i++) {
                float xRatio = (float) x / lod.verticesX;
                float yRatio = (float) y / lod.verticesY;

                float horizontalCornerCoord = patchSize.x * (xRatio - 0.5f);
                float vertexX = horizontalCornerCoord * Mathf.Sin(planeRotation);
                float vertexY = patchSize.y * yRatio;
                float vertexZ = horizontalCornerCoord * Mathf.Cos(planeRotation);

                vertices[i] = new Vector3(vertexX, vertexY, vertexZ);
                uv[i] = new Vector2(xRatio, yRatio);
                normals[i] = Vector3.up;
                tangents[i] = new Vector4(1f, 0f, 0f, -1f);
            }
        }

        int triangleStartIndex = lod.verticesX * lod.verticesY * 6 * planeIndex;
        for (int ti = triangleStartIndex, vi = vertexStartIndex, y = 0; y < lod.verticesY; y++, vi++) {
            for (int x = 0; x < lod.verticesX; x++, ti += 6, vi++) {
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + lod.verticesX + 1;
                triangles[ti + 5] = vi + lod.verticesX + 2;
            }
        }
    }
}
}

#endif