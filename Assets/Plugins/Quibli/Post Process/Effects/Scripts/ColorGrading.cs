using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering.Universal.PostProcessing;

namespace CompoundRendererFeature.PostProcess {
[Serializable, VolumeComponentMenu("Quibli/Stylized Color Grading")]
public class ColorGrading : VolumeComponent {
    [Tooltip("Controls the amount to which image colors are modified.")]
    public ClampedFloatParameter intensity = new ClampedFloatParameter(0f, 0f, 1f, true);

    [Space]
    public ClampedFloatParameter blueShadows = new ClampedFloatParameter(0f, 0f, 1f, true);

    public ClampedFloatParameter greenShadows = new ClampedFloatParameter(0f, 0f, 1f, true);
    public ClampedFloatParameter redHighlights = new ClampedFloatParameter(0f, 0f, 1f, true);
    public ClampedFloatParameter contrast = new ClampedFloatParameter(0f, 0f, 1f, true);

    [Space]
    public ClampedFloatParameter vibrance = new ClampedFloatParameter(0f, 0f, 1f, true);

    public ClampedFloatParameter saturation = new ClampedFloatParameter(0f, 0f, 1f, true);
}

[CompoundRendererFeature("Stylized Color Grading", InjectionPoint.BeforePostProcess)]
public class ColorGradingRenderer : CompoundRenderer {
    private ColorGrading _volumeComponent;

    private Material _effectMaterial;

    static class PropertyIDs {
        internal static readonly int Input = Shader.PropertyToID("_MainTex");
        internal static readonly int Intensity = Shader.PropertyToID("_Intensity");
        internal static readonly int ShadowBezierPoints = Shader.PropertyToID("_ShadowBezierPoints");
        internal static readonly int HighlightBezierPoints = Shader.PropertyToID("_HighlightBezierPoints");
        internal static readonly int Contrast = Shader.PropertyToID("_Contrast");
        internal static readonly int Vibrance = Shader.PropertyToID("_Vibrance");
        internal static readonly int Saturation = Shader.PropertyToID("_Saturation");
    }

    public override bool visibleInSceneView => false;

    public override ScriptableRenderPassInput input =>
        ScriptableRenderPassInput.Color | ScriptableRenderPassInput.Depth;

    // Called only once before the first render call.
    public override void Initialize() {
        base.Initialize();
        _effectMaterial = CoreUtils.CreateEngineMaterial("Hidden/CompoundRendererFeature/ColorGrading");
    }

    // Called for each camera/injection point pair on each frame.
    // Return true if the effect should be rendered for this camera.
    public override bool Setup(in RenderingData renderingData, InjectionPoint injectionPoint) {
        base.Setup(in renderingData, injectionPoint);
        var stack = VolumeManager.instance.stack;
        _volumeComponent = stack.GetComponent<ColorGrading>();
        bool shouldRenderEffect = _volumeComponent.intensity.value > 0;
        return shouldRenderEffect;
    }

    public override void Render(CommandBuffer cmd, RenderTargetIdentifier source, RenderTargetIdentifier destination,
                                ref RenderingData renderingData, InjectionPoint injectionPoint) {
        RenderTextureDescriptor descriptor = GetTempRTDescriptor(renderingData);

        _effectMaterial.SetFloat(PropertyIDs.Intensity, _volumeComponent.intensity.value);
        _effectMaterial.SetVector(PropertyIDs.ShadowBezierPoints,
                                  new Vector4(_volumeComponent.blueShadows.value, _volumeComponent.greenShadows.value));
        _effectMaterial.SetVector(PropertyIDs.HighlightBezierPoints,
                                  new Vector4(_volumeComponent.redHighlights.value, 0, 0, 0));
        _effectMaterial.SetFloat(PropertyIDs.Contrast, _volumeComponent.contrast.value);
        _effectMaterial.SetFloat(PropertyIDs.Vibrance, _volumeComponent.vibrance.value * 0.5f);
        _effectMaterial.SetFloat(PropertyIDs.Saturation, _volumeComponent.saturation.value * 0.5f);

        SetSourceSize(cmd, descriptor);

        cmd.SetGlobalTexture(PropertyIDs.Input, source);
        CoreUtils.DrawFullScreen(cmd, _effectMaterial, destination);
    }

    public override void Dispose(bool disposing) { }
}
}