using System;
using UnityEngine.Experimental.Rendering;

// TODO: Remove for URP 13.
// https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@13.1/manual/upgrade-guide-2022-1.html
#pragma warning disable CS0618

namespace UnityEngine.Rendering.Universal.PostProcessing {
/// <summary>
/// Custom Post Processing injection points.
/// Since this is a flag, you can write a renderer that can be injected at multiple locations.
/// </summary>
[Flags]
public enum InjectionPoint {
    /// <summary>After Opaque and Sky.</summary>
    AfterOpaqueAndSky = 1,

    /// <summary>Before Post Processing.</summary>
    BeforePostProcess = 2,

    /// <summary>After Post Processing.</summary>
    AfterPostProcess = 4,
}

/// <summary>
/// The Base Class for all the custom post process renderers
/// </summary>
public abstract class CompoundRenderer : IDisposable {
    private bool _initialized = false;
    protected GraphicsFormat _defaultHDRFormat;
    protected bool _useRGBM;

    /// <summary>
    /// True if you want your custom post process to be visible in the scene view. False otherwise.
    /// </summary>
    public virtual bool visibleInSceneView => true;

    /// <summary>
    /// Specifies the input needed by this custom post process. Default is Color only.
    /// </summary>
    public virtual ScriptableRenderPassInput input => ScriptableRenderPassInput.Color;

    /// <summary>
    /// Whether the function initialize has already been called
    /// </summary>
    public bool Initialized => _initialized;

    /// <summary>
    /// An intialize function for internal use only
    /// </summary>
    internal void InitializeInternal() {
        Initialize();
        _initialized = true;
    }

    /// <summary>
    /// Initialize function, called once before the effect is first rendered.
    /// If the effect is never rendered, then this function will never be called.
    /// </summary>
    public virtual void Initialize() {
        // Texture format pre-lookup
        if (SystemInfo.IsFormatSupported(GraphicsFormat.B10G11R11_UFloatPack32,
                                         FormatUsage.Linear | FormatUsage.Render)) {
            _defaultHDRFormat = GraphicsFormat.B10G11R11_UFloatPack32;
            _useRGBM = false;
        } else {
            _defaultHDRFormat = QualitySettings.activeColorSpace == ColorSpace.Linear
                ? GraphicsFormat.R8G8B8A8_SRGB
                : GraphicsFormat.R8G8B8A8_UNorm;
            _useRGBM = true;
        }
    }


    /// <summary>
    /// Setup function, called every frame once for each camera before render is called.
    /// </summary>
    /// <param name="renderingData">Current Rendering Data</param>
    /// <param name="injectionPoint">The injection point from which the renderer is being called</param>
    /// <returns>
    /// True if render should be called for this camera. False Otherwise.
    /// </returns>
    public virtual bool Setup(in RenderingData renderingData, InjectionPoint injectionPoint) {
        return true;
    }

    /// <summary>
    /// Called every frame for each camera when the post process needs to be rendered.
    /// </summary>
    /// <param name="cmd">Command Buffer used to issue your commands</param>
    /// <param name="source">Source Render Target, it contains the camera color buffer in it's current state</param>
    /// <param name="destination">Destination Render Target</param>
    /// <param name="renderingData">Current Rendering Data</param>
    /// <param name="injectionPoint">The injection point from which the renderer is being called</param>
    public abstract void Render(CommandBuffer cmd, RenderTargetIdentifier source, RenderTargetIdentifier destination,
                                ref RenderingData renderingData, InjectionPoint injectionPoint);

    /// <summary>
    /// Dispose function, called when the renderer is disposed.
    /// </summary>
    /// <param name="disposing"> If true, dispose of managed objects </param>
    public virtual void Dispose(bool disposing) { }

    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Create a descriptor for intermediate render targets based on the rendering data.
    /// Mainly used to create intermediate render targets.
    /// </summary>
    /// <returns>a descriptor similar to the camera target but with no depth buffer or multisampling</returns>
    public static RenderTextureDescriptor GetTempRTDescriptor(in RenderingData renderingData) {
        RenderTextureDescriptor descriptor = renderingData.cameraData.cameraTargetDescriptor;
        descriptor.depthBufferBits = 0;
        descriptor.msaaSamples = 1;
        return descriptor;
    }

    public static RenderTextureDescriptor GetTempRTDescriptor(in RenderingData renderingData, int width, int height,
                                                              GraphicsFormat format) {
        if (width <= 0 || height <= 0) {
            Debug.LogError($"Invalid parameters for GetTempRTDescriptor: {width}, {height}.");
        }

        RenderTextureDescriptor descriptor = GetTempRTDescriptor(renderingData);
        // descriptor.graphicsFormat = format;
        descriptor.width = width;
        descriptor.height = height;
        return descriptor;
    }

    public static void SetSourceSize(CommandBuffer cmd, RenderTextureDescriptor desc) {
        float width = desc.width;
        float height = desc.height;
        if (desc.useDynamicScale) {
            width *= ScalableBufferManager.widthScaleFactor;
            height *= ScalableBufferManager.heightScaleFactor;
        }

        cmd.SetGlobalVector(ShaderConstants._SourceSize, new Vector4(width, height, 1.0f / width, 1.0f / height));
    }

    // Precomputed shader ids to same some CPU cycles (mostly affects mobile)
    static class ShaderConstants {
        public static readonly int _SourceSize = Shader.PropertyToID("_SourceSize");
    }
}

/// <summary>
/// Use this attribute to mark classes that can be used as a custom post-processing renderer
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class CompoundRendererFeatureAttribute : Attribute {
    // Name of the effect in the custom post-processing render feature editor
    readonly string name;

    // In which render pass this effect should be injected
    readonly InjectionPoint injectionPoint;

    // In case the renderer is added to multiple injection points,
    // If shareInstance = true, one instance of the renderer will be constructed and shared between the injection points.
    // Otherwise, a different instance will be  constructed for every injection point.
    readonly bool shareInstance;

    /// <value> Name of the effect in the custom post-processing render feature editor </value>
    public string Name => name;

    /// <value> In which render pass this effect should be injected </value>
    public InjectionPoint InjectionPoint => injectionPoint;

    /// <value>
    /// In case the renderer is added to multiple injection points,
    /// If shareInstance = true, one instance of the renderer will be constructed and shared between the injection points.
    /// Otherwise, a different instance will be  constructed for every injection point.
    /// </value>
    public bool ShareInstance => shareInstance;

    /// <summary>
    /// Marks this class as a custom post processing renderer
    /// </summary>
    /// <param name="name"> Name of the effect in the custom post-processing render feature editor </param>
    /// <param name="injectionPoint"> In which render pass this effect should be injected </param>
    public CompoundRendererFeatureAttribute(string name, InjectionPoint injectionPoint, bool shareInstance = false) {
        this.name = name;
        this.injectionPoint = injectionPoint;
        this.shareInstance = shareInstance;
    }

    /// <summary>
    /// Get the CompoundRendererFeatureAttribute attached to the type.
    /// </summary>
    /// <param name="type">the type on which the attribute is attached</param>
    /// <returns>the attached CompoundRendererFeatureAttribute or null if none were attached</returns>
    public static CompoundRendererFeatureAttribute GetAttribute(Type type) {
        if (type == null) return null;
        var attributes = type.GetCustomAttributes(typeof(CompoundRendererFeatureAttribute), false);
        return (attributes.Length != 0) ? (attributes[0] as CompoundRendererFeatureAttribute) : null;
    }
}
}
