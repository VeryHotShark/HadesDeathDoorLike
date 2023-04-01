using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace HighlightPlus {

    public class HighlightPlusRenderPassFeature : ScriptableRendererFeature {
        class HighlightPass : ScriptableRenderPass {

            // far objects render first
            class DistanceComparer : IComparer<HighlightEffect> {

                public Vector3 camPos;

                public int Compare(HighlightEffect e1, HighlightEffect e2) {
                    Vector3 e1Pos = e1 == null ? Vector3.zero : e1.transform.position;
                    float dx1 = e1Pos.x - camPos.x;
                    float dy1 = e1Pos.y - camPos.y;
                    float dz1 = e1Pos.z - camPos.z;
                    float distE1 = dx1 * dx1 + dy1 * dy1 + dz1 * dz1;
                    Vector3 e2Pos = e2 == null ? Vector3.zero : e2.transform.position;
                    float dx2 = e2Pos.x - camPos.x;
                    float dy2 = e2Pos.y - camPos.y;
                    float dz2 = e2Pos.z - camPos.z;
                    float distE2 = dx2 * dx2 + dy2 * dy2 + dz2 * dz2;
                    if (distE1 > distE2) return -1;
                    if (distE1 < distE2) return 1;
                    return 0;
                }
            }


            public bool usesCameraOverlay = false;

            ScriptableRenderer renderer;
            RenderTextureDescriptor cameraTextureDescriptor;
            DistanceComparer effectDistanceComparer;
            static bool isVREnabled;
            bool clearStencil;
            FullScreenBlitMethod fullScreenBlitMethod = FullScreenBlit;

            public void Setup(HighlightPlusRenderPassFeature passFeature, ScriptableRenderer renderer) {
                this.renderPassEvent = passFeature.renderPassEvent;
                this.clearStencil = passFeature.clearStencil;
                this.renderer = renderer;
                if (effectDistanceComparer == null) {
                    effectDistanceComparer = new DistanceComparer();
                }
                isVREnabled = UnityEngine.XR.XRSettings.enabled && Application.isPlaying;
            }

            public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor) {
                this.cameraTextureDescriptor = cameraTextureDescriptor;
#if UNITY_2021_2_OR_NEWER
                ConfigureInput(ScriptableRenderPassInput.Depth);
#endif
            }

            public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData) {
                int count = HighlightEffect.effects.Count;
                if (count == 0) return;

                Camera cam = renderingData.cameraData.camera;
                int camLayer = 1 << cam.gameObject.layer;
                CameraType camType = cam.cameraType;

#if UNITY_2022_1_OR_NEWER
                RTHandle cameraColorTarget = renderer.cameraColorTargetHandle;
                RTHandle cameraDepthTarget = renderer.cameraDepthTargetHandle;
#else
                RenderTargetIdentifier cameraColorTarget = renderer.cameraColorTarget;
                RenderTargetIdentifier cameraDepthTarget = renderer.cameraDepthTarget;
#endif
#if !UNITY_2021_2_OR_NEWER
                // In Unity 2021.2, when MSAA > 1, cameraDepthTarget is no longer cameraColorTarget
                if (!usesCameraOverlay && (cameraTextureDescriptor.msaaSamples > 1 || cam.cameraType == CameraType.SceneView)) {
                    cameraDepthTarget = cameraColorTarget;
                }
#endif

                if (!HighlightEffect.customSorting && (Time.frameCount % 10 == 0 || !Application.isPlaying)) {
                    effectDistanceComparer.camPos = cam.transform.position;
                    HighlightEffect.effects.Sort(effectDistanceComparer);
                }

                bool clearStencil = this.clearStencil;

                for (int k = 0; k < count; k++) {
                    HighlightEffect effect = HighlightEffect.effects[k];

                    if (!(effect.ignoreObjectVisibility || effect.isVisible)) continue;

                    if (!effect.isActiveAndEnabled) continue;

                    if (camType == CameraType.Reflection && !effect.reflectionProbes) continue;

                    if ((effect.camerasLayerMask & camLayer) == 0) continue;

                    CommandBuffer cb = effect.GetCommandBuffer(cam, cameraColorTarget, cameraDepthTarget, fullScreenBlitMethod, clearStencil);
                    if (cb != null) {
                        context.ExecuteCommandBuffer(cb);
                        clearStencil = false;
                    }

                }
            }

            static Mesh _fullScreenMesh;
            static Mesh fullscreenMesh {
                get {
                    if (_fullScreenMesh != null) {
                        return _fullScreenMesh;
                    }
                    float num = 1f;
                    float num2 = 0f;
                    Mesh val = new Mesh();
                    _fullScreenMesh = val;
                    _fullScreenMesh.SetVertices(new List<Vector3> {
            new Vector3 (-1f, -1f, 0f),
            new Vector3 (-1f, 1f, 0f),
            new Vector3 (1f, -1f, 0f),
            new Vector3 (1f, 1f, 0f)
        });
                    _fullScreenMesh.SetUVs(0, new List<Vector2> {
            new Vector2 (0f, num2),
            new Vector2 (0f, num),
            new Vector2 (1f, num2),
            new Vector2 (1f, num)
        });
                    _fullScreenMesh.SetIndices(new int[6] { 0, 1, 2, 2, 1, 3 }, (MeshTopology)0, 0, false);
                    _fullScreenMesh.UploadMeshData(true);
                    return _fullScreenMesh;
                }
            }

            static Matrix4x4 matrix4x4Identity = Matrix4x4.identity;
            static void FullScreenBlit(CommandBuffer cmd, RenderTargetIdentifier source, RenderTargetIdentifier destination, Material material, int passIndex) {
                destination = new RenderTargetIdentifier(destination, 0, CubemapFace.Unknown, -1);
                cmd.SetRenderTarget(destination);
                cmd.SetGlobalTexture(ShaderParams.MainTex, source);
                cmd.SetGlobalFloat(ShaderParams.AspectRatio, isVREnabled ? 0.5f : 1);
                cmd.DrawMesh(fullscreenMesh, matrix4x4Identity, material, 0, passIndex);
            }

            public override void FrameCleanup(CommandBuffer cmd) {
            }
        }

        HighlightPass renderPass;
        public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
        [Tooltip("Clears stencil buffer before rendering highlight effects. This option can solve compatibility issues with shaders that also use stencil buffers.")]
        public bool clearStencil;
        public static bool installed;


        void OnDisable() {
            installed = false;
        }

        public override void Create() {
            renderPass = new HighlightPass();
        }

        // This method is called when setting up the renderer once per-camera.
        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData) {
#if UNITY_2019_4_OR_NEWER
            if (renderingData.cameraData.renderType == CameraRenderType.Base) {
                Camera cam = renderingData.cameraData.camera;
                renderPass.usesCameraOverlay = cam.GetUniversalAdditionalCameraData().cameraStack.Count > 0;
            }
#endif
            renderPass.Setup(this, renderer);
            renderer.EnqueuePass(renderPass);
            installed = true;
        }
    }

}
