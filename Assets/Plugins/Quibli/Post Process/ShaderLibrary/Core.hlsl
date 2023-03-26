#ifndef COMPOUND_RENDERER_FEATURE_CORE_INCLUDED
#define COMPOUND_RENDERER_FEATURE_CORE_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

struct FullScreenTrianglePostProcessAttributes
{
    uint vertexID : SV_VertexID;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct PostProcessVaryings
{
    float4 positionCS : SV_POSITION;
    float2 texcoord : TEXCOORD0;
    UNITY_VERTEX_OUTPUT_STEREO
};

// TODO: Replace with Common.hlsl from URP.
PostProcessVaryings FullScreenTrianglePostProcessVertexProgram(
    FullScreenTrianglePostProcessAttributes input) {
    PostProcessVaryings output;
    UNITY_SETUP_INSTANCE_ID(input);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
    output.positionCS = GetFullScreenTriangleVertexPosition(input.vertexID);
    output.texcoord = GetFullScreenTriangleTexCoord(input.vertexID);
    return output;
}

#endif
