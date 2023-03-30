int _ArrayLength = 0;
#if (defined(SHADER_API_GLES) || defined(SHADER_API_GLES3)) 
        float4 _PlayersPosVectorArray[20];
        float _PlayersDataFloatArray[150];     
#else
float4 _PlayersPosVectorArray[100];
float _PlayersDataFloatArray[500];
#endif

#if defined(_ZONING)
    #if (defined(SHADER_API_GLES) || defined(SHADER_API_GLES3)) 
        float _ZDFA[500];
    #else
        float _ZDFA[1000];
    #endif
    float _ZonesDataCount;
#endif

#if defined(_REPLACEMENT)        
        half4 _DissolveColorGlobal;
        float _DissolveColorSaturationGlobal;
        float _DissolveEmissionGlobal;
        float _DissolveEmissionBoosterGlobal;
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

        sampler2D _DissolveTexGlobal;
        sampler2D _DissolveMaskGlobal;
        sampler2D _ObstructionCurveGlobal;
#endif


struct ShaderData
{
    float3 worldSpaceNormal;
    float3 worldSpacePosition;
};

struct Surface
{
    float3 Normal;
    half3 Emission;
    half3 Albedo;
};

#if !defined(_REPLACEMENT)
    #ifndef USE_UNITY_TEXTURE_2D_TYPE
        #define USE_UNITY_TEXTURE_2D_TYPE
    #endif
#else
    #undef USE_UNITY_TEXTURE_2D_TYPE
#endif




#include "Packages/com.shadercrew.seethroughshader.core/Scripts/Shaders/xxSharedSTSDependencies/SeeThroughShaderFunction.hlsl"





void DoSeeThroughShaderEffect_float(float3 inColor,
                                    float3 tangentNormal, float3 worldPosition,
                                    float3 worldNormal,

                                    float _numOfPlayersInside, float _tDirection, float _tValue, float _id,
                                    float _TriggerMode, float _RaycastMode,
                                    float _IsExempt,

                                    float4 _DissolveColor, float _DissolveColorSaturation, float _UVs,
                                    float _DissolveEmission, float _DissolveEmissionBooster, float _TexturedEmissionEdge, float _TexturedEmissionEdgeStrength,
                                    bool _hasClippedShadows,

                                    float _InteractionMode,

                                    float _Obstruction,
                                    float _AngleStrength,
                                    float _ConeStrength, float _ConeObstructionDestroyRadius,
                                    float _CylinderStrength, float _CylinderObstructionDestroyRadius,
                                    float _CircleStrength, float _CircleObstructionDestroyRadius,
                                    float _CurveStrength, float _CurveObstructionDestroyRadius,
                                    float _DissolveFallOff,
                                    float _DissolveMaskEnabled,
                                    float _IntrinsicDissolveStrength,
                                    float _DefaultEffectRadius,

                                    float _IsometricExclusion, float _IsometricExclusionDistance, float _IsometricExclusionGradientLength,
                                    float _Ceiling, float _CeilingMode, float _CeilingBlendMode, float _CeilingY, float _CeilingPlayerYOffset, float _CeilingYGradientLength,
                                    float _Floor, float _FloorMode, float _FloorY, float _PlayerPosYOffset, float _FloorYTextureGradientLength,

                                    float _AnimationEnabled, float _AnimationSpeed,
                                    float _TransitionDuration,

                                    float _Zoning, float _ZoningMode, bool _IsZoningRevealable, float _ZoningEdgeGradientLength,
                                    float _SyncZonesWithFloorY, float _SyncZonesFloorYOffset,

                                    float _PreviewMode,
                                    float _PreviewIndicatorLineThickness,

                                    float _isReferenceMaterial,

                                    UnityTexture2D _DissolveTex,
                                    UnityTexture2D _DissolveMask,
                                    UnityTexture2D _ObstructionCurve,  

                                    out half3 albedo, out half3 emission, out float alphaForClipping
)
{
    
#if defined(NO_STS_CLIPPING)
    #undef NO_STS_CLIPPING
#endif
    
#if defined(_REPLACEMENT)
    DoSeeThroughShading(inColor, tangentNormal, worldPosition, worldNormal,

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
    
    DoSeeThroughShading(inColor, tangentNormal, worldPosition, worldNormal,

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

                                    albedo, emission, alphaForClipping);
#endif 

//#undef USE_UNITY_TEXTURE_2D_TYPE      

}






void DoSeeThroughShaderEffectAlphaOnly_float(float3 inColor,
                                    float3 tangentNormal, float3 worldPosition,
                                    float3 worldNormal,

                                    float _numOfPlayersInside, float _tDirection, float _tValue, float _id,
                                    float _TriggerMode, float _RaycastMode,
                                    float _IsExempt,

                                    float4 _DissolveColor, float _DissolveColorSaturation, float _UVs,
                                    float _DissolveEmission, float _DissolveEmissionBooster, float _TexturedEmissionEdge, float _TexturedEmissionEdgeStrength,
                                    bool _hasClippedShadows,

                                    float _InteractionMode,

                                    float _Obstruction,
                                    float _AngleStrength,
                                    float _ConeStrength, float _ConeObstructionDestroyRadius,
                                    float _CylinderStrength, float _CylinderObstructionDestroyRadius,
                                    float _CircleStrength, float _CircleObstructionDestroyRadius,
                                    float _CurveStrength, float _CurveObstructionDestroyRadius,
                                    float _DissolveFallOff,
                                    float _DissolveMaskEnabled,
                                    float _IntrinsicDissolveStrength,
                                    float _DefaultEffectRadius,

                                    float _IsometricExclusion, float _IsometricExclusionDistance, float _IsometricExclusionGradientLength,
                                    float _Ceiling, float _CeilingMode, float _CeilingBlendMode, float _CeilingY, float _CeilingPlayerYOffset, float _CeilingYGradientLength,
                                    float _Floor, float _FloorMode, float _FloorY, float _PlayerPosYOffset, float _FloorYTextureGradientLength,

                                    float _AnimationEnabled, float _AnimationSpeed,
                                    float _TransitionDuration,

                                    float _Zoning, float _ZoningMode, bool _IsZoningRevealable, float _ZoningEdgeGradientLength,
                                    float _SyncZonesWithFloorY, float _SyncZonesFloorYOffset,

                                    float _PreviewMode,
                                    float _PreviewIndicatorLineThickness,

                                    float _isReferenceMaterial,

                                    UnityTexture2D _DissolveTex,
                                    UnityTexture2D _DissolveMask,
                                    UnityTexture2D _ObstructionCurve,

                                    out half3 albedo, out half3 emission, out float alphaForClipping
)
{
#ifndef NO_STS_CLIPPING
    #define NO_STS_CLIPPING
#endif   

#if defined(_REPLACEMENT)
    DoSeeThroughShading(inColor, tangentNormal, worldPosition, worldNormal,

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
    
    DoSeeThroughShading(inColor, tangentNormal, worldPosition, worldNormal,

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

                                    albedo, emission, alphaForClipping);
#endif 
#undef NO_STS_CLIPPING
//#undef USE_UNITY_TEXTURE_2D_TYPE      

}
