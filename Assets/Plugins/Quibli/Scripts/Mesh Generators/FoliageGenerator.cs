#if UNITY_EDITOR

using System;
using System.IO;
using ExternalPropertyAttributes;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Dustyroom {
public class FoliageGenerator : MonoBehaviour {
    #region Options

    public enum CarrierSampling {
        Random,
        Uniform,
    }

    [BoxGroup("Generation"), Required("Carrier Mesh is required. Unity's standard sphere is a good starting point.")]
    [Tooltip("The triangles of this mesh are used for placement of the spawned particles.")]
    [ShowAssetPreview]
    [SerializeField]
    public Mesh carrierMesh;

    [BoxGroup("Generation"), Required("Particle Mesh is required. Unity's standard plane is a good starting point.")]
    [Tooltip("The building block of the foliage. Multiple copies of this mesh are combined in the exported mesh.")]
    [ShowAssetPreview]
    [SerializeField]
    public Mesh particleMesh;

    [BoxGroup("Generation"), Space]
    [Tooltip("Scaling applied to the carrier mesh when spawning particles.")]
    public Vector3 carrierScale = Vector3.one;

    [BoxGroup("Generation")]
    [Tooltip("Scaling applied to each individual particle.")]
    public Vector3 particleScale = Vector3.one;

    [BoxGroup("Generation")]
    [Tooltip("Randomness of scale applied to each individual particle.")]
    [Range(0, 1)]
    public float particleScaleVariance = 0.2f;

    [BoxGroup("Generation"), Label("Particles"), Space]
    [Tooltip("The number of particles to generate.")]
    public int numParticles = 250;

    [BoxGroup("Generation"), Label("Placement type"), Space]
    [Tooltip("Defines the positions of the carrier mesh at which particles are added. " +
             "'Random' selects random faces on the carrier mesh, 'Uniform' samples the mesh faces sequentially.")]
    public CarrierSampling carrierSampling = CarrierSampling.Random;

    [BoxGroup("Generation")]
    [Tooltip("'Inflate' the mesh by moving each particle along the carrier mesh normal by this value. " +
             "Negative values result in more compact mesh.")]
    public float offsetAlongNormal = -0.25f;

    [BoxGroup("Generation")]
    [Tooltip("Defines which particles offset is applied to. The value of 1 means all particles are offset. " +
             "Useful to create branches that stick out of the general foliage shape.")]
    [Range(0, 1)]
    [EnableIf(nameof(EnableOffsetAlongNormalFraction))]
    [Label("    Fraction Of Particles")]
    public float offsetAlongNormalFraction = 1.0f;

    [BoxGroup("Generation"), Range(0f, 360f)]
    [Tooltip("How much particle rotations can deviate from carrier mesh normals.")]
    public float particleRotationRange = 90f;

    [BoxGroup("Generation")]
    [Space]
    [Tooltip("If enabled, the normals are calculated from the generated geometry, resulting in a more physically " +
             "correct, but less stylized look. If disabled, the normals are transferred from a sphere, regardless of " +
             "the model geometry, achieving a cleaner, more \"fluffy\" look.")]
    public bool geometryBasedNormals = false;

    [BoxGroup("Generation")]
    [Tooltip("If enabled, the vertices within each particle will have the same normal values. " +
             "Useful to hide plane intersections.")]
    public bool oneNormalPerParticle = false;

    [BoxGroup("Billboard whole object"), Range(0f, 1f)]
    [Tooltip("Nudge particles to face 'Bias Toward Rotation' value. Useful for billboard foliage.")]
    public float particleRotationBias = 0f;

    [BoxGroup("Billboard whole object"), EnableIf(nameof(EnableBiasTowardRotation))]
    [Tooltip("Particles are oriented to this rotation based on 'Particle Rotation Bias'.")]
    public Vector3 biasTowardRotation = Vector3.zero;

    private bool EnableBiasTowardRotation => particleRotationBias > 0;
    private bool EnableOffsetAlongNormalFraction => offsetAlongNormal > 0;

    [BoxGroup("Normal Noise")]
    [Label("Enable")]
    public bool noiseEnabled = false;

    [EnableIf(nameof(noiseEnabled)), BoxGroup("Normal Noise")]
    [Label("Frequency")]
    public float noiseFrequency = 1f;

    [EnableIf(nameof(noiseEnabled)), BoxGroup("Normal Noise")]
    [Label("Amplitude")]
    public float noiseAmplitude = 1f;

    [EnableIf(nameof(noiseEnabled)), BoxGroup("Normal Noise")]
    [Label("Octaves"), Range(1, 5)]
    public uint noiseOctaves = 1;

    [EnableIf(nameof(noiseEnabled)), BoxGroup("Normal Noise")]
    [Label("Scale")]
    public Vector3 noiseScale = Vector3.one;

    [EnableIf(nameof(noiseEnabled)), BoxGroup("Normal Noise")]
    [Label("Seed")]
    public uint noiseSeed = 1;

    [BoxGroup("Export")]
    [Tooltip("Where in the project the new mesh should be exported. E.g. 'Meshes' will export to 'Assets/Meshes'.")]
    public string folderPath = "Meshes";

    [BoxGroup("Export")]
    public string fileNamePrefix = "Quad Tree";

    [BoxGroup("Export")]
    public bool appendMeshName = true;

    [BoxGroup("Export")]
    public bool appendTakeNumber = false;

    [BoxGroup("Export")]
    [ShowIf(nameof(appendTakeNumber)), Label("    Take Number")]
    public int takeNumber = 1;

    [BoxGroup("Export")]
    public bool appendTimestamp = false;

    [BoxGroup("Export")]
    [Tooltip("Overwrites the exported mesh when any value is changed in this component.")]
    public bool exportOnEdit = false;

    #endregion

    private void OnValidate() {
        if (exportOnEdit) ExportMesh();
    }

    [Button]
    private void ExportMesh() {
        // Validate input.
        if (carrierMesh == null || particleMesh == null) {
            return;
        }

        GenerateMesh(out Mesh mesh, out Mesh meshDebug);

        // Export asset.
        {
            Directory.CreateDirectory("Assets/" + folderPath);
            SaveMeshAsset(mesh, GetFullFilename());
            AssetDatabase.SaveAssets();
        }
    }

    private void GenerateMesh(out Mesh mesh, out Mesh meshDebug) {
        mesh = new Mesh();
        meshDebug = new Mesh {
            vertices = carrierMesh.vertices, triangles = carrierMesh.triangles, uv = carrierMesh.uv
        };

        Vector3 particleAverageNormal = Vector3.zero;
        for (int i = 0; i < particleMesh.vertexCount; i++) {
            particleAverageNormal += particleMesh.normals[i];
        }

        particleAverageNormal.Normalize();

        var numTriangles = carrierMesh.triangles.Length / 3;
        CombineInstance[] combine = new CombineInstance[numParticles];

        for (int i = 0; i < numParticles; i++) {
            var particle = new Mesh {
                vertices = particleMesh.vertices,
                triangles = particleMesh.triangles,
                colors = particleMesh.colors,
                normals = particleMesh.normals,
                tangents = particleMesh.tangents,
                uv = particleMesh.uv,
            };

            int triangleIndex = carrierSampling == CarrierSampling.Random
                ? Random.Range(0, numTriangles)
                : (int)Mathf.Lerp(0, numTriangles, i / (float)numParticles);

            var vertexIndex0 = carrierMesh.triangles[triangleIndex * 3 + 0];
            var vertexIndex1 = carrierMesh.triangles[triangleIndex * 3 + 1];
            var vertexIndex2 = carrierMesh.triangles[triangleIndex * 3 + 2];

            var a = carrierMesh.vertices[vertexIndex0];
            var b = carrierMesh.vertices[vertexIndex1];
            var c = carrierMesh.vertices[vertexIndex2];

            var p = GetRandomPositionWithinTriangle(a, b, c);
            p = Vector3.Scale(p, carrierScale);

            var triangleNormal = (carrierMesh.normals[vertexIndex0] + carrierMesh.normals[vertexIndex1] +
                                  carrierMesh.normals[vertexIndex2]).normalized;
            var triangleTangent = (carrierMesh.tangents[vertexIndex0] + carrierMesh.tangents[vertexIndex1] +
                                   carrierMesh.tangents[vertexIndex2]).normalized;
            if (Random.value <= offsetAlongNormalFraction) {
                p += triangleNormal * offsetAlongNormal;
            }

            var up = Vector3.Cross(triangleNormal, triangleTangent);
            var rotation = Quaternion.LookRotation(-triangleNormal, up);

            // Random rotation for particle clouds.
            rotation = Quaternion.RotateTowards(rotation, Random.rotationUniform, particleRotationRange);

            // Rotation bias for billboard meshes.
            var biasTowards = Quaternion.Euler(biasTowardRotation);
            rotation = Quaternion.RotateTowards(rotation, biasTowards, 180f * particleRotationBias);

            // Give the particle a random forward rotation.
            rotation *= Quaternion.AngleAxis(360f * Random.value, particleAverageNormal);

            var scaleVariance = Random.Range(1f - particleScaleVariance, 1f);
            var particleTransform = Matrix4x4.Translate(p) * Matrix4x4.Rotate(rotation) *
                                    Matrix4x4.Scale(particleScale * scaleVariance) * Matrix4x4.identity;

            var combineInstance = new CombineInstance { mesh = particle, transform = particleTransform };
            combine[i] = combineInstance;
        }

        mesh.CombineMeshes(combine, true, true);

        // Calculate normals.
        {
            mesh.RecalculateNormals();  // Needed if geometryBasedNormals == true.

            var normals = new Vector3[mesh.vertexCount];

            for (int i = 0; i < normals.Length; i++) {
                int vertexIndex = oneNormalPerParticle ? i - i % particleMesh.vertexCount : i;
                var v = geometryBasedNormals? mesh.normals[vertexIndex] : mesh.vertices[vertexIndex].normalized;

                Vector3 noise = Vector3.zero;
                if (noiseEnabled) {
                    var nv = v * noiseFrequency;
                    var nx = NoiseUtil.Fbm(Hash.Float(noiseSeed, 0u, -1000, 1000), nv.x, noiseOctaves);
                    var ny = NoiseUtil.Fbm(Hash.Float(noiseSeed, 1u, -1000, 1000), nv.y, noiseOctaves);
                    var nz = NoiseUtil.Fbm(Hash.Float(noiseSeed, 2u, -1000, 1000), nv.z, noiseOctaves);
                    noise = Vector3.Scale(new Vector3(nx, ny, nz), noiseScale) / 0.75f * noiseAmplitude;
                }

                normals[i] = (v + noise).normalized;
            }

            mesh.normals = normals;
        }
    }

    private void SaveMeshAsset(Mesh mesh, string filename) {
        Object existingAsset = AssetDatabase.LoadAssetAtPath<Object>(filename);
        if (existingAsset == null) {
            AssetDatabase.CreateAsset(mesh, filename);
        } else {
            if (existingAsset is Mesh asset) {
                asset.Clear();
            }

            EditorUtility.CopySerialized(mesh, existingAsset);
        }
    }

    private string GetFullFilename(string postfix = "") {
        string fileName = fileNamePrefix;

        if (appendMeshName && carrierMesh) {
            fileName += "-" + carrierMesh.name;
        }

        if (appendTakeNumber) {
            fileName += "-Take" + takeNumber;
        }

        if (appendTimestamp) {
            fileName += "-Time" + DateTime.Now.ToFileTime();
        }

        return $"Assets/{folderPath}/{Path.GetFileNameWithoutExtension(fileName)}{postfix}.asset";
    }

    private Vector3 GetRandomPositionWithinTriangle(Vector3 a, Vector3 b, Vector3 c) {
        var r1 = Mathf.Sqrt(Random.value);
        var r2 = Random.value;
        var m1 = 1 - r1;
        var m2 = r1 * (1 - r2);
        var m3 = r2 * r1;
        return m1 * a + m2 * b + m3 * c;
    }

    private static class NoiseUtil {
        public static float Fbm(float x, float y, uint octave) {
            var p = math.float2(x, y);
            var f = 0.0f;
            var w = 0.5f;
            for (var i = 0; i < octave; i++) {
                f += w * noise.snoise(p);
                p *= 2.0f;
                w *= 0.5f;
            }

            return f;
        }
    }

    private static class Hash {
        const uint Prime321 = 2654435761U;
        const uint Prime322 = 2246822519U;
        const uint Prime323 = 3266489917U;
        const uint Prime324 = 668265263U;
        const uint Prime325 = 374761393U;

        static uint Rotl32(uint x, int r) => (x << r) | (x >> 32 - r);

        private static uint Calculate(uint seed, uint data) {
            uint h32 = seed + Prime325;
            h32 += 4U;
            h32 += data * Prime323;
            h32 = Rotl32(h32, 17) * Prime324;
            h32 ^= h32 >> 15;
            h32 *= Prime322;
            h32 ^= h32 >> 13;
            h32 *= Prime323;
            h32 ^= h32 >> 16;
            return h32;
        }

        public static float Float(uint seed, uint data, float min, float max) =>
            Calculate(seed, data) / (float)uint.MaxValue * (max - min) + min;
    }
}
}

#endif