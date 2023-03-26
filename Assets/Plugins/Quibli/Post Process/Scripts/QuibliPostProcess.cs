using System;
using System.Collections.Generic;

// TODO: Remove for URP 13.
// https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@13.1/manual/upgrade-guide-2022-1.html
#pragma warning disable CS0618

namespace UnityEngine.Rendering.Universal.PostProcessing {
/// <summary>
/// This render feature is responsible for:
/// <list type="bullet">
/// <item>
/// <description> Injecting the render passes for custom post processing </description>
/// </item>
/// <item>
/// <description> Ordering and categorizing the custom post processing renderers </description>
/// </item>
/// </list>
/// </summary>
public class QuibliPostProcess : ScriptableRendererFeature {
    /// <summary>
    /// The settings for the custom post processing render feature.
    /// </summary>
    [Serializable]
    public class Settings {
        [SerializeField]
        public List<string> renderersAfterOpaqueAndSky;

        [SerializeField]
        public List<string> renderersBeforePostProcess;

        [SerializeField]
        public List<string> renderersAfterPostProcess;

        public Settings() {
            renderersAfterOpaqueAndSky = new List<string>();
            renderersBeforePostProcess = new List<string>();
            renderersAfterPostProcess = new List<string>();
        }
    }

    /// <summary>
    /// The settings of the render feature.
    /// </summary>
    [SerializeField]
    public Settings settings = new Settings();

    private CompoundPass _afterOpaqueAndSky;
    private CompoundPass _beforePostProcess;
    private CompoundPass _afterPostProcess;

    /// <summary>
    /// A handle to the "_AfterPostProcessTexture" used as the target for the builtin post
    /// processing pass in the last camera in the camera stack.
    /// </summary>
    private RenderTargetHandle _afterPostProcessColor;

    /// <summary>
    /// Injects the custom post-processing render passes.
    /// </summary>
    /// <param name="renderer">The renderer</param>
    /// <param name="renderingData">Rendering state. Use this to setup render passes.</param>
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData) {
        // Only inject passes if post processing is enabled
        if (renderingData.cameraData.postProcessEnabled) {
            // For each pass, only inject if there is at least one custom post-processing renderer class in it.
            if (_afterOpaqueAndSky.HasPostProcessRenderers && _afterOpaqueAndSky.PrepareRenderers(in renderingData)) {
                renderer.EnqueuePass(_afterOpaqueAndSky);
            }

            if (_beforePostProcess.HasPostProcessRenderers && _beforePostProcess.PrepareRenderers(in renderingData)) {
                renderer.EnqueuePass(_beforePostProcess);
            }

            if (_afterPostProcess.HasPostProcessRenderers && _afterPostProcess.PrepareRenderers(in renderingData)) {
                renderer.EnqueuePass(_afterPostProcess);
            }

#if !UNITY_2022_1_OR_NEWER
            SetupRenderPassesCore(renderer, in renderingData);
#endif
        }
    }

#if UNITY_2022_1_OR_NEWER
    public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData) {
        SetupRenderPassesCore(renderer, renderingData);
    }
#endif

    void SetupRenderPassesCore(ScriptableRenderer renderer, in RenderingData renderingData) {
#if UNITY_2022_1_OR_NEWER
        var cameraTarget = renderer.cameraColorTargetHandle;
#else
        var cameraTarget = renderer.cameraColorTarget;
#endif
        // For each pass, only inject if there is at least one custom post-processing renderer class in it.
        if (_afterOpaqueAndSky.HasPostProcessRenderers && _afterOpaqueAndSky.PrepareRenderers(in renderingData)) {
            _afterOpaqueAndSky.Setup(cameraTarget, cameraTarget);
        }

        if (_beforePostProcess.HasPostProcessRenderers && _beforePostProcess.PrepareRenderers(in renderingData)) {
            _beforePostProcess.Setup(cameraTarget, cameraTarget);
        }

        if (_afterPostProcess.HasPostProcessRenderers && _afterPostProcess.PrepareRenderers(in renderingData)) {
#if UNITY_2021_2_OR_NEWER
            _afterPostProcess.Setup(cameraTarget, cameraTarget);
#else
            // If this camera resolve to the final target, then both the source and
            // destination will be "_AfterPostProcessTexture"
            // (Note: a final blit/post pass is added by the renderer).
            var source = renderingData.cameraData.resolveFinalTarget
                ? _afterPostProcessColor.Identifier()
                : renderer.cameraColorTarget;
            _afterPostProcess.Setup(source, source);
#endif
        }
    }


    /// <summary>
    /// Initializes the custom post-processing render passes.
    /// </summary>
    public override void Create() {
        // This is copied from the forward renderer.
        _afterPostProcessColor.Init("_AfterPostProcessTexture");
        // Create the three render passes and send the custom post-processing renderer classes to each.
        Dictionary<string, CompoundRenderer> shared = new Dictionary<string, CompoundRenderer>();
        _afterOpaqueAndSky = new CompoundPass(InjectionPoint.AfterOpaqueAndSky,
                                              InstantiateRenderers(settings.renderersAfterOpaqueAndSky, shared));
        _beforePostProcess = new CompoundPass(InjectionPoint.BeforePostProcess,
                                              InstantiateRenderers(settings.renderersBeforePostProcess, shared));
        _afterPostProcess = new CompoundPass(InjectionPoint.AfterPostProcess,
                                             InstantiateRenderers(settings.renderersAfterPostProcess, shared));
    }

    /// <summary>
    /// Converts the class name (AssemblyQualifiedName) to an instance. Filters out types that
    /// don't exist or don't match the requirements.
    /// </summary>
    /// <param name="names">The list of assembly-qualified class names</param>
    /// <param name="shared">Dictionary of shared instances keyed by class name</param>
    /// <returns>List of renderers</returns>
    private List<CompoundRenderer> InstantiateRenderers(List<String> names,
                                                        Dictionary<string, CompoundRenderer> shared) {
        var renderers = new List<CompoundRenderer>(names.Count);
        foreach (var n in names) {
            if (shared.TryGetValue(n, out var renderer)) {
                renderers.Add(renderer);
            } else {
                var type = Type.GetType(n);
                if (type == null || !type.IsSubclassOf(typeof(CompoundRenderer))) continue;
                var attribute = CompoundRendererFeatureAttribute.GetAttribute(type);
                if (attribute == null) continue;

                renderer = Activator.CreateInstance(type) as CompoundRenderer;
                renderers.Add(renderer);

                if (attribute.ShareInstance) shared.Add(n, renderer);
            }
        }

        return renderers;
    }
}
}
