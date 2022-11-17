Shader "SeeThroughShader/URP/2020/Lit"
{
   Properties
   {
      [HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
      [HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
      [HideInInspector][NoScaleOffset]unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}
      
//SEE: https://github.com/Unity-Technologies/Graphics/blob/569f8878feb8f4340d6de66efa151d0fc1b79c77/Packages/com.unity.render-pipelines.universal/Shaders/Lit.shader
    // Specular vs Metallic workflow
    _WorkflowMode("WorkflowMode", Float) = 1.0

    [MainTexture] _BaseMap("Albedo", 2D) = "white" {}
    [MainColor] _BaseColor("Color", Color) = (1,1,1,1)

    _Cutoff("Alpha Cutoff", Range(0.0, 1.0)) = 0.5

    _Smoothness("Smoothness", Range(0.0, 1.0)) = 0.5
    _SmoothnessTextureChannel("Smoothness texture channel", Float) = 0

    _Metallic("Metallic", Range(0.0, 1.0)) = 0.0
    _MetallicGlossMap("Metallic", 2D) = "white" {}

    _SpecColor("Specular", Color) = (0.2, 0.2, 0.2)
    _SpecGlossMap("Specular", 2D) = "white" {}

    [ToggleOff] _SpecularHighlights("Specular Highlights", Float) = 1.0
    [ToggleOff] _EnvironmentReflections("Environment Reflections", Float) = 1.0

    _BumpScale("Scale", Float) = 1.0
    _BumpMap("Normal Map", 2D) = "bump" {}

    _Parallax("Scale", Range(0.005, 0.08)) = 0.005
    _ParallaxMap("Height Map", 2D) = "black" {}

    _OcclusionStrength("Strength", Range(0.0, 1.0)) = 1.0
    _OcclusionMap("Occlusion", 2D) = "white" {}

    [HDR] _EmissionColor("Color", Color) = (0,0,0)
    _EmissionMap("Emission", 2D) = "white" {}

    _DetailMask("Detail Mask", 2D) = "white" {}
    _DetailAlbedoMapScale("Scale", Range(0.0, 2.0)) = 1.0
    _DetailAlbedoMap("Detail Albedo x2", 2D) = "linearGrey" {}
    _DetailNormalMapScale("Scale", Range(0.0, 2.0)) = 1.0
    [Normal] _DetailNormalMap("Normal Map", 2D) = "bump" {}


    //[HideInInspector] _ClearCoatMask("_ClearCoatMask", Float) = 0.0
    //[HideInInspector] _ClearCoatSmoothness("_ClearCoatSmoothness", Float) = 0.0

    // Blending state
    _Surface("__surface", Float) = 0.0
    _Blend("__blend", Float) = 0.0
    _Cull("__cull", Float) = 2.0
    [ToggleUI] _AlphaClip("__clip", Float) = 0.0


    [ToggleUI] _ReceiveShadows("Receive Shadows", Float) = 1.0
    // Editmode props
    _QueueOffset("Queue offset", Float) = 0.0

    // ObsoleteProperties
    [HideInInspector] _MainTex("BaseMap", 2D) = "white" {}
    [HideInInspector] _Color("Base Color", Color) = (1, 1, 1, 1)
    [HideInInspector] _GlossMapScale("Smoothness", Float) = 0.0
    [HideInInspector] _Glossiness("Smoothness", Float) = 0.0
    [HideInInspector] _GlossyReflections("EnvironmentReflections", Float) = 0.0




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



   }
   SubShader
   {
      Tags { "RenderPipeline"="UniversalPipeline" "RenderType" = "Opaque" "UniversalMaterialType" = "Lit" "Queue" = "Geometry" }

      

      
        Pass
        {
            Name "Universal Forward"
            Tags 
            { 
                "LightMode" = "UniversalForward"
            }
            Cull Back
            Blend One Zero
            ZTest LEqual
            ZWrite On

            Blend One Zero, One Zero
Cull Back
ZTest LEqual
ZWrite On

                Cull [_Cull]


            HLSLPROGRAM

               #pragma vertex Vert
   #pragma fragment Frag

            #pragma target 3.0

            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma multi_compile_fog
            #pragma multi_compile_instancing
            #pragma multi_compile _ DOTS_INSTANCING_ON
        
            // Keywords
            #pragma multi_compile _ _SCREEN_SPACE_OCCLUSION
            #pragma multi_compile _ LIGHTMAP_ON
            #pragma multi_compile _ DIRLIGHTMAP_COMBINED
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS _ADDITIONAL_OFF
            #pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma multi_compile _ _SHADOWS_SOFT
            #pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
            #pragma multi_compile _ SHADOWS_SHADOWMASK
            // GraphKeywords: <None>

            #define SHADER_PASS SHADERPASS_FORWARD
            #define SHADERPASS_FORWARD
            #define VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
            #define _PASSFORWARD 1
            

            
            #pragma shader_feature_local _NORMALMAP
            #pragma shader_feature_local _PARALLAXMAP
            #pragma shader_feature_local _RECEIVE_SHADOWS_OFF
            #pragma shader_feature_local _ _DETAIL_MULX2 _DETAIL_SCALED
            #pragma shader_feature_local_fragment _SURFACE_TYPE_TRANSPARENT
            #pragma shader_feature_local_fragment _ALPHATEST_ON
            #pragma shader_feature_local_fragment _ _ALPHAPREMULTIPLY_ON _ALPHAMODULATE_ON
            #pragma shader_feature_local_fragment _EMISSION
            #pragma shader_feature_local_fragment _METALLICSPECGLOSSMAP
            #pragma shader_feature_local_fragment _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
            #pragma shader_feature_local_fragment _OCCLUSIONMAP
            #pragma shader_feature_local_fragment _SPECULARHIGHLIGHTS_OFF
            #pragma shader_feature_local_fragment _ENVIRONMENTREFLECTIONS_OFF
            #pragma shader_feature_local_fragment _SPECULAR_SETUP


        #pragma shader_feature_local_fragment _OBSTRUCTION_CURVE
        #pragma shader_feature_local_fragment _DISSOLVEMASK
        #pragma shader_feature_local_fragment _ZONING
        #pragma shader_feature_local_fragment _REPLACEMENT
        #pragma shader_feature_local_fragment _PLAYERINDEPENDENT


   #define _URP 1


            // this has to be here or specular color will be ignored. Not in SG code
            #if _SIMPLELIT
               #define _SPECULAR_COLOR
            #endif


            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Version.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"


        

               #undef WorldNormalVector
      #define WorldNormalVector(data, normal) mul(normal, data.TBNMatrix)
      
      #define UnityObjectToWorldNormal(normal) mul(GetObjectToWorldMatrix(), normal)

      #define _WorldSpaceLightPos0 _MainLightPosition
      
      #define UNITY_DECLARE_TEX2D(name) TEXTURE2D(name); SAMPLER(sampler##name);
      #define UNITY_DECLARE_TEX2D_NOSAMPLER(name) TEXTURE2D(name);
      #define UNITY_DECLARE_TEX2DARRAY(name) TEXTURE2D_ARRAY(name); SAMPLER(sampler##name);
      #define UNITY_DECLARE_TEX2DARRAY_NOSAMPLER(name) TEXTURE2D_ARRAY(name);

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

      

      // data across stages, stripped like the above.
      struct VertexToPixel
      {
         float4 pos : SV_POSITION;
         float3 worldPos : TEXCOORD0;
         float3 worldNormal : TEXCOORD1;
         float4 worldTangent : TEXCOORD2;
          float4 texcoord0 : TEXCOORD3;
         // float4 texcoord1 : TEXCOORD4;
         // float4 texcoord2 : TEXCOORD5;

         // #if %TEXCOORD3REQUIREKEY%
         // float4 texcoord3 : TEXCOORD6;
         // #endif

         // #if %SCREENPOSREQUIREKEY%
         // float4 screenPos : TEXCOORD7;
         // #endif

         // #if %VERTEXCOLORREQUIREKEY%
         // half4 vertexColor : COLOR;
         // #endif

         // #if %EXTRAV2F0REQUIREKEY%
         // float4 extraV2F0 : TEXCOORD12;
         // #endif

         // #if %EXTRAV2F1REQUIREKEY%
         // float4 extraV2F1 : TEXCOORD13;
         // #endif

         // #if %EXTRAV2F2REQUIREKEY%
         // float4 extraV2F2 : TEXCOORD14;
         // #endif

         // #if %EXTRAV2F3REQUIREKEY%
         // float4 extraV2F3 : TEXCOORD15;
         // #endif

         // #if %EXTRAV2F4REQUIREKEY%
         // float4 extraV2F4 : TEXCOORD16;
         // #endif

         // #if %EXTRAV2F5REQUIREKEY%
         // float4 extraV2F5 : TEXCOORD17;
         // #endif

         // #if %EXTRAV2F6REQUIREKEY%
         // float4 extraV2F6 : TEXCOORD18;
         // #endif

         // #if %EXTRAV2F7REQUIREKEY%
         // float4 extraV2F7 : TEXCOORD19;
         // #endif
            
         #if defined(LIGHTMAP_ON)
            float2 lightmapUV : TEXCOORD8;
         #endif
         #if !defined(LIGHTMAP_ON)
            float3 sh : TEXCOORD9;
         #endif

         #ifdef VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
            float4 fogFactorAndVertexLight : TEXCOORD10;
         #endif

         #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
           float4 shadowCoord : TEXCOORD11;
         #endif

         #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : CUSTOM_INSTANCE_ID;
         #endif
         #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
         #endif
         #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
         #endif
         #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
         #endif
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
               // float4 texcoord3 : TEXCOORD3;
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
               // float4 texcoord3 : TEXCOORD3;
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

            

//SEE: https://github.com/Unity-Technologies/Graphics/blob/569f8878feb8f4340d6de66efa151d0fc1b79c77/Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl
	float4 _BaseMap_ST;
	float4 _DetailAlbedoMap_ST;
	half4 _BaseColor;
	half4 _SpecColor;
	half4 _EmissionColor;
	half _Cutoff;
	half _Smoothness;
	half _Metallic;
	half _BumpScale;
	half _Parallax;
	half _OcclusionStrength;
	//half _ClearCoatMask;
	//half _ClearCoatSmoothness;
	half _DetailAlbedoMapScale;
	half _DetailNormalMapScale;
	half _Surface;


//SEE: https://github.com/Unity-Technologies/Graphics/blob/86cdbd182b8fa8aeda2ff536434f9456f3e5029b/Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl
	float _AlphaToMaskAvailable;



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

    //////////////////////
    // Texture Sampler:
    //////////////////////


//SEE: https://github.com/Unity-Technologies/Graphics/blob/6fdc7996098aa184495c0a3c0da10135bfe18340/Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl
	TEXTURE2D(_BaseMap);
	SAMPLER(sampler_BaseMap);
	float4 _BaseMap_TexelSize;
	float4 _BaseMap_MipInfo;
	TEXTURE2D(_BumpMap);
	SAMPLER(sampler_BumpMap);
	TEXTURE2D(_EmissionMap);
	SAMPLER(sampler_EmissionMap);

//SEE: https://github.com/Unity-Technologies/Graphics/blob/569f8878feb8f4340d6de66efa151d0fc1b79c77/Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl
	TEXTURE2D(_ParallaxMap);        SAMPLER(sampler_ParallaxMap);
	TEXTURE2D(_OcclusionMap);       SAMPLER(sampler_OcclusionMap);
	TEXTURE2D(_DetailMask);         SAMPLER(sampler_DetailMask);
	TEXTURE2D(_DetailAlbedoMap);    SAMPLER(sampler_DetailAlbedoMap);
	TEXTURE2D(_DetailNormalMap);    SAMPLER(sampler_DetailNormalMap);
	TEXTURE2D(_MetallicGlossMap);   SAMPLER(sampler_MetallicGlossMap);
	TEXTURE2D(_SpecGlossMap);       SAMPLER(sampler_SpecGlossMap);
	TEXTURE2D(_ClearCoatMap);       SAMPLER(sampler_ClearCoatMap);

    //////////////////////
    // Extra Functions:
    //////////////////////

	#if defined(_DETAIL_MULX2) || defined(_DETAIL_SCALED)
		#define _DETAIL
	#endif

	#if _SPECULAR_SETUP
		#define _USESPECULAR 1
	#else
		#undef _USESPECULAR
	#endif

//See: https://github.com/Unity-Technologies/Graphics/blob/4b98cece5623e02f03c9ff311bca0f749ba4fd94/Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl
	float SharpenAlphaMy(float alpha, float alphaClipTreshold)
	{
	    return saturate((alpha - alphaClipTreshold) / max(fwidth(alpha), 0.0001) + 0.5);
	}


// See: https://github.com/Unity-Technologies/Graphics/blob/223d8105e68c5a808027d78f39cb4e2545fd6276/Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderVariablesFunctions.hlsl
	half3 AlphaModulate(half3 albedo, half alpha)
	{
		#if defined(_ALPHAMODULATE_ON)
		    return lerp(half3(1.0, 1.0, 1.0), albedo, alpha);
		#else
		    return albedo;
		#endif
	}


	#if defined(_ALPHATEST_ON)
		bool IsAlphaToMaskAvailable()
		{
		    return (_AlphaToMaskAvailable != 0.0);
		}
		half AlphaClip(half alpha, half cutoff)
		{
		    half clippedAlpha = (alpha >= cutoff) ? float(alpha) : 0.0;

		    half alphaToCoverageAlpha = SharpenAlphaMy(alpha, cutoff);

		    alpha = IsAlphaToMaskAvailable() ? alphaToCoverageAlpha : clippedAlpha;
		    clip(alpha - 0.0001);

		    return alpha;
		}
	#endif


// https://github.com/Unity-Technologies/Graphics/blob/632f80e011f18ea537ee6e2f0be3ff4f4dea6a11/Packages/com.unity.render-pipelines.universal/ShaderLibrary/Debug/DebuggingCommon.hlsl#L122
int _DebugSceneOverrideMode;
bool IsAlphaDiscardEnabledMy()
{
    #if defined(DEBUG_DISPLAY)
    return (_DebugSceneOverrideMode == DEBUGSCENEOVERRIDEMODE_NONE);
    #else
    return true;
    #endif
}


//See: https://github.com/Unity-Technologies/Graphics/blob/6fdc7996098aa184495c0a3c0da10135bfe18340/Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl

	half Alpha(half albedoAlpha, half4 color, half cutoff)
	{
		#if !defined(_SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A) && !defined(_GLOSSINESS_FROM_BASE_ALPHA)
		    half alpha = albedoAlpha * color.a;
		#else
		    half alpha = color.a;
		#endif

		    //alpha = AlphaDiscard(alpha, cutoff);


			#ifdef _ALPHATEST_ON
			    if (IsAlphaDiscardEnabledMy())
			        alpha = AlphaClip(alpha, cutoff);
			#endif

		    return alpha;
	}

	half4 SampleAlbedoAlpha(float2 uv, TEXTURE2D_PARAM(albedoAlphaMap, sampler_albedoAlphaMap))
	{
	    return half4(SAMPLE_TEXTURE2D(albedoAlphaMap, sampler_albedoAlphaMap, uv));
	}

	half3 SampleNormal(float2 uv, TEXTURE2D_PARAM(bumpMap, sampler_bumpMap), half scale = half(1.0))
	{
	#ifdef _NORMALMAP
	    half4 n = SAMPLE_TEXTURE2D(bumpMap, sampler_bumpMap, uv);
	    #if BUMP_SCALE_NOT_SUPPORTED
	        return UnpackNormal(n);
	    #else
	        return UnpackNormalScale(n, scale);
	    #endif
	#else
	    return half3(0.0h, 0.0h, 1.0h);
	#endif
	}

	half3 SampleEmission(float2 uv, half3 emissionColor, TEXTURE2D_PARAM(emissionMap, sampler_emissionMap))
	{
	#ifndef _EMISSION
	    return 0;
	#else
	    return SAMPLE_TEXTURE2D(emissionMap, sampler_emissionMap, uv).rgb * emissionColor;
	#endif
	}


//SEE: https://github.com/Unity-Technologies/Graphics/blob/632f80e011f18ea537ee6e2f0be3ff4f4dea6a11/Packages/com.unity.render-pipelines.core/ShaderLibrary/ParallaxMapping.hlsl

	#ifndef BUILTIN_TARGET_API
		half2 ParallaxOffset1Step(half height, half amplitude, half3 viewDirTS)
		{
		    height = height * amplitude - amplitude / 2.0;
		    half3 v = normalize(viewDirTS);
		    v.z += 0.42;
		    return height * (v.xy / v.z);
		}
	#endif

	float2 ParallaxMapping(TEXTURE2D_PARAM(heightMap, sampler_heightMap), half3 viewDirTS, half scale, float2 uv)
	{
	    half h = SAMPLE_TEXTURE2D(heightMap, sampler_heightMap, uv).g;
	    float2 offset = ParallaxOffset1Step(h, scale, viewDirTS);
	    return offset;
	}



//SEE: https://github.com/Unity-Technologies/Graphics/blob/569f8878feb8f4340d6de66efa151d0fc1b79c77/Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl

	#ifdef _SPECULAR_SETUP
	    #define SAMPLE_METALLICSPECULAR(uv) SAMPLE_TEXTURE2D(_SpecGlossMap, sampler_SpecGlossMap, uv)
	#else
	    #define SAMPLE_METALLICSPECULAR(uv) SAMPLE_TEXTURE2D(_MetallicGlossMap, sampler_MetallicGlossMap, uv)
	#endif

	half4 SampleMetallicSpecGloss(float2 uv, half albedoAlpha)
	{
	    half4 specGloss;

		#ifdef _METALLICSPECGLOSSMAP
		    specGloss = half4(SAMPLE_METALLICSPECULAR(uv));
		    #ifdef _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
		        specGloss.a = albedoAlpha * _Smoothness;
		    #else
		        specGloss.a *= _Smoothness;
		    #endif
		#else // _METALLICSPECGLOSSMAP
		    #if _SPECULAR_SETUP
		        specGloss.rgb = _SpecColor.rgb;
		    #else
		        specGloss.rgb = _Metallic.rrr;
		    #endif

		    #ifdef _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
		        specGloss.a = albedoAlpha * _Smoothness;
		    #else
		        specGloss.a = _Smoothness;
		    #endif
		#endif

	    return specGloss;
	}

	half SampleOcclusion(float2 uv)
	{
	    #ifdef _OCCLUSIONMAP
	        half occ = SAMPLE_TEXTURE2D(_OcclusionMap, sampler_OcclusionMap, uv).g;
	        return LerpWhiteTo(occ, _OcclusionStrength);
	    #else
	        return half(1.0);
	    #endif
	}


	half2 SampleClearCoat(float2 uv)
	{
		#if defined(_CLEARCOAT) || defined(_CLEARCOATMAP)
		    half2 clearCoatMaskSmoothness = half2(_ClearCoatMask, _ClearCoatSmoothness);

		#if defined(_CLEARCOATMAP)
		    clearCoatMaskSmoothness *= SAMPLE_TEXTURE2D(_ClearCoatMap, sampler_ClearCoatMap, uv).rg;
		#endif

		    return clearCoatMaskSmoothness;
		#else
		    return half2(0.0, 1.0);
		#endif  // _CLEARCOAT
	}

	void ApplyPerPixelDisplacement(half3 viewDirTS, inout float2 uv)
	{
		#if defined(_PARALLAXMAP)
		    uv += ParallaxMapping(TEXTURE2D_ARGS(_ParallaxMap, sampler_ParallaxMap), viewDirTS, _Parallax, uv);
		#endif
	}

	half3 ScaleDetailAlbedo(half3 detailAlbedo, half scale)
	{
	    return half(2.0) * detailAlbedo * scale - scale + half(1.0);
	}

	half3 ApplyDetailAlbedo(float2 detailUv, half3 albedo, half detailMask)
	{
		#if defined(_DETAIL)
		    half3 detailAlbedo = SAMPLE_TEXTURE2D(_DetailAlbedoMap, sampler_DetailAlbedoMap, detailUv).rgb;

		    // In order to have same performance as builtin, we do scaling only if scale is not 1.0 (Scaled version has 6 additional instructions)
		#if defined(_DETAIL_SCALED)
		    detailAlbedo = ScaleDetailAlbedo(detailAlbedo, _DetailAlbedoMapScale);
		#else
		    detailAlbedo = half(2.0) * detailAlbedo;
		#endif

		    return albedo * LerpWhiteTo(detailAlbedo, detailMask);
		#else
		    return albedo;
		#endif
	}

	half3 ApplyDetailNormal(float2 detailUv, half3 normalTS, half detailMask)
	{
		#if defined(_DETAIL)
		#if BUMP_SCALE_NOT_SUPPORTED
		    half3 detailNormalTS = UnpackNormal(SAMPLE_TEXTURE2D(_DetailNormalMap, sampler_DetailNormalMap, detailUv));
		#else
		    half3 detailNormalTS = UnpackNormalScale(SAMPLE_TEXTURE2D(_DetailNormalMap, sampler_DetailNormalMap, detailUv), _DetailNormalMapScale);
		#endif

		    // With UNITY_NO_DXT5nm unpacked vector is not normalized for BlendNormalRNM
		    // For visual consistancy we going to do in all cases
		    detailNormalTS = normalize(detailNormalTS);

		    return lerp(normalTS, BlendNormalRNM(normalTS, detailNormalTS), detailMask); // todo: detailMask should lerp the angle of the quaternion rotation, not the normals
		#else
		    return normalTS;
		#endif
	}





    //////////////////////
    // Main Code:
    //////////////////////

	void Ext_SurfaceFunction0 (inout Surface o, ShaderData d)
	{
        float4 texcoords;
        texcoords.xy = d.texcoord0.xy * _BaseMap_ST.xy + _BaseMap_ST.zw; 
        //texcoords.zw = (_UVSec == 0) ? d.texcoord0.xy * _DetailAlbedoMap_ST.xy + _DetailAlbedoMap_ST.zw : d.texcoord1.xy * _DetailAlbedoMap_ST.xy + _DetailAlbedoMap_ST.zw;
        //texcoords.zw = d.texcoord0.xy * _DetailAlbedoMap_ST.xy + _DetailAlbedoMap_ST.zw;

//SEE: https://github.com/Unity-Technologies/Graphics/blob/569f8878feb8f4340d6de66efa151d0fc1b79c77/Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl

        float2 uv = texcoords.xy;

		#if defined(_PARALLAXMAP)
		    ApplyPerPixelDisplacement(d.tangentSpaceViewDir, uv);
		#endif



    	half4 albedoAlpha =  SampleAlbedoAlpha(uv, TEXTURE2D_ARGS(_BaseMap, sampler_BaseMap));
	    o.Alpha = Alpha(albedoAlpha.a, _BaseColor, _Cutoff);

	    half4 specGloss = SampleMetallicSpecGloss(uv, albedoAlpha.a);
	    o.Albedo = albedoAlpha.rgb * _BaseColor.rgb;
	    o.Albedo = AlphaModulate(o.Albedo, o.Alpha);


		#if _SPECULAR_SETUP
		    o.Metallic = half(1.0);
		    o.Specular = specGloss.rgb;
		#else
		    o.Metallic = specGloss.r;
		    o.Specular = half3(0.0, 0.0, 0.0);
		#endif


        o.Smoothness = specGloss.a;
        o.Normal = SampleNormal(uv, TEXTURE2D_ARGS(_BumpMap, sampler_BumpMap), _BumpScale);
	    o.Occlusion = SampleOcclusion(uv);
	    o.Emission = SampleEmission(uv, _EmissionColor.rgb, TEXTURE2D_ARGS(_EmissionMap, sampler_EmissionMap));

		#if defined(_DETAIL)
		    half detailMask = SAMPLE_TEXTURE2D(_DetailMask, sampler_DetailMask, uv).a;
		    float2 detailUv = uv * _DetailAlbedoMap_ST.xy + _DetailAlbedoMap_ST.zw;
		    o.Albedo = ApplyDetailAlbedo(detailUv, o.Albedo, detailMask);
		    o.Normal = ApplyDetailNormal(detailUv, o.Normal, detailMask);
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
            // d.texcoord1 = i.texcoord1;
            // d.texcoord2 = i.texcoord2;

            // #if %TEXCOORD3REQUIREKEY%
            // d.texcoord3 = i.texcoord3;
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
         

         
         #if _PASSSHADOW
            float3 _LightDirection;
         #endif

         // vertex shader
         VertexToPixel Vert (VertexData v)
         {
           
           VertexToPixel o = (VertexToPixel)0;

           UNITY_SETUP_INSTANCE_ID(v);
           UNITY_TRANSFER_INSTANCE_ID(v, o);
           UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);


#if !_TESSELLATION_ON
           ChainModifyVertex(v, o, _Time);
#endif

            o.texcoord0 = v.texcoord0;
           // o.texcoord1 = v.texcoord1;
           // o.texcoord2 = v.texcoord2;

           // #if %TEXCOORD3REQUIREKEY%
           // o.texcoord3 = v.texcoord3;
           // #endif

           // #if %VERTEXCOLORREQUIREKEY%
           // o.vertexColor = v.vertexColor;
           // #endif
           
           VertexPositionInputs vertexInput = GetVertexPositionInputs(v.vertex.xyz);
           o.worldPos = TransformObjectToWorld(v.vertex.xyz);
           o.worldNormal = TransformObjectToWorldNormal(v.normal);
           o.worldTangent = float4(TransformObjectToWorldDir(v.tangent.xyz), v.tangent.w);


          #if _PASSSHADOW
              // Define shadow pass specific clip position for Universal
              o.pos = TransformWorldToHClip(ApplyShadowBias(o.worldPos, o.worldNormal, _LightDirection));
              #if UNITY_REVERSED_Z
                  o.pos.z = min(o.pos.z, o.pos.w * UNITY_NEAR_CLIP_VALUE);
              #else
                  o.pos.z = max(o.pos.z, o.pos.w * UNITY_NEAR_CLIP_VALUE);
              #endif
          #elif _PASSMETA
              o.pos = MetaVertexPosition(float4(v.vertex.xyz, 0), v.texcoord1.xy, v.texcoord2.xy, unity_LightmapST, unity_DynamicLightmapST);
          #else
              o.pos = TransformWorldToHClip(o.worldPos);
          #endif


          // #if %SCREENPOSREQUIREKEY%
          // o.screenPos = ComputeScreenPos(o.pos, _ProjectionParams.x);
          // #endif

          #if _PASSFORWARD || _PASSGBUFFER
              float2 uv1 = v.texcoord1.xy;
              OUTPUT_LIGHTMAP_UV(uv1, unity_LightmapST, o.lightmapUV);
              // o.texcoord1.xy = uv1;
              OUTPUT_SH(o.worldNormal, o.sh);
          #endif

          #ifdef VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
              #if _BAKEDLIT
                 half3 vertexLight = 0;
              #else
                 half3 vertexLight = VertexLighting(o.worldPos, o.worldNormal);
              #endif
              half fogFactor = ComputeFogFactor(o.pos.z);
              o.fogFactorAndVertexLight = half4(fogFactor, vertexLight);
          #endif

          #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
             o.shadowCoord = GetShadowCoord(vertexInput);
          #endif

           return o;
         }


         

         // fragment shader
         half4 Frag (VertexToPixel IN
            #ifdef _DEPTHOFFSET_ON
              , out float outputDepth : SV_Depth
            #endif
            #if NEED_FACING
               , bool facing : SV_IsFrontFace
            #endif
         ) : SV_Target
         {
           UNITY_SETUP_INSTANCE_ID(IN);
           UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);

           ShaderData d = CreateShaderData(IN
                  #if NEED_FACING
                     , facing
                  #endif
               );
           Surface l = (Surface)0;

           #ifdef _DEPTHOFFSET_ON
              l.outputDepth = outputDepth;
           #endif

           l.Albedo = half3(0.5, 0.5, 0.5);
           l.Normal = float3(0,0,1);
           l.Occlusion = 1;
           l.Alpha = 1;

           ChainSurfaceFunction(l, d);

           #ifdef _DEPTHOFFSET_ON
              outputDepth = l.outputDepth;
           #endif

           #if _USESPECULAR || _SIMPLELIT
              float3 specular = l.Specular;
              float metallic = 1;
           #else   
              float3 specular = 0;
              float metallic = l.Metallic;
           #endif


         
           
            InputData inputData;

            inputData.positionWS = IN.worldPos;
            #if _WORLDSPACENORMAL
              inputData.normalWS = l.Normal;
            #else
              inputData.normalWS = normalize(TangentToWorldSpace(d, l.Normal));
            #endif

            inputData.viewDirectionWS = SafeNormalize(d.worldSpaceViewDir);


            #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
                  inputData.shadowCoord = IN.shadowCoord;
            #elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
                  inputData.shadowCoord = TransformWorldToShadowCoord(IN.worldPos);
            #else
                  inputData.shadowCoord = float4(0, 0, 0, 0);
            #endif

            inputData.fogCoord = IN.fogFactorAndVertexLight.x;
            inputData.vertexLighting = IN.fogFactorAndVertexLight.yzw;
            #if defined(_OVERRIDE_BAKEDGI)
               inputData.bakedGI = l.DiffuseGI;
               l.Emission += l.SpecularGI;
            #else
               inputData.bakedGI = SAMPLE_GI(IN.lightmapUV, IN.sh, inputData.normalWS);
            #endif
            inputData.normalizedScreenSpaceUV = GetNormalizedScreenSpaceUV(IN.pos);
            #if !_BAKEDLIT
               inputData.shadowMask = SAMPLE_SHADOWMASK(IN.lightmapUV);

               #if defined(_OVERRIDE_SHADOWMASK)
                  float4 mulColor = saturate(dot(l.ShadowMask, _MainLightOcclusionProbes)); //unity_OcclusionMaskSelector));
                  inputData.shadowMask = mulColor;
               #endif
            #endif

            #if !_UNLIT
               #if _SIMPLELIT
                  half4 color = UniversalFragmentBlinnPhong(
                     inputData,
                     l.Albedo,
                     float4(specular * l.Smoothness, 0),
                     l.SpecularPower * 128,
                     l.Emission,
                     l.Alpha);
                  color.a = l.Alpha;
               #elif _BAKEDLIT
                  color = UniversalFragmentBakedLit(inputData, l.Albedo, l.Alpha, normalTS);
               #else

                  
                  SurfaceData surface         = (SurfaceData)0;
                  surface.albedo              = l.Albedo;
                  surface.metallic            = saturate(metallic);
                  surface.specular            = specular;
                  surface.smoothness          = saturate(l.Smoothness),
                  surface.occlusion           = l.Occlusion,
                  surface.emission            = l.Emission,
                  surface.alpha               = saturate(l.Alpha);
                  surface.clearCoatMask       = 0;
                  surface.clearCoatSmoothness = 1;

                  #ifdef _CLEARCOAT
                      surface.clearCoatMask       = saturate(l.CoatMask);
                      surface.clearCoatSmoothness = saturate(l.CoatSmoothness);
                  #endif

                  half4 color = UniversalFragmentPBR(inputData, surface);

               #endif

            #else
               half4 color = half4(l.Albedo, l.Alpha);
               
            #endif

            #if !DISABLEFOG
                  color.rgb = MixFog(color.rgb, IN.fogFactorAndVertexLight.x);
               #endif
            ChainFinalColorForward(l, d, color);

            return color;

         }

         ENDHLSL

      }


      
      
        Pass
        {
            Name "ShadowCaster"
            Tags 
            { 
                "LightMode" = "ShadowCaster"
            }
           
            // Render State
            Blend One Zero, One Zero
            Cull Back
            ZTest LEqual
            ZWrite On
            // ColorMask: <None>

                Cull [_Cull]


            HLSLPROGRAM

               #pragma vertex Vert
   #pragma fragment Frag

            #pragma target 3.0

            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma multi_compile_instancing
        
            #define _NORMAL_DROPOFF_TS 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define SHADERPASS_SHADOWCASTER
            #define _PASSSHADOW 1

            
            #pragma shader_feature_local _NORMALMAP
            #pragma shader_feature_local _PARALLAXMAP
            #pragma shader_feature_local _RECEIVE_SHADOWS_OFF
            #pragma shader_feature_local _ _DETAIL_MULX2 _DETAIL_SCALED
            #pragma shader_feature_local_fragment _SURFACE_TYPE_TRANSPARENT
            #pragma shader_feature_local_fragment _ALPHATEST_ON
            #pragma shader_feature_local_fragment _ _ALPHAPREMULTIPLY_ON _ALPHAMODULATE_ON
            #pragma shader_feature_local_fragment _EMISSION
            #pragma shader_feature_local_fragment _METALLICSPECGLOSSMAP
            #pragma shader_feature_local_fragment _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
            #pragma shader_feature_local_fragment _OCCLUSIONMAP
            #pragma shader_feature_local_fragment _SPECULARHIGHLIGHTS_OFF
            #pragma shader_feature_local_fragment _ENVIRONMENTREFLECTIONS_OFF
            #pragma shader_feature_local_fragment _SPECULAR_SETUP


        #pragma shader_feature_local_fragment _OBSTRUCTION_CURVE
        #pragma shader_feature_local_fragment _DISSOLVEMASK
        #pragma shader_feature_local_fragment _ZONING
        #pragma shader_feature_local_fragment _REPLACEMENT
        #pragma shader_feature_local_fragment _PLAYERINDEPENDENT


   #define _URP 1

                 
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"

                  #undef WorldNormalVector
      #define WorldNormalVector(data, normal) mul(normal, data.TBNMatrix)
      
      #define UnityObjectToWorldNormal(normal) mul(GetObjectToWorldMatrix(), normal)

      #define _WorldSpaceLightPos0 _MainLightPosition
      
      #define UNITY_DECLARE_TEX2D(name) TEXTURE2D(name); SAMPLER(sampler##name);
      #define UNITY_DECLARE_TEX2D_NOSAMPLER(name) TEXTURE2D(name);
      #define UNITY_DECLARE_TEX2DARRAY(name) TEXTURE2D_ARRAY(name); SAMPLER(sampler##name);
      #define UNITY_DECLARE_TEX2DARRAY_NOSAMPLER(name) TEXTURE2D_ARRAY(name);

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

      

      // data across stages, stripped like the above.
      struct VertexToPixel
      {
         float4 pos : SV_POSITION;
         float3 worldPos : TEXCOORD0;
         float3 worldNormal : TEXCOORD1;
         float4 worldTangent : TEXCOORD2;
          float4 texcoord0 : TEXCOORD3;
         // float4 texcoord1 : TEXCOORD4;
         // float4 texcoord2 : TEXCOORD5;

         // #if %TEXCOORD3REQUIREKEY%
         // float4 texcoord3 : TEXCOORD6;
         // #endif

         // #if %SCREENPOSREQUIREKEY%
         // float4 screenPos : TEXCOORD7;
         // #endif

         // #if %VERTEXCOLORREQUIREKEY%
         // half4 vertexColor : COLOR;
         // #endif

         // #if %EXTRAV2F0REQUIREKEY%
         // float4 extraV2F0 : TEXCOORD12;
         // #endif

         // #if %EXTRAV2F1REQUIREKEY%
         // float4 extraV2F1 : TEXCOORD13;
         // #endif

         // #if %EXTRAV2F2REQUIREKEY%
         // float4 extraV2F2 : TEXCOORD14;
         // #endif

         // #if %EXTRAV2F3REQUIREKEY%
         // float4 extraV2F3 : TEXCOORD15;
         // #endif

         // #if %EXTRAV2F4REQUIREKEY%
         // float4 extraV2F4 : TEXCOORD16;
         // #endif

         // #if %EXTRAV2F5REQUIREKEY%
         // float4 extraV2F5 : TEXCOORD17;
         // #endif

         // #if %EXTRAV2F6REQUIREKEY%
         // float4 extraV2F6 : TEXCOORD18;
         // #endif

         // #if %EXTRAV2F7REQUIREKEY%
         // float4 extraV2F7 : TEXCOORD19;
         // #endif
            
         #if defined(LIGHTMAP_ON)
            float2 lightmapUV : TEXCOORD8;
         #endif
         #if !defined(LIGHTMAP_ON)
            float3 sh : TEXCOORD9;
         #endif

         #ifdef VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
            float4 fogFactorAndVertexLight : TEXCOORD10;
         #endif

         #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
           float4 shadowCoord : TEXCOORD11;
         #endif

         #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : CUSTOM_INSTANCE_ID;
         #endif
         #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
         #endif
         #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
         #endif
         #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
         #endif
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
               // float4 texcoord3 : TEXCOORD3;
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
               // float4 texcoord3 : TEXCOORD3;
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

               

//SEE: https://github.com/Unity-Technologies/Graphics/blob/569f8878feb8f4340d6de66efa151d0fc1b79c77/Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl
	float4 _BaseMap_ST;
	float4 _DetailAlbedoMap_ST;
	half4 _BaseColor;
	half4 _SpecColor;
	half4 _EmissionColor;
	half _Cutoff;
	half _Smoothness;
	half _Metallic;
	half _BumpScale;
	half _Parallax;
	half _OcclusionStrength;
	//half _ClearCoatMask;
	//half _ClearCoatSmoothness;
	half _DetailAlbedoMapScale;
	half _DetailNormalMapScale;
	half _Surface;


//SEE: https://github.com/Unity-Technologies/Graphics/blob/86cdbd182b8fa8aeda2ff536434f9456f3e5029b/Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl
	float _AlphaToMaskAvailable;



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

    //////////////////////
    // Texture Sampler:
    //////////////////////


//SEE: https://github.com/Unity-Technologies/Graphics/blob/6fdc7996098aa184495c0a3c0da10135bfe18340/Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl
	TEXTURE2D(_BaseMap);
	SAMPLER(sampler_BaseMap);
	float4 _BaseMap_TexelSize;
	float4 _BaseMap_MipInfo;
	TEXTURE2D(_BumpMap);
	SAMPLER(sampler_BumpMap);
	TEXTURE2D(_EmissionMap);
	SAMPLER(sampler_EmissionMap);

//SEE: https://github.com/Unity-Technologies/Graphics/blob/569f8878feb8f4340d6de66efa151d0fc1b79c77/Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl
	TEXTURE2D(_ParallaxMap);        SAMPLER(sampler_ParallaxMap);
	TEXTURE2D(_OcclusionMap);       SAMPLER(sampler_OcclusionMap);
	TEXTURE2D(_DetailMask);         SAMPLER(sampler_DetailMask);
	TEXTURE2D(_DetailAlbedoMap);    SAMPLER(sampler_DetailAlbedoMap);
	TEXTURE2D(_DetailNormalMap);    SAMPLER(sampler_DetailNormalMap);
	TEXTURE2D(_MetallicGlossMap);   SAMPLER(sampler_MetallicGlossMap);
	TEXTURE2D(_SpecGlossMap);       SAMPLER(sampler_SpecGlossMap);
	TEXTURE2D(_ClearCoatMap);       SAMPLER(sampler_ClearCoatMap);

    //////////////////////
    // Extra Functions:
    //////////////////////

	#if defined(_DETAIL_MULX2) || defined(_DETAIL_SCALED)
		#define _DETAIL
	#endif

	#if _SPECULAR_SETUP
		#define _USESPECULAR 1
	#else
		#undef _USESPECULAR
	#endif

//See: https://github.com/Unity-Technologies/Graphics/blob/4b98cece5623e02f03c9ff311bca0f749ba4fd94/Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl
	float SharpenAlphaMy(float alpha, float alphaClipTreshold)
	{
	    return saturate((alpha - alphaClipTreshold) / max(fwidth(alpha), 0.0001) + 0.5);
	}


// See: https://github.com/Unity-Technologies/Graphics/blob/223d8105e68c5a808027d78f39cb4e2545fd6276/Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderVariablesFunctions.hlsl
	half3 AlphaModulate(half3 albedo, half alpha)
	{
		#if defined(_ALPHAMODULATE_ON)
		    return lerp(half3(1.0, 1.0, 1.0), albedo, alpha);
		#else
		    return albedo;
		#endif
	}


	#if defined(_ALPHATEST_ON)
		bool IsAlphaToMaskAvailable()
		{
		    return (_AlphaToMaskAvailable != 0.0);
		}
		half AlphaClip(half alpha, half cutoff)
		{
		    half clippedAlpha = (alpha >= cutoff) ? float(alpha) : 0.0;

		    half alphaToCoverageAlpha = SharpenAlphaMy(alpha, cutoff);

		    alpha = IsAlphaToMaskAvailable() ? alphaToCoverageAlpha : clippedAlpha;
		    clip(alpha - 0.0001);

		    return alpha;
		}
	#endif


// https://github.com/Unity-Technologies/Graphics/blob/632f80e011f18ea537ee6e2f0be3ff4f4dea6a11/Packages/com.unity.render-pipelines.universal/ShaderLibrary/Debug/DebuggingCommon.hlsl#L122
int _DebugSceneOverrideMode;
bool IsAlphaDiscardEnabledMy()
{
    #if defined(DEBUG_DISPLAY)
    return (_DebugSceneOverrideMode == DEBUGSCENEOVERRIDEMODE_NONE);
    #else
    return true;
    #endif
}


//See: https://github.com/Unity-Technologies/Graphics/blob/6fdc7996098aa184495c0a3c0da10135bfe18340/Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl

	half Alpha(half albedoAlpha, half4 color, half cutoff)
	{
		#if !defined(_SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A) && !defined(_GLOSSINESS_FROM_BASE_ALPHA)
		    half alpha = albedoAlpha * color.a;
		#else
		    half alpha = color.a;
		#endif

		    //alpha = AlphaDiscard(alpha, cutoff);


			#ifdef _ALPHATEST_ON
			    if (IsAlphaDiscardEnabledMy())
			        alpha = AlphaClip(alpha, cutoff);
			#endif

		    return alpha;
	}

	half4 SampleAlbedoAlpha(float2 uv, TEXTURE2D_PARAM(albedoAlphaMap, sampler_albedoAlphaMap))
	{
	    return half4(SAMPLE_TEXTURE2D(albedoAlphaMap, sampler_albedoAlphaMap, uv));
	}

	half3 SampleNormal(float2 uv, TEXTURE2D_PARAM(bumpMap, sampler_bumpMap), half scale = half(1.0))
	{
	#ifdef _NORMALMAP
	    half4 n = SAMPLE_TEXTURE2D(bumpMap, sampler_bumpMap, uv);
	    #if BUMP_SCALE_NOT_SUPPORTED
	        return UnpackNormal(n);
	    #else
	        return UnpackNormalScale(n, scale);
	    #endif
	#else
	    return half3(0.0h, 0.0h, 1.0h);
	#endif
	}

	half3 SampleEmission(float2 uv, half3 emissionColor, TEXTURE2D_PARAM(emissionMap, sampler_emissionMap))
	{
	#ifndef _EMISSION
	    return 0;
	#else
	    return SAMPLE_TEXTURE2D(emissionMap, sampler_emissionMap, uv).rgb * emissionColor;
	#endif
	}


//SEE: https://github.com/Unity-Technologies/Graphics/blob/632f80e011f18ea537ee6e2f0be3ff4f4dea6a11/Packages/com.unity.render-pipelines.core/ShaderLibrary/ParallaxMapping.hlsl

	#ifndef BUILTIN_TARGET_API
		half2 ParallaxOffset1Step(half height, half amplitude, half3 viewDirTS)
		{
		    height = height * amplitude - amplitude / 2.0;
		    half3 v = normalize(viewDirTS);
		    v.z += 0.42;
		    return height * (v.xy / v.z);
		}
	#endif

	float2 ParallaxMapping(TEXTURE2D_PARAM(heightMap, sampler_heightMap), half3 viewDirTS, half scale, float2 uv)
	{
	    half h = SAMPLE_TEXTURE2D(heightMap, sampler_heightMap, uv).g;
	    float2 offset = ParallaxOffset1Step(h, scale, viewDirTS);
	    return offset;
	}



//SEE: https://github.com/Unity-Technologies/Graphics/blob/569f8878feb8f4340d6de66efa151d0fc1b79c77/Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl

	#ifdef _SPECULAR_SETUP
	    #define SAMPLE_METALLICSPECULAR(uv) SAMPLE_TEXTURE2D(_SpecGlossMap, sampler_SpecGlossMap, uv)
	#else
	    #define SAMPLE_METALLICSPECULAR(uv) SAMPLE_TEXTURE2D(_MetallicGlossMap, sampler_MetallicGlossMap, uv)
	#endif

	half4 SampleMetallicSpecGloss(float2 uv, half albedoAlpha)
	{
	    half4 specGloss;

		#ifdef _METALLICSPECGLOSSMAP
		    specGloss = half4(SAMPLE_METALLICSPECULAR(uv));
		    #ifdef _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
		        specGloss.a = albedoAlpha * _Smoothness;
		    #else
		        specGloss.a *= _Smoothness;
		    #endif
		#else // _METALLICSPECGLOSSMAP
		    #if _SPECULAR_SETUP
		        specGloss.rgb = _SpecColor.rgb;
		    #else
		        specGloss.rgb = _Metallic.rrr;
		    #endif

		    #ifdef _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
		        specGloss.a = albedoAlpha * _Smoothness;
		    #else
		        specGloss.a = _Smoothness;
		    #endif
		#endif

	    return specGloss;
	}

	half SampleOcclusion(float2 uv)
	{
	    #ifdef _OCCLUSIONMAP
	        half occ = SAMPLE_TEXTURE2D(_OcclusionMap, sampler_OcclusionMap, uv).g;
	        return LerpWhiteTo(occ, _OcclusionStrength);
	    #else
	        return half(1.0);
	    #endif
	}


	half2 SampleClearCoat(float2 uv)
	{
		#if defined(_CLEARCOAT) || defined(_CLEARCOATMAP)
		    half2 clearCoatMaskSmoothness = half2(_ClearCoatMask, _ClearCoatSmoothness);

		#if defined(_CLEARCOATMAP)
		    clearCoatMaskSmoothness *= SAMPLE_TEXTURE2D(_ClearCoatMap, sampler_ClearCoatMap, uv).rg;
		#endif

		    return clearCoatMaskSmoothness;
		#else
		    return half2(0.0, 1.0);
		#endif  // _CLEARCOAT
	}

	void ApplyPerPixelDisplacement(half3 viewDirTS, inout float2 uv)
	{
		#if defined(_PARALLAXMAP)
		    uv += ParallaxMapping(TEXTURE2D_ARGS(_ParallaxMap, sampler_ParallaxMap), viewDirTS, _Parallax, uv);
		#endif
	}

	half3 ScaleDetailAlbedo(half3 detailAlbedo, half scale)
	{
	    return half(2.0) * detailAlbedo * scale - scale + half(1.0);
	}

	half3 ApplyDetailAlbedo(float2 detailUv, half3 albedo, half detailMask)
	{
		#if defined(_DETAIL)
		    half3 detailAlbedo = SAMPLE_TEXTURE2D(_DetailAlbedoMap, sampler_DetailAlbedoMap, detailUv).rgb;

		    // In order to have same performance as builtin, we do scaling only if scale is not 1.0 (Scaled version has 6 additional instructions)
		#if defined(_DETAIL_SCALED)
		    detailAlbedo = ScaleDetailAlbedo(detailAlbedo, _DetailAlbedoMapScale);
		#else
		    detailAlbedo = half(2.0) * detailAlbedo;
		#endif

		    return albedo * LerpWhiteTo(detailAlbedo, detailMask);
		#else
		    return albedo;
		#endif
	}

	half3 ApplyDetailNormal(float2 detailUv, half3 normalTS, half detailMask)
	{
		#if defined(_DETAIL)
		#if BUMP_SCALE_NOT_SUPPORTED
		    half3 detailNormalTS = UnpackNormal(SAMPLE_TEXTURE2D(_DetailNormalMap, sampler_DetailNormalMap, detailUv));
		#else
		    half3 detailNormalTS = UnpackNormalScale(SAMPLE_TEXTURE2D(_DetailNormalMap, sampler_DetailNormalMap, detailUv), _DetailNormalMapScale);
		#endif

		    // With UNITY_NO_DXT5nm unpacked vector is not normalized for BlendNormalRNM
		    // For visual consistancy we going to do in all cases
		    detailNormalTS = normalize(detailNormalTS);

		    return lerp(normalTS, BlendNormalRNM(normalTS, detailNormalTS), detailMask); // todo: detailMask should lerp the angle of the quaternion rotation, not the normals
		#else
		    return normalTS;
		#endif
	}





    //////////////////////
    // Main Code:
    //////////////////////

	void Ext_SurfaceFunction0 (inout Surface o, ShaderData d)
	{
        float4 texcoords;
        texcoords.xy = d.texcoord0.xy * _BaseMap_ST.xy + _BaseMap_ST.zw; 
        //texcoords.zw = (_UVSec == 0) ? d.texcoord0.xy * _DetailAlbedoMap_ST.xy + _DetailAlbedoMap_ST.zw : d.texcoord1.xy * _DetailAlbedoMap_ST.xy + _DetailAlbedoMap_ST.zw;
        //texcoords.zw = d.texcoord0.xy * _DetailAlbedoMap_ST.xy + _DetailAlbedoMap_ST.zw;

//SEE: https://github.com/Unity-Technologies/Graphics/blob/569f8878feb8f4340d6de66efa151d0fc1b79c77/Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl

        float2 uv = texcoords.xy;

		#if defined(_PARALLAXMAP)
		    ApplyPerPixelDisplacement(d.tangentSpaceViewDir, uv);
		#endif



    	half4 albedoAlpha =  SampleAlbedoAlpha(uv, TEXTURE2D_ARGS(_BaseMap, sampler_BaseMap));
	    o.Alpha = Alpha(albedoAlpha.a, _BaseColor, _Cutoff);

	    half4 specGloss = SampleMetallicSpecGloss(uv, albedoAlpha.a);
	    o.Albedo = albedoAlpha.rgb * _BaseColor.rgb;
	    o.Albedo = AlphaModulate(o.Albedo, o.Alpha);


		#if _SPECULAR_SETUP
		    o.Metallic = half(1.0);
		    o.Specular = specGloss.rgb;
		#else
		    o.Metallic = specGloss.r;
		    o.Specular = half3(0.0, 0.0, 0.0);
		#endif


        o.Smoothness = specGloss.a;
        o.Normal = SampleNormal(uv, TEXTURE2D_ARGS(_BumpMap, sampler_BumpMap), _BumpScale);
	    o.Occlusion = SampleOcclusion(uv);
	    o.Emission = SampleEmission(uv, _EmissionColor.rgb, TEXTURE2D_ARGS(_EmissionMap, sampler_EmissionMap));

		#if defined(_DETAIL)
		    half detailMask = SAMPLE_TEXTURE2D(_DetailMask, sampler_DetailMask, uv).a;
		    float2 detailUv = uv * _DetailAlbedoMap_ST.xy + _DetailAlbedoMap_ST.zw;
		    o.Albedo = ApplyDetailAlbedo(detailUv, o.Albedo, detailMask);
		    o.Normal = ApplyDetailNormal(detailUv, o.Normal, detailMask);
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
            // d.texcoord1 = i.texcoord1;
            // d.texcoord2 = i.texcoord2;

            // #if %TEXCOORD3REQUIREKEY%
            // d.texcoord3 = i.texcoord3;
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
         

            
         #if _PASSSHADOW
            float3 _LightDirection;
         #endif

         // vertex shader
         VertexToPixel Vert (VertexData v)
         {
           
           VertexToPixel o = (VertexToPixel)0;

           UNITY_SETUP_INSTANCE_ID(v);
           UNITY_TRANSFER_INSTANCE_ID(v, o);
           UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);


#if !_TESSELLATION_ON
           ChainModifyVertex(v, o, _Time);
#endif

            o.texcoord0 = v.texcoord0;
           // o.texcoord1 = v.texcoord1;
           // o.texcoord2 = v.texcoord2;

           // #if %TEXCOORD3REQUIREKEY%
           // o.texcoord3 = v.texcoord3;
           // #endif

           // #if %VERTEXCOLORREQUIREKEY%
           // o.vertexColor = v.vertexColor;
           // #endif
           
           VertexPositionInputs vertexInput = GetVertexPositionInputs(v.vertex.xyz);
           o.worldPos = TransformObjectToWorld(v.vertex.xyz);
           o.worldNormal = TransformObjectToWorldNormal(v.normal);
           o.worldTangent = float4(TransformObjectToWorldDir(v.tangent.xyz), v.tangent.w);


          #if _PASSSHADOW
              // Define shadow pass specific clip position for Universal
              o.pos = TransformWorldToHClip(ApplyShadowBias(o.worldPos, o.worldNormal, _LightDirection));
              #if UNITY_REVERSED_Z
                  o.pos.z = min(o.pos.z, o.pos.w * UNITY_NEAR_CLIP_VALUE);
              #else
                  o.pos.z = max(o.pos.z, o.pos.w * UNITY_NEAR_CLIP_VALUE);
              #endif
          #elif _PASSMETA
              o.pos = MetaVertexPosition(float4(v.vertex.xyz, 0), v.texcoord1.xy, v.texcoord2.xy, unity_LightmapST, unity_DynamicLightmapST);
          #else
              o.pos = TransformWorldToHClip(o.worldPos);
          #endif


          // #if %SCREENPOSREQUIREKEY%
          // o.screenPos = ComputeScreenPos(o.pos, _ProjectionParams.x);
          // #endif

          #if _PASSFORWARD || _PASSGBUFFER
              float2 uv1 = v.texcoord1.xy;
              OUTPUT_LIGHTMAP_UV(uv1, unity_LightmapST, o.lightmapUV);
              // o.texcoord1.xy = uv1;
              OUTPUT_SH(o.worldNormal, o.sh);
          #endif

          #ifdef VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
              #if _BAKEDLIT
                 half3 vertexLight = 0;
              #else
                 half3 vertexLight = VertexLighting(o.worldPos, o.worldNormal);
              #endif
              half fogFactor = ComputeFogFactor(o.pos.z);
              o.fogFactorAndVertexLight = half4(fogFactor, vertexLight);
          #endif

          #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
             o.shadowCoord = GetShadowCoord(vertexInput);
          #endif

           return o;
         }


            

            // fragment shader
            half4 Frag (VertexToPixel IN
            #ifdef _DEPTHOFFSET_ON
              , out float outputDepth : SV_Depth
            #endif
            #if NEED_FACING
               , bool facing : SV_IsFrontFace
            #endif
            ) : SV_Target
            {
               UNITY_SETUP_INSTANCE_ID(IN);

               ShaderData d = CreateShaderData(IN
                  #if NEED_FACING
                     , facing
                  #endif
               );
               Surface l = (Surface)0;

               #ifdef _DEPTHOFFSET_ON
                  l.outputDepth = outputDepth;
               #endif

               l.Albedo = half3(0.5, 0.5, 0.5);
               l.Normal = float3(0,0,1);
               l.Occlusion = 1;
               l.Alpha = 1;

               ChainSurfaceFunction(l, d);

               #ifdef _DEPTHOFFSET_ON
                  outputDepth = l.outputDepth;
               #endif

             return 0;

            }

         ENDHLSL

      }


      
        Pass
        {
            Name "DepthOnly"
            Tags 
            { 
                "LightMode" = "DepthOnly"
            }
           
            // Render State
            Blend One Zero, One Zero
            Cull Back
            ZTest LEqual
            ZWrite On
            ColorMask 0
            
                Cull [_Cull]


            HLSLPROGRAM

               #pragma vertex Vert
   #pragma fragment Frag


            #define SHADERPASS_DEPTHONLY
            #define _PASSDEPTH 1

            #pragma target 3.0
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma multi_compile_instancing
            #pragma multi_compile _ DOTS_INSTANCING_ON

            
            #pragma shader_feature_local _NORMALMAP
            #pragma shader_feature_local _PARALLAXMAP
            #pragma shader_feature_local _RECEIVE_SHADOWS_OFF
            #pragma shader_feature_local _ _DETAIL_MULX2 _DETAIL_SCALED
            #pragma shader_feature_local_fragment _SURFACE_TYPE_TRANSPARENT
            #pragma shader_feature_local_fragment _ALPHATEST_ON
            #pragma shader_feature_local_fragment _ _ALPHAPREMULTIPLY_ON _ALPHAMODULATE_ON
            #pragma shader_feature_local_fragment _EMISSION
            #pragma shader_feature_local_fragment _METALLICSPECGLOSSMAP
            #pragma shader_feature_local_fragment _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
            #pragma shader_feature_local_fragment _OCCLUSIONMAP
            #pragma shader_feature_local_fragment _SPECULARHIGHLIGHTS_OFF
            #pragma shader_feature_local_fragment _ENVIRONMENTREFLECTIONS_OFF
            #pragma shader_feature_local_fragment _SPECULAR_SETUP


        #pragma shader_feature_local_fragment _OBSTRUCTION_CURVE
        #pragma shader_feature_local_fragment _DISSOLVEMASK
        #pragma shader_feature_local_fragment _ZONING
        #pragma shader_feature_local_fragment _REPLACEMENT
        #pragma shader_feature_local_fragment _PLAYERINDEPENDENT


   #define _URP 1

            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Version.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"


                  #undef WorldNormalVector
      #define WorldNormalVector(data, normal) mul(normal, data.TBNMatrix)
      
      #define UnityObjectToWorldNormal(normal) mul(GetObjectToWorldMatrix(), normal)

      #define _WorldSpaceLightPos0 _MainLightPosition
      
      #define UNITY_DECLARE_TEX2D(name) TEXTURE2D(name); SAMPLER(sampler##name);
      #define UNITY_DECLARE_TEX2D_NOSAMPLER(name) TEXTURE2D(name);
      #define UNITY_DECLARE_TEX2DARRAY(name) TEXTURE2D_ARRAY(name); SAMPLER(sampler##name);
      #define UNITY_DECLARE_TEX2DARRAY_NOSAMPLER(name) TEXTURE2D_ARRAY(name);

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

      

      // data across stages, stripped like the above.
      struct VertexToPixel
      {
         float4 pos : SV_POSITION;
         float3 worldPos : TEXCOORD0;
         float3 worldNormal : TEXCOORD1;
         float4 worldTangent : TEXCOORD2;
          float4 texcoord0 : TEXCOORD3;
         // float4 texcoord1 : TEXCOORD4;
         // float4 texcoord2 : TEXCOORD5;

         // #if %TEXCOORD3REQUIREKEY%
         // float4 texcoord3 : TEXCOORD6;
         // #endif

         // #if %SCREENPOSREQUIREKEY%
         // float4 screenPos : TEXCOORD7;
         // #endif

         // #if %VERTEXCOLORREQUIREKEY%
         // half4 vertexColor : COLOR;
         // #endif

         // #if %EXTRAV2F0REQUIREKEY%
         // float4 extraV2F0 : TEXCOORD12;
         // #endif

         // #if %EXTRAV2F1REQUIREKEY%
         // float4 extraV2F1 : TEXCOORD13;
         // #endif

         // #if %EXTRAV2F2REQUIREKEY%
         // float4 extraV2F2 : TEXCOORD14;
         // #endif

         // #if %EXTRAV2F3REQUIREKEY%
         // float4 extraV2F3 : TEXCOORD15;
         // #endif

         // #if %EXTRAV2F4REQUIREKEY%
         // float4 extraV2F4 : TEXCOORD16;
         // #endif

         // #if %EXTRAV2F5REQUIREKEY%
         // float4 extraV2F5 : TEXCOORD17;
         // #endif

         // #if %EXTRAV2F6REQUIREKEY%
         // float4 extraV2F6 : TEXCOORD18;
         // #endif

         // #if %EXTRAV2F7REQUIREKEY%
         // float4 extraV2F7 : TEXCOORD19;
         // #endif
            
         #if defined(LIGHTMAP_ON)
            float2 lightmapUV : TEXCOORD8;
         #endif
         #if !defined(LIGHTMAP_ON)
            float3 sh : TEXCOORD9;
         #endif

         #ifdef VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
            float4 fogFactorAndVertexLight : TEXCOORD10;
         #endif

         #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
           float4 shadowCoord : TEXCOORD11;
         #endif

         #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : CUSTOM_INSTANCE_ID;
         #endif
         #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
         #endif
         #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
         #endif
         #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
         #endif
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
               // float4 texcoord3 : TEXCOORD3;
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
               // float4 texcoord3 : TEXCOORD3;
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

               

//SEE: https://github.com/Unity-Technologies/Graphics/blob/569f8878feb8f4340d6de66efa151d0fc1b79c77/Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl
	float4 _BaseMap_ST;
	float4 _DetailAlbedoMap_ST;
	half4 _BaseColor;
	half4 _SpecColor;
	half4 _EmissionColor;
	half _Cutoff;
	half _Smoothness;
	half _Metallic;
	half _BumpScale;
	half _Parallax;
	half _OcclusionStrength;
	//half _ClearCoatMask;
	//half _ClearCoatSmoothness;
	half _DetailAlbedoMapScale;
	half _DetailNormalMapScale;
	half _Surface;


//SEE: https://github.com/Unity-Technologies/Graphics/blob/86cdbd182b8fa8aeda2ff536434f9456f3e5029b/Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl
	float _AlphaToMaskAvailable;



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

    //////////////////////
    // Texture Sampler:
    //////////////////////


//SEE: https://github.com/Unity-Technologies/Graphics/blob/6fdc7996098aa184495c0a3c0da10135bfe18340/Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl
	TEXTURE2D(_BaseMap);
	SAMPLER(sampler_BaseMap);
	float4 _BaseMap_TexelSize;
	float4 _BaseMap_MipInfo;
	TEXTURE2D(_BumpMap);
	SAMPLER(sampler_BumpMap);
	TEXTURE2D(_EmissionMap);
	SAMPLER(sampler_EmissionMap);

//SEE: https://github.com/Unity-Technologies/Graphics/blob/569f8878feb8f4340d6de66efa151d0fc1b79c77/Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl
	TEXTURE2D(_ParallaxMap);        SAMPLER(sampler_ParallaxMap);
	TEXTURE2D(_OcclusionMap);       SAMPLER(sampler_OcclusionMap);
	TEXTURE2D(_DetailMask);         SAMPLER(sampler_DetailMask);
	TEXTURE2D(_DetailAlbedoMap);    SAMPLER(sampler_DetailAlbedoMap);
	TEXTURE2D(_DetailNormalMap);    SAMPLER(sampler_DetailNormalMap);
	TEXTURE2D(_MetallicGlossMap);   SAMPLER(sampler_MetallicGlossMap);
	TEXTURE2D(_SpecGlossMap);       SAMPLER(sampler_SpecGlossMap);
	TEXTURE2D(_ClearCoatMap);       SAMPLER(sampler_ClearCoatMap);

    //////////////////////
    // Extra Functions:
    //////////////////////

	#if defined(_DETAIL_MULX2) || defined(_DETAIL_SCALED)
		#define _DETAIL
	#endif

	#if _SPECULAR_SETUP
		#define _USESPECULAR 1
	#else
		#undef _USESPECULAR
	#endif

//See: https://github.com/Unity-Technologies/Graphics/blob/4b98cece5623e02f03c9ff311bca0f749ba4fd94/Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl
	float SharpenAlphaMy(float alpha, float alphaClipTreshold)
	{
	    return saturate((alpha - alphaClipTreshold) / max(fwidth(alpha), 0.0001) + 0.5);
	}


// See: https://github.com/Unity-Technologies/Graphics/blob/223d8105e68c5a808027d78f39cb4e2545fd6276/Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderVariablesFunctions.hlsl
	half3 AlphaModulate(half3 albedo, half alpha)
	{
		#if defined(_ALPHAMODULATE_ON)
		    return lerp(half3(1.0, 1.0, 1.0), albedo, alpha);
		#else
		    return albedo;
		#endif
	}


	#if defined(_ALPHATEST_ON)
		bool IsAlphaToMaskAvailable()
		{
		    return (_AlphaToMaskAvailable != 0.0);
		}
		half AlphaClip(half alpha, half cutoff)
		{
		    half clippedAlpha = (alpha >= cutoff) ? float(alpha) : 0.0;

		    half alphaToCoverageAlpha = SharpenAlphaMy(alpha, cutoff);

		    alpha = IsAlphaToMaskAvailable() ? alphaToCoverageAlpha : clippedAlpha;
		    clip(alpha - 0.0001);

		    return alpha;
		}
	#endif


// https://github.com/Unity-Technologies/Graphics/blob/632f80e011f18ea537ee6e2f0be3ff4f4dea6a11/Packages/com.unity.render-pipelines.universal/ShaderLibrary/Debug/DebuggingCommon.hlsl#L122
int _DebugSceneOverrideMode;
bool IsAlphaDiscardEnabledMy()
{
    #if defined(DEBUG_DISPLAY)
    return (_DebugSceneOverrideMode == DEBUGSCENEOVERRIDEMODE_NONE);
    #else
    return true;
    #endif
}


//See: https://github.com/Unity-Technologies/Graphics/blob/6fdc7996098aa184495c0a3c0da10135bfe18340/Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl

	half Alpha(half albedoAlpha, half4 color, half cutoff)
	{
		#if !defined(_SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A) && !defined(_GLOSSINESS_FROM_BASE_ALPHA)
		    half alpha = albedoAlpha * color.a;
		#else
		    half alpha = color.a;
		#endif

		    //alpha = AlphaDiscard(alpha, cutoff);


			#ifdef _ALPHATEST_ON
			    if (IsAlphaDiscardEnabledMy())
			        alpha = AlphaClip(alpha, cutoff);
			#endif

		    return alpha;
	}

	half4 SampleAlbedoAlpha(float2 uv, TEXTURE2D_PARAM(albedoAlphaMap, sampler_albedoAlphaMap))
	{
	    return half4(SAMPLE_TEXTURE2D(albedoAlphaMap, sampler_albedoAlphaMap, uv));
	}

	half3 SampleNormal(float2 uv, TEXTURE2D_PARAM(bumpMap, sampler_bumpMap), half scale = half(1.0))
	{
	#ifdef _NORMALMAP
	    half4 n = SAMPLE_TEXTURE2D(bumpMap, sampler_bumpMap, uv);
	    #if BUMP_SCALE_NOT_SUPPORTED
	        return UnpackNormal(n);
	    #else
	        return UnpackNormalScale(n, scale);
	    #endif
	#else
	    return half3(0.0h, 0.0h, 1.0h);
	#endif
	}

	half3 SampleEmission(float2 uv, half3 emissionColor, TEXTURE2D_PARAM(emissionMap, sampler_emissionMap))
	{
	#ifndef _EMISSION
	    return 0;
	#else
	    return SAMPLE_TEXTURE2D(emissionMap, sampler_emissionMap, uv).rgb * emissionColor;
	#endif
	}


//SEE: https://github.com/Unity-Technologies/Graphics/blob/632f80e011f18ea537ee6e2f0be3ff4f4dea6a11/Packages/com.unity.render-pipelines.core/ShaderLibrary/ParallaxMapping.hlsl

	#ifndef BUILTIN_TARGET_API
		half2 ParallaxOffset1Step(half height, half amplitude, half3 viewDirTS)
		{
		    height = height * amplitude - amplitude / 2.0;
		    half3 v = normalize(viewDirTS);
		    v.z += 0.42;
		    return height * (v.xy / v.z);
		}
	#endif

	float2 ParallaxMapping(TEXTURE2D_PARAM(heightMap, sampler_heightMap), half3 viewDirTS, half scale, float2 uv)
	{
	    half h = SAMPLE_TEXTURE2D(heightMap, sampler_heightMap, uv).g;
	    float2 offset = ParallaxOffset1Step(h, scale, viewDirTS);
	    return offset;
	}



//SEE: https://github.com/Unity-Technologies/Graphics/blob/569f8878feb8f4340d6de66efa151d0fc1b79c77/Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl

	#ifdef _SPECULAR_SETUP
	    #define SAMPLE_METALLICSPECULAR(uv) SAMPLE_TEXTURE2D(_SpecGlossMap, sampler_SpecGlossMap, uv)
	#else
	    #define SAMPLE_METALLICSPECULAR(uv) SAMPLE_TEXTURE2D(_MetallicGlossMap, sampler_MetallicGlossMap, uv)
	#endif

	half4 SampleMetallicSpecGloss(float2 uv, half albedoAlpha)
	{
	    half4 specGloss;

		#ifdef _METALLICSPECGLOSSMAP
		    specGloss = half4(SAMPLE_METALLICSPECULAR(uv));
		    #ifdef _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
		        specGloss.a = albedoAlpha * _Smoothness;
		    #else
		        specGloss.a *= _Smoothness;
		    #endif
		#else // _METALLICSPECGLOSSMAP
		    #if _SPECULAR_SETUP
		        specGloss.rgb = _SpecColor.rgb;
		    #else
		        specGloss.rgb = _Metallic.rrr;
		    #endif

		    #ifdef _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
		        specGloss.a = albedoAlpha * _Smoothness;
		    #else
		        specGloss.a = _Smoothness;
		    #endif
		#endif

	    return specGloss;
	}

	half SampleOcclusion(float2 uv)
	{
	    #ifdef _OCCLUSIONMAP
	        half occ = SAMPLE_TEXTURE2D(_OcclusionMap, sampler_OcclusionMap, uv).g;
	        return LerpWhiteTo(occ, _OcclusionStrength);
	    #else
	        return half(1.0);
	    #endif
	}


	half2 SampleClearCoat(float2 uv)
	{
		#if defined(_CLEARCOAT) || defined(_CLEARCOATMAP)
		    half2 clearCoatMaskSmoothness = half2(_ClearCoatMask, _ClearCoatSmoothness);

		#if defined(_CLEARCOATMAP)
		    clearCoatMaskSmoothness *= SAMPLE_TEXTURE2D(_ClearCoatMap, sampler_ClearCoatMap, uv).rg;
		#endif

		    return clearCoatMaskSmoothness;
		#else
		    return half2(0.0, 1.0);
		#endif  // _CLEARCOAT
	}

	void ApplyPerPixelDisplacement(half3 viewDirTS, inout float2 uv)
	{
		#if defined(_PARALLAXMAP)
		    uv += ParallaxMapping(TEXTURE2D_ARGS(_ParallaxMap, sampler_ParallaxMap), viewDirTS, _Parallax, uv);
		#endif
	}

	half3 ScaleDetailAlbedo(half3 detailAlbedo, half scale)
	{
	    return half(2.0) * detailAlbedo * scale - scale + half(1.0);
	}

	half3 ApplyDetailAlbedo(float2 detailUv, half3 albedo, half detailMask)
	{
		#if defined(_DETAIL)
		    half3 detailAlbedo = SAMPLE_TEXTURE2D(_DetailAlbedoMap, sampler_DetailAlbedoMap, detailUv).rgb;

		    // In order to have same performance as builtin, we do scaling only if scale is not 1.0 (Scaled version has 6 additional instructions)
		#if defined(_DETAIL_SCALED)
		    detailAlbedo = ScaleDetailAlbedo(detailAlbedo, _DetailAlbedoMapScale);
		#else
		    detailAlbedo = half(2.0) * detailAlbedo;
		#endif

		    return albedo * LerpWhiteTo(detailAlbedo, detailMask);
		#else
		    return albedo;
		#endif
	}

	half3 ApplyDetailNormal(float2 detailUv, half3 normalTS, half detailMask)
	{
		#if defined(_DETAIL)
		#if BUMP_SCALE_NOT_SUPPORTED
		    half3 detailNormalTS = UnpackNormal(SAMPLE_TEXTURE2D(_DetailNormalMap, sampler_DetailNormalMap, detailUv));
		#else
		    half3 detailNormalTS = UnpackNormalScale(SAMPLE_TEXTURE2D(_DetailNormalMap, sampler_DetailNormalMap, detailUv), _DetailNormalMapScale);
		#endif

		    // With UNITY_NO_DXT5nm unpacked vector is not normalized for BlendNormalRNM
		    // For visual consistancy we going to do in all cases
		    detailNormalTS = normalize(detailNormalTS);

		    return lerp(normalTS, BlendNormalRNM(normalTS, detailNormalTS), detailMask); // todo: detailMask should lerp the angle of the quaternion rotation, not the normals
		#else
		    return normalTS;
		#endif
	}





    //////////////////////
    // Main Code:
    //////////////////////

	void Ext_SurfaceFunction0 (inout Surface o, ShaderData d)
	{
        float4 texcoords;
        texcoords.xy = d.texcoord0.xy * _BaseMap_ST.xy + _BaseMap_ST.zw; 
        //texcoords.zw = (_UVSec == 0) ? d.texcoord0.xy * _DetailAlbedoMap_ST.xy + _DetailAlbedoMap_ST.zw : d.texcoord1.xy * _DetailAlbedoMap_ST.xy + _DetailAlbedoMap_ST.zw;
        //texcoords.zw = d.texcoord0.xy * _DetailAlbedoMap_ST.xy + _DetailAlbedoMap_ST.zw;

//SEE: https://github.com/Unity-Technologies/Graphics/blob/569f8878feb8f4340d6de66efa151d0fc1b79c77/Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl

        float2 uv = texcoords.xy;

		#if defined(_PARALLAXMAP)
		    ApplyPerPixelDisplacement(d.tangentSpaceViewDir, uv);
		#endif



    	half4 albedoAlpha =  SampleAlbedoAlpha(uv, TEXTURE2D_ARGS(_BaseMap, sampler_BaseMap));
	    o.Alpha = Alpha(albedoAlpha.a, _BaseColor, _Cutoff);

	    half4 specGloss = SampleMetallicSpecGloss(uv, albedoAlpha.a);
	    o.Albedo = albedoAlpha.rgb * _BaseColor.rgb;
	    o.Albedo = AlphaModulate(o.Albedo, o.Alpha);


		#if _SPECULAR_SETUP
		    o.Metallic = half(1.0);
		    o.Specular = specGloss.rgb;
		#else
		    o.Metallic = specGloss.r;
		    o.Specular = half3(0.0, 0.0, 0.0);
		#endif


        o.Smoothness = specGloss.a;
        o.Normal = SampleNormal(uv, TEXTURE2D_ARGS(_BumpMap, sampler_BumpMap), _BumpScale);
	    o.Occlusion = SampleOcclusion(uv);
	    o.Emission = SampleEmission(uv, _EmissionColor.rgb, TEXTURE2D_ARGS(_EmissionMap, sampler_EmissionMap));

		#if defined(_DETAIL)
		    half detailMask = SAMPLE_TEXTURE2D(_DetailMask, sampler_DetailMask, uv).a;
		    float2 detailUv = uv * _DetailAlbedoMap_ST.xy + _DetailAlbedoMap_ST.zw;
		    o.Albedo = ApplyDetailAlbedo(detailUv, o.Albedo, detailMask);
		    o.Normal = ApplyDetailNormal(detailUv, o.Normal, detailMask);
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
            // d.texcoord1 = i.texcoord1;
            // d.texcoord2 = i.texcoord2;

            // #if %TEXCOORD3REQUIREKEY%
            // d.texcoord3 = i.texcoord3;
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
         

            
         #if _PASSSHADOW
            float3 _LightDirection;
         #endif

         // vertex shader
         VertexToPixel Vert (VertexData v)
         {
           
           VertexToPixel o = (VertexToPixel)0;

           UNITY_SETUP_INSTANCE_ID(v);
           UNITY_TRANSFER_INSTANCE_ID(v, o);
           UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);


#if !_TESSELLATION_ON
           ChainModifyVertex(v, o, _Time);
#endif

            o.texcoord0 = v.texcoord0;
           // o.texcoord1 = v.texcoord1;
           // o.texcoord2 = v.texcoord2;

           // #if %TEXCOORD3REQUIREKEY%
           // o.texcoord3 = v.texcoord3;
           // #endif

           // #if %VERTEXCOLORREQUIREKEY%
           // o.vertexColor = v.vertexColor;
           // #endif
           
           VertexPositionInputs vertexInput = GetVertexPositionInputs(v.vertex.xyz);
           o.worldPos = TransformObjectToWorld(v.vertex.xyz);
           o.worldNormal = TransformObjectToWorldNormal(v.normal);
           o.worldTangent = float4(TransformObjectToWorldDir(v.tangent.xyz), v.tangent.w);


          #if _PASSSHADOW
              // Define shadow pass specific clip position for Universal
              o.pos = TransformWorldToHClip(ApplyShadowBias(o.worldPos, o.worldNormal, _LightDirection));
              #if UNITY_REVERSED_Z
                  o.pos.z = min(o.pos.z, o.pos.w * UNITY_NEAR_CLIP_VALUE);
              #else
                  o.pos.z = max(o.pos.z, o.pos.w * UNITY_NEAR_CLIP_VALUE);
              #endif
          #elif _PASSMETA
              o.pos = MetaVertexPosition(float4(v.vertex.xyz, 0), v.texcoord1.xy, v.texcoord2.xy, unity_LightmapST, unity_DynamicLightmapST);
          #else
              o.pos = TransformWorldToHClip(o.worldPos);
          #endif


          // #if %SCREENPOSREQUIREKEY%
          // o.screenPos = ComputeScreenPos(o.pos, _ProjectionParams.x);
          // #endif

          #if _PASSFORWARD || _PASSGBUFFER
              float2 uv1 = v.texcoord1.xy;
              OUTPUT_LIGHTMAP_UV(uv1, unity_LightmapST, o.lightmapUV);
              // o.texcoord1.xy = uv1;
              OUTPUT_SH(o.worldNormal, o.sh);
          #endif

          #ifdef VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
              #if _BAKEDLIT
                 half3 vertexLight = 0;
              #else
                 half3 vertexLight = VertexLighting(o.worldPos, o.worldNormal);
              #endif
              half fogFactor = ComputeFogFactor(o.pos.z);
              o.fogFactorAndVertexLight = half4(fogFactor, vertexLight);
          #endif

          #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
             o.shadowCoord = GetShadowCoord(vertexInput);
          #endif

           return o;
         }


            

            // fragment shader
            half4 Frag (VertexToPixel IN
            #ifdef _DEPTHOFFSET_ON
              , out float outputDepth : SV_Depth
            #endif
            #if NEED_FACING
               , bool facing : SV_IsFrontFace
            #endif
            ) : SV_Target
            {
               UNITY_SETUP_INSTANCE_ID(IN);
               UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);

               ShaderData d = CreateShaderData(IN
                  #if NEED_FACING
                     , facing
                  #endif
               );
               Surface l = (Surface)0;

               #ifdef _DEPTHOFFSET_ON
                  l.outputDepth = outputDepth;
               #endif

               l.Albedo = half3(0.5, 0.5, 0.5);
               l.Normal = float3(0,0,1);
               l.Occlusion = 1;
               l.Alpha = 1;

               ChainSurfaceFunction(l, d);

               #ifdef _DEPTHOFFSET_ON
                  outputDepth = l.outputDepth;
               #endif

               return 0;

            }

         ENDHLSL

      }


      
        Pass
        {
            Name "Meta"
            Tags 
            { 
                "LightMode" = "Meta"
            }

             // Render State
            Cull Off
            // ColorMask: <None>

                Cull [_Cull]


            HLSLPROGRAM

               #pragma vertex Vert
   #pragma fragment Frag

            #pragma target 3.0

            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
        
            #define SHADERPASS_META
            #define _PASSMETA 1


            
            #pragma shader_feature_local _NORMALMAP
            #pragma shader_feature_local _PARALLAXMAP
            #pragma shader_feature_local _RECEIVE_SHADOWS_OFF
            #pragma shader_feature_local _ _DETAIL_MULX2 _DETAIL_SCALED
            #pragma shader_feature_local_fragment _SURFACE_TYPE_TRANSPARENT
            #pragma shader_feature_local_fragment _ALPHATEST_ON
            #pragma shader_feature_local_fragment _ _ALPHAPREMULTIPLY_ON _ALPHAMODULATE_ON
            #pragma shader_feature_local_fragment _EMISSION
            #pragma shader_feature_local_fragment _METALLICSPECGLOSSMAP
            #pragma shader_feature_local_fragment _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
            #pragma shader_feature_local_fragment _OCCLUSIONMAP
            #pragma shader_feature_local_fragment _SPECULARHIGHLIGHTS_OFF
            #pragma shader_feature_local_fragment _ENVIRONMENTREFLECTIONS_OFF
            #pragma shader_feature_local_fragment _SPECULAR_SETUP


        #pragma shader_feature_local_fragment _OBSTRUCTION_CURVE
        #pragma shader_feature_local_fragment _DISSOLVEMASK
        #pragma shader_feature_local_fragment _ZONING
        #pragma shader_feature_local_fragment _REPLACEMENT
        #pragma shader_feature_local_fragment _PLAYERINDEPENDENT


   #define _URP 1



            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Version.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/MetaInput.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"
        

                  #undef WorldNormalVector
      #define WorldNormalVector(data, normal) mul(normal, data.TBNMatrix)
      
      #define UnityObjectToWorldNormal(normal) mul(GetObjectToWorldMatrix(), normal)

      #define _WorldSpaceLightPos0 _MainLightPosition
      
      #define UNITY_DECLARE_TEX2D(name) TEXTURE2D(name); SAMPLER(sampler##name);
      #define UNITY_DECLARE_TEX2D_NOSAMPLER(name) TEXTURE2D(name);
      #define UNITY_DECLARE_TEX2DARRAY(name) TEXTURE2D_ARRAY(name); SAMPLER(sampler##name);
      #define UNITY_DECLARE_TEX2DARRAY_NOSAMPLER(name) TEXTURE2D_ARRAY(name);

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

      

      // data across stages, stripped like the above.
      struct VertexToPixel
      {
         float4 pos : SV_POSITION;
         float3 worldPos : TEXCOORD0;
         float3 worldNormal : TEXCOORD1;
         float4 worldTangent : TEXCOORD2;
          float4 texcoord0 : TEXCOORD3;
         // float4 texcoord1 : TEXCOORD4;
         // float4 texcoord2 : TEXCOORD5;

         // #if %TEXCOORD3REQUIREKEY%
         // float4 texcoord3 : TEXCOORD6;
         // #endif

         // #if %SCREENPOSREQUIREKEY%
         // float4 screenPos : TEXCOORD7;
         // #endif

         // #if %VERTEXCOLORREQUIREKEY%
         // half4 vertexColor : COLOR;
         // #endif

         // #if %EXTRAV2F0REQUIREKEY%
         // float4 extraV2F0 : TEXCOORD12;
         // #endif

         // #if %EXTRAV2F1REQUIREKEY%
         // float4 extraV2F1 : TEXCOORD13;
         // #endif

         // #if %EXTRAV2F2REQUIREKEY%
         // float4 extraV2F2 : TEXCOORD14;
         // #endif

         // #if %EXTRAV2F3REQUIREKEY%
         // float4 extraV2F3 : TEXCOORD15;
         // #endif

         // #if %EXTRAV2F4REQUIREKEY%
         // float4 extraV2F4 : TEXCOORD16;
         // #endif

         // #if %EXTRAV2F5REQUIREKEY%
         // float4 extraV2F5 : TEXCOORD17;
         // #endif

         // #if %EXTRAV2F6REQUIREKEY%
         // float4 extraV2F6 : TEXCOORD18;
         // #endif

         // #if %EXTRAV2F7REQUIREKEY%
         // float4 extraV2F7 : TEXCOORD19;
         // #endif
            
         #if defined(LIGHTMAP_ON)
            float2 lightmapUV : TEXCOORD8;
         #endif
         #if !defined(LIGHTMAP_ON)
            float3 sh : TEXCOORD9;
         #endif

         #ifdef VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
            float4 fogFactorAndVertexLight : TEXCOORD10;
         #endif

         #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
           float4 shadowCoord : TEXCOORD11;
         #endif

         #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : CUSTOM_INSTANCE_ID;
         #endif
         #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
         #endif
         #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
         #endif
         #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
         #endif
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
               // float4 texcoord3 : TEXCOORD3;
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
               // float4 texcoord3 : TEXCOORD3;
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

               

//SEE: https://github.com/Unity-Technologies/Graphics/blob/569f8878feb8f4340d6de66efa151d0fc1b79c77/Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl
	float4 _BaseMap_ST;
	float4 _DetailAlbedoMap_ST;
	half4 _BaseColor;
	half4 _SpecColor;
	half4 _EmissionColor;
	half _Cutoff;
	half _Smoothness;
	half _Metallic;
	half _BumpScale;
	half _Parallax;
	half _OcclusionStrength;
	//half _ClearCoatMask;
	//half _ClearCoatSmoothness;
	half _DetailAlbedoMapScale;
	half _DetailNormalMapScale;
	half _Surface;


//SEE: https://github.com/Unity-Technologies/Graphics/blob/86cdbd182b8fa8aeda2ff536434f9456f3e5029b/Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl
	float _AlphaToMaskAvailable;



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

    //////////////////////
    // Texture Sampler:
    //////////////////////


//SEE: https://github.com/Unity-Technologies/Graphics/blob/6fdc7996098aa184495c0a3c0da10135bfe18340/Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl
	TEXTURE2D(_BaseMap);
	SAMPLER(sampler_BaseMap);
	float4 _BaseMap_TexelSize;
	float4 _BaseMap_MipInfo;
	TEXTURE2D(_BumpMap);
	SAMPLER(sampler_BumpMap);
	TEXTURE2D(_EmissionMap);
	SAMPLER(sampler_EmissionMap);

//SEE: https://github.com/Unity-Technologies/Graphics/blob/569f8878feb8f4340d6de66efa151d0fc1b79c77/Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl
	TEXTURE2D(_ParallaxMap);        SAMPLER(sampler_ParallaxMap);
	TEXTURE2D(_OcclusionMap);       SAMPLER(sampler_OcclusionMap);
	TEXTURE2D(_DetailMask);         SAMPLER(sampler_DetailMask);
	TEXTURE2D(_DetailAlbedoMap);    SAMPLER(sampler_DetailAlbedoMap);
	TEXTURE2D(_DetailNormalMap);    SAMPLER(sampler_DetailNormalMap);
	TEXTURE2D(_MetallicGlossMap);   SAMPLER(sampler_MetallicGlossMap);
	TEXTURE2D(_SpecGlossMap);       SAMPLER(sampler_SpecGlossMap);
	TEXTURE2D(_ClearCoatMap);       SAMPLER(sampler_ClearCoatMap);

    //////////////////////
    // Extra Functions:
    //////////////////////

	#if defined(_DETAIL_MULX2) || defined(_DETAIL_SCALED)
		#define _DETAIL
	#endif

	#if _SPECULAR_SETUP
		#define _USESPECULAR 1
	#else
		#undef _USESPECULAR
	#endif

//See: https://github.com/Unity-Technologies/Graphics/blob/4b98cece5623e02f03c9ff311bca0f749ba4fd94/Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl
	float SharpenAlphaMy(float alpha, float alphaClipTreshold)
	{
	    return saturate((alpha - alphaClipTreshold) / max(fwidth(alpha), 0.0001) + 0.5);
	}


// See: https://github.com/Unity-Technologies/Graphics/blob/223d8105e68c5a808027d78f39cb4e2545fd6276/Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderVariablesFunctions.hlsl
	half3 AlphaModulate(half3 albedo, half alpha)
	{
		#if defined(_ALPHAMODULATE_ON)
		    return lerp(half3(1.0, 1.0, 1.0), albedo, alpha);
		#else
		    return albedo;
		#endif
	}


	#if defined(_ALPHATEST_ON)
		bool IsAlphaToMaskAvailable()
		{
		    return (_AlphaToMaskAvailable != 0.0);
		}
		half AlphaClip(half alpha, half cutoff)
		{
		    half clippedAlpha = (alpha >= cutoff) ? float(alpha) : 0.0;

		    half alphaToCoverageAlpha = SharpenAlphaMy(alpha, cutoff);

		    alpha = IsAlphaToMaskAvailable() ? alphaToCoverageAlpha : clippedAlpha;
		    clip(alpha - 0.0001);

		    return alpha;
		}
	#endif


// https://github.com/Unity-Technologies/Graphics/blob/632f80e011f18ea537ee6e2f0be3ff4f4dea6a11/Packages/com.unity.render-pipelines.universal/ShaderLibrary/Debug/DebuggingCommon.hlsl#L122
int _DebugSceneOverrideMode;
bool IsAlphaDiscardEnabledMy()
{
    #if defined(DEBUG_DISPLAY)
    return (_DebugSceneOverrideMode == DEBUGSCENEOVERRIDEMODE_NONE);
    #else
    return true;
    #endif
}


//See: https://github.com/Unity-Technologies/Graphics/blob/6fdc7996098aa184495c0a3c0da10135bfe18340/Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl

	half Alpha(half albedoAlpha, half4 color, half cutoff)
	{
		#if !defined(_SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A) && !defined(_GLOSSINESS_FROM_BASE_ALPHA)
		    half alpha = albedoAlpha * color.a;
		#else
		    half alpha = color.a;
		#endif

		    //alpha = AlphaDiscard(alpha, cutoff);


			#ifdef _ALPHATEST_ON
			    if (IsAlphaDiscardEnabledMy())
			        alpha = AlphaClip(alpha, cutoff);
			#endif

		    return alpha;
	}

	half4 SampleAlbedoAlpha(float2 uv, TEXTURE2D_PARAM(albedoAlphaMap, sampler_albedoAlphaMap))
	{
	    return half4(SAMPLE_TEXTURE2D(albedoAlphaMap, sampler_albedoAlphaMap, uv));
	}

	half3 SampleNormal(float2 uv, TEXTURE2D_PARAM(bumpMap, sampler_bumpMap), half scale = half(1.0))
	{
	#ifdef _NORMALMAP
	    half4 n = SAMPLE_TEXTURE2D(bumpMap, sampler_bumpMap, uv);
	    #if BUMP_SCALE_NOT_SUPPORTED
	        return UnpackNormal(n);
	    #else
	        return UnpackNormalScale(n, scale);
	    #endif
	#else
	    return half3(0.0h, 0.0h, 1.0h);
	#endif
	}

	half3 SampleEmission(float2 uv, half3 emissionColor, TEXTURE2D_PARAM(emissionMap, sampler_emissionMap))
	{
	#ifndef _EMISSION
	    return 0;
	#else
	    return SAMPLE_TEXTURE2D(emissionMap, sampler_emissionMap, uv).rgb * emissionColor;
	#endif
	}


//SEE: https://github.com/Unity-Technologies/Graphics/blob/632f80e011f18ea537ee6e2f0be3ff4f4dea6a11/Packages/com.unity.render-pipelines.core/ShaderLibrary/ParallaxMapping.hlsl

	#ifndef BUILTIN_TARGET_API
		half2 ParallaxOffset1Step(half height, half amplitude, half3 viewDirTS)
		{
		    height = height * amplitude - amplitude / 2.0;
		    half3 v = normalize(viewDirTS);
		    v.z += 0.42;
		    return height * (v.xy / v.z);
		}
	#endif

	float2 ParallaxMapping(TEXTURE2D_PARAM(heightMap, sampler_heightMap), half3 viewDirTS, half scale, float2 uv)
	{
	    half h = SAMPLE_TEXTURE2D(heightMap, sampler_heightMap, uv).g;
	    float2 offset = ParallaxOffset1Step(h, scale, viewDirTS);
	    return offset;
	}



//SEE: https://github.com/Unity-Technologies/Graphics/blob/569f8878feb8f4340d6de66efa151d0fc1b79c77/Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl

	#ifdef _SPECULAR_SETUP
	    #define SAMPLE_METALLICSPECULAR(uv) SAMPLE_TEXTURE2D(_SpecGlossMap, sampler_SpecGlossMap, uv)
	#else
	    #define SAMPLE_METALLICSPECULAR(uv) SAMPLE_TEXTURE2D(_MetallicGlossMap, sampler_MetallicGlossMap, uv)
	#endif

	half4 SampleMetallicSpecGloss(float2 uv, half albedoAlpha)
	{
	    half4 specGloss;

		#ifdef _METALLICSPECGLOSSMAP
		    specGloss = half4(SAMPLE_METALLICSPECULAR(uv));
		    #ifdef _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
		        specGloss.a = albedoAlpha * _Smoothness;
		    #else
		        specGloss.a *= _Smoothness;
		    #endif
		#else // _METALLICSPECGLOSSMAP
		    #if _SPECULAR_SETUP
		        specGloss.rgb = _SpecColor.rgb;
		    #else
		        specGloss.rgb = _Metallic.rrr;
		    #endif

		    #ifdef _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
		        specGloss.a = albedoAlpha * _Smoothness;
		    #else
		        specGloss.a = _Smoothness;
		    #endif
		#endif

	    return specGloss;
	}

	half SampleOcclusion(float2 uv)
	{
	    #ifdef _OCCLUSIONMAP
	        half occ = SAMPLE_TEXTURE2D(_OcclusionMap, sampler_OcclusionMap, uv).g;
	        return LerpWhiteTo(occ, _OcclusionStrength);
	    #else
	        return half(1.0);
	    #endif
	}


	half2 SampleClearCoat(float2 uv)
	{
		#if defined(_CLEARCOAT) || defined(_CLEARCOATMAP)
		    half2 clearCoatMaskSmoothness = half2(_ClearCoatMask, _ClearCoatSmoothness);

		#if defined(_CLEARCOATMAP)
		    clearCoatMaskSmoothness *= SAMPLE_TEXTURE2D(_ClearCoatMap, sampler_ClearCoatMap, uv).rg;
		#endif

		    return clearCoatMaskSmoothness;
		#else
		    return half2(0.0, 1.0);
		#endif  // _CLEARCOAT
	}

	void ApplyPerPixelDisplacement(half3 viewDirTS, inout float2 uv)
	{
		#if defined(_PARALLAXMAP)
		    uv += ParallaxMapping(TEXTURE2D_ARGS(_ParallaxMap, sampler_ParallaxMap), viewDirTS, _Parallax, uv);
		#endif
	}

	half3 ScaleDetailAlbedo(half3 detailAlbedo, half scale)
	{
	    return half(2.0) * detailAlbedo * scale - scale + half(1.0);
	}

	half3 ApplyDetailAlbedo(float2 detailUv, half3 albedo, half detailMask)
	{
		#if defined(_DETAIL)
		    half3 detailAlbedo = SAMPLE_TEXTURE2D(_DetailAlbedoMap, sampler_DetailAlbedoMap, detailUv).rgb;

		    // In order to have same performance as builtin, we do scaling only if scale is not 1.0 (Scaled version has 6 additional instructions)
		#if defined(_DETAIL_SCALED)
		    detailAlbedo = ScaleDetailAlbedo(detailAlbedo, _DetailAlbedoMapScale);
		#else
		    detailAlbedo = half(2.0) * detailAlbedo;
		#endif

		    return albedo * LerpWhiteTo(detailAlbedo, detailMask);
		#else
		    return albedo;
		#endif
	}

	half3 ApplyDetailNormal(float2 detailUv, half3 normalTS, half detailMask)
	{
		#if defined(_DETAIL)
		#if BUMP_SCALE_NOT_SUPPORTED
		    half3 detailNormalTS = UnpackNormal(SAMPLE_TEXTURE2D(_DetailNormalMap, sampler_DetailNormalMap, detailUv));
		#else
		    half3 detailNormalTS = UnpackNormalScale(SAMPLE_TEXTURE2D(_DetailNormalMap, sampler_DetailNormalMap, detailUv), _DetailNormalMapScale);
		#endif

		    // With UNITY_NO_DXT5nm unpacked vector is not normalized for BlendNormalRNM
		    // For visual consistancy we going to do in all cases
		    detailNormalTS = normalize(detailNormalTS);

		    return lerp(normalTS, BlendNormalRNM(normalTS, detailNormalTS), detailMask); // todo: detailMask should lerp the angle of the quaternion rotation, not the normals
		#else
		    return normalTS;
		#endif
	}





    //////////////////////
    // Main Code:
    //////////////////////

	void Ext_SurfaceFunction0 (inout Surface o, ShaderData d)
	{
        float4 texcoords;
        texcoords.xy = d.texcoord0.xy * _BaseMap_ST.xy + _BaseMap_ST.zw; 
        //texcoords.zw = (_UVSec == 0) ? d.texcoord0.xy * _DetailAlbedoMap_ST.xy + _DetailAlbedoMap_ST.zw : d.texcoord1.xy * _DetailAlbedoMap_ST.xy + _DetailAlbedoMap_ST.zw;
        //texcoords.zw = d.texcoord0.xy * _DetailAlbedoMap_ST.xy + _DetailAlbedoMap_ST.zw;

//SEE: https://github.com/Unity-Technologies/Graphics/blob/569f8878feb8f4340d6de66efa151d0fc1b79c77/Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl

        float2 uv = texcoords.xy;

		#if defined(_PARALLAXMAP)
		    ApplyPerPixelDisplacement(d.tangentSpaceViewDir, uv);
		#endif



    	half4 albedoAlpha =  SampleAlbedoAlpha(uv, TEXTURE2D_ARGS(_BaseMap, sampler_BaseMap));
	    o.Alpha = Alpha(albedoAlpha.a, _BaseColor, _Cutoff);

	    half4 specGloss = SampleMetallicSpecGloss(uv, albedoAlpha.a);
	    o.Albedo = albedoAlpha.rgb * _BaseColor.rgb;
	    o.Albedo = AlphaModulate(o.Albedo, o.Alpha);


		#if _SPECULAR_SETUP
		    o.Metallic = half(1.0);
		    o.Specular = specGloss.rgb;
		#else
		    o.Metallic = specGloss.r;
		    o.Specular = half3(0.0, 0.0, 0.0);
		#endif


        o.Smoothness = specGloss.a;
        o.Normal = SampleNormal(uv, TEXTURE2D_ARGS(_BumpMap, sampler_BumpMap), _BumpScale);
	    o.Occlusion = SampleOcclusion(uv);
	    o.Emission = SampleEmission(uv, _EmissionColor.rgb, TEXTURE2D_ARGS(_EmissionMap, sampler_EmissionMap));

		#if defined(_DETAIL)
		    half detailMask = SAMPLE_TEXTURE2D(_DetailMask, sampler_DetailMask, uv).a;
		    float2 detailUv = uv * _DetailAlbedoMap_ST.xy + _DetailAlbedoMap_ST.zw;
		    o.Albedo = ApplyDetailAlbedo(detailUv, o.Albedo, detailMask);
		    o.Normal = ApplyDetailNormal(detailUv, o.Normal, detailMask);
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
            // d.texcoord1 = i.texcoord1;
            // d.texcoord2 = i.texcoord2;

            // #if %TEXCOORD3REQUIREKEY%
            // d.texcoord3 = i.texcoord3;
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
         

            
         #if _PASSSHADOW
            float3 _LightDirection;
         #endif

         // vertex shader
         VertexToPixel Vert (VertexData v)
         {
           
           VertexToPixel o = (VertexToPixel)0;

           UNITY_SETUP_INSTANCE_ID(v);
           UNITY_TRANSFER_INSTANCE_ID(v, o);
           UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);


#if !_TESSELLATION_ON
           ChainModifyVertex(v, o, _Time);
#endif

            o.texcoord0 = v.texcoord0;
           // o.texcoord1 = v.texcoord1;
           // o.texcoord2 = v.texcoord2;

           // #if %TEXCOORD3REQUIREKEY%
           // o.texcoord3 = v.texcoord3;
           // #endif

           // #if %VERTEXCOLORREQUIREKEY%
           // o.vertexColor = v.vertexColor;
           // #endif
           
           VertexPositionInputs vertexInput = GetVertexPositionInputs(v.vertex.xyz);
           o.worldPos = TransformObjectToWorld(v.vertex.xyz);
           o.worldNormal = TransformObjectToWorldNormal(v.normal);
           o.worldTangent = float4(TransformObjectToWorldDir(v.tangent.xyz), v.tangent.w);


          #if _PASSSHADOW
              // Define shadow pass specific clip position for Universal
              o.pos = TransformWorldToHClip(ApplyShadowBias(o.worldPos, o.worldNormal, _LightDirection));
              #if UNITY_REVERSED_Z
                  o.pos.z = min(o.pos.z, o.pos.w * UNITY_NEAR_CLIP_VALUE);
              #else
                  o.pos.z = max(o.pos.z, o.pos.w * UNITY_NEAR_CLIP_VALUE);
              #endif
          #elif _PASSMETA
              o.pos = MetaVertexPosition(float4(v.vertex.xyz, 0), v.texcoord1.xy, v.texcoord2.xy, unity_LightmapST, unity_DynamicLightmapST);
          #else
              o.pos = TransformWorldToHClip(o.worldPos);
          #endif


          // #if %SCREENPOSREQUIREKEY%
          // o.screenPos = ComputeScreenPos(o.pos, _ProjectionParams.x);
          // #endif

          #if _PASSFORWARD || _PASSGBUFFER
              float2 uv1 = v.texcoord1.xy;
              OUTPUT_LIGHTMAP_UV(uv1, unity_LightmapST, o.lightmapUV);
              // o.texcoord1.xy = uv1;
              OUTPUT_SH(o.worldNormal, o.sh);
          #endif

          #ifdef VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
              #if _BAKEDLIT
                 half3 vertexLight = 0;
              #else
                 half3 vertexLight = VertexLighting(o.worldPos, o.worldNormal);
              #endif
              half fogFactor = ComputeFogFactor(o.pos.z);
              o.fogFactorAndVertexLight = half4(fogFactor, vertexLight);
          #endif

          #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
             o.shadowCoord = GetShadowCoord(vertexInput);
          #endif

           return o;
         }


            

            // fragment shader
            half4 Frag (VertexToPixel IN
               #if NEED_FACING
                  , bool facing : SV_IsFrontFace
               #endif
            ) : SV_Target
            {
               UNITY_SETUP_INSTANCE_ID(IN);

               ShaderData d = CreateShaderData(IN
                  #if NEED_FACING
                     , facing
                  #endif
               );

               Surface l = (Surface)0;

               l.Albedo = half3(0.5, 0.5, 0.5);
               l.Normal = float3(0,0,1);
               l.Occlusion = 1;
               l.Alpha = 1;

               ChainSurfaceFunction(l, d);

               MetaInput metaInput = (MetaInput)0;
               metaInput.Albedo = l.Albedo;
               metaInput.Emission = l.Emission;

               return MetaFragment(metaInput);

            }

         ENDHLSL

      }


      
        Pass
        {
            Name "DepthNormals"
            Tags
            {
               "LightMode" = "DepthNormals"
            }
    
            // Render State
            Cull Back
            Blend One Zero
            ZTest LEqual
            ZWrite On

                Cull [_Cull]


            HLSLPROGRAM

               #pragma vertex Vert
   #pragma fragment Frag

            #pragma target 3.0

            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma multi_compile_fog
            #pragma multi_compile_instancing
            #pragma multi_compile _ DOTS_INSTANCING_ON
        
            #define SHADERPASS SHADERPASS_DEPTHNORMALSONLY
            #define _PASSDEPTH 1
            #define _PASSDEPTHNORMALS 1


            
            #pragma shader_feature_local _NORMALMAP
            #pragma shader_feature_local _PARALLAXMAP
            #pragma shader_feature_local _RECEIVE_SHADOWS_OFF
            #pragma shader_feature_local _ _DETAIL_MULX2 _DETAIL_SCALED
            #pragma shader_feature_local_fragment _SURFACE_TYPE_TRANSPARENT
            #pragma shader_feature_local_fragment _ALPHATEST_ON
            #pragma shader_feature_local_fragment _ _ALPHAPREMULTIPLY_ON _ALPHAMODULATE_ON
            #pragma shader_feature_local_fragment _EMISSION
            #pragma shader_feature_local_fragment _METALLICSPECGLOSSMAP
            #pragma shader_feature_local_fragment _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
            #pragma shader_feature_local_fragment _OCCLUSIONMAP
            #pragma shader_feature_local_fragment _SPECULARHIGHLIGHTS_OFF
            #pragma shader_feature_local_fragment _ENVIRONMENTREFLECTIONS_OFF
            #pragma shader_feature_local_fragment _SPECULAR_SETUP


        #pragma shader_feature_local_fragment _OBSTRUCTION_CURVE
        #pragma shader_feature_local_fragment _DISSOLVEMASK
        #pragma shader_feature_local_fragment _ZONING
        #pragma shader_feature_local_fragment _REPLACEMENT
        #pragma shader_feature_local_fragment _PLAYERINDEPENDENT


   #define _URP 1


            // this has to be here or specular color will be ignored. Not in SG code
            #if _SIMPLELIT
               #define _SPECULAR_COLOR
            #endif


            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Version.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"


        

               #undef WorldNormalVector
      #define WorldNormalVector(data, normal) mul(normal, data.TBNMatrix)
      
      #define UnityObjectToWorldNormal(normal) mul(GetObjectToWorldMatrix(), normal)

      #define _WorldSpaceLightPos0 _MainLightPosition
      
      #define UNITY_DECLARE_TEX2D(name) TEXTURE2D(name); SAMPLER(sampler##name);
      #define UNITY_DECLARE_TEX2D_NOSAMPLER(name) TEXTURE2D(name);
      #define UNITY_DECLARE_TEX2DARRAY(name) TEXTURE2D_ARRAY(name); SAMPLER(sampler##name);
      #define UNITY_DECLARE_TEX2DARRAY_NOSAMPLER(name) TEXTURE2D_ARRAY(name);

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

      

      // data across stages, stripped like the above.
      struct VertexToPixel
      {
         float4 pos : SV_POSITION;
         float3 worldPos : TEXCOORD0;
         float3 worldNormal : TEXCOORD1;
         float4 worldTangent : TEXCOORD2;
          float4 texcoord0 : TEXCOORD3;
         // float4 texcoord1 : TEXCOORD4;
         // float4 texcoord2 : TEXCOORD5;

         // #if %TEXCOORD3REQUIREKEY%
         // float4 texcoord3 : TEXCOORD6;
         // #endif

         // #if %SCREENPOSREQUIREKEY%
         // float4 screenPos : TEXCOORD7;
         // #endif

         // #if %VERTEXCOLORREQUIREKEY%
         // half4 vertexColor : COLOR;
         // #endif

         // #if %EXTRAV2F0REQUIREKEY%
         // float4 extraV2F0 : TEXCOORD12;
         // #endif

         // #if %EXTRAV2F1REQUIREKEY%
         // float4 extraV2F1 : TEXCOORD13;
         // #endif

         // #if %EXTRAV2F2REQUIREKEY%
         // float4 extraV2F2 : TEXCOORD14;
         // #endif

         // #if %EXTRAV2F3REQUIREKEY%
         // float4 extraV2F3 : TEXCOORD15;
         // #endif

         // #if %EXTRAV2F4REQUIREKEY%
         // float4 extraV2F4 : TEXCOORD16;
         // #endif

         // #if %EXTRAV2F5REQUIREKEY%
         // float4 extraV2F5 : TEXCOORD17;
         // #endif

         // #if %EXTRAV2F6REQUIREKEY%
         // float4 extraV2F6 : TEXCOORD18;
         // #endif

         // #if %EXTRAV2F7REQUIREKEY%
         // float4 extraV2F7 : TEXCOORD19;
         // #endif
            
         #if defined(LIGHTMAP_ON)
            float2 lightmapUV : TEXCOORD8;
         #endif
         #if !defined(LIGHTMAP_ON)
            float3 sh : TEXCOORD9;
         #endif

         #ifdef VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
            float4 fogFactorAndVertexLight : TEXCOORD10;
         #endif

         #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
           float4 shadowCoord : TEXCOORD11;
         #endif

         #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : CUSTOM_INSTANCE_ID;
         #endif
         #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
         #endif
         #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
         #endif
         #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
         #endif
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
               // float4 texcoord3 : TEXCOORD3;
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
               // float4 texcoord3 : TEXCOORD3;
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

            

//SEE: https://github.com/Unity-Technologies/Graphics/blob/569f8878feb8f4340d6de66efa151d0fc1b79c77/Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl
	float4 _BaseMap_ST;
	float4 _DetailAlbedoMap_ST;
	half4 _BaseColor;
	half4 _SpecColor;
	half4 _EmissionColor;
	half _Cutoff;
	half _Smoothness;
	half _Metallic;
	half _BumpScale;
	half _Parallax;
	half _OcclusionStrength;
	//half _ClearCoatMask;
	//half _ClearCoatSmoothness;
	half _DetailAlbedoMapScale;
	half _DetailNormalMapScale;
	half _Surface;


//SEE: https://github.com/Unity-Technologies/Graphics/blob/86cdbd182b8fa8aeda2ff536434f9456f3e5029b/Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl
	float _AlphaToMaskAvailable;



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

    //////////////////////
    // Texture Sampler:
    //////////////////////


//SEE: https://github.com/Unity-Technologies/Graphics/blob/6fdc7996098aa184495c0a3c0da10135bfe18340/Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl
	TEXTURE2D(_BaseMap);
	SAMPLER(sampler_BaseMap);
	float4 _BaseMap_TexelSize;
	float4 _BaseMap_MipInfo;
	TEXTURE2D(_BumpMap);
	SAMPLER(sampler_BumpMap);
	TEXTURE2D(_EmissionMap);
	SAMPLER(sampler_EmissionMap);

//SEE: https://github.com/Unity-Technologies/Graphics/blob/569f8878feb8f4340d6de66efa151d0fc1b79c77/Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl
	TEXTURE2D(_ParallaxMap);        SAMPLER(sampler_ParallaxMap);
	TEXTURE2D(_OcclusionMap);       SAMPLER(sampler_OcclusionMap);
	TEXTURE2D(_DetailMask);         SAMPLER(sampler_DetailMask);
	TEXTURE2D(_DetailAlbedoMap);    SAMPLER(sampler_DetailAlbedoMap);
	TEXTURE2D(_DetailNormalMap);    SAMPLER(sampler_DetailNormalMap);
	TEXTURE2D(_MetallicGlossMap);   SAMPLER(sampler_MetallicGlossMap);
	TEXTURE2D(_SpecGlossMap);       SAMPLER(sampler_SpecGlossMap);
	TEXTURE2D(_ClearCoatMap);       SAMPLER(sampler_ClearCoatMap);

    //////////////////////
    // Extra Functions:
    //////////////////////

	#if defined(_DETAIL_MULX2) || defined(_DETAIL_SCALED)
		#define _DETAIL
	#endif

	#if _SPECULAR_SETUP
		#define _USESPECULAR 1
	#else
		#undef _USESPECULAR
	#endif

//See: https://github.com/Unity-Technologies/Graphics/blob/4b98cece5623e02f03c9ff311bca0f749ba4fd94/Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl
	float SharpenAlphaMy(float alpha, float alphaClipTreshold)
	{
	    return saturate((alpha - alphaClipTreshold) / max(fwidth(alpha), 0.0001) + 0.5);
	}


// See: https://github.com/Unity-Technologies/Graphics/blob/223d8105e68c5a808027d78f39cb4e2545fd6276/Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderVariablesFunctions.hlsl
	half3 AlphaModulate(half3 albedo, half alpha)
	{
		#if defined(_ALPHAMODULATE_ON)
		    return lerp(half3(1.0, 1.0, 1.0), albedo, alpha);
		#else
		    return albedo;
		#endif
	}


	#if defined(_ALPHATEST_ON)
		bool IsAlphaToMaskAvailable()
		{
		    return (_AlphaToMaskAvailable != 0.0);
		}
		half AlphaClip(half alpha, half cutoff)
		{
		    half clippedAlpha = (alpha >= cutoff) ? float(alpha) : 0.0;

		    half alphaToCoverageAlpha = SharpenAlphaMy(alpha, cutoff);

		    alpha = IsAlphaToMaskAvailable() ? alphaToCoverageAlpha : clippedAlpha;
		    clip(alpha - 0.0001);

		    return alpha;
		}
	#endif


// https://github.com/Unity-Technologies/Graphics/blob/632f80e011f18ea537ee6e2f0be3ff4f4dea6a11/Packages/com.unity.render-pipelines.universal/ShaderLibrary/Debug/DebuggingCommon.hlsl#L122
int _DebugSceneOverrideMode;
bool IsAlphaDiscardEnabledMy()
{
    #if defined(DEBUG_DISPLAY)
    return (_DebugSceneOverrideMode == DEBUGSCENEOVERRIDEMODE_NONE);
    #else
    return true;
    #endif
}


//See: https://github.com/Unity-Technologies/Graphics/blob/6fdc7996098aa184495c0a3c0da10135bfe18340/Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl

	half Alpha(half albedoAlpha, half4 color, half cutoff)
	{
		#if !defined(_SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A) && !defined(_GLOSSINESS_FROM_BASE_ALPHA)
		    half alpha = albedoAlpha * color.a;
		#else
		    half alpha = color.a;
		#endif

		    //alpha = AlphaDiscard(alpha, cutoff);


			#ifdef _ALPHATEST_ON
			    if (IsAlphaDiscardEnabledMy())
			        alpha = AlphaClip(alpha, cutoff);
			#endif

		    return alpha;
	}

	half4 SampleAlbedoAlpha(float2 uv, TEXTURE2D_PARAM(albedoAlphaMap, sampler_albedoAlphaMap))
	{
	    return half4(SAMPLE_TEXTURE2D(albedoAlphaMap, sampler_albedoAlphaMap, uv));
	}

	half3 SampleNormal(float2 uv, TEXTURE2D_PARAM(bumpMap, sampler_bumpMap), half scale = half(1.0))
	{
	#ifdef _NORMALMAP
	    half4 n = SAMPLE_TEXTURE2D(bumpMap, sampler_bumpMap, uv);
	    #if BUMP_SCALE_NOT_SUPPORTED
	        return UnpackNormal(n);
	    #else
	        return UnpackNormalScale(n, scale);
	    #endif
	#else
	    return half3(0.0h, 0.0h, 1.0h);
	#endif
	}

	half3 SampleEmission(float2 uv, half3 emissionColor, TEXTURE2D_PARAM(emissionMap, sampler_emissionMap))
	{
	#ifndef _EMISSION
	    return 0;
	#else
	    return SAMPLE_TEXTURE2D(emissionMap, sampler_emissionMap, uv).rgb * emissionColor;
	#endif
	}


//SEE: https://github.com/Unity-Technologies/Graphics/blob/632f80e011f18ea537ee6e2f0be3ff4f4dea6a11/Packages/com.unity.render-pipelines.core/ShaderLibrary/ParallaxMapping.hlsl

	#ifndef BUILTIN_TARGET_API
		half2 ParallaxOffset1Step(half height, half amplitude, half3 viewDirTS)
		{
		    height = height * amplitude - amplitude / 2.0;
		    half3 v = normalize(viewDirTS);
		    v.z += 0.42;
		    return height * (v.xy / v.z);
		}
	#endif

	float2 ParallaxMapping(TEXTURE2D_PARAM(heightMap, sampler_heightMap), half3 viewDirTS, half scale, float2 uv)
	{
	    half h = SAMPLE_TEXTURE2D(heightMap, sampler_heightMap, uv).g;
	    float2 offset = ParallaxOffset1Step(h, scale, viewDirTS);
	    return offset;
	}



//SEE: https://github.com/Unity-Technologies/Graphics/blob/569f8878feb8f4340d6de66efa151d0fc1b79c77/Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl

	#ifdef _SPECULAR_SETUP
	    #define SAMPLE_METALLICSPECULAR(uv) SAMPLE_TEXTURE2D(_SpecGlossMap, sampler_SpecGlossMap, uv)
	#else
	    #define SAMPLE_METALLICSPECULAR(uv) SAMPLE_TEXTURE2D(_MetallicGlossMap, sampler_MetallicGlossMap, uv)
	#endif

	half4 SampleMetallicSpecGloss(float2 uv, half albedoAlpha)
	{
	    half4 specGloss;

		#ifdef _METALLICSPECGLOSSMAP
		    specGloss = half4(SAMPLE_METALLICSPECULAR(uv));
		    #ifdef _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
		        specGloss.a = albedoAlpha * _Smoothness;
		    #else
		        specGloss.a *= _Smoothness;
		    #endif
		#else // _METALLICSPECGLOSSMAP
		    #if _SPECULAR_SETUP
		        specGloss.rgb = _SpecColor.rgb;
		    #else
		        specGloss.rgb = _Metallic.rrr;
		    #endif

		    #ifdef _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
		        specGloss.a = albedoAlpha * _Smoothness;
		    #else
		        specGloss.a = _Smoothness;
		    #endif
		#endif

	    return specGloss;
	}

	half SampleOcclusion(float2 uv)
	{
	    #ifdef _OCCLUSIONMAP
	        half occ = SAMPLE_TEXTURE2D(_OcclusionMap, sampler_OcclusionMap, uv).g;
	        return LerpWhiteTo(occ, _OcclusionStrength);
	    #else
	        return half(1.0);
	    #endif
	}


	half2 SampleClearCoat(float2 uv)
	{
		#if defined(_CLEARCOAT) || defined(_CLEARCOATMAP)
		    half2 clearCoatMaskSmoothness = half2(_ClearCoatMask, _ClearCoatSmoothness);

		#if defined(_CLEARCOATMAP)
		    clearCoatMaskSmoothness *= SAMPLE_TEXTURE2D(_ClearCoatMap, sampler_ClearCoatMap, uv).rg;
		#endif

		    return clearCoatMaskSmoothness;
		#else
		    return half2(0.0, 1.0);
		#endif  // _CLEARCOAT
	}

	void ApplyPerPixelDisplacement(half3 viewDirTS, inout float2 uv)
	{
		#if defined(_PARALLAXMAP)
		    uv += ParallaxMapping(TEXTURE2D_ARGS(_ParallaxMap, sampler_ParallaxMap), viewDirTS, _Parallax, uv);
		#endif
	}

	half3 ScaleDetailAlbedo(half3 detailAlbedo, half scale)
	{
	    return half(2.0) * detailAlbedo * scale - scale + half(1.0);
	}

	half3 ApplyDetailAlbedo(float2 detailUv, half3 albedo, half detailMask)
	{
		#if defined(_DETAIL)
		    half3 detailAlbedo = SAMPLE_TEXTURE2D(_DetailAlbedoMap, sampler_DetailAlbedoMap, detailUv).rgb;

		    // In order to have same performance as builtin, we do scaling only if scale is not 1.0 (Scaled version has 6 additional instructions)
		#if defined(_DETAIL_SCALED)
		    detailAlbedo = ScaleDetailAlbedo(detailAlbedo, _DetailAlbedoMapScale);
		#else
		    detailAlbedo = half(2.0) * detailAlbedo;
		#endif

		    return albedo * LerpWhiteTo(detailAlbedo, detailMask);
		#else
		    return albedo;
		#endif
	}

	half3 ApplyDetailNormal(float2 detailUv, half3 normalTS, half detailMask)
	{
		#if defined(_DETAIL)
		#if BUMP_SCALE_NOT_SUPPORTED
		    half3 detailNormalTS = UnpackNormal(SAMPLE_TEXTURE2D(_DetailNormalMap, sampler_DetailNormalMap, detailUv));
		#else
		    half3 detailNormalTS = UnpackNormalScale(SAMPLE_TEXTURE2D(_DetailNormalMap, sampler_DetailNormalMap, detailUv), _DetailNormalMapScale);
		#endif

		    // With UNITY_NO_DXT5nm unpacked vector is not normalized for BlendNormalRNM
		    // For visual consistancy we going to do in all cases
		    detailNormalTS = normalize(detailNormalTS);

		    return lerp(normalTS, BlendNormalRNM(normalTS, detailNormalTS), detailMask); // todo: detailMask should lerp the angle of the quaternion rotation, not the normals
		#else
		    return normalTS;
		#endif
	}





    //////////////////////
    // Main Code:
    //////////////////////

	void Ext_SurfaceFunction0 (inout Surface o, ShaderData d)
	{
        float4 texcoords;
        texcoords.xy = d.texcoord0.xy * _BaseMap_ST.xy + _BaseMap_ST.zw; 
        //texcoords.zw = (_UVSec == 0) ? d.texcoord0.xy * _DetailAlbedoMap_ST.xy + _DetailAlbedoMap_ST.zw : d.texcoord1.xy * _DetailAlbedoMap_ST.xy + _DetailAlbedoMap_ST.zw;
        //texcoords.zw = d.texcoord0.xy * _DetailAlbedoMap_ST.xy + _DetailAlbedoMap_ST.zw;

//SEE: https://github.com/Unity-Technologies/Graphics/blob/569f8878feb8f4340d6de66efa151d0fc1b79c77/Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl

        float2 uv = texcoords.xy;

		#if defined(_PARALLAXMAP)
		    ApplyPerPixelDisplacement(d.tangentSpaceViewDir, uv);
		#endif



    	half4 albedoAlpha =  SampleAlbedoAlpha(uv, TEXTURE2D_ARGS(_BaseMap, sampler_BaseMap));
	    o.Alpha = Alpha(albedoAlpha.a, _BaseColor, _Cutoff);

	    half4 specGloss = SampleMetallicSpecGloss(uv, albedoAlpha.a);
	    o.Albedo = albedoAlpha.rgb * _BaseColor.rgb;
	    o.Albedo = AlphaModulate(o.Albedo, o.Alpha);


		#if _SPECULAR_SETUP
		    o.Metallic = half(1.0);
		    o.Specular = specGloss.rgb;
		#else
		    o.Metallic = specGloss.r;
		    o.Specular = half3(0.0, 0.0, 0.0);
		#endif


        o.Smoothness = specGloss.a;
        o.Normal = SampleNormal(uv, TEXTURE2D_ARGS(_BumpMap, sampler_BumpMap), _BumpScale);
	    o.Occlusion = SampleOcclusion(uv);
	    o.Emission = SampleEmission(uv, _EmissionColor.rgb, TEXTURE2D_ARGS(_EmissionMap, sampler_EmissionMap));

		#if defined(_DETAIL)
		    half detailMask = SAMPLE_TEXTURE2D(_DetailMask, sampler_DetailMask, uv).a;
		    float2 detailUv = uv * _DetailAlbedoMap_ST.xy + _DetailAlbedoMap_ST.zw;
		    o.Albedo = ApplyDetailAlbedo(detailUv, o.Albedo, detailMask);
		    o.Normal = ApplyDetailNormal(detailUv, o.Normal, detailMask);
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
            // d.texcoord1 = i.texcoord1;
            // d.texcoord2 = i.texcoord2;

            // #if %TEXCOORD3REQUIREKEY%
            // d.texcoord3 = i.texcoord3;
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
         

         
         #if _PASSSHADOW
            float3 _LightDirection;
         #endif

         // vertex shader
         VertexToPixel Vert (VertexData v)
         {
           
           VertexToPixel o = (VertexToPixel)0;

           UNITY_SETUP_INSTANCE_ID(v);
           UNITY_TRANSFER_INSTANCE_ID(v, o);
           UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);


#if !_TESSELLATION_ON
           ChainModifyVertex(v, o, _Time);
#endif

            o.texcoord0 = v.texcoord0;
           // o.texcoord1 = v.texcoord1;
           // o.texcoord2 = v.texcoord2;

           // #if %TEXCOORD3REQUIREKEY%
           // o.texcoord3 = v.texcoord3;
           // #endif

           // #if %VERTEXCOLORREQUIREKEY%
           // o.vertexColor = v.vertexColor;
           // #endif
           
           VertexPositionInputs vertexInput = GetVertexPositionInputs(v.vertex.xyz);
           o.worldPos = TransformObjectToWorld(v.vertex.xyz);
           o.worldNormal = TransformObjectToWorldNormal(v.normal);
           o.worldTangent = float4(TransformObjectToWorldDir(v.tangent.xyz), v.tangent.w);


          #if _PASSSHADOW
              // Define shadow pass specific clip position for Universal
              o.pos = TransformWorldToHClip(ApplyShadowBias(o.worldPos, o.worldNormal, _LightDirection));
              #if UNITY_REVERSED_Z
                  o.pos.z = min(o.pos.z, o.pos.w * UNITY_NEAR_CLIP_VALUE);
              #else
                  o.pos.z = max(o.pos.z, o.pos.w * UNITY_NEAR_CLIP_VALUE);
              #endif
          #elif _PASSMETA
              o.pos = MetaVertexPosition(float4(v.vertex.xyz, 0), v.texcoord1.xy, v.texcoord2.xy, unity_LightmapST, unity_DynamicLightmapST);
          #else
              o.pos = TransformWorldToHClip(o.worldPos);
          #endif


          // #if %SCREENPOSREQUIREKEY%
          // o.screenPos = ComputeScreenPos(o.pos, _ProjectionParams.x);
          // #endif

          #if _PASSFORWARD || _PASSGBUFFER
              float2 uv1 = v.texcoord1.xy;
              OUTPUT_LIGHTMAP_UV(uv1, unity_LightmapST, o.lightmapUV);
              // o.texcoord1.xy = uv1;
              OUTPUT_SH(o.worldNormal, o.sh);
          #endif

          #ifdef VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
              #if _BAKEDLIT
                 half3 vertexLight = 0;
              #else
                 half3 vertexLight = VertexLighting(o.worldPos, o.worldNormal);
              #endif
              half fogFactor = ComputeFogFactor(o.pos.z);
              o.fogFactorAndVertexLight = half4(fogFactor, vertexLight);
          #endif

          #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
             o.shadowCoord = GetShadowCoord(vertexInput);
          #endif

           return o;
         }


         

         // fragment shader
         half4 Frag (VertexToPixel IN
            #ifdef _DEPTHOFFSET_ON
              , out float outputDepth : SV_Depth
            #endif
            #if NEED_FACING
               , bool facing : SV_IsFrontFace
            #endif
         ) : SV_Target
         {
           UNITY_SETUP_INSTANCE_ID(IN);
           UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);

           ShaderData d = CreateShaderData(IN
                  #if NEED_FACING
                     , facing
                  #endif
               );
           Surface l = (Surface)0;

           #ifdef _DEPTHOFFSET_ON
              l.outputDepth = outputDepth;
           #endif

           l.Albedo = half3(0.5, 0.5, 0.5);
           l.Normal = float3(0,0,1);
           l.Occlusion = 1;
           l.Alpha = 1;

           ChainSurfaceFunction(l, d);

           #ifdef _DEPTHOFFSET_ON
              outputDepth = l.outputDepth;
           #endif

           return float4(PackNormalOctRectEncode(TransformWorldToViewDir(d.worldSpaceNormal, true)), 0.0, 0.0);

         }

         ENDHLSL

      }


      




      

   }
   
   
   CustomEditor "ShaderCrew.SeeThroughShader.StandardLitSeeThroughShaderEditor"
}
