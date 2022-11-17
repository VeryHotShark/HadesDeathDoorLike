Shader "SeeThroughShader/HDRP/2019/Lit"
{
   Properties
   {
      
// SEE: https://github.com/Unity-Technologies/Graphics/blob/4b98cece5623e02f03c9ff311bca0f749ba4fd94/Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.shader
        [MainColor] _BaseColor("BaseColor", Color) = (1,1,1,1)
        [MainTexture] _BaseColorMap("BaseColorMap", 2D) = "white" {}
        [HideInInspector] _BaseColorMap_MipInfo("_BaseColorMap_MipInfo", Vector) = (0, 0, 0, 0)

        _Metallic("_Metallic", Range(0.0, 1.0)) = 0
        _Smoothness("Smoothness", Range(0.0, 1.0)) = 0.5
        _MaskMap("MaskMap", 2D) = "white" {}
        _MetallicRemapMin("MetallicRemapMin", Float) = 0.0
        _MetallicRemapMax("MetallicRemapMax", Float) = 1.0
        _SmoothnessRemapMin("SmoothnessRemapMin", Float) = 0.0
        _SmoothnessRemapMax("SmoothnessRemapMax", Float) = 1.0
        _AlphaRemapMin("AlphaRemapMin", Float) = 0.0
        _AlphaRemapMax("AlphaRemapMax", Float) = 1.0
        _AORemapMin("AORemapMin", Float) = 0.0
        _AORemapMax("AORemapMax", Float) = 1.0

        _NormalMap("NormalMap", 2D) = "bump" {}     // Tangent space normal map
        _NormalMapOS("NormalMapOS", 2D) = "white" {} // Object space normal map - no good default value
        _NormalScale("_NormalScale", Range(0.0, 8.0)) = 1

        _BentNormalMap("_BentNormalMap", 2D) = "bump" {}
        _BentNormalMapOS("_BentNormalMapOS", 2D) = "white" {}

        _HeightMap("HeightMap", 2D) = "black" {}
        // Caution: Default value of _HeightAmplitude must be (_HeightMax - _HeightMin) * 0.01
        // Those two properties are computed from the ones exposed in the UI and depends on the displaement mode so they are separate because we don't want to lose information upon displacement mode change.
        [HideInInspector] _HeightAmplitude("Height Amplitude", Float) = 0.02 // In world units. This will be computed in the UI.
        [HideInInspector] _HeightCenter("Height Center", Range(0.0, 1.0)) = 0.5 // In texture space

        [Enum(MinMax, 0, Amplitude, 1)] _HeightMapParametrization("Heightmap Parametrization", Int) = 0
        // These parameters are for vertex displacement/Tessellation
        _HeightOffset("Height Offset", Float) = 0
        // MinMax mode
        _HeightMin("Heightmap Min", Float) = -1
        _HeightMax("Heightmap Max", Float) = 1
        // Amplitude mode
        _HeightTessAmplitude("Amplitude", Float) = 2.0 // in Centimeters
        _HeightTessCenter("Height Center", Range(0.0, 1.0)) = 0.5 // In texture space

        // These parameters are for pixel displacement
        _HeightPoMAmplitude("Height Amplitude", Float) = 2.0 // In centimeters

        _DetailMap("DetailMap", 2D) = "linearGrey" {}
        _DetailAlbedoScale("_DetailAlbedoScale", Range(0.0, 2.0)) = 1
        _DetailNormalScale("_DetailNormalScale", Range(0.0, 2.0)) = 1
        _DetailSmoothnessScale("_DetailSmoothnessScale", Range(0.0, 2.0)) = 1

        _TangentMap("TangentMap", 2D) = "bump" {}
        _TangentMapOS("TangentMapOS", 2D) = "white" {}
        _Anisotropy("Anisotropy", Range(-1.0, 1.0)) = 0
        _AnisotropyMap("AnisotropyMap", 2D) = "white" {}

        _SubsurfaceMask("Subsurface Radius", Range(0.0, 1.0)) = 1.0
        _SubsurfaceMaskMap("Subsurface Radius Map", 2D) = "white" {}
        _TransmissionMask("Transmission Mask", Range(0.0, 1.0)) = 1.0
        _TransmissionMaskMap("Transmission Mask Map", 2D) = "white" {}
        _Thickness("Thickness", Float) = 1.0
        _ThicknessMap("Thickness Map", 2D) = "white" {}
        _ThicknessRemap("Thickness Remap", Vector) = (0, 1, 0, 0)

        _IridescenceThickness("Iridescence Thickness", Range(0.0, 1.0)) = 1.0
        _IridescenceThicknessMap("Iridescence Thickness Map", 2D) = "white" {}
        _IridescenceThicknessRemap("Iridescence Thickness Remap", Vector) = (0, 1, 0, 0)
        _IridescenceMask("Iridescence Mask", Range(0.0, 1.0)) = 1.0
        _IridescenceMaskMap("Iridescence Mask Map", 2D) = "white" {}

        _CoatMask("Coat Mask", Range(0.0, 1.0)) = 0.0
        _CoatMaskMap("CoatMaskMap", 2D) = "white" {}

        [ToggleUI] _EnergyConservingSpecularColor("_EnergyConservingSpecularColor", Float) = 1.0
        _SpecularColor("SpecularColor", Color) = (1, 1, 1, 1)
        _SpecularColorMap("SpecularColorMap", 2D) = "white" {}

        // Following options are for the GUI inspector and different from the input parameters above
        // These option below will cause different compilation flag.
        [Enum(Off, 0, From Ambient Occlusion, 1, From AO and Bent Normals, 2)]  _SpecularOcclusionMode("Specular Occlusion Mode", Int) = 1

        [HDR] _EmissiveColor("EmissiveColor", Color) = (0, 0, 0)
        // Used only to serialize the LDR and HDR emissive color in the material UI,
        // in the shader only the _EmissiveColor should be used
        [HideInInspector] _EmissiveColorLDR("EmissiveColor LDR", Color) = (0, 0, 0)
        _EmissiveColorMap("EmissiveColorMap", 2D) = "white" {}
        [ToggleUI] _AlbedoAffectEmissive("Albedo Affect Emissive", Float) = 0.0
        _EmissiveIntensityUnit("Emissive Mode", Int) = 0
        [ToggleUI] _UseEmissiveIntensity("Use Emissive Intensity", Int) = 0
        _EmissiveIntensity("Emissive Intensity", Float) = 1
        _EmissiveExposureWeight("Emissive Pre Exposure", Range(0.0, 1.0)) = 1.0

        ////[ToggleUI]  _UseShadowThreshold("_UseShadowThreshold", Float) = 0.0
        ////[ToggleUI]  _AlphaCutoffEnable("Alpha Cutoff Enable", Float) = 0.0
        _AlphaCutoff("Alpha Cutoff", Range(0.0, 1.0)) = 0.5
        _AlphaCutoffShadow("_AlphaCutoffShadow", Range(0.0, 1.0)) = 0.5
        _AlphaCutoffPrepass("_AlphaCutoffPrepass", Range(0.0, 1.0)) = 0.5
        _AlphaCutoffPostpass("_AlphaCutoffPostpass", Range(0.0, 1.0)) = 0.5
        ////[ToggleUI] _TransparentDepthPrepassEnable("_TransparentDepthPrepassEnable", Float) = 0.0
        ////[ToggleUI] _TransparentBackfaceEnable("_TransparentBackfaceEnable", Float) = 0.0
        ////[ToggleUI] _TransparentDepthPostpassEnable("_TransparentDepthPostpassEnable", Float) = 0.0
        ////_TransparentSortPriority("_TransparentSortPriority", Float) = 0

        // Transparency
        ////[Enum(None, 0, Planar, 1, Sphere, 2, Thin, 3)]_RefractionModel("Refraction Model", Int) = 0
        _Ior("Index Of Refraction", Range(1.0, 2.5)) = 1.5
        _TransmittanceColor("Transmittance Color", Color) = (1.0, 1.0, 1.0)
        _TransmittanceColorMap("TransmittanceColorMap", 2D) = "white" {}
        _ATDistance("Transmittance Absorption Distance", Float) = 1.0
        ////[ToggleUI] _TransparentWritingMotionVec("_TransparentWritingMotionVec", Float) = 0.0

        // Stencil state

        // Forward
        ////[HideInInspector] _StencilRef("_StencilRef", Int) = 0 // StencilUsage.Clear
        ////[HideInInspector] _StencilWriteMask("_StencilWriteMask", Int) = 3 // StencilUsage.RequiresDeferredLighting | StencilUsage.SubsurfaceScattering
        // GBuffer
        ////[HideInInspector] _StencilRefGBuffer("_StencilRefGBuffer", Int) = 2 // StencilUsage.RequiresDeferredLighting
        ////[HideInInspector] _StencilWriteMaskGBuffer("_StencilWriteMaskGBuffer", Int) = 3 // StencilUsage.RequiresDeferredLighting | StencilUsage.SubsurfaceScattering
        // Depth prepass
        ////[HideInInspector] _StencilRefDepth("_StencilRefDepth", Int) = 0 // Nothing
        ////[HideInInspector] _StencilWriteMaskDepth("_StencilWriteMaskDepth", Int) = 8 // StencilUsage.TraceReflectionRay
        // Motion vector pass
        ////[HideInInspector] _StencilRefMV("_StencilRefMV", Int) = 32 // StencilUsage.ObjectMotionVector
        ////[HideInInspector] _StencilWriteMaskMV("_StencilWriteMaskMV", Int) = 32 // StencilUsage.ObjectMotionVector

        // Blending state
        ////_SurfaceType("__surfacetype", Float) = 0.0
        ////_BlendMode("__blendmode", Float) = 0.0
        ////[HideInInspector] _SrcBlend("__src", Float) = 1.0
        ////[HideInInspector] _DstBlend("__dst", Float) = 0.0
        ////[HideInInspector] _AlphaSrcBlend("__alphaSrc", Float) = 1.0
        ////[HideInInspector] _AlphaDstBlend("__alphaDst", Float) = 0.0
        ////[HideInInspector][ToggleUI] _ZWrite("__zw", Float) = 1.0
        ////[HideInInspector][ToggleUI] _TransparentZWrite("_TransparentZWrite", Float) = 0.0
        [HideInInspector] _CullMode("__cullmode", Float) = 2.0
        [HideInInspector] _CullModeForward("__cullmodeForward", Float) = 2.0 // This mode is dedicated to Forward to correctly handle backface then front face rendering thin transparent
        [Enum(UnityEditor.Rendering.HighDefinition.TransparentCullMode)] _TransparentCullMode("_TransparentCullMode", Int) = 2 // Back culling by default
        //[Enum(UnityEditor.Rendering.HighDefinition.OpaqueCullMode)] _OpaqueCullMode("_OpaqueCullMode", Int) = 2 // Back culling by default

        [Enum(OpaqueCullMode)] _OpaqueCullMode("_OpaqueCullMode", Int) = 2 // Back culling by default

        ////[HideInInspector] _ZTestDepthEqualForOpaque("_ZTestDepthEqualForOpaque", Int) = 4 // Less equal
        ////[HideInInspector] _ZTestGBuffer("_ZTestGBuffer", Int) = 4
        ////[Enum(UnityEngine.Rendering.CompareFunction)] _ZTestTransparent("Transparent ZTest", Int) = 4 // Less equal

        ////[ToggleUI] _EnableFogOnTransparent("Enable Fog", Float) = 1.0
        ////[ToggleUI] _EnableBlendModePreserveSpecularLighting("Enable Blend Mode Preserve Specular Lighting", Float) = 1.0

        [ToggleUI] _DoubleSidedEnable("Double sided enable", Float) = 0.0
        [Enum(Flip, 0, Mirror, 1, None, 2)] _DoubleSidedNormalMode("Double sided normal mode", Float) = 1
        [HideInInspector] _DoubleSidedConstants("_DoubleSidedConstants", Vector) = (1, 1, -1, 0)
        [Enum(Auto, 0, On, 1, Off, 2)] _DoubleSidedGIMode("Double sided GI mode", Float) = 0

        [Enum(UV0, 0, UV1, 1, UV2, 2, UV3, 3, Planar, 4, Triplanar, 5)] _UVBase("UV Set for base", Float) = 0
        [Enum(WorldSpace, 0, ObjectSpace, 1)] _ObjectSpaceUVMapping("Mapping space", Float) = 0.0
        _TexWorldScale("Scale to apply on world coordinate", Float) = 1.0
        [HideInInspector] _InvTilingScale("Inverse tiling scale = 2 / (abs(_BaseColorMap_ST.x) + abs(_BaseColorMap_ST.y))", Float) = 1
        [HideInInspector] _UVMappingMask("_UVMappingMask", Color) = (1, 0, 0, 0)
        [Enum(TangentSpace, 0, ObjectSpace, 1)] _NormalMapSpace("NormalMap space", Float) = 0

        // Following enum should be material feature flags (i.e bitfield), however due to Gbuffer encoding constrain many combination exclude each other
        // so we use this enum as "material ID" which can be interpreted as preset of bitfield of material feature
        // The only material feature flag that can be added in all cases is clear coat
        [Enum(Subsurface Scattering, 0, Standard, 1, Anisotropy, 2, Iridescence, 3, Specular Color, 4, Translucent, 5)] _MaterialID("MaterialId", Int) = 1 // MaterialId.Standard
        [ToggleUI] _TransmissionEnable("_TransmissionEnable", Float) = 1.0

        _DisplacementMode("DisplacementMode", Int) = 0
        [ToggleUI] _DisplacementLockObjectScale("displacement lock object scale", Float) = 1.0
        [ToggleUI] _DisplacementLockTilingScale("displacement lock tiling scale", Float) = 1.0
        ////[ToggleUI] _DepthOffsetEnable("Depth Offset View space", Float) = 0.0

        [ToggleUI] _EnableGeometricSpecularAA("EnableGeometricSpecularAA", Float) = 0.0
        _SpecularAAScreenSpaceVariance("SpecularAAScreenSpaceVariance", Range(0.0, 1.0)) = 0.1
        _SpecularAAThreshold("SpecularAAThreshold", Range(0.0, 1.0)) = 0.2

        _PPDMinSamples("Min sample for POM", Range(1.0, 64.0)) = 5
        _PPDMaxSamples("Max sample for POM", Range(1.0, 64.0)) = 15
        _PPDLodThreshold("Start lod to fade out the POM effect", Range(0.0, 16.0)) = 5
        _PPDPrimitiveLength("Primitive length for POM", Float) = 1
        _PPDPrimitiveWidth("Primitive width for POM", Float) = 1
        [HideInInspector] _InvPrimScale("Inverse primitive scale for non-planar POM", Vector) = (1, 1, 0, 0)

        [Enum(UV0, 0, UV1, 1, UV2, 2, UV3, 3)] _UVDetail("UV Set for detail", Float) = 0
        [HideInInspector] _UVDetailsMappingMask("_UVDetailsMappingMask", Color) = (1, 0, 0, 0)
        [ToggleUI] _LinkDetailsWithBase("LinkDetailsWithBase", Float) = 1.0

        [Enum(Use Emissive Color, 0, Use Emissive Mask, 1)] _EmissiveColorMode("Emissive color mode", Float) = 1
        [Enum(UV0, 0, UV1, 1, UV2, 2, UV3, 3, Planar, 4, Triplanar, 5, Same as Base, 6)] _UVEmissive("UV Set for emissive", Float) = 0
        [Enum(WorldSpace, 0, ObjectSpace, 1)] _ObjectSpaceUVMappingEmissive("Mapping space", Float) = 0.0
        _TexWorldScaleEmissive("Scale to apply on world coordinate", Float) = 1.0
        [HideInInspector] _UVMappingMaskEmissive("_UVMappingMaskEmissive", Color) = (1, 0, 0, 0)

        // Caution: C# code in BaseLitUI.cs call LightmapEmissionFlagsProperty() which assume that there is an existing "_EmissionColor"
        // value that exist to identify if the GI emission need to be enabled.
        // In our case we don't use such a mechanism but need to keep the code quiet. We declare the value and always enable it.
        // TODO: Fix the code in legacy unity so we can customize the beahvior for GI
        _EmissionColor("Color", Color) = (1, 1, 1)

        // HACK: GI Baking system relies on some properties existing in the shader ("_MainTex", "_Cutoff" and "_Color") for opacity handling, so we need to store our version of those parameters in the hard-coded name the GI baking system recognizes.
        [HideInInspector] _MainTex("Albedo", 2D) = "white" {}
        [HideInInspector] _Color("Color", Color) = (1,1,1,1)
        [HideInInspector] _Cutoff("Alpha Cutoff", Range(0.0, 1.0)) = 0.5

        ////[ToggleUI] _SupportDecals("Support Decals", Float) = 1.0
        ////[ToggleUI] _ReceivesSSR("Receives SSR", Float) = 1.0
        ////[ToggleUI] _ReceivesSSRTransparent("Receives SSR Transparent", Float) = 0.0
        ////[ToggleUI] _AddPrecomputedVelocity("AddPrecomputedVelocity", Float) = 0.0

        // Ray Tracing
        ////[ToggleUI] _RayTracing("Ray Tracing (Preview)", Float) = 0

        [HideInInspector] _DiffusionProfile("Obsolete, kept for migration purpose", Int) = 0
        [HideInInspector] _DiffusionProfileAsset("Diffusion Profile Asset", Vector) = (0, 0, 0, 0)
        [HideInInspector] _DiffusionProfileHash("Diffusion Profile Hash", Float) = 0

        ////[HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        ////[HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
        ////[HideInInspector][NoScaleOffset]unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}



	_DissolveColor("Dissolve Color", Color) = (1,1,1,1)
	_DissolveColorSaturation("Dissolve Color Saturation", Range(0,1)) = 1.0
	_DissolveEmission("Dissolve Emission", Range(0,1)) = 1.0
	[AbsoluteValue()] _DissolveEmissionBooster("Dissolve Emission Booster", float) = 1
	_DissolveTex("Dissolve Effect Texture", 2D) = "white" {}

	[Enum(STSInteractionMode)] _InteractionMode ("Interaction Mode", Float) = 0
	[Enum(ObstructionMode)] _Obstruction ("Obstruction Mode", Float) = 0
	_AngleStrength("Angle Obstruction Strength", Range(0,1)) = 1.0
        
	_ConeStrength ("Cone Obstruction Strength", Range(0,1)) = 1.0
	_ConeObstructionDestroyRadius ("Cone Obstruction Destroy Radius", float) = 10.0

        
	_CylinderStrength ("Cylinder Obstruction Strength", Range(0,1)) = 1.0
	_CylinderObstructionDestroyRadius ("Cylinder Obstruction Destroy Radius", float) = 10.0

	_CircleStrength ("Circle Obstruction Strength", Range(0,1)) = 1.0
	_CircleObstructionDestroyRadius ("Circle Obstruction Destroy Radius", float) = 10.0

	_CurveStrength ("Curve Obstruction Strength", Range(0,1)) = 1.0
	_CurveObstructionDestroyRadius ("Curve Obstruction Destroy Radius", float) = 10.0
	[HideInInspector] _ObstructionCurve("Obstruction Curve", 2D) = "white" {}

	_DissolveFallOff("Dissolve FallOff", Range(0,1)) = 0.0

	_DissolveMask("Dissolve Mask", 2D) = "white" {}
	_DissolveMaskEnabled("Use DissolveMask", float) = 0.0

	_IntrinsicDissolveStrength("Intrinsic Dissolve Strength", Range(0,1)) = 0.0

	[MaterialToggle] _PreviewMode("Preview Mode", float) = 0.0
	_PreviewIndicatorLineThickness("Indicator Line Thickness",  Range(0.01,0.5)) = 0.04        

	[AbsoluteValue()] _UVs ("Dissolve Texture Scale", float) = 1.0
	[MaterialToggle] _hasClippedShadows("Has Clipped Shadows", Float) = 0
        
	[MaterialToggle] _Floor ("Floor", float) = 1.0
	[Enum(FloorMode)] _FloorMode ("Floor Mode", Float) = 0
	_FloorY ("FloorY",  float) = 1.0
	_PlayerPosYOffset ("PlayerPos Y Offset", float) = 1.0  

	[AbsoluteValue()] _FloorYTextureGradientLength ("FloorY Texture Gradient Length", float) = 0.1  

	[MaterialToggle] _AnimationEnabled("Animation Enabled", Float) = 1
	_AnimationSpeed("Animation Speed", Range(0,2)) = 1

	[MaterialToggle] _IsReplacementShader ("hidden: _IsReplacementShader", Float) = 0

	[HideInInspector] _RaycastMode ("hidden: _RaycastMode", Float) = 0
	[HideInInspector] _TriggerMode ("hidden: _TriggerMode", Float) = 0

	[HideInInspector] _IsExempt ("_IsExempt", Float) = 0

	[AbsoluteValue()] _TransitionDuration ("Transition Duration In Seconds", Float) = 2
	[AbsoluteValue()] _DefaultEffectRadius ("Default Effect Radius",Float) = 25


	[HideInInspector] _numOfPlayersInside ("hidden: _numOfPlayersInside", Float) = 0
	[HideInInspector] _tValue ("hidden: _tValue", Float) = 0
	[HideInInspector] _tDirection ("hidden: _tDirection", Float) = 0
	[HideInInspector] _id ("hidden: _id", Float) = 0

	[MaterialToggle] _TexturedEmissionEdge("Textured Emission Edge", float) = 1.0
	_TexturedEmissionEdgeStrength("Textured Emission Edge Strength", Range(0,1)) = 0.3

	[MaterialToggle] _IsometricExclusion("Isometric Exclusion", float) = 0.0
	_IsometricExclusionDistance("Isometric Exclusion Distance", float) = 0.0
	_IsometricExclusionGradientLength("Isometric Exclusion Gradient Length", float) = 0.1

	[MaterialToggle] _Ceiling ("Ceiling", float) = 0.0

	[Enum(CeilingMode)] _CeilingMode ("Ceiling Mode", Float) = 0
	[Enum(CeilingBlendMode)] _CeilingBlendMode ("Blending Mode", Float) = 1.0
	_CeilingY ("CeilingY",  float) = 1.0
	_CeilingPlayerYOffset ("PlayerPos Y Offset", float) = 1.0  
	_CeilingYGradientLength ("CeilingY Gradient Length", float) = 0.1

	[MaterialToggle] _Zoning("Zoning", float) = 0.0
	[Enum(ZoningMode)] _ZoningMode("Zoning Mode", Float) = 0.0
	_ZoningEdgeGradientLength ("Edge Gradient Length", float) = 0.1
	[MaterialToggle] _IsZoningRevealable ("Is Zoning Revealable", float) = 0.0

	[MaterialToggle] _SyncZonesWithFloorY ("Sync Zones With FloorY", float) = 0.0
	_SyncZonesFloorYOffset ("Sync Zones Floor YOffset", float) = 0.0

	[MaterialToggle] _isReferenceMaterial("Is Reference Material", float) = 0.0


    // FOR UI ONLY
    [HideInInspector] _ShowContentDissolveArea ("hidden: _ShowContentDissolveArea", Float) = 1
    [HideInInspector] _ShowContentInteractionOptionsArea ("hidden: _ShowContentInteractionOptionsArea", Float) = 1
    [HideInInspector] _ShowContentObstructionOptionsArea ("hidden: _ShowContentObstructionOptionsArea", Float) = 1
    [HideInInspector] _ShowContentAnimationArea ("hidden: _ShowContentAnimationArea", Float) = 1
    [HideInInspector] _ShowContentZoningArea ("hidden: _ShowContentZoningArea", Float) = 1
    [HideInInspector] _ShowContentReplacementOptionsArea ("hidden: _ShowContentReplacementOptionsArea", Float) = 1
    [HideInInspector] _ShowContentDebugArea ("hidden: _ShowContentDebugArea", Float) = 1
    
    [MaterialToggle] _SyncCullMode ("_SyncCullMode", Float) = 0




      [HideInInspector] _StencilRef("Vector1 ", Int) = 0
      [HideInInspector] _StencilWriteMask("Vector1 ", Int) = 3
      [HideInInspector] _StencilRefDepth("Vector1 ", Int) = 0
      [HideInInspector] _StencilWriteMaskDepth("Vector1 ", Int) = 32
      [HideInInspector] _StencilRefMV("Vector1 ", Int) = 128
      [HideInInspector] _StencilWriteMaskMV("Vector1 ", Int) = 128
      [HideInInspector] _StencilRefDistortionVec("Vector1 ", Int) = 64
      [HideInInspector] _StencilWriteMaskDistortionVec("Vector1 ", Int) = 64
      [HideInInspector] _StencilWriteMaskGBuffer("Vector1 ", Int) = 3
      [HideInInspector] _StencilRefGBuffer("Vector1 ", Int) = 2
      [HideInInspector] _ZTestGBuffer("Vector1 ", Int) = 4
      [HideInInspector] [ToggleUI] _RequireSplitLighting("Boolean", Float) = 0
      [HideInInspector] [ToggleUI] _ReceivesSSR("Boolean", Float) = 1
      [HideInInspector] _SurfaceType("Vector1 ", Float) = 0
      [HideInInspector] [ToggleUI] _ZWrite("Boolean", Float) = 0
      [HideInInspector] _TransparentSortPriority("Vector1 ", Int) = 0
      [HideInInspector] _ZTestDepthEqualForOpaque("Vector1 ", Int) = 4
      [HideInInspector] [Enum(UnityEngine.Rendering.CompareFunction)] _ZTestTransparent("Vector1", Float) = 4
      [HideInInspector] [ToggleUI] _TransparentBackfaceEnable("Boolean", Float) = 0
      [HideInInspector] [ToggleUI] _AlphaCutoffEnable("Boolean", Float) = 0
      [HideInInspector] [ToggleUI] _UseShadowThreshold("Boolean", Float) = 0
      [HideInInspector] _BlendMode("Float", Float) = 0
   }
   SubShader
   {
      Tags { "RenderPipeline"="HDRenderPipeline" "RenderPipeline" = "HDRenderPipeline" "RenderType" = "HDLitShader" "Queue" = "Geometry+225" }

      
              Pass
        {
            // based on HDLitPass.template
            Name "Forward"
            Tags { "LightMode" = "Forward" }

            
            
        
            
            // Stencil setup
        Stencil
        {
           WriteMask [_StencilWriteMask]
           Ref [_StencilRef]
           Comp Always
           Pass Replace
        }
        
            ColorMask [_ColorMaskTransparentVel] 1

                Cull [_CullMode]

            
            //-------------------------------------------------------------------------------------
            // End Render Modes
            //-------------------------------------------------------------------------------------
        
            HLSLPROGRAM
        
            #pragma target 4.5
            #pragma only_renderers d3d11 ps4 xboxone vulkan metal switch
            //#pragma enable_d3d11_debug_symbols
        
            #pragma multi_compile_instancing
        
            //#pragma multi_compile_local _ _ALPHATEST_ON
        
            // #pragma multi_compile _ LOD_FADE_CROSSFADE
        
            //#pragma shader_feature _SURFACE_TYPE_TRANSPARENT
            //#pragma shader_feature_local _BLENDMODE_OFF _BLENDMODE_ALPHA _BLENDMODE_ADDITIVE _BLENDMODE_PRE_MULTIPLY
        
            //-------------------------------------------------------------------------------------
            // Variant Definitions (active field translations to HDRP defines)
            //-------------------------------------------------------------------------------------
            // #define _MATERIAL_FEATURE_SUBSURFACE_SCATTERING 1
            // #define _MATERIAL_FEATURE_TRANSMISSION 1
            // #define _MATERIAL_FEATURE_ANISOTROPY 1
            // #define _MATERIAL_FEATURE_IRIDESCENCE 1
            // #define _MATERIAL_FEATURE_SPECULAR_COLOR 1
            #define _ENABLE_FOG_ON_TRANSPARENT 1
            // #define _AMBIENT_OCCLUSION 1
            // #define _SPECULAR_OCCLUSION_FROM_AO 1
            // #define _SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL 1
            // #define _SPECULAR_OCCLUSION_CUSTOM 1
            // #define _ENERGY_CONSERVING_SPECULAR 1
            // #define _ENABLE_GEOMETRIC_SPECULAR_AA 1
            // #define _HAS_REFRACTION 1
            // #define _REFRACTION_PLANE 1
            // #define _REFRACTION_SPHERE 1
            // #define _DISABLE_DECALS 1
            // #define _DISABLE_SSR 1
            // #define _ADD_PRECOMPUTED_VELOCITY
            // #define _WRITE_TRANSPARENT_MOTION_VECTOR 1
            // #define _DEPTHOFFSET_ON 1
            // #define _BLENDMODE_PRESERVE_SPECULAR_LIGHTING 1

            #define SHADERPASS SHADERPASS_FORWARD
            #define _PASSFORWARD 1
            
            


    #pragma shader_feature_local _ALPHATEST_ON

    #pragma shader_feature_local_fragment _NORMALMAP_TANGENT_SPACE


    #pragma shader_feature_local _NORMALMAP
    #pragma shader_feature_local_fragment _MASKMAP
    #pragma shader_feature_local_fragment _EMISSIVE_COLOR_MAP
    #pragma shader_feature_local_fragment _ANISOTROPYMAP
    #pragma shader_feature_local_fragment _DETAIL_MAP
    #pragma shader_feature_local_fragment _SUBSURFACE_MASK_MAP
    #pragma shader_feature_local_fragment _THICKNESSMAP
    #pragma shader_feature_local_fragment _IRIDESCENCE_THICKNESSMAP
    #pragma shader_feature_local_fragment _SPECULARCOLORMAP

    // MaterialFeature are used as shader feature to allow compiler to optimize properly
    #pragma shader_feature_local_fragment _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
    #pragma shader_feature_local_fragment _MATERIAL_FEATURE_TRANSMISSION
    #pragma shader_feature_local_fragment _MATERIAL_FEATURE_ANISOTROPY
    #pragma shader_feature_local_fragment _MATERIAL_FEATURE_CLEAR_COAT
    #pragma shader_feature_local_fragment _MATERIAL_FEATURE_IRIDESCENCE
    #pragma shader_feature_local_fragment _MATERIAL_FEATURE_SPECULAR_COLOR


    #pragma shader_feature_local_fragment _ENABLESPECULAROCCLUSION
    #pragma shader_feature_local_fragment _ _SPECULAR_OCCLUSION_NONE _SPECULAR_OCCLUSION_FROM_BENT_NORMAL_MAP

    #ifdef _ENABLESPECULAROCCLUSION
        #define _SPECULAR_OCCLUSION_FROM_BENT_NORMAL_MAP
    #endif


        #pragma shader_feature_local_fragment _OBSTRUCTION_CURVE
        #pragma shader_feature_local_fragment _DISSOLVEMASK
        #pragma shader_feature_local_fragment _ZONING
        #pragma shader_feature_local_fragment _REPLACEMENT
        #pragma shader_feature_local_fragment _PLAYERINDEPENDENT


   #define _HDRP 1
#define _USINGTEXCOORD1 1
#define _USINGTEXCOORD2 1


               #pragma vertex Vert
   #pragma fragment Frag

            
            #pragma multi_compile _ DEBUG_DISPLAY
            #pragma multi_compile _ LIGHTMAP_ON
            #pragma multi_compile _ DIRLIGHTMAP_COMBINED
            #pragma multi_compile _ DYNAMICLIGHTMAP_ON
            #pragma multi_compile _ SHADOWS_SHADOWMASK
            #pragma multi_compile DECALS_OFF DECALS_3RT DECALS_4RT
            #pragma multi_compile USE_FPTL_LIGHTLIST USE_CLUSTERED_LIGHTLIST
            #pragma multi_compile SHADOW_LOW SHADOW_MEDIUM SHADOW_HIGH
            #define REQUIRE_DEPTH_TEXTURE
            


                  // useful conversion functions to make surface shader code just work

      #define UNITY_DECLARE_TEX2D(name) TEXTURE2D(name); SAMPLER(sampler##name);
      #define UNITY_DECLARE_TEX2D_NOSAMPLER(name) TEXTURE2D(name);
      #define UNITY_DECLARE_TEX2DARRAY(name) TEXTURE2D_ARRAY(name); SAMPLER(sampler##name);
      #define UNITY_DECLARE_TEX2DARRAY_NOSAMPLER(tex) TEXTURE2D_ARRAY(tex);

      #define UNITY_SAMPLE_TEX2DARRAY(tex,coord)            SAMPLE_TEXTURE2D_ARRAY(tex, sampler##tex, coord.xy, coord.z)
      #define UNITY_SAMPLE_TEX2DARRAY_LOD(tex,coord,lod)    SAMPLE_TEXTURE2D_ARRAY_LOD(tex, sampler##tex, coord.xy, coord.z, lod)
      #define UNITY_SAMPLE_TEX2D(tex, coord)                SAMPLE_TEXTURE2D(tex, sampler##tex, coord)
      #define UNITY_SAMPLE_TEX2D_SAMPLER(tex, samp, coord)  SAMPLE_TEXTURE2D(tex, sampler##samp, coord)

      #define UNITY_SAMPLE_TEX2D_LOD(tex,coord, lod)   SAMPLE_TEXTURE2D_LOD(tex, sampler_##tex, coord, lod)
      #define UNITY_SAMPLE_TEX2D_SAMPLER_LOD(tex,samplertex,coord, lod) SAMPLE_TEXTURE2D_LOD (tex, sampler##samplertex,coord, lod)

      #if defined(UNITY_COMPILER_HLSL)
         #define UNITY_INITIALIZE_OUTPUT(type,name) name = (type)0;
      #else
         #define UNITY_INITIALIZE_OUTPUT(type,name)
      #endif

      #define sampler2D_float sampler2D
      #define sampler2D_half sampler2D

      #undef WorldNormalVector
      #define WorldNormalVector(data, normal) mul(normal, data.TBNMatrix)

      #define UnityObjectToWorldNormal(normal) mul(GetObjectToWorldMatrix(), normal)




// HDRP Adapter stuff


            // If we use subsurface scattering, enable output split lighting (for forward pass)
            #if defined(_MATERIAL_FEATURE_SUBSURFACE_SCATTERING) && !defined(_SURFACE_TYPE_TRANSPARENT)
            #define OUTPUT_SPLIT_LIGHTING
            #endif

            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Version.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
        
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"
        
            // define FragInputs structure
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
            #if (SHADER_LIBRARY_VERSION_MAJOR >= 10)
               #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/Functions.hlsl"
            #endif


        

            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
        #ifdef DEBUG_DISPLAY
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
        #endif
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
        
        #if (SHADERPASS == SHADERPASS_FORWARD)
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/Lighting.hlsl"
        
            #define HAS_LIGHTLOOP
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/LightLoopDef.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/LightLoop.hlsl"
        #else
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
        #endif
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalUtilities.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitDecalData.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"
        
            // Used by SceneSelectionPass
            int _ObjectId;
            int _PassValue;
        
           
            // data across stages, stripped like the above.
            struct VertexToPixel
            {
               float4 pos : SV_POSITION;
               float3 worldPos : TEXCOORD0;
               float3 worldNormal : TEXCOORD1;
               float4 worldTangent : TEXCOORD2;
               float4 texcoord0 : TEXCOORD3;
               float4 texcoord1 : TEXCOORD4;
               float4 texcoord2 : TEXCOORD5;

               // #if %TEXCOORD3REQUIREKEY%
                float4 texcoord3 : TEXCOORD6;
               // #endif

               // #if %SCREENPOSREQUIREKEY%
               // float4 screenPos : TEXCOORD7;
               // #endif

               // #if %VERTEXCOLORREQUIREKEY%
               // float4 vertexColor : COLOR;
               // #endif

               // #if %EXTRAV2F0REQUIREKEY%
               // float4 extraV2F0 : TEXCOORD8;
               // #endif

               // #if %EXTRAV2F1REQUIREKEY%
               // float4 extraV2F1 : TEXCOORD9;
               // #endif

               // #if %EXTRAV2F2REQUIREKEY%
               // float4 extraV2F2 : TEXCOORD10;
               // #endif

               // #if %EXTRAV2F3REQUIREKEY%
               // float4 extraV2F3 : TEXCOORD11;
               // #endif

               // #if %EXTRAV2F4REQUIREKEY%
               // float4 extraV2F4 : TEXCOORD12;
               // #endif

               // #if %EXTRAV2F5REQUIREKEY%
               // float4 extraV2F5 : TEXCOORD13;
               // #endif

               // #if %EXTRAV2F6REQUIREKEY%
               // float4 extraV2F6 : TEXCOORD14;
               // #endif

               // #if %EXTRAV2F7REQUIREKEY%
               // float4 extraV2F7 : TEXCOORD15;
               // #endif

               #if UNITY_ANY_INSTANCING_ENABLED
                  UNITY_VERTEX_INPUT_INSTANCE_ID
               #endif // UNITY_ANY_INSTANCING_ENABLED

               UNITY_VERTEX_OUTPUT_STEREO
            };



            
            
            // data describing the user output of a pixel
            struct Surface
            {
               half3 Albedo;
               half Height;
               half3 Normal;
               half Smoothness;
               half3 Emission;
               half Metallic;
               half3 Specular;
               half Occlusion;
               half SpecularPower; // for simple lighting
               half Alpha;
               float outputDepth; // if written, SV_Depth semantic is used. ShaderData.clipPos.z is unused value
               // HDRP Only
               half SpecularOcclusion;
               half SubsurfaceMask;
               half Thickness;
               half CoatMask;
               half CoatSmoothness;
               half Anisotropy;
               half IridescenceMask;
               half IridescenceThickness;
               int DiffusionProfileHash;
               float SpecularAAThreshold;
               float SpecularAAScreenSpaceVariance;
               // requires _OVERRIDE_BAKEDGI to be defined, but is mapped in all pipelines
               float3 DiffuseGI;
               float3 BackDiffuseGI;
               float3 SpecularGI;
               float ior;
               float3 transmittanceColor;
               float atDistance;
               float transmittanceMask;
               // requires _OVERRIDE_SHADOWMASK to be defines
               float4 ShadowMask;


            };

            // Data the user declares in blackboard blocks
            struct Blackboard
            {
                
                float blackboardDummyData;
            };

            // data the user might need, this will grow to be big. But easy to strip
            struct ShaderData
            {
               float4 clipPos; // SV_POSITION
               float3 localSpacePosition;
               float3 localSpaceNormal;
               float3 localSpaceTangent;
        
               float3 worldSpacePosition;
               float3 worldSpaceNormal;
               float3 worldSpaceTangent;
               float tangentSign;

               float3 worldSpaceViewDir;
               float3 tangentSpaceViewDir;

               float4 texcoord0;
               float4 texcoord1;
               float4 texcoord2;
               float4 texcoord3;

               float2 screenUV;
               float4 screenPos;

               float4 vertexColor;
               bool isFrontFace;

               float4 extraV2F0;
               float4 extraV2F1;
               float4 extraV2F2;
               float4 extraV2F3;
               float4 extraV2F4;
               float4 extraV2F5;
               float4 extraV2F6;
               float4 extraV2F7;

               float3x3 TBNMatrix;
               Blackboard blackboard;
            };

            struct VertexData
            {
               #if SHADER_TARGET > 30
               // UNITY_VERTEX_INPUT_INSTANCE_ID;
               #endif
               float4 vertex : POSITION;
               float3 normal : NORMAL;
               float4 tangent : TANGENT;
               float4 texcoord0 : TEXCOORD0;

               // optimize out mesh coords when not in use by user or lighting system
               #if _URP && (_USINGTEXCOORD1 || _PASSMETA || _PASSFORWARD || _PASSGBUFFER)
                  float4 texcoord1 : TEXCOORD1;
               #endif

               #if _URP && (_USINGTEXCOORD2 || _PASSMETA || ((_PASSFORWARD || _PASSGBUFFER) && defined(DYNAMICLIGHTMAP_ON)))
                  float4 texcoord2 : TEXCOORD2;
               #endif

               #if _STANDARD && (_USINGTEXCOORD1 || (_PASSMETA || ((_PASSFORWARD || _PASSGBUFFER || _PASSFORWARDADD) && LIGHTMAP_ON)))
                  float4 texcoord1 : TEXCOORD1;
               #endif
               #if _STANDARD && (_USINGTEXCOORD2 || (_PASSMETA || ((_PASSFORWARD || _PASSGBUFFER) && DYNAMICLIGHTMAP_ON)))
                  float4 texcoord2 : TEXCOORD2;
               #endif


               #if _HDRP
                  float4 texcoord1 : TEXCOORD1;
                  float4 texcoord2 : TEXCOORD2;
               #endif

               // #if %TEXCOORD3REQUIREKEY%
                float4 texcoord3 : TEXCOORD3;
               // #endif

               // #if %VERTEXCOLORREQUIREKEY%
               // float4 vertexColor : COLOR;
               // #endif

               #if _HDRP && (_PASSMOTIONVECTOR || ((_PASSFORWARD || _PASSUNLIT) && defined(_WRITE_TRANSPARENT_MOTION_VECTOR)))
                  float3 previousPositionOS : TEXCOORD4; // Contain previous transform position (in case of skinning for example)
                  #if defined (_ADD_PRECOMPUTED_VELOCITY)
                     float3 precomputedVelocity    : TEXCOORD5; // Add Precomputed Velocity (Alembic computes velocities on runtime side).
                  #endif
               #endif

               UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct TessVertex 
            {
               float4 vertex : INTERNALTESSPOS;
               float3 normal : NORMAL;
               float4 tangent : TANGENT;
               float4 texcoord0 : TEXCOORD0;
               float4 texcoord1 : TEXCOORD1;
               float4 texcoord2 : TEXCOORD2;

               // #if %TEXCOORD3REQUIREKEY%
                float4 texcoord3 : TEXCOORD3;
               // #endif

               // #if %VERTEXCOLORREQUIREKEY%
               // float4 vertexColor : COLOR;
               // #endif

               // #if %EXTRAV2F0REQUIREKEY%
               // float4 extraV2F0 : TEXCOORD5;
               // #endif

               // #if %EXTRAV2F1REQUIREKEY%
               // float4 extraV2F1 : TEXCOORD6;
               // #endif

               // #if %EXTRAV2F2REQUIREKEY%
               // float4 extraV2F2 : TEXCOORD7;
               // #endif

               // #if %EXTRAV2F3REQUIREKEY%
               // float4 extraV2F3 : TEXCOORD8;
               // #endif

               // #if %EXTRAV2F4REQUIREKEY%
               // float4 extraV2F4 : TEXCOORD9;
               // #endif

               // #if %EXTRAV2F5REQUIREKEY%
               // float4 extraV2F5 : TEXCOORD10;
               // #endif

               // #if %EXTRAV2F6REQUIREKEY%
               // float4 extraV2F6 : TEXCOORD11;
               // #endif

               // #if %EXTRAV2F7REQUIREKEY%
               // float4 extraV2F7 : TEXCOORD12;
               // #endif

               #if _HDRP && (_PASSMOTIONVECTOR || ((_PASSFORWARD || _PASSUNLIT) && defined(_WRITE_TRANSPARENT_MOTION_VECTOR)))
                  float3 previousPositionOS : TEXCOORD13; // Contain previous transform position (in case of skinning for example)
                  #if defined (_ADD_PRECOMPUTED_VELOCITY)
                     float3 precomputedVelocity : TEXCOORD14;
                  #endif
               #endif

               UNITY_VERTEX_INPUT_INSTANCE_ID
               UNITY_VERTEX_OUTPUT_STEREO
            };

            struct ExtraV2F
            {
               float4 extraV2F0;
               float4 extraV2F1;
               float4 extraV2F2;
               float4 extraV2F3;
               float4 extraV2F4;
               float4 extraV2F5;
               float4 extraV2F6;
               float4 extraV2F7;
               Blackboard blackboard;
               float4 time;
            };


            float3 WorldToTangentSpace(ShaderData d, float3 normal)
            {
               return mul(d.TBNMatrix, normal);
            }

            float3 TangentToWorldSpace(ShaderData d, float3 normal)
            {
               return mul(normal, d.TBNMatrix);
            }

            // in this case, make standard more like SRPs, because we can't fix
            // unity_WorldToObject in HDRP, since it already does macro-fu there

            #if _STANDARD
               float3 TransformWorldToObject(float3 p) { return mul(unity_WorldToObject, float4(p, 1)); };
               float3 TransformObjectToWorld(float3 p) { return mul(unity_ObjectToWorld, float4(p, 1)); };
               float4 TransformWorldToObject(float4 p) { return mul(unity_WorldToObject, p); };
               float4 TransformObjectToWorld(float4 p) { return mul(unity_ObjectToWorld, p); };
               float4x4 GetWorldToObjectMatrix() { return unity_WorldToObject; }
               float4x4 GetObjectToWorldMatrix() { return unity_ObjectToWorld; }
               #if (defined(SHADER_API_D3D11) || defined(SHADER_API_XBOXONE) || defined(UNITY_COMPILER_HLSLCC) || defined(SHADER_API_PSSL) || (SHADER_TARGET_SURFACE_ANALYSIS && !SHADER_TARGET_SURFACE_ANALYSIS_MOJOSHADER))
                 #define UNITY_SAMPLE_TEX2D_LOD(tex,coord, lod) tex.SampleLevel (sampler##tex,coord, lod)
                 #define UNITY_SAMPLE_TEX2D_SAMPLER_LOD(tex,samplertex,coord, lod) tex.SampleLevel (sampler##samplertex,coord, lod)
              #else
                 #define UNITY_SAMPLE_TEX2D_LOD(tex,coord,lod) tex2D (tex,coord,0,lod)
                 #define UNITY_SAMPLE_TEX2D_SAMPLER_LOD(tex,samplertex,coord,lod) tex2D (tex,coord,0,lod)
              #endif

               #undef GetWorldToObjectMatrix()

               #define GetWorldToObjectMatrix()   unity_WorldToObject


            #endif

            float3 GetCameraWorldPosition()
            {
               #if _HDRP
                  return GetCameraRelativePositionWS(_WorldSpaceCameraPos);
               #else
                  return _WorldSpaceCameraPos;
               #endif
            }

            #if _GRABPASSUSED
               #if _STANDARD
                  TEXTURE2D(%GRABTEXTURE%);
                  SAMPLER(sampler_%GRABTEXTURE%);
               #endif

               half3 GetSceneColor(float2 uv)
               {
                  #if _STANDARD
                     return SAMPLE_TEXTURE2D(%GRABTEXTURE%, sampler_%GRABTEXTURE%, uv).rgb;
                  #else
                     return SHADERGRAPH_SAMPLE_SCENE_COLOR(uv);
                  #endif
               }
            #endif


      
            #if _STANDARD
               UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);
               float GetSceneDepth(float2 uv) { return SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, uv); }
               float GetLinear01Depth(float2 uv) { return Linear01Depth(GetSceneDepth(uv)); }
               float GetLinearEyeDepth(float2 uv) { return LinearEyeDepth(GetSceneDepth(uv)); } 
            #else
               float GetSceneDepth(float2 uv) { return SHADERGRAPH_SAMPLE_SCENE_DEPTH(uv); }
               float GetLinear01Depth(float2 uv) { return Linear01Depth(GetSceneDepth(uv), _ZBufferParams); }
               float GetLinearEyeDepth(float2 uv) { return LinearEyeDepth(GetSceneDepth(uv), _ZBufferParams); } 
            #endif

            float3 GetWorldPositionFromDepthBuffer(float2 uv, float3 worldSpaceViewDir)
            {
               float eye = GetLinearEyeDepth(uv);
               float3 camView = mul((float3x3)GetObjectToWorldMatrix(), transpose(mul(GetWorldToObjectMatrix(), UNITY_MATRIX_I_V)) [2].xyz);

               float dt = dot(worldSpaceViewDir, camView);
               float3 div = worldSpaceViewDir/dt;
               float3 wpos = (eye * div) + GetCameraWorldPosition();
               return wpos;
            }

            #if _HDRP
            float3 ObjectToWorldSpacePosition(float3 pos)
            {
               return GetAbsolutePositionWS(TransformObjectToWorld(pos));
            }
            #else
            float3 ObjectToWorldSpacePosition(float3 pos)
            {
               return TransformObjectToWorld(pos);
            }
            #endif

            #if _STANDARD
               UNITY_DECLARE_SCREENSPACE_TEXTURE(_CameraDepthNormalsTexture);
               float3 GetSceneNormal(float2 uv, float3 worldSpaceViewDir)
               {
                  float4 depthNorms = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_CameraDepthNormalsTexture, uv);
                  float3 norms = DecodeViewNormalStereo(depthNorms);
                  norms = mul((float3x3)GetWorldToViewMatrix(), norms) * 0.5 + 0.5;
                  return norms;
               }
            #elif _HDRP
               float3 GetSceneNormal(float2 uv, float3 worldSpaceViewDir)
               {
                  NormalData nd;
                  DecodeFromNormalBuffer(_ScreenSize.xy * uv, nd);
                  return nd.normalWS;
               }
            #elif _URP
               #if (SHADER_LIBRARY_VERSION_MAJOR >= 10)
                  #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareNormalsTexture.hlsl"
               #endif

               float3 GetSceneNormal(float2 uv, float3 worldSpaceViewDir)
               {
                  #if (SHADER_LIBRARY_VERSION_MAJOR >= 10)
                     return SampleSceneNormals(uv);
                  #else
                     float3 wpos = GetWorldPositionFromDepthBuffer(uv, worldSpaceViewDir);
                     return normalize(-cross(ddx(wpos), ddy(wpos))) * 0.5 + 0.5;
                  #endif

                }
             #endif

             #if _HDRP

               half3 UnpackNormalmapRGorAG(half4 packednormal)
               {
                     // This do the trick
                  packednormal.x *= packednormal.w;

                  half3 normal;
                  normal.xy = packednormal.xy * 2 - 1;
                  normal.z = sqrt(1 - saturate(dot(normal.xy, normal.xy)));
                  return normal;
               }
               half3 UnpackNormal(half4 packednormal)
               {
                  #if defined(UNITY_NO_DXT5nm)
                     return packednormal.xyz * 2 - 1;
                  #else
                     return UnpackNormalmapRGorAG(packednormal);
                  #endif
               }
               #endif
               #if _HDRP || _URP

               half3 UnpackScaleNormal(half4 packednormal, half scale)
               {
                 #ifndef UNITY_NO_DXT5nm
                   // Unpack normal as DXT5nm (1, y, 1, x) or BC5 (x, y, 0, 1)
                   // Note neutral texture like "bump" is (0, 0, 1, 1) to work with both plain RGB normal and DXT5nm/BC5
                   packednormal.x *= packednormal.w;
                 #endif
                   half3 normal;
                   normal.xy = (packednormal.xy * 2 - 1) * scale;
                   normal.z = sqrt(1 - saturate(dot(normal.xy, normal.xy)));
                   return normal;
               }	

             #endif


            void GetSun(out float3 lightDir, out float3 color)
            {
               lightDir = float3(0.5, 0.5, 0);
               color = 1;
               #if _HDRP
                  if (_DirectionalLightCount > 0)
                  {
                     DirectionalLightData light = _DirectionalLightDatas[0];
                     lightDir = -light.forward.xyz;
                     color = light.color;
                  }
               #elif _STANDARD
			         lightDir = normalize(_WorldSpaceLightPos0.xyz);
                  color = _LightColor0.rgb;
               #elif _URP
	               Light light = GetMainLight();
	               lightDir = light.direction;
	               color = light.color;
               #endif
            }




            CBUFFER_START(UnityPerMaterial)
               float _StencilRef;
               float _StencilWriteMask;
               float _StencilRefDepth;
               float _StencilWriteMaskDepth;
               float _StencilRefMV;
               float _StencilWriteMaskMV;
               float _StencilRefDistortionVec;
               float _StencilWriteMaskDistortionVec;
               float _StencilWriteMaskGBuffer;
               float _StencilRefGBuffer;
               float _ZTestGBuffer;
               float _RequireSplitLighting;
               float _ReceivesSSR;
               float _ZWrite;
               float _TransparentSortPriority;
               float _ZTestDepthEqualForOpaque;
               float _ZTestTransparent;
               float _TransparentBackfaceEnable;
               float _AlphaCutoffEnable;
               float _UseShadowThreshold;

               
    float4 _BaseColorMap_ST;
    float4 _DetailMap_ST;
    float4 _EmissiveColorMap_ST;

    float _UVBase;
    half4 _Color;
    half4 _BaseColor; // TODO CHECK
    half _Cutoff; 
    half _Mode;
    //float _Cull;
    float _CullMode;
    float _CullModeForward;
    half _Metallic;
    half3 _EmissionColor;
    half _UVSec;

    float3 _EmissiveColor;
    float _AlbedoAffectEmissive;
    float _EmissiveExposureWeight;
    float _MetallicRemapMin;
    float _MetallicRemapMax;
    float _Smoothness;
    float _SmoothnessRemapMin;
    float _SmoothnessRemapMax;
    float _AlphaRemapMin;
    float _AlphaRemapMax;
    float _AORemapMin;
    float _AORemapMax;
    float _NormalScale;
    float _DetailAlbedoScale;
    float _DetailNormalScale;
    float _DetailSmoothnessScale;
    float _Anisotropy;
    float _DiffusionProfileHash;
    float _SubsurfaceMask;
    float _Thickness;
    float4 _ThicknessRemap;
    float _IridescenceThickness;
    float4 _IridescenceThicknessRemap;
    float _IridescenceMask;
    float _CoatMask;
    float4 _SpecularColor;
    float _EnergyConservingSpecularColor;
    int _MaterialID;

    float _LinkDetailsWithBase;
    float4 _UVMappingMask;
    float4 _UVDetailsMappingMask;

    float4 _UVMappingMaskEmissive;

    float _AlphaCutoff;
    //float _AlphaCutoffEnable;


    float _SyncCullMode;

    float _IsReplacementShader;
    float _TriggerMode;
    float _RaycastMode;
    float _IsExempt;
    float _isReferenceMaterial;
    float _InteractionMode;
    int _ArrayLength = 0;
    #if (defined(SHADER_API_GLES) || defined(SHADER_API_GLES3)) 
        float4 _PlayersPosVectorArray[20];
        float _PlayersDataFloatArray[150];     
    #else
        float4 _PlayersPosVectorArray[100];
        float _PlayersDataFloatArray[500];  
    #endif
    float _tDirection = 0;
    float _numOfPlayersInside = 0;
    float _tValue = 0;
    float _id = 0;
    #if _ZONING
        #if (defined(SHADER_API_GLES) || defined(SHADER_API_GLES3)) 
            float _ZDFA[500];
        #else
            float _ZDFA[1000];
        #endif
        float _ZonesDataCount;
    #endif
    #if _REPLACEMENT        
        half4 _DissolveColorGlobal;
        float _DissolveColorSaturationGlobal;
        float _DissolveEmissionGlobal;
        float _DissolveEmissionBoosterGlobal;
        float _TextureVisibilityGlobal;
        float _ObstructionGlobal;
        float _AngleStrengthGlobal;
        float _ConeStrengthGlobal;
        float _ConeObstructionDestroyRadiusGlobal;
        float _CylinderStrengthGlobal;
        float _CylinderObstructionDestroyRadiusGlobal;
        float _CircleStrengthGlobal;
        float _CircleObstructionDestroyRadiusGlobal;
        float _CurveStrengthGlobal;
        float _CurveObstructionDestroyRadiusGlobal;
        float _DissolveFallOffGlobal;
        float _IntrinsicDissolveStrengthGlobal;
        float _PreviewModeGlobal;
        float _UVsGlobal;
        float _hasClippedShadowsGlobal;
        float _FloorGlobal;
        float _FloorModeGlobal;
        float _FloorYGlobal;
        float _PlayerPosYOffsetGlobal;
        float _FloorYTextureGradientLengthGlobal;
        float _AnimationEnabledGlobal;
        float _AnimationSpeedGlobal;
        float _DefaultEffectRadiusGlobal;
        float _TransitionDurationGlobal;        
        float _TexturedEmissionEdgeGlobal;
        float _TexturedEmissionEdgeStrengthGlobal;
        float _IsometricExclusionGlobal;
        float _IsometricExclusionDistanceGlobal;
        float _IsometricExclusionGradientLengthGlobal;
        float _CeilingGlobal;
        float _CeilingModeGlobal;
        float _CeilingBlendModeGlobal;
        float _CeilingYGlobal;
        float _CeilingPlayerYOffsetGlobal;
        float _CeilingYGradientLengthGlobal;
        float _ZoningGlobal;
        float _ZoningModeGlobal;
        float _ZoningEdgeGradientLengthGlobal;
        float _IsZoningRevealableGlobal;
        float _SyncZonesWithFloorYGlobal;
        float _SyncZonesFloorYOffsetGlobal;
        float4 _ObstructionCurveGlobal_TexelSize;
        float4 _DissolveMaskGlobal_TexelSize;
        float _DissolveMaskEnabledGlobal;
        float _PreviewIndicatorLineThicknessGlobal;
    #else
        half _TextureVisibility;
        half _AngleStrength;
        float _Obstruction;
        float _UVs;
        float4 _ObstructionCurve_TexelSize;      
        float _DissolveMaskEnabled;
        float4 _DissolveMask_TexelSize;
        half4 _DissolveColor;
        float _DissolveColorSaturation;
        float _DissolveEmission;
        float _DissolveEmissionBooster;
        float _hasClippedShadows;
        float _ConeStrength;
        float _ConeObstructionDestroyRadius;
        float _CylinderStrength;
        float _CylinderObstructionDestroyRadius;
        float _CircleStrength;
        float _CircleObstructionDestroyRadius;
        float _CurveStrength;
        float _CurveObstructionDestroyRadius;
        float _IntrinsicDissolveStrength;
        float _DissolveFallOff;
        float _PreviewMode;
        float _PreviewIndicatorLineThickness;
        float _AnimationEnabled;
        float _AnimationSpeed;
        float _DefaultEffectRadius;
        float _TransitionDuration;
        float _TexturedEmissionEdge;
        float _TexturedEmissionEdgeStrength;
        float _IsometricExclusion;
        float _IsometricExclusionDistance;
        float _IsometricExclusionGradientLength;
        float _Floor;
        float _FloorMode;
        float _FloorY;
        float _FloorYTextureGradientLength;
        float _PlayerPosYOffset;
        float _Ceiling;
        float _CeilingMode;
        float _CeilingBlendMode;
        float _CeilingY;
        float _CeilingPlayerYOffset;
        float _CeilingYGradientLength;
        float _Zoning;
        float _ZoningMode;
        float _ZoningEdgeGradientLength;
        float _IsZoningRevealable;
        float _SyncZonesWithFloorY;
        float _SyncZonesFloorYOffset;
    #endif



            CBUFFER_END

            

            

            #ifdef unity_WorldToObject
#undef unity_WorldToObject
#endif
#ifdef unity_ObjectToWorld
#undef unity_ObjectToWorld
#endif
#define unity_ObjectToWorld GetObjectToWorldMatrix()
#define unity_WorldToObject GetWorldToObjectMatrix()

    sampler2D _EmissiveColorMap;
    sampler2D _BaseColorMap;
    sampler2D _MaskMap;
    sampler2D _NormalMap;
    sampler2D _NormalMapOS;
    sampler2D _DetailMap;
    sampler2D _TangentMap;
    sampler2D _TangentMapOS;
    sampler2D _AnisotropyMap;
    sampler2D _SubsurfaceMaskMap;
    sampler2D _TransmissionMaskMap;
    sampler2D _ThicknessMap;
    sampler2D _IridescenceThicknessMap;
    sampler2D _IridescenceMaskMap;
    sampler2D _SpecularColorMap;

    sampler2D _CoatMaskMap;

    float3 DecodeNormal (float4 sample, float scale) {
        #if defined(UNITY_NO_DXT5nm)
            return UnpackNormalRGB(sample, scale);
        #else
            return UnpackNormalmapRGorAG(sample, scale);
        #endif
    }

	void Ext_SurfaceFunction0 (inout Surface o, ShaderData d)
	{


//SEE: https://github.com/Unity-Technologies/Graphics/blob/6fdc7996098aa184495c0a3c0da10135bfe18340/Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitDataIndividualLayer.hlsl

        float2 uvBase = _UVMappingMask.x * d.texcoord0 +
                        _UVMappingMask.y * d.texcoord1 +
                        _UVMappingMask.z * d.texcoord2 +
                        _UVMappingMask.w * d.texcoord3;

        float2 uvDetails =  _UVDetailsMappingMask.x * d.texcoord0 +
                            _UVDetailsMappingMask.y * d.texcoord1 +
                            _UVDetailsMappingMask.z * d.texcoord2 +
                            _UVDetailsMappingMask.w * d.texcoord3;


        //TODO: add planar/triplanar

        float4 texcoords;
        //texcoords.xy = d.texcoord0.xy * _BaseColorMap_ST.xy + _BaseColorMap_ST.zw; 
        texcoords.xy = uvBase * _BaseColorMap_ST.xy + _BaseColorMap_ST.zw; 

        texcoords.zw = uvDetails * _DetailMap_ST.xy + _DetailMap_ST.zw;

        if (_LinkDetailsWithBase > 0.0)
        {
            texcoords.zw = texcoords.zw  * _BaseColorMap_ST.xy + _BaseColorMap_ST.zw;

        }

            float3 detailNormalTS = float3(0.0, 0.0, 0.0);
            float detailMask = 0.0;
            #ifdef _DETAIL_MAP
                detailMask = 1.0;
                #ifdef _MASKMAP
                    detailMask = tex2D(_MaskMap, texcoords.xy).b;
                #endif
                float2 detailAlbedoAndSmoothness = tex2D(_DetailMap, texcoords.zw).rb;
                float detailAlbedo = detailAlbedoAndSmoothness.r * 2.0 - 1.0;
                float detailSmoothness = detailAlbedoAndSmoothness.g * 2.0 - 1.0;
                real4 packedNormal = tex2D(_DetailMap, texcoords.zw).rgba;
                detailNormalTS = UnpackNormalAG(packedNormal, _DetailNormalScale);
            #endif
            float4 color = tex2D(_BaseColorMap, texcoords.xy).rgba  * _Color.rgba;
            o.Albedo = color; 
            float alpha = color.a;
            alpha = lerp(_AlphaRemapMin, _AlphaRemapMax, alpha);
            o.Alpha = alpha;
            #ifdef _DETAIL_MAP
                float albedoDetailSpeed = saturate(abs(detailAlbedo) * _DetailAlbedoScale);
                float3 baseColorOverlay = lerp(sqrt(o.Albedo), (detailAlbedo < 0.0) ? float3(0.0, 0.0, 0.0) : float3(1.0, 1.0, 1.0), albedoDetailSpeed * albedoDetailSpeed);
                baseColorOverlay *= baseColorOverlay;
                o.Albedo = lerp(o.Albedo, saturate(baseColorOverlay), detailMask);
            #endif

            #ifdef _NORMALMAP
                float3 normalTS;

                //normalTS = UnpackScaleNormal(tex2D(_NormalMap, texcoords.xy), _NormalScale);
                //#ifdef _DETAIL_MAP
                //    normalTS = lerp(normalTS, BlendNormalRNM(normalTS, normalize(detailNormalTS)), detailMask); 
                //#endif
                //o.Normal = normalTS;


                #ifdef _NORMALMAP_TANGENT_SPACE
                    //normalTS = UnpackScaleNormal(tex2D(_NormalMap, texcoords.xy), _NormalScale);
                    normalTS = DecodeNormal(tex2D(_NormalMap, texcoords.xy), _NormalScale);
                #else
                    #ifdef SURFACE_GRADIENT
                        float3 normalOS = tex2D(_NormalMapOS, texcoords.xy).xyz * 2.0 - 1.0;
                        normalTS = SurfaceGradientFromPerturbedNormal(d.worldSpaceNormal, TransformObjectToWorldNormal(normalOS));
                    #else
                        float3 normalOS = UnpackNormalRGB(tex2D(_NormalMapOS, texcoords.xy), 1.0);
                        float3 bitangent = d.tangentSign * cross(d.worldSpaceNormal.xyz, d.worldSpaceTangent.xyz);
                        half3x3 tangentToWorld = half3x3(d.worldSpaceTangent.xyz, bitangent.xyz, d.worldSpaceNormal.xyz);
                        normalTS = TransformObjectToTangent(normalOS, tangentToWorld);
                    #endif
                #endif

                #ifdef _DETAIL_MAP
                    #ifdef SURFACE_GRADIENT
                    normalTS += detailNormalTS * detailMask;
                    #else
                    normalTS = lerp(normalTS, BlendNormalRNM(normalTS, normalize(detailNormalTS)), detailMask); // todo: detailMask should lerp the angle of the quaternion rotation, not the normals
                    #endif
                #endif
                o.Normal = normalTS;

            #endif

            #if defined(_MASKMAP)
                o.Smoothness = tex2D(_MaskMap, texcoords.xy).a;
                o.Smoothness = lerp(_SmoothnessRemapMin, _SmoothnessRemapMax, o.Smoothness);
            #else
                o.Smoothness = _Smoothness;
            #endif
            #ifdef _DETAIL_MAP
                float smoothnessDetailSpeed = saturate(abs(detailSmoothness) * _DetailSmoothnessScale);
                float smoothnessOverlay = lerp(o.Smoothness, (detailSmoothness < 0.0) ? 0.0 : 1.0, smoothnessDetailSpeed);
                o.Smoothness = lerp(o.Smoothness, saturate(smoothnessOverlay), detailMask);
            #endif
            #ifdef _MASKMAP
                o.Metallic = tex2D(_MaskMap, texcoords.xy).r;
                o.Metallic = lerp(_MetallicRemapMin, _MetallicRemapMax, o.Metallic);
                o.Occlusion = tex2D(_MaskMap, texcoords.xy).g;
                o.Occlusion = lerp(_AORemapMin, _AORemapMax, o.Occlusion);
            #else
                o.Metallic = _Metallic;
                o.Occlusion = 1.0;
            #endif

            #if defined(_SPECULAR_OCCLUSION_FROM_BENT_NORMAL_MAP)
                // TODO: implenent bent normals for this to work
                //o.SpecularOcclusion = GetSpecularOcclusionFromBentAO(V, bentNormalWS, d.worldSpaceNormal, o.Occlusion, PerceptualSmoothnessToRoughness(o.Smoothness));
            #elif  defined(_MASKMAP) && !defined(_SPECULAR_OCCLUSION_NONE)
                float3 V = normalize(d.worldSpaceViewDir);
                o.SpecularOcclusion = GetSpecularOcclusionFromAmbientOcclusion(ClampNdotV(dot(d.worldSpaceNormal, V)), o.Occlusion, PerceptualSmoothnessToRoughness(o.Smoothness));
            #endif

            o.DiffusionProfileHash = asuint(_DiffusionProfileHash);
            o.SubsurfaceMask = _SubsurfaceMask;
            #ifdef _SUBSURFACE_MASK_MAP
                o.SubsurfaceMask *= tex2D(_SubsurfaceMaskMap, texcoords.xy).r;
            #endif
            #ifdef _THICKNESSMAP
                o.Thickness = tex2D(_ThicknessMap, texcoords.xy).r;
                o.Thickness = _ThicknessRemap.x + _ThicknessRemap.y * surfaceData.thickness;
            #else
                o.Thickness = _Thickness;
            #endif
            #ifdef _ANISOTROPYMAP
                o.Anisotropy = tex2D(_AnisotropyMap, texcoords.xy).r;
            #else
                o.Anisotropy = 1.0;
            #endif
                o.Anisotropy *= _Anisotropy;
                o.Specular = _SpecularColor.rgb;
            #ifdef _SPECULARCOLORMAP
                o.Specular *= tex2D(_SpecularColorMap, texcoords.xy).rgb;
            #endif
            #ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
                o.Albedo *= _EnergyConservingSpecularColor > 0.0 ? (1.0 - Max3(o.Specular.r, o.Specular.g, o.Specular.b)) : 1.0;
            #endif
            #ifdef _MATERIAL_FEATURE_CLEAR_COAT
                o.CoatMask = _CoatMask;
                o.CoatMask *= tex2D(_CoatMaskMap, texcoords.xy).r;
            #else
                o.CoatMask = 0.0;
            #endif
            #ifdef _MATERIAL_FEATURE_IRIDESCENCE
                #ifdef _IRIDESCENCE_THICKNESSMAP
                o.IridescenceThickness = tex2D(_IridescenceThicknessMap, texcoords.xy).r;
                o.IridescenceThickness = _IridescenceThicknessRemap.x + _IridescenceThicknessRemap.y * o.IridescenceThickness;
                #else
                o.IridescenceThickness = _IridescenceThickness;
                #endif
                o.IridescenceMask = _IridescenceMask;
                o.IridescenceMask *= tex2D(_IridescenceMaskMap, texcoords.xy).r;
            #else
                o.IridescenceThickness = 0.0;
                o.IridescenceMask = 0.0;
            #endif


//SEE: https://github.com/Unity-Technologies/Graphics/blob/632f80e011f18ea537ee6e2f0be3ff4f4dea6a11/Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitBuiltinData.hlsl

            float3 emissiveColor = _EmissiveColor * lerp(float3(1.0, 1.0, 1.0), o.Albedo.rgb, _AlbedoAffectEmissive);
            #ifdef _EMISSIVE_COLOR_MAP                
                float2 uvEmission = _UVMappingMaskEmissive.x * d.texcoord0 +
                                    _UVMappingMaskEmissive.y * d.texcoord1 +
                                    _UVMappingMaskEmissive.z * d.texcoord2 +
                                    _UVMappingMaskEmissive.w * d.texcoord3;

                uvEmission = uvEmission * _EmissiveColorMap_ST.xy + _EmissiveColorMap_ST.zw;                     

                emissiveColor *= tex2D(_EmissiveColorMap, uvEmission).rgb;
            #endif
            o.Emission += emissiveColor;
            float currentExposureMultiplier = 0;
            #if SHADEROPTIONS_PRE_EXPOSITION
                currentExposureMultiplier =  LOAD_TEXTURE2D(_ExposureTexture, int2(0, 0)).x * _ProbeExposureScale;
            #else
                currentExposureMultiplier =  _ProbeExposureScale;
            #endif
            float InverseCurrentExposureMultiplier = 0;
            float exposure = currentExposureMultiplier;
            InverseCurrentExposureMultiplier =  rcp(exposure + (exposure == 0.0)); 
            float3 emissiveRcpExposure = o.Emission * InverseCurrentExposureMultiplier;
            o.Emission = lerp(emissiveRcpExposure, o.Emission, _EmissiveExposureWeight);


// SEE: https://github.com/Unity-Technologies/Graphics/blob/223d8105e68c5a808027d78f39cb4e2545fd6276/Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitData.hlsl
            #if defined(_ALPHATEST_ON)
                float alphaTex = tex2D(_BaseColorMap, texcoords.xy).a;
                alphaTex = lerp(_AlphaRemapMin, _AlphaRemapMax, alphaTex);
                float alphaValue = alphaTex * _BaseColor.a;

                // Perform alha test very early to save performance (a killed pixel will not sample textures)
                //#if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
                //float alphaCutoff = _AlphaCutoffPrepass;
                //#elif SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_POSTPASS
                //float alphaCutoff = _AlphaCutoffPostpass;
                //#elif (SHADERPASS == SHADERPASS_SHADOWS) || (SHADERPASS == SHADERPASS_RAYTRACING_VISIBILITY)
                //float alphaCutoff = _UseShadowThreshold ? _AlphaCutoffShadow : _AlphaCutoff;
                //#else
                float alphaCutoff = _AlphaCutoff;
                //#endif

                // GENERIC_ALPHA_TEST(alphaValue, alphaCutoff);
                clip(alpha - alphaCutoff);
            #endif

	}




    #if _REPLACEMENT
        sampler2D _DissolveTexGlobal;
    #else
        sampler2D _DissolveTex;
    #endif

    //#if _DISSOLVEMASK
    #if _REPLACEMENT
        sampler2D _DissolveMaskGlobal;
    #else
        sampler2D _DissolveMask;
    #endif
    //#endif

    #if _REPLACEMENT
        sampler2D _ObstructionCurveGlobal;
    #else
        sampler2D _ObstructionCurve;
    #endif



    #if _URP
        half2 ParallaxOffset1Step (half h, half height, half3 viewDir)
        {
            h = h * height - height/2.0;
            half3 v = normalize(viewDir);
            v.z += 0.42;
            return h * (v.xy / v.z);
        }
        #ifdef UNITY_COLORSPACE_GAMMA
            #define unity_ColorSpaceDouble half4(2.0, 2.0, 2.0, 2.0)
        #else 
            #define unity_ColorSpaceDouble half4(4.59479380, 4.59479380, 4.59479380, 2.0)
        #endif
        half3 BlendNormals(half3 n1, half3 n2)
        {
            return normalize(half3(n1.xy + n2.xy, n1.z*n2.z));
        }
    #endif


#ifdef USE_UNITY_TEXTURE_2D_TYPE
#undef USE_UNITY_TEXTURE_2D_TYPE
#endif
#include "Assets/See-through Shader/Core/Shaders/xxSharedSTSDependencies/SeeThroughShaderFunction.hlsl"



	void Ext_SurfaceFunction1 (inout Surface o, ShaderData d)
	{
        half3 albedo;
        half3 emission;
        float alphaForClipping;
#ifdef USE_UNITY_TEXTURE_2D_TYPE
#undef USE_UNITY_TEXTURE_2D_TYPE
#endif
#if _REPLACEMENT
    DoSeeThroughShading( o.Albedo, o.Normal, d.worldSpacePosition, d.worldSpaceNormal,

                        _numOfPlayersInside, _tDirection, _tValue, _id,
                        _TriggerMode, _RaycastMode,
                        _IsExempt,

                        _DissolveColorGlobal, _DissolveColorSaturationGlobal, _UVsGlobal,
                        _DissolveEmissionGlobal, _DissolveEmissionBoosterGlobal, _TexturedEmissionEdgeGlobal, _TexturedEmissionEdgeStrengthGlobal,
                        _hasClippedShadowsGlobal,

                        _ObstructionGlobal,
                        _AngleStrengthGlobal,
                        _ConeStrengthGlobal, _ConeObstructionDestroyRadiusGlobal,
                        _CylinderStrengthGlobal, _CylinderObstructionDestroyRadiusGlobal,
                        _CircleStrengthGlobal, _CircleObstructionDestroyRadiusGlobal,
                        _CurveStrengthGlobal, _CurveObstructionDestroyRadiusGlobal,
                        _DissolveFallOffGlobal,
                        _DissolveMaskEnabledGlobal,
                        _IntrinsicDissolveStrengthGlobal,
                        _DefaultEffectRadiusGlobal,

                        _IsometricExclusionGlobal, _IsometricExclusionDistanceGlobal, _IsometricExclusionGradientLengthGlobal,
                        _CeilingGlobal, _CeilingModeGlobal, _CeilingBlendModeGlobal, _CeilingYGlobal, _CeilingPlayerYOffsetGlobal, _CeilingYGradientLengthGlobal,
                        _FloorGlobal, _FloorModeGlobal, _FloorYGlobal, _PlayerPosYOffsetGlobal, _FloorYTextureGradientLengthGlobal,

                        _AnimationEnabledGlobal, _AnimationSpeedGlobal,
                        _TransitionDurationGlobal,

                        _ZoningGlobal, _ZoningModeGlobal, _IsZoningRevealableGlobal, _ZoningEdgeGradientLengthGlobal,
                        _SyncZonesWithFloorYGlobal, _SyncZonesFloorYOffsetGlobal,

                        _PreviewModeGlobal,
                        _PreviewIndicatorLineThicknessGlobal,

                        _DissolveTexGlobal,
                        _DissolveMaskGlobal,
                        _ObstructionCurveGlobal,

                        _DissolveMaskGlobal_TexelSize,
                        _ObstructionCurveGlobal_TexelSize,

                        albedo, emission, alphaForClipping);
#else 
    
    DoSeeThroughShading( o.Albedo, o.Normal, d.worldSpacePosition, d.worldSpaceNormal,

                        _numOfPlayersInside, _tDirection, _tValue, _id,
                        _TriggerMode, _RaycastMode,
                        _IsExempt,

                        _DissolveColor, _DissolveColorSaturation, _UVs,
                        _DissolveEmission, _DissolveEmissionBooster, _TexturedEmissionEdge, _TexturedEmissionEdgeStrength,
                        _hasClippedShadows,

                        _Obstruction,
                        _AngleStrength,
                        _ConeStrength, _ConeObstructionDestroyRadius,
                        _CylinderStrength, _CylinderObstructionDestroyRadius,
                        _CircleStrength, _CircleObstructionDestroyRadius,
                        _CurveStrength, _CurveObstructionDestroyRadius,
                        _DissolveFallOff,
                        _DissolveMaskEnabled,
                        _IntrinsicDissolveStrength,
                        _DefaultEffectRadius,

                        _IsometricExclusion, _IsometricExclusionDistance, _IsometricExclusionGradientLength,
                        _Ceiling, _CeilingMode, _CeilingBlendMode, _CeilingY, _CeilingPlayerYOffset, _CeilingYGradientLength,
                        _Floor, _FloorMode, _FloorY, _PlayerPosYOffset, _FloorYTextureGradientLength,

                        _AnimationEnabled, _AnimationSpeed,
                        _TransitionDuration,

                        _Zoning, _ZoningMode, _IsZoningRevealable, _ZoningEdgeGradientLength,
                        _SyncZonesWithFloorY, _SyncZonesFloorYOffset,

                        _PreviewMode,
                        _PreviewIndicatorLineThickness,                            

                        _DissolveTex,
                        _DissolveMask,
                        _ObstructionCurve,

                        _DissolveMask_TexelSize,
                        _ObstructionCurve_TexelSize,

                        albedo, emission, alphaForClipping);
#endif 



        o.Albedo = albedo;
        o.Emission += emission;   

	}




        
            void ChainSurfaceFunction(inout Surface l, inout ShaderData d)
            {
                  Ext_SurfaceFunction0(l, d);
                  Ext_SurfaceFunction1(l, d);
                 // Ext_SurfaceFunction2(l, d);
                 // Ext_SurfaceFunction3(l, d);
                 // Ext_SurfaceFunction4(l, d);
                 // Ext_SurfaceFunction5(l, d);
                 // Ext_SurfaceFunction6(l, d);
                 // Ext_SurfaceFunction7(l, d);
                 // Ext_SurfaceFunction8(l, d);
                 // Ext_SurfaceFunction9(l, d);
		           // Ext_SurfaceFunction10(l, d);
                 // Ext_SurfaceFunction11(l, d);
                 // Ext_SurfaceFunction12(l, d);
                 // Ext_SurfaceFunction13(l, d);
                 // Ext_SurfaceFunction14(l, d);
                 // Ext_SurfaceFunction15(l, d);
                 // Ext_SurfaceFunction16(l, d);
                 // Ext_SurfaceFunction17(l, d);
                 // Ext_SurfaceFunction18(l, d);
		           // Ext_SurfaceFunction19(l, d);
                 // Ext_SurfaceFunction20(l, d);
                 // Ext_SurfaceFunction21(l, d);
                 // Ext_SurfaceFunction22(l, d);
                 // Ext_SurfaceFunction23(l, d);
                 // Ext_SurfaceFunction24(l, d);
                 // Ext_SurfaceFunction25(l, d);
                 // Ext_SurfaceFunction26(l, d);
                 // Ext_SurfaceFunction27(l, d);
                 // Ext_SurfaceFunction28(l, d);
		           // Ext_SurfaceFunction29(l, d);
            }

            void ChainModifyVertex(inout VertexData v, inout VertexToPixel v2p, float4 time)
            {
                 ExtraV2F d;
                 
                 ZERO_INITIALIZE(ExtraV2F, d);
                 ZERO_INITIALIZE(Blackboard, d.blackboard);
                 // due to motion vectors in HDRP, we need to use the last
                 // time in certain spots. So if you are going to use _Time to adjust vertices,
                 // you need to use this time or motion vectors will break. 
                 d.time = time;

                 //  Ext_ModifyVertex0(v, d);
                 // Ext_ModifyVertex1(v, d);
                 // Ext_ModifyVertex2(v, d);
                 // Ext_ModifyVertex3(v, d);
                 // Ext_ModifyVertex4(v, d);
                 // Ext_ModifyVertex5(v, d);
                 // Ext_ModifyVertex6(v, d);
                 // Ext_ModifyVertex7(v, d);
                 // Ext_ModifyVertex8(v, d);
                 // Ext_ModifyVertex9(v, d);
                 // Ext_ModifyVertex10(v, d);
                 // Ext_ModifyVertex11(v, d);
                 // Ext_ModifyVertex12(v, d);
                 // Ext_ModifyVertex13(v, d);
                 // Ext_ModifyVertex14(v, d);
                 // Ext_ModifyVertex15(v, d);
                 // Ext_ModifyVertex16(v, d);
                 // Ext_ModifyVertex17(v, d);
                 // Ext_ModifyVertex18(v, d);
                 // Ext_ModifyVertex19(v, d);
                 // Ext_ModifyVertex20(v, d);
                 // Ext_ModifyVertex21(v, d);
                 // Ext_ModifyVertex22(v, d);
                 // Ext_ModifyVertex23(v, d);
                 // Ext_ModifyVertex24(v, d);
                 // Ext_ModifyVertex25(v, d);
                 // Ext_ModifyVertex26(v, d);
                 // Ext_ModifyVertex27(v, d);
                 // Ext_ModifyVertex28(v, d);
                 // Ext_ModifyVertex29(v, d);


                 // #if %EXTRAV2F0REQUIREKEY%
                 // v2p.extraV2F0 = d.extraV2F0;
                 // #endif

                 // #if %EXTRAV2F1REQUIREKEY%
                 // v2p.extraV2F1 = d.extraV2F1;
                 // #endif

                 // #if %EXTRAV2F2REQUIREKEY%
                 // v2p.extraV2F2 = d.extraV2F2;
                 // #endif

                 // #if %EXTRAV2F3REQUIREKEY%
                 // v2p.extraV2F3 = d.extraV2F3;
                 // #endif

                 // #if %EXTRAV2F4REQUIREKEY%
                 // v2p.extraV2F4 = d.extraV2F4;
                 // #endif

                 // #if %EXTRAV2F5REQUIREKEY%
                 // v2p.extraV2F5 = d.extraV2F5;
                 // #endif

                 // #if %EXTRAV2F6REQUIREKEY%
                 // v2p.extraV2F6 = d.extraV2F6;
                 // #endif

                 // #if %EXTRAV2F7REQUIREKEY%
                 // v2p.extraV2F7 = d.extraV2F7;
                 // #endif
            }

            void ChainModifyTessellatedVertex(inout VertexData v, inout VertexToPixel v2p)
            {
               ExtraV2F d;
               ZERO_INITIALIZE(ExtraV2F, d);
               ZERO_INITIALIZE(Blackboard, d.blackboard);

               // #if %EXTRAV2F0REQUIREKEY%
               // d.extraV2F0 = v2p.extraV2F0;
               // #endif

               // #if %EXTRAV2F1REQUIREKEY%
               // d.extraV2F1 = v2p.extraV2F1;
               // #endif

               // #if %EXTRAV2F2REQUIREKEY%
               // d.extraV2F2 = v2p.extraV2F2;
               // #endif

               // #if %EXTRAV2F3REQUIREKEY%
               // d.extraV2F3 = v2p.extraV2F3;
               // #endif

               // #if %EXTRAV2F4REQUIREKEY%
               // d.extraV2F4 = v2p.extraV2F4;
               // #endif

               // #if %EXTRAV2F5REQUIREKEY%
               // d.extraV2F5 = v2p.extraV2F5;
               // #endif

               // #if %EXTRAV2F6REQUIREKEY%
               // d.extraV2F6 = v2p.extraV2F6;
               // #endif

               // #if %EXTRAV2F7REQUIREKEY%
               // d.extraV2F7 = v2p.extraV2F7;
               // #endif


               // Ext_ModifyTessellatedVertex0(v, d);
               // Ext_ModifyTessellatedVertex1(v, d);
               // Ext_ModifyTessellatedVertex2(v, d);
               // Ext_ModifyTessellatedVertex3(v, d);
               // Ext_ModifyTessellatedVertex4(v, d);
               // Ext_ModifyTessellatedVertex5(v, d);
               // Ext_ModifyTessellatedVertex6(v, d);
               // Ext_ModifyTessellatedVertex7(v, d);
               // Ext_ModifyTessellatedVertex8(v, d);
               // Ext_ModifyTessellatedVertex9(v, d);
               // Ext_ModifyTessellatedVertex10(v, d);
               // Ext_ModifyTessellatedVertex11(v, d);
               // Ext_ModifyTessellatedVertex12(v, d);
               // Ext_ModifyTessellatedVertex13(v, d);
               // Ext_ModifyTessellatedVertex14(v, d);
               // Ext_ModifyTessellatedVertex15(v, d);
               // Ext_ModifyTessellatedVertex16(v, d);
               // Ext_ModifyTessellatedVertex17(v, d);
               // Ext_ModifyTessellatedVertex18(v, d);
               // Ext_ModifyTessellatedVertex19(v, d);
               // Ext_ModifyTessellatedVertex20(v, d);
               // Ext_ModifyTessellatedVertex21(v, d);
               // Ext_ModifyTessellatedVertex22(v, d);
               // Ext_ModifyTessellatedVertex23(v, d);
               // Ext_ModifyTessellatedVertex24(v, d);
               // Ext_ModifyTessellatedVertex25(v, d);
               // Ext_ModifyTessellatedVertex26(v, d);
               // Ext_ModifyTessellatedVertex27(v, d);
               // Ext_ModifyTessellatedVertex28(v, d);
               // Ext_ModifyTessellatedVertex29(v, d);

               // #if %EXTRAV2F0REQUIREKEY%
               // v2p.extraV2F0 = d.extraV2F0;
               // #endif

               // #if %EXTRAV2F1REQUIREKEY%
               // v2p.extraV2F1 = d.extraV2F1;
               // #endif

               // #if %EXTRAV2F2REQUIREKEY%
               // v2p.extraV2F2 = d.extraV2F2;
               // #endif

               // #if %EXTRAV2F3REQUIREKEY%
               // v2p.extraV2F3 = d.extraV2F3;
               // #endif

               // #if %EXTRAV2F4REQUIREKEY%
               // v2p.extraV2F4 = d.extraV2F4;
               // #endif

               // #if %EXTRAV2F5REQUIREKEY%
               // v2p.extraV2F5 = d.extraV2F5;
               // #endif

               // #if %EXTRAV2F6REQUIREKEY%
               // v2p.extraV2F6 = d.extraV2F6;
               // #endif

               // #if %EXTRAV2F7REQUIREKEY%
               // v2p.extraV2F7 = d.extraV2F7;
               // #endif
            }

            void ChainFinalColorForward(inout Surface l, inout ShaderData d, inout half4 color)
            {
               //   Ext_FinalColorForward0(l, d, color);
               //   Ext_FinalColorForward1(l, d, color);
               //   Ext_FinalColorForward2(l, d, color);
               //   Ext_FinalColorForward3(l, d, color);
               //   Ext_FinalColorForward4(l, d, color);
               //   Ext_FinalColorForward5(l, d, color);
               //   Ext_FinalColorForward6(l, d, color);
               //   Ext_FinalColorForward7(l, d, color);
               //   Ext_FinalColorForward8(l, d, color);
               //   Ext_FinalColorForward9(l, d, color);
               //  Ext_FinalColorForward10(l, d, color);
               //  Ext_FinalColorForward11(l, d, color);
               //  Ext_FinalColorForward12(l, d, color);
               //  Ext_FinalColorForward13(l, d, color);
               //  Ext_FinalColorForward14(l, d, color);
               //  Ext_FinalColorForward15(l, d, color);
               //  Ext_FinalColorForward16(l, d, color);
               //  Ext_FinalColorForward17(l, d, color);
               //  Ext_FinalColorForward18(l, d, color);
               //  Ext_FinalColorForward19(l, d, color);
               //  Ext_FinalColorForward20(l, d, color);
               //  Ext_FinalColorForward21(l, d, color);
               //  Ext_FinalColorForward22(l, d, color);
               //  Ext_FinalColorForward23(l, d, color);
               //  Ext_FinalColorForward24(l, d, color);
               //  Ext_FinalColorForward25(l, d, color);
               //  Ext_FinalColorForward26(l, d, color);
               //  Ext_FinalColorForward27(l, d, color);
               //  Ext_FinalColorForward28(l, d, color);
               //  Ext_FinalColorForward29(l, d, color);
            }

            void ChainFinalGBufferStandard(inout Surface s, inout ShaderData d, inout half4 GBuffer0, inout half4 GBuffer1, inout half4 GBuffer2, inout half4 outEmission, inout half4 outShadowMask)
            {
               //   Ext_FinalGBufferStandard0(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard1(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard2(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard3(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard4(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard5(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard6(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard7(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard8(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard9(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard10(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard11(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard12(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard13(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard14(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard15(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard16(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard17(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard18(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard19(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard20(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard21(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard22(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard23(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard24(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard25(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard26(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard27(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard28(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard29(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
            }



            

         ShaderData CreateShaderData(VertexToPixel i
                  #if NEED_FACING
                     , bool facing
                  #endif
         )
         {
            ShaderData d = (ShaderData)0;
            d.clipPos = i.pos;
            d.worldSpacePosition = i.worldPos;

            d.worldSpaceNormal = normalize(i.worldNormal);
            d.worldSpaceTangent = normalize(i.worldTangent.xyz);
            d.tangentSign = i.worldTangent.w;
            float3 bitangent = cross(i.worldTangent.xyz, i.worldNormal) * d.tangentSign * -1;
            

            d.TBNMatrix = float3x3(d.worldSpaceTangent, bitangent, d.worldSpaceNormal);
            d.worldSpaceViewDir = normalize(_WorldSpaceCameraPos - i.worldPos);

            d.tangentSpaceViewDir = mul(d.TBNMatrix, d.worldSpaceViewDir);
             d.texcoord0 = i.texcoord0;
             d.texcoord1 = i.texcoord1;
             d.texcoord2 = i.texcoord2;

            // #if %TEXCOORD3REQUIREKEY%
             d.texcoord3 = i.texcoord3;
            // #endif

            // d.isFrontFace = facing;
            // #if %VERTEXCOLORREQUIREKEY%
            // d.vertexColor = i.vertexColor;
            // #endif

            // these rarely get used, so we back transform them. Usually will be stripped.
            #if _HDRP
                // d.localSpacePosition = mul(unity_WorldToObject, float4(GetCameraRelativePositionWS(i.worldPos), 1)).xyz;
            #else
                // d.localSpacePosition = mul(unity_WorldToObject, float4(i.worldPos, 1)).xyz;
            #endif
            // d.localSpaceNormal = normalize(mul((float3x3)unity_WorldToObject, i.worldNormal));
            // d.localSpaceTangent = normalize(mul((float3x3)unity_WorldToObject, i.worldTangent.xyz));

            // #if %SCREENPOSREQUIREKEY%
            // d.screenPos = i.screenPos;
            // d.screenUV = (i.screenPos.xy / i.screenPos.w);
            // #endif


            // #if %EXTRAV2F0REQUIREKEY%
            // d.extraV2F0 = i.extraV2F0;
            // #endif

            // #if %EXTRAV2F1REQUIREKEY%
            // d.extraV2F1 = i.extraV2F1;
            // #endif

            // #if %EXTRAV2F2REQUIREKEY%
            // d.extraV2F2 = i.extraV2F2;
            // #endif

            // #if %EXTRAV2F3REQUIREKEY%
            // d.extraV2F3 = i.extraV2F3;
            // #endif

            // #if %EXTRAV2F4REQUIREKEY%
            // d.extraV2F4 = i.extraV2F4;
            // #endif

            // #if %EXTRAV2F5REQUIREKEY%
            // d.extraV2F5 = i.extraV2F5;
            // #endif

            // #if %EXTRAV2F6REQUIREKEY%
            // d.extraV2F6 = i.extraV2F6;
            // #endif

            // #if %EXTRAV2F7REQUIREKEY%
            // d.extraV2F7 = i.extraV2F7;
            // #endif

            return d;
         }
         

            

struct VaryingsToPS
{
   VertexToPixel vmesh;
   #ifdef VARYINGS_NEED_PASS
      VaryingsPassToPS vpass;
   #endif
};

struct PackedVaryingsToPS
{
   #ifdef VARYINGS_NEED_PASS
      PackedVaryingsPassToPS vpass;
   #endif
   VertexToPixel vmesh;

   UNITY_VERTEX_OUTPUT_STEREO
};

PackedVaryingsToPS PackVaryingsToPS(VaryingsToPS input)
{
   PackedVaryingsToPS output = (PackedVaryingsToPS)0;
   output.vmesh = input.vmesh;
   #ifdef VARYINGS_NEED_PASS
      output.vpass = PackVaryingsPassToPS(input.vpass);
   #endif

   UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
   return output;
}




VertexToPixel VertMesh(VertexData input)
{
    VertexToPixel output = (VertexToPixel)0;

    UNITY_SETUP_INSTANCE_ID(input);
    UNITY_TRANSFER_INSTANCE_ID(input, output);

    
    ChainModifyVertex(input, output, _Time);


    // This return the camera relative position (if enable)
    float3 positionRWS = TransformObjectToWorld(input.vertex.xyz);
    float3 normalWS = TransformObjectToWorldNormal(input.normal);
    float4 tangentWS = float4(TransformObjectToWorldDir(input.tangent.xyz), input.tangent.w);


    output.worldPos = GetAbsolutePositionWS(positionRWS);
    output.pos = TransformWorldToHClip(positionRWS);
    output.worldNormal = normalWS;
    output.worldTangent = tangentWS;


    output.texcoord0 = input.texcoord0;
    output.texcoord1 = input.texcoord1;
    output.texcoord2 = input.texcoord2;
    // #if %TEXCOORD3REQUIREKEY%
     output.texcoord3 = input.texcoord3;
    // #endif

    // #if %SCREENPOSREQUIREKEY%
    // output.screenPos = ComputeScreenPos(output.pos, _ProjectionParams.x);
    // #endif

    // #if %VERTEXCOLORREQUIREKEY%
    // output.vertexColor = input.vertexColor;
    // #endif
    return output;
}


#if (SHADERPASS == SHADERPASS_DBUFFER_MESH)
void MeshDecalsPositionZBias(inout VaryingsToPS input)
{
#if defined(UNITY_REVERSED_Z)
    input.vmesh.pos.z -= _DecalMeshDepthBias;
#else
    input.vmesh.pos.z += _DecalMeshDepthBias;
#endif
}
#endif


#if (SHADERPASS == SHADERPASS_LIGHT_TRANSPORT)

// This was not in constant buffer in original unity, so keep outiside. But should be in as ShaderRenderPass frequency
float unity_OneOverOutputBoost;
float unity_MaxOutputValue;

CBUFFER_START(UnityMetaPass)
// x = use uv1 as raster position
// y = use uv2 as raster position
bool4 unity_MetaVertexControl;

// x = return albedo
// y = return normal
bool4 unity_MetaFragmentControl;
CBUFFER_END

PackedVaryingsToPS Vert(VertexData inputMesh)
{
    VaryingsToPS output = (VaryingsToPS)0;
    output.vmesh = (VertexToPixel)0;

    UNITY_SETUP_INSTANCE_ID(inputMesh);
    UNITY_TRANSFER_INSTANCE_ID(inputMesh, output.vmesh);

    // Output UV coordinate in vertex shader
    float2 uv = float2(0.0, 0.0);

    if (unity_MetaVertexControl.x)
    {
        uv = inputMesh.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
    }
    else if (unity_MetaVertexControl.y)
    {
        uv = inputMesh.texcoord2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
    }

    // OpenGL right now needs to actually use the incoming vertex position
    // so we create a fake dependency on it here that haven't any impact.
    output.vmesh.pos = float4(uv * 2.0 - 1.0, inputMesh.vertex.z > 0 ? 1.0e-4 : 0.0, 1.0);

#ifdef VARYINGS_NEED_POSITION_WS
    output.vmesh.worldPos = TransformObjectToWorld(inputMesh.vertex.xyz);
#endif

#ifdef VARYINGS_NEED_TANGENT_TO_WORLD
    // Normal is required for triplanar mapping
    output.vmesh.worldNormal = TransformObjectToWorldNormal(inputMesh.normal);
    // Not required but assign to silent compiler warning
    output.vmesh.worldTangent = float4(1.0, 0.0, 0.0, 0.0);
#endif

    output.vmesh.texcoord0 = inputMesh.texcoord0;
    output.vmesh.texcoord1 = inputMesh.texcoord1;
    output.vmesh.texcoord2 = inputMesh.texcoord2;
    // #if %TEXCOORD3REQUIREKEY%
     output.vmesh.texcoord3 = inputMesh.texcoord3;
    // #endif

    // #if %VERTEXCOLORREQUIREKEY%
    // output.vmesh.vertexColor = inputMesh.vertexColor;
    // #endif

    return PackVaryingsToPS(output);
}
#else

PackedVaryingsToPS Vert(VertexData inputMesh)
{
    VaryingsToPS varyingsType;
    varyingsType.vmesh = VertMesh(inputMesh);
    #if (SHADERPASS == SHADERPASS_DBUFFER_MESH)
       MeshDecalsPositionZBias(varyingsType);
    #endif
    return PackVaryingsToPS(varyingsType);
}

#endif



            

            
                FragInputs BuildFragInputs(VertexToPixel input)
                {
                    UNITY_SETUP_INSTANCE_ID(input);
                    FragInputs output;
                    ZERO_INITIALIZE(FragInputs, output);
            
                    // Init to some default value to make the computer quiet (else it output 'divide by zero' warning even if value is not used).
                    // TODO: this is a really poor workaround, but the variable is used in a bunch of places
                    // to compute normals which are then passed on elsewhere to compute other values...
                    output.tangentToWorld = k_identity3x3;
                    output.positionSS = input.pos;       // input.positionCS is SV_Position
            
                    output.positionRWS = GetCameraRelativePositionWS(input.worldPos);
                    output.tangentToWorld = BuildTangentToWorld(input.worldTangent, input.worldNormal);
                    output.texCoord0 = input.texcoord0;
                    output.texCoord1 = input.texcoord1;
                    output.texCoord2 = input.texcoord2;
            
                    return output;
                }
            
               void BuildSurfaceData(FragInputs fragInputs, inout Surface surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData, out float3 bentNormalWS)
               {
                   // setup defaults -- these are used if the graph doesn't output a value
                   ZERO_INITIALIZE(SurfaceData, surfaceData);
        
                   // specularOcclusion need to be init ahead of decal to quiet the compiler that modify the SurfaceData struct
                   // however specularOcclusion can come from the graph, so need to be init here so it can be override.
                   surfaceData.specularOcclusion = 1.0;
        
                   // copy across graph values, if defined
                   surfaceData.baseColor =                 surfaceDescription.Albedo;
                   surfaceData.perceptualSmoothness =      surfaceDescription.Smoothness;
                   surfaceData.ambientOcclusion =          surfaceDescription.Occlusion;
                   surfaceData.specularOcclusion =         surfaceDescription.SpecularOcclusion;
                   surfaceData.metallic =                  surfaceDescription.Metallic;
                   surfaceData.subsurfaceMask =            surfaceDescription.SubsurfaceMask;
                   surfaceData.thickness =                 surfaceDescription.Thickness;
                   surfaceData.diffusionProfileHash =      asuint(surfaceDescription.DiffusionProfileHash);
                   #if _USESPECULAR
                      surfaceData.specularColor =             surfaceDescription.Specular;
                   #endif
                   surfaceData.coatMask =                  surfaceDescription.CoatMask;
                   surfaceData.anisotropy =                surfaceDescription.Anisotropy;
                   surfaceData.iridescenceMask =           surfaceDescription.IridescenceMask;
                   surfaceData.iridescenceThickness =      surfaceDescription.IridescenceThickness;
        
           #ifdef _HAS_REFRACTION
                   if (_EnableSSRefraction)
                   {
                       // surfaceData.ior =                       surfaceDescription.RefractionIndex;
                       // surfaceData.transmittanceColor =        surfaceDescription.RefractionColor;
                       // surfaceData.atDistance =                surfaceDescription.RefractionDistance;
        
                       surfaceData.transmittanceMask = (1.0 - surfaceDescription.Alpha);
                       surfaceDescription.Alpha = 1.0;
                   }
                   else
                   {
                       surfaceData.ior = surfaceDescription.ior;
                       surfaceData.transmittanceColor = surfaceDescription.transmittanceColor;
                       surfaceData.atDistance = surfaceDescription.atDistance;
                       surfaceData.transmittanceMask = surfaceDescription.transmittanceMask;
                       surfaceDescription.Alpha = 1.0;
                   }
           #else
                   surfaceData.ior = 1.0;
                   surfaceData.transmittanceColor = float3(1.0, 1.0, 1.0);
                   surfaceData.atDistance = 1.0;
                   surfaceData.transmittanceMask = 0.0;
           #endif
                
                   // These static material feature allow compile time optimization
                   surfaceData.materialFeatures = MATERIALFEATUREFLAGS_LIT_STANDARD;
           #ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
                   surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SUBSURFACE_SCATTERING;
           #endif
           #ifdef _MATERIAL_FEATURE_TRANSMISSION
                   surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_TRANSMISSION;
           #endif
           #ifdef _MATERIAL_FEATURE_ANISOTROPY
                   surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_ANISOTROPY;
           #endif
                   // surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_CLEAR_COAT;
        
           #ifdef _MATERIAL_FEATURE_IRIDESCENCE
                   surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_IRIDESCENCE;
           #endif
           #ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
                   surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SPECULAR_COLOR;
           #endif
        
           #if defined (_MATERIAL_FEATURE_SPECULAR_COLOR) && defined (_ENERGY_CONSERVING_SPECULAR)
                   // Require to have setup baseColor
                   // Reproduce the energy conservation done in legacy Unity. Not ideal but better for compatibility and users can unchek it
                   surfaceData.baseColor *= (1.0 - Max3(surfaceData.specularColor.r, surfaceData.specularColor.g, surfaceData.specularColor.b));
           #endif

        
                   // tangent-space normal
                   float3 normalTS = float3(0.0f, 0.0f, 1.0f);
                   normalTS = surfaceDescription.Normal;
        
                   // compute world space normal
                   #if !_WORLDSPACENORMAL
                      surfaceData.normalWS = mul(normalTS, fragInputs.tangentToWorld);
                   #else
                      surfaceData.normalWS = normalTS;  
                   #endif
                   surfaceData.geomNormalWS = fragInputs.tangentToWorld[2];
        
                   surfaceData.tangentWS = normalize(fragInputs.tangentToWorld[0].xyz);    // The tangent is not normalize in tangentToWorld for mikkt. TODO: Check if it expected that we normalize with Morten. Tag: SURFACE_GRADIENT
                   // surfaceData.tangentWS = TransformTangentToWorld(surfaceDescription.Tangent, fragInputs.tangentToWorld);
        
           #if HAVE_DECALS
                   if (_EnableDecals)
                   {
                       #if VERSION_GREATER_EQUAL(10,2)
                          DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput,  surfaceData.geomNormalWS, surfaceDescription.Alpha);
                          ApplyDecalToSurfaceData(decalSurfaceData,  surfaceData.geomNormalWS, surfaceData);
                       #else
                          DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, surfaceDescription.Alpha);
                          ApplyDecalToSurfaceData(decalSurfaceData, surfaceData);
                       #endif
                   }
           #endif
        
                   bentNormalWS = surfaceData.normalWS;
               
                   surfaceData.tangentWS = Orthonormalize(surfaceData.tangentWS, surfaceData.normalWS);
        
        
                   // By default we use the ambient occlusion with Tri-ace trick (apply outside) for specular occlusion.
                   // If user provide bent normal then we process a better term
           #if defined(_SPECULAR_OCCLUSION_CUSTOM)
                   // Just use the value passed through via the slot (not active otherwise)
           #elif defined(_SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL)
                   // If we have bent normal and ambient occlusion, process a specular occlusion
                   surfaceData.specularOcclusion = GetSpecularOcclusionFromBentAO(V, bentNormalWS, surfaceData.normalWS, surfaceData.ambientOcclusion, PerceptualSmoothnessToPerceptualRoughness(surfaceData.perceptualSmoothness));
           #elif defined(_AMBIENT_OCCLUSION) && defined(_SPECULAR_OCCLUSION_FROM_AO)
                   surfaceData.specularOcclusion = GetSpecularOcclusionFromAmbientOcclusion(ClampNdotV(dot(surfaceData.normalWS, V)), surfaceData.ambientOcclusion, PerceptualSmoothnessToRoughness(surfaceData.perceptualSmoothness));
           #endif
        
           #ifdef _ENABLE_GEOMETRIC_SPECULAR_AA
                   surfaceData.perceptualSmoothness = GeometricNormalFiltering(surfaceData.perceptualSmoothness, fragInputs.tangentToWorld[2], surfaceDescription.SpecularAAScreenSpaceVariance, surfaceDescription.SpecularAAThreshold);
           #endif
        
           #ifdef DEBUG_DISPLAY
                   if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
                   {
                       // TODO: need to update mip info
                       surfaceData.metallic = 0;
                   }
        
                   // We need to call ApplyDebugToSurfaceData after filling the surfarcedata and before filling builtinData
                   // as it can modify attribute use for static lighting
                   ApplyDebugToSurfaceData(fragInputs.tangentToWorld, surfaceData);
           #endif
               }
        
               void GetSurfaceAndBuiltinData(VertexToPixel m2ps, FragInputs fragInputs, float3 V, inout PositionInputs posInput,
                     out SurfaceData surfaceData, out BuiltinData builtinData, inout Surface l, inout ShaderData d
                     #if NEED_FACING
                        , bool facing
                     #endif
                  )
               {
                 // Removed since crossfade does not work, probably needs extra material setup.   
                 //#ifdef LOD_FADE_CROSSFADE // enable dithering LOD transition if user select CrossFade transition in LOD group
                 //    uint3 fadeMaskSeed = asuint((int3)(V * _ScreenSize.xyx)); // Quantize V to _ScreenSize values
                 //    LODDitheringTransition(fadeMaskSeed, unity_LODFade.x);
                 //#endif
        
                 d = CreateShaderData(m2ps
                  #if NEED_FACING
                    , facing
                  #endif
                 );

                 

                 l = (Surface)0;

                 l.Albedo = half3(0.5, 0.5, 0.5);
                 l.Normal = float3(0,0,1);
                 l.Occlusion = 1;
                 l.Alpha = 1;
                 l.SpecularOcclusion = 1;

                 #ifdef _DEPTHOFFSET_ON
                    l.outputDepth = posInput.deviceDepth;
                 #endif

                 ChainSurfaceFunction(l, d);

                 #ifdef _DEPTHOFFSET_ON
                    posInput.deviceDepth = l.outputDepth;
                 #endif

                 #if _UNLIT
                     //l.Emission = l.Albedo;
                     //l.Albedo = 0;
                     l.Normal = half3(0,0,1);
                     l.Occlusion = 1;
                     l.Metallic = 0;
                     l.Specular = 0;
                 #endif

                 surfaceData.geomNormalWS = d.worldSpaceNormal;
                 surfaceData.tangentWS = d.worldSpaceTangent;
                 fragInputs.tangentToWorld = d.TBNMatrix;

                 float3 bentNormalWS;
                 BuildSurfaceData(fragInputs, l, V, posInput, surfaceData, bentNormalWS);

                 InitBuiltinData(posInput, l.Alpha, bentNormalWS, -d.worldSpaceNormal, fragInputs.texCoord1, fragInputs.texCoord2, builtinData);

                 

                 builtinData.emissiveColor = l.Emission;

                 #if defined(_OVERRIDE_BAKEDGI)
                    builtinData.bakeDiffuseLighting = l.DiffuseGI;
                    builtinData.backBakeDiffuseLighting = l.BackDiffuseGI;
                    builtinData.emissiveColor += l.SpecularGI;
                 #endif
                 
                 #if defined(_OVERRIDE_SHADOWMASK)
                   builtinData.shadowMask0 = l.ShadowMask.x;
                   builtinData.shadowMask1 = l.ShadowMask.y;
                   builtinData.shadowMask2 = l.ShadowMask.z;
                   builtinData.shadowMask3 = l.ShadowMask.w;
                #endif
        
                 #if (SHADERPASS == SHADERPASS_DISTORTION)
                     //builtinData.distortion = surfaceDescription.Distortion;
                     //builtinData.distortionBlur = surfaceDescription.DistortionBlur;
                     builtinData.distortion = float2(0.0, 0.0);
                     builtinData.distortionBlur = 0.0;
                 #else
                     builtinData.distortion = float2(0.0, 0.0);
                     builtinData.distortionBlur = 0.0;
                 #endif
        
                   PostInitBuiltinData(V, posInput, surfaceData, builtinData);
               }
            
                      
          void Frag(PackedVaryingsToPS packedInput,
          #ifdef OUTPUT_SPLIT_LIGHTING
              out float4 outColor : SV_Target0,  // outSpecularLighting
              out float4 outDiffuseLighting : SV_Target1,
              OUTPUT_SSSBUFFER(outSSSBuffer)
          #else
              out float4 outColor : SV_Target0
          #ifdef _WRITE_TRANSPARENT_MOTION_VECTOR
              , out float4 outMotionVec : SV_Target1
          #endif // _WRITE_TRANSPARENT_MOTION_VECTOR
          #endif // OUTPUT_SPLIT_LIGHTING
          #ifdef _DEPTHOFFSET_ON
              , out float outputDepth : SV_Depth
          #endif
          #if NEED_FACING
            , bool facing : SV_IsFrontFace
          #endif
          )
          {
          #ifdef _WRITE_TRANSPARENT_MOTION_VECTOR
              // Init outMotionVector here to solve compiler warning (potentially unitialized variable)
              // It is init to the value of forceNoMotion (with 2.0)
              outMotionVec = float4(2.0, 0.0, 0.0, 0.0);
          #endif

              UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(packedInput);
              FragInputs input = BuildFragInputs(packedInput.vmesh);

              // We need to readapt the SS position as our screen space positions are for a low res buffer, but we try to access a full res buffer.
              input.positionSS.xy = _OffScreenRendering > 0 ? (input.positionSS.xy * _OffScreenDownsampleFactor) : input.positionSS.xy;

              uint2 tileIndex = uint2(input.positionSS.xy) / GetTileSize();

              // input.positionSS is SV_Position
              PositionInputs posInput = GetPositionInput(input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS.xyz, tileIndex);

              float3 V = GetWorldSpaceNormalizeViewDir(input.positionRWS);


              SurfaceData surfaceData;
              BuiltinData builtinData;
              Surface l;
              ShaderData d;
              GetSurfaceAndBuiltinData(packedInput.vmesh, input, V, posInput, surfaceData, builtinData, l, d
               #if NEED_FACING
                  , facing
               #endif
               );

              BSDFData bsdfData = ConvertSurfaceDataToBSDFData(input.positionSS.xy, surfaceData);

              PreLightData preLightData = GetPreLightData(V, posInput, bsdfData);

              outColor = float4(0.0, 0.0, 0.0, 0.0);

              // We need to skip lighting when doing debug pass because the debug pass is done before lighting so some buffers may not be properly initialized potentially causing crashes on PS4.

          #ifdef DEBUG_DISPLAY
              // Init in debug display mode to quiet warning
          #ifdef OUTPUT_SPLIT_LIGHTING
              outDiffuseLighting = 0;
              ENCODE_INTO_SSSBUFFER(surfaceData, posInput.positionSS, outSSSBuffer);
          #endif

              

              // Same code in ShaderPassForwardUnlit.shader
              // Reminder: _DebugViewMaterialArray[i]
              //   i==0 -> the size used in the buffer
              //   i>0  -> the index used (0 value means nothing)
              // The index stored in this buffer could either be
              //   - a gBufferIndex (always stored in _DebugViewMaterialArray[1] as only one supported)
              //   - a property index which is different for each kind of material even if reflecting the same thing (see MaterialSharedProperty)
              bool viewMaterial = false;
              int bufferSize = int(_DebugViewMaterialArray[0]);
              if (bufferSize != 0)
              {
                  bool needLinearToSRGB = false;
                  float3 result = float3(1.0, 0.0, 1.0);

                  // Loop through the whole buffer
                  // Works because GetSurfaceDataDebug will do nothing if the index is not a known one
                  for (int index = 1; index <= bufferSize; index++)
                  {
                      int indexMaterialProperty = int(_DebugViewMaterialArray[index]);

                      // skip if not really in use
                      if (indexMaterialProperty != 0)
                      {
                          viewMaterial = true;

                          GetPropertiesDataDebug(indexMaterialProperty, result, needLinearToSRGB);
                          GetVaryingsDataDebug(indexMaterialProperty, input, result, needLinearToSRGB);
                          GetBuiltinDataDebug(indexMaterialProperty, builtinData, result, needLinearToSRGB);
                          GetSurfaceDataDebug(indexMaterialProperty, surfaceData, result, needLinearToSRGB);
                          GetBSDFDataDebug(indexMaterialProperty, bsdfData, result, needLinearToSRGB);
                      }
                  }

                  // TEMP!
                  // For now, the final blit in the backbuffer performs an sRGB write
                  // So in the meantime we apply the inverse transform to linear data to compensate.
                  if (!needLinearToSRGB)
                      result = SRGBToLinear(max(0, result));

                  outColor = float4(result, 1.0);
              }

              if (!viewMaterial)
              {
                  if (_DebugFullScreenMode == FULLSCREENDEBUGMODE_VALIDATE_DIFFUSE_COLOR || _DebugFullScreenMode == FULLSCREENDEBUGMODE_VALIDATE_SPECULAR_COLOR)
                  {
                      float3 result = float3(0.0, 0.0, 0.0);

                      GetPBRValidatorDebug(surfaceData, result);

                      outColor = float4(result, 1.0f);
                  }
                  else if (_DebugFullScreenMode == FULLSCREENDEBUGMODE_TRANSPARENCY_OVERDRAW)
                  {
                      float4 result = _DebugTransparencyOverdrawWeight * float4(TRANSPARENCY_OVERDRAW_COST, TRANSPARENCY_OVERDRAW_COST, TRANSPARENCY_OVERDRAW_COST, TRANSPARENCY_OVERDRAW_A);
                      outColor = result;
                  }
                  else
          #endif
                  {
          #ifdef _SURFACE_TYPE_TRANSPARENT
                      uint featureFlags = LIGHT_FEATURE_MASK_FLAGS_TRANSPARENT;
          #else
                      uint featureFlags = LIGHT_FEATURE_MASK_FLAGS_OPAQUE;
          #endif

                      float3 diffuseLighting;
                      float3 specularLighting;

                      #if (SHADER_LIBRARY_VERSION_MAJOR >= 10)
                      {
                         LightLoopOutput lightLoopOutput;
                         LightLoop(V, posInput, preLightData, bsdfData, builtinData, featureFlags, lightLoopOutput);

                         // Alias
                         diffuseLighting = lightLoopOutput.diffuseLighting;
                         specularLighting = lightLoopOutput.specularLighting;
                      }
                      #else
                      {
                         LightLoop(V, posInput, preLightData, bsdfData, builtinData, featureFlags, diffuseLighting, specularLighting);
                      }
                      #endif

                      diffuseLighting *= GetCurrentExposureMultiplier();
                      specularLighting *= GetCurrentExposureMultiplier();

          #ifdef OUTPUT_SPLIT_LIGHTING
                      if (_EnableSubsurfaceScattering != 0 && ShouldOutputSplitLighting(bsdfData))
                      {
                          outColor = float4(specularLighting, 1.0);
                          outDiffuseLighting = float4(TagLightingForSSS(diffuseLighting), 1.0);
                      }
                      else
                      {
                          outColor = float4(diffuseLighting + specularLighting, 1.0);
                          outDiffuseLighting = 0;
                      }
                      ENCODE_INTO_SSSBUFFER(surfaceData, posInput.positionSS, outSSSBuffer);
          #else
                      outColor = ApplyBlendMode(diffuseLighting, specularLighting, builtinData.opacity);
                      outColor = EvaluateAtmosphericScattering(posInput, V, outColor);
          #endif

          ChainFinalColorForward(l, d, outColor);

          #ifdef _WRITE_TRANSPARENT_MOTION_VECTOR
                      VaryingsPassToPS inputPass = UnpackVaryingsPassToPS(packedInput.vpass);
                      bool forceNoMotion = any(unity_MotionVectorsParams.yw == 0.0);
                      // outMotionVec is already initialize at the value of forceNoMotion (see above)
                      if (!forceNoMotion)
                      {
                          float2 motionVec = CalculateMotionVector(inputPass.positionCS, inputPass.previousPositionCS);
                          EncodeMotionVector(motionVec * 0.5, outMotionVec);
                          outMotionVec.zw = 1.0;
                      }
          #endif
                  }

          #ifdef DEBUG_DISPLAY
              }
          #endif

          #ifdef _DEPTHOFFSET_ON
              outputDepth = posInput.deviceDepth;
          #endif
          }

            ENDHLSL
        }

               Pass
        {
            // based on HDLitPass.template
            Name "GBuffer"
            Tags { "LightMode" = "GBuffer" }
            //-------------------------------------------------------------------------------------
            // Render Modes (Blend, Cull, ZTest, Stencil, etc)
            //-------------------------------------------------------------------------------------
            
            Cull Back
        
            ZTest [_ZTestGBuffer]
        
            
            
            // Stencil setup
           Stencil
           {
              WriteMask [_StencilWriteMaskGBuffer]
              Ref [_StencilRefGBuffer]
              Comp Always
              Pass Replace
           }
                Cull [_CullMode]

            
            //-------------------------------------------------------------------------------------
            // End Render Modes
            //-------------------------------------------------------------------------------------
        
            HLSLPROGRAM
        
            #pragma target 4.5
            #pragma only_renderers d3d11 ps4 xboxone vulkan metal switch
            //#pragma enable_d3d11_debug_symbols
        
            #pragma multi_compile_instancing
        
            //#pragma multi_compile_local _ _ALPHATEST_ON
        
            //#pragma shader_feature _SURFACE_TYPE_TRANSPARENT
            //#pragma shader_feature_local _ _BLENDMODE_ALPHA _BLENDMODE_ADD _BLENDMODE_PRE_MULTIPLY
        
            //-------------------------------------------------------------------------------------
            // Variant Definitions (active field translations to HDRP defines)
            //-------------------------------------------------------------------------------------
            // #define _MATERIAL_FEATURE_SUBSURFACE_SCATTERING 1
            // #define _MATERIAL_FEATURE_TRANSMISSION 1
            // #define _MATERIAL_FEATURE_ANISOTROPY 1
            // #define _MATERIAL_FEATURE_IRIDESCENCE 1
            // #define _MATERIAL_FEATURE_SPECULAR_COLOR 1
            #define _ENABLE_FOG_ON_TRANSPARENT 1
            // #define _AMBIENT_OCCLUSION 1
            // #define _SPECULAR_OCCLUSION_FROM_AO 1
            // #define _SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL 1
            // #define _SPECULAR_OCCLUSION_CUSTOM 1
            // #define _ENERGY_CONSERVING_SPECULAR 1
            // #define _ENABLE_GEOMETRIC_SPECULAR_AA 1
            // #define _HAS_REFRACTION 1
            // #define _REFRACTION_PLANE 1
            // #define _REFRACTION_SPHERE 1
            // #define _DISABLE_DECALS 1
            // #define _DISABLE_SSR 1
            // #define _ADD_PRECOMPUTED_VELOCITY
            // #define _WRITE_TRANSPARENT_MOTION_VECTOR 1
            // #define _DEPTHOFFSET_ON 1
            // #define _BLENDMODE_PRESERVE_SPECULAR_LIGHTING 1

            #define _PASSGBUFFER 1

            


    #pragma shader_feature_local _ALPHATEST_ON

    #pragma shader_feature_local_fragment _NORMALMAP_TANGENT_SPACE


    #pragma shader_feature_local _NORMALMAP
    #pragma shader_feature_local_fragment _MASKMAP
    #pragma shader_feature_local_fragment _EMISSIVE_COLOR_MAP
    #pragma shader_feature_local_fragment _ANISOTROPYMAP
    #pragma shader_feature_local_fragment _DETAIL_MAP
    #pragma shader_feature_local_fragment _SUBSURFACE_MASK_MAP
    #pragma shader_feature_local_fragment _THICKNESSMAP
    #pragma shader_feature_local_fragment _IRIDESCENCE_THICKNESSMAP
    #pragma shader_feature_local_fragment _SPECULARCOLORMAP

    // MaterialFeature are used as shader feature to allow compiler to optimize properly
    #pragma shader_feature_local_fragment _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
    #pragma shader_feature_local_fragment _MATERIAL_FEATURE_TRANSMISSION
    #pragma shader_feature_local_fragment _MATERIAL_FEATURE_ANISOTROPY
    #pragma shader_feature_local_fragment _MATERIAL_FEATURE_CLEAR_COAT
    #pragma shader_feature_local_fragment _MATERIAL_FEATURE_IRIDESCENCE
    #pragma shader_feature_local_fragment _MATERIAL_FEATURE_SPECULAR_COLOR


    #pragma shader_feature_local_fragment _ENABLESPECULAROCCLUSION
    #pragma shader_feature_local_fragment _ _SPECULAR_OCCLUSION_NONE _SPECULAR_OCCLUSION_FROM_BENT_NORMAL_MAP

    #ifdef _ENABLESPECULAROCCLUSION
        #define _SPECULAR_OCCLUSION_FROM_BENT_NORMAL_MAP
    #endif


        #pragma shader_feature_local_fragment _OBSTRUCTION_CURVE
        #pragma shader_feature_local_fragment _DISSOLVEMASK
        #pragma shader_feature_local_fragment _ZONING
        #pragma shader_feature_local_fragment _REPLACEMENT
        #pragma shader_feature_local_fragment _PLAYERINDEPENDENT


   #define _HDRP 1
#define _USINGTEXCOORD1 1
#define _USINGTEXCOORD2 1


               #pragma vertex Vert
   #pragma fragment Frag
        
            
            //-------------------------------------------------------------------------------------
            // Defines
            //-------------------------------------------------------------------------------------
                #define SHADERPASS SHADERPASS_GBUFFER
                #pragma multi_compile _ DEBUG_DISPLAY
                #pragma multi_compile _ LIGHTMAP_ON
                #pragma multi_compile _ DIRLIGHTMAP_COMBINED
                #pragma multi_compile _ DYNAMICLIGHTMAP_ON
                #pragma multi_compile _ SHADOWS_SHADOWMASK
                #pragma multi_compile DECALS_OFF DECALS_3RT DECALS_4RT
                #pragma multi_compile _ LIGHT_LAYERS
                #define RAYTRACING_SHADER_GRAPH_HIGH
                #define REQUIRE_DEPTH_TEXTURE
                
        
                  // useful conversion functions to make surface shader code just work

      #define UNITY_DECLARE_TEX2D(name) TEXTURE2D(name); SAMPLER(sampler##name);
      #define UNITY_DECLARE_TEX2D_NOSAMPLER(name) TEXTURE2D(name);
      #define UNITY_DECLARE_TEX2DARRAY(name) TEXTURE2D_ARRAY(name); SAMPLER(sampler##name);
      #define UNITY_DECLARE_TEX2DARRAY_NOSAMPLER(tex) TEXTURE2D_ARRAY(tex);

      #define UNITY_SAMPLE_TEX2DARRAY(tex,coord)            SAMPLE_TEXTURE2D_ARRAY(tex, sampler##tex, coord.xy, coord.z)
      #define UNITY_SAMPLE_TEX2DARRAY_LOD(tex,coord,lod)    SAMPLE_TEXTURE2D_ARRAY_LOD(tex, sampler##tex, coord.xy, coord.z, lod)
      #define UNITY_SAMPLE_TEX2D(tex, coord)                SAMPLE_TEXTURE2D(tex, sampler##tex, coord)
      #define UNITY_SAMPLE_TEX2D_SAMPLER(tex, samp, coord)  SAMPLE_TEXTURE2D(tex, sampler##samp, coord)

      #define UNITY_SAMPLE_TEX2D_LOD(tex,coord, lod)   SAMPLE_TEXTURE2D_LOD(tex, sampler_##tex, coord, lod)
      #define UNITY_SAMPLE_TEX2D_SAMPLER_LOD(tex,samplertex,coord, lod) SAMPLE_TEXTURE2D_LOD (tex, sampler##samplertex,coord, lod)

      #if defined(UNITY_COMPILER_HLSL)
         #define UNITY_INITIALIZE_OUTPUT(type,name) name = (type)0;
      #else
         #define UNITY_INITIALIZE_OUTPUT(type,name)
      #endif

      #define sampler2D_float sampler2D
      #define sampler2D_half sampler2D

      #undef WorldNormalVector
      #define WorldNormalVector(data, normal) mul(normal, data.TBNMatrix)

      #define UnityObjectToWorldNormal(normal) mul(GetObjectToWorldMatrix(), normal)




// HDRP Adapter stuff


            // If we use subsurface scattering, enable output split lighting (for forward pass)
            #if defined(_MATERIAL_FEATURE_SUBSURFACE_SCATTERING) && !defined(_SURFACE_TYPE_TRANSPARENT)
            #define OUTPUT_SPLIT_LIGHTING
            #endif

            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Version.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
        
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"
        
            // define FragInputs structure
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
            #if (SHADER_LIBRARY_VERSION_MAJOR >= 10)
               #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/Functions.hlsl"
            #endif


        

            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
        #ifdef DEBUG_DISPLAY
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
        #endif
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
        
        #if (SHADERPASS == SHADERPASS_FORWARD)
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/Lighting.hlsl"
        
            #define HAS_LIGHTLOOP
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/LightLoopDef.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/LightLoop.hlsl"
        #else
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
        #endif
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalUtilities.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitDecalData.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"
        
            // Used by SceneSelectionPass
            int _ObjectId;
            int _PassValue;
        
           
            // data across stages, stripped like the above.
            struct VertexToPixel
            {
               float4 pos : SV_POSITION;
               float3 worldPos : TEXCOORD0;
               float3 worldNormal : TEXCOORD1;
               float4 worldTangent : TEXCOORD2;
               float4 texcoord0 : TEXCOORD3;
               float4 texcoord1 : TEXCOORD4;
               float4 texcoord2 : TEXCOORD5;

               // #if %TEXCOORD3REQUIREKEY%
                float4 texcoord3 : TEXCOORD6;
               // #endif

               // #if %SCREENPOSREQUIREKEY%
               // float4 screenPos : TEXCOORD7;
               // #endif

               // #if %VERTEXCOLORREQUIREKEY%
               // float4 vertexColor : COLOR;
               // #endif

               // #if %EXTRAV2F0REQUIREKEY%
               // float4 extraV2F0 : TEXCOORD8;
               // #endif

               // #if %EXTRAV2F1REQUIREKEY%
               // float4 extraV2F1 : TEXCOORD9;
               // #endif

               // #if %EXTRAV2F2REQUIREKEY%
               // float4 extraV2F2 : TEXCOORD10;
               // #endif

               // #if %EXTRAV2F3REQUIREKEY%
               // float4 extraV2F3 : TEXCOORD11;
               // #endif

               // #if %EXTRAV2F4REQUIREKEY%
               // float4 extraV2F4 : TEXCOORD12;
               // #endif

               // #if %EXTRAV2F5REQUIREKEY%
               // float4 extraV2F5 : TEXCOORD13;
               // #endif

               // #if %EXTRAV2F6REQUIREKEY%
               // float4 extraV2F6 : TEXCOORD14;
               // #endif

               // #if %EXTRAV2F7REQUIREKEY%
               // float4 extraV2F7 : TEXCOORD15;
               // #endif

               #if UNITY_ANY_INSTANCING_ENABLED
                  UNITY_VERTEX_INPUT_INSTANCE_ID
               #endif // UNITY_ANY_INSTANCING_ENABLED

               UNITY_VERTEX_OUTPUT_STEREO
            };



            
            
            // data describing the user output of a pixel
            struct Surface
            {
               half3 Albedo;
               half Height;
               half3 Normal;
               half Smoothness;
               half3 Emission;
               half Metallic;
               half3 Specular;
               half Occlusion;
               half SpecularPower; // for simple lighting
               half Alpha;
               float outputDepth; // if written, SV_Depth semantic is used. ShaderData.clipPos.z is unused value
               // HDRP Only
               half SpecularOcclusion;
               half SubsurfaceMask;
               half Thickness;
               half CoatMask;
               half CoatSmoothness;
               half Anisotropy;
               half IridescenceMask;
               half IridescenceThickness;
               int DiffusionProfileHash;
               float SpecularAAThreshold;
               float SpecularAAScreenSpaceVariance;
               // requires _OVERRIDE_BAKEDGI to be defined, but is mapped in all pipelines
               float3 DiffuseGI;
               float3 BackDiffuseGI;
               float3 SpecularGI;
               float ior;
               float3 transmittanceColor;
               float atDistance;
               float transmittanceMask;
               // requires _OVERRIDE_SHADOWMASK to be defines
               float4 ShadowMask;


            };

            // Data the user declares in blackboard blocks
            struct Blackboard
            {
                
                float blackboardDummyData;
            };

            // data the user might need, this will grow to be big. But easy to strip
            struct ShaderData
            {
               float4 clipPos; // SV_POSITION
               float3 localSpacePosition;
               float3 localSpaceNormal;
               float3 localSpaceTangent;
        
               float3 worldSpacePosition;
               float3 worldSpaceNormal;
               float3 worldSpaceTangent;
               float tangentSign;

               float3 worldSpaceViewDir;
               float3 tangentSpaceViewDir;

               float4 texcoord0;
               float4 texcoord1;
               float4 texcoord2;
               float4 texcoord3;

               float2 screenUV;
               float4 screenPos;

               float4 vertexColor;
               bool isFrontFace;

               float4 extraV2F0;
               float4 extraV2F1;
               float4 extraV2F2;
               float4 extraV2F3;
               float4 extraV2F4;
               float4 extraV2F5;
               float4 extraV2F6;
               float4 extraV2F7;

               float3x3 TBNMatrix;
               Blackboard blackboard;
            };

            struct VertexData
            {
               #if SHADER_TARGET > 30
               // UNITY_VERTEX_INPUT_INSTANCE_ID;
               #endif
               float4 vertex : POSITION;
               float3 normal : NORMAL;
               float4 tangent : TANGENT;
               float4 texcoord0 : TEXCOORD0;

               // optimize out mesh coords when not in use by user or lighting system
               #if _URP && (_USINGTEXCOORD1 || _PASSMETA || _PASSFORWARD || _PASSGBUFFER)
                  float4 texcoord1 : TEXCOORD1;
               #endif

               #if _URP && (_USINGTEXCOORD2 || _PASSMETA || ((_PASSFORWARD || _PASSGBUFFER) && defined(DYNAMICLIGHTMAP_ON)))
                  float4 texcoord2 : TEXCOORD2;
               #endif

               #if _STANDARD && (_USINGTEXCOORD1 || (_PASSMETA || ((_PASSFORWARD || _PASSGBUFFER || _PASSFORWARDADD) && LIGHTMAP_ON)))
                  float4 texcoord1 : TEXCOORD1;
               #endif
               #if _STANDARD && (_USINGTEXCOORD2 || (_PASSMETA || ((_PASSFORWARD || _PASSGBUFFER) && DYNAMICLIGHTMAP_ON)))
                  float4 texcoord2 : TEXCOORD2;
               #endif


               #if _HDRP
                  float4 texcoord1 : TEXCOORD1;
                  float4 texcoord2 : TEXCOORD2;
               #endif

               // #if %TEXCOORD3REQUIREKEY%
                float4 texcoord3 : TEXCOORD3;
               // #endif

               // #if %VERTEXCOLORREQUIREKEY%
               // float4 vertexColor : COLOR;
               // #endif

               #if _HDRP && (_PASSMOTIONVECTOR || ((_PASSFORWARD || _PASSUNLIT) && defined(_WRITE_TRANSPARENT_MOTION_VECTOR)))
                  float3 previousPositionOS : TEXCOORD4; // Contain previous transform position (in case of skinning for example)
                  #if defined (_ADD_PRECOMPUTED_VELOCITY)
                     float3 precomputedVelocity    : TEXCOORD5; // Add Precomputed Velocity (Alembic computes velocities on runtime side).
                  #endif
               #endif

               UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct TessVertex 
            {
               float4 vertex : INTERNALTESSPOS;
               float3 normal : NORMAL;
               float4 tangent : TANGENT;
               float4 texcoord0 : TEXCOORD0;
               float4 texcoord1 : TEXCOORD1;
               float4 texcoord2 : TEXCOORD2;

               // #if %TEXCOORD3REQUIREKEY%
                float4 texcoord3 : TEXCOORD3;
               // #endif

               // #if %VERTEXCOLORREQUIREKEY%
               // float4 vertexColor : COLOR;
               // #endif

               // #if %EXTRAV2F0REQUIREKEY%
               // float4 extraV2F0 : TEXCOORD5;
               // #endif

               // #if %EXTRAV2F1REQUIREKEY%
               // float4 extraV2F1 : TEXCOORD6;
               // #endif

               // #if %EXTRAV2F2REQUIREKEY%
               // float4 extraV2F2 : TEXCOORD7;
               // #endif

               // #if %EXTRAV2F3REQUIREKEY%
               // float4 extraV2F3 : TEXCOORD8;
               // #endif

               // #if %EXTRAV2F4REQUIREKEY%
               // float4 extraV2F4 : TEXCOORD9;
               // #endif

               // #if %EXTRAV2F5REQUIREKEY%
               // float4 extraV2F5 : TEXCOORD10;
               // #endif

               // #if %EXTRAV2F6REQUIREKEY%
               // float4 extraV2F6 : TEXCOORD11;
               // #endif

               // #if %EXTRAV2F7REQUIREKEY%
               // float4 extraV2F7 : TEXCOORD12;
               // #endif

               #if _HDRP && (_PASSMOTIONVECTOR || ((_PASSFORWARD || _PASSUNLIT) && defined(_WRITE_TRANSPARENT_MOTION_VECTOR)))
                  float3 previousPositionOS : TEXCOORD13; // Contain previous transform position (in case of skinning for example)
                  #if defined (_ADD_PRECOMPUTED_VELOCITY)
                     float3 precomputedVelocity : TEXCOORD14;
                  #endif
               #endif

               UNITY_VERTEX_INPUT_INSTANCE_ID
               UNITY_VERTEX_OUTPUT_STEREO
            };

            struct ExtraV2F
            {
               float4 extraV2F0;
               float4 extraV2F1;
               float4 extraV2F2;
               float4 extraV2F3;
               float4 extraV2F4;
               float4 extraV2F5;
               float4 extraV2F6;
               float4 extraV2F7;
               Blackboard blackboard;
               float4 time;
            };


            float3 WorldToTangentSpace(ShaderData d, float3 normal)
            {
               return mul(d.TBNMatrix, normal);
            }

            float3 TangentToWorldSpace(ShaderData d, float3 normal)
            {
               return mul(normal, d.TBNMatrix);
            }

            // in this case, make standard more like SRPs, because we can't fix
            // unity_WorldToObject in HDRP, since it already does macro-fu there

            #if _STANDARD
               float3 TransformWorldToObject(float3 p) { return mul(unity_WorldToObject, float4(p, 1)); };
               float3 TransformObjectToWorld(float3 p) { return mul(unity_ObjectToWorld, float4(p, 1)); };
               float4 TransformWorldToObject(float4 p) { return mul(unity_WorldToObject, p); };
               float4 TransformObjectToWorld(float4 p) { return mul(unity_ObjectToWorld, p); };
               float4x4 GetWorldToObjectMatrix() { return unity_WorldToObject; }
               float4x4 GetObjectToWorldMatrix() { return unity_ObjectToWorld; }
               #if (defined(SHADER_API_D3D11) || defined(SHADER_API_XBOXONE) || defined(UNITY_COMPILER_HLSLCC) || defined(SHADER_API_PSSL) || (SHADER_TARGET_SURFACE_ANALYSIS && !SHADER_TARGET_SURFACE_ANALYSIS_MOJOSHADER))
                 #define UNITY_SAMPLE_TEX2D_LOD(tex,coord, lod) tex.SampleLevel (sampler##tex,coord, lod)
                 #define UNITY_SAMPLE_TEX2D_SAMPLER_LOD(tex,samplertex,coord, lod) tex.SampleLevel (sampler##samplertex,coord, lod)
              #else
                 #define UNITY_SAMPLE_TEX2D_LOD(tex,coord,lod) tex2D (tex,coord,0,lod)
                 #define UNITY_SAMPLE_TEX2D_SAMPLER_LOD(tex,samplertex,coord,lod) tex2D (tex,coord,0,lod)
              #endif

               #undef GetWorldToObjectMatrix()

               #define GetWorldToObjectMatrix()   unity_WorldToObject


            #endif

            float3 GetCameraWorldPosition()
            {
               #if _HDRP
                  return GetCameraRelativePositionWS(_WorldSpaceCameraPos);
               #else
                  return _WorldSpaceCameraPos;
               #endif
            }

            #if _GRABPASSUSED
               #if _STANDARD
                  TEXTURE2D(%GRABTEXTURE%);
                  SAMPLER(sampler_%GRABTEXTURE%);
               #endif

               half3 GetSceneColor(float2 uv)
               {
                  #if _STANDARD
                     return SAMPLE_TEXTURE2D(%GRABTEXTURE%, sampler_%GRABTEXTURE%, uv).rgb;
                  #else
                     return SHADERGRAPH_SAMPLE_SCENE_COLOR(uv);
                  #endif
               }
            #endif


      
            #if _STANDARD
               UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);
               float GetSceneDepth(float2 uv) { return SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, uv); }
               float GetLinear01Depth(float2 uv) { return Linear01Depth(GetSceneDepth(uv)); }
               float GetLinearEyeDepth(float2 uv) { return LinearEyeDepth(GetSceneDepth(uv)); } 
            #else
               float GetSceneDepth(float2 uv) { return SHADERGRAPH_SAMPLE_SCENE_DEPTH(uv); }
               float GetLinear01Depth(float2 uv) { return Linear01Depth(GetSceneDepth(uv), _ZBufferParams); }
               float GetLinearEyeDepth(float2 uv) { return LinearEyeDepth(GetSceneDepth(uv), _ZBufferParams); } 
            #endif

            float3 GetWorldPositionFromDepthBuffer(float2 uv, float3 worldSpaceViewDir)
            {
               float eye = GetLinearEyeDepth(uv);
               float3 camView = mul((float3x3)GetObjectToWorldMatrix(), transpose(mul(GetWorldToObjectMatrix(), UNITY_MATRIX_I_V)) [2].xyz);

               float dt = dot(worldSpaceViewDir, camView);
               float3 div = worldSpaceViewDir/dt;
               float3 wpos = (eye * div) + GetCameraWorldPosition();
               return wpos;
            }

            #if _HDRP
            float3 ObjectToWorldSpacePosition(float3 pos)
            {
               return GetAbsolutePositionWS(TransformObjectToWorld(pos));
            }
            #else
            float3 ObjectToWorldSpacePosition(float3 pos)
            {
               return TransformObjectToWorld(pos);
            }
            #endif

            #if _STANDARD
               UNITY_DECLARE_SCREENSPACE_TEXTURE(_CameraDepthNormalsTexture);
               float3 GetSceneNormal(float2 uv, float3 worldSpaceViewDir)
               {
                  float4 depthNorms = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_CameraDepthNormalsTexture, uv);
                  float3 norms = DecodeViewNormalStereo(depthNorms);
                  norms = mul((float3x3)GetWorldToViewMatrix(), norms) * 0.5 + 0.5;
                  return norms;
               }
            #elif _HDRP
               float3 GetSceneNormal(float2 uv, float3 worldSpaceViewDir)
               {
                  NormalData nd;
                  DecodeFromNormalBuffer(_ScreenSize.xy * uv, nd);
                  return nd.normalWS;
               }
            #elif _URP
               #if (SHADER_LIBRARY_VERSION_MAJOR >= 10)
                  #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareNormalsTexture.hlsl"
               #endif

               float3 GetSceneNormal(float2 uv, float3 worldSpaceViewDir)
               {
                  #if (SHADER_LIBRARY_VERSION_MAJOR >= 10)
                     return SampleSceneNormals(uv);
                  #else
                     float3 wpos = GetWorldPositionFromDepthBuffer(uv, worldSpaceViewDir);
                     return normalize(-cross(ddx(wpos), ddy(wpos))) * 0.5 + 0.5;
                  #endif

                }
             #endif

             #if _HDRP

               half3 UnpackNormalmapRGorAG(half4 packednormal)
               {
                     // This do the trick
                  packednormal.x *= packednormal.w;

                  half3 normal;
                  normal.xy = packednormal.xy * 2 - 1;
                  normal.z = sqrt(1 - saturate(dot(normal.xy, normal.xy)));
                  return normal;
               }
               half3 UnpackNormal(half4 packednormal)
               {
                  #if defined(UNITY_NO_DXT5nm)
                     return packednormal.xyz * 2 - 1;
                  #else
                     return UnpackNormalmapRGorAG(packednormal);
                  #endif
               }
               #endif
               #if _HDRP || _URP

               half3 UnpackScaleNormal(half4 packednormal, half scale)
               {
                 #ifndef UNITY_NO_DXT5nm
                   // Unpack normal as DXT5nm (1, y, 1, x) or BC5 (x, y, 0, 1)
                   // Note neutral texture like "bump" is (0, 0, 1, 1) to work with both plain RGB normal and DXT5nm/BC5
                   packednormal.x *= packednormal.w;
                 #endif
                   half3 normal;
                   normal.xy = (packednormal.xy * 2 - 1) * scale;
                   normal.z = sqrt(1 - saturate(dot(normal.xy, normal.xy)));
                   return normal;
               }	

             #endif


            void GetSun(out float3 lightDir, out float3 color)
            {
               lightDir = float3(0.5, 0.5, 0);
               color = 1;
               #if _HDRP
                  if (_DirectionalLightCount > 0)
                  {
                     DirectionalLightData light = _DirectionalLightDatas[0];
                     lightDir = -light.forward.xyz;
                     color = light.color;
                  }
               #elif _STANDARD
			         lightDir = normalize(_WorldSpaceLightPos0.xyz);
                  color = _LightColor0.rgb;
               #elif _URP
	               Light light = GetMainLight();
	               lightDir = light.direction;
	               color = light.color;
               #endif
            }




            CBUFFER_START(UnityPerMaterial)
               float _StencilRef;
               float _StencilWriteMask;
               float _StencilRefDepth;
               float _StencilWriteMaskDepth;
               float _StencilRefMV;
               float _StencilWriteMaskMV;
               float _StencilRefDistortionVec;
               float _StencilWriteMaskDistortionVec;
               float _StencilWriteMaskGBuffer;
               float _StencilRefGBuffer;
               float _ZTestGBuffer;
               float _RequireSplitLighting;
               float _ReceivesSSR;
               float _ZWrite;
               float _TransparentSortPriority;
               float _ZTestDepthEqualForOpaque;
               float _ZTestTransparent;
               float _TransparentBackfaceEnable;
               float _AlphaCutoffEnable;
               float _UseShadowThreshold;

               
    float4 _BaseColorMap_ST;
    float4 _DetailMap_ST;
    float4 _EmissiveColorMap_ST;

    float _UVBase;
    half4 _Color;
    half4 _BaseColor; // TODO CHECK
    half _Cutoff; 
    half _Mode;
    //float _Cull;
    float _CullMode;
    float _CullModeForward;
    half _Metallic;
    half3 _EmissionColor;
    half _UVSec;

    float3 _EmissiveColor;
    float _AlbedoAffectEmissive;
    float _EmissiveExposureWeight;
    float _MetallicRemapMin;
    float _MetallicRemapMax;
    float _Smoothness;
    float _SmoothnessRemapMin;
    float _SmoothnessRemapMax;
    float _AlphaRemapMin;
    float _AlphaRemapMax;
    float _AORemapMin;
    float _AORemapMax;
    float _NormalScale;
    float _DetailAlbedoScale;
    float _DetailNormalScale;
    float _DetailSmoothnessScale;
    float _Anisotropy;
    float _DiffusionProfileHash;
    float _SubsurfaceMask;
    float _Thickness;
    float4 _ThicknessRemap;
    float _IridescenceThickness;
    float4 _IridescenceThicknessRemap;
    float _IridescenceMask;
    float _CoatMask;
    float4 _SpecularColor;
    float _EnergyConservingSpecularColor;
    int _MaterialID;

    float _LinkDetailsWithBase;
    float4 _UVMappingMask;
    float4 _UVDetailsMappingMask;

    float4 _UVMappingMaskEmissive;

    float _AlphaCutoff;
    //float _AlphaCutoffEnable;


    float _SyncCullMode;

    float _IsReplacementShader;
    float _TriggerMode;
    float _RaycastMode;
    float _IsExempt;
    float _isReferenceMaterial;
    float _InteractionMode;
    int _ArrayLength = 0;
    #if (defined(SHADER_API_GLES) || defined(SHADER_API_GLES3)) 
        float4 _PlayersPosVectorArray[20];
        float _PlayersDataFloatArray[150];     
    #else
        float4 _PlayersPosVectorArray[100];
        float _PlayersDataFloatArray[500];  
    #endif
    float _tDirection = 0;
    float _numOfPlayersInside = 0;
    float _tValue = 0;
    float _id = 0;
    #if _ZONING
        #if (defined(SHADER_API_GLES) || defined(SHADER_API_GLES3)) 
            float _ZDFA[500];
        #else
            float _ZDFA[1000];
        #endif
        float _ZonesDataCount;
    #endif
    #if _REPLACEMENT        
        half4 _DissolveColorGlobal;
        float _DissolveColorSaturationGlobal;
        float _DissolveEmissionGlobal;
        float _DissolveEmissionBoosterGlobal;
        float _TextureVisibilityGlobal;
        float _ObstructionGlobal;
        float _AngleStrengthGlobal;
        float _ConeStrengthGlobal;
        float _ConeObstructionDestroyRadiusGlobal;
        float _CylinderStrengthGlobal;
        float _CylinderObstructionDestroyRadiusGlobal;
        float _CircleStrengthGlobal;
        float _CircleObstructionDestroyRadiusGlobal;
        float _CurveStrengthGlobal;
        float _CurveObstructionDestroyRadiusGlobal;
        float _DissolveFallOffGlobal;
        float _IntrinsicDissolveStrengthGlobal;
        float _PreviewModeGlobal;
        float _UVsGlobal;
        float _hasClippedShadowsGlobal;
        float _FloorGlobal;
        float _FloorModeGlobal;
        float _FloorYGlobal;
        float _PlayerPosYOffsetGlobal;
        float _FloorYTextureGradientLengthGlobal;
        float _AnimationEnabledGlobal;
        float _AnimationSpeedGlobal;
        float _DefaultEffectRadiusGlobal;
        float _TransitionDurationGlobal;        
        float _TexturedEmissionEdgeGlobal;
        float _TexturedEmissionEdgeStrengthGlobal;
        float _IsometricExclusionGlobal;
        float _IsometricExclusionDistanceGlobal;
        float _IsometricExclusionGradientLengthGlobal;
        float _CeilingGlobal;
        float _CeilingModeGlobal;
        float _CeilingBlendModeGlobal;
        float _CeilingYGlobal;
        float _CeilingPlayerYOffsetGlobal;
        float _CeilingYGradientLengthGlobal;
        float _ZoningGlobal;
        float _ZoningModeGlobal;
        float _ZoningEdgeGradientLengthGlobal;
        float _IsZoningRevealableGlobal;
        float _SyncZonesWithFloorYGlobal;
        float _SyncZonesFloorYOffsetGlobal;
        float4 _ObstructionCurveGlobal_TexelSize;
        float4 _DissolveMaskGlobal_TexelSize;
        float _DissolveMaskEnabledGlobal;
        float _PreviewIndicatorLineThicknessGlobal;
    #else
        half _TextureVisibility;
        half _AngleStrength;
        float _Obstruction;
        float _UVs;
        float4 _ObstructionCurve_TexelSize;      
        float _DissolveMaskEnabled;
        float4 _DissolveMask_TexelSize;
        half4 _DissolveColor;
        float _DissolveColorSaturation;
        float _DissolveEmission;
        float _DissolveEmissionBooster;
        float _hasClippedShadows;
        float _ConeStrength;
        float _ConeObstructionDestroyRadius;
        float _CylinderStrength;
        float _CylinderObstructionDestroyRadius;
        float _CircleStrength;
        float _CircleObstructionDestroyRadius;
        float _CurveStrength;
        float _CurveObstructionDestroyRadius;
        float _IntrinsicDissolveStrength;
        float _DissolveFallOff;
        float _PreviewMode;
        float _PreviewIndicatorLineThickness;
        float _AnimationEnabled;
        float _AnimationSpeed;
        float _DefaultEffectRadius;
        float _TransitionDuration;
        float _TexturedEmissionEdge;
        float _TexturedEmissionEdgeStrength;
        float _IsometricExclusion;
        float _IsometricExclusionDistance;
        float _IsometricExclusionGradientLength;
        float _Floor;
        float _FloorMode;
        float _FloorY;
        float _FloorYTextureGradientLength;
        float _PlayerPosYOffset;
        float _Ceiling;
        float _CeilingMode;
        float _CeilingBlendMode;
        float _CeilingY;
        float _CeilingPlayerYOffset;
        float _CeilingYGradientLength;
        float _Zoning;
        float _ZoningMode;
        float _ZoningEdgeGradientLength;
        float _IsZoningRevealable;
        float _SyncZonesWithFloorY;
        float _SyncZonesFloorYOffset;
    #endif



            CBUFFER_END

            

            

            #ifdef unity_WorldToObject
#undef unity_WorldToObject
#endif
#ifdef unity_ObjectToWorld
#undef unity_ObjectToWorld
#endif
#define unity_ObjectToWorld GetObjectToWorldMatrix()
#define unity_WorldToObject GetWorldToObjectMatrix()

    sampler2D _EmissiveColorMap;
    sampler2D _BaseColorMap;
    sampler2D _MaskMap;
    sampler2D _NormalMap;
    sampler2D _NormalMapOS;
    sampler2D _DetailMap;
    sampler2D _TangentMap;
    sampler2D _TangentMapOS;
    sampler2D _AnisotropyMap;
    sampler2D _SubsurfaceMaskMap;
    sampler2D _TransmissionMaskMap;
    sampler2D _ThicknessMap;
    sampler2D _IridescenceThicknessMap;
    sampler2D _IridescenceMaskMap;
    sampler2D _SpecularColorMap;

    sampler2D _CoatMaskMap;

    float3 DecodeNormal (float4 sample, float scale) {
        #if defined(UNITY_NO_DXT5nm)
            return UnpackNormalRGB(sample, scale);
        #else
            return UnpackNormalmapRGorAG(sample, scale);
        #endif
    }

	void Ext_SurfaceFunction0 (inout Surface o, ShaderData d)
	{


//SEE: https://github.com/Unity-Technologies/Graphics/blob/6fdc7996098aa184495c0a3c0da10135bfe18340/Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitDataIndividualLayer.hlsl

        float2 uvBase = _UVMappingMask.x * d.texcoord0 +
                        _UVMappingMask.y * d.texcoord1 +
                        _UVMappingMask.z * d.texcoord2 +
                        _UVMappingMask.w * d.texcoord3;

        float2 uvDetails =  _UVDetailsMappingMask.x * d.texcoord0 +
                            _UVDetailsMappingMask.y * d.texcoord1 +
                            _UVDetailsMappingMask.z * d.texcoord2 +
                            _UVDetailsMappingMask.w * d.texcoord3;


        //TODO: add planar/triplanar

        float4 texcoords;
        //texcoords.xy = d.texcoord0.xy * _BaseColorMap_ST.xy + _BaseColorMap_ST.zw; 
        texcoords.xy = uvBase * _BaseColorMap_ST.xy + _BaseColorMap_ST.zw; 

        texcoords.zw = uvDetails * _DetailMap_ST.xy + _DetailMap_ST.zw;

        if (_LinkDetailsWithBase > 0.0)
        {
            texcoords.zw = texcoords.zw  * _BaseColorMap_ST.xy + _BaseColorMap_ST.zw;

        }

            float3 detailNormalTS = float3(0.0, 0.0, 0.0);
            float detailMask = 0.0;
            #ifdef _DETAIL_MAP
                detailMask = 1.0;
                #ifdef _MASKMAP
                    detailMask = tex2D(_MaskMap, texcoords.xy).b;
                #endif
                float2 detailAlbedoAndSmoothness = tex2D(_DetailMap, texcoords.zw).rb;
                float detailAlbedo = detailAlbedoAndSmoothness.r * 2.0 - 1.0;
                float detailSmoothness = detailAlbedoAndSmoothness.g * 2.0 - 1.0;
                real4 packedNormal = tex2D(_DetailMap, texcoords.zw).rgba;
                detailNormalTS = UnpackNormalAG(packedNormal, _DetailNormalScale);
            #endif
            float4 color = tex2D(_BaseColorMap, texcoords.xy).rgba  * _Color.rgba;
            o.Albedo = color; 
            float alpha = color.a;
            alpha = lerp(_AlphaRemapMin, _AlphaRemapMax, alpha);
            o.Alpha = alpha;
            #ifdef _DETAIL_MAP
                float albedoDetailSpeed = saturate(abs(detailAlbedo) * _DetailAlbedoScale);
                float3 baseColorOverlay = lerp(sqrt(o.Albedo), (detailAlbedo < 0.0) ? float3(0.0, 0.0, 0.0) : float3(1.0, 1.0, 1.0), albedoDetailSpeed * albedoDetailSpeed);
                baseColorOverlay *= baseColorOverlay;
                o.Albedo = lerp(o.Albedo, saturate(baseColorOverlay), detailMask);
            #endif

            #ifdef _NORMALMAP
                float3 normalTS;

                //normalTS = UnpackScaleNormal(tex2D(_NormalMap, texcoords.xy), _NormalScale);
                //#ifdef _DETAIL_MAP
                //    normalTS = lerp(normalTS, BlendNormalRNM(normalTS, normalize(detailNormalTS)), detailMask); 
                //#endif
                //o.Normal = normalTS;


                #ifdef _NORMALMAP_TANGENT_SPACE
                    //normalTS = UnpackScaleNormal(tex2D(_NormalMap, texcoords.xy), _NormalScale);
                    normalTS = DecodeNormal(tex2D(_NormalMap, texcoords.xy), _NormalScale);
                #else
                    #ifdef SURFACE_GRADIENT
                        float3 normalOS = tex2D(_NormalMapOS, texcoords.xy).xyz * 2.0 - 1.0;
                        normalTS = SurfaceGradientFromPerturbedNormal(d.worldSpaceNormal, TransformObjectToWorldNormal(normalOS));
                    #else
                        float3 normalOS = UnpackNormalRGB(tex2D(_NormalMapOS, texcoords.xy), 1.0);
                        float3 bitangent = d.tangentSign * cross(d.worldSpaceNormal.xyz, d.worldSpaceTangent.xyz);
                        half3x3 tangentToWorld = half3x3(d.worldSpaceTangent.xyz, bitangent.xyz, d.worldSpaceNormal.xyz);
                        normalTS = TransformObjectToTangent(normalOS, tangentToWorld);
                    #endif
                #endif

                #ifdef _DETAIL_MAP
                    #ifdef SURFACE_GRADIENT
                    normalTS += detailNormalTS * detailMask;
                    #else
                    normalTS = lerp(normalTS, BlendNormalRNM(normalTS, normalize(detailNormalTS)), detailMask); // todo: detailMask should lerp the angle of the quaternion rotation, not the normals
                    #endif
                #endif
                o.Normal = normalTS;

            #endif

            #if defined(_MASKMAP)
                o.Smoothness = tex2D(_MaskMap, texcoords.xy).a;
                o.Smoothness = lerp(_SmoothnessRemapMin, _SmoothnessRemapMax, o.Smoothness);
            #else
                o.Smoothness = _Smoothness;
            #endif
            #ifdef _DETAIL_MAP
                float smoothnessDetailSpeed = saturate(abs(detailSmoothness) * _DetailSmoothnessScale);
                float smoothnessOverlay = lerp(o.Smoothness, (detailSmoothness < 0.0) ? 0.0 : 1.0, smoothnessDetailSpeed);
                o.Smoothness = lerp(o.Smoothness, saturate(smoothnessOverlay), detailMask);
            #endif
            #ifdef _MASKMAP
                o.Metallic = tex2D(_MaskMap, texcoords.xy).r;
                o.Metallic = lerp(_MetallicRemapMin, _MetallicRemapMax, o.Metallic);
                o.Occlusion = tex2D(_MaskMap, texcoords.xy).g;
                o.Occlusion = lerp(_AORemapMin, _AORemapMax, o.Occlusion);
            #else
                o.Metallic = _Metallic;
                o.Occlusion = 1.0;
            #endif

            #if defined(_SPECULAR_OCCLUSION_FROM_BENT_NORMAL_MAP)
                // TODO: implenent bent normals for this to work
                //o.SpecularOcclusion = GetSpecularOcclusionFromBentAO(V, bentNormalWS, d.worldSpaceNormal, o.Occlusion, PerceptualSmoothnessToRoughness(o.Smoothness));
            #elif  defined(_MASKMAP) && !defined(_SPECULAR_OCCLUSION_NONE)
                float3 V = normalize(d.worldSpaceViewDir);
                o.SpecularOcclusion = GetSpecularOcclusionFromAmbientOcclusion(ClampNdotV(dot(d.worldSpaceNormal, V)), o.Occlusion, PerceptualSmoothnessToRoughness(o.Smoothness));
            #endif

            o.DiffusionProfileHash = asuint(_DiffusionProfileHash);
            o.SubsurfaceMask = _SubsurfaceMask;
            #ifdef _SUBSURFACE_MASK_MAP
                o.SubsurfaceMask *= tex2D(_SubsurfaceMaskMap, texcoords.xy).r;
            #endif
            #ifdef _THICKNESSMAP
                o.Thickness = tex2D(_ThicknessMap, texcoords.xy).r;
                o.Thickness = _ThicknessRemap.x + _ThicknessRemap.y * surfaceData.thickness;
            #else
                o.Thickness = _Thickness;
            #endif
            #ifdef _ANISOTROPYMAP
                o.Anisotropy = tex2D(_AnisotropyMap, texcoords.xy).r;
            #else
                o.Anisotropy = 1.0;
            #endif
                o.Anisotropy *= _Anisotropy;
                o.Specular = _SpecularColor.rgb;
            #ifdef _SPECULARCOLORMAP
                o.Specular *= tex2D(_SpecularColorMap, texcoords.xy).rgb;
            #endif
            #ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
                o.Albedo *= _EnergyConservingSpecularColor > 0.0 ? (1.0 - Max3(o.Specular.r, o.Specular.g, o.Specular.b)) : 1.0;
            #endif
            #ifdef _MATERIAL_FEATURE_CLEAR_COAT
                o.CoatMask = _CoatMask;
                o.CoatMask *= tex2D(_CoatMaskMap, texcoords.xy).r;
            #else
                o.CoatMask = 0.0;
            #endif
            #ifdef _MATERIAL_FEATURE_IRIDESCENCE
                #ifdef _IRIDESCENCE_THICKNESSMAP
                o.IridescenceThickness = tex2D(_IridescenceThicknessMap, texcoords.xy).r;
                o.IridescenceThickness = _IridescenceThicknessRemap.x + _IridescenceThicknessRemap.y * o.IridescenceThickness;
                #else
                o.IridescenceThickness = _IridescenceThickness;
                #endif
                o.IridescenceMask = _IridescenceMask;
                o.IridescenceMask *= tex2D(_IridescenceMaskMap, texcoords.xy).r;
            #else
                o.IridescenceThickness = 0.0;
                o.IridescenceMask = 0.0;
            #endif


//SEE: https://github.com/Unity-Technologies/Graphics/blob/632f80e011f18ea537ee6e2f0be3ff4f4dea6a11/Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitBuiltinData.hlsl

            float3 emissiveColor = _EmissiveColor * lerp(float3(1.0, 1.0, 1.0), o.Albedo.rgb, _AlbedoAffectEmissive);
            #ifdef _EMISSIVE_COLOR_MAP                
                float2 uvEmission = _UVMappingMaskEmissive.x * d.texcoord0 +
                                    _UVMappingMaskEmissive.y * d.texcoord1 +
                                    _UVMappingMaskEmissive.z * d.texcoord2 +
                                    _UVMappingMaskEmissive.w * d.texcoord3;

                uvEmission = uvEmission * _EmissiveColorMap_ST.xy + _EmissiveColorMap_ST.zw;                     

                emissiveColor *= tex2D(_EmissiveColorMap, uvEmission).rgb;
            #endif
            o.Emission += emissiveColor;
            float currentExposureMultiplier = 0;
            #if SHADEROPTIONS_PRE_EXPOSITION
                currentExposureMultiplier =  LOAD_TEXTURE2D(_ExposureTexture, int2(0, 0)).x * _ProbeExposureScale;
            #else
                currentExposureMultiplier =  _ProbeExposureScale;
            #endif
            float InverseCurrentExposureMultiplier = 0;
            float exposure = currentExposureMultiplier;
            InverseCurrentExposureMultiplier =  rcp(exposure + (exposure == 0.0)); 
            float3 emissiveRcpExposure = o.Emission * InverseCurrentExposureMultiplier;
            o.Emission = lerp(emissiveRcpExposure, o.Emission, _EmissiveExposureWeight);


// SEE: https://github.com/Unity-Technologies/Graphics/blob/223d8105e68c5a808027d78f39cb4e2545fd6276/Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitData.hlsl
            #if defined(_ALPHATEST_ON)
                float alphaTex = tex2D(_BaseColorMap, texcoords.xy).a;
                alphaTex = lerp(_AlphaRemapMin, _AlphaRemapMax, alphaTex);
                float alphaValue = alphaTex * _BaseColor.a;

                // Perform alha test very early to save performance (a killed pixel will not sample textures)
                //#if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
                //float alphaCutoff = _AlphaCutoffPrepass;
                //#elif SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_POSTPASS
                //float alphaCutoff = _AlphaCutoffPostpass;
                //#elif (SHADERPASS == SHADERPASS_SHADOWS) || (SHADERPASS == SHADERPASS_RAYTRACING_VISIBILITY)
                //float alphaCutoff = _UseShadowThreshold ? _AlphaCutoffShadow : _AlphaCutoff;
                //#else
                float alphaCutoff = _AlphaCutoff;
                //#endif

                // GENERIC_ALPHA_TEST(alphaValue, alphaCutoff);
                clip(alpha - alphaCutoff);
            #endif

	}




    #if _REPLACEMENT
        sampler2D _DissolveTexGlobal;
    #else
        sampler2D _DissolveTex;
    #endif

    //#if _DISSOLVEMASK
    #if _REPLACEMENT
        sampler2D _DissolveMaskGlobal;
    #else
        sampler2D _DissolveMask;
    #endif
    //#endif

    #if _REPLACEMENT
        sampler2D _ObstructionCurveGlobal;
    #else
        sampler2D _ObstructionCurve;
    #endif



    #if _URP
        half2 ParallaxOffset1Step (half h, half height, half3 viewDir)
        {
            h = h * height - height/2.0;
            half3 v = normalize(viewDir);
            v.z += 0.42;
            return h * (v.xy / v.z);
        }
        #ifdef UNITY_COLORSPACE_GAMMA
            #define unity_ColorSpaceDouble half4(2.0, 2.0, 2.0, 2.0)
        #else 
            #define unity_ColorSpaceDouble half4(4.59479380, 4.59479380, 4.59479380, 2.0)
        #endif
        half3 BlendNormals(half3 n1, half3 n2)
        {
            return normalize(half3(n1.xy + n2.xy, n1.z*n2.z));
        }
    #endif


#ifdef USE_UNITY_TEXTURE_2D_TYPE
#undef USE_UNITY_TEXTURE_2D_TYPE
#endif
#include "Assets/See-through Shader/Core/Shaders/xxSharedSTSDependencies/SeeThroughShaderFunction.hlsl"



	void Ext_SurfaceFunction1 (inout Surface o, ShaderData d)
	{
        half3 albedo;
        half3 emission;
        float alphaForClipping;
#ifdef USE_UNITY_TEXTURE_2D_TYPE
#undef USE_UNITY_TEXTURE_2D_TYPE
#endif
#if _REPLACEMENT
    DoSeeThroughShading( o.Albedo, o.Normal, d.worldSpacePosition, d.worldSpaceNormal,

                        _numOfPlayersInside, _tDirection, _tValue, _id,
                        _TriggerMode, _RaycastMode,
                        _IsExempt,

                        _DissolveColorGlobal, _DissolveColorSaturationGlobal, _UVsGlobal,
                        _DissolveEmissionGlobal, _DissolveEmissionBoosterGlobal, _TexturedEmissionEdgeGlobal, _TexturedEmissionEdgeStrengthGlobal,
                        _hasClippedShadowsGlobal,

                        _ObstructionGlobal,
                        _AngleStrengthGlobal,
                        _ConeStrengthGlobal, _ConeObstructionDestroyRadiusGlobal,
                        _CylinderStrengthGlobal, _CylinderObstructionDestroyRadiusGlobal,
                        _CircleStrengthGlobal, _CircleObstructionDestroyRadiusGlobal,
                        _CurveStrengthGlobal, _CurveObstructionDestroyRadiusGlobal,
                        _DissolveFallOffGlobal,
                        _DissolveMaskEnabledGlobal,
                        _IntrinsicDissolveStrengthGlobal,
                        _DefaultEffectRadiusGlobal,

                        _IsometricExclusionGlobal, _IsometricExclusionDistanceGlobal, _IsometricExclusionGradientLengthGlobal,
                        _CeilingGlobal, _CeilingModeGlobal, _CeilingBlendModeGlobal, _CeilingYGlobal, _CeilingPlayerYOffsetGlobal, _CeilingYGradientLengthGlobal,
                        _FloorGlobal, _FloorModeGlobal, _FloorYGlobal, _PlayerPosYOffsetGlobal, _FloorYTextureGradientLengthGlobal,

                        _AnimationEnabledGlobal, _AnimationSpeedGlobal,
                        _TransitionDurationGlobal,

                        _ZoningGlobal, _ZoningModeGlobal, _IsZoningRevealableGlobal, _ZoningEdgeGradientLengthGlobal,
                        _SyncZonesWithFloorYGlobal, _SyncZonesFloorYOffsetGlobal,

                        _PreviewModeGlobal,
                        _PreviewIndicatorLineThicknessGlobal,

                        _DissolveTexGlobal,
                        _DissolveMaskGlobal,
                        _ObstructionCurveGlobal,

                        _DissolveMaskGlobal_TexelSize,
                        _ObstructionCurveGlobal_TexelSize,

                        albedo, emission, alphaForClipping);
#else 
    
    DoSeeThroughShading( o.Albedo, o.Normal, d.worldSpacePosition, d.worldSpaceNormal,

                        _numOfPlayersInside, _tDirection, _tValue, _id,
                        _TriggerMode, _RaycastMode,
                        _IsExempt,

                        _DissolveColor, _DissolveColorSaturation, _UVs,
                        _DissolveEmission, _DissolveEmissionBooster, _TexturedEmissionEdge, _TexturedEmissionEdgeStrength,
                        _hasClippedShadows,

                        _Obstruction,
                        _AngleStrength,
                        _ConeStrength, _ConeObstructionDestroyRadius,
                        _CylinderStrength, _CylinderObstructionDestroyRadius,
                        _CircleStrength, _CircleObstructionDestroyRadius,
                        _CurveStrength, _CurveObstructionDestroyRadius,
                        _DissolveFallOff,
                        _DissolveMaskEnabled,
                        _IntrinsicDissolveStrength,
                        _DefaultEffectRadius,

                        _IsometricExclusion, _IsometricExclusionDistance, _IsometricExclusionGradientLength,
                        _Ceiling, _CeilingMode, _CeilingBlendMode, _CeilingY, _CeilingPlayerYOffset, _CeilingYGradientLength,
                        _Floor, _FloorMode, _FloorY, _PlayerPosYOffset, _FloorYTextureGradientLength,

                        _AnimationEnabled, _AnimationSpeed,
                        _TransitionDuration,

                        _Zoning, _ZoningMode, _IsZoningRevealable, _ZoningEdgeGradientLength,
                        _SyncZonesWithFloorY, _SyncZonesFloorYOffset,

                        _PreviewMode,
                        _PreviewIndicatorLineThickness,                            

                        _DissolveTex,
                        _DissolveMask,
                        _ObstructionCurve,

                        _DissolveMask_TexelSize,
                        _ObstructionCurve_TexelSize,

                        albedo, emission, alphaForClipping);
#endif 



        o.Albedo = albedo;
        o.Emission += emission;   

	}




        
            void ChainSurfaceFunction(inout Surface l, inout ShaderData d)
            {
                  Ext_SurfaceFunction0(l, d);
                  Ext_SurfaceFunction1(l, d);
                 // Ext_SurfaceFunction2(l, d);
                 // Ext_SurfaceFunction3(l, d);
                 // Ext_SurfaceFunction4(l, d);
                 // Ext_SurfaceFunction5(l, d);
                 // Ext_SurfaceFunction6(l, d);
                 // Ext_SurfaceFunction7(l, d);
                 // Ext_SurfaceFunction8(l, d);
                 // Ext_SurfaceFunction9(l, d);
		           // Ext_SurfaceFunction10(l, d);
                 // Ext_SurfaceFunction11(l, d);
                 // Ext_SurfaceFunction12(l, d);
                 // Ext_SurfaceFunction13(l, d);
                 // Ext_SurfaceFunction14(l, d);
                 // Ext_SurfaceFunction15(l, d);
                 // Ext_SurfaceFunction16(l, d);
                 // Ext_SurfaceFunction17(l, d);
                 // Ext_SurfaceFunction18(l, d);
		           // Ext_SurfaceFunction19(l, d);
                 // Ext_SurfaceFunction20(l, d);
                 // Ext_SurfaceFunction21(l, d);
                 // Ext_SurfaceFunction22(l, d);
                 // Ext_SurfaceFunction23(l, d);
                 // Ext_SurfaceFunction24(l, d);
                 // Ext_SurfaceFunction25(l, d);
                 // Ext_SurfaceFunction26(l, d);
                 // Ext_SurfaceFunction27(l, d);
                 // Ext_SurfaceFunction28(l, d);
		           // Ext_SurfaceFunction29(l, d);
            }

            void ChainModifyVertex(inout VertexData v, inout VertexToPixel v2p, float4 time)
            {
                 ExtraV2F d;
                 
                 ZERO_INITIALIZE(ExtraV2F, d);
                 ZERO_INITIALIZE(Blackboard, d.blackboard);
                 // due to motion vectors in HDRP, we need to use the last
                 // time in certain spots. So if you are going to use _Time to adjust vertices,
                 // you need to use this time or motion vectors will break. 
                 d.time = time;

                 //  Ext_ModifyVertex0(v, d);
                 // Ext_ModifyVertex1(v, d);
                 // Ext_ModifyVertex2(v, d);
                 // Ext_ModifyVertex3(v, d);
                 // Ext_ModifyVertex4(v, d);
                 // Ext_ModifyVertex5(v, d);
                 // Ext_ModifyVertex6(v, d);
                 // Ext_ModifyVertex7(v, d);
                 // Ext_ModifyVertex8(v, d);
                 // Ext_ModifyVertex9(v, d);
                 // Ext_ModifyVertex10(v, d);
                 // Ext_ModifyVertex11(v, d);
                 // Ext_ModifyVertex12(v, d);
                 // Ext_ModifyVertex13(v, d);
                 // Ext_ModifyVertex14(v, d);
                 // Ext_ModifyVertex15(v, d);
                 // Ext_ModifyVertex16(v, d);
                 // Ext_ModifyVertex17(v, d);
                 // Ext_ModifyVertex18(v, d);
                 // Ext_ModifyVertex19(v, d);
                 // Ext_ModifyVertex20(v, d);
                 // Ext_ModifyVertex21(v, d);
                 // Ext_ModifyVertex22(v, d);
                 // Ext_ModifyVertex23(v, d);
                 // Ext_ModifyVertex24(v, d);
                 // Ext_ModifyVertex25(v, d);
                 // Ext_ModifyVertex26(v, d);
                 // Ext_ModifyVertex27(v, d);
                 // Ext_ModifyVertex28(v, d);
                 // Ext_ModifyVertex29(v, d);


                 // #if %EXTRAV2F0REQUIREKEY%
                 // v2p.extraV2F0 = d.extraV2F0;
                 // #endif

                 // #if %EXTRAV2F1REQUIREKEY%
                 // v2p.extraV2F1 = d.extraV2F1;
                 // #endif

                 // #if %EXTRAV2F2REQUIREKEY%
                 // v2p.extraV2F2 = d.extraV2F2;
                 // #endif

                 // #if %EXTRAV2F3REQUIREKEY%
                 // v2p.extraV2F3 = d.extraV2F3;
                 // #endif

                 // #if %EXTRAV2F4REQUIREKEY%
                 // v2p.extraV2F4 = d.extraV2F4;
                 // #endif

                 // #if %EXTRAV2F5REQUIREKEY%
                 // v2p.extraV2F5 = d.extraV2F5;
                 // #endif

                 // #if %EXTRAV2F6REQUIREKEY%
                 // v2p.extraV2F6 = d.extraV2F6;
                 // #endif

                 // #if %EXTRAV2F7REQUIREKEY%
                 // v2p.extraV2F7 = d.extraV2F7;
                 // #endif
            }

            void ChainModifyTessellatedVertex(inout VertexData v, inout VertexToPixel v2p)
            {
               ExtraV2F d;
               ZERO_INITIALIZE(ExtraV2F, d);
               ZERO_INITIALIZE(Blackboard, d.blackboard);

               // #if %EXTRAV2F0REQUIREKEY%
               // d.extraV2F0 = v2p.extraV2F0;
               // #endif

               // #if %EXTRAV2F1REQUIREKEY%
               // d.extraV2F1 = v2p.extraV2F1;
               // #endif

               // #if %EXTRAV2F2REQUIREKEY%
               // d.extraV2F2 = v2p.extraV2F2;
               // #endif

               // #if %EXTRAV2F3REQUIREKEY%
               // d.extraV2F3 = v2p.extraV2F3;
               // #endif

               // #if %EXTRAV2F4REQUIREKEY%
               // d.extraV2F4 = v2p.extraV2F4;
               // #endif

               // #if %EXTRAV2F5REQUIREKEY%
               // d.extraV2F5 = v2p.extraV2F5;
               // #endif

               // #if %EXTRAV2F6REQUIREKEY%
               // d.extraV2F6 = v2p.extraV2F6;
               // #endif

               // #if %EXTRAV2F7REQUIREKEY%
               // d.extraV2F7 = v2p.extraV2F7;
               // #endif


               // Ext_ModifyTessellatedVertex0(v, d);
               // Ext_ModifyTessellatedVertex1(v, d);
               // Ext_ModifyTessellatedVertex2(v, d);
               // Ext_ModifyTessellatedVertex3(v, d);
               // Ext_ModifyTessellatedVertex4(v, d);
               // Ext_ModifyTessellatedVertex5(v, d);
               // Ext_ModifyTessellatedVertex6(v, d);
               // Ext_ModifyTessellatedVertex7(v, d);
               // Ext_ModifyTessellatedVertex8(v, d);
               // Ext_ModifyTessellatedVertex9(v, d);
               // Ext_ModifyTessellatedVertex10(v, d);
               // Ext_ModifyTessellatedVertex11(v, d);
               // Ext_ModifyTessellatedVertex12(v, d);
               // Ext_ModifyTessellatedVertex13(v, d);
               // Ext_ModifyTessellatedVertex14(v, d);
               // Ext_ModifyTessellatedVertex15(v, d);
               // Ext_ModifyTessellatedVertex16(v, d);
               // Ext_ModifyTessellatedVertex17(v, d);
               // Ext_ModifyTessellatedVertex18(v, d);
               // Ext_ModifyTessellatedVertex19(v, d);
               // Ext_ModifyTessellatedVertex20(v, d);
               // Ext_ModifyTessellatedVertex21(v, d);
               // Ext_ModifyTessellatedVertex22(v, d);
               // Ext_ModifyTessellatedVertex23(v, d);
               // Ext_ModifyTessellatedVertex24(v, d);
               // Ext_ModifyTessellatedVertex25(v, d);
               // Ext_ModifyTessellatedVertex26(v, d);
               // Ext_ModifyTessellatedVertex27(v, d);
               // Ext_ModifyTessellatedVertex28(v, d);
               // Ext_ModifyTessellatedVertex29(v, d);

               // #if %EXTRAV2F0REQUIREKEY%
               // v2p.extraV2F0 = d.extraV2F0;
               // #endif

               // #if %EXTRAV2F1REQUIREKEY%
               // v2p.extraV2F1 = d.extraV2F1;
               // #endif

               // #if %EXTRAV2F2REQUIREKEY%
               // v2p.extraV2F2 = d.extraV2F2;
               // #endif

               // #if %EXTRAV2F3REQUIREKEY%
               // v2p.extraV2F3 = d.extraV2F3;
               // #endif

               // #if %EXTRAV2F4REQUIREKEY%
               // v2p.extraV2F4 = d.extraV2F4;
               // #endif

               // #if %EXTRAV2F5REQUIREKEY%
               // v2p.extraV2F5 = d.extraV2F5;
               // #endif

               // #if %EXTRAV2F6REQUIREKEY%
               // v2p.extraV2F6 = d.extraV2F6;
               // #endif

               // #if %EXTRAV2F7REQUIREKEY%
               // v2p.extraV2F7 = d.extraV2F7;
               // #endif
            }

            void ChainFinalColorForward(inout Surface l, inout ShaderData d, inout half4 color)
            {
               //   Ext_FinalColorForward0(l, d, color);
               //   Ext_FinalColorForward1(l, d, color);
               //   Ext_FinalColorForward2(l, d, color);
               //   Ext_FinalColorForward3(l, d, color);
               //   Ext_FinalColorForward4(l, d, color);
               //   Ext_FinalColorForward5(l, d, color);
               //   Ext_FinalColorForward6(l, d, color);
               //   Ext_FinalColorForward7(l, d, color);
               //   Ext_FinalColorForward8(l, d, color);
               //   Ext_FinalColorForward9(l, d, color);
               //  Ext_FinalColorForward10(l, d, color);
               //  Ext_FinalColorForward11(l, d, color);
               //  Ext_FinalColorForward12(l, d, color);
               //  Ext_FinalColorForward13(l, d, color);
               //  Ext_FinalColorForward14(l, d, color);
               //  Ext_FinalColorForward15(l, d, color);
               //  Ext_FinalColorForward16(l, d, color);
               //  Ext_FinalColorForward17(l, d, color);
               //  Ext_FinalColorForward18(l, d, color);
               //  Ext_FinalColorForward19(l, d, color);
               //  Ext_FinalColorForward20(l, d, color);
               //  Ext_FinalColorForward21(l, d, color);
               //  Ext_FinalColorForward22(l, d, color);
               //  Ext_FinalColorForward23(l, d, color);
               //  Ext_FinalColorForward24(l, d, color);
               //  Ext_FinalColorForward25(l, d, color);
               //  Ext_FinalColorForward26(l, d, color);
               //  Ext_FinalColorForward27(l, d, color);
               //  Ext_FinalColorForward28(l, d, color);
               //  Ext_FinalColorForward29(l, d, color);
            }

            void ChainFinalGBufferStandard(inout Surface s, inout ShaderData d, inout half4 GBuffer0, inout half4 GBuffer1, inout half4 GBuffer2, inout half4 outEmission, inout half4 outShadowMask)
            {
               //   Ext_FinalGBufferStandard0(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard1(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard2(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard3(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard4(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard5(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard6(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard7(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard8(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard9(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard10(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard11(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard12(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard13(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard14(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard15(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard16(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard17(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard18(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard19(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard20(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard21(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard22(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard23(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard24(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard25(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard26(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard27(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard28(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard29(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
            }



            

         ShaderData CreateShaderData(VertexToPixel i
                  #if NEED_FACING
                     , bool facing
                  #endif
         )
         {
            ShaderData d = (ShaderData)0;
            d.clipPos = i.pos;
            d.worldSpacePosition = i.worldPos;

            d.worldSpaceNormal = normalize(i.worldNormal);
            d.worldSpaceTangent = normalize(i.worldTangent.xyz);
            d.tangentSign = i.worldTangent.w;
            float3 bitangent = cross(i.worldTangent.xyz, i.worldNormal) * d.tangentSign * -1;
            

            d.TBNMatrix = float3x3(d.worldSpaceTangent, bitangent, d.worldSpaceNormal);
            d.worldSpaceViewDir = normalize(_WorldSpaceCameraPos - i.worldPos);

            d.tangentSpaceViewDir = mul(d.TBNMatrix, d.worldSpaceViewDir);
             d.texcoord0 = i.texcoord0;
             d.texcoord1 = i.texcoord1;
             d.texcoord2 = i.texcoord2;

            // #if %TEXCOORD3REQUIREKEY%
             d.texcoord3 = i.texcoord3;
            // #endif

            // d.isFrontFace = facing;
            // #if %VERTEXCOLORREQUIREKEY%
            // d.vertexColor = i.vertexColor;
            // #endif

            // these rarely get used, so we back transform them. Usually will be stripped.
            #if _HDRP
                // d.localSpacePosition = mul(unity_WorldToObject, float4(GetCameraRelativePositionWS(i.worldPos), 1)).xyz;
            #else
                // d.localSpacePosition = mul(unity_WorldToObject, float4(i.worldPos, 1)).xyz;
            #endif
            // d.localSpaceNormal = normalize(mul((float3x3)unity_WorldToObject, i.worldNormal));
            // d.localSpaceTangent = normalize(mul((float3x3)unity_WorldToObject, i.worldTangent.xyz));

            // #if %SCREENPOSREQUIREKEY%
            // d.screenPos = i.screenPos;
            // d.screenUV = (i.screenPos.xy / i.screenPos.w);
            // #endif


            // #if %EXTRAV2F0REQUIREKEY%
            // d.extraV2F0 = i.extraV2F0;
            // #endif

            // #if %EXTRAV2F1REQUIREKEY%
            // d.extraV2F1 = i.extraV2F1;
            // #endif

            // #if %EXTRAV2F2REQUIREKEY%
            // d.extraV2F2 = i.extraV2F2;
            // #endif

            // #if %EXTRAV2F3REQUIREKEY%
            // d.extraV2F3 = i.extraV2F3;
            // #endif

            // #if %EXTRAV2F4REQUIREKEY%
            // d.extraV2F4 = i.extraV2F4;
            // #endif

            // #if %EXTRAV2F5REQUIREKEY%
            // d.extraV2F5 = i.extraV2F5;
            // #endif

            // #if %EXTRAV2F6REQUIREKEY%
            // d.extraV2F6 = i.extraV2F6;
            // #endif

            // #if %EXTRAV2F7REQUIREKEY%
            // d.extraV2F7 = i.extraV2F7;
            // #endif

            return d;
         }
         

            

struct VaryingsToPS
{
   VertexToPixel vmesh;
   #ifdef VARYINGS_NEED_PASS
      VaryingsPassToPS vpass;
   #endif
};

struct PackedVaryingsToPS
{
   #ifdef VARYINGS_NEED_PASS
      PackedVaryingsPassToPS vpass;
   #endif
   VertexToPixel vmesh;

   UNITY_VERTEX_OUTPUT_STEREO
};

PackedVaryingsToPS PackVaryingsToPS(VaryingsToPS input)
{
   PackedVaryingsToPS output = (PackedVaryingsToPS)0;
   output.vmesh = input.vmesh;
   #ifdef VARYINGS_NEED_PASS
      output.vpass = PackVaryingsPassToPS(input.vpass);
   #endif

   UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
   return output;
}




VertexToPixel VertMesh(VertexData input)
{
    VertexToPixel output = (VertexToPixel)0;

    UNITY_SETUP_INSTANCE_ID(input);
    UNITY_TRANSFER_INSTANCE_ID(input, output);

    
    ChainModifyVertex(input, output, _Time);


    // This return the camera relative position (if enable)
    float3 positionRWS = TransformObjectToWorld(input.vertex.xyz);
    float3 normalWS = TransformObjectToWorldNormal(input.normal);
    float4 tangentWS = float4(TransformObjectToWorldDir(input.tangent.xyz), input.tangent.w);


    output.worldPos = GetAbsolutePositionWS(positionRWS);
    output.pos = TransformWorldToHClip(positionRWS);
    output.worldNormal = normalWS;
    output.worldTangent = tangentWS;


    output.texcoord0 = input.texcoord0;
    output.texcoord1 = input.texcoord1;
    output.texcoord2 = input.texcoord2;
    // #if %TEXCOORD3REQUIREKEY%
     output.texcoord3 = input.texcoord3;
    // #endif

    // #if %SCREENPOSREQUIREKEY%
    // output.screenPos = ComputeScreenPos(output.pos, _ProjectionParams.x);
    // #endif

    // #if %VERTEXCOLORREQUIREKEY%
    // output.vertexColor = input.vertexColor;
    // #endif
    return output;
}


#if (SHADERPASS == SHADERPASS_DBUFFER_MESH)
void MeshDecalsPositionZBias(inout VaryingsToPS input)
{
#if defined(UNITY_REVERSED_Z)
    input.vmesh.pos.z -= _DecalMeshDepthBias;
#else
    input.vmesh.pos.z += _DecalMeshDepthBias;
#endif
}
#endif


#if (SHADERPASS == SHADERPASS_LIGHT_TRANSPORT)

// This was not in constant buffer in original unity, so keep outiside. But should be in as ShaderRenderPass frequency
float unity_OneOverOutputBoost;
float unity_MaxOutputValue;

CBUFFER_START(UnityMetaPass)
// x = use uv1 as raster position
// y = use uv2 as raster position
bool4 unity_MetaVertexControl;

// x = return albedo
// y = return normal
bool4 unity_MetaFragmentControl;
CBUFFER_END

PackedVaryingsToPS Vert(VertexData inputMesh)
{
    VaryingsToPS output = (VaryingsToPS)0;
    output.vmesh = (VertexToPixel)0;

    UNITY_SETUP_INSTANCE_ID(inputMesh);
    UNITY_TRANSFER_INSTANCE_ID(inputMesh, output.vmesh);

    // Output UV coordinate in vertex shader
    float2 uv = float2(0.0, 0.0);

    if (unity_MetaVertexControl.x)
    {
        uv = inputMesh.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
    }
    else if (unity_MetaVertexControl.y)
    {
        uv = inputMesh.texcoord2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
    }

    // OpenGL right now needs to actually use the incoming vertex position
    // so we create a fake dependency on it here that haven't any impact.
    output.vmesh.pos = float4(uv * 2.0 - 1.0, inputMesh.vertex.z > 0 ? 1.0e-4 : 0.0, 1.0);

#ifdef VARYINGS_NEED_POSITION_WS
    output.vmesh.worldPos = TransformObjectToWorld(inputMesh.vertex.xyz);
#endif

#ifdef VARYINGS_NEED_TANGENT_TO_WORLD
    // Normal is required for triplanar mapping
    output.vmesh.worldNormal = TransformObjectToWorldNormal(inputMesh.normal);
    // Not required but assign to silent compiler warning
    output.vmesh.worldTangent = float4(1.0, 0.0, 0.0, 0.0);
#endif

    output.vmesh.texcoord0 = inputMesh.texcoord0;
    output.vmesh.texcoord1 = inputMesh.texcoord1;
    output.vmesh.texcoord2 = inputMesh.texcoord2;
    // #if %TEXCOORD3REQUIREKEY%
     output.vmesh.texcoord3 = inputMesh.texcoord3;
    // #endif

    // #if %VERTEXCOLORREQUIREKEY%
    // output.vmesh.vertexColor = inputMesh.vertexColor;
    // #endif

    return PackVaryingsToPS(output);
}
#else

PackedVaryingsToPS Vert(VertexData inputMesh)
{
    VaryingsToPS varyingsType;
    varyingsType.vmesh = VertMesh(inputMesh);
    #if (SHADERPASS == SHADERPASS_DBUFFER_MESH)
       MeshDecalsPositionZBias(varyingsType);
    #endif
    return PackVaryingsToPS(varyingsType);
}

#endif



            

            
                FragInputs BuildFragInputs(VertexToPixel input)
                {
                    UNITY_SETUP_INSTANCE_ID(input);
                    FragInputs output;
                    ZERO_INITIALIZE(FragInputs, output);
            
                    // Init to some default value to make the computer quiet (else it output 'divide by zero' warning even if value is not used).
                    // TODO: this is a really poor workaround, but the variable is used in a bunch of places
                    // to compute normals which are then passed on elsewhere to compute other values...
                    output.tangentToWorld = k_identity3x3;
                    output.positionSS = input.pos;       // input.positionCS is SV_Position
            
                    output.positionRWS = GetCameraRelativePositionWS(input.worldPos);
                    output.tangentToWorld = BuildTangentToWorld(input.worldTangent, input.worldNormal);
                    output.texCoord0 = input.texcoord0;
                    output.texCoord1 = input.texcoord1;
                    output.texCoord2 = input.texcoord2;
            
                    return output;
                }
            
               void BuildSurfaceData(FragInputs fragInputs, inout Surface surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData, out float3 bentNormalWS)
               {
                   // setup defaults -- these are used if the graph doesn't output a value
                   ZERO_INITIALIZE(SurfaceData, surfaceData);
        
                   // specularOcclusion need to be init ahead of decal to quiet the compiler that modify the SurfaceData struct
                   // however specularOcclusion can come from the graph, so need to be init here so it can be override.
                   surfaceData.specularOcclusion = 1.0;
        
                   // copy across graph values, if defined
                   surfaceData.baseColor =                 surfaceDescription.Albedo;
                   surfaceData.perceptualSmoothness =      surfaceDescription.Smoothness;
                   surfaceData.ambientOcclusion =          surfaceDescription.Occlusion;
                   surfaceData.specularOcclusion =         surfaceDescription.SpecularOcclusion;
                   surfaceData.metallic =                  surfaceDescription.Metallic;
                   surfaceData.subsurfaceMask =            surfaceDescription.SubsurfaceMask;
                   surfaceData.thickness =                 surfaceDescription.Thickness;
                   surfaceData.diffusionProfileHash =      asuint(surfaceDescription.DiffusionProfileHash);
                   #if _USESPECULAR
                      surfaceData.specularColor =             surfaceDescription.Specular;
                   #endif
                   surfaceData.coatMask =                  surfaceDescription.CoatMask;
                   surfaceData.anisotropy =                surfaceDescription.Anisotropy;
                   surfaceData.iridescenceMask =           surfaceDescription.IridescenceMask;
                   surfaceData.iridescenceThickness =      surfaceDescription.IridescenceThickness;
        
           #ifdef _HAS_REFRACTION
                   if (_EnableSSRefraction)
                   {
                       // surfaceData.ior =                       surfaceDescription.RefractionIndex;
                       // surfaceData.transmittanceColor =        surfaceDescription.RefractionColor;
                       // surfaceData.atDistance =                surfaceDescription.RefractionDistance;
        
                       surfaceData.transmittanceMask = (1.0 - surfaceDescription.Alpha);
                       surfaceDescription.Alpha = 1.0;
                   }
                   else
                   {
                       surfaceData.ior = surfaceDescription.ior;
                       surfaceData.transmittanceColor = surfaceDescription.transmittanceColor;
                       surfaceData.atDistance = surfaceDescription.atDistance;
                       surfaceData.transmittanceMask = surfaceDescription.transmittanceMask;
                       surfaceDescription.Alpha = 1.0;
                   }
           #else
                   surfaceData.ior = 1.0;
                   surfaceData.transmittanceColor = float3(1.0, 1.0, 1.0);
                   surfaceData.atDistance = 1.0;
                   surfaceData.transmittanceMask = 0.0;
           #endif
                
                   // These static material feature allow compile time optimization
                   surfaceData.materialFeatures = MATERIALFEATUREFLAGS_LIT_STANDARD;
           #ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
                   surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SUBSURFACE_SCATTERING;
           #endif
           #ifdef _MATERIAL_FEATURE_TRANSMISSION
                   surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_TRANSMISSION;
           #endif
           #ifdef _MATERIAL_FEATURE_ANISOTROPY
                   surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_ANISOTROPY;
           #endif
                   // surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_CLEAR_COAT;
        
           #ifdef _MATERIAL_FEATURE_IRIDESCENCE
                   surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_IRIDESCENCE;
           #endif
           #ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
                   surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SPECULAR_COLOR;
           #endif
        
           #if defined (_MATERIAL_FEATURE_SPECULAR_COLOR) && defined (_ENERGY_CONSERVING_SPECULAR)
                   // Require to have setup baseColor
                   // Reproduce the energy conservation done in legacy Unity. Not ideal but better for compatibility and users can unchek it
                   surfaceData.baseColor *= (1.0 - Max3(surfaceData.specularColor.r, surfaceData.specularColor.g, surfaceData.specularColor.b));
           #endif

        
                   // tangent-space normal
                   float3 normalTS = float3(0.0f, 0.0f, 1.0f);
                   normalTS = surfaceDescription.Normal;
        
                   // compute world space normal
                   #if !_WORLDSPACENORMAL
                      surfaceData.normalWS = mul(normalTS, fragInputs.tangentToWorld);
                   #else
                      surfaceData.normalWS = normalTS;  
                   #endif
                   surfaceData.geomNormalWS = fragInputs.tangentToWorld[2];
        
                   surfaceData.tangentWS = normalize(fragInputs.tangentToWorld[0].xyz);    // The tangent is not normalize in tangentToWorld for mikkt. TODO: Check if it expected that we normalize with Morten. Tag: SURFACE_GRADIENT
                   // surfaceData.tangentWS = TransformTangentToWorld(surfaceDescription.Tangent, fragInputs.tangentToWorld);
        
           #if HAVE_DECALS
                   if (_EnableDecals)
                   {
                       #if VERSION_GREATER_EQUAL(10,2)
                          DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput,  surfaceData.geomNormalWS, surfaceDescription.Alpha);
                          ApplyDecalToSurfaceData(decalSurfaceData,  surfaceData.geomNormalWS, surfaceData);
                       #else
                          DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, surfaceDescription.Alpha);
                          ApplyDecalToSurfaceData(decalSurfaceData, surfaceData);
                       #endif
                   }
           #endif
        
                   bentNormalWS = surfaceData.normalWS;
               
                   surfaceData.tangentWS = Orthonormalize(surfaceData.tangentWS, surfaceData.normalWS);
        
        
                   // By default we use the ambient occlusion with Tri-ace trick (apply outside) for specular occlusion.
                   // If user provide bent normal then we process a better term
           #if defined(_SPECULAR_OCCLUSION_CUSTOM)
                   // Just use the value passed through via the slot (not active otherwise)
           #elif defined(_SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL)
                   // If we have bent normal and ambient occlusion, process a specular occlusion
                   surfaceData.specularOcclusion = GetSpecularOcclusionFromBentAO(V, bentNormalWS, surfaceData.normalWS, surfaceData.ambientOcclusion, PerceptualSmoothnessToPerceptualRoughness(surfaceData.perceptualSmoothness));
           #elif defined(_AMBIENT_OCCLUSION) && defined(_SPECULAR_OCCLUSION_FROM_AO)
                   surfaceData.specularOcclusion = GetSpecularOcclusionFromAmbientOcclusion(ClampNdotV(dot(surfaceData.normalWS, V)), surfaceData.ambientOcclusion, PerceptualSmoothnessToRoughness(surfaceData.perceptualSmoothness));
           #endif
        
           #ifdef _ENABLE_GEOMETRIC_SPECULAR_AA
                   surfaceData.perceptualSmoothness = GeometricNormalFiltering(surfaceData.perceptualSmoothness, fragInputs.tangentToWorld[2], surfaceDescription.SpecularAAScreenSpaceVariance, surfaceDescription.SpecularAAThreshold);
           #endif
        
           #ifdef DEBUG_DISPLAY
                   if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
                   {
                       // TODO: need to update mip info
                       surfaceData.metallic = 0;
                   }
        
                   // We need to call ApplyDebugToSurfaceData after filling the surfarcedata and before filling builtinData
                   // as it can modify attribute use for static lighting
                   ApplyDebugToSurfaceData(fragInputs.tangentToWorld, surfaceData);
           #endif
               }
        
               void GetSurfaceAndBuiltinData(VertexToPixel m2ps, FragInputs fragInputs, float3 V, inout PositionInputs posInput,
                     out SurfaceData surfaceData, out BuiltinData builtinData, inout Surface l, inout ShaderData d
                     #if NEED_FACING
                        , bool facing
                     #endif
                  )
               {
                 // Removed since crossfade does not work, probably needs extra material setup.   
                 //#ifdef LOD_FADE_CROSSFADE // enable dithering LOD transition if user select CrossFade transition in LOD group
                 //    uint3 fadeMaskSeed = asuint((int3)(V * _ScreenSize.xyx)); // Quantize V to _ScreenSize values
                 //    LODDitheringTransition(fadeMaskSeed, unity_LODFade.x);
                 //#endif
        
                 d = CreateShaderData(m2ps
                  #if NEED_FACING
                    , facing
                  #endif
                 );

                 

                 l = (Surface)0;

                 l.Albedo = half3(0.5, 0.5, 0.5);
                 l.Normal = float3(0,0,1);
                 l.Occlusion = 1;
                 l.Alpha = 1;
                 l.SpecularOcclusion = 1;

                 #ifdef _DEPTHOFFSET_ON
                    l.outputDepth = posInput.deviceDepth;
                 #endif

                 ChainSurfaceFunction(l, d);

                 #ifdef _DEPTHOFFSET_ON
                    posInput.deviceDepth = l.outputDepth;
                 #endif

                 #if _UNLIT
                     //l.Emission = l.Albedo;
                     //l.Albedo = 0;
                     l.Normal = half3(0,0,1);
                     l.Occlusion = 1;
                     l.Metallic = 0;
                     l.Specular = 0;
                 #endif

                 surfaceData.geomNormalWS = d.worldSpaceNormal;
                 surfaceData.tangentWS = d.worldSpaceTangent;
                 fragInputs.tangentToWorld = d.TBNMatrix;

                 float3 bentNormalWS;
                 BuildSurfaceData(fragInputs, l, V, posInput, surfaceData, bentNormalWS);

                 InitBuiltinData(posInput, l.Alpha, bentNormalWS, -d.worldSpaceNormal, fragInputs.texCoord1, fragInputs.texCoord2, builtinData);

                 

                 builtinData.emissiveColor = l.Emission;

                 #if defined(_OVERRIDE_BAKEDGI)
                    builtinData.bakeDiffuseLighting = l.DiffuseGI;
                    builtinData.backBakeDiffuseLighting = l.BackDiffuseGI;
                    builtinData.emissiveColor += l.SpecularGI;
                 #endif
                 
                 #if defined(_OVERRIDE_SHADOWMASK)
                   builtinData.shadowMask0 = l.ShadowMask.x;
                   builtinData.shadowMask1 = l.ShadowMask.y;
                   builtinData.shadowMask2 = l.ShadowMask.z;
                   builtinData.shadowMask3 = l.ShadowMask.w;
                #endif
        
                 #if (SHADERPASS == SHADERPASS_DISTORTION)
                     //builtinData.distortion = surfaceDescription.Distortion;
                     //builtinData.distortionBlur = surfaceDescription.DistortionBlur;
                     builtinData.distortion = float2(0.0, 0.0);
                     builtinData.distortionBlur = 0.0;
                 #else
                     builtinData.distortion = float2(0.0, 0.0);
                     builtinData.distortionBlur = 0.0;
                 #endif
        
                   PostInitBuiltinData(V, posInput, surfaceData, builtinData);
               }

            void Frag(  PackedVaryingsToPS packedInput,
                        OUTPUT_GBUFFER(outGBuffer)
                        #ifdef _DEPTHOFFSET_ON
                        , out float outputDepth : SV_Depth
                        #endif
                        #if NEED_FACING
                           , bool facing : SV_IsFrontFace
                        #endif
                        )
            {
                  UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(packedInput);
                  FragInputs input = BuildFragInputs(packedInput.vmesh);

                  // input.positionSS is SV_Position
                  PositionInputs posInput = GetPositionInput(input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS);

                  float3 V = GetWorldSpaceNormalizeViewDir(input.positionRWS);

                  SurfaceData surfaceData;
                  BuiltinData builtinData;
                  Surface l;
                  ShaderData d;
                  GetSurfaceAndBuiltinData(packedInput.vmesh, input, V, posInput, surfaceData, builtinData, l, d
               #if NEED_FACING
                  , facing
               #endif
               );

                  ENCODE_INTO_GBUFFER(surfaceData, builtinData, posInput.positionSS, outGBuffer);

                  #ifdef _DEPTHOFFSET_ON
                        outputDepth = posInput.deviceDepth;
                  #endif
            }

            ENDHLSL
        }
        
      Pass
        {
            // based on HDLitPass.template
            Name "ShadowCaster"
            Tags { "LightMode" = "ShadowCaster" }

            

            //-------------------------------------------------------------------------------------
            // Render Modes (Blend, Cull, ZTest, Stencil, etc)
            //-------------------------------------------------------------------------------------
            
            Cull Back

            ZClip [_ZClip]
            ZWrite On
            ZTest LEqual

            ColorMask 0

                Cull [_CullMode]

        
            //-------------------------------------------------------------------------------------
            // End Render Modes
            //-------------------------------------------------------------------------------------
        
            HLSLPROGRAM
        
            #pragma target 4.5
            #pragma only_renderers d3d11 ps4 xboxone vulkan metal switch
            //#pragma enable_d3d11_debug_symbols
        
            #pragma multi_compile_instancing

            //#pragma multi_compile_local _ _ALPHATEST_ON


            //#pragma shader_feature _SURFACE_TYPE_TRANSPARENT
            //#pragma shader_feature_local _ _BLENDMODE_ALPHA _BLENDMODE_ADD _BLENDMODE_PRE_MULTIPLY
        
            //-------------------------------------------------------------------------------------
            // Variant Definitions (active field translations to HDRP defines)
            //-------------------------------------------------------------------------------------
            // #define _MATERIAL_FEATURE_SUBSURFACE_SCATTERING 1
            // #define _MATERIAL_FEATURE_TRANSMISSION 1
            // #define _MATERIAL_FEATURE_ANISOTROPY 1
            // #define _MATERIAL_FEATURE_IRIDESCENCE 1
            // #define _MATERIAL_FEATURE_SPECULAR_COLOR 1
            #define _ENABLE_FOG_ON_TRANSPARENT 1
            // #define _AMBIENT_OCCLUSION 1
            // #define _SPECULAR_OCCLUSION_FROM_AO 1
            // #define _SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL 1
            // #define _SPECULAR_OCCLUSION_CUSTOM 1
            // #define _ENERGY_CONSERVING_SPECULAR 1
            // #define _ENABLE_GEOMETRIC_SPECULAR_AA 1
            // #define _HAS_REFRACTION 1
            // #define _REFRACTION_PLANE 1
            // #define _REFRACTION_SPHERE 1
            // #define _DISABLE_DECALS 1
            // #define _DISABLE_SSR 1
            // #define _ADD_PRECOMPUTED_VELOCITY
            // #define _WRITE_TRANSPARENT_MOTION_VECTOR 1
            // #define _DEPTHOFFSET_ON 1
            // #define _BLENDMODE_PRESERVE_SPECULAR_LIGHTING 1

            #define SHADERPASS SHADERPASS_SHADOWS
            #define RAYTRACING_SHADER_GRAPH_HIGH
            #define _PASSSHADOW 1

            


    #pragma shader_feature_local _ALPHATEST_ON

    #pragma shader_feature_local_fragment _NORMALMAP_TANGENT_SPACE


    #pragma shader_feature_local _NORMALMAP
    #pragma shader_feature_local_fragment _MASKMAP
    #pragma shader_feature_local_fragment _EMISSIVE_COLOR_MAP
    #pragma shader_feature_local_fragment _ANISOTROPYMAP
    #pragma shader_feature_local_fragment _DETAIL_MAP
    #pragma shader_feature_local_fragment _SUBSURFACE_MASK_MAP
    #pragma shader_feature_local_fragment _THICKNESSMAP
    #pragma shader_feature_local_fragment _IRIDESCENCE_THICKNESSMAP
    #pragma shader_feature_local_fragment _SPECULARCOLORMAP

    // MaterialFeature are used as shader feature to allow compiler to optimize properly
    #pragma shader_feature_local_fragment _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
    #pragma shader_feature_local_fragment _MATERIAL_FEATURE_TRANSMISSION
    #pragma shader_feature_local_fragment _MATERIAL_FEATURE_ANISOTROPY
    #pragma shader_feature_local_fragment _MATERIAL_FEATURE_CLEAR_COAT
    #pragma shader_feature_local_fragment _MATERIAL_FEATURE_IRIDESCENCE
    #pragma shader_feature_local_fragment _MATERIAL_FEATURE_SPECULAR_COLOR


    #pragma shader_feature_local_fragment _ENABLESPECULAROCCLUSION
    #pragma shader_feature_local_fragment _ _SPECULAR_OCCLUSION_NONE _SPECULAR_OCCLUSION_FROM_BENT_NORMAL_MAP

    #ifdef _ENABLESPECULAROCCLUSION
        #define _SPECULAR_OCCLUSION_FROM_BENT_NORMAL_MAP
    #endif


        #pragma shader_feature_local_fragment _OBSTRUCTION_CURVE
        #pragma shader_feature_local_fragment _DISSOLVEMASK
        #pragma shader_feature_local_fragment _ZONING
        #pragma shader_feature_local_fragment _REPLACEMENT
        #pragma shader_feature_local_fragment _PLAYERINDEPENDENT


   #define _HDRP 1
#define _USINGTEXCOORD1 1
#define _USINGTEXCOORD2 1


               #pragma vertex Vert
   #pragma fragment Frag
        
            //-------------------------------------------------------------------------------------
            // Defines
            //-------------------------------------------------------------------------------------
            
        
                  // useful conversion functions to make surface shader code just work

      #define UNITY_DECLARE_TEX2D(name) TEXTURE2D(name); SAMPLER(sampler##name);
      #define UNITY_DECLARE_TEX2D_NOSAMPLER(name) TEXTURE2D(name);
      #define UNITY_DECLARE_TEX2DARRAY(name) TEXTURE2D_ARRAY(name); SAMPLER(sampler##name);
      #define UNITY_DECLARE_TEX2DARRAY_NOSAMPLER(tex) TEXTURE2D_ARRAY(tex);

      #define UNITY_SAMPLE_TEX2DARRAY(tex,coord)            SAMPLE_TEXTURE2D_ARRAY(tex, sampler##tex, coord.xy, coord.z)
      #define UNITY_SAMPLE_TEX2DARRAY_LOD(tex,coord,lod)    SAMPLE_TEXTURE2D_ARRAY_LOD(tex, sampler##tex, coord.xy, coord.z, lod)
      #define UNITY_SAMPLE_TEX2D(tex, coord)                SAMPLE_TEXTURE2D(tex, sampler##tex, coord)
      #define UNITY_SAMPLE_TEX2D_SAMPLER(tex, samp, coord)  SAMPLE_TEXTURE2D(tex, sampler##samp, coord)

      #define UNITY_SAMPLE_TEX2D_LOD(tex,coord, lod)   SAMPLE_TEXTURE2D_LOD(tex, sampler_##tex, coord, lod)
      #define UNITY_SAMPLE_TEX2D_SAMPLER_LOD(tex,samplertex,coord, lod) SAMPLE_TEXTURE2D_LOD (tex, sampler##samplertex,coord, lod)

      #if defined(UNITY_COMPILER_HLSL)
         #define UNITY_INITIALIZE_OUTPUT(type,name) name = (type)0;
      #else
         #define UNITY_INITIALIZE_OUTPUT(type,name)
      #endif

      #define sampler2D_float sampler2D
      #define sampler2D_half sampler2D

      #undef WorldNormalVector
      #define WorldNormalVector(data, normal) mul(normal, data.TBNMatrix)

      #define UnityObjectToWorldNormal(normal) mul(GetObjectToWorldMatrix(), normal)




// HDRP Adapter stuff


            // If we use subsurface scattering, enable output split lighting (for forward pass)
            #if defined(_MATERIAL_FEATURE_SUBSURFACE_SCATTERING) && !defined(_SURFACE_TYPE_TRANSPARENT)
            #define OUTPUT_SPLIT_LIGHTING
            #endif

            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Version.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
        
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"
        
            // define FragInputs structure
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
            #if (SHADER_LIBRARY_VERSION_MAJOR >= 10)
               #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/Functions.hlsl"
            #endif


        

            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
        #ifdef DEBUG_DISPLAY
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
        #endif
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
        
        #if (SHADERPASS == SHADERPASS_FORWARD)
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/Lighting.hlsl"
        
            #define HAS_LIGHTLOOP
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/LightLoopDef.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/LightLoop.hlsl"
        #else
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
        #endif
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalUtilities.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitDecalData.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"
        
            // Used by SceneSelectionPass
            int _ObjectId;
            int _PassValue;
        
           
            // data across stages, stripped like the above.
            struct VertexToPixel
            {
               float4 pos : SV_POSITION;
               float3 worldPos : TEXCOORD0;
               float3 worldNormal : TEXCOORD1;
               float4 worldTangent : TEXCOORD2;
               float4 texcoord0 : TEXCOORD3;
               float4 texcoord1 : TEXCOORD4;
               float4 texcoord2 : TEXCOORD5;

               // #if %TEXCOORD3REQUIREKEY%
                float4 texcoord3 : TEXCOORD6;
               // #endif

               // #if %SCREENPOSREQUIREKEY%
               // float4 screenPos : TEXCOORD7;
               // #endif

               // #if %VERTEXCOLORREQUIREKEY%
               // float4 vertexColor : COLOR;
               // #endif

               // #if %EXTRAV2F0REQUIREKEY%
               // float4 extraV2F0 : TEXCOORD8;
               // #endif

               // #if %EXTRAV2F1REQUIREKEY%
               // float4 extraV2F1 : TEXCOORD9;
               // #endif

               // #if %EXTRAV2F2REQUIREKEY%
               // float4 extraV2F2 : TEXCOORD10;
               // #endif

               // #if %EXTRAV2F3REQUIREKEY%
               // float4 extraV2F3 : TEXCOORD11;
               // #endif

               // #if %EXTRAV2F4REQUIREKEY%
               // float4 extraV2F4 : TEXCOORD12;
               // #endif

               // #if %EXTRAV2F5REQUIREKEY%
               // float4 extraV2F5 : TEXCOORD13;
               // #endif

               // #if %EXTRAV2F6REQUIREKEY%
               // float4 extraV2F6 : TEXCOORD14;
               // #endif

               // #if %EXTRAV2F7REQUIREKEY%
               // float4 extraV2F7 : TEXCOORD15;
               // #endif

               #if UNITY_ANY_INSTANCING_ENABLED
                  UNITY_VERTEX_INPUT_INSTANCE_ID
               #endif // UNITY_ANY_INSTANCING_ENABLED

               UNITY_VERTEX_OUTPUT_STEREO
            };
      
  
            
            
            // data describing the user output of a pixel
            struct Surface
            {
               half3 Albedo;
               half Height;
               half3 Normal;
               half Smoothness;
               half3 Emission;
               half Metallic;
               half3 Specular;
               half Occlusion;
               half SpecularPower; // for simple lighting
               half Alpha;
               float outputDepth; // if written, SV_Depth semantic is used. ShaderData.clipPos.z is unused value
               // HDRP Only
               half SpecularOcclusion;
               half SubsurfaceMask;
               half Thickness;
               half CoatMask;
               half CoatSmoothness;
               half Anisotropy;
               half IridescenceMask;
               half IridescenceThickness;
               int DiffusionProfileHash;
               float SpecularAAThreshold;
               float SpecularAAScreenSpaceVariance;
               // requires _OVERRIDE_BAKEDGI to be defined, but is mapped in all pipelines
               float3 DiffuseGI;
               float3 BackDiffuseGI;
               float3 SpecularGI;
               float ior;
               float3 transmittanceColor;
               float atDistance;
               float transmittanceMask;
               // requires _OVERRIDE_SHADOWMASK to be defines
               float4 ShadowMask;


            };

            // Data the user declares in blackboard blocks
            struct Blackboard
            {
                
                float blackboardDummyData;
            };

            // data the user might need, this will grow to be big. But easy to strip
            struct ShaderData
            {
               float4 clipPos; // SV_POSITION
               float3 localSpacePosition;
               float3 localSpaceNormal;
               float3 localSpaceTangent;
        
               float3 worldSpacePosition;
               float3 worldSpaceNormal;
               float3 worldSpaceTangent;
               float tangentSign;

               float3 worldSpaceViewDir;
               float3 tangentSpaceViewDir;

               float4 texcoord0;
               float4 texcoord1;
               float4 texcoord2;
               float4 texcoord3;

               float2 screenUV;
               float4 screenPos;

               float4 vertexColor;
               bool isFrontFace;

               float4 extraV2F0;
               float4 extraV2F1;
               float4 extraV2F2;
               float4 extraV2F3;
               float4 extraV2F4;
               float4 extraV2F5;
               float4 extraV2F6;
               float4 extraV2F7;

               float3x3 TBNMatrix;
               Blackboard blackboard;
            };

            struct VertexData
            {
               #if SHADER_TARGET > 30
               // UNITY_VERTEX_INPUT_INSTANCE_ID;
               #endif
               float4 vertex : POSITION;
               float3 normal : NORMAL;
               float4 tangent : TANGENT;
               float4 texcoord0 : TEXCOORD0;

               // optimize out mesh coords when not in use by user or lighting system
               #if _URP && (_USINGTEXCOORD1 || _PASSMETA || _PASSFORWARD || _PASSGBUFFER)
                  float4 texcoord1 : TEXCOORD1;
               #endif

               #if _URP && (_USINGTEXCOORD2 || _PASSMETA || ((_PASSFORWARD || _PASSGBUFFER) && defined(DYNAMICLIGHTMAP_ON)))
                  float4 texcoord2 : TEXCOORD2;
               #endif

               #if _STANDARD && (_USINGTEXCOORD1 || (_PASSMETA || ((_PASSFORWARD || _PASSGBUFFER || _PASSFORWARDADD) && LIGHTMAP_ON)))
                  float4 texcoord1 : TEXCOORD1;
               #endif
               #if _STANDARD && (_USINGTEXCOORD2 || (_PASSMETA || ((_PASSFORWARD || _PASSGBUFFER) && DYNAMICLIGHTMAP_ON)))
                  float4 texcoord2 : TEXCOORD2;
               #endif


               #if _HDRP
                  float4 texcoord1 : TEXCOORD1;
                  float4 texcoord2 : TEXCOORD2;
               #endif

               // #if %TEXCOORD3REQUIREKEY%
                float4 texcoord3 : TEXCOORD3;
               // #endif

               // #if %VERTEXCOLORREQUIREKEY%
               // float4 vertexColor : COLOR;
               // #endif

               #if _HDRP && (_PASSMOTIONVECTOR || ((_PASSFORWARD || _PASSUNLIT) && defined(_WRITE_TRANSPARENT_MOTION_VECTOR)))
                  float3 previousPositionOS : TEXCOORD4; // Contain previous transform position (in case of skinning for example)
                  #if defined (_ADD_PRECOMPUTED_VELOCITY)
                     float3 precomputedVelocity    : TEXCOORD5; // Add Precomputed Velocity (Alembic computes velocities on runtime side).
                  #endif
               #endif

               UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct TessVertex 
            {
               float4 vertex : INTERNALTESSPOS;
               float3 normal : NORMAL;
               float4 tangent : TANGENT;
               float4 texcoord0 : TEXCOORD0;
               float4 texcoord1 : TEXCOORD1;
               float4 texcoord2 : TEXCOORD2;

               // #if %TEXCOORD3REQUIREKEY%
                float4 texcoord3 : TEXCOORD3;
               // #endif

               // #if %VERTEXCOLORREQUIREKEY%
               // float4 vertexColor : COLOR;
               // #endif

               // #if %EXTRAV2F0REQUIREKEY%
               // float4 extraV2F0 : TEXCOORD5;
               // #endif

               // #if %EXTRAV2F1REQUIREKEY%
               // float4 extraV2F1 : TEXCOORD6;
               // #endif

               // #if %EXTRAV2F2REQUIREKEY%
               // float4 extraV2F2 : TEXCOORD7;
               // #endif

               // #if %EXTRAV2F3REQUIREKEY%
               // float4 extraV2F3 : TEXCOORD8;
               // #endif

               // #if %EXTRAV2F4REQUIREKEY%
               // float4 extraV2F4 : TEXCOORD9;
               // #endif

               // #if %EXTRAV2F5REQUIREKEY%
               // float4 extraV2F5 : TEXCOORD10;
               // #endif

               // #if %EXTRAV2F6REQUIREKEY%
               // float4 extraV2F6 : TEXCOORD11;
               // #endif

               // #if %EXTRAV2F7REQUIREKEY%
               // float4 extraV2F7 : TEXCOORD12;
               // #endif

               #if _HDRP && (_PASSMOTIONVECTOR || ((_PASSFORWARD || _PASSUNLIT) && defined(_WRITE_TRANSPARENT_MOTION_VECTOR)))
                  float3 previousPositionOS : TEXCOORD13; // Contain previous transform position (in case of skinning for example)
                  #if defined (_ADD_PRECOMPUTED_VELOCITY)
                     float3 precomputedVelocity : TEXCOORD14;
                  #endif
               #endif

               UNITY_VERTEX_INPUT_INSTANCE_ID
               UNITY_VERTEX_OUTPUT_STEREO
            };

            struct ExtraV2F
            {
               float4 extraV2F0;
               float4 extraV2F1;
               float4 extraV2F2;
               float4 extraV2F3;
               float4 extraV2F4;
               float4 extraV2F5;
               float4 extraV2F6;
               float4 extraV2F7;
               Blackboard blackboard;
               float4 time;
            };


            float3 WorldToTangentSpace(ShaderData d, float3 normal)
            {
               return mul(d.TBNMatrix, normal);
            }

            float3 TangentToWorldSpace(ShaderData d, float3 normal)
            {
               return mul(normal, d.TBNMatrix);
            }

            // in this case, make standard more like SRPs, because we can't fix
            // unity_WorldToObject in HDRP, since it already does macro-fu there

            #if _STANDARD
               float3 TransformWorldToObject(float3 p) { return mul(unity_WorldToObject, float4(p, 1)); };
               float3 TransformObjectToWorld(float3 p) { return mul(unity_ObjectToWorld, float4(p, 1)); };
               float4 TransformWorldToObject(float4 p) { return mul(unity_WorldToObject, p); };
               float4 TransformObjectToWorld(float4 p) { return mul(unity_ObjectToWorld, p); };
               float4x4 GetWorldToObjectMatrix() { return unity_WorldToObject; }
               float4x4 GetObjectToWorldMatrix() { return unity_ObjectToWorld; }
               #if (defined(SHADER_API_D3D11) || defined(SHADER_API_XBOXONE) || defined(UNITY_COMPILER_HLSLCC) || defined(SHADER_API_PSSL) || (SHADER_TARGET_SURFACE_ANALYSIS && !SHADER_TARGET_SURFACE_ANALYSIS_MOJOSHADER))
                 #define UNITY_SAMPLE_TEX2D_LOD(tex,coord, lod) tex.SampleLevel (sampler##tex,coord, lod)
                 #define UNITY_SAMPLE_TEX2D_SAMPLER_LOD(tex,samplertex,coord, lod) tex.SampleLevel (sampler##samplertex,coord, lod)
              #else
                 #define UNITY_SAMPLE_TEX2D_LOD(tex,coord,lod) tex2D (tex,coord,0,lod)
                 #define UNITY_SAMPLE_TEX2D_SAMPLER_LOD(tex,samplertex,coord,lod) tex2D (tex,coord,0,lod)
              #endif

               #undef GetWorldToObjectMatrix()

               #define GetWorldToObjectMatrix()   unity_WorldToObject


            #endif

            float3 GetCameraWorldPosition()
            {
               #if _HDRP
                  return GetCameraRelativePositionWS(_WorldSpaceCameraPos);
               #else
                  return _WorldSpaceCameraPos;
               #endif
            }

            #if _GRABPASSUSED
               #if _STANDARD
                  TEXTURE2D(%GRABTEXTURE%);
                  SAMPLER(sampler_%GRABTEXTURE%);
               #endif

               half3 GetSceneColor(float2 uv)
               {
                  #if _STANDARD
                     return SAMPLE_TEXTURE2D(%GRABTEXTURE%, sampler_%GRABTEXTURE%, uv).rgb;
                  #else
                     return SHADERGRAPH_SAMPLE_SCENE_COLOR(uv);
                  #endif
               }
            #endif


      
            #if _STANDARD
               UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);
               float GetSceneDepth(float2 uv) { return SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, uv); }
               float GetLinear01Depth(float2 uv) { return Linear01Depth(GetSceneDepth(uv)); }
               float GetLinearEyeDepth(float2 uv) { return LinearEyeDepth(GetSceneDepth(uv)); } 
            #else
               float GetSceneDepth(float2 uv) { return SHADERGRAPH_SAMPLE_SCENE_DEPTH(uv); }
               float GetLinear01Depth(float2 uv) { return Linear01Depth(GetSceneDepth(uv), _ZBufferParams); }
               float GetLinearEyeDepth(float2 uv) { return LinearEyeDepth(GetSceneDepth(uv), _ZBufferParams); } 
            #endif

            float3 GetWorldPositionFromDepthBuffer(float2 uv, float3 worldSpaceViewDir)
            {
               float eye = GetLinearEyeDepth(uv);
               float3 camView = mul((float3x3)GetObjectToWorldMatrix(), transpose(mul(GetWorldToObjectMatrix(), UNITY_MATRIX_I_V)) [2].xyz);

               float dt = dot(worldSpaceViewDir, camView);
               float3 div = worldSpaceViewDir/dt;
               float3 wpos = (eye * div) + GetCameraWorldPosition();
               return wpos;
            }

            #if _HDRP
            float3 ObjectToWorldSpacePosition(float3 pos)
            {
               return GetAbsolutePositionWS(TransformObjectToWorld(pos));
            }
            #else
            float3 ObjectToWorldSpacePosition(float3 pos)
            {
               return TransformObjectToWorld(pos);
            }
            #endif

            #if _STANDARD
               UNITY_DECLARE_SCREENSPACE_TEXTURE(_CameraDepthNormalsTexture);
               float3 GetSceneNormal(float2 uv, float3 worldSpaceViewDir)
               {
                  float4 depthNorms = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_CameraDepthNormalsTexture, uv);
                  float3 norms = DecodeViewNormalStereo(depthNorms);
                  norms = mul((float3x3)GetWorldToViewMatrix(), norms) * 0.5 + 0.5;
                  return norms;
               }
            #elif _HDRP
               float3 GetSceneNormal(float2 uv, float3 worldSpaceViewDir)
               {
                  NormalData nd;
                  DecodeFromNormalBuffer(_ScreenSize.xy * uv, nd);
                  return nd.normalWS;
               }
            #elif _URP
               #if (SHADER_LIBRARY_VERSION_MAJOR >= 10)
                  #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareNormalsTexture.hlsl"
               #endif

               float3 GetSceneNormal(float2 uv, float3 worldSpaceViewDir)
               {
                  #if (SHADER_LIBRARY_VERSION_MAJOR >= 10)
                     return SampleSceneNormals(uv);
                  #else
                     float3 wpos = GetWorldPositionFromDepthBuffer(uv, worldSpaceViewDir);
                     return normalize(-cross(ddx(wpos), ddy(wpos))) * 0.5 + 0.5;
                  #endif

                }
             #endif

             #if _HDRP

               half3 UnpackNormalmapRGorAG(half4 packednormal)
               {
                     // This do the trick
                  packednormal.x *= packednormal.w;

                  half3 normal;
                  normal.xy = packednormal.xy * 2 - 1;
                  normal.z = sqrt(1 - saturate(dot(normal.xy, normal.xy)));
                  return normal;
               }
               half3 UnpackNormal(half4 packednormal)
               {
                  #if defined(UNITY_NO_DXT5nm)
                     return packednormal.xyz * 2 - 1;
                  #else
                     return UnpackNormalmapRGorAG(packednormal);
                  #endif
               }
               #endif
               #if _HDRP || _URP

               half3 UnpackScaleNormal(half4 packednormal, half scale)
               {
                 #ifndef UNITY_NO_DXT5nm
                   // Unpack normal as DXT5nm (1, y, 1, x) or BC5 (x, y, 0, 1)
                   // Note neutral texture like "bump" is (0, 0, 1, 1) to work with both plain RGB normal and DXT5nm/BC5
                   packednormal.x *= packednormal.w;
                 #endif
                   half3 normal;
                   normal.xy = (packednormal.xy * 2 - 1) * scale;
                   normal.z = sqrt(1 - saturate(dot(normal.xy, normal.xy)));
                   return normal;
               }	

             #endif


            void GetSun(out float3 lightDir, out float3 color)
            {
               lightDir = float3(0.5, 0.5, 0);
               color = 1;
               #if _HDRP
                  if (_DirectionalLightCount > 0)
                  {
                     DirectionalLightData light = _DirectionalLightDatas[0];
                     lightDir = -light.forward.xyz;
                     color = light.color;
                  }
               #elif _STANDARD
			         lightDir = normalize(_WorldSpaceLightPos0.xyz);
                  color = _LightColor0.rgb;
               #elif _URP
	               Light light = GetMainLight();
	               lightDir = light.direction;
	               color = light.color;
               #endif
            }




            CBUFFER_START(UnityPerMaterial)

               float _StencilRef;
               float _StencilWriteMask;
               float _StencilRefDepth;
               float _StencilWriteMaskDepth;
               float _StencilRefMV;
               float _StencilWriteMaskMV;
               float _StencilRefDistortionVec;
               float _StencilWriteMaskDistortionVec;
               float _StencilWriteMaskGBuffer;
               float _StencilRefGBuffer;
               float _ZTestGBuffer;
               float _RequireSplitLighting;
               float _ReceivesSSR;
               float _ZWrite;
               float _TransparentSortPriority;
               float _ZTestDepthEqualForOpaque;
               float _ZTestTransparent;
               float _TransparentBackfaceEnable;
               float _AlphaCutoffEnable;
               float _UseShadowThreshold;

               
    float4 _BaseColorMap_ST;
    float4 _DetailMap_ST;
    float4 _EmissiveColorMap_ST;

    float _UVBase;
    half4 _Color;
    half4 _BaseColor; // TODO CHECK
    half _Cutoff; 
    half _Mode;
    //float _Cull;
    float _CullMode;
    float _CullModeForward;
    half _Metallic;
    half3 _EmissionColor;
    half _UVSec;

    float3 _EmissiveColor;
    float _AlbedoAffectEmissive;
    float _EmissiveExposureWeight;
    float _MetallicRemapMin;
    float _MetallicRemapMax;
    float _Smoothness;
    float _SmoothnessRemapMin;
    float _SmoothnessRemapMax;
    float _AlphaRemapMin;
    float _AlphaRemapMax;
    float _AORemapMin;
    float _AORemapMax;
    float _NormalScale;
    float _DetailAlbedoScale;
    float _DetailNormalScale;
    float _DetailSmoothnessScale;
    float _Anisotropy;
    float _DiffusionProfileHash;
    float _SubsurfaceMask;
    float _Thickness;
    float4 _ThicknessRemap;
    float _IridescenceThickness;
    float4 _IridescenceThicknessRemap;
    float _IridescenceMask;
    float _CoatMask;
    float4 _SpecularColor;
    float _EnergyConservingSpecularColor;
    int _MaterialID;

    float _LinkDetailsWithBase;
    float4 _UVMappingMask;
    float4 _UVDetailsMappingMask;

    float4 _UVMappingMaskEmissive;

    float _AlphaCutoff;
    //float _AlphaCutoffEnable;


    float _SyncCullMode;

    float _IsReplacementShader;
    float _TriggerMode;
    float _RaycastMode;
    float _IsExempt;
    float _isReferenceMaterial;
    float _InteractionMode;
    int _ArrayLength = 0;
    #if (defined(SHADER_API_GLES) || defined(SHADER_API_GLES3)) 
        float4 _PlayersPosVectorArray[20];
        float _PlayersDataFloatArray[150];     
    #else
        float4 _PlayersPosVectorArray[100];
        float _PlayersDataFloatArray[500];  
    #endif
    float _tDirection = 0;
    float _numOfPlayersInside = 0;
    float _tValue = 0;
    float _id = 0;
    #if _ZONING
        #if (defined(SHADER_API_GLES) || defined(SHADER_API_GLES3)) 
            float _ZDFA[500];
        #else
            float _ZDFA[1000];
        #endif
        float _ZonesDataCount;
    #endif
    #if _REPLACEMENT        
        half4 _DissolveColorGlobal;
        float _DissolveColorSaturationGlobal;
        float _DissolveEmissionGlobal;
        float _DissolveEmissionBoosterGlobal;
        float _TextureVisibilityGlobal;
        float _ObstructionGlobal;
        float _AngleStrengthGlobal;
        float _ConeStrengthGlobal;
        float _ConeObstructionDestroyRadiusGlobal;
        float _CylinderStrengthGlobal;
        float _CylinderObstructionDestroyRadiusGlobal;
        float _CircleStrengthGlobal;
        float _CircleObstructionDestroyRadiusGlobal;
        float _CurveStrengthGlobal;
        float _CurveObstructionDestroyRadiusGlobal;
        float _DissolveFallOffGlobal;
        float _IntrinsicDissolveStrengthGlobal;
        float _PreviewModeGlobal;
        float _UVsGlobal;
        float _hasClippedShadowsGlobal;
        float _FloorGlobal;
        float _FloorModeGlobal;
        float _FloorYGlobal;
        float _PlayerPosYOffsetGlobal;
        float _FloorYTextureGradientLengthGlobal;
        float _AnimationEnabledGlobal;
        float _AnimationSpeedGlobal;
        float _DefaultEffectRadiusGlobal;
        float _TransitionDurationGlobal;        
        float _TexturedEmissionEdgeGlobal;
        float _TexturedEmissionEdgeStrengthGlobal;
        float _IsometricExclusionGlobal;
        float _IsometricExclusionDistanceGlobal;
        float _IsometricExclusionGradientLengthGlobal;
        float _CeilingGlobal;
        float _CeilingModeGlobal;
        float _CeilingBlendModeGlobal;
        float _CeilingYGlobal;
        float _CeilingPlayerYOffsetGlobal;
        float _CeilingYGradientLengthGlobal;
        float _ZoningGlobal;
        float _ZoningModeGlobal;
        float _ZoningEdgeGradientLengthGlobal;
        float _IsZoningRevealableGlobal;
        float _SyncZonesWithFloorYGlobal;
        float _SyncZonesFloorYOffsetGlobal;
        float4 _ObstructionCurveGlobal_TexelSize;
        float4 _DissolveMaskGlobal_TexelSize;
        float _DissolveMaskEnabledGlobal;
        float _PreviewIndicatorLineThicknessGlobal;
    #else
        half _TextureVisibility;
        half _AngleStrength;
        float _Obstruction;
        float _UVs;
        float4 _ObstructionCurve_TexelSize;      
        float _DissolveMaskEnabled;
        float4 _DissolveMask_TexelSize;
        half4 _DissolveColor;
        float _DissolveColorSaturation;
        float _DissolveEmission;
        float _DissolveEmissionBooster;
        float _hasClippedShadows;
        float _ConeStrength;
        float _ConeObstructionDestroyRadius;
        float _CylinderStrength;
        float _CylinderObstructionDestroyRadius;
        float _CircleStrength;
        float _CircleObstructionDestroyRadius;
        float _CurveStrength;
        float _CurveObstructionDestroyRadius;
        float _IntrinsicDissolveStrength;
        float _DissolveFallOff;
        float _PreviewMode;
        float _PreviewIndicatorLineThickness;
        float _AnimationEnabled;
        float _AnimationSpeed;
        float _DefaultEffectRadius;
        float _TransitionDuration;
        float _TexturedEmissionEdge;
        float _TexturedEmissionEdgeStrength;
        float _IsometricExclusion;
        float _IsometricExclusionDistance;
        float _IsometricExclusionGradientLength;
        float _Floor;
        float _FloorMode;
        float _FloorY;
        float _FloorYTextureGradientLength;
        float _PlayerPosYOffset;
        float _Ceiling;
        float _CeilingMode;
        float _CeilingBlendMode;
        float _CeilingY;
        float _CeilingPlayerYOffset;
        float _CeilingYGradientLength;
        float _Zoning;
        float _ZoningMode;
        float _ZoningEdgeGradientLength;
        float _IsZoningRevealable;
        float _SyncZonesWithFloorY;
        float _SyncZonesFloorYOffset;
    #endif



            CBUFFER_END

            

            

            #ifdef unity_WorldToObject
#undef unity_WorldToObject
#endif
#ifdef unity_ObjectToWorld
#undef unity_ObjectToWorld
#endif
#define unity_ObjectToWorld GetObjectToWorldMatrix()
#define unity_WorldToObject GetWorldToObjectMatrix()

    sampler2D _EmissiveColorMap;
    sampler2D _BaseColorMap;
    sampler2D _MaskMap;
    sampler2D _NormalMap;
    sampler2D _NormalMapOS;
    sampler2D _DetailMap;
    sampler2D _TangentMap;
    sampler2D _TangentMapOS;
    sampler2D _AnisotropyMap;
    sampler2D _SubsurfaceMaskMap;
    sampler2D _TransmissionMaskMap;
    sampler2D _ThicknessMap;
    sampler2D _IridescenceThicknessMap;
    sampler2D _IridescenceMaskMap;
    sampler2D _SpecularColorMap;

    sampler2D _CoatMaskMap;

    float3 DecodeNormal (float4 sample, float scale) {
        #if defined(UNITY_NO_DXT5nm)
            return UnpackNormalRGB(sample, scale);
        #else
            return UnpackNormalmapRGorAG(sample, scale);
        #endif
    }

	void Ext_SurfaceFunction0 (inout Surface o, ShaderData d)
	{


//SEE: https://github.com/Unity-Technologies/Graphics/blob/6fdc7996098aa184495c0a3c0da10135bfe18340/Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitDataIndividualLayer.hlsl

        float2 uvBase = _UVMappingMask.x * d.texcoord0 +
                        _UVMappingMask.y * d.texcoord1 +
                        _UVMappingMask.z * d.texcoord2 +
                        _UVMappingMask.w * d.texcoord3;

        float2 uvDetails =  _UVDetailsMappingMask.x * d.texcoord0 +
                            _UVDetailsMappingMask.y * d.texcoord1 +
                            _UVDetailsMappingMask.z * d.texcoord2 +
                            _UVDetailsMappingMask.w * d.texcoord3;


        //TODO: add planar/triplanar

        float4 texcoords;
        //texcoords.xy = d.texcoord0.xy * _BaseColorMap_ST.xy + _BaseColorMap_ST.zw; 
        texcoords.xy = uvBase * _BaseColorMap_ST.xy + _BaseColorMap_ST.zw; 

        texcoords.zw = uvDetails * _DetailMap_ST.xy + _DetailMap_ST.zw;

        if (_LinkDetailsWithBase > 0.0)
        {
            texcoords.zw = texcoords.zw  * _BaseColorMap_ST.xy + _BaseColorMap_ST.zw;

        }

            float3 detailNormalTS = float3(0.0, 0.0, 0.0);
            float detailMask = 0.0;
            #ifdef _DETAIL_MAP
                detailMask = 1.0;
                #ifdef _MASKMAP
                    detailMask = tex2D(_MaskMap, texcoords.xy).b;
                #endif
                float2 detailAlbedoAndSmoothness = tex2D(_DetailMap, texcoords.zw).rb;
                float detailAlbedo = detailAlbedoAndSmoothness.r * 2.0 - 1.0;
                float detailSmoothness = detailAlbedoAndSmoothness.g * 2.0 - 1.0;
                real4 packedNormal = tex2D(_DetailMap, texcoords.zw).rgba;
                detailNormalTS = UnpackNormalAG(packedNormal, _DetailNormalScale);
            #endif
            float4 color = tex2D(_BaseColorMap, texcoords.xy).rgba  * _Color.rgba;
            o.Albedo = color; 
            float alpha = color.a;
            alpha = lerp(_AlphaRemapMin, _AlphaRemapMax, alpha);
            o.Alpha = alpha;
            #ifdef _DETAIL_MAP
                float albedoDetailSpeed = saturate(abs(detailAlbedo) * _DetailAlbedoScale);
                float3 baseColorOverlay = lerp(sqrt(o.Albedo), (detailAlbedo < 0.0) ? float3(0.0, 0.0, 0.0) : float3(1.0, 1.0, 1.0), albedoDetailSpeed * albedoDetailSpeed);
                baseColorOverlay *= baseColorOverlay;
                o.Albedo = lerp(o.Albedo, saturate(baseColorOverlay), detailMask);
            #endif

            #ifdef _NORMALMAP
                float3 normalTS;

                //normalTS = UnpackScaleNormal(tex2D(_NormalMap, texcoords.xy), _NormalScale);
                //#ifdef _DETAIL_MAP
                //    normalTS = lerp(normalTS, BlendNormalRNM(normalTS, normalize(detailNormalTS)), detailMask); 
                //#endif
                //o.Normal = normalTS;


                #ifdef _NORMALMAP_TANGENT_SPACE
                    //normalTS = UnpackScaleNormal(tex2D(_NormalMap, texcoords.xy), _NormalScale);
                    normalTS = DecodeNormal(tex2D(_NormalMap, texcoords.xy), _NormalScale);
                #else
                    #ifdef SURFACE_GRADIENT
                        float3 normalOS = tex2D(_NormalMapOS, texcoords.xy).xyz * 2.0 - 1.0;
                        normalTS = SurfaceGradientFromPerturbedNormal(d.worldSpaceNormal, TransformObjectToWorldNormal(normalOS));
                    #else
                        float3 normalOS = UnpackNormalRGB(tex2D(_NormalMapOS, texcoords.xy), 1.0);
                        float3 bitangent = d.tangentSign * cross(d.worldSpaceNormal.xyz, d.worldSpaceTangent.xyz);
                        half3x3 tangentToWorld = half3x3(d.worldSpaceTangent.xyz, bitangent.xyz, d.worldSpaceNormal.xyz);
                        normalTS = TransformObjectToTangent(normalOS, tangentToWorld);
                    #endif
                #endif

                #ifdef _DETAIL_MAP
                    #ifdef SURFACE_GRADIENT
                    normalTS += detailNormalTS * detailMask;
                    #else
                    normalTS = lerp(normalTS, BlendNormalRNM(normalTS, normalize(detailNormalTS)), detailMask); // todo: detailMask should lerp the angle of the quaternion rotation, not the normals
                    #endif
                #endif
                o.Normal = normalTS;

            #endif

            #if defined(_MASKMAP)
                o.Smoothness = tex2D(_MaskMap, texcoords.xy).a;
                o.Smoothness = lerp(_SmoothnessRemapMin, _SmoothnessRemapMax, o.Smoothness);
            #else
                o.Smoothness = _Smoothness;
            #endif
            #ifdef _DETAIL_MAP
                float smoothnessDetailSpeed = saturate(abs(detailSmoothness) * _DetailSmoothnessScale);
                float smoothnessOverlay = lerp(o.Smoothness, (detailSmoothness < 0.0) ? 0.0 : 1.0, smoothnessDetailSpeed);
                o.Smoothness = lerp(o.Smoothness, saturate(smoothnessOverlay), detailMask);
            #endif
            #ifdef _MASKMAP
                o.Metallic = tex2D(_MaskMap, texcoords.xy).r;
                o.Metallic = lerp(_MetallicRemapMin, _MetallicRemapMax, o.Metallic);
                o.Occlusion = tex2D(_MaskMap, texcoords.xy).g;
                o.Occlusion = lerp(_AORemapMin, _AORemapMax, o.Occlusion);
            #else
                o.Metallic = _Metallic;
                o.Occlusion = 1.0;
            #endif

            #if defined(_SPECULAR_OCCLUSION_FROM_BENT_NORMAL_MAP)
                // TODO: implenent bent normals for this to work
                //o.SpecularOcclusion = GetSpecularOcclusionFromBentAO(V, bentNormalWS, d.worldSpaceNormal, o.Occlusion, PerceptualSmoothnessToRoughness(o.Smoothness));
            #elif  defined(_MASKMAP) && !defined(_SPECULAR_OCCLUSION_NONE)
                float3 V = normalize(d.worldSpaceViewDir);
                o.SpecularOcclusion = GetSpecularOcclusionFromAmbientOcclusion(ClampNdotV(dot(d.worldSpaceNormal, V)), o.Occlusion, PerceptualSmoothnessToRoughness(o.Smoothness));
            #endif

            o.DiffusionProfileHash = asuint(_DiffusionProfileHash);
            o.SubsurfaceMask = _SubsurfaceMask;
            #ifdef _SUBSURFACE_MASK_MAP
                o.SubsurfaceMask *= tex2D(_SubsurfaceMaskMap, texcoords.xy).r;
            #endif
            #ifdef _THICKNESSMAP
                o.Thickness = tex2D(_ThicknessMap, texcoords.xy).r;
                o.Thickness = _ThicknessRemap.x + _ThicknessRemap.y * surfaceData.thickness;
            #else
                o.Thickness = _Thickness;
            #endif
            #ifdef _ANISOTROPYMAP
                o.Anisotropy = tex2D(_AnisotropyMap, texcoords.xy).r;
            #else
                o.Anisotropy = 1.0;
            #endif
                o.Anisotropy *= _Anisotropy;
                o.Specular = _SpecularColor.rgb;
            #ifdef _SPECULARCOLORMAP
                o.Specular *= tex2D(_SpecularColorMap, texcoords.xy).rgb;
            #endif
            #ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
                o.Albedo *= _EnergyConservingSpecularColor > 0.0 ? (1.0 - Max3(o.Specular.r, o.Specular.g, o.Specular.b)) : 1.0;
            #endif
            #ifdef _MATERIAL_FEATURE_CLEAR_COAT
                o.CoatMask = _CoatMask;
                o.CoatMask *= tex2D(_CoatMaskMap, texcoords.xy).r;
            #else
                o.CoatMask = 0.0;
            #endif
            #ifdef _MATERIAL_FEATURE_IRIDESCENCE
                #ifdef _IRIDESCENCE_THICKNESSMAP
                o.IridescenceThickness = tex2D(_IridescenceThicknessMap, texcoords.xy).r;
                o.IridescenceThickness = _IridescenceThicknessRemap.x + _IridescenceThicknessRemap.y * o.IridescenceThickness;
                #else
                o.IridescenceThickness = _IridescenceThickness;
                #endif
                o.IridescenceMask = _IridescenceMask;
                o.IridescenceMask *= tex2D(_IridescenceMaskMap, texcoords.xy).r;
            #else
                o.IridescenceThickness = 0.0;
                o.IridescenceMask = 0.0;
            #endif


//SEE: https://github.com/Unity-Technologies/Graphics/blob/632f80e011f18ea537ee6e2f0be3ff4f4dea6a11/Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitBuiltinData.hlsl

            float3 emissiveColor = _EmissiveColor * lerp(float3(1.0, 1.0, 1.0), o.Albedo.rgb, _AlbedoAffectEmissive);
            #ifdef _EMISSIVE_COLOR_MAP                
                float2 uvEmission = _UVMappingMaskEmissive.x * d.texcoord0 +
                                    _UVMappingMaskEmissive.y * d.texcoord1 +
                                    _UVMappingMaskEmissive.z * d.texcoord2 +
                                    _UVMappingMaskEmissive.w * d.texcoord3;

                uvEmission = uvEmission * _EmissiveColorMap_ST.xy + _EmissiveColorMap_ST.zw;                     

                emissiveColor *= tex2D(_EmissiveColorMap, uvEmission).rgb;
            #endif
            o.Emission += emissiveColor;
            float currentExposureMultiplier = 0;
            #if SHADEROPTIONS_PRE_EXPOSITION
                currentExposureMultiplier =  LOAD_TEXTURE2D(_ExposureTexture, int2(0, 0)).x * _ProbeExposureScale;
            #else
                currentExposureMultiplier =  _ProbeExposureScale;
            #endif
            float InverseCurrentExposureMultiplier = 0;
            float exposure = currentExposureMultiplier;
            InverseCurrentExposureMultiplier =  rcp(exposure + (exposure == 0.0)); 
            float3 emissiveRcpExposure = o.Emission * InverseCurrentExposureMultiplier;
            o.Emission = lerp(emissiveRcpExposure, o.Emission, _EmissiveExposureWeight);


// SEE: https://github.com/Unity-Technologies/Graphics/blob/223d8105e68c5a808027d78f39cb4e2545fd6276/Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitData.hlsl
            #if defined(_ALPHATEST_ON)
                float alphaTex = tex2D(_BaseColorMap, texcoords.xy).a;
                alphaTex = lerp(_AlphaRemapMin, _AlphaRemapMax, alphaTex);
                float alphaValue = alphaTex * _BaseColor.a;

                // Perform alha test very early to save performance (a killed pixel will not sample textures)
                //#if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
                //float alphaCutoff = _AlphaCutoffPrepass;
                //#elif SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_POSTPASS
                //float alphaCutoff = _AlphaCutoffPostpass;
                //#elif (SHADERPASS == SHADERPASS_SHADOWS) || (SHADERPASS == SHADERPASS_RAYTRACING_VISIBILITY)
                //float alphaCutoff = _UseShadowThreshold ? _AlphaCutoffShadow : _AlphaCutoff;
                //#else
                float alphaCutoff = _AlphaCutoff;
                //#endif

                // GENERIC_ALPHA_TEST(alphaValue, alphaCutoff);
                clip(alpha - alphaCutoff);
            #endif

	}




    #if _REPLACEMENT
        sampler2D _DissolveTexGlobal;
    #else
        sampler2D _DissolveTex;
    #endif

    //#if _DISSOLVEMASK
    #if _REPLACEMENT
        sampler2D _DissolveMaskGlobal;
    #else
        sampler2D _DissolveMask;
    #endif
    //#endif

    #if _REPLACEMENT
        sampler2D _ObstructionCurveGlobal;
    #else
        sampler2D _ObstructionCurve;
    #endif



    #if _URP
        half2 ParallaxOffset1Step (half h, half height, half3 viewDir)
        {
            h = h * height - height/2.0;
            half3 v = normalize(viewDir);
            v.z += 0.42;
            return h * (v.xy / v.z);
        }
        #ifdef UNITY_COLORSPACE_GAMMA
            #define unity_ColorSpaceDouble half4(2.0, 2.0, 2.0, 2.0)
        #else 
            #define unity_ColorSpaceDouble half4(4.59479380, 4.59479380, 4.59479380, 2.0)
        #endif
        half3 BlendNormals(half3 n1, half3 n2)
        {
            return normalize(half3(n1.xy + n2.xy, n1.z*n2.z));
        }
    #endif


#ifdef USE_UNITY_TEXTURE_2D_TYPE
#undef USE_UNITY_TEXTURE_2D_TYPE
#endif
#include "Assets/See-through Shader/Core/Shaders/xxSharedSTSDependencies/SeeThroughShaderFunction.hlsl"



	void Ext_SurfaceFunction1 (inout Surface o, ShaderData d)
	{
        half3 albedo;
        half3 emission;
        float alphaForClipping;
#ifdef USE_UNITY_TEXTURE_2D_TYPE
#undef USE_UNITY_TEXTURE_2D_TYPE
#endif
#if _REPLACEMENT
    DoSeeThroughShading( o.Albedo, o.Normal, d.worldSpacePosition, d.worldSpaceNormal,

                        _numOfPlayersInside, _tDirection, _tValue, _id,
                        _TriggerMode, _RaycastMode,
                        _IsExempt,

                        _DissolveColorGlobal, _DissolveColorSaturationGlobal, _UVsGlobal,
                        _DissolveEmissionGlobal, _DissolveEmissionBoosterGlobal, _TexturedEmissionEdgeGlobal, _TexturedEmissionEdgeStrengthGlobal,
                        _hasClippedShadowsGlobal,

                        _ObstructionGlobal,
                        _AngleStrengthGlobal,
                        _ConeStrengthGlobal, _ConeObstructionDestroyRadiusGlobal,
                        _CylinderStrengthGlobal, _CylinderObstructionDestroyRadiusGlobal,
                        _CircleStrengthGlobal, _CircleObstructionDestroyRadiusGlobal,
                        _CurveStrengthGlobal, _CurveObstructionDestroyRadiusGlobal,
                        _DissolveFallOffGlobal,
                        _DissolveMaskEnabledGlobal,
                        _IntrinsicDissolveStrengthGlobal,
                        _DefaultEffectRadiusGlobal,

                        _IsometricExclusionGlobal, _IsometricExclusionDistanceGlobal, _IsometricExclusionGradientLengthGlobal,
                        _CeilingGlobal, _CeilingModeGlobal, _CeilingBlendModeGlobal, _CeilingYGlobal, _CeilingPlayerYOffsetGlobal, _CeilingYGradientLengthGlobal,
                        _FloorGlobal, _FloorModeGlobal, _FloorYGlobal, _PlayerPosYOffsetGlobal, _FloorYTextureGradientLengthGlobal,

                        _AnimationEnabledGlobal, _AnimationSpeedGlobal,
                        _TransitionDurationGlobal,

                        _ZoningGlobal, _ZoningModeGlobal, _IsZoningRevealableGlobal, _ZoningEdgeGradientLengthGlobal,
                        _SyncZonesWithFloorYGlobal, _SyncZonesFloorYOffsetGlobal,

                        _PreviewModeGlobal,
                        _PreviewIndicatorLineThicknessGlobal,

                        _DissolveTexGlobal,
                        _DissolveMaskGlobal,
                        _ObstructionCurveGlobal,

                        _DissolveMaskGlobal_TexelSize,
                        _ObstructionCurveGlobal_TexelSize,

                        albedo, emission, alphaForClipping);
#else 
    
    DoSeeThroughShading( o.Albedo, o.Normal, d.worldSpacePosition, d.worldSpaceNormal,

                        _numOfPlayersInside, _tDirection, _tValue, _id,
                        _TriggerMode, _RaycastMode,
                        _IsExempt,

                        _DissolveColor, _DissolveColorSaturation, _UVs,
                        _DissolveEmission, _DissolveEmissionBooster, _TexturedEmissionEdge, _TexturedEmissionEdgeStrength,
                        _hasClippedShadows,

                        _Obstruction,
                        _AngleStrength,
                        _ConeStrength, _ConeObstructionDestroyRadius,
                        _CylinderStrength, _CylinderObstructionDestroyRadius,
                        _CircleStrength, _CircleObstructionDestroyRadius,
                        _CurveStrength, _CurveObstructionDestroyRadius,
                        _DissolveFallOff,
                        _DissolveMaskEnabled,
                        _IntrinsicDissolveStrength,
                        _DefaultEffectRadius,

                        _IsometricExclusion, _IsometricExclusionDistance, _IsometricExclusionGradientLength,
                        _Ceiling, _CeilingMode, _CeilingBlendMode, _CeilingY, _CeilingPlayerYOffset, _CeilingYGradientLength,
                        _Floor, _FloorMode, _FloorY, _PlayerPosYOffset, _FloorYTextureGradientLength,

                        _AnimationEnabled, _AnimationSpeed,
                        _TransitionDuration,

                        _Zoning, _ZoningMode, _IsZoningRevealable, _ZoningEdgeGradientLength,
                        _SyncZonesWithFloorY, _SyncZonesFloorYOffset,

                        _PreviewMode,
                        _PreviewIndicatorLineThickness,                            

                        _DissolveTex,
                        _DissolveMask,
                        _ObstructionCurve,

                        _DissolveMask_TexelSize,
                        _ObstructionCurve_TexelSize,

                        albedo, emission, alphaForClipping);
#endif 



        o.Albedo = albedo;
        o.Emission += emission;   

	}




        
            void ChainSurfaceFunction(inout Surface l, inout ShaderData d)
            {
                  Ext_SurfaceFunction0(l, d);
                  Ext_SurfaceFunction1(l, d);
                 // Ext_SurfaceFunction2(l, d);
                 // Ext_SurfaceFunction3(l, d);
                 // Ext_SurfaceFunction4(l, d);
                 // Ext_SurfaceFunction5(l, d);
                 // Ext_SurfaceFunction6(l, d);
                 // Ext_SurfaceFunction7(l, d);
                 // Ext_SurfaceFunction8(l, d);
                 // Ext_SurfaceFunction9(l, d);
		           // Ext_SurfaceFunction10(l, d);
                 // Ext_SurfaceFunction11(l, d);
                 // Ext_SurfaceFunction12(l, d);
                 // Ext_SurfaceFunction13(l, d);
                 // Ext_SurfaceFunction14(l, d);
                 // Ext_SurfaceFunction15(l, d);
                 // Ext_SurfaceFunction16(l, d);
                 // Ext_SurfaceFunction17(l, d);
                 // Ext_SurfaceFunction18(l, d);
		           // Ext_SurfaceFunction19(l, d);
                 // Ext_SurfaceFunction20(l, d);
                 // Ext_SurfaceFunction21(l, d);
                 // Ext_SurfaceFunction22(l, d);
                 // Ext_SurfaceFunction23(l, d);
                 // Ext_SurfaceFunction24(l, d);
                 // Ext_SurfaceFunction25(l, d);
                 // Ext_SurfaceFunction26(l, d);
                 // Ext_SurfaceFunction27(l, d);
                 // Ext_SurfaceFunction28(l, d);
		           // Ext_SurfaceFunction29(l, d);
            }

            void ChainModifyVertex(inout VertexData v, inout VertexToPixel v2p, float4 time)
            {
                 ExtraV2F d;
                 
                 ZERO_INITIALIZE(ExtraV2F, d);
                 ZERO_INITIALIZE(Blackboard, d.blackboard);
                 // due to motion vectors in HDRP, we need to use the last
                 // time in certain spots. So if you are going to use _Time to adjust vertices,
                 // you need to use this time or motion vectors will break. 
                 d.time = time;

                 //  Ext_ModifyVertex0(v, d);
                 // Ext_ModifyVertex1(v, d);
                 // Ext_ModifyVertex2(v, d);
                 // Ext_ModifyVertex3(v, d);
                 // Ext_ModifyVertex4(v, d);
                 // Ext_ModifyVertex5(v, d);
                 // Ext_ModifyVertex6(v, d);
                 // Ext_ModifyVertex7(v, d);
                 // Ext_ModifyVertex8(v, d);
                 // Ext_ModifyVertex9(v, d);
                 // Ext_ModifyVertex10(v, d);
                 // Ext_ModifyVertex11(v, d);
                 // Ext_ModifyVertex12(v, d);
                 // Ext_ModifyVertex13(v, d);
                 // Ext_ModifyVertex14(v, d);
                 // Ext_ModifyVertex15(v, d);
                 // Ext_ModifyVertex16(v, d);
                 // Ext_ModifyVertex17(v, d);
                 // Ext_ModifyVertex18(v, d);
                 // Ext_ModifyVertex19(v, d);
                 // Ext_ModifyVertex20(v, d);
                 // Ext_ModifyVertex21(v, d);
                 // Ext_ModifyVertex22(v, d);
                 // Ext_ModifyVertex23(v, d);
                 // Ext_ModifyVertex24(v, d);
                 // Ext_ModifyVertex25(v, d);
                 // Ext_ModifyVertex26(v, d);
                 // Ext_ModifyVertex27(v, d);
                 // Ext_ModifyVertex28(v, d);
                 // Ext_ModifyVertex29(v, d);


                 // #if %EXTRAV2F0REQUIREKEY%
                 // v2p.extraV2F0 = d.extraV2F0;
                 // #endif

                 // #if %EXTRAV2F1REQUIREKEY%
                 // v2p.extraV2F1 = d.extraV2F1;
                 // #endif

                 // #if %EXTRAV2F2REQUIREKEY%
                 // v2p.extraV2F2 = d.extraV2F2;
                 // #endif

                 // #if %EXTRAV2F3REQUIREKEY%
                 // v2p.extraV2F3 = d.extraV2F3;
                 // #endif

                 // #if %EXTRAV2F4REQUIREKEY%
                 // v2p.extraV2F4 = d.extraV2F4;
                 // #endif

                 // #if %EXTRAV2F5REQUIREKEY%
                 // v2p.extraV2F5 = d.extraV2F5;
                 // #endif

                 // #if %EXTRAV2F6REQUIREKEY%
                 // v2p.extraV2F6 = d.extraV2F6;
                 // #endif

                 // #if %EXTRAV2F7REQUIREKEY%
                 // v2p.extraV2F7 = d.extraV2F7;
                 // #endif
            }

            void ChainModifyTessellatedVertex(inout VertexData v, inout VertexToPixel v2p)
            {
               ExtraV2F d;
               ZERO_INITIALIZE(ExtraV2F, d);
               ZERO_INITIALIZE(Blackboard, d.blackboard);

               // #if %EXTRAV2F0REQUIREKEY%
               // d.extraV2F0 = v2p.extraV2F0;
               // #endif

               // #if %EXTRAV2F1REQUIREKEY%
               // d.extraV2F1 = v2p.extraV2F1;
               // #endif

               // #if %EXTRAV2F2REQUIREKEY%
               // d.extraV2F2 = v2p.extraV2F2;
               // #endif

               // #if %EXTRAV2F3REQUIREKEY%
               // d.extraV2F3 = v2p.extraV2F3;
               // #endif

               // #if %EXTRAV2F4REQUIREKEY%
               // d.extraV2F4 = v2p.extraV2F4;
               // #endif

               // #if %EXTRAV2F5REQUIREKEY%
               // d.extraV2F5 = v2p.extraV2F5;
               // #endif

               // #if %EXTRAV2F6REQUIREKEY%
               // d.extraV2F6 = v2p.extraV2F6;
               // #endif

               // #if %EXTRAV2F7REQUIREKEY%
               // d.extraV2F7 = v2p.extraV2F7;
               // #endif


               // Ext_ModifyTessellatedVertex0(v, d);
               // Ext_ModifyTessellatedVertex1(v, d);
               // Ext_ModifyTessellatedVertex2(v, d);
               // Ext_ModifyTessellatedVertex3(v, d);
               // Ext_ModifyTessellatedVertex4(v, d);
               // Ext_ModifyTessellatedVertex5(v, d);
               // Ext_ModifyTessellatedVertex6(v, d);
               // Ext_ModifyTessellatedVertex7(v, d);
               // Ext_ModifyTessellatedVertex8(v, d);
               // Ext_ModifyTessellatedVertex9(v, d);
               // Ext_ModifyTessellatedVertex10(v, d);
               // Ext_ModifyTessellatedVertex11(v, d);
               // Ext_ModifyTessellatedVertex12(v, d);
               // Ext_ModifyTessellatedVertex13(v, d);
               // Ext_ModifyTessellatedVertex14(v, d);
               // Ext_ModifyTessellatedVertex15(v, d);
               // Ext_ModifyTessellatedVertex16(v, d);
               // Ext_ModifyTessellatedVertex17(v, d);
               // Ext_ModifyTessellatedVertex18(v, d);
               // Ext_ModifyTessellatedVertex19(v, d);
               // Ext_ModifyTessellatedVertex20(v, d);
               // Ext_ModifyTessellatedVertex21(v, d);
               // Ext_ModifyTessellatedVertex22(v, d);
               // Ext_ModifyTessellatedVertex23(v, d);
               // Ext_ModifyTessellatedVertex24(v, d);
               // Ext_ModifyTessellatedVertex25(v, d);
               // Ext_ModifyTessellatedVertex26(v, d);
               // Ext_ModifyTessellatedVertex27(v, d);
               // Ext_ModifyTessellatedVertex28(v, d);
               // Ext_ModifyTessellatedVertex29(v, d);

               // #if %EXTRAV2F0REQUIREKEY%
               // v2p.extraV2F0 = d.extraV2F0;
               // #endif

               // #if %EXTRAV2F1REQUIREKEY%
               // v2p.extraV2F1 = d.extraV2F1;
               // #endif

               // #if %EXTRAV2F2REQUIREKEY%
               // v2p.extraV2F2 = d.extraV2F2;
               // #endif

               // #if %EXTRAV2F3REQUIREKEY%
               // v2p.extraV2F3 = d.extraV2F3;
               // #endif

               // #if %EXTRAV2F4REQUIREKEY%
               // v2p.extraV2F4 = d.extraV2F4;
               // #endif

               // #if %EXTRAV2F5REQUIREKEY%
               // v2p.extraV2F5 = d.extraV2F5;
               // #endif

               // #if %EXTRAV2F6REQUIREKEY%
               // v2p.extraV2F6 = d.extraV2F6;
               // #endif

               // #if %EXTRAV2F7REQUIREKEY%
               // v2p.extraV2F7 = d.extraV2F7;
               // #endif
            }

            void ChainFinalColorForward(inout Surface l, inout ShaderData d, inout half4 color)
            {
               //   Ext_FinalColorForward0(l, d, color);
               //   Ext_FinalColorForward1(l, d, color);
               //   Ext_FinalColorForward2(l, d, color);
               //   Ext_FinalColorForward3(l, d, color);
               //   Ext_FinalColorForward4(l, d, color);
               //   Ext_FinalColorForward5(l, d, color);
               //   Ext_FinalColorForward6(l, d, color);
               //   Ext_FinalColorForward7(l, d, color);
               //   Ext_FinalColorForward8(l, d, color);
               //   Ext_FinalColorForward9(l, d, color);
               //  Ext_FinalColorForward10(l, d, color);
               //  Ext_FinalColorForward11(l, d, color);
               //  Ext_FinalColorForward12(l, d, color);
               //  Ext_FinalColorForward13(l, d, color);
               //  Ext_FinalColorForward14(l, d, color);
               //  Ext_FinalColorForward15(l, d, color);
               //  Ext_FinalColorForward16(l, d, color);
               //  Ext_FinalColorForward17(l, d, color);
               //  Ext_FinalColorForward18(l, d, color);
               //  Ext_FinalColorForward19(l, d, color);
               //  Ext_FinalColorForward20(l, d, color);
               //  Ext_FinalColorForward21(l, d, color);
               //  Ext_FinalColorForward22(l, d, color);
               //  Ext_FinalColorForward23(l, d, color);
               //  Ext_FinalColorForward24(l, d, color);
               //  Ext_FinalColorForward25(l, d, color);
               //  Ext_FinalColorForward26(l, d, color);
               //  Ext_FinalColorForward27(l, d, color);
               //  Ext_FinalColorForward28(l, d, color);
               //  Ext_FinalColorForward29(l, d, color);
            }

            void ChainFinalGBufferStandard(inout Surface s, inout ShaderData d, inout half4 GBuffer0, inout half4 GBuffer1, inout half4 GBuffer2, inout half4 outEmission, inout half4 outShadowMask)
            {
               //   Ext_FinalGBufferStandard0(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard1(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard2(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard3(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard4(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard5(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard6(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard7(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard8(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard9(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard10(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard11(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard12(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard13(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard14(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard15(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard16(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard17(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard18(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard19(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard20(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard21(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard22(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard23(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard24(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard25(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard26(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard27(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard28(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard29(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
            }



            

         ShaderData CreateShaderData(VertexToPixel i
                  #if NEED_FACING
                     , bool facing
                  #endif
         )
         {
            ShaderData d = (ShaderData)0;
            d.clipPos = i.pos;
            d.worldSpacePosition = i.worldPos;

            d.worldSpaceNormal = normalize(i.worldNormal);
            d.worldSpaceTangent = normalize(i.worldTangent.xyz);
            d.tangentSign = i.worldTangent.w;
            float3 bitangent = cross(i.worldTangent.xyz, i.worldNormal) * d.tangentSign * -1;
            

            d.TBNMatrix = float3x3(d.worldSpaceTangent, bitangent, d.worldSpaceNormal);
            d.worldSpaceViewDir = normalize(_WorldSpaceCameraPos - i.worldPos);

            d.tangentSpaceViewDir = mul(d.TBNMatrix, d.worldSpaceViewDir);
             d.texcoord0 = i.texcoord0;
             d.texcoord1 = i.texcoord1;
             d.texcoord2 = i.texcoord2;

            // #if %TEXCOORD3REQUIREKEY%
             d.texcoord3 = i.texcoord3;
            // #endif

            // d.isFrontFace = facing;
            // #if %VERTEXCOLORREQUIREKEY%
            // d.vertexColor = i.vertexColor;
            // #endif

            // these rarely get used, so we back transform them. Usually will be stripped.
            #if _HDRP
                // d.localSpacePosition = mul(unity_WorldToObject, float4(GetCameraRelativePositionWS(i.worldPos), 1)).xyz;
            #else
                // d.localSpacePosition = mul(unity_WorldToObject, float4(i.worldPos, 1)).xyz;
            #endif
            // d.localSpaceNormal = normalize(mul((float3x3)unity_WorldToObject, i.worldNormal));
            // d.localSpaceTangent = normalize(mul((float3x3)unity_WorldToObject, i.worldTangent.xyz));

            // #if %SCREENPOSREQUIREKEY%
            // d.screenPos = i.screenPos;
            // d.screenUV = (i.screenPos.xy / i.screenPos.w);
            // #endif


            // #if %EXTRAV2F0REQUIREKEY%
            // d.extraV2F0 = i.extraV2F0;
            // #endif

            // #if %EXTRAV2F1REQUIREKEY%
            // d.extraV2F1 = i.extraV2F1;
            // #endif

            // #if %EXTRAV2F2REQUIREKEY%
            // d.extraV2F2 = i.extraV2F2;
            // #endif

            // #if %EXTRAV2F3REQUIREKEY%
            // d.extraV2F3 = i.extraV2F3;
            // #endif

            // #if %EXTRAV2F4REQUIREKEY%
            // d.extraV2F4 = i.extraV2F4;
            // #endif

            // #if %EXTRAV2F5REQUIREKEY%
            // d.extraV2F5 = i.extraV2F5;
            // #endif

            // #if %EXTRAV2F6REQUIREKEY%
            // d.extraV2F6 = i.extraV2F6;
            // #endif

            // #if %EXTRAV2F7REQUIREKEY%
            // d.extraV2F7 = i.extraV2F7;
            // #endif

            return d;
         }
         

            

struct VaryingsToPS
{
   VertexToPixel vmesh;
   #ifdef VARYINGS_NEED_PASS
      VaryingsPassToPS vpass;
   #endif
};

struct PackedVaryingsToPS
{
   #ifdef VARYINGS_NEED_PASS
      PackedVaryingsPassToPS vpass;
   #endif
   VertexToPixel vmesh;

   UNITY_VERTEX_OUTPUT_STEREO
};

PackedVaryingsToPS PackVaryingsToPS(VaryingsToPS input)
{
   PackedVaryingsToPS output = (PackedVaryingsToPS)0;
   output.vmesh = input.vmesh;
   #ifdef VARYINGS_NEED_PASS
      output.vpass = PackVaryingsPassToPS(input.vpass);
   #endif

   UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
   return output;
}




VertexToPixel VertMesh(VertexData input)
{
    VertexToPixel output = (VertexToPixel)0;

    UNITY_SETUP_INSTANCE_ID(input);
    UNITY_TRANSFER_INSTANCE_ID(input, output);

    
    ChainModifyVertex(input, output, _Time);


    // This return the camera relative position (if enable)
    float3 positionRWS = TransformObjectToWorld(input.vertex.xyz);
    float3 normalWS = TransformObjectToWorldNormal(input.normal);
    float4 tangentWS = float4(TransformObjectToWorldDir(input.tangent.xyz), input.tangent.w);


    output.worldPos = GetAbsolutePositionWS(positionRWS);
    output.pos = TransformWorldToHClip(positionRWS);
    output.worldNormal = normalWS;
    output.worldTangent = tangentWS;


    output.texcoord0 = input.texcoord0;
    output.texcoord1 = input.texcoord1;
    output.texcoord2 = input.texcoord2;
    // #if %TEXCOORD3REQUIREKEY%
     output.texcoord3 = input.texcoord3;
    // #endif

    // #if %SCREENPOSREQUIREKEY%
    // output.screenPos = ComputeScreenPos(output.pos, _ProjectionParams.x);
    // #endif

    // #if %VERTEXCOLORREQUIREKEY%
    // output.vertexColor = input.vertexColor;
    // #endif
    return output;
}


#if (SHADERPASS == SHADERPASS_DBUFFER_MESH)
void MeshDecalsPositionZBias(inout VaryingsToPS input)
{
#if defined(UNITY_REVERSED_Z)
    input.vmesh.pos.z -= _DecalMeshDepthBias;
#else
    input.vmesh.pos.z += _DecalMeshDepthBias;
#endif
}
#endif


#if (SHADERPASS == SHADERPASS_LIGHT_TRANSPORT)

// This was not in constant buffer in original unity, so keep outiside. But should be in as ShaderRenderPass frequency
float unity_OneOverOutputBoost;
float unity_MaxOutputValue;

CBUFFER_START(UnityMetaPass)
// x = use uv1 as raster position
// y = use uv2 as raster position
bool4 unity_MetaVertexControl;

// x = return albedo
// y = return normal
bool4 unity_MetaFragmentControl;
CBUFFER_END

PackedVaryingsToPS Vert(VertexData inputMesh)
{
    VaryingsToPS output = (VaryingsToPS)0;
    output.vmesh = (VertexToPixel)0;

    UNITY_SETUP_INSTANCE_ID(inputMesh);
    UNITY_TRANSFER_INSTANCE_ID(inputMesh, output.vmesh);

    // Output UV coordinate in vertex shader
    float2 uv = float2(0.0, 0.0);

    if (unity_MetaVertexControl.x)
    {
        uv = inputMesh.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
    }
    else if (unity_MetaVertexControl.y)
    {
        uv = inputMesh.texcoord2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
    }

    // OpenGL right now needs to actually use the incoming vertex position
    // so we create a fake dependency on it here that haven't any impact.
    output.vmesh.pos = float4(uv * 2.0 - 1.0, inputMesh.vertex.z > 0 ? 1.0e-4 : 0.0, 1.0);

#ifdef VARYINGS_NEED_POSITION_WS
    output.vmesh.worldPos = TransformObjectToWorld(inputMesh.vertex.xyz);
#endif

#ifdef VARYINGS_NEED_TANGENT_TO_WORLD
    // Normal is required for triplanar mapping
    output.vmesh.worldNormal = TransformObjectToWorldNormal(inputMesh.normal);
    // Not required but assign to silent compiler warning
    output.vmesh.worldTangent = float4(1.0, 0.0, 0.0, 0.0);
#endif

    output.vmesh.texcoord0 = inputMesh.texcoord0;
    output.vmesh.texcoord1 = inputMesh.texcoord1;
    output.vmesh.texcoord2 = inputMesh.texcoord2;
    // #if %TEXCOORD3REQUIREKEY%
     output.vmesh.texcoord3 = inputMesh.texcoord3;
    // #endif

    // #if %VERTEXCOLORREQUIREKEY%
    // output.vmesh.vertexColor = inputMesh.vertexColor;
    // #endif

    return PackVaryingsToPS(output);
}
#else

PackedVaryingsToPS Vert(VertexData inputMesh)
{
    VaryingsToPS varyingsType;
    varyingsType.vmesh = VertMesh(inputMesh);
    #if (SHADERPASS == SHADERPASS_DBUFFER_MESH)
       MeshDecalsPositionZBias(varyingsType);
    #endif
    return PackVaryingsToPS(varyingsType);
}

#endif



            

            
                FragInputs BuildFragInputs(VertexToPixel input)
                {
                    UNITY_SETUP_INSTANCE_ID(input);
                    FragInputs output;
                    ZERO_INITIALIZE(FragInputs, output);
            
                    // Init to some default value to make the computer quiet (else it output 'divide by zero' warning even if value is not used).
                    // TODO: this is a really poor workaround, but the variable is used in a bunch of places
                    // to compute normals which are then passed on elsewhere to compute other values...
                    output.tangentToWorld = k_identity3x3;
                    output.positionSS = input.pos;       // input.positionCS is SV_Position
            
                    output.positionRWS = GetCameraRelativePositionWS(input.worldPos);
                    output.tangentToWorld = BuildTangentToWorld(input.worldTangent, input.worldNormal);
                    output.texCoord0 = input.texcoord0;
                    output.texCoord1 = input.texcoord1;
                    output.texCoord2 = input.texcoord2;
            
                    return output;
                }
            
               void BuildSurfaceData(FragInputs fragInputs, inout Surface surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData, out float3 bentNormalWS)
               {
                   // setup defaults -- these are used if the graph doesn't output a value
                   ZERO_INITIALIZE(SurfaceData, surfaceData);
        
                   // specularOcclusion need to be init ahead of decal to quiet the compiler that modify the SurfaceData struct
                   // however specularOcclusion can come from the graph, so need to be init here so it can be override.
                   surfaceData.specularOcclusion = 1.0;
        
                   // copy across graph values, if defined
                   surfaceData.baseColor =                 surfaceDescription.Albedo;
                   surfaceData.perceptualSmoothness =      surfaceDescription.Smoothness;
                   surfaceData.ambientOcclusion =          surfaceDescription.Occlusion;
                   surfaceData.specularOcclusion =         surfaceDescription.SpecularOcclusion;
                   surfaceData.metallic =                  surfaceDescription.Metallic;
                   surfaceData.subsurfaceMask =            surfaceDescription.SubsurfaceMask;
                   surfaceData.thickness =                 surfaceDescription.Thickness;
                   surfaceData.diffusionProfileHash =      asuint(surfaceDescription.DiffusionProfileHash);
                   #if _USESPECULAR
                      surfaceData.specularColor =             surfaceDescription.Specular;
                   #endif
                   surfaceData.coatMask =                  surfaceDescription.CoatMask;
                   surfaceData.anisotropy =                surfaceDescription.Anisotropy;
                   surfaceData.iridescenceMask =           surfaceDescription.IridescenceMask;
                   surfaceData.iridescenceThickness =      surfaceDescription.IridescenceThickness;
        
           #ifdef _HAS_REFRACTION
                   if (_EnableSSRefraction)
                   {
                       // surfaceData.ior =                       surfaceDescription.RefractionIndex;
                       // surfaceData.transmittanceColor =        surfaceDescription.RefractionColor;
                       // surfaceData.atDistance =                surfaceDescription.RefractionDistance;
        
                       surfaceData.transmittanceMask = (1.0 - surfaceDescription.Alpha);
                       surfaceDescription.Alpha = 1.0;
                   }
                   else
                   {
                       surfaceData.ior = surfaceDescription.ior;
                       surfaceData.transmittanceColor = surfaceDescription.transmittanceColor;
                       surfaceData.atDistance = surfaceDescription.atDistance;
                       surfaceData.transmittanceMask = surfaceDescription.transmittanceMask;
                       surfaceDescription.Alpha = 1.0;
                   }
           #else
                   surfaceData.ior = 1.0;
                   surfaceData.transmittanceColor = float3(1.0, 1.0, 1.0);
                   surfaceData.atDistance = 1.0;
                   surfaceData.transmittanceMask = 0.0;
           #endif
                
                   // These static material feature allow compile time optimization
                   surfaceData.materialFeatures = MATERIALFEATUREFLAGS_LIT_STANDARD;
           #ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
                   surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SUBSURFACE_SCATTERING;
           #endif
           #ifdef _MATERIAL_FEATURE_TRANSMISSION
                   surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_TRANSMISSION;
           #endif
           #ifdef _MATERIAL_FEATURE_ANISOTROPY
                   surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_ANISOTROPY;
           #endif
                   // surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_CLEAR_COAT;
        
           #ifdef _MATERIAL_FEATURE_IRIDESCENCE
                   surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_IRIDESCENCE;
           #endif
           #ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
                   surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SPECULAR_COLOR;
           #endif
        
           #if defined (_MATERIAL_FEATURE_SPECULAR_COLOR) && defined (_ENERGY_CONSERVING_SPECULAR)
                   // Require to have setup baseColor
                   // Reproduce the energy conservation done in legacy Unity. Not ideal but better for compatibility and users can unchek it
                   surfaceData.baseColor *= (1.0 - Max3(surfaceData.specularColor.r, surfaceData.specularColor.g, surfaceData.specularColor.b));
           #endif

        
                   // tangent-space normal
                   float3 normalTS = float3(0.0f, 0.0f, 1.0f);
                   normalTS = surfaceDescription.Normal;
        
                   // compute world space normal
                   #if !_WORLDSPACENORMAL
                      surfaceData.normalWS = mul(normalTS, fragInputs.tangentToWorld);
                   #else
                      surfaceData.normalWS = normalTS;  
                   #endif
                   surfaceData.geomNormalWS = fragInputs.tangentToWorld[2];
        
                   surfaceData.tangentWS = normalize(fragInputs.tangentToWorld[0].xyz);    // The tangent is not normalize in tangentToWorld for mikkt. TODO: Check if it expected that we normalize with Morten. Tag: SURFACE_GRADIENT
                   // surfaceData.tangentWS = TransformTangentToWorld(surfaceDescription.Tangent, fragInputs.tangentToWorld);
        
           #if HAVE_DECALS
                   if (_EnableDecals)
                   {
                       #if VERSION_GREATER_EQUAL(10,2)
                          DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput,  surfaceData.geomNormalWS, surfaceDescription.Alpha);
                          ApplyDecalToSurfaceData(decalSurfaceData,  surfaceData.geomNormalWS, surfaceData);
                       #else
                          DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, surfaceDescription.Alpha);
                          ApplyDecalToSurfaceData(decalSurfaceData, surfaceData);
                       #endif
                   }
           #endif
        
                   bentNormalWS = surfaceData.normalWS;
               
                   surfaceData.tangentWS = Orthonormalize(surfaceData.tangentWS, surfaceData.normalWS);
        
        
                   // By default we use the ambient occlusion with Tri-ace trick (apply outside) for specular occlusion.
                   // If user provide bent normal then we process a better term
           #if defined(_SPECULAR_OCCLUSION_CUSTOM)
                   // Just use the value passed through via the slot (not active otherwise)
           #elif defined(_SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL)
                   // If we have bent normal and ambient occlusion, process a specular occlusion
                   surfaceData.specularOcclusion = GetSpecularOcclusionFromBentAO(V, bentNormalWS, surfaceData.normalWS, surfaceData.ambientOcclusion, PerceptualSmoothnessToPerceptualRoughness(surfaceData.perceptualSmoothness));
           #elif defined(_AMBIENT_OCCLUSION) && defined(_SPECULAR_OCCLUSION_FROM_AO)
                   surfaceData.specularOcclusion = GetSpecularOcclusionFromAmbientOcclusion(ClampNdotV(dot(surfaceData.normalWS, V)), surfaceData.ambientOcclusion, PerceptualSmoothnessToRoughness(surfaceData.perceptualSmoothness));
           #endif
        
           #ifdef _ENABLE_GEOMETRIC_SPECULAR_AA
                   surfaceData.perceptualSmoothness = GeometricNormalFiltering(surfaceData.perceptualSmoothness, fragInputs.tangentToWorld[2], surfaceDescription.SpecularAAScreenSpaceVariance, surfaceDescription.SpecularAAThreshold);
           #endif
        
           #ifdef DEBUG_DISPLAY
                   if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
                   {
                       // TODO: need to update mip info
                       surfaceData.metallic = 0;
                   }
        
                   // We need to call ApplyDebugToSurfaceData after filling the surfarcedata and before filling builtinData
                   // as it can modify attribute use for static lighting
                   ApplyDebugToSurfaceData(fragInputs.tangentToWorld, surfaceData);
           #endif
               }
        
               void GetSurfaceAndBuiltinData(VertexToPixel m2ps, FragInputs fragInputs, float3 V, inout PositionInputs posInput,
                     out SurfaceData surfaceData, out BuiltinData builtinData, inout Surface l, inout ShaderData d
                     #if NEED_FACING
                        , bool facing
                     #endif
                  )
               {
                 // Removed since crossfade does not work, probably needs extra material setup.   
                 //#ifdef LOD_FADE_CROSSFADE // enable dithering LOD transition if user select CrossFade transition in LOD group
                 //    uint3 fadeMaskSeed = asuint((int3)(V * _ScreenSize.xyx)); // Quantize V to _ScreenSize values
                 //    LODDitheringTransition(fadeMaskSeed, unity_LODFade.x);
                 //#endif
        
                 d = CreateShaderData(m2ps
                  #if NEED_FACING
                    , facing
                  #endif
                 );

                 

                 l = (Surface)0;

                 l.Albedo = half3(0.5, 0.5, 0.5);
                 l.Normal = float3(0,0,1);
                 l.Occlusion = 1;
                 l.Alpha = 1;
                 l.SpecularOcclusion = 1;

                 #ifdef _DEPTHOFFSET_ON
                    l.outputDepth = posInput.deviceDepth;
                 #endif

                 ChainSurfaceFunction(l, d);

                 #ifdef _DEPTHOFFSET_ON
                    posInput.deviceDepth = l.outputDepth;
                 #endif

                 #if _UNLIT
                     //l.Emission = l.Albedo;
                     //l.Albedo = 0;
                     l.Normal = half3(0,0,1);
                     l.Occlusion = 1;
                     l.Metallic = 0;
                     l.Specular = 0;
                 #endif

                 surfaceData.geomNormalWS = d.worldSpaceNormal;
                 surfaceData.tangentWS = d.worldSpaceTangent;
                 fragInputs.tangentToWorld = d.TBNMatrix;

                 float3 bentNormalWS;
                 BuildSurfaceData(fragInputs, l, V, posInput, surfaceData, bentNormalWS);

                 InitBuiltinData(posInput, l.Alpha, bentNormalWS, -d.worldSpaceNormal, fragInputs.texCoord1, fragInputs.texCoord2, builtinData);

                 

                 builtinData.emissiveColor = l.Emission;

                 #if defined(_OVERRIDE_BAKEDGI)
                    builtinData.bakeDiffuseLighting = l.DiffuseGI;
                    builtinData.backBakeDiffuseLighting = l.BackDiffuseGI;
                    builtinData.emissiveColor += l.SpecularGI;
                 #endif
                 
                 #if defined(_OVERRIDE_SHADOWMASK)
                   builtinData.shadowMask0 = l.ShadowMask.x;
                   builtinData.shadowMask1 = l.ShadowMask.y;
                   builtinData.shadowMask2 = l.ShadowMask.z;
                   builtinData.shadowMask3 = l.ShadowMask.w;
                #endif
        
                 #if (SHADERPASS == SHADERPASS_DISTORTION)
                     //builtinData.distortion = surfaceDescription.Distortion;
                     //builtinData.distortionBlur = surfaceDescription.DistortionBlur;
                     builtinData.distortion = float2(0.0, 0.0);
                     builtinData.distortionBlur = 0.0;
                 #else
                     builtinData.distortion = float2(0.0, 0.0);
                     builtinData.distortionBlur = 0.0;
                 #endif
        
                   PostInitBuiltinData(V, posInput, surfaceData, builtinData);
               }
        


              void Frag(  PackedVaryingsToPS packedInput
                          #ifdef WRITE_NORMAL_BUFFER
                          , out float4 outNormalBuffer : SV_Target0
                              #ifdef WRITE_MSAA_DEPTH
                              , out float1 depthColor : SV_Target1
                              #endif
                          #elif defined(WRITE_MSAA_DEPTH) // When only WRITE_MSAA_DEPTH is define and not WRITE_NORMAL_BUFFER it mean we are Unlit and only need depth, but we still have normal buffer binded
                          , out float4 outNormalBuffer : SV_Target0
                          , out float1 depthColor : SV_Target1
                          #elif defined(SCENESELECTIONPASS)
                          , out float4 outColor : SV_Target0
                          #endif

                          #ifdef _DEPTHOFFSET_ON
                          , out float outputDepth : SV_Depth
                          #endif
                          #if NEED_FACING
                          , bool facing : SV_IsFrontFace
                          #endif
                      )
              {
                  UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(packedInput);
                  FragInputs input = BuildFragInputs(packedInput.vmesh);

                  // input.positionSS is SV_Position
                  PositionInputs posInput = GetPositionInput(input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS);

   
                  float3 V = GetWorldSpaceNormalizeViewDir(input.positionRWS);


                  SurfaceData surfaceData;
                  BuiltinData builtinData;
                  Surface l;
                  ShaderData d;
                  GetSurfaceAndBuiltinData(packedInput.vmesh, input, V, posInput, surfaceData, builtinData, l, d
               #if NEED_FACING
                  , facing
               #endif
               );



              #ifdef _DEPTHOFFSET_ON
                  outputDepth = posInput.deviceDepth;
              #endif

              #ifdef WRITE_NORMAL_BUFFER
                  EncodeIntoNormalBuffer(ConvertSurfaceDataToNormalData(surfaceData), posInput.positionSS, outNormalBuffer);
                  #ifdef WRITE_MSAA_DEPTH
                  // In case we are rendering in MSAA, reading the an MSAA depth buffer is way too expensive. To avoid that, we export the depth to a color buffer
                  depthColor = packedInput.vmesh.pos.z;
                  #endif
              #elif defined(WRITE_MSAA_DEPTH) // When we are MSAA depth only without normal buffer
                  // Due to the binding order of these two render targets, we need to have them both declared
                  outNormalBuffer = float4(0.0, 0.0, 0.0, 1.0);
                  // In case we are rendering in MSAA, reading the an MSAA depth buffer is way too expensive. To avoid that, we export the depth to a color buffer
                  depthColor = packedInput.vmesh.pos.z;
              #elif defined(SCENESELECTIONPASS)
                  // We use depth prepass for scene selection in the editor, this code allow to output the outline correctly
                  outColor = float4(_ObjectId, _PassValue, 1.0, 1.0);
              #endif
              }




            ENDHLSL
        }
        
      Pass
        {
            // based on HDLitPass.template
            Name "META"
            Tags { "LightMode" = "META" }
            
            Cull Off
        
                Cull [_CullMode]

        
            //-------------------------------------------------------------------------------------
            // End Render Modes
            //-------------------------------------------------------------------------------------
        
            HLSLPROGRAM
        
            #pragma target 4.5
            #pragma only_renderers d3d11 ps4 xboxone vulkan metal switch
            //#pragma enable_d3d11_debug_symbols
        
            #pragma multi_compile_instancing

            //#pragma multi_compile_local _ _ALPHATEST_ON


            //#pragma shader_feature _SURFACE_TYPE_TRANSPARENT
            //#pragma shader_feature_local _ _BLENDMODE_ALPHA _BLENDMODE_ADD _BLENDMODE_PRE_MULTIPLY
        
            //-------------------------------------------------------------------------------------
            // Variant Definitions (active field translations to HDRP defines)
            //-------------------------------------------------------------------------------------
            // #define _MATERIAL_FEATURE_SUBSURFACE_SCATTERING 1
            // #define _MATERIAL_FEATURE_TRANSMISSION 1
            // #define _MATERIAL_FEATURE_ANISOTROPY 1
            // #define _MATERIAL_FEATURE_IRIDESCENCE 1
            // #define _MATERIAL_FEATURE_SPECULAR_COLOR 1
            #define _ENABLE_FOG_ON_TRANSPARENT 1
            // #define _AMBIENT_OCCLUSION 1
            // #define _SPECULAR_OCCLUSION_FROM_AO 1
            // #define _SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL 1
            // #define _SPECULAR_OCCLUSION_CUSTOM 1
            // #define _ENERGY_CONSERVING_SPECULAR 1
            // #define _ENABLE_GEOMETRIC_SPECULAR_AA 1
            // #define _HAS_REFRACTION 1
            // #define _REFRACTION_PLANE 1
            // #define _REFRACTION_SPHERE 1
            // #define _DISABLE_DECALS 1
            // #define _DISABLE_SSR 1
            // #define _ADD_PRECOMPUTED_VELOCITY
            // #define _WRITE_TRANSPARENT_MOTION_VECTOR 1
            // #define _DEPTHOFFSET_ON 1
            // #define _BLENDMODE_PRESERVE_SPECULAR_LIGHTING 1

            #define SHADERPASS SHADERPASS_LIGHT_TRANSPORT
            #define RAYTRACING_SHADER_GRAPH_HIGH
            #define REQUIRE_DEPTH_TEXTURE
            #define _PASSMETA 1

        
            


    #pragma shader_feature_local _ALPHATEST_ON

    #pragma shader_feature_local_fragment _NORMALMAP_TANGENT_SPACE


    #pragma shader_feature_local _NORMALMAP
    #pragma shader_feature_local_fragment _MASKMAP
    #pragma shader_feature_local_fragment _EMISSIVE_COLOR_MAP
    #pragma shader_feature_local_fragment _ANISOTROPYMAP
    #pragma shader_feature_local_fragment _DETAIL_MAP
    #pragma shader_feature_local_fragment _SUBSURFACE_MASK_MAP
    #pragma shader_feature_local_fragment _THICKNESSMAP
    #pragma shader_feature_local_fragment _IRIDESCENCE_THICKNESSMAP
    #pragma shader_feature_local_fragment _SPECULARCOLORMAP

    // MaterialFeature are used as shader feature to allow compiler to optimize properly
    #pragma shader_feature_local_fragment _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
    #pragma shader_feature_local_fragment _MATERIAL_FEATURE_TRANSMISSION
    #pragma shader_feature_local_fragment _MATERIAL_FEATURE_ANISOTROPY
    #pragma shader_feature_local_fragment _MATERIAL_FEATURE_CLEAR_COAT
    #pragma shader_feature_local_fragment _MATERIAL_FEATURE_IRIDESCENCE
    #pragma shader_feature_local_fragment _MATERIAL_FEATURE_SPECULAR_COLOR


    #pragma shader_feature_local_fragment _ENABLESPECULAROCCLUSION
    #pragma shader_feature_local_fragment _ _SPECULAR_OCCLUSION_NONE _SPECULAR_OCCLUSION_FROM_BENT_NORMAL_MAP

    #ifdef _ENABLESPECULAROCCLUSION
        #define _SPECULAR_OCCLUSION_FROM_BENT_NORMAL_MAP
    #endif


        #pragma shader_feature_local_fragment _OBSTRUCTION_CURVE
        #pragma shader_feature_local_fragment _DISSOLVEMASK
        #pragma shader_feature_local_fragment _ZONING
        #pragma shader_feature_local_fragment _REPLACEMENT
        #pragma shader_feature_local_fragment _PLAYERINDEPENDENT


   #define _HDRP 1
#define _USINGTEXCOORD1 1
#define _USINGTEXCOORD2 1


               #pragma vertex Vert
   #pragma fragment Frag
        

            

                  // useful conversion functions to make surface shader code just work

      #define UNITY_DECLARE_TEX2D(name) TEXTURE2D(name); SAMPLER(sampler##name);
      #define UNITY_DECLARE_TEX2D_NOSAMPLER(name) TEXTURE2D(name);
      #define UNITY_DECLARE_TEX2DARRAY(name) TEXTURE2D_ARRAY(name); SAMPLER(sampler##name);
      #define UNITY_DECLARE_TEX2DARRAY_NOSAMPLER(tex) TEXTURE2D_ARRAY(tex);

      #define UNITY_SAMPLE_TEX2DARRAY(tex,coord)            SAMPLE_TEXTURE2D_ARRAY(tex, sampler##tex, coord.xy, coord.z)
      #define UNITY_SAMPLE_TEX2DARRAY_LOD(tex,coord,lod)    SAMPLE_TEXTURE2D_ARRAY_LOD(tex, sampler##tex, coord.xy, coord.z, lod)
      #define UNITY_SAMPLE_TEX2D(tex, coord)                SAMPLE_TEXTURE2D(tex, sampler##tex, coord)
      #define UNITY_SAMPLE_TEX2D_SAMPLER(tex, samp, coord)  SAMPLE_TEXTURE2D(tex, sampler##samp, coord)

      #define UNITY_SAMPLE_TEX2D_LOD(tex,coord, lod)   SAMPLE_TEXTURE2D_LOD(tex, sampler_##tex, coord, lod)
      #define UNITY_SAMPLE_TEX2D_SAMPLER_LOD(tex,samplertex,coord, lod) SAMPLE_TEXTURE2D_LOD (tex, sampler##samplertex,coord, lod)

      #if defined(UNITY_COMPILER_HLSL)
         #define UNITY_INITIALIZE_OUTPUT(type,name) name = (type)0;
      #else
         #define UNITY_INITIALIZE_OUTPUT(type,name)
      #endif

      #define sampler2D_float sampler2D
      #define sampler2D_half sampler2D

      #undef WorldNormalVector
      #define WorldNormalVector(data, normal) mul(normal, data.TBNMatrix)

      #define UnityObjectToWorldNormal(normal) mul(GetObjectToWorldMatrix(), normal)




// HDRP Adapter stuff


            // If we use subsurface scattering, enable output split lighting (for forward pass)
            #if defined(_MATERIAL_FEATURE_SUBSURFACE_SCATTERING) && !defined(_SURFACE_TYPE_TRANSPARENT)
            #define OUTPUT_SPLIT_LIGHTING
            #endif

            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Version.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
        
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"
        
            // define FragInputs structure
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
            #if (SHADER_LIBRARY_VERSION_MAJOR >= 10)
               #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/Functions.hlsl"
            #endif


        

            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
        #ifdef DEBUG_DISPLAY
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
        #endif
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
        
        #if (SHADERPASS == SHADERPASS_FORWARD)
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/Lighting.hlsl"
        
            #define HAS_LIGHTLOOP
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/LightLoopDef.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/LightLoop.hlsl"
        #else
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
        #endif
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalUtilities.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitDecalData.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"
        
            // Used by SceneSelectionPass
            int _ObjectId;
            int _PassValue;
        
           
            // data across stages, stripped like the above.
            struct VertexToPixel
            {
               float4 pos : SV_POSITION;
               float3 worldPos : TEXCOORD0;
               float3 worldNormal : TEXCOORD1;
               float4 worldTangent : TEXCOORD2;
               float4 texcoord0 : TEXCOORD3;
               float4 texcoord1 : TEXCOORD4;
               float4 texcoord2 : TEXCOORD5;

               // #if %TEXCOORD3REQUIREKEY%
                float4 texcoord3 : TEXCOORD6;
               // #endif

               // #if %SCREENPOSREQUIREKEY%
               // float4 screenPos : TEXCOORD7;
               // #endif

               // #if %VERTEXCOLORREQUIREKEY%
               // float4 vertexColor : COLOR;
               // #endif

               // #if %EXTRAV2F0REQUIREKEY%
               // float4 extraV2F0 : TEXCOORD8;
               // #endif

               // #if %EXTRAV2F1REQUIREKEY%
               // float4 extraV2F1 : TEXCOORD9;
               // #endif

               // #if %EXTRAV2F2REQUIREKEY%
               // float4 extraV2F2 : TEXCOORD10;
               // #endif

               // #if %EXTRAV2F3REQUIREKEY%
               // float4 extraV2F3 : TEXCOORD11;
               // #endif

               // #if %EXTRAV2F4REQUIREKEY%
               // float4 extraV2F4 : TEXCOORD12;
               // #endif

               // #if %EXTRAV2F5REQUIREKEY%
               // float4 extraV2F5 : TEXCOORD13;
               // #endif

               // #if %EXTRAV2F6REQUIREKEY%
               // float4 extraV2F6 : TEXCOORD14;
               // #endif

               // #if %EXTRAV2F7REQUIREKEY%
               // float4 extraV2F7 : TEXCOORD15;
               // #endif

               #if UNITY_ANY_INSTANCING_ENABLED
                  UNITY_VERTEX_INPUT_INSTANCE_ID
               #endif // UNITY_ANY_INSTANCING_ENABLED

               UNITY_VERTEX_OUTPUT_STEREO
            };


  
            
            
            // data describing the user output of a pixel
            struct Surface
            {
               half3 Albedo;
               half Height;
               half3 Normal;
               half Smoothness;
               half3 Emission;
               half Metallic;
               half3 Specular;
               half Occlusion;
               half SpecularPower; // for simple lighting
               half Alpha;
               float outputDepth; // if written, SV_Depth semantic is used. ShaderData.clipPos.z is unused value
               // HDRP Only
               half SpecularOcclusion;
               half SubsurfaceMask;
               half Thickness;
               half CoatMask;
               half CoatSmoothness;
               half Anisotropy;
               half IridescenceMask;
               half IridescenceThickness;
               int DiffusionProfileHash;
               float SpecularAAThreshold;
               float SpecularAAScreenSpaceVariance;
               // requires _OVERRIDE_BAKEDGI to be defined, but is mapped in all pipelines
               float3 DiffuseGI;
               float3 BackDiffuseGI;
               float3 SpecularGI;
               float ior;
               float3 transmittanceColor;
               float atDistance;
               float transmittanceMask;
               // requires _OVERRIDE_SHADOWMASK to be defines
               float4 ShadowMask;


            };

            // Data the user declares in blackboard blocks
            struct Blackboard
            {
                
                float blackboardDummyData;
            };

            // data the user might need, this will grow to be big. But easy to strip
            struct ShaderData
            {
               float4 clipPos; // SV_POSITION
               float3 localSpacePosition;
               float3 localSpaceNormal;
               float3 localSpaceTangent;
        
               float3 worldSpacePosition;
               float3 worldSpaceNormal;
               float3 worldSpaceTangent;
               float tangentSign;

               float3 worldSpaceViewDir;
               float3 tangentSpaceViewDir;

               float4 texcoord0;
               float4 texcoord1;
               float4 texcoord2;
               float4 texcoord3;

               float2 screenUV;
               float4 screenPos;

               float4 vertexColor;
               bool isFrontFace;

               float4 extraV2F0;
               float4 extraV2F1;
               float4 extraV2F2;
               float4 extraV2F3;
               float4 extraV2F4;
               float4 extraV2F5;
               float4 extraV2F6;
               float4 extraV2F7;

               float3x3 TBNMatrix;
               Blackboard blackboard;
            };

            struct VertexData
            {
               #if SHADER_TARGET > 30
               // UNITY_VERTEX_INPUT_INSTANCE_ID;
               #endif
               float4 vertex : POSITION;
               float3 normal : NORMAL;
               float4 tangent : TANGENT;
               float4 texcoord0 : TEXCOORD0;

               // optimize out mesh coords when not in use by user or lighting system
               #if _URP && (_USINGTEXCOORD1 || _PASSMETA || _PASSFORWARD || _PASSGBUFFER)
                  float4 texcoord1 : TEXCOORD1;
               #endif

               #if _URP && (_USINGTEXCOORD2 || _PASSMETA || ((_PASSFORWARD || _PASSGBUFFER) && defined(DYNAMICLIGHTMAP_ON)))
                  float4 texcoord2 : TEXCOORD2;
               #endif

               #if _STANDARD && (_USINGTEXCOORD1 || (_PASSMETA || ((_PASSFORWARD || _PASSGBUFFER || _PASSFORWARDADD) && LIGHTMAP_ON)))
                  float4 texcoord1 : TEXCOORD1;
               #endif
               #if _STANDARD && (_USINGTEXCOORD2 || (_PASSMETA || ((_PASSFORWARD || _PASSGBUFFER) && DYNAMICLIGHTMAP_ON)))
                  float4 texcoord2 : TEXCOORD2;
               #endif


               #if _HDRP
                  float4 texcoord1 : TEXCOORD1;
                  float4 texcoord2 : TEXCOORD2;
               #endif

               // #if %TEXCOORD3REQUIREKEY%
                float4 texcoord3 : TEXCOORD3;
               // #endif

               // #if %VERTEXCOLORREQUIREKEY%
               // float4 vertexColor : COLOR;
               // #endif

               #if _HDRP && (_PASSMOTIONVECTOR || ((_PASSFORWARD || _PASSUNLIT) && defined(_WRITE_TRANSPARENT_MOTION_VECTOR)))
                  float3 previousPositionOS : TEXCOORD4; // Contain previous transform position (in case of skinning for example)
                  #if defined (_ADD_PRECOMPUTED_VELOCITY)
                     float3 precomputedVelocity    : TEXCOORD5; // Add Precomputed Velocity (Alembic computes velocities on runtime side).
                  #endif
               #endif

               UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct TessVertex 
            {
               float4 vertex : INTERNALTESSPOS;
               float3 normal : NORMAL;
               float4 tangent : TANGENT;
               float4 texcoord0 : TEXCOORD0;
               float4 texcoord1 : TEXCOORD1;
               float4 texcoord2 : TEXCOORD2;

               // #if %TEXCOORD3REQUIREKEY%
                float4 texcoord3 : TEXCOORD3;
               // #endif

               // #if %VERTEXCOLORREQUIREKEY%
               // float4 vertexColor : COLOR;
               // #endif

               // #if %EXTRAV2F0REQUIREKEY%
               // float4 extraV2F0 : TEXCOORD5;
               // #endif

               // #if %EXTRAV2F1REQUIREKEY%
               // float4 extraV2F1 : TEXCOORD6;
               // #endif

               // #if %EXTRAV2F2REQUIREKEY%
               // float4 extraV2F2 : TEXCOORD7;
               // #endif

               // #if %EXTRAV2F3REQUIREKEY%
               // float4 extraV2F3 : TEXCOORD8;
               // #endif

               // #if %EXTRAV2F4REQUIREKEY%
               // float4 extraV2F4 : TEXCOORD9;
               // #endif

               // #if %EXTRAV2F5REQUIREKEY%
               // float4 extraV2F5 : TEXCOORD10;
               // #endif

               // #if %EXTRAV2F6REQUIREKEY%
               // float4 extraV2F6 : TEXCOORD11;
               // #endif

               // #if %EXTRAV2F7REQUIREKEY%
               // float4 extraV2F7 : TEXCOORD12;
               // #endif

               #if _HDRP && (_PASSMOTIONVECTOR || ((_PASSFORWARD || _PASSUNLIT) && defined(_WRITE_TRANSPARENT_MOTION_VECTOR)))
                  float3 previousPositionOS : TEXCOORD13; // Contain previous transform position (in case of skinning for example)
                  #if defined (_ADD_PRECOMPUTED_VELOCITY)
                     float3 precomputedVelocity : TEXCOORD14;
                  #endif
               #endif

               UNITY_VERTEX_INPUT_INSTANCE_ID
               UNITY_VERTEX_OUTPUT_STEREO
            };

            struct ExtraV2F
            {
               float4 extraV2F0;
               float4 extraV2F1;
               float4 extraV2F2;
               float4 extraV2F3;
               float4 extraV2F4;
               float4 extraV2F5;
               float4 extraV2F6;
               float4 extraV2F7;
               Blackboard blackboard;
               float4 time;
            };


            float3 WorldToTangentSpace(ShaderData d, float3 normal)
            {
               return mul(d.TBNMatrix, normal);
            }

            float3 TangentToWorldSpace(ShaderData d, float3 normal)
            {
               return mul(normal, d.TBNMatrix);
            }

            // in this case, make standard more like SRPs, because we can't fix
            // unity_WorldToObject in HDRP, since it already does macro-fu there

            #if _STANDARD
               float3 TransformWorldToObject(float3 p) { return mul(unity_WorldToObject, float4(p, 1)); };
               float3 TransformObjectToWorld(float3 p) { return mul(unity_ObjectToWorld, float4(p, 1)); };
               float4 TransformWorldToObject(float4 p) { return mul(unity_WorldToObject, p); };
               float4 TransformObjectToWorld(float4 p) { return mul(unity_ObjectToWorld, p); };
               float4x4 GetWorldToObjectMatrix() { return unity_WorldToObject; }
               float4x4 GetObjectToWorldMatrix() { return unity_ObjectToWorld; }
               #if (defined(SHADER_API_D3D11) || defined(SHADER_API_XBOXONE) || defined(UNITY_COMPILER_HLSLCC) || defined(SHADER_API_PSSL) || (SHADER_TARGET_SURFACE_ANALYSIS && !SHADER_TARGET_SURFACE_ANALYSIS_MOJOSHADER))
                 #define UNITY_SAMPLE_TEX2D_LOD(tex,coord, lod) tex.SampleLevel (sampler##tex,coord, lod)
                 #define UNITY_SAMPLE_TEX2D_SAMPLER_LOD(tex,samplertex,coord, lod) tex.SampleLevel (sampler##samplertex,coord, lod)
              #else
                 #define UNITY_SAMPLE_TEX2D_LOD(tex,coord,lod) tex2D (tex,coord,0,lod)
                 #define UNITY_SAMPLE_TEX2D_SAMPLER_LOD(tex,samplertex,coord,lod) tex2D (tex,coord,0,lod)
              #endif

               #undef GetWorldToObjectMatrix()

               #define GetWorldToObjectMatrix()   unity_WorldToObject


            #endif

            float3 GetCameraWorldPosition()
            {
               #if _HDRP
                  return GetCameraRelativePositionWS(_WorldSpaceCameraPos);
               #else
                  return _WorldSpaceCameraPos;
               #endif
            }

            #if _GRABPASSUSED
               #if _STANDARD
                  TEXTURE2D(%GRABTEXTURE%);
                  SAMPLER(sampler_%GRABTEXTURE%);
               #endif

               half3 GetSceneColor(float2 uv)
               {
                  #if _STANDARD
                     return SAMPLE_TEXTURE2D(%GRABTEXTURE%, sampler_%GRABTEXTURE%, uv).rgb;
                  #else
                     return SHADERGRAPH_SAMPLE_SCENE_COLOR(uv);
                  #endif
               }
            #endif


      
            #if _STANDARD
               UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);
               float GetSceneDepth(float2 uv) { return SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, uv); }
               float GetLinear01Depth(float2 uv) { return Linear01Depth(GetSceneDepth(uv)); }
               float GetLinearEyeDepth(float2 uv) { return LinearEyeDepth(GetSceneDepth(uv)); } 
            #else
               float GetSceneDepth(float2 uv) { return SHADERGRAPH_SAMPLE_SCENE_DEPTH(uv); }
               float GetLinear01Depth(float2 uv) { return Linear01Depth(GetSceneDepth(uv), _ZBufferParams); }
               float GetLinearEyeDepth(float2 uv) { return LinearEyeDepth(GetSceneDepth(uv), _ZBufferParams); } 
            #endif

            float3 GetWorldPositionFromDepthBuffer(float2 uv, float3 worldSpaceViewDir)
            {
               float eye = GetLinearEyeDepth(uv);
               float3 camView = mul((float3x3)GetObjectToWorldMatrix(), transpose(mul(GetWorldToObjectMatrix(), UNITY_MATRIX_I_V)) [2].xyz);

               float dt = dot(worldSpaceViewDir, camView);
               float3 div = worldSpaceViewDir/dt;
               float3 wpos = (eye * div) + GetCameraWorldPosition();
               return wpos;
            }

            #if _HDRP
            float3 ObjectToWorldSpacePosition(float3 pos)
            {
               return GetAbsolutePositionWS(TransformObjectToWorld(pos));
            }
            #else
            float3 ObjectToWorldSpacePosition(float3 pos)
            {
               return TransformObjectToWorld(pos);
            }
            #endif

            #if _STANDARD
               UNITY_DECLARE_SCREENSPACE_TEXTURE(_CameraDepthNormalsTexture);
               float3 GetSceneNormal(float2 uv, float3 worldSpaceViewDir)
               {
                  float4 depthNorms = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_CameraDepthNormalsTexture, uv);
                  float3 norms = DecodeViewNormalStereo(depthNorms);
                  norms = mul((float3x3)GetWorldToViewMatrix(), norms) * 0.5 + 0.5;
                  return norms;
               }
            #elif _HDRP
               float3 GetSceneNormal(float2 uv, float3 worldSpaceViewDir)
               {
                  NormalData nd;
                  DecodeFromNormalBuffer(_ScreenSize.xy * uv, nd);
                  return nd.normalWS;
               }
            #elif _URP
               #if (SHADER_LIBRARY_VERSION_MAJOR >= 10)
                  #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareNormalsTexture.hlsl"
               #endif

               float3 GetSceneNormal(float2 uv, float3 worldSpaceViewDir)
               {
                  #if (SHADER_LIBRARY_VERSION_MAJOR >= 10)
                     return SampleSceneNormals(uv);
                  #else
                     float3 wpos = GetWorldPositionFromDepthBuffer(uv, worldSpaceViewDir);
                     return normalize(-cross(ddx(wpos), ddy(wpos))) * 0.5 + 0.5;
                  #endif

                }
             #endif

             #if _HDRP

               half3 UnpackNormalmapRGorAG(half4 packednormal)
               {
                     // This do the trick
                  packednormal.x *= packednormal.w;

                  half3 normal;
                  normal.xy = packednormal.xy * 2 - 1;
                  normal.z = sqrt(1 - saturate(dot(normal.xy, normal.xy)));
                  return normal;
               }
               half3 UnpackNormal(half4 packednormal)
               {
                  #if defined(UNITY_NO_DXT5nm)
                     return packednormal.xyz * 2 - 1;
                  #else
                     return UnpackNormalmapRGorAG(packednormal);
                  #endif
               }
               #endif
               #if _HDRP || _URP

               half3 UnpackScaleNormal(half4 packednormal, half scale)
               {
                 #ifndef UNITY_NO_DXT5nm
                   // Unpack normal as DXT5nm (1, y, 1, x) or BC5 (x, y, 0, 1)
                   // Note neutral texture like "bump" is (0, 0, 1, 1) to work with both plain RGB normal and DXT5nm/BC5
                   packednormal.x *= packednormal.w;
                 #endif
                   half3 normal;
                   normal.xy = (packednormal.xy * 2 - 1) * scale;
                   normal.z = sqrt(1 - saturate(dot(normal.xy, normal.xy)));
                   return normal;
               }	

             #endif


            void GetSun(out float3 lightDir, out float3 color)
            {
               lightDir = float3(0.5, 0.5, 0);
               color = 1;
               #if _HDRP
                  if (_DirectionalLightCount > 0)
                  {
                     DirectionalLightData light = _DirectionalLightDatas[0];
                     lightDir = -light.forward.xyz;
                     color = light.color;
                  }
               #elif _STANDARD
			         lightDir = normalize(_WorldSpaceLightPos0.xyz);
                  color = _LightColor0.rgb;
               #elif _URP
	               Light light = GetMainLight();
	               lightDir = light.direction;
	               color = light.color;
               #endif
            }




            CBUFFER_START(UnityPerMaterial)

               float _StencilRef;
               float _StencilWriteMask;
               float _StencilRefDepth;
               float _StencilWriteMaskDepth;
               float _StencilRefMV;
               float _StencilWriteMaskMV;
               float _StencilRefDistortionVec;
               float _StencilWriteMaskDistortionVec;
               float _StencilWriteMaskGBuffer;
               float _StencilRefGBuffer;
               float _ZTestGBuffer;
               float _RequireSplitLighting;
               float _ReceivesSSR;
               float _ZWrite;
               float _TransparentSortPriority;
               float _ZTestDepthEqualForOpaque;
               float _ZTestTransparent;
               float _TransparentBackfaceEnable;
               float _AlphaCutoffEnable;
               float _UseShadowThreshold;

               
    float4 _BaseColorMap_ST;
    float4 _DetailMap_ST;
    float4 _EmissiveColorMap_ST;

    float _UVBase;
    half4 _Color;
    half4 _BaseColor; // TODO CHECK
    half _Cutoff; 
    half _Mode;
    //float _Cull;
    float _CullMode;
    float _CullModeForward;
    half _Metallic;
    half3 _EmissionColor;
    half _UVSec;

    float3 _EmissiveColor;
    float _AlbedoAffectEmissive;
    float _EmissiveExposureWeight;
    float _MetallicRemapMin;
    float _MetallicRemapMax;
    float _Smoothness;
    float _SmoothnessRemapMin;
    float _SmoothnessRemapMax;
    float _AlphaRemapMin;
    float _AlphaRemapMax;
    float _AORemapMin;
    float _AORemapMax;
    float _NormalScale;
    float _DetailAlbedoScale;
    float _DetailNormalScale;
    float _DetailSmoothnessScale;
    float _Anisotropy;
    float _DiffusionProfileHash;
    float _SubsurfaceMask;
    float _Thickness;
    float4 _ThicknessRemap;
    float _IridescenceThickness;
    float4 _IridescenceThicknessRemap;
    float _IridescenceMask;
    float _CoatMask;
    float4 _SpecularColor;
    float _EnergyConservingSpecularColor;
    int _MaterialID;

    float _LinkDetailsWithBase;
    float4 _UVMappingMask;
    float4 _UVDetailsMappingMask;

    float4 _UVMappingMaskEmissive;

    float _AlphaCutoff;
    //float _AlphaCutoffEnable;


    float _SyncCullMode;

    float _IsReplacementShader;
    float _TriggerMode;
    float _RaycastMode;
    float _IsExempt;
    float _isReferenceMaterial;
    float _InteractionMode;
    int _ArrayLength = 0;
    #if (defined(SHADER_API_GLES) || defined(SHADER_API_GLES3)) 
        float4 _PlayersPosVectorArray[20];
        float _PlayersDataFloatArray[150];     
    #else
        float4 _PlayersPosVectorArray[100];
        float _PlayersDataFloatArray[500];  
    #endif
    float _tDirection = 0;
    float _numOfPlayersInside = 0;
    float _tValue = 0;
    float _id = 0;
    #if _ZONING
        #if (defined(SHADER_API_GLES) || defined(SHADER_API_GLES3)) 
            float _ZDFA[500];
        #else
            float _ZDFA[1000];
        #endif
        float _ZonesDataCount;
    #endif
    #if _REPLACEMENT        
        half4 _DissolveColorGlobal;
        float _DissolveColorSaturationGlobal;
        float _DissolveEmissionGlobal;
        float _DissolveEmissionBoosterGlobal;
        float _TextureVisibilityGlobal;
        float _ObstructionGlobal;
        float _AngleStrengthGlobal;
        float _ConeStrengthGlobal;
        float _ConeObstructionDestroyRadiusGlobal;
        float _CylinderStrengthGlobal;
        float _CylinderObstructionDestroyRadiusGlobal;
        float _CircleStrengthGlobal;
        float _CircleObstructionDestroyRadiusGlobal;
        float _CurveStrengthGlobal;
        float _CurveObstructionDestroyRadiusGlobal;
        float _DissolveFallOffGlobal;
        float _IntrinsicDissolveStrengthGlobal;
        float _PreviewModeGlobal;
        float _UVsGlobal;
        float _hasClippedShadowsGlobal;
        float _FloorGlobal;
        float _FloorModeGlobal;
        float _FloorYGlobal;
        float _PlayerPosYOffsetGlobal;
        float _FloorYTextureGradientLengthGlobal;
        float _AnimationEnabledGlobal;
        float _AnimationSpeedGlobal;
        float _DefaultEffectRadiusGlobal;
        float _TransitionDurationGlobal;        
        float _TexturedEmissionEdgeGlobal;
        float _TexturedEmissionEdgeStrengthGlobal;
        float _IsometricExclusionGlobal;
        float _IsometricExclusionDistanceGlobal;
        float _IsometricExclusionGradientLengthGlobal;
        float _CeilingGlobal;
        float _CeilingModeGlobal;
        float _CeilingBlendModeGlobal;
        float _CeilingYGlobal;
        float _CeilingPlayerYOffsetGlobal;
        float _CeilingYGradientLengthGlobal;
        float _ZoningGlobal;
        float _ZoningModeGlobal;
        float _ZoningEdgeGradientLengthGlobal;
        float _IsZoningRevealableGlobal;
        float _SyncZonesWithFloorYGlobal;
        float _SyncZonesFloorYOffsetGlobal;
        float4 _ObstructionCurveGlobal_TexelSize;
        float4 _DissolveMaskGlobal_TexelSize;
        float _DissolveMaskEnabledGlobal;
        float _PreviewIndicatorLineThicknessGlobal;
    #else
        half _TextureVisibility;
        half _AngleStrength;
        float _Obstruction;
        float _UVs;
        float4 _ObstructionCurve_TexelSize;      
        float _DissolveMaskEnabled;
        float4 _DissolveMask_TexelSize;
        half4 _DissolveColor;
        float _DissolveColorSaturation;
        float _DissolveEmission;
        float _DissolveEmissionBooster;
        float _hasClippedShadows;
        float _ConeStrength;
        float _ConeObstructionDestroyRadius;
        float _CylinderStrength;
        float _CylinderObstructionDestroyRadius;
        float _CircleStrength;
        float _CircleObstructionDestroyRadius;
        float _CurveStrength;
        float _CurveObstructionDestroyRadius;
        float _IntrinsicDissolveStrength;
        float _DissolveFallOff;
        float _PreviewMode;
        float _PreviewIndicatorLineThickness;
        float _AnimationEnabled;
        float _AnimationSpeed;
        float _DefaultEffectRadius;
        float _TransitionDuration;
        float _TexturedEmissionEdge;
        float _TexturedEmissionEdgeStrength;
        float _IsometricExclusion;
        float _IsometricExclusionDistance;
        float _IsometricExclusionGradientLength;
        float _Floor;
        float _FloorMode;
        float _FloorY;
        float _FloorYTextureGradientLength;
        float _PlayerPosYOffset;
        float _Ceiling;
        float _CeilingMode;
        float _CeilingBlendMode;
        float _CeilingY;
        float _CeilingPlayerYOffset;
        float _CeilingYGradientLength;
        float _Zoning;
        float _ZoningMode;
        float _ZoningEdgeGradientLength;
        float _IsZoningRevealable;
        float _SyncZonesWithFloorY;
        float _SyncZonesFloorYOffset;
    #endif



            CBUFFER_END

            

            

            #ifdef unity_WorldToObject
#undef unity_WorldToObject
#endif
#ifdef unity_ObjectToWorld
#undef unity_ObjectToWorld
#endif
#define unity_ObjectToWorld GetObjectToWorldMatrix()
#define unity_WorldToObject GetWorldToObjectMatrix()

    sampler2D _EmissiveColorMap;
    sampler2D _BaseColorMap;
    sampler2D _MaskMap;
    sampler2D _NormalMap;
    sampler2D _NormalMapOS;
    sampler2D _DetailMap;
    sampler2D _TangentMap;
    sampler2D _TangentMapOS;
    sampler2D _AnisotropyMap;
    sampler2D _SubsurfaceMaskMap;
    sampler2D _TransmissionMaskMap;
    sampler2D _ThicknessMap;
    sampler2D _IridescenceThicknessMap;
    sampler2D _IridescenceMaskMap;
    sampler2D _SpecularColorMap;

    sampler2D _CoatMaskMap;

    float3 DecodeNormal (float4 sample, float scale) {
        #if defined(UNITY_NO_DXT5nm)
            return UnpackNormalRGB(sample, scale);
        #else
            return UnpackNormalmapRGorAG(sample, scale);
        #endif
    }

	void Ext_SurfaceFunction0 (inout Surface o, ShaderData d)
	{


//SEE: https://github.com/Unity-Technologies/Graphics/blob/6fdc7996098aa184495c0a3c0da10135bfe18340/Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitDataIndividualLayer.hlsl

        float2 uvBase = _UVMappingMask.x * d.texcoord0 +
                        _UVMappingMask.y * d.texcoord1 +
                        _UVMappingMask.z * d.texcoord2 +
                        _UVMappingMask.w * d.texcoord3;

        float2 uvDetails =  _UVDetailsMappingMask.x * d.texcoord0 +
                            _UVDetailsMappingMask.y * d.texcoord1 +
                            _UVDetailsMappingMask.z * d.texcoord2 +
                            _UVDetailsMappingMask.w * d.texcoord3;


        //TODO: add planar/triplanar

        float4 texcoords;
        //texcoords.xy = d.texcoord0.xy * _BaseColorMap_ST.xy + _BaseColorMap_ST.zw; 
        texcoords.xy = uvBase * _BaseColorMap_ST.xy + _BaseColorMap_ST.zw; 

        texcoords.zw = uvDetails * _DetailMap_ST.xy + _DetailMap_ST.zw;

        if (_LinkDetailsWithBase > 0.0)
        {
            texcoords.zw = texcoords.zw  * _BaseColorMap_ST.xy + _BaseColorMap_ST.zw;

        }

            float3 detailNormalTS = float3(0.0, 0.0, 0.0);
            float detailMask = 0.0;
            #ifdef _DETAIL_MAP
                detailMask = 1.0;
                #ifdef _MASKMAP
                    detailMask = tex2D(_MaskMap, texcoords.xy).b;
                #endif
                float2 detailAlbedoAndSmoothness = tex2D(_DetailMap, texcoords.zw).rb;
                float detailAlbedo = detailAlbedoAndSmoothness.r * 2.0 - 1.0;
                float detailSmoothness = detailAlbedoAndSmoothness.g * 2.0 - 1.0;
                real4 packedNormal = tex2D(_DetailMap, texcoords.zw).rgba;
                detailNormalTS = UnpackNormalAG(packedNormal, _DetailNormalScale);
            #endif
            float4 color = tex2D(_BaseColorMap, texcoords.xy).rgba  * _Color.rgba;
            o.Albedo = color; 
            float alpha = color.a;
            alpha = lerp(_AlphaRemapMin, _AlphaRemapMax, alpha);
            o.Alpha = alpha;
            #ifdef _DETAIL_MAP
                float albedoDetailSpeed = saturate(abs(detailAlbedo) * _DetailAlbedoScale);
                float3 baseColorOverlay = lerp(sqrt(o.Albedo), (detailAlbedo < 0.0) ? float3(0.0, 0.0, 0.0) : float3(1.0, 1.0, 1.0), albedoDetailSpeed * albedoDetailSpeed);
                baseColorOverlay *= baseColorOverlay;
                o.Albedo = lerp(o.Albedo, saturate(baseColorOverlay), detailMask);
            #endif

            #ifdef _NORMALMAP
                float3 normalTS;

                //normalTS = UnpackScaleNormal(tex2D(_NormalMap, texcoords.xy), _NormalScale);
                //#ifdef _DETAIL_MAP
                //    normalTS = lerp(normalTS, BlendNormalRNM(normalTS, normalize(detailNormalTS)), detailMask); 
                //#endif
                //o.Normal = normalTS;


                #ifdef _NORMALMAP_TANGENT_SPACE
                    //normalTS = UnpackScaleNormal(tex2D(_NormalMap, texcoords.xy), _NormalScale);
                    normalTS = DecodeNormal(tex2D(_NormalMap, texcoords.xy), _NormalScale);
                #else
                    #ifdef SURFACE_GRADIENT
                        float3 normalOS = tex2D(_NormalMapOS, texcoords.xy).xyz * 2.0 - 1.0;
                        normalTS = SurfaceGradientFromPerturbedNormal(d.worldSpaceNormal, TransformObjectToWorldNormal(normalOS));
                    #else
                        float3 normalOS = UnpackNormalRGB(tex2D(_NormalMapOS, texcoords.xy), 1.0);
                        float3 bitangent = d.tangentSign * cross(d.worldSpaceNormal.xyz, d.worldSpaceTangent.xyz);
                        half3x3 tangentToWorld = half3x3(d.worldSpaceTangent.xyz, bitangent.xyz, d.worldSpaceNormal.xyz);
                        normalTS = TransformObjectToTangent(normalOS, tangentToWorld);
                    #endif
                #endif

                #ifdef _DETAIL_MAP
                    #ifdef SURFACE_GRADIENT
                    normalTS += detailNormalTS * detailMask;
                    #else
                    normalTS = lerp(normalTS, BlendNormalRNM(normalTS, normalize(detailNormalTS)), detailMask); // todo: detailMask should lerp the angle of the quaternion rotation, not the normals
                    #endif
                #endif
                o.Normal = normalTS;

            #endif

            #if defined(_MASKMAP)
                o.Smoothness = tex2D(_MaskMap, texcoords.xy).a;
                o.Smoothness = lerp(_SmoothnessRemapMin, _SmoothnessRemapMax, o.Smoothness);
            #else
                o.Smoothness = _Smoothness;
            #endif
            #ifdef _DETAIL_MAP
                float smoothnessDetailSpeed = saturate(abs(detailSmoothness) * _DetailSmoothnessScale);
                float smoothnessOverlay = lerp(o.Smoothness, (detailSmoothness < 0.0) ? 0.0 : 1.0, smoothnessDetailSpeed);
                o.Smoothness = lerp(o.Smoothness, saturate(smoothnessOverlay), detailMask);
            #endif
            #ifdef _MASKMAP
                o.Metallic = tex2D(_MaskMap, texcoords.xy).r;
                o.Metallic = lerp(_MetallicRemapMin, _MetallicRemapMax, o.Metallic);
                o.Occlusion = tex2D(_MaskMap, texcoords.xy).g;
                o.Occlusion = lerp(_AORemapMin, _AORemapMax, o.Occlusion);
            #else
                o.Metallic = _Metallic;
                o.Occlusion = 1.0;
            #endif

            #if defined(_SPECULAR_OCCLUSION_FROM_BENT_NORMAL_MAP)
                // TODO: implenent bent normals for this to work
                //o.SpecularOcclusion = GetSpecularOcclusionFromBentAO(V, bentNormalWS, d.worldSpaceNormal, o.Occlusion, PerceptualSmoothnessToRoughness(o.Smoothness));
            #elif  defined(_MASKMAP) && !defined(_SPECULAR_OCCLUSION_NONE)
                float3 V = normalize(d.worldSpaceViewDir);
                o.SpecularOcclusion = GetSpecularOcclusionFromAmbientOcclusion(ClampNdotV(dot(d.worldSpaceNormal, V)), o.Occlusion, PerceptualSmoothnessToRoughness(o.Smoothness));
            #endif

            o.DiffusionProfileHash = asuint(_DiffusionProfileHash);
            o.SubsurfaceMask = _SubsurfaceMask;
            #ifdef _SUBSURFACE_MASK_MAP
                o.SubsurfaceMask *= tex2D(_SubsurfaceMaskMap, texcoords.xy).r;
            #endif
            #ifdef _THICKNESSMAP
                o.Thickness = tex2D(_ThicknessMap, texcoords.xy).r;
                o.Thickness = _ThicknessRemap.x + _ThicknessRemap.y * surfaceData.thickness;
            #else
                o.Thickness = _Thickness;
            #endif
            #ifdef _ANISOTROPYMAP
                o.Anisotropy = tex2D(_AnisotropyMap, texcoords.xy).r;
            #else
                o.Anisotropy = 1.0;
            #endif
                o.Anisotropy *= _Anisotropy;
                o.Specular = _SpecularColor.rgb;
            #ifdef _SPECULARCOLORMAP
                o.Specular *= tex2D(_SpecularColorMap, texcoords.xy).rgb;
            #endif
            #ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
                o.Albedo *= _EnergyConservingSpecularColor > 0.0 ? (1.0 - Max3(o.Specular.r, o.Specular.g, o.Specular.b)) : 1.0;
            #endif
            #ifdef _MATERIAL_FEATURE_CLEAR_COAT
                o.CoatMask = _CoatMask;
                o.CoatMask *= tex2D(_CoatMaskMap, texcoords.xy).r;
            #else
                o.CoatMask = 0.0;
            #endif
            #ifdef _MATERIAL_FEATURE_IRIDESCENCE
                #ifdef _IRIDESCENCE_THICKNESSMAP
                o.IridescenceThickness = tex2D(_IridescenceThicknessMap, texcoords.xy).r;
                o.IridescenceThickness = _IridescenceThicknessRemap.x + _IridescenceThicknessRemap.y * o.IridescenceThickness;
                #else
                o.IridescenceThickness = _IridescenceThickness;
                #endif
                o.IridescenceMask = _IridescenceMask;
                o.IridescenceMask *= tex2D(_IridescenceMaskMap, texcoords.xy).r;
            #else
                o.IridescenceThickness = 0.0;
                o.IridescenceMask = 0.0;
            #endif


//SEE: https://github.com/Unity-Technologies/Graphics/blob/632f80e011f18ea537ee6e2f0be3ff4f4dea6a11/Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitBuiltinData.hlsl

            float3 emissiveColor = _EmissiveColor * lerp(float3(1.0, 1.0, 1.0), o.Albedo.rgb, _AlbedoAffectEmissive);
            #ifdef _EMISSIVE_COLOR_MAP                
                float2 uvEmission = _UVMappingMaskEmissive.x * d.texcoord0 +
                                    _UVMappingMaskEmissive.y * d.texcoord1 +
                                    _UVMappingMaskEmissive.z * d.texcoord2 +
                                    _UVMappingMaskEmissive.w * d.texcoord3;

                uvEmission = uvEmission * _EmissiveColorMap_ST.xy + _EmissiveColorMap_ST.zw;                     

                emissiveColor *= tex2D(_EmissiveColorMap, uvEmission).rgb;
            #endif
            o.Emission += emissiveColor;
            float currentExposureMultiplier = 0;
            #if SHADEROPTIONS_PRE_EXPOSITION
                currentExposureMultiplier =  LOAD_TEXTURE2D(_ExposureTexture, int2(0, 0)).x * _ProbeExposureScale;
            #else
                currentExposureMultiplier =  _ProbeExposureScale;
            #endif
            float InverseCurrentExposureMultiplier = 0;
            float exposure = currentExposureMultiplier;
            InverseCurrentExposureMultiplier =  rcp(exposure + (exposure == 0.0)); 
            float3 emissiveRcpExposure = o.Emission * InverseCurrentExposureMultiplier;
            o.Emission = lerp(emissiveRcpExposure, o.Emission, _EmissiveExposureWeight);


// SEE: https://github.com/Unity-Technologies/Graphics/blob/223d8105e68c5a808027d78f39cb4e2545fd6276/Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitData.hlsl
            #if defined(_ALPHATEST_ON)
                float alphaTex = tex2D(_BaseColorMap, texcoords.xy).a;
                alphaTex = lerp(_AlphaRemapMin, _AlphaRemapMax, alphaTex);
                float alphaValue = alphaTex * _BaseColor.a;

                // Perform alha test very early to save performance (a killed pixel will not sample textures)
                //#if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
                //float alphaCutoff = _AlphaCutoffPrepass;
                //#elif SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_POSTPASS
                //float alphaCutoff = _AlphaCutoffPostpass;
                //#elif (SHADERPASS == SHADERPASS_SHADOWS) || (SHADERPASS == SHADERPASS_RAYTRACING_VISIBILITY)
                //float alphaCutoff = _UseShadowThreshold ? _AlphaCutoffShadow : _AlphaCutoff;
                //#else
                float alphaCutoff = _AlphaCutoff;
                //#endif

                // GENERIC_ALPHA_TEST(alphaValue, alphaCutoff);
                clip(alpha - alphaCutoff);
            #endif

	}




    #if _REPLACEMENT
        sampler2D _DissolveTexGlobal;
    #else
        sampler2D _DissolveTex;
    #endif

    //#if _DISSOLVEMASK
    #if _REPLACEMENT
        sampler2D _DissolveMaskGlobal;
    #else
        sampler2D _DissolveMask;
    #endif
    //#endif

    #if _REPLACEMENT
        sampler2D _ObstructionCurveGlobal;
    #else
        sampler2D _ObstructionCurve;
    #endif



    #if _URP
        half2 ParallaxOffset1Step (half h, half height, half3 viewDir)
        {
            h = h * height - height/2.0;
            half3 v = normalize(viewDir);
            v.z += 0.42;
            return h * (v.xy / v.z);
        }
        #ifdef UNITY_COLORSPACE_GAMMA
            #define unity_ColorSpaceDouble half4(2.0, 2.0, 2.0, 2.0)
        #else 
            #define unity_ColorSpaceDouble half4(4.59479380, 4.59479380, 4.59479380, 2.0)
        #endif
        half3 BlendNormals(half3 n1, half3 n2)
        {
            return normalize(half3(n1.xy + n2.xy, n1.z*n2.z));
        }
    #endif


#ifdef USE_UNITY_TEXTURE_2D_TYPE
#undef USE_UNITY_TEXTURE_2D_TYPE
#endif
#include "Assets/See-through Shader/Core/Shaders/xxSharedSTSDependencies/SeeThroughShaderFunction.hlsl"



	void Ext_SurfaceFunction1 (inout Surface o, ShaderData d)
	{
        half3 albedo;
        half3 emission;
        float alphaForClipping;
#ifdef USE_UNITY_TEXTURE_2D_TYPE
#undef USE_UNITY_TEXTURE_2D_TYPE
#endif
#if _REPLACEMENT
    DoSeeThroughShading( o.Albedo, o.Normal, d.worldSpacePosition, d.worldSpaceNormal,

                        _numOfPlayersInside, _tDirection, _tValue, _id,
                        _TriggerMode, _RaycastMode,
                        _IsExempt,

                        _DissolveColorGlobal, _DissolveColorSaturationGlobal, _UVsGlobal,
                        _DissolveEmissionGlobal, _DissolveEmissionBoosterGlobal, _TexturedEmissionEdgeGlobal, _TexturedEmissionEdgeStrengthGlobal,
                        _hasClippedShadowsGlobal,

                        _ObstructionGlobal,
                        _AngleStrengthGlobal,
                        _ConeStrengthGlobal, _ConeObstructionDestroyRadiusGlobal,
                        _CylinderStrengthGlobal, _CylinderObstructionDestroyRadiusGlobal,
                        _CircleStrengthGlobal, _CircleObstructionDestroyRadiusGlobal,
                        _CurveStrengthGlobal, _CurveObstructionDestroyRadiusGlobal,
                        _DissolveFallOffGlobal,
                        _DissolveMaskEnabledGlobal,
                        _IntrinsicDissolveStrengthGlobal,
                        _DefaultEffectRadiusGlobal,

                        _IsometricExclusionGlobal, _IsometricExclusionDistanceGlobal, _IsometricExclusionGradientLengthGlobal,
                        _CeilingGlobal, _CeilingModeGlobal, _CeilingBlendModeGlobal, _CeilingYGlobal, _CeilingPlayerYOffsetGlobal, _CeilingYGradientLengthGlobal,
                        _FloorGlobal, _FloorModeGlobal, _FloorYGlobal, _PlayerPosYOffsetGlobal, _FloorYTextureGradientLengthGlobal,

                        _AnimationEnabledGlobal, _AnimationSpeedGlobal,
                        _TransitionDurationGlobal,

                        _ZoningGlobal, _ZoningModeGlobal, _IsZoningRevealableGlobal, _ZoningEdgeGradientLengthGlobal,
                        _SyncZonesWithFloorYGlobal, _SyncZonesFloorYOffsetGlobal,

                        _PreviewModeGlobal,
                        _PreviewIndicatorLineThicknessGlobal,

                        _DissolveTexGlobal,
                        _DissolveMaskGlobal,
                        _ObstructionCurveGlobal,

                        _DissolveMaskGlobal_TexelSize,
                        _ObstructionCurveGlobal_TexelSize,

                        albedo, emission, alphaForClipping);
#else 
    
    DoSeeThroughShading( o.Albedo, o.Normal, d.worldSpacePosition, d.worldSpaceNormal,

                        _numOfPlayersInside, _tDirection, _tValue, _id,
                        _TriggerMode, _RaycastMode,
                        _IsExempt,

                        _DissolveColor, _DissolveColorSaturation, _UVs,
                        _DissolveEmission, _DissolveEmissionBooster, _TexturedEmissionEdge, _TexturedEmissionEdgeStrength,
                        _hasClippedShadows,

                        _Obstruction,
                        _AngleStrength,
                        _ConeStrength, _ConeObstructionDestroyRadius,
                        _CylinderStrength, _CylinderObstructionDestroyRadius,
                        _CircleStrength, _CircleObstructionDestroyRadius,
                        _CurveStrength, _CurveObstructionDestroyRadius,
                        _DissolveFallOff,
                        _DissolveMaskEnabled,
                        _IntrinsicDissolveStrength,
                        _DefaultEffectRadius,

                        _IsometricExclusion, _IsometricExclusionDistance, _IsometricExclusionGradientLength,
                        _Ceiling, _CeilingMode, _CeilingBlendMode, _CeilingY, _CeilingPlayerYOffset, _CeilingYGradientLength,
                        _Floor, _FloorMode, _FloorY, _PlayerPosYOffset, _FloorYTextureGradientLength,

                        _AnimationEnabled, _AnimationSpeed,
                        _TransitionDuration,

                        _Zoning, _ZoningMode, _IsZoningRevealable, _ZoningEdgeGradientLength,
                        _SyncZonesWithFloorY, _SyncZonesFloorYOffset,

                        _PreviewMode,
                        _PreviewIndicatorLineThickness,                            

                        _DissolveTex,
                        _DissolveMask,
                        _ObstructionCurve,

                        _DissolveMask_TexelSize,
                        _ObstructionCurve_TexelSize,

                        albedo, emission, alphaForClipping);
#endif 



        o.Albedo = albedo;
        o.Emission += emission;   

	}




        
            void ChainSurfaceFunction(inout Surface l, inout ShaderData d)
            {
                  Ext_SurfaceFunction0(l, d);
                  Ext_SurfaceFunction1(l, d);
                 // Ext_SurfaceFunction2(l, d);
                 // Ext_SurfaceFunction3(l, d);
                 // Ext_SurfaceFunction4(l, d);
                 // Ext_SurfaceFunction5(l, d);
                 // Ext_SurfaceFunction6(l, d);
                 // Ext_SurfaceFunction7(l, d);
                 // Ext_SurfaceFunction8(l, d);
                 // Ext_SurfaceFunction9(l, d);
		           // Ext_SurfaceFunction10(l, d);
                 // Ext_SurfaceFunction11(l, d);
                 // Ext_SurfaceFunction12(l, d);
                 // Ext_SurfaceFunction13(l, d);
                 // Ext_SurfaceFunction14(l, d);
                 // Ext_SurfaceFunction15(l, d);
                 // Ext_SurfaceFunction16(l, d);
                 // Ext_SurfaceFunction17(l, d);
                 // Ext_SurfaceFunction18(l, d);
		           // Ext_SurfaceFunction19(l, d);
                 // Ext_SurfaceFunction20(l, d);
                 // Ext_SurfaceFunction21(l, d);
                 // Ext_SurfaceFunction22(l, d);
                 // Ext_SurfaceFunction23(l, d);
                 // Ext_SurfaceFunction24(l, d);
                 // Ext_SurfaceFunction25(l, d);
                 // Ext_SurfaceFunction26(l, d);
                 // Ext_SurfaceFunction27(l, d);
                 // Ext_SurfaceFunction28(l, d);
		           // Ext_SurfaceFunction29(l, d);
            }

            void ChainModifyVertex(inout VertexData v, inout VertexToPixel v2p, float4 time)
            {
                 ExtraV2F d;
                 
                 ZERO_INITIALIZE(ExtraV2F, d);
                 ZERO_INITIALIZE(Blackboard, d.blackboard);
                 // due to motion vectors in HDRP, we need to use the last
                 // time in certain spots. So if you are going to use _Time to adjust vertices,
                 // you need to use this time or motion vectors will break. 
                 d.time = time;

                 //  Ext_ModifyVertex0(v, d);
                 // Ext_ModifyVertex1(v, d);
                 // Ext_ModifyVertex2(v, d);
                 // Ext_ModifyVertex3(v, d);
                 // Ext_ModifyVertex4(v, d);
                 // Ext_ModifyVertex5(v, d);
                 // Ext_ModifyVertex6(v, d);
                 // Ext_ModifyVertex7(v, d);
                 // Ext_ModifyVertex8(v, d);
                 // Ext_ModifyVertex9(v, d);
                 // Ext_ModifyVertex10(v, d);
                 // Ext_ModifyVertex11(v, d);
                 // Ext_ModifyVertex12(v, d);
                 // Ext_ModifyVertex13(v, d);
                 // Ext_ModifyVertex14(v, d);
                 // Ext_ModifyVertex15(v, d);
                 // Ext_ModifyVertex16(v, d);
                 // Ext_ModifyVertex17(v, d);
                 // Ext_ModifyVertex18(v, d);
                 // Ext_ModifyVertex19(v, d);
                 // Ext_ModifyVertex20(v, d);
                 // Ext_ModifyVertex21(v, d);
                 // Ext_ModifyVertex22(v, d);
                 // Ext_ModifyVertex23(v, d);
                 // Ext_ModifyVertex24(v, d);
                 // Ext_ModifyVertex25(v, d);
                 // Ext_ModifyVertex26(v, d);
                 // Ext_ModifyVertex27(v, d);
                 // Ext_ModifyVertex28(v, d);
                 // Ext_ModifyVertex29(v, d);


                 // #if %EXTRAV2F0REQUIREKEY%
                 // v2p.extraV2F0 = d.extraV2F0;
                 // #endif

                 // #if %EXTRAV2F1REQUIREKEY%
                 // v2p.extraV2F1 = d.extraV2F1;
                 // #endif

                 // #if %EXTRAV2F2REQUIREKEY%
                 // v2p.extraV2F2 = d.extraV2F2;
                 // #endif

                 // #if %EXTRAV2F3REQUIREKEY%
                 // v2p.extraV2F3 = d.extraV2F3;
                 // #endif

                 // #if %EXTRAV2F4REQUIREKEY%
                 // v2p.extraV2F4 = d.extraV2F4;
                 // #endif

                 // #if %EXTRAV2F5REQUIREKEY%
                 // v2p.extraV2F5 = d.extraV2F5;
                 // #endif

                 // #if %EXTRAV2F6REQUIREKEY%
                 // v2p.extraV2F6 = d.extraV2F6;
                 // #endif

                 // #if %EXTRAV2F7REQUIREKEY%
                 // v2p.extraV2F7 = d.extraV2F7;
                 // #endif
            }

            void ChainModifyTessellatedVertex(inout VertexData v, inout VertexToPixel v2p)
            {
               ExtraV2F d;
               ZERO_INITIALIZE(ExtraV2F, d);
               ZERO_INITIALIZE(Blackboard, d.blackboard);

               // #if %EXTRAV2F0REQUIREKEY%
               // d.extraV2F0 = v2p.extraV2F0;
               // #endif

               // #if %EXTRAV2F1REQUIREKEY%
               // d.extraV2F1 = v2p.extraV2F1;
               // #endif

               // #if %EXTRAV2F2REQUIREKEY%
               // d.extraV2F2 = v2p.extraV2F2;
               // #endif

               // #if %EXTRAV2F3REQUIREKEY%
               // d.extraV2F3 = v2p.extraV2F3;
               // #endif

               // #if %EXTRAV2F4REQUIREKEY%
               // d.extraV2F4 = v2p.extraV2F4;
               // #endif

               // #if %EXTRAV2F5REQUIREKEY%
               // d.extraV2F5 = v2p.extraV2F5;
               // #endif

               // #if %EXTRAV2F6REQUIREKEY%
               // d.extraV2F6 = v2p.extraV2F6;
               // #endif

               // #if %EXTRAV2F7REQUIREKEY%
               // d.extraV2F7 = v2p.extraV2F7;
               // #endif


               // Ext_ModifyTessellatedVertex0(v, d);
               // Ext_ModifyTessellatedVertex1(v, d);
               // Ext_ModifyTessellatedVertex2(v, d);
               // Ext_ModifyTessellatedVertex3(v, d);
               // Ext_ModifyTessellatedVertex4(v, d);
               // Ext_ModifyTessellatedVertex5(v, d);
               // Ext_ModifyTessellatedVertex6(v, d);
               // Ext_ModifyTessellatedVertex7(v, d);
               // Ext_ModifyTessellatedVertex8(v, d);
               // Ext_ModifyTessellatedVertex9(v, d);
               // Ext_ModifyTessellatedVertex10(v, d);
               // Ext_ModifyTessellatedVertex11(v, d);
               // Ext_ModifyTessellatedVertex12(v, d);
               // Ext_ModifyTessellatedVertex13(v, d);
               // Ext_ModifyTessellatedVertex14(v, d);
               // Ext_ModifyTessellatedVertex15(v, d);
               // Ext_ModifyTessellatedVertex16(v, d);
               // Ext_ModifyTessellatedVertex17(v, d);
               // Ext_ModifyTessellatedVertex18(v, d);
               // Ext_ModifyTessellatedVertex19(v, d);
               // Ext_ModifyTessellatedVertex20(v, d);
               // Ext_ModifyTessellatedVertex21(v, d);
               // Ext_ModifyTessellatedVertex22(v, d);
               // Ext_ModifyTessellatedVertex23(v, d);
               // Ext_ModifyTessellatedVertex24(v, d);
               // Ext_ModifyTessellatedVertex25(v, d);
               // Ext_ModifyTessellatedVertex26(v, d);
               // Ext_ModifyTessellatedVertex27(v, d);
               // Ext_ModifyTessellatedVertex28(v, d);
               // Ext_ModifyTessellatedVertex29(v, d);

               // #if %EXTRAV2F0REQUIREKEY%
               // v2p.extraV2F0 = d.extraV2F0;
               // #endif

               // #if %EXTRAV2F1REQUIREKEY%
               // v2p.extraV2F1 = d.extraV2F1;
               // #endif

               // #if %EXTRAV2F2REQUIREKEY%
               // v2p.extraV2F2 = d.extraV2F2;
               // #endif

               // #if %EXTRAV2F3REQUIREKEY%
               // v2p.extraV2F3 = d.extraV2F3;
               // #endif

               // #if %EXTRAV2F4REQUIREKEY%
               // v2p.extraV2F4 = d.extraV2F4;
               // #endif

               // #if %EXTRAV2F5REQUIREKEY%
               // v2p.extraV2F5 = d.extraV2F5;
               // #endif

               // #if %EXTRAV2F6REQUIREKEY%
               // v2p.extraV2F6 = d.extraV2F6;
               // #endif

               // #if %EXTRAV2F7REQUIREKEY%
               // v2p.extraV2F7 = d.extraV2F7;
               // #endif
            }

            void ChainFinalColorForward(inout Surface l, inout ShaderData d, inout half4 color)
            {
               //   Ext_FinalColorForward0(l, d, color);
               //   Ext_FinalColorForward1(l, d, color);
               //   Ext_FinalColorForward2(l, d, color);
               //   Ext_FinalColorForward3(l, d, color);
               //   Ext_FinalColorForward4(l, d, color);
               //   Ext_FinalColorForward5(l, d, color);
               //   Ext_FinalColorForward6(l, d, color);
               //   Ext_FinalColorForward7(l, d, color);
               //   Ext_FinalColorForward8(l, d, color);
               //   Ext_FinalColorForward9(l, d, color);
               //  Ext_FinalColorForward10(l, d, color);
               //  Ext_FinalColorForward11(l, d, color);
               //  Ext_FinalColorForward12(l, d, color);
               //  Ext_FinalColorForward13(l, d, color);
               //  Ext_FinalColorForward14(l, d, color);
               //  Ext_FinalColorForward15(l, d, color);
               //  Ext_FinalColorForward16(l, d, color);
               //  Ext_FinalColorForward17(l, d, color);
               //  Ext_FinalColorForward18(l, d, color);
               //  Ext_FinalColorForward19(l, d, color);
               //  Ext_FinalColorForward20(l, d, color);
               //  Ext_FinalColorForward21(l, d, color);
               //  Ext_FinalColorForward22(l, d, color);
               //  Ext_FinalColorForward23(l, d, color);
               //  Ext_FinalColorForward24(l, d, color);
               //  Ext_FinalColorForward25(l, d, color);
               //  Ext_FinalColorForward26(l, d, color);
               //  Ext_FinalColorForward27(l, d, color);
               //  Ext_FinalColorForward28(l, d, color);
               //  Ext_FinalColorForward29(l, d, color);
            }

            void ChainFinalGBufferStandard(inout Surface s, inout ShaderData d, inout half4 GBuffer0, inout half4 GBuffer1, inout half4 GBuffer2, inout half4 outEmission, inout half4 outShadowMask)
            {
               //   Ext_FinalGBufferStandard0(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard1(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard2(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard3(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard4(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard5(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard6(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard7(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard8(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard9(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard10(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard11(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard12(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard13(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard14(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard15(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard16(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard17(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard18(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard19(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard20(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard21(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard22(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard23(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard24(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard25(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard26(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard27(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard28(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard29(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
            }



            

         ShaderData CreateShaderData(VertexToPixel i
                  #if NEED_FACING
                     , bool facing
                  #endif
         )
         {
            ShaderData d = (ShaderData)0;
            d.clipPos = i.pos;
            d.worldSpacePosition = i.worldPos;

            d.worldSpaceNormal = normalize(i.worldNormal);
            d.worldSpaceTangent = normalize(i.worldTangent.xyz);
            d.tangentSign = i.worldTangent.w;
            float3 bitangent = cross(i.worldTangent.xyz, i.worldNormal) * d.tangentSign * -1;
            

            d.TBNMatrix = float3x3(d.worldSpaceTangent, bitangent, d.worldSpaceNormal);
            d.worldSpaceViewDir = normalize(_WorldSpaceCameraPos - i.worldPos);

            d.tangentSpaceViewDir = mul(d.TBNMatrix, d.worldSpaceViewDir);
             d.texcoord0 = i.texcoord0;
             d.texcoord1 = i.texcoord1;
             d.texcoord2 = i.texcoord2;

            // #if %TEXCOORD3REQUIREKEY%
             d.texcoord3 = i.texcoord3;
            // #endif

            // d.isFrontFace = facing;
            // #if %VERTEXCOLORREQUIREKEY%
            // d.vertexColor = i.vertexColor;
            // #endif

            // these rarely get used, so we back transform them. Usually will be stripped.
            #if _HDRP
                // d.localSpacePosition = mul(unity_WorldToObject, float4(GetCameraRelativePositionWS(i.worldPos), 1)).xyz;
            #else
                // d.localSpacePosition = mul(unity_WorldToObject, float4(i.worldPos, 1)).xyz;
            #endif
            // d.localSpaceNormal = normalize(mul((float3x3)unity_WorldToObject, i.worldNormal));
            // d.localSpaceTangent = normalize(mul((float3x3)unity_WorldToObject, i.worldTangent.xyz));

            // #if %SCREENPOSREQUIREKEY%
            // d.screenPos = i.screenPos;
            // d.screenUV = (i.screenPos.xy / i.screenPos.w);
            // #endif


            // #if %EXTRAV2F0REQUIREKEY%
            // d.extraV2F0 = i.extraV2F0;
            // #endif

            // #if %EXTRAV2F1REQUIREKEY%
            // d.extraV2F1 = i.extraV2F1;
            // #endif

            // #if %EXTRAV2F2REQUIREKEY%
            // d.extraV2F2 = i.extraV2F2;
            // #endif

            // #if %EXTRAV2F3REQUIREKEY%
            // d.extraV2F3 = i.extraV2F3;
            // #endif

            // #if %EXTRAV2F4REQUIREKEY%
            // d.extraV2F4 = i.extraV2F4;
            // #endif

            // #if %EXTRAV2F5REQUIREKEY%
            // d.extraV2F5 = i.extraV2F5;
            // #endif

            // #if %EXTRAV2F6REQUIREKEY%
            // d.extraV2F6 = i.extraV2F6;
            // #endif

            // #if %EXTRAV2F7REQUIREKEY%
            // d.extraV2F7 = i.extraV2F7;
            // #endif

            return d;
         }
         

            

struct VaryingsToPS
{
   VertexToPixel vmesh;
   #ifdef VARYINGS_NEED_PASS
      VaryingsPassToPS vpass;
   #endif
};

struct PackedVaryingsToPS
{
   #ifdef VARYINGS_NEED_PASS
      PackedVaryingsPassToPS vpass;
   #endif
   VertexToPixel vmesh;

   UNITY_VERTEX_OUTPUT_STEREO
};

PackedVaryingsToPS PackVaryingsToPS(VaryingsToPS input)
{
   PackedVaryingsToPS output = (PackedVaryingsToPS)0;
   output.vmesh = input.vmesh;
   #ifdef VARYINGS_NEED_PASS
      output.vpass = PackVaryingsPassToPS(input.vpass);
   #endif

   UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
   return output;
}




VertexToPixel VertMesh(VertexData input)
{
    VertexToPixel output = (VertexToPixel)0;

    UNITY_SETUP_INSTANCE_ID(input);
    UNITY_TRANSFER_INSTANCE_ID(input, output);

    
    ChainModifyVertex(input, output, _Time);


    // This return the camera relative position (if enable)
    float3 positionRWS = TransformObjectToWorld(input.vertex.xyz);
    float3 normalWS = TransformObjectToWorldNormal(input.normal);
    float4 tangentWS = float4(TransformObjectToWorldDir(input.tangent.xyz), input.tangent.w);


    output.worldPos = GetAbsolutePositionWS(positionRWS);
    output.pos = TransformWorldToHClip(positionRWS);
    output.worldNormal = normalWS;
    output.worldTangent = tangentWS;


    output.texcoord0 = input.texcoord0;
    output.texcoord1 = input.texcoord1;
    output.texcoord2 = input.texcoord2;
    // #if %TEXCOORD3REQUIREKEY%
     output.texcoord3 = input.texcoord3;
    // #endif

    // #if %SCREENPOSREQUIREKEY%
    // output.screenPos = ComputeScreenPos(output.pos, _ProjectionParams.x);
    // #endif

    // #if %VERTEXCOLORREQUIREKEY%
    // output.vertexColor = input.vertexColor;
    // #endif
    return output;
}


#if (SHADERPASS == SHADERPASS_DBUFFER_MESH)
void MeshDecalsPositionZBias(inout VaryingsToPS input)
{
#if defined(UNITY_REVERSED_Z)
    input.vmesh.pos.z -= _DecalMeshDepthBias;
#else
    input.vmesh.pos.z += _DecalMeshDepthBias;
#endif
}
#endif


#if (SHADERPASS == SHADERPASS_LIGHT_TRANSPORT)

// This was not in constant buffer in original unity, so keep outiside. But should be in as ShaderRenderPass frequency
float unity_OneOverOutputBoost;
float unity_MaxOutputValue;

CBUFFER_START(UnityMetaPass)
// x = use uv1 as raster position
// y = use uv2 as raster position
bool4 unity_MetaVertexControl;

// x = return albedo
// y = return normal
bool4 unity_MetaFragmentControl;
CBUFFER_END

PackedVaryingsToPS Vert(VertexData inputMesh)
{
    VaryingsToPS output = (VaryingsToPS)0;
    output.vmesh = (VertexToPixel)0;

    UNITY_SETUP_INSTANCE_ID(inputMesh);
    UNITY_TRANSFER_INSTANCE_ID(inputMesh, output.vmesh);

    // Output UV coordinate in vertex shader
    float2 uv = float2(0.0, 0.0);

    if (unity_MetaVertexControl.x)
    {
        uv = inputMesh.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
    }
    else if (unity_MetaVertexControl.y)
    {
        uv = inputMesh.texcoord2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
    }

    // OpenGL right now needs to actually use the incoming vertex position
    // so we create a fake dependency on it here that haven't any impact.
    output.vmesh.pos = float4(uv * 2.0 - 1.0, inputMesh.vertex.z > 0 ? 1.0e-4 : 0.0, 1.0);

#ifdef VARYINGS_NEED_POSITION_WS
    output.vmesh.worldPos = TransformObjectToWorld(inputMesh.vertex.xyz);
#endif

#ifdef VARYINGS_NEED_TANGENT_TO_WORLD
    // Normal is required for triplanar mapping
    output.vmesh.worldNormal = TransformObjectToWorldNormal(inputMesh.normal);
    // Not required but assign to silent compiler warning
    output.vmesh.worldTangent = float4(1.0, 0.0, 0.0, 0.0);
#endif

    output.vmesh.texcoord0 = inputMesh.texcoord0;
    output.vmesh.texcoord1 = inputMesh.texcoord1;
    output.vmesh.texcoord2 = inputMesh.texcoord2;
    // #if %TEXCOORD3REQUIREKEY%
     output.vmesh.texcoord3 = inputMesh.texcoord3;
    // #endif

    // #if %VERTEXCOLORREQUIREKEY%
    // output.vmesh.vertexColor = inputMesh.vertexColor;
    // #endif

    return PackVaryingsToPS(output);
}
#else

PackedVaryingsToPS Vert(VertexData inputMesh)
{
    VaryingsToPS varyingsType;
    varyingsType.vmesh = VertMesh(inputMesh);
    #if (SHADERPASS == SHADERPASS_DBUFFER_MESH)
       MeshDecalsPositionZBias(varyingsType);
    #endif
    return PackVaryingsToPS(varyingsType);
}

#endif



            

            
                FragInputs BuildFragInputs(VertexToPixel input)
                {
                    UNITY_SETUP_INSTANCE_ID(input);
                    FragInputs output;
                    ZERO_INITIALIZE(FragInputs, output);
            
                    // Init to some default value to make the computer quiet (else it output 'divide by zero' warning even if value is not used).
                    // TODO: this is a really poor workaround, but the variable is used in a bunch of places
                    // to compute normals which are then passed on elsewhere to compute other values...
                    output.tangentToWorld = k_identity3x3;
                    output.positionSS = input.pos;       // input.positionCS is SV_Position
            
                    output.positionRWS = GetCameraRelativePositionWS(input.worldPos);
                    output.tangentToWorld = BuildTangentToWorld(input.worldTangent, input.worldNormal);
                    output.texCoord0 = input.texcoord0;
                    output.texCoord1 = input.texcoord1;
                    output.texCoord2 = input.texcoord2;
            
                    return output;
                }
            
               void BuildSurfaceData(FragInputs fragInputs, inout Surface surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData, out float3 bentNormalWS)
               {
                   // setup defaults -- these are used if the graph doesn't output a value
                   ZERO_INITIALIZE(SurfaceData, surfaceData);
        
                   // specularOcclusion need to be init ahead of decal to quiet the compiler that modify the SurfaceData struct
                   // however specularOcclusion can come from the graph, so need to be init here so it can be override.
                   surfaceData.specularOcclusion = 1.0;
        
                   // copy across graph values, if defined
                   surfaceData.baseColor =                 surfaceDescription.Albedo;
                   surfaceData.perceptualSmoothness =      surfaceDescription.Smoothness;
                   surfaceData.ambientOcclusion =          surfaceDescription.Occlusion;
                   surfaceData.specularOcclusion =         surfaceDescription.SpecularOcclusion;
                   surfaceData.metallic =                  surfaceDescription.Metallic;
                   surfaceData.subsurfaceMask =            surfaceDescription.SubsurfaceMask;
                   surfaceData.thickness =                 surfaceDescription.Thickness;
                   surfaceData.diffusionProfileHash =      asuint(surfaceDescription.DiffusionProfileHash);
                   #if _USESPECULAR
                      surfaceData.specularColor =             surfaceDescription.Specular;
                   #endif
                   surfaceData.coatMask =                  surfaceDescription.CoatMask;
                   surfaceData.anisotropy =                surfaceDescription.Anisotropy;
                   surfaceData.iridescenceMask =           surfaceDescription.IridescenceMask;
                   surfaceData.iridescenceThickness =      surfaceDescription.IridescenceThickness;
        
           #ifdef _HAS_REFRACTION
                   if (_EnableSSRefraction)
                   {
                       // surfaceData.ior =                       surfaceDescription.RefractionIndex;
                       // surfaceData.transmittanceColor =        surfaceDescription.RefractionColor;
                       // surfaceData.atDistance =                surfaceDescription.RefractionDistance;
        
                       surfaceData.transmittanceMask = (1.0 - surfaceDescription.Alpha);
                       surfaceDescription.Alpha = 1.0;
                   }
                   else
                   {
                       surfaceData.ior = surfaceDescription.ior;
                       surfaceData.transmittanceColor = surfaceDescription.transmittanceColor;
                       surfaceData.atDistance = surfaceDescription.atDistance;
                       surfaceData.transmittanceMask = surfaceDescription.transmittanceMask;
                       surfaceDescription.Alpha = 1.0;
                   }
           #else
                   surfaceData.ior = 1.0;
                   surfaceData.transmittanceColor = float3(1.0, 1.0, 1.0);
                   surfaceData.atDistance = 1.0;
                   surfaceData.transmittanceMask = 0.0;
           #endif
                
                   // These static material feature allow compile time optimization
                   surfaceData.materialFeatures = MATERIALFEATUREFLAGS_LIT_STANDARD;
           #ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
                   surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SUBSURFACE_SCATTERING;
           #endif
           #ifdef _MATERIAL_FEATURE_TRANSMISSION
                   surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_TRANSMISSION;
           #endif
           #ifdef _MATERIAL_FEATURE_ANISOTROPY
                   surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_ANISOTROPY;
           #endif
                   // surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_CLEAR_COAT;
        
           #ifdef _MATERIAL_FEATURE_IRIDESCENCE
                   surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_IRIDESCENCE;
           #endif
           #ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
                   surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SPECULAR_COLOR;
           #endif
        
           #if defined (_MATERIAL_FEATURE_SPECULAR_COLOR) && defined (_ENERGY_CONSERVING_SPECULAR)
                   // Require to have setup baseColor
                   // Reproduce the energy conservation done in legacy Unity. Not ideal but better for compatibility and users can unchek it
                   surfaceData.baseColor *= (1.0 - Max3(surfaceData.specularColor.r, surfaceData.specularColor.g, surfaceData.specularColor.b));
           #endif

        
                   // tangent-space normal
                   float3 normalTS = float3(0.0f, 0.0f, 1.0f);
                   normalTS = surfaceDescription.Normal;
        
                   // compute world space normal
                   #if !_WORLDSPACENORMAL
                      surfaceData.normalWS = mul(normalTS, fragInputs.tangentToWorld);
                   #else
                      surfaceData.normalWS = normalTS;  
                   #endif
                   surfaceData.geomNormalWS = fragInputs.tangentToWorld[2];
        
                   surfaceData.tangentWS = normalize(fragInputs.tangentToWorld[0].xyz);    // The tangent is not normalize in tangentToWorld for mikkt. TODO: Check if it expected that we normalize with Morten. Tag: SURFACE_GRADIENT
                   // surfaceData.tangentWS = TransformTangentToWorld(surfaceDescription.Tangent, fragInputs.tangentToWorld);
        
           #if HAVE_DECALS
                   if (_EnableDecals)
                   {
                       #if VERSION_GREATER_EQUAL(10,2)
                          DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput,  surfaceData.geomNormalWS, surfaceDescription.Alpha);
                          ApplyDecalToSurfaceData(decalSurfaceData,  surfaceData.geomNormalWS, surfaceData);
                       #else
                          DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, surfaceDescription.Alpha);
                          ApplyDecalToSurfaceData(decalSurfaceData, surfaceData);
                       #endif
                   }
           #endif
        
                   bentNormalWS = surfaceData.normalWS;
               
                   surfaceData.tangentWS = Orthonormalize(surfaceData.tangentWS, surfaceData.normalWS);
        
        
                   // By default we use the ambient occlusion with Tri-ace trick (apply outside) for specular occlusion.
                   // If user provide bent normal then we process a better term
           #if defined(_SPECULAR_OCCLUSION_CUSTOM)
                   // Just use the value passed through via the slot (not active otherwise)
           #elif defined(_SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL)
                   // If we have bent normal and ambient occlusion, process a specular occlusion
                   surfaceData.specularOcclusion = GetSpecularOcclusionFromBentAO(V, bentNormalWS, surfaceData.normalWS, surfaceData.ambientOcclusion, PerceptualSmoothnessToPerceptualRoughness(surfaceData.perceptualSmoothness));
           #elif defined(_AMBIENT_OCCLUSION) && defined(_SPECULAR_OCCLUSION_FROM_AO)
                   surfaceData.specularOcclusion = GetSpecularOcclusionFromAmbientOcclusion(ClampNdotV(dot(surfaceData.normalWS, V)), surfaceData.ambientOcclusion, PerceptualSmoothnessToRoughness(surfaceData.perceptualSmoothness));
           #endif
        
           #ifdef _ENABLE_GEOMETRIC_SPECULAR_AA
                   surfaceData.perceptualSmoothness = GeometricNormalFiltering(surfaceData.perceptualSmoothness, fragInputs.tangentToWorld[2], surfaceDescription.SpecularAAScreenSpaceVariance, surfaceDescription.SpecularAAThreshold);
           #endif
        
           #ifdef DEBUG_DISPLAY
                   if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
                   {
                       // TODO: need to update mip info
                       surfaceData.metallic = 0;
                   }
        
                   // We need to call ApplyDebugToSurfaceData after filling the surfarcedata and before filling builtinData
                   // as it can modify attribute use for static lighting
                   ApplyDebugToSurfaceData(fragInputs.tangentToWorld, surfaceData);
           #endif
               }
        
               void GetSurfaceAndBuiltinData(VertexToPixel m2ps, FragInputs fragInputs, float3 V, inout PositionInputs posInput,
                     out SurfaceData surfaceData, out BuiltinData builtinData, inout Surface l, inout ShaderData d
                     #if NEED_FACING
                        , bool facing
                     #endif
                  )
               {
                 // Removed since crossfade does not work, probably needs extra material setup.   
                 //#ifdef LOD_FADE_CROSSFADE // enable dithering LOD transition if user select CrossFade transition in LOD group
                 //    uint3 fadeMaskSeed = asuint((int3)(V * _ScreenSize.xyx)); // Quantize V to _ScreenSize values
                 //    LODDitheringTransition(fadeMaskSeed, unity_LODFade.x);
                 //#endif
        
                 d = CreateShaderData(m2ps
                  #if NEED_FACING
                    , facing
                  #endif
                 );

                 

                 l = (Surface)0;

                 l.Albedo = half3(0.5, 0.5, 0.5);
                 l.Normal = float3(0,0,1);
                 l.Occlusion = 1;
                 l.Alpha = 1;
                 l.SpecularOcclusion = 1;

                 #ifdef _DEPTHOFFSET_ON
                    l.outputDepth = posInput.deviceDepth;
                 #endif

                 ChainSurfaceFunction(l, d);

                 #ifdef _DEPTHOFFSET_ON
                    posInput.deviceDepth = l.outputDepth;
                 #endif

                 #if _UNLIT
                     //l.Emission = l.Albedo;
                     //l.Albedo = 0;
                     l.Normal = half3(0,0,1);
                     l.Occlusion = 1;
                     l.Metallic = 0;
                     l.Specular = 0;
                 #endif

                 surfaceData.geomNormalWS = d.worldSpaceNormal;
                 surfaceData.tangentWS = d.worldSpaceTangent;
                 fragInputs.tangentToWorld = d.TBNMatrix;

                 float3 bentNormalWS;
                 BuildSurfaceData(fragInputs, l, V, posInput, surfaceData, bentNormalWS);

                 InitBuiltinData(posInput, l.Alpha, bentNormalWS, -d.worldSpaceNormal, fragInputs.texCoord1, fragInputs.texCoord2, builtinData);

                 

                 builtinData.emissiveColor = l.Emission;

                 #if defined(_OVERRIDE_BAKEDGI)
                    builtinData.bakeDiffuseLighting = l.DiffuseGI;
                    builtinData.backBakeDiffuseLighting = l.BackDiffuseGI;
                    builtinData.emissiveColor += l.SpecularGI;
                 #endif
                 
                 #if defined(_OVERRIDE_SHADOWMASK)
                   builtinData.shadowMask0 = l.ShadowMask.x;
                   builtinData.shadowMask1 = l.ShadowMask.y;
                   builtinData.shadowMask2 = l.ShadowMask.z;
                   builtinData.shadowMask3 = l.ShadowMask.w;
                #endif
        
                 #if (SHADERPASS == SHADERPASS_DISTORTION)
                     //builtinData.distortion = surfaceDescription.Distortion;
                     //builtinData.distortionBlur = surfaceDescription.DistortionBlur;
                     builtinData.distortion = float2(0.0, 0.0);
                     builtinData.distortionBlur = 0.0;
                 #else
                     builtinData.distortion = float2(0.0, 0.0);
                     builtinData.distortionBlur = 0.0;
                 #endif
        
                   PostInitBuiltinData(V, posInput, surfaceData, builtinData);
               }


            float4 Frag(PackedVaryingsToPS packedInput
               #if NEED_FACING
                  , bool facing : SV_IsFrontFace
               #endif

            ) : SV_Target
            {
                FragInputs input = BuildFragInputs(packedInput.vmesh);

                // input.positionSS is SV_Position
                PositionInputs posInput = GetPositionInput(input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS);

                float3 V = GetWorldSpaceNormalizeViewDir(input.positionRWS);


                SurfaceData surfaceData;
                BuiltinData builtinData;
                Surface l;
                ShaderData d;
                GetSurfaceAndBuiltinData(packedInput.vmesh, input, V, posInput, surfaceData, builtinData, l, d
               #if NEED_FACING
                  , facing
               #endif
               );

                // no debug apply during light transport pass

                BSDFData bsdfData = ConvertSurfaceDataToBSDFData(input.positionSS.xy, surfaceData);
                LightTransportData lightTransportData = GetLightTransportData(surfaceData, builtinData, bsdfData);

                // This shader is call two times. Once for getting emissiveColor, the other time to get diffuseColor
                // We use unity_MetaFragmentControl to make the distinction.
                float4 res = float4(0.0, 0.0, 0.0, 1.0);

                if (unity_MetaFragmentControl.x)
                {
                    // Apply diffuseColor Boost from LightmapSettings.
                    // put abs here to silent a warning, no cost, no impact as color is assume to be positive.
                    res.rgb = clamp(pow(abs(lightTransportData.diffuseColor), saturate(unity_OneOverOutputBoost)), 0, unity_MaxOutputValue);
                }

                if (unity_MetaFragmentControl.y)
                {
                    // emissive use HDR format
                    res.rgb = lightTransportData.emissiveColor;
                }

                return res;
            }



            ENDHLSL
        }
        
              Pass
        {
            // based on HDLitPass.template
            Name "SceneSelectionPass"
            Tags { "LightMode" = "SceneSelectionPass" }
        
            ColorMask 0

                Cull [_CullMode]


            HLSLPROGRAM
        
            #pragma target 4.5
            #pragma only_renderers d3d11 ps4 xboxone vulkan metal switch
            //#pragma enable_d3d11_debug_symbols
        
            #pragma multi_compile_instancing
 
            //#pragma multi_compile_local _ _ALPHATEST_ON


            //#pragma shader_feature _SURFACE_TYPE_TRANSPARENT
            //#pragma shader_feature_local _ _BLENDMODE_ALPHA _BLENDMODE_ADD _BLENDMODE_PRE_MULTIPLY
        
            //-------------------------------------------------------------------------------------
            // Variant Definitions (active field translations to HDRP defines)
            //-------------------------------------------------------------------------------------
            // #define _MATERIAL_FEATURE_SUBSURFACE_SCATTERING 1
            // #define _MATERIAL_FEATURE_TRANSMISSION 1
            // #define _MATERIAL_FEATURE_ANISOTROPY 1
            // #define _MATERIAL_FEATURE_IRIDESCENCE 1
            // #define _MATERIAL_FEATURE_SPECULAR_COLOR 1
            #define _ENABLE_FOG_ON_TRANSPARENT 1
            // #define _AMBIENT_OCCLUSION 1
            // #define _SPECULAR_OCCLUSION_FROM_AO 1
            // #define _SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL 1
            // #define _SPECULAR_OCCLUSION_CUSTOM 1
            // #define _ENERGY_CONSERVING_SPECULAR 1
            // #define _ENABLE_GEOMETRIC_SPECULAR_AA 1
            // #define _HAS_REFRACTION 1
            // #define _REFRACTION_PLANE 1
            // #define _REFRACTION_SPHERE 1
            // #define _DISABLE_DECALS 1
            // #define _DISABLE_SSR 1
            // #define _ADD_PRECOMPUTED_VELOCITY
            // #define _WRITE_TRANSPARENT_MOTION_VECTOR 1
            // #define _DEPTHOFFSET_ON 1
            // #define _BLENDMODE_PRESERVE_SPECULAR_LIGHTING 1

            #define SHADERPASS SHADERPASS_DEPTH_ONLY
            #define SCENESELECTIONPASS
            #pragma editor_sync_compilation
            #define RAYTRACING_SHADER_GRAPH_HIGH
            #define _PASSSCENESELECT 1

            


    #pragma shader_feature_local _ALPHATEST_ON

    #pragma shader_feature_local_fragment _NORMALMAP_TANGENT_SPACE


    #pragma shader_feature_local _NORMALMAP
    #pragma shader_feature_local_fragment _MASKMAP
    #pragma shader_feature_local_fragment _EMISSIVE_COLOR_MAP
    #pragma shader_feature_local_fragment _ANISOTROPYMAP
    #pragma shader_feature_local_fragment _DETAIL_MAP
    #pragma shader_feature_local_fragment _SUBSURFACE_MASK_MAP
    #pragma shader_feature_local_fragment _THICKNESSMAP
    #pragma shader_feature_local_fragment _IRIDESCENCE_THICKNESSMAP
    #pragma shader_feature_local_fragment _SPECULARCOLORMAP

    // MaterialFeature are used as shader feature to allow compiler to optimize properly
    #pragma shader_feature_local_fragment _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
    #pragma shader_feature_local_fragment _MATERIAL_FEATURE_TRANSMISSION
    #pragma shader_feature_local_fragment _MATERIAL_FEATURE_ANISOTROPY
    #pragma shader_feature_local_fragment _MATERIAL_FEATURE_CLEAR_COAT
    #pragma shader_feature_local_fragment _MATERIAL_FEATURE_IRIDESCENCE
    #pragma shader_feature_local_fragment _MATERIAL_FEATURE_SPECULAR_COLOR


    #pragma shader_feature_local_fragment _ENABLESPECULAROCCLUSION
    #pragma shader_feature_local_fragment _ _SPECULAR_OCCLUSION_NONE _SPECULAR_OCCLUSION_FROM_BENT_NORMAL_MAP

    #ifdef _ENABLESPECULAROCCLUSION
        #define _SPECULAR_OCCLUSION_FROM_BENT_NORMAL_MAP
    #endif


        #pragma shader_feature_local_fragment _OBSTRUCTION_CURVE
        #pragma shader_feature_local_fragment _DISSOLVEMASK
        #pragma shader_feature_local_fragment _ZONING
        #pragma shader_feature_local_fragment _REPLACEMENT
        #pragma shader_feature_local_fragment _PLAYERINDEPENDENT


   #define _HDRP 1
#define _USINGTEXCOORD1 1
#define _USINGTEXCOORD2 1


               #pragma vertex Vert
   #pragma fragment Frag
        
            
        
                  // useful conversion functions to make surface shader code just work

      #define UNITY_DECLARE_TEX2D(name) TEXTURE2D(name); SAMPLER(sampler##name);
      #define UNITY_DECLARE_TEX2D_NOSAMPLER(name) TEXTURE2D(name);
      #define UNITY_DECLARE_TEX2DARRAY(name) TEXTURE2D_ARRAY(name); SAMPLER(sampler##name);
      #define UNITY_DECLARE_TEX2DARRAY_NOSAMPLER(tex) TEXTURE2D_ARRAY(tex);

      #define UNITY_SAMPLE_TEX2DARRAY(tex,coord)            SAMPLE_TEXTURE2D_ARRAY(tex, sampler##tex, coord.xy, coord.z)
      #define UNITY_SAMPLE_TEX2DARRAY_LOD(tex,coord,lod)    SAMPLE_TEXTURE2D_ARRAY_LOD(tex, sampler##tex, coord.xy, coord.z, lod)
      #define UNITY_SAMPLE_TEX2D(tex, coord)                SAMPLE_TEXTURE2D(tex, sampler##tex, coord)
      #define UNITY_SAMPLE_TEX2D_SAMPLER(tex, samp, coord)  SAMPLE_TEXTURE2D(tex, sampler##samp, coord)

      #define UNITY_SAMPLE_TEX2D_LOD(tex,coord, lod)   SAMPLE_TEXTURE2D_LOD(tex, sampler_##tex, coord, lod)
      #define UNITY_SAMPLE_TEX2D_SAMPLER_LOD(tex,samplertex,coord, lod) SAMPLE_TEXTURE2D_LOD (tex, sampler##samplertex,coord, lod)

      #if defined(UNITY_COMPILER_HLSL)
         #define UNITY_INITIALIZE_OUTPUT(type,name) name = (type)0;
      #else
         #define UNITY_INITIALIZE_OUTPUT(type,name)
      #endif

      #define sampler2D_float sampler2D
      #define sampler2D_half sampler2D

      #undef WorldNormalVector
      #define WorldNormalVector(data, normal) mul(normal, data.TBNMatrix)

      #define UnityObjectToWorldNormal(normal) mul(GetObjectToWorldMatrix(), normal)




// HDRP Adapter stuff


            // If we use subsurface scattering, enable output split lighting (for forward pass)
            #if defined(_MATERIAL_FEATURE_SUBSURFACE_SCATTERING) && !defined(_SURFACE_TYPE_TRANSPARENT)
            #define OUTPUT_SPLIT_LIGHTING
            #endif

            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Version.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
        
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"
        
            // define FragInputs structure
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
            #if (SHADER_LIBRARY_VERSION_MAJOR >= 10)
               #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/Functions.hlsl"
            #endif


        

            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
        #ifdef DEBUG_DISPLAY
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
        #endif
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
        
        #if (SHADERPASS == SHADERPASS_FORWARD)
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/Lighting.hlsl"
        
            #define HAS_LIGHTLOOP
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/LightLoopDef.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/LightLoop.hlsl"
        #else
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
        #endif
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalUtilities.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitDecalData.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"
        
            // Used by SceneSelectionPass
            int _ObjectId;
            int _PassValue;
        
           
            // data across stages, stripped like the above.
            struct VertexToPixel
            {
               float4 pos : SV_POSITION;
               float3 worldPos : TEXCOORD0;
               float3 worldNormal : TEXCOORD1;
               float4 worldTangent : TEXCOORD2;
               float4 texcoord0 : TEXCOORD3;
               float4 texcoord1 : TEXCOORD4;
               float4 texcoord2 : TEXCOORD5;

               // #if %TEXCOORD3REQUIREKEY%
                float4 texcoord3 : TEXCOORD6;
               // #endif

               // #if %SCREENPOSREQUIREKEY%
               // float4 screenPos : TEXCOORD7;
               // #endif

               // #if %VERTEXCOLORREQUIREKEY%
               // float4 vertexColor : COLOR;
               // #endif

               // #if %EXTRAV2F0REQUIREKEY%
               // float4 extraV2F0 : TEXCOORD8;
               // #endif

               // #if %EXTRAV2F1REQUIREKEY%
               // float4 extraV2F1 : TEXCOORD9;
               // #endif

               // #if %EXTRAV2F2REQUIREKEY%
               // float4 extraV2F2 : TEXCOORD10;
               // #endif

               // #if %EXTRAV2F3REQUIREKEY%
               // float4 extraV2F3 : TEXCOORD11;
               // #endif

               // #if %EXTRAV2F4REQUIREKEY%
               // float4 extraV2F4 : TEXCOORD12;
               // #endif

               // #if %EXTRAV2F5REQUIREKEY%
               // float4 extraV2F5 : TEXCOORD13;
               // #endif

               // #if %EXTRAV2F6REQUIREKEY%
               // float4 extraV2F6 : TEXCOORD14;
               // #endif

               // #if %EXTRAV2F7REQUIREKEY%
               // float4 extraV2F7 : TEXCOORD15;
               // #endif

               #if UNITY_ANY_INSTANCING_ENABLED
                  UNITY_VERTEX_INPUT_INSTANCE_ID
               #endif // UNITY_ANY_INSTANCING_ENABLED

               UNITY_VERTEX_OUTPUT_STEREO
            };
      
  
            
            
            // data describing the user output of a pixel
            struct Surface
            {
               half3 Albedo;
               half Height;
               half3 Normal;
               half Smoothness;
               half3 Emission;
               half Metallic;
               half3 Specular;
               half Occlusion;
               half SpecularPower; // for simple lighting
               half Alpha;
               float outputDepth; // if written, SV_Depth semantic is used. ShaderData.clipPos.z is unused value
               // HDRP Only
               half SpecularOcclusion;
               half SubsurfaceMask;
               half Thickness;
               half CoatMask;
               half CoatSmoothness;
               half Anisotropy;
               half IridescenceMask;
               half IridescenceThickness;
               int DiffusionProfileHash;
               float SpecularAAThreshold;
               float SpecularAAScreenSpaceVariance;
               // requires _OVERRIDE_BAKEDGI to be defined, but is mapped in all pipelines
               float3 DiffuseGI;
               float3 BackDiffuseGI;
               float3 SpecularGI;
               float ior;
               float3 transmittanceColor;
               float atDistance;
               float transmittanceMask;
               // requires _OVERRIDE_SHADOWMASK to be defines
               float4 ShadowMask;


            };

            // Data the user declares in blackboard blocks
            struct Blackboard
            {
                
                float blackboardDummyData;
            };

            // data the user might need, this will grow to be big. But easy to strip
            struct ShaderData
            {
               float4 clipPos; // SV_POSITION
               float3 localSpacePosition;
               float3 localSpaceNormal;
               float3 localSpaceTangent;
        
               float3 worldSpacePosition;
               float3 worldSpaceNormal;
               float3 worldSpaceTangent;
               float tangentSign;

               float3 worldSpaceViewDir;
               float3 tangentSpaceViewDir;

               float4 texcoord0;
               float4 texcoord1;
               float4 texcoord2;
               float4 texcoord3;

               float2 screenUV;
               float4 screenPos;

               float4 vertexColor;
               bool isFrontFace;

               float4 extraV2F0;
               float4 extraV2F1;
               float4 extraV2F2;
               float4 extraV2F3;
               float4 extraV2F4;
               float4 extraV2F5;
               float4 extraV2F6;
               float4 extraV2F7;

               float3x3 TBNMatrix;
               Blackboard blackboard;
            };

            struct VertexData
            {
               #if SHADER_TARGET > 30
               // UNITY_VERTEX_INPUT_INSTANCE_ID;
               #endif
               float4 vertex : POSITION;
               float3 normal : NORMAL;
               float4 tangent : TANGENT;
               float4 texcoord0 : TEXCOORD0;

               // optimize out mesh coords when not in use by user or lighting system
               #if _URP && (_USINGTEXCOORD1 || _PASSMETA || _PASSFORWARD || _PASSGBUFFER)
                  float4 texcoord1 : TEXCOORD1;
               #endif

               #if _URP && (_USINGTEXCOORD2 || _PASSMETA || ((_PASSFORWARD || _PASSGBUFFER) && defined(DYNAMICLIGHTMAP_ON)))
                  float4 texcoord2 : TEXCOORD2;
               #endif

               #if _STANDARD && (_USINGTEXCOORD1 || (_PASSMETA || ((_PASSFORWARD || _PASSGBUFFER || _PASSFORWARDADD) && LIGHTMAP_ON)))
                  float4 texcoord1 : TEXCOORD1;
               #endif
               #if _STANDARD && (_USINGTEXCOORD2 || (_PASSMETA || ((_PASSFORWARD || _PASSGBUFFER) && DYNAMICLIGHTMAP_ON)))
                  float4 texcoord2 : TEXCOORD2;
               #endif


               #if _HDRP
                  float4 texcoord1 : TEXCOORD1;
                  float4 texcoord2 : TEXCOORD2;
               #endif

               // #if %TEXCOORD3REQUIREKEY%
                float4 texcoord3 : TEXCOORD3;
               // #endif

               // #if %VERTEXCOLORREQUIREKEY%
               // float4 vertexColor : COLOR;
               // #endif

               #if _HDRP && (_PASSMOTIONVECTOR || ((_PASSFORWARD || _PASSUNLIT) && defined(_WRITE_TRANSPARENT_MOTION_VECTOR)))
                  float3 previousPositionOS : TEXCOORD4; // Contain previous transform position (in case of skinning for example)
                  #if defined (_ADD_PRECOMPUTED_VELOCITY)
                     float3 precomputedVelocity    : TEXCOORD5; // Add Precomputed Velocity (Alembic computes velocities on runtime side).
                  #endif
               #endif

               UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct TessVertex 
            {
               float4 vertex : INTERNALTESSPOS;
               float3 normal : NORMAL;
               float4 tangent : TANGENT;
               float4 texcoord0 : TEXCOORD0;
               float4 texcoord1 : TEXCOORD1;
               float4 texcoord2 : TEXCOORD2;

               // #if %TEXCOORD3REQUIREKEY%
                float4 texcoord3 : TEXCOORD3;
               // #endif

               // #if %VERTEXCOLORREQUIREKEY%
               // float4 vertexColor : COLOR;
               // #endif

               // #if %EXTRAV2F0REQUIREKEY%
               // float4 extraV2F0 : TEXCOORD5;
               // #endif

               // #if %EXTRAV2F1REQUIREKEY%
               // float4 extraV2F1 : TEXCOORD6;
               // #endif

               // #if %EXTRAV2F2REQUIREKEY%
               // float4 extraV2F2 : TEXCOORD7;
               // #endif

               // #if %EXTRAV2F3REQUIREKEY%
               // float4 extraV2F3 : TEXCOORD8;
               // #endif

               // #if %EXTRAV2F4REQUIREKEY%
               // float4 extraV2F4 : TEXCOORD9;
               // #endif

               // #if %EXTRAV2F5REQUIREKEY%
               // float4 extraV2F5 : TEXCOORD10;
               // #endif

               // #if %EXTRAV2F6REQUIREKEY%
               // float4 extraV2F6 : TEXCOORD11;
               // #endif

               // #if %EXTRAV2F7REQUIREKEY%
               // float4 extraV2F7 : TEXCOORD12;
               // #endif

               #if _HDRP && (_PASSMOTIONVECTOR || ((_PASSFORWARD || _PASSUNLIT) && defined(_WRITE_TRANSPARENT_MOTION_VECTOR)))
                  float3 previousPositionOS : TEXCOORD13; // Contain previous transform position (in case of skinning for example)
                  #if defined (_ADD_PRECOMPUTED_VELOCITY)
                     float3 precomputedVelocity : TEXCOORD14;
                  #endif
               #endif

               UNITY_VERTEX_INPUT_INSTANCE_ID
               UNITY_VERTEX_OUTPUT_STEREO
            };

            struct ExtraV2F
            {
               float4 extraV2F0;
               float4 extraV2F1;
               float4 extraV2F2;
               float4 extraV2F3;
               float4 extraV2F4;
               float4 extraV2F5;
               float4 extraV2F6;
               float4 extraV2F7;
               Blackboard blackboard;
               float4 time;
            };


            float3 WorldToTangentSpace(ShaderData d, float3 normal)
            {
               return mul(d.TBNMatrix, normal);
            }

            float3 TangentToWorldSpace(ShaderData d, float3 normal)
            {
               return mul(normal, d.TBNMatrix);
            }

            // in this case, make standard more like SRPs, because we can't fix
            // unity_WorldToObject in HDRP, since it already does macro-fu there

            #if _STANDARD
               float3 TransformWorldToObject(float3 p) { return mul(unity_WorldToObject, float4(p, 1)); };
               float3 TransformObjectToWorld(float3 p) { return mul(unity_ObjectToWorld, float4(p, 1)); };
               float4 TransformWorldToObject(float4 p) { return mul(unity_WorldToObject, p); };
               float4 TransformObjectToWorld(float4 p) { return mul(unity_ObjectToWorld, p); };
               float4x4 GetWorldToObjectMatrix() { return unity_WorldToObject; }
               float4x4 GetObjectToWorldMatrix() { return unity_ObjectToWorld; }
               #if (defined(SHADER_API_D3D11) || defined(SHADER_API_XBOXONE) || defined(UNITY_COMPILER_HLSLCC) || defined(SHADER_API_PSSL) || (SHADER_TARGET_SURFACE_ANALYSIS && !SHADER_TARGET_SURFACE_ANALYSIS_MOJOSHADER))
                 #define UNITY_SAMPLE_TEX2D_LOD(tex,coord, lod) tex.SampleLevel (sampler##tex,coord, lod)
                 #define UNITY_SAMPLE_TEX2D_SAMPLER_LOD(tex,samplertex,coord, lod) tex.SampleLevel (sampler##samplertex,coord, lod)
              #else
                 #define UNITY_SAMPLE_TEX2D_LOD(tex,coord,lod) tex2D (tex,coord,0,lod)
                 #define UNITY_SAMPLE_TEX2D_SAMPLER_LOD(tex,samplertex,coord,lod) tex2D (tex,coord,0,lod)
              #endif

               #undef GetWorldToObjectMatrix()

               #define GetWorldToObjectMatrix()   unity_WorldToObject


            #endif

            float3 GetCameraWorldPosition()
            {
               #if _HDRP
                  return GetCameraRelativePositionWS(_WorldSpaceCameraPos);
               #else
                  return _WorldSpaceCameraPos;
               #endif
            }

            #if _GRABPASSUSED
               #if _STANDARD
                  TEXTURE2D(%GRABTEXTURE%);
                  SAMPLER(sampler_%GRABTEXTURE%);
               #endif

               half3 GetSceneColor(float2 uv)
               {
                  #if _STANDARD
                     return SAMPLE_TEXTURE2D(%GRABTEXTURE%, sampler_%GRABTEXTURE%, uv).rgb;
                  #else
                     return SHADERGRAPH_SAMPLE_SCENE_COLOR(uv);
                  #endif
               }
            #endif


      
            #if _STANDARD
               UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);
               float GetSceneDepth(float2 uv) { return SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, uv); }
               float GetLinear01Depth(float2 uv) { return Linear01Depth(GetSceneDepth(uv)); }
               float GetLinearEyeDepth(float2 uv) { return LinearEyeDepth(GetSceneDepth(uv)); } 
            #else
               float GetSceneDepth(float2 uv) { return SHADERGRAPH_SAMPLE_SCENE_DEPTH(uv); }
               float GetLinear01Depth(float2 uv) { return Linear01Depth(GetSceneDepth(uv), _ZBufferParams); }
               float GetLinearEyeDepth(float2 uv) { return LinearEyeDepth(GetSceneDepth(uv), _ZBufferParams); } 
            #endif

            float3 GetWorldPositionFromDepthBuffer(float2 uv, float3 worldSpaceViewDir)
            {
               float eye = GetLinearEyeDepth(uv);
               float3 camView = mul((float3x3)GetObjectToWorldMatrix(), transpose(mul(GetWorldToObjectMatrix(), UNITY_MATRIX_I_V)) [2].xyz);

               float dt = dot(worldSpaceViewDir, camView);
               float3 div = worldSpaceViewDir/dt;
               float3 wpos = (eye * div) + GetCameraWorldPosition();
               return wpos;
            }

            #if _HDRP
            float3 ObjectToWorldSpacePosition(float3 pos)
            {
               return GetAbsolutePositionWS(TransformObjectToWorld(pos));
            }
            #else
            float3 ObjectToWorldSpacePosition(float3 pos)
            {
               return TransformObjectToWorld(pos);
            }
            #endif

            #if _STANDARD
               UNITY_DECLARE_SCREENSPACE_TEXTURE(_CameraDepthNormalsTexture);
               float3 GetSceneNormal(float2 uv, float3 worldSpaceViewDir)
               {
                  float4 depthNorms = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_CameraDepthNormalsTexture, uv);
                  float3 norms = DecodeViewNormalStereo(depthNorms);
                  norms = mul((float3x3)GetWorldToViewMatrix(), norms) * 0.5 + 0.5;
                  return norms;
               }
            #elif _HDRP
               float3 GetSceneNormal(float2 uv, float3 worldSpaceViewDir)
               {
                  NormalData nd;
                  DecodeFromNormalBuffer(_ScreenSize.xy * uv, nd);
                  return nd.normalWS;
               }
            #elif _URP
               #if (SHADER_LIBRARY_VERSION_MAJOR >= 10)
                  #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareNormalsTexture.hlsl"
               #endif

               float3 GetSceneNormal(float2 uv, float3 worldSpaceViewDir)
               {
                  #if (SHADER_LIBRARY_VERSION_MAJOR >= 10)
                     return SampleSceneNormals(uv);
                  #else
                     float3 wpos = GetWorldPositionFromDepthBuffer(uv, worldSpaceViewDir);
                     return normalize(-cross(ddx(wpos), ddy(wpos))) * 0.5 + 0.5;
                  #endif

                }
             #endif

             #if _HDRP

               half3 UnpackNormalmapRGorAG(half4 packednormal)
               {
                     // This do the trick
                  packednormal.x *= packednormal.w;

                  half3 normal;
                  normal.xy = packednormal.xy * 2 - 1;
                  normal.z = sqrt(1 - saturate(dot(normal.xy, normal.xy)));
                  return normal;
               }
               half3 UnpackNormal(half4 packednormal)
               {
                  #if defined(UNITY_NO_DXT5nm)
                     return packednormal.xyz * 2 - 1;
                  #else
                     return UnpackNormalmapRGorAG(packednormal);
                  #endif
               }
               #endif
               #if _HDRP || _URP

               half3 UnpackScaleNormal(half4 packednormal, half scale)
               {
                 #ifndef UNITY_NO_DXT5nm
                   // Unpack normal as DXT5nm (1, y, 1, x) or BC5 (x, y, 0, 1)
                   // Note neutral texture like "bump" is (0, 0, 1, 1) to work with both plain RGB normal and DXT5nm/BC5
                   packednormal.x *= packednormal.w;
                 #endif
                   half3 normal;
                   normal.xy = (packednormal.xy * 2 - 1) * scale;
                   normal.z = sqrt(1 - saturate(dot(normal.xy, normal.xy)));
                   return normal;
               }	

             #endif


            void GetSun(out float3 lightDir, out float3 color)
            {
               lightDir = float3(0.5, 0.5, 0);
               color = 1;
               #if _HDRP
                  if (_DirectionalLightCount > 0)
                  {
                     DirectionalLightData light = _DirectionalLightDatas[0];
                     lightDir = -light.forward.xyz;
                     color = light.color;
                  }
               #elif _STANDARD
			         lightDir = normalize(_WorldSpaceLightPos0.xyz);
                  color = _LightColor0.rgb;
               #elif _URP
	               Light light = GetMainLight();
	               lightDir = light.direction;
	               color = light.color;
               #endif
            }




            CBUFFER_START(UnityPerMaterial)

               float _StencilRef;
               float _StencilWriteMask;
               float _StencilRefDepth;
               float _StencilWriteMaskDepth;
               float _StencilRefMV;
               float _StencilWriteMaskMV;
               float _StencilRefDistortionVec;
               float _StencilWriteMaskDistortionVec;
               float _StencilWriteMaskGBuffer;
               float _StencilRefGBuffer;
               float _ZTestGBuffer;
               float _RequireSplitLighting;
               float _ReceivesSSR;
               float _ZWrite;
               float _TransparentSortPriority;
               float _ZTestDepthEqualForOpaque;
               float _ZTestTransparent;
               float _TransparentBackfaceEnable;
               float _AlphaCutoffEnable;
               float _UseShadowThreshold;

               
    float4 _BaseColorMap_ST;
    float4 _DetailMap_ST;
    float4 _EmissiveColorMap_ST;

    float _UVBase;
    half4 _Color;
    half4 _BaseColor; // TODO CHECK
    half _Cutoff; 
    half _Mode;
    //float _Cull;
    float _CullMode;
    float _CullModeForward;
    half _Metallic;
    half3 _EmissionColor;
    half _UVSec;

    float3 _EmissiveColor;
    float _AlbedoAffectEmissive;
    float _EmissiveExposureWeight;
    float _MetallicRemapMin;
    float _MetallicRemapMax;
    float _Smoothness;
    float _SmoothnessRemapMin;
    float _SmoothnessRemapMax;
    float _AlphaRemapMin;
    float _AlphaRemapMax;
    float _AORemapMin;
    float _AORemapMax;
    float _NormalScale;
    float _DetailAlbedoScale;
    float _DetailNormalScale;
    float _DetailSmoothnessScale;
    float _Anisotropy;
    float _DiffusionProfileHash;
    float _SubsurfaceMask;
    float _Thickness;
    float4 _ThicknessRemap;
    float _IridescenceThickness;
    float4 _IridescenceThicknessRemap;
    float _IridescenceMask;
    float _CoatMask;
    float4 _SpecularColor;
    float _EnergyConservingSpecularColor;
    int _MaterialID;

    float _LinkDetailsWithBase;
    float4 _UVMappingMask;
    float4 _UVDetailsMappingMask;

    float4 _UVMappingMaskEmissive;

    float _AlphaCutoff;
    //float _AlphaCutoffEnable;


    float _SyncCullMode;

    float _IsReplacementShader;
    float _TriggerMode;
    float _RaycastMode;
    float _IsExempt;
    float _isReferenceMaterial;
    float _InteractionMode;
    int _ArrayLength = 0;
    #if (defined(SHADER_API_GLES) || defined(SHADER_API_GLES3)) 
        float4 _PlayersPosVectorArray[20];
        float _PlayersDataFloatArray[150];     
    #else
        float4 _PlayersPosVectorArray[100];
        float _PlayersDataFloatArray[500];  
    #endif
    float _tDirection = 0;
    float _numOfPlayersInside = 0;
    float _tValue = 0;
    float _id = 0;
    #if _ZONING
        #if (defined(SHADER_API_GLES) || defined(SHADER_API_GLES3)) 
            float _ZDFA[500];
        #else
            float _ZDFA[1000];
        #endif
        float _ZonesDataCount;
    #endif
    #if _REPLACEMENT        
        half4 _DissolveColorGlobal;
        float _DissolveColorSaturationGlobal;
        float _DissolveEmissionGlobal;
        float _DissolveEmissionBoosterGlobal;
        float _TextureVisibilityGlobal;
        float _ObstructionGlobal;
        float _AngleStrengthGlobal;
        float _ConeStrengthGlobal;
        float _ConeObstructionDestroyRadiusGlobal;
        float _CylinderStrengthGlobal;
        float _CylinderObstructionDestroyRadiusGlobal;
        float _CircleStrengthGlobal;
        float _CircleObstructionDestroyRadiusGlobal;
        float _CurveStrengthGlobal;
        float _CurveObstructionDestroyRadiusGlobal;
        float _DissolveFallOffGlobal;
        float _IntrinsicDissolveStrengthGlobal;
        float _PreviewModeGlobal;
        float _UVsGlobal;
        float _hasClippedShadowsGlobal;
        float _FloorGlobal;
        float _FloorModeGlobal;
        float _FloorYGlobal;
        float _PlayerPosYOffsetGlobal;
        float _FloorYTextureGradientLengthGlobal;
        float _AnimationEnabledGlobal;
        float _AnimationSpeedGlobal;
        float _DefaultEffectRadiusGlobal;
        float _TransitionDurationGlobal;        
        float _TexturedEmissionEdgeGlobal;
        float _TexturedEmissionEdgeStrengthGlobal;
        float _IsometricExclusionGlobal;
        float _IsometricExclusionDistanceGlobal;
        float _IsometricExclusionGradientLengthGlobal;
        float _CeilingGlobal;
        float _CeilingModeGlobal;
        float _CeilingBlendModeGlobal;
        float _CeilingYGlobal;
        float _CeilingPlayerYOffsetGlobal;
        float _CeilingYGradientLengthGlobal;
        float _ZoningGlobal;
        float _ZoningModeGlobal;
        float _ZoningEdgeGradientLengthGlobal;
        float _IsZoningRevealableGlobal;
        float _SyncZonesWithFloorYGlobal;
        float _SyncZonesFloorYOffsetGlobal;
        float4 _ObstructionCurveGlobal_TexelSize;
        float4 _DissolveMaskGlobal_TexelSize;
        float _DissolveMaskEnabledGlobal;
        float _PreviewIndicatorLineThicknessGlobal;
    #else
        half _TextureVisibility;
        half _AngleStrength;
        float _Obstruction;
        float _UVs;
        float4 _ObstructionCurve_TexelSize;      
        float _DissolveMaskEnabled;
        float4 _DissolveMask_TexelSize;
        half4 _DissolveColor;
        float _DissolveColorSaturation;
        float _DissolveEmission;
        float _DissolveEmissionBooster;
        float _hasClippedShadows;
        float _ConeStrength;
        float _ConeObstructionDestroyRadius;
        float _CylinderStrength;
        float _CylinderObstructionDestroyRadius;
        float _CircleStrength;
        float _CircleObstructionDestroyRadius;
        float _CurveStrength;
        float _CurveObstructionDestroyRadius;
        float _IntrinsicDissolveStrength;
        float _DissolveFallOff;
        float _PreviewMode;
        float _PreviewIndicatorLineThickness;
        float _AnimationEnabled;
        float _AnimationSpeed;
        float _DefaultEffectRadius;
        float _TransitionDuration;
        float _TexturedEmissionEdge;
        float _TexturedEmissionEdgeStrength;
        float _IsometricExclusion;
        float _IsometricExclusionDistance;
        float _IsometricExclusionGradientLength;
        float _Floor;
        float _FloorMode;
        float _FloorY;
        float _FloorYTextureGradientLength;
        float _PlayerPosYOffset;
        float _Ceiling;
        float _CeilingMode;
        float _CeilingBlendMode;
        float _CeilingY;
        float _CeilingPlayerYOffset;
        float _CeilingYGradientLength;
        float _Zoning;
        float _ZoningMode;
        float _ZoningEdgeGradientLength;
        float _IsZoningRevealable;
        float _SyncZonesWithFloorY;
        float _SyncZonesFloorYOffset;
    #endif



            CBUFFER_END

            

            

            #ifdef unity_WorldToObject
#undef unity_WorldToObject
#endif
#ifdef unity_ObjectToWorld
#undef unity_ObjectToWorld
#endif
#define unity_ObjectToWorld GetObjectToWorldMatrix()
#define unity_WorldToObject GetWorldToObjectMatrix()

    sampler2D _EmissiveColorMap;
    sampler2D _BaseColorMap;
    sampler2D _MaskMap;
    sampler2D _NormalMap;
    sampler2D _NormalMapOS;
    sampler2D _DetailMap;
    sampler2D _TangentMap;
    sampler2D _TangentMapOS;
    sampler2D _AnisotropyMap;
    sampler2D _SubsurfaceMaskMap;
    sampler2D _TransmissionMaskMap;
    sampler2D _ThicknessMap;
    sampler2D _IridescenceThicknessMap;
    sampler2D _IridescenceMaskMap;
    sampler2D _SpecularColorMap;

    sampler2D _CoatMaskMap;

    float3 DecodeNormal (float4 sample, float scale) {
        #if defined(UNITY_NO_DXT5nm)
            return UnpackNormalRGB(sample, scale);
        #else
            return UnpackNormalmapRGorAG(sample, scale);
        #endif
    }

	void Ext_SurfaceFunction0 (inout Surface o, ShaderData d)
	{


//SEE: https://github.com/Unity-Technologies/Graphics/blob/6fdc7996098aa184495c0a3c0da10135bfe18340/Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitDataIndividualLayer.hlsl

        float2 uvBase = _UVMappingMask.x * d.texcoord0 +
                        _UVMappingMask.y * d.texcoord1 +
                        _UVMappingMask.z * d.texcoord2 +
                        _UVMappingMask.w * d.texcoord3;

        float2 uvDetails =  _UVDetailsMappingMask.x * d.texcoord0 +
                            _UVDetailsMappingMask.y * d.texcoord1 +
                            _UVDetailsMappingMask.z * d.texcoord2 +
                            _UVDetailsMappingMask.w * d.texcoord3;


        //TODO: add planar/triplanar

        float4 texcoords;
        //texcoords.xy = d.texcoord0.xy * _BaseColorMap_ST.xy + _BaseColorMap_ST.zw; 
        texcoords.xy = uvBase * _BaseColorMap_ST.xy + _BaseColorMap_ST.zw; 

        texcoords.zw = uvDetails * _DetailMap_ST.xy + _DetailMap_ST.zw;

        if (_LinkDetailsWithBase > 0.0)
        {
            texcoords.zw = texcoords.zw  * _BaseColorMap_ST.xy + _BaseColorMap_ST.zw;

        }

            float3 detailNormalTS = float3(0.0, 0.0, 0.0);
            float detailMask = 0.0;
            #ifdef _DETAIL_MAP
                detailMask = 1.0;
                #ifdef _MASKMAP
                    detailMask = tex2D(_MaskMap, texcoords.xy).b;
                #endif
                float2 detailAlbedoAndSmoothness = tex2D(_DetailMap, texcoords.zw).rb;
                float detailAlbedo = detailAlbedoAndSmoothness.r * 2.0 - 1.0;
                float detailSmoothness = detailAlbedoAndSmoothness.g * 2.0 - 1.0;
                real4 packedNormal = tex2D(_DetailMap, texcoords.zw).rgba;
                detailNormalTS = UnpackNormalAG(packedNormal, _DetailNormalScale);
            #endif
            float4 color = tex2D(_BaseColorMap, texcoords.xy).rgba  * _Color.rgba;
            o.Albedo = color; 
            float alpha = color.a;
            alpha = lerp(_AlphaRemapMin, _AlphaRemapMax, alpha);
            o.Alpha = alpha;
            #ifdef _DETAIL_MAP
                float albedoDetailSpeed = saturate(abs(detailAlbedo) * _DetailAlbedoScale);
                float3 baseColorOverlay = lerp(sqrt(o.Albedo), (detailAlbedo < 0.0) ? float3(0.0, 0.0, 0.0) : float3(1.0, 1.0, 1.0), albedoDetailSpeed * albedoDetailSpeed);
                baseColorOverlay *= baseColorOverlay;
                o.Albedo = lerp(o.Albedo, saturate(baseColorOverlay), detailMask);
            #endif

            #ifdef _NORMALMAP
                float3 normalTS;

                //normalTS = UnpackScaleNormal(tex2D(_NormalMap, texcoords.xy), _NormalScale);
                //#ifdef _DETAIL_MAP
                //    normalTS = lerp(normalTS, BlendNormalRNM(normalTS, normalize(detailNormalTS)), detailMask); 
                //#endif
                //o.Normal = normalTS;


                #ifdef _NORMALMAP_TANGENT_SPACE
                    //normalTS = UnpackScaleNormal(tex2D(_NormalMap, texcoords.xy), _NormalScale);
                    normalTS = DecodeNormal(tex2D(_NormalMap, texcoords.xy), _NormalScale);
                #else
                    #ifdef SURFACE_GRADIENT
                        float3 normalOS = tex2D(_NormalMapOS, texcoords.xy).xyz * 2.0 - 1.0;
                        normalTS = SurfaceGradientFromPerturbedNormal(d.worldSpaceNormal, TransformObjectToWorldNormal(normalOS));
                    #else
                        float3 normalOS = UnpackNormalRGB(tex2D(_NormalMapOS, texcoords.xy), 1.0);
                        float3 bitangent = d.tangentSign * cross(d.worldSpaceNormal.xyz, d.worldSpaceTangent.xyz);
                        half3x3 tangentToWorld = half3x3(d.worldSpaceTangent.xyz, bitangent.xyz, d.worldSpaceNormal.xyz);
                        normalTS = TransformObjectToTangent(normalOS, tangentToWorld);
                    #endif
                #endif

                #ifdef _DETAIL_MAP
                    #ifdef SURFACE_GRADIENT
                    normalTS += detailNormalTS * detailMask;
                    #else
                    normalTS = lerp(normalTS, BlendNormalRNM(normalTS, normalize(detailNormalTS)), detailMask); // todo: detailMask should lerp the angle of the quaternion rotation, not the normals
                    #endif
                #endif
                o.Normal = normalTS;

            #endif

            #if defined(_MASKMAP)
                o.Smoothness = tex2D(_MaskMap, texcoords.xy).a;
                o.Smoothness = lerp(_SmoothnessRemapMin, _SmoothnessRemapMax, o.Smoothness);
            #else
                o.Smoothness = _Smoothness;
            #endif
            #ifdef _DETAIL_MAP
                float smoothnessDetailSpeed = saturate(abs(detailSmoothness) * _DetailSmoothnessScale);
                float smoothnessOverlay = lerp(o.Smoothness, (detailSmoothness < 0.0) ? 0.0 : 1.0, smoothnessDetailSpeed);
                o.Smoothness = lerp(o.Smoothness, saturate(smoothnessOverlay), detailMask);
            #endif
            #ifdef _MASKMAP
                o.Metallic = tex2D(_MaskMap, texcoords.xy).r;
                o.Metallic = lerp(_MetallicRemapMin, _MetallicRemapMax, o.Metallic);
                o.Occlusion = tex2D(_MaskMap, texcoords.xy).g;
                o.Occlusion = lerp(_AORemapMin, _AORemapMax, o.Occlusion);
            #else
                o.Metallic = _Metallic;
                o.Occlusion = 1.0;
            #endif

            #if defined(_SPECULAR_OCCLUSION_FROM_BENT_NORMAL_MAP)
                // TODO: implenent bent normals for this to work
                //o.SpecularOcclusion = GetSpecularOcclusionFromBentAO(V, bentNormalWS, d.worldSpaceNormal, o.Occlusion, PerceptualSmoothnessToRoughness(o.Smoothness));
            #elif  defined(_MASKMAP) && !defined(_SPECULAR_OCCLUSION_NONE)
                float3 V = normalize(d.worldSpaceViewDir);
                o.SpecularOcclusion = GetSpecularOcclusionFromAmbientOcclusion(ClampNdotV(dot(d.worldSpaceNormal, V)), o.Occlusion, PerceptualSmoothnessToRoughness(o.Smoothness));
            #endif

            o.DiffusionProfileHash = asuint(_DiffusionProfileHash);
            o.SubsurfaceMask = _SubsurfaceMask;
            #ifdef _SUBSURFACE_MASK_MAP
                o.SubsurfaceMask *= tex2D(_SubsurfaceMaskMap, texcoords.xy).r;
            #endif
            #ifdef _THICKNESSMAP
                o.Thickness = tex2D(_ThicknessMap, texcoords.xy).r;
                o.Thickness = _ThicknessRemap.x + _ThicknessRemap.y * surfaceData.thickness;
            #else
                o.Thickness = _Thickness;
            #endif
            #ifdef _ANISOTROPYMAP
                o.Anisotropy = tex2D(_AnisotropyMap, texcoords.xy).r;
            #else
                o.Anisotropy = 1.0;
            #endif
                o.Anisotropy *= _Anisotropy;
                o.Specular = _SpecularColor.rgb;
            #ifdef _SPECULARCOLORMAP
                o.Specular *= tex2D(_SpecularColorMap, texcoords.xy).rgb;
            #endif
            #ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
                o.Albedo *= _EnergyConservingSpecularColor > 0.0 ? (1.0 - Max3(o.Specular.r, o.Specular.g, o.Specular.b)) : 1.0;
            #endif
            #ifdef _MATERIAL_FEATURE_CLEAR_COAT
                o.CoatMask = _CoatMask;
                o.CoatMask *= tex2D(_CoatMaskMap, texcoords.xy).r;
            #else
                o.CoatMask = 0.0;
            #endif
            #ifdef _MATERIAL_FEATURE_IRIDESCENCE
                #ifdef _IRIDESCENCE_THICKNESSMAP
                o.IridescenceThickness = tex2D(_IridescenceThicknessMap, texcoords.xy).r;
                o.IridescenceThickness = _IridescenceThicknessRemap.x + _IridescenceThicknessRemap.y * o.IridescenceThickness;
                #else
                o.IridescenceThickness = _IridescenceThickness;
                #endif
                o.IridescenceMask = _IridescenceMask;
                o.IridescenceMask *= tex2D(_IridescenceMaskMap, texcoords.xy).r;
            #else
                o.IridescenceThickness = 0.0;
                o.IridescenceMask = 0.0;
            #endif


//SEE: https://github.com/Unity-Technologies/Graphics/blob/632f80e011f18ea537ee6e2f0be3ff4f4dea6a11/Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitBuiltinData.hlsl

            float3 emissiveColor = _EmissiveColor * lerp(float3(1.0, 1.0, 1.0), o.Albedo.rgb, _AlbedoAffectEmissive);
            #ifdef _EMISSIVE_COLOR_MAP                
                float2 uvEmission = _UVMappingMaskEmissive.x * d.texcoord0 +
                                    _UVMappingMaskEmissive.y * d.texcoord1 +
                                    _UVMappingMaskEmissive.z * d.texcoord2 +
                                    _UVMappingMaskEmissive.w * d.texcoord3;

                uvEmission = uvEmission * _EmissiveColorMap_ST.xy + _EmissiveColorMap_ST.zw;                     

                emissiveColor *= tex2D(_EmissiveColorMap, uvEmission).rgb;
            #endif
            o.Emission += emissiveColor;
            float currentExposureMultiplier = 0;
            #if SHADEROPTIONS_PRE_EXPOSITION
                currentExposureMultiplier =  LOAD_TEXTURE2D(_ExposureTexture, int2(0, 0)).x * _ProbeExposureScale;
            #else
                currentExposureMultiplier =  _ProbeExposureScale;
            #endif
            float InverseCurrentExposureMultiplier = 0;
            float exposure = currentExposureMultiplier;
            InverseCurrentExposureMultiplier =  rcp(exposure + (exposure == 0.0)); 
            float3 emissiveRcpExposure = o.Emission * InverseCurrentExposureMultiplier;
            o.Emission = lerp(emissiveRcpExposure, o.Emission, _EmissiveExposureWeight);


// SEE: https://github.com/Unity-Technologies/Graphics/blob/223d8105e68c5a808027d78f39cb4e2545fd6276/Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitData.hlsl
            #if defined(_ALPHATEST_ON)
                float alphaTex = tex2D(_BaseColorMap, texcoords.xy).a;
                alphaTex = lerp(_AlphaRemapMin, _AlphaRemapMax, alphaTex);
                float alphaValue = alphaTex * _BaseColor.a;

                // Perform alha test very early to save performance (a killed pixel will not sample textures)
                //#if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
                //float alphaCutoff = _AlphaCutoffPrepass;
                //#elif SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_POSTPASS
                //float alphaCutoff = _AlphaCutoffPostpass;
                //#elif (SHADERPASS == SHADERPASS_SHADOWS) || (SHADERPASS == SHADERPASS_RAYTRACING_VISIBILITY)
                //float alphaCutoff = _UseShadowThreshold ? _AlphaCutoffShadow : _AlphaCutoff;
                //#else
                float alphaCutoff = _AlphaCutoff;
                //#endif

                // GENERIC_ALPHA_TEST(alphaValue, alphaCutoff);
                clip(alpha - alphaCutoff);
            #endif

	}




    #if _REPLACEMENT
        sampler2D _DissolveTexGlobal;
    #else
        sampler2D _DissolveTex;
    #endif

    //#if _DISSOLVEMASK
    #if _REPLACEMENT
        sampler2D _DissolveMaskGlobal;
    #else
        sampler2D _DissolveMask;
    #endif
    //#endif

    #if _REPLACEMENT
        sampler2D _ObstructionCurveGlobal;
    #else
        sampler2D _ObstructionCurve;
    #endif



    #if _URP
        half2 ParallaxOffset1Step (half h, half height, half3 viewDir)
        {
            h = h * height - height/2.0;
            half3 v = normalize(viewDir);
            v.z += 0.42;
            return h * (v.xy / v.z);
        }
        #ifdef UNITY_COLORSPACE_GAMMA
            #define unity_ColorSpaceDouble half4(2.0, 2.0, 2.0, 2.0)
        #else 
            #define unity_ColorSpaceDouble half4(4.59479380, 4.59479380, 4.59479380, 2.0)
        #endif
        half3 BlendNormals(half3 n1, half3 n2)
        {
            return normalize(half3(n1.xy + n2.xy, n1.z*n2.z));
        }
    #endif


#ifdef USE_UNITY_TEXTURE_2D_TYPE
#undef USE_UNITY_TEXTURE_2D_TYPE
#endif
#include "Assets/See-through Shader/Core/Shaders/xxSharedSTSDependencies/SeeThroughShaderFunction.hlsl"



	void Ext_SurfaceFunction1 (inout Surface o, ShaderData d)
	{
        half3 albedo;
        half3 emission;
        float alphaForClipping;
#ifdef USE_UNITY_TEXTURE_2D_TYPE
#undef USE_UNITY_TEXTURE_2D_TYPE
#endif
#if _REPLACEMENT
    DoSeeThroughShading( o.Albedo, o.Normal, d.worldSpacePosition, d.worldSpaceNormal,

                        _numOfPlayersInside, _tDirection, _tValue, _id,
                        _TriggerMode, _RaycastMode,
                        _IsExempt,

                        _DissolveColorGlobal, _DissolveColorSaturationGlobal, _UVsGlobal,
                        _DissolveEmissionGlobal, _DissolveEmissionBoosterGlobal, _TexturedEmissionEdgeGlobal, _TexturedEmissionEdgeStrengthGlobal,
                        _hasClippedShadowsGlobal,

                        _ObstructionGlobal,
                        _AngleStrengthGlobal,
                        _ConeStrengthGlobal, _ConeObstructionDestroyRadiusGlobal,
                        _CylinderStrengthGlobal, _CylinderObstructionDestroyRadiusGlobal,
                        _CircleStrengthGlobal, _CircleObstructionDestroyRadiusGlobal,
                        _CurveStrengthGlobal, _CurveObstructionDestroyRadiusGlobal,
                        _DissolveFallOffGlobal,
                        _DissolveMaskEnabledGlobal,
                        _IntrinsicDissolveStrengthGlobal,
                        _DefaultEffectRadiusGlobal,

                        _IsometricExclusionGlobal, _IsometricExclusionDistanceGlobal, _IsometricExclusionGradientLengthGlobal,
                        _CeilingGlobal, _CeilingModeGlobal, _CeilingBlendModeGlobal, _CeilingYGlobal, _CeilingPlayerYOffsetGlobal, _CeilingYGradientLengthGlobal,
                        _FloorGlobal, _FloorModeGlobal, _FloorYGlobal, _PlayerPosYOffsetGlobal, _FloorYTextureGradientLengthGlobal,

                        _AnimationEnabledGlobal, _AnimationSpeedGlobal,
                        _TransitionDurationGlobal,

                        _ZoningGlobal, _ZoningModeGlobal, _IsZoningRevealableGlobal, _ZoningEdgeGradientLengthGlobal,
                        _SyncZonesWithFloorYGlobal, _SyncZonesFloorYOffsetGlobal,

                        _PreviewModeGlobal,
                        _PreviewIndicatorLineThicknessGlobal,

                        _DissolveTexGlobal,
                        _DissolveMaskGlobal,
                        _ObstructionCurveGlobal,

                        _DissolveMaskGlobal_TexelSize,
                        _ObstructionCurveGlobal_TexelSize,

                        albedo, emission, alphaForClipping);
#else 
    
    DoSeeThroughShading( o.Albedo, o.Normal, d.worldSpacePosition, d.worldSpaceNormal,

                        _numOfPlayersInside, _tDirection, _tValue, _id,
                        _TriggerMode, _RaycastMode,
                        _IsExempt,

                        _DissolveColor, _DissolveColorSaturation, _UVs,
                        _DissolveEmission, _DissolveEmissionBooster, _TexturedEmissionEdge, _TexturedEmissionEdgeStrength,
                        _hasClippedShadows,

                        _Obstruction,
                        _AngleStrength,
                        _ConeStrength, _ConeObstructionDestroyRadius,
                        _CylinderStrength, _CylinderObstructionDestroyRadius,
                        _CircleStrength, _CircleObstructionDestroyRadius,
                        _CurveStrength, _CurveObstructionDestroyRadius,
                        _DissolveFallOff,
                        _DissolveMaskEnabled,
                        _IntrinsicDissolveStrength,
                        _DefaultEffectRadius,

                        _IsometricExclusion, _IsometricExclusionDistance, _IsometricExclusionGradientLength,
                        _Ceiling, _CeilingMode, _CeilingBlendMode, _CeilingY, _CeilingPlayerYOffset, _CeilingYGradientLength,
                        _Floor, _FloorMode, _FloorY, _PlayerPosYOffset, _FloorYTextureGradientLength,

                        _AnimationEnabled, _AnimationSpeed,
                        _TransitionDuration,

                        _Zoning, _ZoningMode, _IsZoningRevealable, _ZoningEdgeGradientLength,
                        _SyncZonesWithFloorY, _SyncZonesFloorYOffset,

                        _PreviewMode,
                        _PreviewIndicatorLineThickness,                            

                        _DissolveTex,
                        _DissolveMask,
                        _ObstructionCurve,

                        _DissolveMask_TexelSize,
                        _ObstructionCurve_TexelSize,

                        albedo, emission, alphaForClipping);
#endif 



        o.Albedo = albedo;
        o.Emission += emission;   

	}




        
            void ChainSurfaceFunction(inout Surface l, inout ShaderData d)
            {
                  Ext_SurfaceFunction0(l, d);
                  Ext_SurfaceFunction1(l, d);
                 // Ext_SurfaceFunction2(l, d);
                 // Ext_SurfaceFunction3(l, d);
                 // Ext_SurfaceFunction4(l, d);
                 // Ext_SurfaceFunction5(l, d);
                 // Ext_SurfaceFunction6(l, d);
                 // Ext_SurfaceFunction7(l, d);
                 // Ext_SurfaceFunction8(l, d);
                 // Ext_SurfaceFunction9(l, d);
		           // Ext_SurfaceFunction10(l, d);
                 // Ext_SurfaceFunction11(l, d);
                 // Ext_SurfaceFunction12(l, d);
                 // Ext_SurfaceFunction13(l, d);
                 // Ext_SurfaceFunction14(l, d);
                 // Ext_SurfaceFunction15(l, d);
                 // Ext_SurfaceFunction16(l, d);
                 // Ext_SurfaceFunction17(l, d);
                 // Ext_SurfaceFunction18(l, d);
		           // Ext_SurfaceFunction19(l, d);
                 // Ext_SurfaceFunction20(l, d);
                 // Ext_SurfaceFunction21(l, d);
                 // Ext_SurfaceFunction22(l, d);
                 // Ext_SurfaceFunction23(l, d);
                 // Ext_SurfaceFunction24(l, d);
                 // Ext_SurfaceFunction25(l, d);
                 // Ext_SurfaceFunction26(l, d);
                 // Ext_SurfaceFunction27(l, d);
                 // Ext_SurfaceFunction28(l, d);
		           // Ext_SurfaceFunction29(l, d);
            }

            void ChainModifyVertex(inout VertexData v, inout VertexToPixel v2p, float4 time)
            {
                 ExtraV2F d;
                 
                 ZERO_INITIALIZE(ExtraV2F, d);
                 ZERO_INITIALIZE(Blackboard, d.blackboard);
                 // due to motion vectors in HDRP, we need to use the last
                 // time in certain spots. So if you are going to use _Time to adjust vertices,
                 // you need to use this time or motion vectors will break. 
                 d.time = time;

                 //  Ext_ModifyVertex0(v, d);
                 // Ext_ModifyVertex1(v, d);
                 // Ext_ModifyVertex2(v, d);
                 // Ext_ModifyVertex3(v, d);
                 // Ext_ModifyVertex4(v, d);
                 // Ext_ModifyVertex5(v, d);
                 // Ext_ModifyVertex6(v, d);
                 // Ext_ModifyVertex7(v, d);
                 // Ext_ModifyVertex8(v, d);
                 // Ext_ModifyVertex9(v, d);
                 // Ext_ModifyVertex10(v, d);
                 // Ext_ModifyVertex11(v, d);
                 // Ext_ModifyVertex12(v, d);
                 // Ext_ModifyVertex13(v, d);
                 // Ext_ModifyVertex14(v, d);
                 // Ext_ModifyVertex15(v, d);
                 // Ext_ModifyVertex16(v, d);
                 // Ext_ModifyVertex17(v, d);
                 // Ext_ModifyVertex18(v, d);
                 // Ext_ModifyVertex19(v, d);
                 // Ext_ModifyVertex20(v, d);
                 // Ext_ModifyVertex21(v, d);
                 // Ext_ModifyVertex22(v, d);
                 // Ext_ModifyVertex23(v, d);
                 // Ext_ModifyVertex24(v, d);
                 // Ext_ModifyVertex25(v, d);
                 // Ext_ModifyVertex26(v, d);
                 // Ext_ModifyVertex27(v, d);
                 // Ext_ModifyVertex28(v, d);
                 // Ext_ModifyVertex29(v, d);


                 // #if %EXTRAV2F0REQUIREKEY%
                 // v2p.extraV2F0 = d.extraV2F0;
                 // #endif

                 // #if %EXTRAV2F1REQUIREKEY%
                 // v2p.extraV2F1 = d.extraV2F1;
                 // #endif

                 // #if %EXTRAV2F2REQUIREKEY%
                 // v2p.extraV2F2 = d.extraV2F2;
                 // #endif

                 // #if %EXTRAV2F3REQUIREKEY%
                 // v2p.extraV2F3 = d.extraV2F3;
                 // #endif

                 // #if %EXTRAV2F4REQUIREKEY%
                 // v2p.extraV2F4 = d.extraV2F4;
                 // #endif

                 // #if %EXTRAV2F5REQUIREKEY%
                 // v2p.extraV2F5 = d.extraV2F5;
                 // #endif

                 // #if %EXTRAV2F6REQUIREKEY%
                 // v2p.extraV2F6 = d.extraV2F6;
                 // #endif

                 // #if %EXTRAV2F7REQUIREKEY%
                 // v2p.extraV2F7 = d.extraV2F7;
                 // #endif
            }

            void ChainModifyTessellatedVertex(inout VertexData v, inout VertexToPixel v2p)
            {
               ExtraV2F d;
               ZERO_INITIALIZE(ExtraV2F, d);
               ZERO_INITIALIZE(Blackboard, d.blackboard);

               // #if %EXTRAV2F0REQUIREKEY%
               // d.extraV2F0 = v2p.extraV2F0;
               // #endif

               // #if %EXTRAV2F1REQUIREKEY%
               // d.extraV2F1 = v2p.extraV2F1;
               // #endif

               // #if %EXTRAV2F2REQUIREKEY%
               // d.extraV2F2 = v2p.extraV2F2;
               // #endif

               // #if %EXTRAV2F3REQUIREKEY%
               // d.extraV2F3 = v2p.extraV2F3;
               // #endif

               // #if %EXTRAV2F4REQUIREKEY%
               // d.extraV2F4 = v2p.extraV2F4;
               // #endif

               // #if %EXTRAV2F5REQUIREKEY%
               // d.extraV2F5 = v2p.extraV2F5;
               // #endif

               // #if %EXTRAV2F6REQUIREKEY%
               // d.extraV2F6 = v2p.extraV2F6;
               // #endif

               // #if %EXTRAV2F7REQUIREKEY%
               // d.extraV2F7 = v2p.extraV2F7;
               // #endif


               // Ext_ModifyTessellatedVertex0(v, d);
               // Ext_ModifyTessellatedVertex1(v, d);
               // Ext_ModifyTessellatedVertex2(v, d);
               // Ext_ModifyTessellatedVertex3(v, d);
               // Ext_ModifyTessellatedVertex4(v, d);
               // Ext_ModifyTessellatedVertex5(v, d);
               // Ext_ModifyTessellatedVertex6(v, d);
               // Ext_ModifyTessellatedVertex7(v, d);
               // Ext_ModifyTessellatedVertex8(v, d);
               // Ext_ModifyTessellatedVertex9(v, d);
               // Ext_ModifyTessellatedVertex10(v, d);
               // Ext_ModifyTessellatedVertex11(v, d);
               // Ext_ModifyTessellatedVertex12(v, d);
               // Ext_ModifyTessellatedVertex13(v, d);
               // Ext_ModifyTessellatedVertex14(v, d);
               // Ext_ModifyTessellatedVertex15(v, d);
               // Ext_ModifyTessellatedVertex16(v, d);
               // Ext_ModifyTessellatedVertex17(v, d);
               // Ext_ModifyTessellatedVertex18(v, d);
               // Ext_ModifyTessellatedVertex19(v, d);
               // Ext_ModifyTessellatedVertex20(v, d);
               // Ext_ModifyTessellatedVertex21(v, d);
               // Ext_ModifyTessellatedVertex22(v, d);
               // Ext_ModifyTessellatedVertex23(v, d);
               // Ext_ModifyTessellatedVertex24(v, d);
               // Ext_ModifyTessellatedVertex25(v, d);
               // Ext_ModifyTessellatedVertex26(v, d);
               // Ext_ModifyTessellatedVertex27(v, d);
               // Ext_ModifyTessellatedVertex28(v, d);
               // Ext_ModifyTessellatedVertex29(v, d);

               // #if %EXTRAV2F0REQUIREKEY%
               // v2p.extraV2F0 = d.extraV2F0;
               // #endif

               // #if %EXTRAV2F1REQUIREKEY%
               // v2p.extraV2F1 = d.extraV2F1;
               // #endif

               // #if %EXTRAV2F2REQUIREKEY%
               // v2p.extraV2F2 = d.extraV2F2;
               // #endif

               // #if %EXTRAV2F3REQUIREKEY%
               // v2p.extraV2F3 = d.extraV2F3;
               // #endif

               // #if %EXTRAV2F4REQUIREKEY%
               // v2p.extraV2F4 = d.extraV2F4;
               // #endif

               // #if %EXTRAV2F5REQUIREKEY%
               // v2p.extraV2F5 = d.extraV2F5;
               // #endif

               // #if %EXTRAV2F6REQUIREKEY%
               // v2p.extraV2F6 = d.extraV2F6;
               // #endif

               // #if %EXTRAV2F7REQUIREKEY%
               // v2p.extraV2F7 = d.extraV2F7;
               // #endif
            }

            void ChainFinalColorForward(inout Surface l, inout ShaderData d, inout half4 color)
            {
               //   Ext_FinalColorForward0(l, d, color);
               //   Ext_FinalColorForward1(l, d, color);
               //   Ext_FinalColorForward2(l, d, color);
               //   Ext_FinalColorForward3(l, d, color);
               //   Ext_FinalColorForward4(l, d, color);
               //   Ext_FinalColorForward5(l, d, color);
               //   Ext_FinalColorForward6(l, d, color);
               //   Ext_FinalColorForward7(l, d, color);
               //   Ext_FinalColorForward8(l, d, color);
               //   Ext_FinalColorForward9(l, d, color);
               //  Ext_FinalColorForward10(l, d, color);
               //  Ext_FinalColorForward11(l, d, color);
               //  Ext_FinalColorForward12(l, d, color);
               //  Ext_FinalColorForward13(l, d, color);
               //  Ext_FinalColorForward14(l, d, color);
               //  Ext_FinalColorForward15(l, d, color);
               //  Ext_FinalColorForward16(l, d, color);
               //  Ext_FinalColorForward17(l, d, color);
               //  Ext_FinalColorForward18(l, d, color);
               //  Ext_FinalColorForward19(l, d, color);
               //  Ext_FinalColorForward20(l, d, color);
               //  Ext_FinalColorForward21(l, d, color);
               //  Ext_FinalColorForward22(l, d, color);
               //  Ext_FinalColorForward23(l, d, color);
               //  Ext_FinalColorForward24(l, d, color);
               //  Ext_FinalColorForward25(l, d, color);
               //  Ext_FinalColorForward26(l, d, color);
               //  Ext_FinalColorForward27(l, d, color);
               //  Ext_FinalColorForward28(l, d, color);
               //  Ext_FinalColorForward29(l, d, color);
            }

            void ChainFinalGBufferStandard(inout Surface s, inout ShaderData d, inout half4 GBuffer0, inout half4 GBuffer1, inout half4 GBuffer2, inout half4 outEmission, inout half4 outShadowMask)
            {
               //   Ext_FinalGBufferStandard0(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard1(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard2(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard3(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard4(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard5(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard6(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard7(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard8(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard9(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard10(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard11(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard12(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard13(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard14(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard15(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard16(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard17(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard18(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard19(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard20(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard21(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard22(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard23(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard24(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard25(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard26(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard27(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard28(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard29(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
            }



            

         ShaderData CreateShaderData(VertexToPixel i
                  #if NEED_FACING
                     , bool facing
                  #endif
         )
         {
            ShaderData d = (ShaderData)0;
            d.clipPos = i.pos;
            d.worldSpacePosition = i.worldPos;

            d.worldSpaceNormal = normalize(i.worldNormal);
            d.worldSpaceTangent = normalize(i.worldTangent.xyz);
            d.tangentSign = i.worldTangent.w;
            float3 bitangent = cross(i.worldTangent.xyz, i.worldNormal) * d.tangentSign * -1;
            

            d.TBNMatrix = float3x3(d.worldSpaceTangent, bitangent, d.worldSpaceNormal);
            d.worldSpaceViewDir = normalize(_WorldSpaceCameraPos - i.worldPos);

            d.tangentSpaceViewDir = mul(d.TBNMatrix, d.worldSpaceViewDir);
             d.texcoord0 = i.texcoord0;
             d.texcoord1 = i.texcoord1;
             d.texcoord2 = i.texcoord2;

            // #if %TEXCOORD3REQUIREKEY%
             d.texcoord3 = i.texcoord3;
            // #endif

            // d.isFrontFace = facing;
            // #if %VERTEXCOLORREQUIREKEY%
            // d.vertexColor = i.vertexColor;
            // #endif

            // these rarely get used, so we back transform them. Usually will be stripped.
            #if _HDRP
                // d.localSpacePosition = mul(unity_WorldToObject, float4(GetCameraRelativePositionWS(i.worldPos), 1)).xyz;
            #else
                // d.localSpacePosition = mul(unity_WorldToObject, float4(i.worldPos, 1)).xyz;
            #endif
            // d.localSpaceNormal = normalize(mul((float3x3)unity_WorldToObject, i.worldNormal));
            // d.localSpaceTangent = normalize(mul((float3x3)unity_WorldToObject, i.worldTangent.xyz));

            // #if %SCREENPOSREQUIREKEY%
            // d.screenPos = i.screenPos;
            // d.screenUV = (i.screenPos.xy / i.screenPos.w);
            // #endif


            // #if %EXTRAV2F0REQUIREKEY%
            // d.extraV2F0 = i.extraV2F0;
            // #endif

            // #if %EXTRAV2F1REQUIREKEY%
            // d.extraV2F1 = i.extraV2F1;
            // #endif

            // #if %EXTRAV2F2REQUIREKEY%
            // d.extraV2F2 = i.extraV2F2;
            // #endif

            // #if %EXTRAV2F3REQUIREKEY%
            // d.extraV2F3 = i.extraV2F3;
            // #endif

            // #if %EXTRAV2F4REQUIREKEY%
            // d.extraV2F4 = i.extraV2F4;
            // #endif

            // #if %EXTRAV2F5REQUIREKEY%
            // d.extraV2F5 = i.extraV2F5;
            // #endif

            // #if %EXTRAV2F6REQUIREKEY%
            // d.extraV2F6 = i.extraV2F6;
            // #endif

            // #if %EXTRAV2F7REQUIREKEY%
            // d.extraV2F7 = i.extraV2F7;
            // #endif

            return d;
         }
         

            

struct VaryingsToPS
{
   VertexToPixel vmesh;
   #ifdef VARYINGS_NEED_PASS
      VaryingsPassToPS vpass;
   #endif
};

struct PackedVaryingsToPS
{
   #ifdef VARYINGS_NEED_PASS
      PackedVaryingsPassToPS vpass;
   #endif
   VertexToPixel vmesh;

   UNITY_VERTEX_OUTPUT_STEREO
};

PackedVaryingsToPS PackVaryingsToPS(VaryingsToPS input)
{
   PackedVaryingsToPS output = (PackedVaryingsToPS)0;
   output.vmesh = input.vmesh;
   #ifdef VARYINGS_NEED_PASS
      output.vpass = PackVaryingsPassToPS(input.vpass);
   #endif

   UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
   return output;
}




VertexToPixel VertMesh(VertexData input)
{
    VertexToPixel output = (VertexToPixel)0;

    UNITY_SETUP_INSTANCE_ID(input);
    UNITY_TRANSFER_INSTANCE_ID(input, output);

    
    ChainModifyVertex(input, output, _Time);


    // This return the camera relative position (if enable)
    float3 positionRWS = TransformObjectToWorld(input.vertex.xyz);
    float3 normalWS = TransformObjectToWorldNormal(input.normal);
    float4 tangentWS = float4(TransformObjectToWorldDir(input.tangent.xyz), input.tangent.w);


    output.worldPos = GetAbsolutePositionWS(positionRWS);
    output.pos = TransformWorldToHClip(positionRWS);
    output.worldNormal = normalWS;
    output.worldTangent = tangentWS;


    output.texcoord0 = input.texcoord0;
    output.texcoord1 = input.texcoord1;
    output.texcoord2 = input.texcoord2;
    // #if %TEXCOORD3REQUIREKEY%
     output.texcoord3 = input.texcoord3;
    // #endif

    // #if %SCREENPOSREQUIREKEY%
    // output.screenPos = ComputeScreenPos(output.pos, _ProjectionParams.x);
    // #endif

    // #if %VERTEXCOLORREQUIREKEY%
    // output.vertexColor = input.vertexColor;
    // #endif
    return output;
}


#if (SHADERPASS == SHADERPASS_DBUFFER_MESH)
void MeshDecalsPositionZBias(inout VaryingsToPS input)
{
#if defined(UNITY_REVERSED_Z)
    input.vmesh.pos.z -= _DecalMeshDepthBias;
#else
    input.vmesh.pos.z += _DecalMeshDepthBias;
#endif
}
#endif


#if (SHADERPASS == SHADERPASS_LIGHT_TRANSPORT)

// This was not in constant buffer in original unity, so keep outiside. But should be in as ShaderRenderPass frequency
float unity_OneOverOutputBoost;
float unity_MaxOutputValue;

CBUFFER_START(UnityMetaPass)
// x = use uv1 as raster position
// y = use uv2 as raster position
bool4 unity_MetaVertexControl;

// x = return albedo
// y = return normal
bool4 unity_MetaFragmentControl;
CBUFFER_END

PackedVaryingsToPS Vert(VertexData inputMesh)
{
    VaryingsToPS output = (VaryingsToPS)0;
    output.vmesh = (VertexToPixel)0;

    UNITY_SETUP_INSTANCE_ID(inputMesh);
    UNITY_TRANSFER_INSTANCE_ID(inputMesh, output.vmesh);

    // Output UV coordinate in vertex shader
    float2 uv = float2(0.0, 0.0);

    if (unity_MetaVertexControl.x)
    {
        uv = inputMesh.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
    }
    else if (unity_MetaVertexControl.y)
    {
        uv = inputMesh.texcoord2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
    }

    // OpenGL right now needs to actually use the incoming vertex position
    // so we create a fake dependency on it here that haven't any impact.
    output.vmesh.pos = float4(uv * 2.0 - 1.0, inputMesh.vertex.z > 0 ? 1.0e-4 : 0.0, 1.0);

#ifdef VARYINGS_NEED_POSITION_WS
    output.vmesh.worldPos = TransformObjectToWorld(inputMesh.vertex.xyz);
#endif

#ifdef VARYINGS_NEED_TANGENT_TO_WORLD
    // Normal is required for triplanar mapping
    output.vmesh.worldNormal = TransformObjectToWorldNormal(inputMesh.normal);
    // Not required but assign to silent compiler warning
    output.vmesh.worldTangent = float4(1.0, 0.0, 0.0, 0.0);
#endif

    output.vmesh.texcoord0 = inputMesh.texcoord0;
    output.vmesh.texcoord1 = inputMesh.texcoord1;
    output.vmesh.texcoord2 = inputMesh.texcoord2;
    // #if %TEXCOORD3REQUIREKEY%
     output.vmesh.texcoord3 = inputMesh.texcoord3;
    // #endif

    // #if %VERTEXCOLORREQUIREKEY%
    // output.vmesh.vertexColor = inputMesh.vertexColor;
    // #endif

    return PackVaryingsToPS(output);
}
#else

PackedVaryingsToPS Vert(VertexData inputMesh)
{
    VaryingsToPS varyingsType;
    varyingsType.vmesh = VertMesh(inputMesh);
    #if (SHADERPASS == SHADERPASS_DBUFFER_MESH)
       MeshDecalsPositionZBias(varyingsType);
    #endif
    return PackVaryingsToPS(varyingsType);
}

#endif



            

            
                FragInputs BuildFragInputs(VertexToPixel input)
                {
                    UNITY_SETUP_INSTANCE_ID(input);
                    FragInputs output;
                    ZERO_INITIALIZE(FragInputs, output);
            
                    // Init to some default value to make the computer quiet (else it output 'divide by zero' warning even if value is not used).
                    // TODO: this is a really poor workaround, but the variable is used in a bunch of places
                    // to compute normals which are then passed on elsewhere to compute other values...
                    output.tangentToWorld = k_identity3x3;
                    output.positionSS = input.pos;       // input.positionCS is SV_Position
            
                    output.positionRWS = GetCameraRelativePositionWS(input.worldPos);
                    output.tangentToWorld = BuildTangentToWorld(input.worldTangent, input.worldNormal);
                    output.texCoord0 = input.texcoord0;
                    output.texCoord1 = input.texcoord1;
                    output.texCoord2 = input.texcoord2;
            
                    return output;
                }
            
               void BuildSurfaceData(FragInputs fragInputs, inout Surface surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData, out float3 bentNormalWS)
               {
                   // setup defaults -- these are used if the graph doesn't output a value
                   ZERO_INITIALIZE(SurfaceData, surfaceData);
        
                   // specularOcclusion need to be init ahead of decal to quiet the compiler that modify the SurfaceData struct
                   // however specularOcclusion can come from the graph, so need to be init here so it can be override.
                   surfaceData.specularOcclusion = 1.0;
        
                   // copy across graph values, if defined
                   surfaceData.baseColor =                 surfaceDescription.Albedo;
                   surfaceData.perceptualSmoothness =      surfaceDescription.Smoothness;
                   surfaceData.ambientOcclusion =          surfaceDescription.Occlusion;
                   surfaceData.specularOcclusion =         surfaceDescription.SpecularOcclusion;
                   surfaceData.metallic =                  surfaceDescription.Metallic;
                   surfaceData.subsurfaceMask =            surfaceDescription.SubsurfaceMask;
                   surfaceData.thickness =                 surfaceDescription.Thickness;
                   surfaceData.diffusionProfileHash =      asuint(surfaceDescription.DiffusionProfileHash);
                   #if _USESPECULAR
                      surfaceData.specularColor =             surfaceDescription.Specular;
                   #endif
                   surfaceData.coatMask =                  surfaceDescription.CoatMask;
                   surfaceData.anisotropy =                surfaceDescription.Anisotropy;
                   surfaceData.iridescenceMask =           surfaceDescription.IridescenceMask;
                   surfaceData.iridescenceThickness =      surfaceDescription.IridescenceThickness;
        
           #ifdef _HAS_REFRACTION
                   if (_EnableSSRefraction)
                   {
                       // surfaceData.ior =                       surfaceDescription.RefractionIndex;
                       // surfaceData.transmittanceColor =        surfaceDescription.RefractionColor;
                       // surfaceData.atDistance =                surfaceDescription.RefractionDistance;
        
                       surfaceData.transmittanceMask = (1.0 - surfaceDescription.Alpha);
                       surfaceDescription.Alpha = 1.0;
                   }
                   else
                   {
                       surfaceData.ior = surfaceDescription.ior;
                       surfaceData.transmittanceColor = surfaceDescription.transmittanceColor;
                       surfaceData.atDistance = surfaceDescription.atDistance;
                       surfaceData.transmittanceMask = surfaceDescription.transmittanceMask;
                       surfaceDescription.Alpha = 1.0;
                   }
           #else
                   surfaceData.ior = 1.0;
                   surfaceData.transmittanceColor = float3(1.0, 1.0, 1.0);
                   surfaceData.atDistance = 1.0;
                   surfaceData.transmittanceMask = 0.0;
           #endif
                
                   // These static material feature allow compile time optimization
                   surfaceData.materialFeatures = MATERIALFEATUREFLAGS_LIT_STANDARD;
           #ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
                   surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SUBSURFACE_SCATTERING;
           #endif
           #ifdef _MATERIAL_FEATURE_TRANSMISSION
                   surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_TRANSMISSION;
           #endif
           #ifdef _MATERIAL_FEATURE_ANISOTROPY
                   surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_ANISOTROPY;
           #endif
                   // surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_CLEAR_COAT;
        
           #ifdef _MATERIAL_FEATURE_IRIDESCENCE
                   surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_IRIDESCENCE;
           #endif
           #ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
                   surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SPECULAR_COLOR;
           #endif
        
           #if defined (_MATERIAL_FEATURE_SPECULAR_COLOR) && defined (_ENERGY_CONSERVING_SPECULAR)
                   // Require to have setup baseColor
                   // Reproduce the energy conservation done in legacy Unity. Not ideal but better for compatibility and users can unchek it
                   surfaceData.baseColor *= (1.0 - Max3(surfaceData.specularColor.r, surfaceData.specularColor.g, surfaceData.specularColor.b));
           #endif

        
                   // tangent-space normal
                   float3 normalTS = float3(0.0f, 0.0f, 1.0f);
                   normalTS = surfaceDescription.Normal;
        
                   // compute world space normal
                   #if !_WORLDSPACENORMAL
                      surfaceData.normalWS = mul(normalTS, fragInputs.tangentToWorld);
                   #else
                      surfaceData.normalWS = normalTS;  
                   #endif
                   surfaceData.geomNormalWS = fragInputs.tangentToWorld[2];
        
                   surfaceData.tangentWS = normalize(fragInputs.tangentToWorld[0].xyz);    // The tangent is not normalize in tangentToWorld for mikkt. TODO: Check if it expected that we normalize with Morten. Tag: SURFACE_GRADIENT
                   // surfaceData.tangentWS = TransformTangentToWorld(surfaceDescription.Tangent, fragInputs.tangentToWorld);
        
           #if HAVE_DECALS
                   if (_EnableDecals)
                   {
                       #if VERSION_GREATER_EQUAL(10,2)
                          DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput,  surfaceData.geomNormalWS, surfaceDescription.Alpha);
                          ApplyDecalToSurfaceData(decalSurfaceData,  surfaceData.geomNormalWS, surfaceData);
                       #else
                          DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, surfaceDescription.Alpha);
                          ApplyDecalToSurfaceData(decalSurfaceData, surfaceData);
                       #endif
                   }
           #endif
        
                   bentNormalWS = surfaceData.normalWS;
               
                   surfaceData.tangentWS = Orthonormalize(surfaceData.tangentWS, surfaceData.normalWS);
        
        
                   // By default we use the ambient occlusion with Tri-ace trick (apply outside) for specular occlusion.
                   // If user provide bent normal then we process a better term
           #if defined(_SPECULAR_OCCLUSION_CUSTOM)
                   // Just use the value passed through via the slot (not active otherwise)
           #elif defined(_SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL)
                   // If we have bent normal and ambient occlusion, process a specular occlusion
                   surfaceData.specularOcclusion = GetSpecularOcclusionFromBentAO(V, bentNormalWS, surfaceData.normalWS, surfaceData.ambientOcclusion, PerceptualSmoothnessToPerceptualRoughness(surfaceData.perceptualSmoothness));
           #elif defined(_AMBIENT_OCCLUSION) && defined(_SPECULAR_OCCLUSION_FROM_AO)
                   surfaceData.specularOcclusion = GetSpecularOcclusionFromAmbientOcclusion(ClampNdotV(dot(surfaceData.normalWS, V)), surfaceData.ambientOcclusion, PerceptualSmoothnessToRoughness(surfaceData.perceptualSmoothness));
           #endif
        
           #ifdef _ENABLE_GEOMETRIC_SPECULAR_AA
                   surfaceData.perceptualSmoothness = GeometricNormalFiltering(surfaceData.perceptualSmoothness, fragInputs.tangentToWorld[2], surfaceDescription.SpecularAAScreenSpaceVariance, surfaceDescription.SpecularAAThreshold);
           #endif
        
           #ifdef DEBUG_DISPLAY
                   if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
                   {
                       // TODO: need to update mip info
                       surfaceData.metallic = 0;
                   }
        
                   // We need to call ApplyDebugToSurfaceData after filling the surfarcedata and before filling builtinData
                   // as it can modify attribute use for static lighting
                   ApplyDebugToSurfaceData(fragInputs.tangentToWorld, surfaceData);
           #endif
               }
        
               void GetSurfaceAndBuiltinData(VertexToPixel m2ps, FragInputs fragInputs, float3 V, inout PositionInputs posInput,
                     out SurfaceData surfaceData, out BuiltinData builtinData, inout Surface l, inout ShaderData d
                     #if NEED_FACING
                        , bool facing
                     #endif
                  )
               {
                 // Removed since crossfade does not work, probably needs extra material setup.   
                 //#ifdef LOD_FADE_CROSSFADE // enable dithering LOD transition if user select CrossFade transition in LOD group
                 //    uint3 fadeMaskSeed = asuint((int3)(V * _ScreenSize.xyx)); // Quantize V to _ScreenSize values
                 //    LODDitheringTransition(fadeMaskSeed, unity_LODFade.x);
                 //#endif
        
                 d = CreateShaderData(m2ps
                  #if NEED_FACING
                    , facing
                  #endif
                 );

                 

                 l = (Surface)0;

                 l.Albedo = half3(0.5, 0.5, 0.5);
                 l.Normal = float3(0,0,1);
                 l.Occlusion = 1;
                 l.Alpha = 1;
                 l.SpecularOcclusion = 1;

                 #ifdef _DEPTHOFFSET_ON
                    l.outputDepth = posInput.deviceDepth;
                 #endif

                 ChainSurfaceFunction(l, d);

                 #ifdef _DEPTHOFFSET_ON
                    posInput.deviceDepth = l.outputDepth;
                 #endif

                 #if _UNLIT
                     //l.Emission = l.Albedo;
                     //l.Albedo = 0;
                     l.Normal = half3(0,0,1);
                     l.Occlusion = 1;
                     l.Metallic = 0;
                     l.Specular = 0;
                 #endif

                 surfaceData.geomNormalWS = d.worldSpaceNormal;
                 surfaceData.tangentWS = d.worldSpaceTangent;
                 fragInputs.tangentToWorld = d.TBNMatrix;

                 float3 bentNormalWS;
                 BuildSurfaceData(fragInputs, l, V, posInput, surfaceData, bentNormalWS);

                 InitBuiltinData(posInput, l.Alpha, bentNormalWS, -d.worldSpaceNormal, fragInputs.texCoord1, fragInputs.texCoord2, builtinData);

                 

                 builtinData.emissiveColor = l.Emission;

                 #if defined(_OVERRIDE_BAKEDGI)
                    builtinData.bakeDiffuseLighting = l.DiffuseGI;
                    builtinData.backBakeDiffuseLighting = l.BackDiffuseGI;
                    builtinData.emissiveColor += l.SpecularGI;
                 #endif
                 
                 #if defined(_OVERRIDE_SHADOWMASK)
                   builtinData.shadowMask0 = l.ShadowMask.x;
                   builtinData.shadowMask1 = l.ShadowMask.y;
                   builtinData.shadowMask2 = l.ShadowMask.z;
                   builtinData.shadowMask3 = l.ShadowMask.w;
                #endif
        
                 #if (SHADERPASS == SHADERPASS_DISTORTION)
                     //builtinData.distortion = surfaceDescription.Distortion;
                     //builtinData.distortionBlur = surfaceDescription.DistortionBlur;
                     builtinData.distortion = float2(0.0, 0.0);
                     builtinData.distortionBlur = 0.0;
                 #else
                     builtinData.distortion = float2(0.0, 0.0);
                     builtinData.distortionBlur = 0.0;
                 #endif
        
                   PostInitBuiltinData(V, posInput, surfaceData, builtinData);
               }
        

            
            void Frag(  PackedVaryingsToPS packedInput
            #ifdef WRITE_NORMAL_BUFFER
            , out float4 outNormalBuffer : SV_Target0
                #ifdef WRITE_MSAA_DEPTH
                , out float1 depthColor : SV_Target1
                #endif
            #elif defined(WRITE_MSAA_DEPTH) // When only WRITE_MSAA_DEPTH is define and not WRITE_NORMAL_BUFFER it mean we are Unlit and only need depth, but we still have normal buffer binded
            , out float4 outNormalBuffer : SV_Target0
            , out float1 depthColor : SV_Target1
            #elif defined(SCENESELECTIONPASS)
            , out float4 outColor : SV_Target0
            #endif

            #ifdef _DEPTHOFFSET_ON
            , out float outputDepth : SV_Depth
            #endif
            #if NEED_FACING
            , bool facing : SV_IsFrontFace
            #endif
        )
         {
             UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(packedInput);
             FragInputs input = BuildFragInputs(packedInput.vmesh);

             // input.positionSS is SV_Position
             PositionInputs posInput = GetPositionInput(input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS);

             float3 V = GetWorldSpaceNormalizeViewDir(input.positionRWS);

            SurfaceData surfaceData;
            BuiltinData builtinData;
            Surface l;
            ShaderData d;
            GetSurfaceAndBuiltinData(packedInput.vmesh, input, V, posInput, surfaceData, builtinData, l, d
               #if NEED_FACING
                  , facing
               #endif
               );


         #ifdef _DEPTHOFFSET_ON
             outputDepth = posInput.deviceDepth;
         #endif

         #ifdef WRITE_NORMAL_BUFFER
             EncodeIntoNormalBuffer(ConvertSurfaceDataToNormalData(surfaceData), posInput.positionSS, outNormalBuffer);
             #ifdef WRITE_MSAA_DEPTH
             // In case we are rendering in MSAA, reading the an MSAA depth buffer is way too expensive. To avoid that, we export the depth to a color buffer
             depthColor = packedInput.vmesh.pos.z;
             #endif
         #elif defined(WRITE_MSAA_DEPTH) // When we are MSAA depth only without normal buffer
             // Due to the binding order of these two render targets, we need to have them both declared
             outNormalBuffer = float4(0.0, 0.0, 0.0, 1.0);
             // In case we are rendering in MSAA, reading the an MSAA depth buffer is way too expensive. To avoid that, we export the depth to a color buffer
             depthColor = packedInput.vmesh.pos.z;
         #elif defined(SCENESELECTIONPASS)
             // We use depth prepass for scene selection in the editor, this code allow to output the outline correctly
             outColor = float4(_ObjectId, _PassValue, 1.0, 1.0);
         #endif
         }

         ENDHLSL
     }

        
              Pass
        {
            // based on HDLitPass.template
            Name "DepthOnly"
            Tags { "LightMode" = "DepthOnly" }
            
            //-------------------------------------------------------------------------------------
            // Render Modes (Blend, Cull, ZTest, Stencil, etc)
            //-------------------------------------------------------------------------------------
            
            Cull Back
        
            
            ZWrite On
        
            
            // Stencil setup
        Stencil
        {
           WriteMask [_StencilWriteMaskDepth]
           Ref [_StencilRefDepth]
           Comp Always
           Pass Replace
        }
                Cull [_CullMode]

            
            //-------------------------------------------------------------------------------------
            // End Render Modes
            //-------------------------------------------------------------------------------------
        
            HLSLPROGRAM
        
            #pragma target 4.5
            #pragma only_renderers d3d11 ps4 xboxone vulkan metal switch
            //#pragma enable_d3d11_debug_symbols
        
            #pragma multi_compile_instancing
        
        //#pragma multi_compile_local _ _ALPHATEST_ON
        
            // #pragma multi_compile _ LOD_FADE_CROSSFADE
        
            //#pragma shader_feature _SURFACE_TYPE_TRANSPARENT
            //#pragma shader_feature_local _ _BLENDMODE_ALPHA _BLENDMODE_ADD _BLENDMODE_PRE_MULTIPLY
        
            //-------------------------------------------------------------------------------------
            // Variant Definitions (active field translations to HDRP defines)
            //-------------------------------------------------------------------------------------
            // #define _MATERIAL_FEATURE_SUBSURFACE_SCATTERING 1
            // #define _MATERIAL_FEATURE_TRANSMISSION 1
            // #define _MATERIAL_FEATURE_ANISOTROPY 1
            // #define _MATERIAL_FEATURE_IRIDESCENCE 1
            // #define _MATERIAL_FEATURE_SPECULAR_COLOR 1
            #define _ENABLE_FOG_ON_TRANSPARENT 1
            // #define _AMBIENT_OCCLUSION 1
            // #define _SPECULAR_OCCLUSION_FROM_AO 1
            // #define _SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL 1
            // #define _SPECULAR_OCCLUSION_CUSTOM 1
            // #define _ENERGY_CONSERVING_SPECULAR 1
            // #define _ENABLE_GEOMETRIC_SPECULAR_AA 1
            // #define _HAS_REFRACTION 1
            // #define _REFRACTION_PLANE 1
            // #define _REFRACTION_SPHERE 1
            // #define _DISABLE_DECALS 1
            // #define _DISABLE_SSR 1
            // #define _ADD_PRECOMPUTED_VELOCITY
            // #define _WRITE_TRANSPARENT_MOTION_VECTOR 1
            // #define _DEPTHOFFSET_ON 1
            // #define _BLENDMODE_PRESERVE_SPECULAR_LIGHTING 1

            #define SHADERPASS SHADERPASS_DEPTH_ONLY
            #pragma multi_compile _ WRITE_NORMAL_BUFFER
            #pragma multi_compile _ WRITE_MSAA_DEPTH
            #define RAYTRACING_SHADER_GRAPH_HIGH
            #define _PASSDEPTH 1

            
            


    #pragma shader_feature_local _ALPHATEST_ON

    #pragma shader_feature_local_fragment _NORMALMAP_TANGENT_SPACE


    #pragma shader_feature_local _NORMALMAP
    #pragma shader_feature_local_fragment _MASKMAP
    #pragma shader_feature_local_fragment _EMISSIVE_COLOR_MAP
    #pragma shader_feature_local_fragment _ANISOTROPYMAP
    #pragma shader_feature_local_fragment _DETAIL_MAP
    #pragma shader_feature_local_fragment _SUBSURFACE_MASK_MAP
    #pragma shader_feature_local_fragment _THICKNESSMAP
    #pragma shader_feature_local_fragment _IRIDESCENCE_THICKNESSMAP
    #pragma shader_feature_local_fragment _SPECULARCOLORMAP

    // MaterialFeature are used as shader feature to allow compiler to optimize properly
    #pragma shader_feature_local_fragment _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
    #pragma shader_feature_local_fragment _MATERIAL_FEATURE_TRANSMISSION
    #pragma shader_feature_local_fragment _MATERIAL_FEATURE_ANISOTROPY
    #pragma shader_feature_local_fragment _MATERIAL_FEATURE_CLEAR_COAT
    #pragma shader_feature_local_fragment _MATERIAL_FEATURE_IRIDESCENCE
    #pragma shader_feature_local_fragment _MATERIAL_FEATURE_SPECULAR_COLOR


    #pragma shader_feature_local_fragment _ENABLESPECULAROCCLUSION
    #pragma shader_feature_local_fragment _ _SPECULAR_OCCLUSION_NONE _SPECULAR_OCCLUSION_FROM_BENT_NORMAL_MAP

    #ifdef _ENABLESPECULAROCCLUSION
        #define _SPECULAR_OCCLUSION_FROM_BENT_NORMAL_MAP
    #endif


        #pragma shader_feature_local_fragment _OBSTRUCTION_CURVE
        #pragma shader_feature_local_fragment _DISSOLVEMASK
        #pragma shader_feature_local_fragment _ZONING
        #pragma shader_feature_local_fragment _REPLACEMENT
        #pragma shader_feature_local_fragment _PLAYERINDEPENDENT


   #define _HDRP 1
#define _USINGTEXCOORD1 1
#define _USINGTEXCOORD2 1


               #pragma vertex Vert
   #pragma fragment Frag
        
            

                  // useful conversion functions to make surface shader code just work

      #define UNITY_DECLARE_TEX2D(name) TEXTURE2D(name); SAMPLER(sampler##name);
      #define UNITY_DECLARE_TEX2D_NOSAMPLER(name) TEXTURE2D(name);
      #define UNITY_DECLARE_TEX2DARRAY(name) TEXTURE2D_ARRAY(name); SAMPLER(sampler##name);
      #define UNITY_DECLARE_TEX2DARRAY_NOSAMPLER(tex) TEXTURE2D_ARRAY(tex);

      #define UNITY_SAMPLE_TEX2DARRAY(tex,coord)            SAMPLE_TEXTURE2D_ARRAY(tex, sampler##tex, coord.xy, coord.z)
      #define UNITY_SAMPLE_TEX2DARRAY_LOD(tex,coord,lod)    SAMPLE_TEXTURE2D_ARRAY_LOD(tex, sampler##tex, coord.xy, coord.z, lod)
      #define UNITY_SAMPLE_TEX2D(tex, coord)                SAMPLE_TEXTURE2D(tex, sampler##tex, coord)
      #define UNITY_SAMPLE_TEX2D_SAMPLER(tex, samp, coord)  SAMPLE_TEXTURE2D(tex, sampler##samp, coord)

      #define UNITY_SAMPLE_TEX2D_LOD(tex,coord, lod)   SAMPLE_TEXTURE2D_LOD(tex, sampler_##tex, coord, lod)
      #define UNITY_SAMPLE_TEX2D_SAMPLER_LOD(tex,samplertex,coord, lod) SAMPLE_TEXTURE2D_LOD (tex, sampler##samplertex,coord, lod)

      #if defined(UNITY_COMPILER_HLSL)
         #define UNITY_INITIALIZE_OUTPUT(type,name) name = (type)0;
      #else
         #define UNITY_INITIALIZE_OUTPUT(type,name)
      #endif

      #define sampler2D_float sampler2D
      #define sampler2D_half sampler2D

      #undef WorldNormalVector
      #define WorldNormalVector(data, normal) mul(normal, data.TBNMatrix)

      #define UnityObjectToWorldNormal(normal) mul(GetObjectToWorldMatrix(), normal)




// HDRP Adapter stuff


            // If we use subsurface scattering, enable output split lighting (for forward pass)
            #if defined(_MATERIAL_FEATURE_SUBSURFACE_SCATTERING) && !defined(_SURFACE_TYPE_TRANSPARENT)
            #define OUTPUT_SPLIT_LIGHTING
            #endif

            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Version.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
        
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"
        
            // define FragInputs structure
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
            #if (SHADER_LIBRARY_VERSION_MAJOR >= 10)
               #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/Functions.hlsl"
            #endif


        

            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
        #ifdef DEBUG_DISPLAY
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
        #endif
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
        
        #if (SHADERPASS == SHADERPASS_FORWARD)
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/Lighting.hlsl"
        
            #define HAS_LIGHTLOOP
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/LightLoopDef.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/LightLoop.hlsl"
        #else
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
        #endif
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalUtilities.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitDecalData.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"
        
            // Used by SceneSelectionPass
            int _ObjectId;
            int _PassValue;
        
           
            // data across stages, stripped like the above.
            struct VertexToPixel
            {
               float4 pos : SV_POSITION;
               float3 worldPos : TEXCOORD0;
               float3 worldNormal : TEXCOORD1;
               float4 worldTangent : TEXCOORD2;
               float4 texcoord0 : TEXCOORD3;
               float4 texcoord1 : TEXCOORD4;
               float4 texcoord2 : TEXCOORD5;

               // #if %TEXCOORD3REQUIREKEY%
                float4 texcoord3 : TEXCOORD6;
               // #endif

               // #if %SCREENPOSREQUIREKEY%
               // float4 screenPos : TEXCOORD7;
               // #endif

               // #if %VERTEXCOLORREQUIREKEY%
               // float4 vertexColor : COLOR;
               // #endif

               // #if %EXTRAV2F0REQUIREKEY%
               // float4 extraV2F0 : TEXCOORD8;
               // #endif

               // #if %EXTRAV2F1REQUIREKEY%
               // float4 extraV2F1 : TEXCOORD9;
               // #endif

               // #if %EXTRAV2F2REQUIREKEY%
               // float4 extraV2F2 : TEXCOORD10;
               // #endif

               // #if %EXTRAV2F3REQUIREKEY%
               // float4 extraV2F3 : TEXCOORD11;
               // #endif

               // #if %EXTRAV2F4REQUIREKEY%
               // float4 extraV2F4 : TEXCOORD12;
               // #endif

               // #if %EXTRAV2F5REQUIREKEY%
               // float4 extraV2F5 : TEXCOORD13;
               // #endif

               // #if %EXTRAV2F6REQUIREKEY%
               // float4 extraV2F6 : TEXCOORD14;
               // #endif

               // #if %EXTRAV2F7REQUIREKEY%
               // float4 extraV2F7 : TEXCOORD15;
               // #endif

               #if UNITY_ANY_INSTANCING_ENABLED
                  UNITY_VERTEX_INPUT_INSTANCE_ID
               #endif // UNITY_ANY_INSTANCING_ENABLED

               UNITY_VERTEX_OUTPUT_STEREO
            };



            
            
            // data describing the user output of a pixel
            struct Surface
            {
               half3 Albedo;
               half Height;
               half3 Normal;
               half Smoothness;
               half3 Emission;
               half Metallic;
               half3 Specular;
               half Occlusion;
               half SpecularPower; // for simple lighting
               half Alpha;
               float outputDepth; // if written, SV_Depth semantic is used. ShaderData.clipPos.z is unused value
               // HDRP Only
               half SpecularOcclusion;
               half SubsurfaceMask;
               half Thickness;
               half CoatMask;
               half CoatSmoothness;
               half Anisotropy;
               half IridescenceMask;
               half IridescenceThickness;
               int DiffusionProfileHash;
               float SpecularAAThreshold;
               float SpecularAAScreenSpaceVariance;
               // requires _OVERRIDE_BAKEDGI to be defined, but is mapped in all pipelines
               float3 DiffuseGI;
               float3 BackDiffuseGI;
               float3 SpecularGI;
               float ior;
               float3 transmittanceColor;
               float atDistance;
               float transmittanceMask;
               // requires _OVERRIDE_SHADOWMASK to be defines
               float4 ShadowMask;


            };

            // Data the user declares in blackboard blocks
            struct Blackboard
            {
                
                float blackboardDummyData;
            };

            // data the user might need, this will grow to be big. But easy to strip
            struct ShaderData
            {
               float4 clipPos; // SV_POSITION
               float3 localSpacePosition;
               float3 localSpaceNormal;
               float3 localSpaceTangent;
        
               float3 worldSpacePosition;
               float3 worldSpaceNormal;
               float3 worldSpaceTangent;
               float tangentSign;

               float3 worldSpaceViewDir;
               float3 tangentSpaceViewDir;

               float4 texcoord0;
               float4 texcoord1;
               float4 texcoord2;
               float4 texcoord3;

               float2 screenUV;
               float4 screenPos;

               float4 vertexColor;
               bool isFrontFace;

               float4 extraV2F0;
               float4 extraV2F1;
               float4 extraV2F2;
               float4 extraV2F3;
               float4 extraV2F4;
               float4 extraV2F5;
               float4 extraV2F6;
               float4 extraV2F7;

               float3x3 TBNMatrix;
               Blackboard blackboard;
            };

            struct VertexData
            {
               #if SHADER_TARGET > 30
               // UNITY_VERTEX_INPUT_INSTANCE_ID;
               #endif
               float4 vertex : POSITION;
               float3 normal : NORMAL;
               float4 tangent : TANGENT;
               float4 texcoord0 : TEXCOORD0;

               // optimize out mesh coords when not in use by user or lighting system
               #if _URP && (_USINGTEXCOORD1 || _PASSMETA || _PASSFORWARD || _PASSGBUFFER)
                  float4 texcoord1 : TEXCOORD1;
               #endif

               #if _URP && (_USINGTEXCOORD2 || _PASSMETA || ((_PASSFORWARD || _PASSGBUFFER) && defined(DYNAMICLIGHTMAP_ON)))
                  float4 texcoord2 : TEXCOORD2;
               #endif

               #if _STANDARD && (_USINGTEXCOORD1 || (_PASSMETA || ((_PASSFORWARD || _PASSGBUFFER || _PASSFORWARDADD) && LIGHTMAP_ON)))
                  float4 texcoord1 : TEXCOORD1;
               #endif
               #if _STANDARD && (_USINGTEXCOORD2 || (_PASSMETA || ((_PASSFORWARD || _PASSGBUFFER) && DYNAMICLIGHTMAP_ON)))
                  float4 texcoord2 : TEXCOORD2;
               #endif


               #if _HDRP
                  float4 texcoord1 : TEXCOORD1;
                  float4 texcoord2 : TEXCOORD2;
               #endif

               // #if %TEXCOORD3REQUIREKEY%
                float4 texcoord3 : TEXCOORD3;
               // #endif

               // #if %VERTEXCOLORREQUIREKEY%
               // float4 vertexColor : COLOR;
               // #endif

               #if _HDRP && (_PASSMOTIONVECTOR || ((_PASSFORWARD || _PASSUNLIT) && defined(_WRITE_TRANSPARENT_MOTION_VECTOR)))
                  float3 previousPositionOS : TEXCOORD4; // Contain previous transform position (in case of skinning for example)
                  #if defined (_ADD_PRECOMPUTED_VELOCITY)
                     float3 precomputedVelocity    : TEXCOORD5; // Add Precomputed Velocity (Alembic computes velocities on runtime side).
                  #endif
               #endif

               UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct TessVertex 
            {
               float4 vertex : INTERNALTESSPOS;
               float3 normal : NORMAL;
               float4 tangent : TANGENT;
               float4 texcoord0 : TEXCOORD0;
               float4 texcoord1 : TEXCOORD1;
               float4 texcoord2 : TEXCOORD2;

               // #if %TEXCOORD3REQUIREKEY%
                float4 texcoord3 : TEXCOORD3;
               // #endif

               // #if %VERTEXCOLORREQUIREKEY%
               // float4 vertexColor : COLOR;
               // #endif

               // #if %EXTRAV2F0REQUIREKEY%
               // float4 extraV2F0 : TEXCOORD5;
               // #endif

               // #if %EXTRAV2F1REQUIREKEY%
               // float4 extraV2F1 : TEXCOORD6;
               // #endif

               // #if %EXTRAV2F2REQUIREKEY%
               // float4 extraV2F2 : TEXCOORD7;
               // #endif

               // #if %EXTRAV2F3REQUIREKEY%
               // float4 extraV2F3 : TEXCOORD8;
               // #endif

               // #if %EXTRAV2F4REQUIREKEY%
               // float4 extraV2F4 : TEXCOORD9;
               // #endif

               // #if %EXTRAV2F5REQUIREKEY%
               // float4 extraV2F5 : TEXCOORD10;
               // #endif

               // #if %EXTRAV2F6REQUIREKEY%
               // float4 extraV2F6 : TEXCOORD11;
               // #endif

               // #if %EXTRAV2F7REQUIREKEY%
               // float4 extraV2F7 : TEXCOORD12;
               // #endif

               #if _HDRP && (_PASSMOTIONVECTOR || ((_PASSFORWARD || _PASSUNLIT) && defined(_WRITE_TRANSPARENT_MOTION_VECTOR)))
                  float3 previousPositionOS : TEXCOORD13; // Contain previous transform position (in case of skinning for example)
                  #if defined (_ADD_PRECOMPUTED_VELOCITY)
                     float3 precomputedVelocity : TEXCOORD14;
                  #endif
               #endif

               UNITY_VERTEX_INPUT_INSTANCE_ID
               UNITY_VERTEX_OUTPUT_STEREO
            };

            struct ExtraV2F
            {
               float4 extraV2F0;
               float4 extraV2F1;
               float4 extraV2F2;
               float4 extraV2F3;
               float4 extraV2F4;
               float4 extraV2F5;
               float4 extraV2F6;
               float4 extraV2F7;
               Blackboard blackboard;
               float4 time;
            };


            float3 WorldToTangentSpace(ShaderData d, float3 normal)
            {
               return mul(d.TBNMatrix, normal);
            }

            float3 TangentToWorldSpace(ShaderData d, float3 normal)
            {
               return mul(normal, d.TBNMatrix);
            }

            // in this case, make standard more like SRPs, because we can't fix
            // unity_WorldToObject in HDRP, since it already does macro-fu there

            #if _STANDARD
               float3 TransformWorldToObject(float3 p) { return mul(unity_WorldToObject, float4(p, 1)); };
               float3 TransformObjectToWorld(float3 p) { return mul(unity_ObjectToWorld, float4(p, 1)); };
               float4 TransformWorldToObject(float4 p) { return mul(unity_WorldToObject, p); };
               float4 TransformObjectToWorld(float4 p) { return mul(unity_ObjectToWorld, p); };
               float4x4 GetWorldToObjectMatrix() { return unity_WorldToObject; }
               float4x4 GetObjectToWorldMatrix() { return unity_ObjectToWorld; }
               #if (defined(SHADER_API_D3D11) || defined(SHADER_API_XBOXONE) || defined(UNITY_COMPILER_HLSLCC) || defined(SHADER_API_PSSL) || (SHADER_TARGET_SURFACE_ANALYSIS && !SHADER_TARGET_SURFACE_ANALYSIS_MOJOSHADER))
                 #define UNITY_SAMPLE_TEX2D_LOD(tex,coord, lod) tex.SampleLevel (sampler##tex,coord, lod)
                 #define UNITY_SAMPLE_TEX2D_SAMPLER_LOD(tex,samplertex,coord, lod) tex.SampleLevel (sampler##samplertex,coord, lod)
              #else
                 #define UNITY_SAMPLE_TEX2D_LOD(tex,coord,lod) tex2D (tex,coord,0,lod)
                 #define UNITY_SAMPLE_TEX2D_SAMPLER_LOD(tex,samplertex,coord,lod) tex2D (tex,coord,0,lod)
              #endif

               #undef GetWorldToObjectMatrix()

               #define GetWorldToObjectMatrix()   unity_WorldToObject


            #endif

            float3 GetCameraWorldPosition()
            {
               #if _HDRP
                  return GetCameraRelativePositionWS(_WorldSpaceCameraPos);
               #else
                  return _WorldSpaceCameraPos;
               #endif
            }

            #if _GRABPASSUSED
               #if _STANDARD
                  TEXTURE2D(%GRABTEXTURE%);
                  SAMPLER(sampler_%GRABTEXTURE%);
               #endif

               half3 GetSceneColor(float2 uv)
               {
                  #if _STANDARD
                     return SAMPLE_TEXTURE2D(%GRABTEXTURE%, sampler_%GRABTEXTURE%, uv).rgb;
                  #else
                     return SHADERGRAPH_SAMPLE_SCENE_COLOR(uv);
                  #endif
               }
            #endif


      
            #if _STANDARD
               UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);
               float GetSceneDepth(float2 uv) { return SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, uv); }
               float GetLinear01Depth(float2 uv) { return Linear01Depth(GetSceneDepth(uv)); }
               float GetLinearEyeDepth(float2 uv) { return LinearEyeDepth(GetSceneDepth(uv)); } 
            #else
               float GetSceneDepth(float2 uv) { return SHADERGRAPH_SAMPLE_SCENE_DEPTH(uv); }
               float GetLinear01Depth(float2 uv) { return Linear01Depth(GetSceneDepth(uv), _ZBufferParams); }
               float GetLinearEyeDepth(float2 uv) { return LinearEyeDepth(GetSceneDepth(uv), _ZBufferParams); } 
            #endif

            float3 GetWorldPositionFromDepthBuffer(float2 uv, float3 worldSpaceViewDir)
            {
               float eye = GetLinearEyeDepth(uv);
               float3 camView = mul((float3x3)GetObjectToWorldMatrix(), transpose(mul(GetWorldToObjectMatrix(), UNITY_MATRIX_I_V)) [2].xyz);

               float dt = dot(worldSpaceViewDir, camView);
               float3 div = worldSpaceViewDir/dt;
               float3 wpos = (eye * div) + GetCameraWorldPosition();
               return wpos;
            }

            #if _HDRP
            float3 ObjectToWorldSpacePosition(float3 pos)
            {
               return GetAbsolutePositionWS(TransformObjectToWorld(pos));
            }
            #else
            float3 ObjectToWorldSpacePosition(float3 pos)
            {
               return TransformObjectToWorld(pos);
            }
            #endif

            #if _STANDARD
               UNITY_DECLARE_SCREENSPACE_TEXTURE(_CameraDepthNormalsTexture);
               float3 GetSceneNormal(float2 uv, float3 worldSpaceViewDir)
               {
                  float4 depthNorms = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_CameraDepthNormalsTexture, uv);
                  float3 norms = DecodeViewNormalStereo(depthNorms);
                  norms = mul((float3x3)GetWorldToViewMatrix(), norms) * 0.5 + 0.5;
                  return norms;
               }
            #elif _HDRP
               float3 GetSceneNormal(float2 uv, float3 worldSpaceViewDir)
               {
                  NormalData nd;
                  DecodeFromNormalBuffer(_ScreenSize.xy * uv, nd);
                  return nd.normalWS;
               }
            #elif _URP
               #if (SHADER_LIBRARY_VERSION_MAJOR >= 10)
                  #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareNormalsTexture.hlsl"
               #endif

               float3 GetSceneNormal(float2 uv, float3 worldSpaceViewDir)
               {
                  #if (SHADER_LIBRARY_VERSION_MAJOR >= 10)
                     return SampleSceneNormals(uv);
                  #else
                     float3 wpos = GetWorldPositionFromDepthBuffer(uv, worldSpaceViewDir);
                     return normalize(-cross(ddx(wpos), ddy(wpos))) * 0.5 + 0.5;
                  #endif

                }
             #endif

             #if _HDRP

               half3 UnpackNormalmapRGorAG(half4 packednormal)
               {
                     // This do the trick
                  packednormal.x *= packednormal.w;

                  half3 normal;
                  normal.xy = packednormal.xy * 2 - 1;
                  normal.z = sqrt(1 - saturate(dot(normal.xy, normal.xy)));
                  return normal;
               }
               half3 UnpackNormal(half4 packednormal)
               {
                  #if defined(UNITY_NO_DXT5nm)
                     return packednormal.xyz * 2 - 1;
                  #else
                     return UnpackNormalmapRGorAG(packednormal);
                  #endif
               }
               #endif
               #if _HDRP || _URP

               half3 UnpackScaleNormal(half4 packednormal, half scale)
               {
                 #ifndef UNITY_NO_DXT5nm
                   // Unpack normal as DXT5nm (1, y, 1, x) or BC5 (x, y, 0, 1)
                   // Note neutral texture like "bump" is (0, 0, 1, 1) to work with both plain RGB normal and DXT5nm/BC5
                   packednormal.x *= packednormal.w;
                 #endif
                   half3 normal;
                   normal.xy = (packednormal.xy * 2 - 1) * scale;
                   normal.z = sqrt(1 - saturate(dot(normal.xy, normal.xy)));
                   return normal;
               }	

             #endif


            void GetSun(out float3 lightDir, out float3 color)
            {
               lightDir = float3(0.5, 0.5, 0);
               color = 1;
               #if _HDRP
                  if (_DirectionalLightCount > 0)
                  {
                     DirectionalLightData light = _DirectionalLightDatas[0];
                     lightDir = -light.forward.xyz;
                     color = light.color;
                  }
               #elif _STANDARD
			         lightDir = normalize(_WorldSpaceLightPos0.xyz);
                  color = _LightColor0.rgb;
               #elif _URP
	               Light light = GetMainLight();
	               lightDir = light.direction;
	               color = light.color;
               #endif
            }




            CBUFFER_START(UnityPerMaterial)
               float _StencilRef;
               float _StencilWriteMask;
               float _StencilRefDepth;
               float _StencilWriteMaskDepth;
               float _StencilRefMV;
               float _StencilWriteMaskMV;
               float _StencilRefDistortionVec;
               float _StencilWriteMaskDistortionVec;
               float _StencilWriteMaskGBuffer;
               float _StencilRefGBuffer;
               float _ZTestGBuffer;
               float _RequireSplitLighting;
               float _ReceivesSSR;
               float _ZWrite;
               float _TransparentSortPriority;
               float _ZTestDepthEqualForOpaque;
               float _ZTestTransparent;
               float _TransparentBackfaceEnable;
               float _AlphaCutoffEnable;
               float _UseShadowThreshold;

               
    float4 _BaseColorMap_ST;
    float4 _DetailMap_ST;
    float4 _EmissiveColorMap_ST;

    float _UVBase;
    half4 _Color;
    half4 _BaseColor; // TODO CHECK
    half _Cutoff; 
    half _Mode;
    //float _Cull;
    float _CullMode;
    float _CullModeForward;
    half _Metallic;
    half3 _EmissionColor;
    half _UVSec;

    float3 _EmissiveColor;
    float _AlbedoAffectEmissive;
    float _EmissiveExposureWeight;
    float _MetallicRemapMin;
    float _MetallicRemapMax;
    float _Smoothness;
    float _SmoothnessRemapMin;
    float _SmoothnessRemapMax;
    float _AlphaRemapMin;
    float _AlphaRemapMax;
    float _AORemapMin;
    float _AORemapMax;
    float _NormalScale;
    float _DetailAlbedoScale;
    float _DetailNormalScale;
    float _DetailSmoothnessScale;
    float _Anisotropy;
    float _DiffusionProfileHash;
    float _SubsurfaceMask;
    float _Thickness;
    float4 _ThicknessRemap;
    float _IridescenceThickness;
    float4 _IridescenceThicknessRemap;
    float _IridescenceMask;
    float _CoatMask;
    float4 _SpecularColor;
    float _EnergyConservingSpecularColor;
    int _MaterialID;

    float _LinkDetailsWithBase;
    float4 _UVMappingMask;
    float4 _UVDetailsMappingMask;

    float4 _UVMappingMaskEmissive;

    float _AlphaCutoff;
    //float _AlphaCutoffEnable;


    float _SyncCullMode;

    float _IsReplacementShader;
    float _TriggerMode;
    float _RaycastMode;
    float _IsExempt;
    float _isReferenceMaterial;
    float _InteractionMode;
    int _ArrayLength = 0;
    #if (defined(SHADER_API_GLES) || defined(SHADER_API_GLES3)) 
        float4 _PlayersPosVectorArray[20];
        float _PlayersDataFloatArray[150];     
    #else
        float4 _PlayersPosVectorArray[100];
        float _PlayersDataFloatArray[500];  
    #endif
    float _tDirection = 0;
    float _numOfPlayersInside = 0;
    float _tValue = 0;
    float _id = 0;
    #if _ZONING
        #if (defined(SHADER_API_GLES) || defined(SHADER_API_GLES3)) 
            float _ZDFA[500];
        #else
            float _ZDFA[1000];
        #endif
        float _ZonesDataCount;
    #endif
    #if _REPLACEMENT        
        half4 _DissolveColorGlobal;
        float _DissolveColorSaturationGlobal;
        float _DissolveEmissionGlobal;
        float _DissolveEmissionBoosterGlobal;
        float _TextureVisibilityGlobal;
        float _ObstructionGlobal;
        float _AngleStrengthGlobal;
        float _ConeStrengthGlobal;
        float _ConeObstructionDestroyRadiusGlobal;
        float _CylinderStrengthGlobal;
        float _CylinderObstructionDestroyRadiusGlobal;
        float _CircleStrengthGlobal;
        float _CircleObstructionDestroyRadiusGlobal;
        float _CurveStrengthGlobal;
        float _CurveObstructionDestroyRadiusGlobal;
        float _DissolveFallOffGlobal;
        float _IntrinsicDissolveStrengthGlobal;
        float _PreviewModeGlobal;
        float _UVsGlobal;
        float _hasClippedShadowsGlobal;
        float _FloorGlobal;
        float _FloorModeGlobal;
        float _FloorYGlobal;
        float _PlayerPosYOffsetGlobal;
        float _FloorYTextureGradientLengthGlobal;
        float _AnimationEnabledGlobal;
        float _AnimationSpeedGlobal;
        float _DefaultEffectRadiusGlobal;
        float _TransitionDurationGlobal;        
        float _TexturedEmissionEdgeGlobal;
        float _TexturedEmissionEdgeStrengthGlobal;
        float _IsometricExclusionGlobal;
        float _IsometricExclusionDistanceGlobal;
        float _IsometricExclusionGradientLengthGlobal;
        float _CeilingGlobal;
        float _CeilingModeGlobal;
        float _CeilingBlendModeGlobal;
        float _CeilingYGlobal;
        float _CeilingPlayerYOffsetGlobal;
        float _CeilingYGradientLengthGlobal;
        float _ZoningGlobal;
        float _ZoningModeGlobal;
        float _ZoningEdgeGradientLengthGlobal;
        float _IsZoningRevealableGlobal;
        float _SyncZonesWithFloorYGlobal;
        float _SyncZonesFloorYOffsetGlobal;
        float4 _ObstructionCurveGlobal_TexelSize;
        float4 _DissolveMaskGlobal_TexelSize;
        float _DissolveMaskEnabledGlobal;
        float _PreviewIndicatorLineThicknessGlobal;
    #else
        half _TextureVisibility;
        half _AngleStrength;
        float _Obstruction;
        float _UVs;
        float4 _ObstructionCurve_TexelSize;      
        float _DissolveMaskEnabled;
        float4 _DissolveMask_TexelSize;
        half4 _DissolveColor;
        float _DissolveColorSaturation;
        float _DissolveEmission;
        float _DissolveEmissionBooster;
        float _hasClippedShadows;
        float _ConeStrength;
        float _ConeObstructionDestroyRadius;
        float _CylinderStrength;
        float _CylinderObstructionDestroyRadius;
        float _CircleStrength;
        float _CircleObstructionDestroyRadius;
        float _CurveStrength;
        float _CurveObstructionDestroyRadius;
        float _IntrinsicDissolveStrength;
        float _DissolveFallOff;
        float _PreviewMode;
        float _PreviewIndicatorLineThickness;
        float _AnimationEnabled;
        float _AnimationSpeed;
        float _DefaultEffectRadius;
        float _TransitionDuration;
        float _TexturedEmissionEdge;
        float _TexturedEmissionEdgeStrength;
        float _IsometricExclusion;
        float _IsometricExclusionDistance;
        float _IsometricExclusionGradientLength;
        float _Floor;
        float _FloorMode;
        float _FloorY;
        float _FloorYTextureGradientLength;
        float _PlayerPosYOffset;
        float _Ceiling;
        float _CeilingMode;
        float _CeilingBlendMode;
        float _CeilingY;
        float _CeilingPlayerYOffset;
        float _CeilingYGradientLength;
        float _Zoning;
        float _ZoningMode;
        float _ZoningEdgeGradientLength;
        float _IsZoningRevealable;
        float _SyncZonesWithFloorY;
        float _SyncZonesFloorYOffset;
    #endif



            CBUFFER_END

            

            

            #ifdef unity_WorldToObject
#undef unity_WorldToObject
#endif
#ifdef unity_ObjectToWorld
#undef unity_ObjectToWorld
#endif
#define unity_ObjectToWorld GetObjectToWorldMatrix()
#define unity_WorldToObject GetWorldToObjectMatrix()

    sampler2D _EmissiveColorMap;
    sampler2D _BaseColorMap;
    sampler2D _MaskMap;
    sampler2D _NormalMap;
    sampler2D _NormalMapOS;
    sampler2D _DetailMap;
    sampler2D _TangentMap;
    sampler2D _TangentMapOS;
    sampler2D _AnisotropyMap;
    sampler2D _SubsurfaceMaskMap;
    sampler2D _TransmissionMaskMap;
    sampler2D _ThicknessMap;
    sampler2D _IridescenceThicknessMap;
    sampler2D _IridescenceMaskMap;
    sampler2D _SpecularColorMap;

    sampler2D _CoatMaskMap;

    float3 DecodeNormal (float4 sample, float scale) {
        #if defined(UNITY_NO_DXT5nm)
            return UnpackNormalRGB(sample, scale);
        #else
            return UnpackNormalmapRGorAG(sample, scale);
        #endif
    }

	void Ext_SurfaceFunction0 (inout Surface o, ShaderData d)
	{


//SEE: https://github.com/Unity-Technologies/Graphics/blob/6fdc7996098aa184495c0a3c0da10135bfe18340/Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitDataIndividualLayer.hlsl

        float2 uvBase = _UVMappingMask.x * d.texcoord0 +
                        _UVMappingMask.y * d.texcoord1 +
                        _UVMappingMask.z * d.texcoord2 +
                        _UVMappingMask.w * d.texcoord3;

        float2 uvDetails =  _UVDetailsMappingMask.x * d.texcoord0 +
                            _UVDetailsMappingMask.y * d.texcoord1 +
                            _UVDetailsMappingMask.z * d.texcoord2 +
                            _UVDetailsMappingMask.w * d.texcoord3;


        //TODO: add planar/triplanar

        float4 texcoords;
        //texcoords.xy = d.texcoord0.xy * _BaseColorMap_ST.xy + _BaseColorMap_ST.zw; 
        texcoords.xy = uvBase * _BaseColorMap_ST.xy + _BaseColorMap_ST.zw; 

        texcoords.zw = uvDetails * _DetailMap_ST.xy + _DetailMap_ST.zw;

        if (_LinkDetailsWithBase > 0.0)
        {
            texcoords.zw = texcoords.zw  * _BaseColorMap_ST.xy + _BaseColorMap_ST.zw;

        }

            float3 detailNormalTS = float3(0.0, 0.0, 0.0);
            float detailMask = 0.0;
            #ifdef _DETAIL_MAP
                detailMask = 1.0;
                #ifdef _MASKMAP
                    detailMask = tex2D(_MaskMap, texcoords.xy).b;
                #endif
                float2 detailAlbedoAndSmoothness = tex2D(_DetailMap, texcoords.zw).rb;
                float detailAlbedo = detailAlbedoAndSmoothness.r * 2.0 - 1.0;
                float detailSmoothness = detailAlbedoAndSmoothness.g * 2.0 - 1.0;
                real4 packedNormal = tex2D(_DetailMap, texcoords.zw).rgba;
                detailNormalTS = UnpackNormalAG(packedNormal, _DetailNormalScale);
            #endif
            float4 color = tex2D(_BaseColorMap, texcoords.xy).rgba  * _Color.rgba;
            o.Albedo = color; 
            float alpha = color.a;
            alpha = lerp(_AlphaRemapMin, _AlphaRemapMax, alpha);
            o.Alpha = alpha;
            #ifdef _DETAIL_MAP
                float albedoDetailSpeed = saturate(abs(detailAlbedo) * _DetailAlbedoScale);
                float3 baseColorOverlay = lerp(sqrt(o.Albedo), (detailAlbedo < 0.0) ? float3(0.0, 0.0, 0.0) : float3(1.0, 1.0, 1.0), albedoDetailSpeed * albedoDetailSpeed);
                baseColorOverlay *= baseColorOverlay;
                o.Albedo = lerp(o.Albedo, saturate(baseColorOverlay), detailMask);
            #endif

            #ifdef _NORMALMAP
                float3 normalTS;

                //normalTS = UnpackScaleNormal(tex2D(_NormalMap, texcoords.xy), _NormalScale);
                //#ifdef _DETAIL_MAP
                //    normalTS = lerp(normalTS, BlendNormalRNM(normalTS, normalize(detailNormalTS)), detailMask); 
                //#endif
                //o.Normal = normalTS;


                #ifdef _NORMALMAP_TANGENT_SPACE
                    //normalTS = UnpackScaleNormal(tex2D(_NormalMap, texcoords.xy), _NormalScale);
                    normalTS = DecodeNormal(tex2D(_NormalMap, texcoords.xy), _NormalScale);
                #else
                    #ifdef SURFACE_GRADIENT
                        float3 normalOS = tex2D(_NormalMapOS, texcoords.xy).xyz * 2.0 - 1.0;
                        normalTS = SurfaceGradientFromPerturbedNormal(d.worldSpaceNormal, TransformObjectToWorldNormal(normalOS));
                    #else
                        float3 normalOS = UnpackNormalRGB(tex2D(_NormalMapOS, texcoords.xy), 1.0);
                        float3 bitangent = d.tangentSign * cross(d.worldSpaceNormal.xyz, d.worldSpaceTangent.xyz);
                        half3x3 tangentToWorld = half3x3(d.worldSpaceTangent.xyz, bitangent.xyz, d.worldSpaceNormal.xyz);
                        normalTS = TransformObjectToTangent(normalOS, tangentToWorld);
                    #endif
                #endif

                #ifdef _DETAIL_MAP
                    #ifdef SURFACE_GRADIENT
                    normalTS += detailNormalTS * detailMask;
                    #else
                    normalTS = lerp(normalTS, BlendNormalRNM(normalTS, normalize(detailNormalTS)), detailMask); // todo: detailMask should lerp the angle of the quaternion rotation, not the normals
                    #endif
                #endif
                o.Normal = normalTS;

            #endif

            #if defined(_MASKMAP)
                o.Smoothness = tex2D(_MaskMap, texcoords.xy).a;
                o.Smoothness = lerp(_SmoothnessRemapMin, _SmoothnessRemapMax, o.Smoothness);
            #else
                o.Smoothness = _Smoothness;
            #endif
            #ifdef _DETAIL_MAP
                float smoothnessDetailSpeed = saturate(abs(detailSmoothness) * _DetailSmoothnessScale);
                float smoothnessOverlay = lerp(o.Smoothness, (detailSmoothness < 0.0) ? 0.0 : 1.0, smoothnessDetailSpeed);
                o.Smoothness = lerp(o.Smoothness, saturate(smoothnessOverlay), detailMask);
            #endif
            #ifdef _MASKMAP
                o.Metallic = tex2D(_MaskMap, texcoords.xy).r;
                o.Metallic = lerp(_MetallicRemapMin, _MetallicRemapMax, o.Metallic);
                o.Occlusion = tex2D(_MaskMap, texcoords.xy).g;
                o.Occlusion = lerp(_AORemapMin, _AORemapMax, o.Occlusion);
            #else
                o.Metallic = _Metallic;
                o.Occlusion = 1.0;
            #endif

            #if defined(_SPECULAR_OCCLUSION_FROM_BENT_NORMAL_MAP)
                // TODO: implenent bent normals for this to work
                //o.SpecularOcclusion = GetSpecularOcclusionFromBentAO(V, bentNormalWS, d.worldSpaceNormal, o.Occlusion, PerceptualSmoothnessToRoughness(o.Smoothness));
            #elif  defined(_MASKMAP) && !defined(_SPECULAR_OCCLUSION_NONE)
                float3 V = normalize(d.worldSpaceViewDir);
                o.SpecularOcclusion = GetSpecularOcclusionFromAmbientOcclusion(ClampNdotV(dot(d.worldSpaceNormal, V)), o.Occlusion, PerceptualSmoothnessToRoughness(o.Smoothness));
            #endif

            o.DiffusionProfileHash = asuint(_DiffusionProfileHash);
            o.SubsurfaceMask = _SubsurfaceMask;
            #ifdef _SUBSURFACE_MASK_MAP
                o.SubsurfaceMask *= tex2D(_SubsurfaceMaskMap, texcoords.xy).r;
            #endif
            #ifdef _THICKNESSMAP
                o.Thickness = tex2D(_ThicknessMap, texcoords.xy).r;
                o.Thickness = _ThicknessRemap.x + _ThicknessRemap.y * surfaceData.thickness;
            #else
                o.Thickness = _Thickness;
            #endif
            #ifdef _ANISOTROPYMAP
                o.Anisotropy = tex2D(_AnisotropyMap, texcoords.xy).r;
            #else
                o.Anisotropy = 1.0;
            #endif
                o.Anisotropy *= _Anisotropy;
                o.Specular = _SpecularColor.rgb;
            #ifdef _SPECULARCOLORMAP
                o.Specular *= tex2D(_SpecularColorMap, texcoords.xy).rgb;
            #endif
            #ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
                o.Albedo *= _EnergyConservingSpecularColor > 0.0 ? (1.0 - Max3(o.Specular.r, o.Specular.g, o.Specular.b)) : 1.0;
            #endif
            #ifdef _MATERIAL_FEATURE_CLEAR_COAT
                o.CoatMask = _CoatMask;
                o.CoatMask *= tex2D(_CoatMaskMap, texcoords.xy).r;
            #else
                o.CoatMask = 0.0;
            #endif
            #ifdef _MATERIAL_FEATURE_IRIDESCENCE
                #ifdef _IRIDESCENCE_THICKNESSMAP
                o.IridescenceThickness = tex2D(_IridescenceThicknessMap, texcoords.xy).r;
                o.IridescenceThickness = _IridescenceThicknessRemap.x + _IridescenceThicknessRemap.y * o.IridescenceThickness;
                #else
                o.IridescenceThickness = _IridescenceThickness;
                #endif
                o.IridescenceMask = _IridescenceMask;
                o.IridescenceMask *= tex2D(_IridescenceMaskMap, texcoords.xy).r;
            #else
                o.IridescenceThickness = 0.0;
                o.IridescenceMask = 0.0;
            #endif


//SEE: https://github.com/Unity-Technologies/Graphics/blob/632f80e011f18ea537ee6e2f0be3ff4f4dea6a11/Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitBuiltinData.hlsl

            float3 emissiveColor = _EmissiveColor * lerp(float3(1.0, 1.0, 1.0), o.Albedo.rgb, _AlbedoAffectEmissive);
            #ifdef _EMISSIVE_COLOR_MAP                
                float2 uvEmission = _UVMappingMaskEmissive.x * d.texcoord0 +
                                    _UVMappingMaskEmissive.y * d.texcoord1 +
                                    _UVMappingMaskEmissive.z * d.texcoord2 +
                                    _UVMappingMaskEmissive.w * d.texcoord3;

                uvEmission = uvEmission * _EmissiveColorMap_ST.xy + _EmissiveColorMap_ST.zw;                     

                emissiveColor *= tex2D(_EmissiveColorMap, uvEmission).rgb;
            #endif
            o.Emission += emissiveColor;
            float currentExposureMultiplier = 0;
            #if SHADEROPTIONS_PRE_EXPOSITION
                currentExposureMultiplier =  LOAD_TEXTURE2D(_ExposureTexture, int2(0, 0)).x * _ProbeExposureScale;
            #else
                currentExposureMultiplier =  _ProbeExposureScale;
            #endif
            float InverseCurrentExposureMultiplier = 0;
            float exposure = currentExposureMultiplier;
            InverseCurrentExposureMultiplier =  rcp(exposure + (exposure == 0.0)); 
            float3 emissiveRcpExposure = o.Emission * InverseCurrentExposureMultiplier;
            o.Emission = lerp(emissiveRcpExposure, o.Emission, _EmissiveExposureWeight);


// SEE: https://github.com/Unity-Technologies/Graphics/blob/223d8105e68c5a808027d78f39cb4e2545fd6276/Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitData.hlsl
            #if defined(_ALPHATEST_ON)
                float alphaTex = tex2D(_BaseColorMap, texcoords.xy).a;
                alphaTex = lerp(_AlphaRemapMin, _AlphaRemapMax, alphaTex);
                float alphaValue = alphaTex * _BaseColor.a;

                // Perform alha test very early to save performance (a killed pixel will not sample textures)
                //#if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
                //float alphaCutoff = _AlphaCutoffPrepass;
                //#elif SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_POSTPASS
                //float alphaCutoff = _AlphaCutoffPostpass;
                //#elif (SHADERPASS == SHADERPASS_SHADOWS) || (SHADERPASS == SHADERPASS_RAYTRACING_VISIBILITY)
                //float alphaCutoff = _UseShadowThreshold ? _AlphaCutoffShadow : _AlphaCutoff;
                //#else
                float alphaCutoff = _AlphaCutoff;
                //#endif

                // GENERIC_ALPHA_TEST(alphaValue, alphaCutoff);
                clip(alpha - alphaCutoff);
            #endif

	}




    #if _REPLACEMENT
        sampler2D _DissolveTexGlobal;
    #else
        sampler2D _DissolveTex;
    #endif

    //#if _DISSOLVEMASK
    #if _REPLACEMENT
        sampler2D _DissolveMaskGlobal;
    #else
        sampler2D _DissolveMask;
    #endif
    //#endif

    #if _REPLACEMENT
        sampler2D _ObstructionCurveGlobal;
    #else
        sampler2D _ObstructionCurve;
    #endif



    #if _URP
        half2 ParallaxOffset1Step (half h, half height, half3 viewDir)
        {
            h = h * height - height/2.0;
            half3 v = normalize(viewDir);
            v.z += 0.42;
            return h * (v.xy / v.z);
        }
        #ifdef UNITY_COLORSPACE_GAMMA
            #define unity_ColorSpaceDouble half4(2.0, 2.0, 2.0, 2.0)
        #else 
            #define unity_ColorSpaceDouble half4(4.59479380, 4.59479380, 4.59479380, 2.0)
        #endif
        half3 BlendNormals(half3 n1, half3 n2)
        {
            return normalize(half3(n1.xy + n2.xy, n1.z*n2.z));
        }
    #endif


#ifdef USE_UNITY_TEXTURE_2D_TYPE
#undef USE_UNITY_TEXTURE_2D_TYPE
#endif
#include "Assets/See-through Shader/Core/Shaders/xxSharedSTSDependencies/SeeThroughShaderFunction.hlsl"



	void Ext_SurfaceFunction1 (inout Surface o, ShaderData d)
	{
        half3 albedo;
        half3 emission;
        float alphaForClipping;
#ifdef USE_UNITY_TEXTURE_2D_TYPE
#undef USE_UNITY_TEXTURE_2D_TYPE
#endif
#if _REPLACEMENT
    DoSeeThroughShading( o.Albedo, o.Normal, d.worldSpacePosition, d.worldSpaceNormal,

                        _numOfPlayersInside, _tDirection, _tValue, _id,
                        _TriggerMode, _RaycastMode,
                        _IsExempt,

                        _DissolveColorGlobal, _DissolveColorSaturationGlobal, _UVsGlobal,
                        _DissolveEmissionGlobal, _DissolveEmissionBoosterGlobal, _TexturedEmissionEdgeGlobal, _TexturedEmissionEdgeStrengthGlobal,
                        _hasClippedShadowsGlobal,

                        _ObstructionGlobal,
                        _AngleStrengthGlobal,
                        _ConeStrengthGlobal, _ConeObstructionDestroyRadiusGlobal,
                        _CylinderStrengthGlobal, _CylinderObstructionDestroyRadiusGlobal,
                        _CircleStrengthGlobal, _CircleObstructionDestroyRadiusGlobal,
                        _CurveStrengthGlobal, _CurveObstructionDestroyRadiusGlobal,
                        _DissolveFallOffGlobal,
                        _DissolveMaskEnabledGlobal,
                        _IntrinsicDissolveStrengthGlobal,
                        _DefaultEffectRadiusGlobal,

                        _IsometricExclusionGlobal, _IsometricExclusionDistanceGlobal, _IsometricExclusionGradientLengthGlobal,
                        _CeilingGlobal, _CeilingModeGlobal, _CeilingBlendModeGlobal, _CeilingYGlobal, _CeilingPlayerYOffsetGlobal, _CeilingYGradientLengthGlobal,
                        _FloorGlobal, _FloorModeGlobal, _FloorYGlobal, _PlayerPosYOffsetGlobal, _FloorYTextureGradientLengthGlobal,

                        _AnimationEnabledGlobal, _AnimationSpeedGlobal,
                        _TransitionDurationGlobal,

                        _ZoningGlobal, _ZoningModeGlobal, _IsZoningRevealableGlobal, _ZoningEdgeGradientLengthGlobal,
                        _SyncZonesWithFloorYGlobal, _SyncZonesFloorYOffsetGlobal,

                        _PreviewModeGlobal,
                        _PreviewIndicatorLineThicknessGlobal,

                        _DissolveTexGlobal,
                        _DissolveMaskGlobal,
                        _ObstructionCurveGlobal,

                        _DissolveMaskGlobal_TexelSize,
                        _ObstructionCurveGlobal_TexelSize,

                        albedo, emission, alphaForClipping);
#else 
    
    DoSeeThroughShading( o.Albedo, o.Normal, d.worldSpacePosition, d.worldSpaceNormal,

                        _numOfPlayersInside, _tDirection, _tValue, _id,
                        _TriggerMode, _RaycastMode,
                        _IsExempt,

                        _DissolveColor, _DissolveColorSaturation, _UVs,
                        _DissolveEmission, _DissolveEmissionBooster, _TexturedEmissionEdge, _TexturedEmissionEdgeStrength,
                        _hasClippedShadows,

                        _Obstruction,
                        _AngleStrength,
                        _ConeStrength, _ConeObstructionDestroyRadius,
                        _CylinderStrength, _CylinderObstructionDestroyRadius,
                        _CircleStrength, _CircleObstructionDestroyRadius,
                        _CurveStrength, _CurveObstructionDestroyRadius,
                        _DissolveFallOff,
                        _DissolveMaskEnabled,
                        _IntrinsicDissolveStrength,
                        _DefaultEffectRadius,

                        _IsometricExclusion, _IsometricExclusionDistance, _IsometricExclusionGradientLength,
                        _Ceiling, _CeilingMode, _CeilingBlendMode, _CeilingY, _CeilingPlayerYOffset, _CeilingYGradientLength,
                        _Floor, _FloorMode, _FloorY, _PlayerPosYOffset, _FloorYTextureGradientLength,

                        _AnimationEnabled, _AnimationSpeed,
                        _TransitionDuration,

                        _Zoning, _ZoningMode, _IsZoningRevealable, _ZoningEdgeGradientLength,
                        _SyncZonesWithFloorY, _SyncZonesFloorYOffset,

                        _PreviewMode,
                        _PreviewIndicatorLineThickness,                            

                        _DissolveTex,
                        _DissolveMask,
                        _ObstructionCurve,

                        _DissolveMask_TexelSize,
                        _ObstructionCurve_TexelSize,

                        albedo, emission, alphaForClipping);
#endif 



        o.Albedo = albedo;
        o.Emission += emission;   

	}




        
            void ChainSurfaceFunction(inout Surface l, inout ShaderData d)
            {
                  Ext_SurfaceFunction0(l, d);
                  Ext_SurfaceFunction1(l, d);
                 // Ext_SurfaceFunction2(l, d);
                 // Ext_SurfaceFunction3(l, d);
                 // Ext_SurfaceFunction4(l, d);
                 // Ext_SurfaceFunction5(l, d);
                 // Ext_SurfaceFunction6(l, d);
                 // Ext_SurfaceFunction7(l, d);
                 // Ext_SurfaceFunction8(l, d);
                 // Ext_SurfaceFunction9(l, d);
		           // Ext_SurfaceFunction10(l, d);
                 // Ext_SurfaceFunction11(l, d);
                 // Ext_SurfaceFunction12(l, d);
                 // Ext_SurfaceFunction13(l, d);
                 // Ext_SurfaceFunction14(l, d);
                 // Ext_SurfaceFunction15(l, d);
                 // Ext_SurfaceFunction16(l, d);
                 // Ext_SurfaceFunction17(l, d);
                 // Ext_SurfaceFunction18(l, d);
		           // Ext_SurfaceFunction19(l, d);
                 // Ext_SurfaceFunction20(l, d);
                 // Ext_SurfaceFunction21(l, d);
                 // Ext_SurfaceFunction22(l, d);
                 // Ext_SurfaceFunction23(l, d);
                 // Ext_SurfaceFunction24(l, d);
                 // Ext_SurfaceFunction25(l, d);
                 // Ext_SurfaceFunction26(l, d);
                 // Ext_SurfaceFunction27(l, d);
                 // Ext_SurfaceFunction28(l, d);
		           // Ext_SurfaceFunction29(l, d);
            }

            void ChainModifyVertex(inout VertexData v, inout VertexToPixel v2p, float4 time)
            {
                 ExtraV2F d;
                 
                 ZERO_INITIALIZE(ExtraV2F, d);
                 ZERO_INITIALIZE(Blackboard, d.blackboard);
                 // due to motion vectors in HDRP, we need to use the last
                 // time in certain spots. So if you are going to use _Time to adjust vertices,
                 // you need to use this time or motion vectors will break. 
                 d.time = time;

                 //  Ext_ModifyVertex0(v, d);
                 // Ext_ModifyVertex1(v, d);
                 // Ext_ModifyVertex2(v, d);
                 // Ext_ModifyVertex3(v, d);
                 // Ext_ModifyVertex4(v, d);
                 // Ext_ModifyVertex5(v, d);
                 // Ext_ModifyVertex6(v, d);
                 // Ext_ModifyVertex7(v, d);
                 // Ext_ModifyVertex8(v, d);
                 // Ext_ModifyVertex9(v, d);
                 // Ext_ModifyVertex10(v, d);
                 // Ext_ModifyVertex11(v, d);
                 // Ext_ModifyVertex12(v, d);
                 // Ext_ModifyVertex13(v, d);
                 // Ext_ModifyVertex14(v, d);
                 // Ext_ModifyVertex15(v, d);
                 // Ext_ModifyVertex16(v, d);
                 // Ext_ModifyVertex17(v, d);
                 // Ext_ModifyVertex18(v, d);
                 // Ext_ModifyVertex19(v, d);
                 // Ext_ModifyVertex20(v, d);
                 // Ext_ModifyVertex21(v, d);
                 // Ext_ModifyVertex22(v, d);
                 // Ext_ModifyVertex23(v, d);
                 // Ext_ModifyVertex24(v, d);
                 // Ext_ModifyVertex25(v, d);
                 // Ext_ModifyVertex26(v, d);
                 // Ext_ModifyVertex27(v, d);
                 // Ext_ModifyVertex28(v, d);
                 // Ext_ModifyVertex29(v, d);


                 // #if %EXTRAV2F0REQUIREKEY%
                 // v2p.extraV2F0 = d.extraV2F0;
                 // #endif

                 // #if %EXTRAV2F1REQUIREKEY%
                 // v2p.extraV2F1 = d.extraV2F1;
                 // #endif

                 // #if %EXTRAV2F2REQUIREKEY%
                 // v2p.extraV2F2 = d.extraV2F2;
                 // #endif

                 // #if %EXTRAV2F3REQUIREKEY%
                 // v2p.extraV2F3 = d.extraV2F3;
                 // #endif

                 // #if %EXTRAV2F4REQUIREKEY%
                 // v2p.extraV2F4 = d.extraV2F4;
                 // #endif

                 // #if %EXTRAV2F5REQUIREKEY%
                 // v2p.extraV2F5 = d.extraV2F5;
                 // #endif

                 // #if %EXTRAV2F6REQUIREKEY%
                 // v2p.extraV2F6 = d.extraV2F6;
                 // #endif

                 // #if %EXTRAV2F7REQUIREKEY%
                 // v2p.extraV2F7 = d.extraV2F7;
                 // #endif
            }

            void ChainModifyTessellatedVertex(inout VertexData v, inout VertexToPixel v2p)
            {
               ExtraV2F d;
               ZERO_INITIALIZE(ExtraV2F, d);
               ZERO_INITIALIZE(Blackboard, d.blackboard);

               // #if %EXTRAV2F0REQUIREKEY%
               // d.extraV2F0 = v2p.extraV2F0;
               // #endif

               // #if %EXTRAV2F1REQUIREKEY%
               // d.extraV2F1 = v2p.extraV2F1;
               // #endif

               // #if %EXTRAV2F2REQUIREKEY%
               // d.extraV2F2 = v2p.extraV2F2;
               // #endif

               // #if %EXTRAV2F3REQUIREKEY%
               // d.extraV2F3 = v2p.extraV2F3;
               // #endif

               // #if %EXTRAV2F4REQUIREKEY%
               // d.extraV2F4 = v2p.extraV2F4;
               // #endif

               // #if %EXTRAV2F5REQUIREKEY%
               // d.extraV2F5 = v2p.extraV2F5;
               // #endif

               // #if %EXTRAV2F6REQUIREKEY%
               // d.extraV2F6 = v2p.extraV2F6;
               // #endif

               // #if %EXTRAV2F7REQUIREKEY%
               // d.extraV2F7 = v2p.extraV2F7;
               // #endif


               // Ext_ModifyTessellatedVertex0(v, d);
               // Ext_ModifyTessellatedVertex1(v, d);
               // Ext_ModifyTessellatedVertex2(v, d);
               // Ext_ModifyTessellatedVertex3(v, d);
               // Ext_ModifyTessellatedVertex4(v, d);
               // Ext_ModifyTessellatedVertex5(v, d);
               // Ext_ModifyTessellatedVertex6(v, d);
               // Ext_ModifyTessellatedVertex7(v, d);
               // Ext_ModifyTessellatedVertex8(v, d);
               // Ext_ModifyTessellatedVertex9(v, d);
               // Ext_ModifyTessellatedVertex10(v, d);
               // Ext_ModifyTessellatedVertex11(v, d);
               // Ext_ModifyTessellatedVertex12(v, d);
               // Ext_ModifyTessellatedVertex13(v, d);
               // Ext_ModifyTessellatedVertex14(v, d);
               // Ext_ModifyTessellatedVertex15(v, d);
               // Ext_ModifyTessellatedVertex16(v, d);
               // Ext_ModifyTessellatedVertex17(v, d);
               // Ext_ModifyTessellatedVertex18(v, d);
               // Ext_ModifyTessellatedVertex19(v, d);
               // Ext_ModifyTessellatedVertex20(v, d);
               // Ext_ModifyTessellatedVertex21(v, d);
               // Ext_ModifyTessellatedVertex22(v, d);
               // Ext_ModifyTessellatedVertex23(v, d);
               // Ext_ModifyTessellatedVertex24(v, d);
               // Ext_ModifyTessellatedVertex25(v, d);
               // Ext_ModifyTessellatedVertex26(v, d);
               // Ext_ModifyTessellatedVertex27(v, d);
               // Ext_ModifyTessellatedVertex28(v, d);
               // Ext_ModifyTessellatedVertex29(v, d);

               // #if %EXTRAV2F0REQUIREKEY%
               // v2p.extraV2F0 = d.extraV2F0;
               // #endif

               // #if %EXTRAV2F1REQUIREKEY%
               // v2p.extraV2F1 = d.extraV2F1;
               // #endif

               // #if %EXTRAV2F2REQUIREKEY%
               // v2p.extraV2F2 = d.extraV2F2;
               // #endif

               // #if %EXTRAV2F3REQUIREKEY%
               // v2p.extraV2F3 = d.extraV2F3;
               // #endif

               // #if %EXTRAV2F4REQUIREKEY%
               // v2p.extraV2F4 = d.extraV2F4;
               // #endif

               // #if %EXTRAV2F5REQUIREKEY%
               // v2p.extraV2F5 = d.extraV2F5;
               // #endif

               // #if %EXTRAV2F6REQUIREKEY%
               // v2p.extraV2F6 = d.extraV2F6;
               // #endif

               // #if %EXTRAV2F7REQUIREKEY%
               // v2p.extraV2F7 = d.extraV2F7;
               // #endif
            }

            void ChainFinalColorForward(inout Surface l, inout ShaderData d, inout half4 color)
            {
               //   Ext_FinalColorForward0(l, d, color);
               //   Ext_FinalColorForward1(l, d, color);
               //   Ext_FinalColorForward2(l, d, color);
               //   Ext_FinalColorForward3(l, d, color);
               //   Ext_FinalColorForward4(l, d, color);
               //   Ext_FinalColorForward5(l, d, color);
               //   Ext_FinalColorForward6(l, d, color);
               //   Ext_FinalColorForward7(l, d, color);
               //   Ext_FinalColorForward8(l, d, color);
               //   Ext_FinalColorForward9(l, d, color);
               //  Ext_FinalColorForward10(l, d, color);
               //  Ext_FinalColorForward11(l, d, color);
               //  Ext_FinalColorForward12(l, d, color);
               //  Ext_FinalColorForward13(l, d, color);
               //  Ext_FinalColorForward14(l, d, color);
               //  Ext_FinalColorForward15(l, d, color);
               //  Ext_FinalColorForward16(l, d, color);
               //  Ext_FinalColorForward17(l, d, color);
               //  Ext_FinalColorForward18(l, d, color);
               //  Ext_FinalColorForward19(l, d, color);
               //  Ext_FinalColorForward20(l, d, color);
               //  Ext_FinalColorForward21(l, d, color);
               //  Ext_FinalColorForward22(l, d, color);
               //  Ext_FinalColorForward23(l, d, color);
               //  Ext_FinalColorForward24(l, d, color);
               //  Ext_FinalColorForward25(l, d, color);
               //  Ext_FinalColorForward26(l, d, color);
               //  Ext_FinalColorForward27(l, d, color);
               //  Ext_FinalColorForward28(l, d, color);
               //  Ext_FinalColorForward29(l, d, color);
            }

            void ChainFinalGBufferStandard(inout Surface s, inout ShaderData d, inout half4 GBuffer0, inout half4 GBuffer1, inout half4 GBuffer2, inout half4 outEmission, inout half4 outShadowMask)
            {
               //   Ext_FinalGBufferStandard0(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard1(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard2(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard3(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard4(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard5(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard6(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard7(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard8(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard9(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard10(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard11(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard12(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard13(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard14(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard15(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard16(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard17(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard18(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard19(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard20(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard21(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard22(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard23(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard24(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard25(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard26(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard27(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard28(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard29(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
            }



            

         ShaderData CreateShaderData(VertexToPixel i
                  #if NEED_FACING
                     , bool facing
                  #endif
         )
         {
            ShaderData d = (ShaderData)0;
            d.clipPos = i.pos;
            d.worldSpacePosition = i.worldPos;

            d.worldSpaceNormal = normalize(i.worldNormal);
            d.worldSpaceTangent = normalize(i.worldTangent.xyz);
            d.tangentSign = i.worldTangent.w;
            float3 bitangent = cross(i.worldTangent.xyz, i.worldNormal) * d.tangentSign * -1;
            

            d.TBNMatrix = float3x3(d.worldSpaceTangent, bitangent, d.worldSpaceNormal);
            d.worldSpaceViewDir = normalize(_WorldSpaceCameraPos - i.worldPos);

            d.tangentSpaceViewDir = mul(d.TBNMatrix, d.worldSpaceViewDir);
             d.texcoord0 = i.texcoord0;
             d.texcoord1 = i.texcoord1;
             d.texcoord2 = i.texcoord2;

            // #if %TEXCOORD3REQUIREKEY%
             d.texcoord3 = i.texcoord3;
            // #endif

            // d.isFrontFace = facing;
            // #if %VERTEXCOLORREQUIREKEY%
            // d.vertexColor = i.vertexColor;
            // #endif

            // these rarely get used, so we back transform them. Usually will be stripped.
            #if _HDRP
                // d.localSpacePosition = mul(unity_WorldToObject, float4(GetCameraRelativePositionWS(i.worldPos), 1)).xyz;
            #else
                // d.localSpacePosition = mul(unity_WorldToObject, float4(i.worldPos, 1)).xyz;
            #endif
            // d.localSpaceNormal = normalize(mul((float3x3)unity_WorldToObject, i.worldNormal));
            // d.localSpaceTangent = normalize(mul((float3x3)unity_WorldToObject, i.worldTangent.xyz));

            // #if %SCREENPOSREQUIREKEY%
            // d.screenPos = i.screenPos;
            // d.screenUV = (i.screenPos.xy / i.screenPos.w);
            // #endif


            // #if %EXTRAV2F0REQUIREKEY%
            // d.extraV2F0 = i.extraV2F0;
            // #endif

            // #if %EXTRAV2F1REQUIREKEY%
            // d.extraV2F1 = i.extraV2F1;
            // #endif

            // #if %EXTRAV2F2REQUIREKEY%
            // d.extraV2F2 = i.extraV2F2;
            // #endif

            // #if %EXTRAV2F3REQUIREKEY%
            // d.extraV2F3 = i.extraV2F3;
            // #endif

            // #if %EXTRAV2F4REQUIREKEY%
            // d.extraV2F4 = i.extraV2F4;
            // #endif

            // #if %EXTRAV2F5REQUIREKEY%
            // d.extraV2F5 = i.extraV2F5;
            // #endif

            // #if %EXTRAV2F6REQUIREKEY%
            // d.extraV2F6 = i.extraV2F6;
            // #endif

            // #if %EXTRAV2F7REQUIREKEY%
            // d.extraV2F7 = i.extraV2F7;
            // #endif

            return d;
         }
         

            

struct VaryingsToPS
{
   VertexToPixel vmesh;
   #ifdef VARYINGS_NEED_PASS
      VaryingsPassToPS vpass;
   #endif
};

struct PackedVaryingsToPS
{
   #ifdef VARYINGS_NEED_PASS
      PackedVaryingsPassToPS vpass;
   #endif
   VertexToPixel vmesh;

   UNITY_VERTEX_OUTPUT_STEREO
};

PackedVaryingsToPS PackVaryingsToPS(VaryingsToPS input)
{
   PackedVaryingsToPS output = (PackedVaryingsToPS)0;
   output.vmesh = input.vmesh;
   #ifdef VARYINGS_NEED_PASS
      output.vpass = PackVaryingsPassToPS(input.vpass);
   #endif

   UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
   return output;
}




VertexToPixel VertMesh(VertexData input)
{
    VertexToPixel output = (VertexToPixel)0;

    UNITY_SETUP_INSTANCE_ID(input);
    UNITY_TRANSFER_INSTANCE_ID(input, output);

    
    ChainModifyVertex(input, output, _Time);


    // This return the camera relative position (if enable)
    float3 positionRWS = TransformObjectToWorld(input.vertex.xyz);
    float3 normalWS = TransformObjectToWorldNormal(input.normal);
    float4 tangentWS = float4(TransformObjectToWorldDir(input.tangent.xyz), input.tangent.w);


    output.worldPos = GetAbsolutePositionWS(positionRWS);
    output.pos = TransformWorldToHClip(positionRWS);
    output.worldNormal = normalWS;
    output.worldTangent = tangentWS;


    output.texcoord0 = input.texcoord0;
    output.texcoord1 = input.texcoord1;
    output.texcoord2 = input.texcoord2;
    // #if %TEXCOORD3REQUIREKEY%
     output.texcoord3 = input.texcoord3;
    // #endif

    // #if %SCREENPOSREQUIREKEY%
    // output.screenPos = ComputeScreenPos(output.pos, _ProjectionParams.x);
    // #endif

    // #if %VERTEXCOLORREQUIREKEY%
    // output.vertexColor = input.vertexColor;
    // #endif
    return output;
}


#if (SHADERPASS == SHADERPASS_DBUFFER_MESH)
void MeshDecalsPositionZBias(inout VaryingsToPS input)
{
#if defined(UNITY_REVERSED_Z)
    input.vmesh.pos.z -= _DecalMeshDepthBias;
#else
    input.vmesh.pos.z += _DecalMeshDepthBias;
#endif
}
#endif


#if (SHADERPASS == SHADERPASS_LIGHT_TRANSPORT)

// This was not in constant buffer in original unity, so keep outiside. But should be in as ShaderRenderPass frequency
float unity_OneOverOutputBoost;
float unity_MaxOutputValue;

CBUFFER_START(UnityMetaPass)
// x = use uv1 as raster position
// y = use uv2 as raster position
bool4 unity_MetaVertexControl;

// x = return albedo
// y = return normal
bool4 unity_MetaFragmentControl;
CBUFFER_END

PackedVaryingsToPS Vert(VertexData inputMesh)
{
    VaryingsToPS output = (VaryingsToPS)0;
    output.vmesh = (VertexToPixel)0;

    UNITY_SETUP_INSTANCE_ID(inputMesh);
    UNITY_TRANSFER_INSTANCE_ID(inputMesh, output.vmesh);

    // Output UV coordinate in vertex shader
    float2 uv = float2(0.0, 0.0);

    if (unity_MetaVertexControl.x)
    {
        uv = inputMesh.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
    }
    else if (unity_MetaVertexControl.y)
    {
        uv = inputMesh.texcoord2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
    }

    // OpenGL right now needs to actually use the incoming vertex position
    // so we create a fake dependency on it here that haven't any impact.
    output.vmesh.pos = float4(uv * 2.0 - 1.0, inputMesh.vertex.z > 0 ? 1.0e-4 : 0.0, 1.0);

#ifdef VARYINGS_NEED_POSITION_WS
    output.vmesh.worldPos = TransformObjectToWorld(inputMesh.vertex.xyz);
#endif

#ifdef VARYINGS_NEED_TANGENT_TO_WORLD
    // Normal is required for triplanar mapping
    output.vmesh.worldNormal = TransformObjectToWorldNormal(inputMesh.normal);
    // Not required but assign to silent compiler warning
    output.vmesh.worldTangent = float4(1.0, 0.0, 0.0, 0.0);
#endif

    output.vmesh.texcoord0 = inputMesh.texcoord0;
    output.vmesh.texcoord1 = inputMesh.texcoord1;
    output.vmesh.texcoord2 = inputMesh.texcoord2;
    // #if %TEXCOORD3REQUIREKEY%
     output.vmesh.texcoord3 = inputMesh.texcoord3;
    // #endif

    // #if %VERTEXCOLORREQUIREKEY%
    // output.vmesh.vertexColor = inputMesh.vertexColor;
    // #endif

    return PackVaryingsToPS(output);
}
#else

PackedVaryingsToPS Vert(VertexData inputMesh)
{
    VaryingsToPS varyingsType;
    varyingsType.vmesh = VertMesh(inputMesh);
    #if (SHADERPASS == SHADERPASS_DBUFFER_MESH)
       MeshDecalsPositionZBias(varyingsType);
    #endif
    return PackVaryingsToPS(varyingsType);
}

#endif



            

            
                FragInputs BuildFragInputs(VertexToPixel input)
                {
                    UNITY_SETUP_INSTANCE_ID(input);
                    FragInputs output;
                    ZERO_INITIALIZE(FragInputs, output);
            
                    // Init to some default value to make the computer quiet (else it output 'divide by zero' warning even if value is not used).
                    // TODO: this is a really poor workaround, but the variable is used in a bunch of places
                    // to compute normals which are then passed on elsewhere to compute other values...
                    output.tangentToWorld = k_identity3x3;
                    output.positionSS = input.pos;       // input.positionCS is SV_Position
            
                    output.positionRWS = GetCameraRelativePositionWS(input.worldPos);
                    output.tangentToWorld = BuildTangentToWorld(input.worldTangent, input.worldNormal);
                    output.texCoord0 = input.texcoord0;
                    output.texCoord1 = input.texcoord1;
                    output.texCoord2 = input.texcoord2;
            
                    return output;
                }
            
               void BuildSurfaceData(FragInputs fragInputs, inout Surface surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData, out float3 bentNormalWS)
               {
                   // setup defaults -- these are used if the graph doesn't output a value
                   ZERO_INITIALIZE(SurfaceData, surfaceData);
        
                   // specularOcclusion need to be init ahead of decal to quiet the compiler that modify the SurfaceData struct
                   // however specularOcclusion can come from the graph, so need to be init here so it can be override.
                   surfaceData.specularOcclusion = 1.0;
        
                   // copy across graph values, if defined
                   surfaceData.baseColor =                 surfaceDescription.Albedo;
                   surfaceData.perceptualSmoothness =      surfaceDescription.Smoothness;
                   surfaceData.ambientOcclusion =          surfaceDescription.Occlusion;
                   surfaceData.specularOcclusion =         surfaceDescription.SpecularOcclusion;
                   surfaceData.metallic =                  surfaceDescription.Metallic;
                   surfaceData.subsurfaceMask =            surfaceDescription.SubsurfaceMask;
                   surfaceData.thickness =                 surfaceDescription.Thickness;
                   surfaceData.diffusionProfileHash =      asuint(surfaceDescription.DiffusionProfileHash);
                   #if _USESPECULAR
                      surfaceData.specularColor =             surfaceDescription.Specular;
                   #endif
                   surfaceData.coatMask =                  surfaceDescription.CoatMask;
                   surfaceData.anisotropy =                surfaceDescription.Anisotropy;
                   surfaceData.iridescenceMask =           surfaceDescription.IridescenceMask;
                   surfaceData.iridescenceThickness =      surfaceDescription.IridescenceThickness;
        
           #ifdef _HAS_REFRACTION
                   if (_EnableSSRefraction)
                   {
                       // surfaceData.ior =                       surfaceDescription.RefractionIndex;
                       // surfaceData.transmittanceColor =        surfaceDescription.RefractionColor;
                       // surfaceData.atDistance =                surfaceDescription.RefractionDistance;
        
                       surfaceData.transmittanceMask = (1.0 - surfaceDescription.Alpha);
                       surfaceDescription.Alpha = 1.0;
                   }
                   else
                   {
                       surfaceData.ior = surfaceDescription.ior;
                       surfaceData.transmittanceColor = surfaceDescription.transmittanceColor;
                       surfaceData.atDistance = surfaceDescription.atDistance;
                       surfaceData.transmittanceMask = surfaceDescription.transmittanceMask;
                       surfaceDescription.Alpha = 1.0;
                   }
           #else
                   surfaceData.ior = 1.0;
                   surfaceData.transmittanceColor = float3(1.0, 1.0, 1.0);
                   surfaceData.atDistance = 1.0;
                   surfaceData.transmittanceMask = 0.0;
           #endif
                
                   // These static material feature allow compile time optimization
                   surfaceData.materialFeatures = MATERIALFEATUREFLAGS_LIT_STANDARD;
           #ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
                   surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SUBSURFACE_SCATTERING;
           #endif
           #ifdef _MATERIAL_FEATURE_TRANSMISSION
                   surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_TRANSMISSION;
           #endif
           #ifdef _MATERIAL_FEATURE_ANISOTROPY
                   surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_ANISOTROPY;
           #endif
                   // surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_CLEAR_COAT;
        
           #ifdef _MATERIAL_FEATURE_IRIDESCENCE
                   surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_IRIDESCENCE;
           #endif
           #ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
                   surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SPECULAR_COLOR;
           #endif
        
           #if defined (_MATERIAL_FEATURE_SPECULAR_COLOR) && defined (_ENERGY_CONSERVING_SPECULAR)
                   // Require to have setup baseColor
                   // Reproduce the energy conservation done in legacy Unity. Not ideal but better for compatibility and users can unchek it
                   surfaceData.baseColor *= (1.0 - Max3(surfaceData.specularColor.r, surfaceData.specularColor.g, surfaceData.specularColor.b));
           #endif

        
                   // tangent-space normal
                   float3 normalTS = float3(0.0f, 0.0f, 1.0f);
                   normalTS = surfaceDescription.Normal;
        
                   // compute world space normal
                   #if !_WORLDSPACENORMAL
                      surfaceData.normalWS = mul(normalTS, fragInputs.tangentToWorld);
                   #else
                      surfaceData.normalWS = normalTS;  
                   #endif
                   surfaceData.geomNormalWS = fragInputs.tangentToWorld[2];
        
                   surfaceData.tangentWS = normalize(fragInputs.tangentToWorld[0].xyz);    // The tangent is not normalize in tangentToWorld for mikkt. TODO: Check if it expected that we normalize with Morten. Tag: SURFACE_GRADIENT
                   // surfaceData.tangentWS = TransformTangentToWorld(surfaceDescription.Tangent, fragInputs.tangentToWorld);
        
           #if HAVE_DECALS
                   if (_EnableDecals)
                   {
                       #if VERSION_GREATER_EQUAL(10,2)
                          DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput,  surfaceData.geomNormalWS, surfaceDescription.Alpha);
                          ApplyDecalToSurfaceData(decalSurfaceData,  surfaceData.geomNormalWS, surfaceData);
                       #else
                          DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, surfaceDescription.Alpha);
                          ApplyDecalToSurfaceData(decalSurfaceData, surfaceData);
                       #endif
                   }
           #endif
        
                   bentNormalWS = surfaceData.normalWS;
               
                   surfaceData.tangentWS = Orthonormalize(surfaceData.tangentWS, surfaceData.normalWS);
        
        
                   // By default we use the ambient occlusion with Tri-ace trick (apply outside) for specular occlusion.
                   // If user provide bent normal then we process a better term
           #if defined(_SPECULAR_OCCLUSION_CUSTOM)
                   // Just use the value passed through via the slot (not active otherwise)
           #elif defined(_SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL)
                   // If we have bent normal and ambient occlusion, process a specular occlusion
                   surfaceData.specularOcclusion = GetSpecularOcclusionFromBentAO(V, bentNormalWS, surfaceData.normalWS, surfaceData.ambientOcclusion, PerceptualSmoothnessToPerceptualRoughness(surfaceData.perceptualSmoothness));
           #elif defined(_AMBIENT_OCCLUSION) && defined(_SPECULAR_OCCLUSION_FROM_AO)
                   surfaceData.specularOcclusion = GetSpecularOcclusionFromAmbientOcclusion(ClampNdotV(dot(surfaceData.normalWS, V)), surfaceData.ambientOcclusion, PerceptualSmoothnessToRoughness(surfaceData.perceptualSmoothness));
           #endif
        
           #ifdef _ENABLE_GEOMETRIC_SPECULAR_AA
                   surfaceData.perceptualSmoothness = GeometricNormalFiltering(surfaceData.perceptualSmoothness, fragInputs.tangentToWorld[2], surfaceDescription.SpecularAAScreenSpaceVariance, surfaceDescription.SpecularAAThreshold);
           #endif
        
           #ifdef DEBUG_DISPLAY
                   if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
                   {
                       // TODO: need to update mip info
                       surfaceData.metallic = 0;
                   }
        
                   // We need to call ApplyDebugToSurfaceData after filling the surfarcedata and before filling builtinData
                   // as it can modify attribute use for static lighting
                   ApplyDebugToSurfaceData(fragInputs.tangentToWorld, surfaceData);
           #endif
               }
        
               void GetSurfaceAndBuiltinData(VertexToPixel m2ps, FragInputs fragInputs, float3 V, inout PositionInputs posInput,
                     out SurfaceData surfaceData, out BuiltinData builtinData, inout Surface l, inout ShaderData d
                     #if NEED_FACING
                        , bool facing
                     #endif
                  )
               {
                 // Removed since crossfade does not work, probably needs extra material setup.   
                 //#ifdef LOD_FADE_CROSSFADE // enable dithering LOD transition if user select CrossFade transition in LOD group
                 //    uint3 fadeMaskSeed = asuint((int3)(V * _ScreenSize.xyx)); // Quantize V to _ScreenSize values
                 //    LODDitheringTransition(fadeMaskSeed, unity_LODFade.x);
                 //#endif
        
                 d = CreateShaderData(m2ps
                  #if NEED_FACING
                    , facing
                  #endif
                 );

                 

                 l = (Surface)0;

                 l.Albedo = half3(0.5, 0.5, 0.5);
                 l.Normal = float3(0,0,1);
                 l.Occlusion = 1;
                 l.Alpha = 1;
                 l.SpecularOcclusion = 1;

                 #ifdef _DEPTHOFFSET_ON
                    l.outputDepth = posInput.deviceDepth;
                 #endif

                 ChainSurfaceFunction(l, d);

                 #ifdef _DEPTHOFFSET_ON
                    posInput.deviceDepth = l.outputDepth;
                 #endif

                 #if _UNLIT
                     //l.Emission = l.Albedo;
                     //l.Albedo = 0;
                     l.Normal = half3(0,0,1);
                     l.Occlusion = 1;
                     l.Metallic = 0;
                     l.Specular = 0;
                 #endif

                 surfaceData.geomNormalWS = d.worldSpaceNormal;
                 surfaceData.tangentWS = d.worldSpaceTangent;
                 fragInputs.tangentToWorld = d.TBNMatrix;

                 float3 bentNormalWS;
                 BuildSurfaceData(fragInputs, l, V, posInput, surfaceData, bentNormalWS);

                 InitBuiltinData(posInput, l.Alpha, bentNormalWS, -d.worldSpaceNormal, fragInputs.texCoord1, fragInputs.texCoord2, builtinData);

                 

                 builtinData.emissiveColor = l.Emission;

                 #if defined(_OVERRIDE_BAKEDGI)
                    builtinData.bakeDiffuseLighting = l.DiffuseGI;
                    builtinData.backBakeDiffuseLighting = l.BackDiffuseGI;
                    builtinData.emissiveColor += l.SpecularGI;
                 #endif
                 
                 #if defined(_OVERRIDE_SHADOWMASK)
                   builtinData.shadowMask0 = l.ShadowMask.x;
                   builtinData.shadowMask1 = l.ShadowMask.y;
                   builtinData.shadowMask2 = l.ShadowMask.z;
                   builtinData.shadowMask3 = l.ShadowMask.w;
                #endif
        
                 #if (SHADERPASS == SHADERPASS_DISTORTION)
                     //builtinData.distortion = surfaceDescription.Distortion;
                     //builtinData.distortionBlur = surfaceDescription.DistortionBlur;
                     builtinData.distortion = float2(0.0, 0.0);
                     builtinData.distortionBlur = 0.0;
                 #else
                     builtinData.distortion = float2(0.0, 0.0);
                     builtinData.distortionBlur = 0.0;
                 #endif
        
                   PostInitBuiltinData(V, posInput, surfaceData, builtinData);
               }


            void Frag(  PackedVaryingsToPS packedInput
            #ifdef WRITE_NORMAL_BUFFER
            , out float4 outNormalBuffer : SV_Target0
                #ifdef WRITE_MSAA_DEPTH
                , out float1 depthColor : SV_Target1
                #endif
            #elif defined(WRITE_MSAA_DEPTH) // When only WRITE_MSAA_DEPTH is define and not WRITE_NORMAL_BUFFER it mean we are Unlit and only need depth, but we still have normal buffer binded
            , out float4 outNormalBuffer : SV_Target0
            , out float1 depthColor : SV_Target1
            #elif defined(SCENESELECTIONPASS)
            , out float4 outColor : SV_Target0
            #endif

            #ifdef _DEPTHOFFSET_ON
            , out float outputDepth : SV_Depth
            #endif
            #if NEED_FACING
            , bool facing : SV_IsFrontFace
            #endif
        )
         {
             UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(packedInput);
             FragInputs input = BuildFragInputs(packedInput.vmesh);

             // input.positionSS is SV_Position
             PositionInputs posInput = GetPositionInput(input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS);

             float3 V = GetWorldSpaceNormalizeViewDir(input.positionRWS);

            SurfaceData surfaceData;
            BuiltinData builtinData;
            Surface l;
            ShaderData d;
            GetSurfaceAndBuiltinData(packedInput.vmesh, input, V, posInput, surfaceData, builtinData, l, d
               #if NEED_FACING
                  , facing
               #endif
            );

            // to prevent stripping
            surfaceData.normalWS *= saturate(l.Albedo.r + 9999);



         #ifdef _DEPTHOFFSET_ON
             outputDepth = posInput.deviceDepth;
         #endif

         #ifdef WRITE_NORMAL_BUFFER
             EncodeIntoNormalBuffer(ConvertSurfaceDataToNormalData(surfaceData), posInput.positionSS, outNormalBuffer);
             #ifdef WRITE_MSAA_DEPTH
             // In case we are rendering in MSAA, reading the an MSAA depth buffer is way too expensive. To avoid that, we export the depth to a color buffer
             depthColor = packedInput.vmesh.pos.z;
             #endif
         #elif defined(WRITE_MSAA_DEPTH) // When we are MSAA depth only without normal buffer
             // Due to the binding order of these two render targets, we need to have them both declared
             outNormalBuffer = float4(0.0, 0.0, 0.0, 1.0);
             // In case we are rendering in MSAA, reading the an MSAA depth buffer is way too expensive. To avoid that, we export the depth to a color buffer
             depthColor = packedInput.vmesh.pos.z;
         #elif defined(SCENESELECTIONPASS)
             // We use depth prepass for scene selection in the editor, this code allow to output the outline correctly
             outColor = float4(_ObjectId, _PassValue, 1.0, 1.0);
         #endif
         }

         ENDHLSL
    }


      





   }
   
   
   CustomEditor "ShaderCrew.SeeThroughShader.StandardLitSeeThroughShaderEditor"
}
