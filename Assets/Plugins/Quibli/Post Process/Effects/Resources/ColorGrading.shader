Shader "Hidden/CompoundRendererFeature/ColorGrading"
{
    HLSLINCLUDE
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
    #include "../../ShaderLibrary/Core.hlsl"

    TEXTURE2D_X(_MainTex);
    float4 _MainTex_TexelSize;
    SAMPLER(sampler_MainTex);

    float _Intensity;
    float4 _SourceSize;

    float4 _ShadowBezierPoints; // x - blue shadows, y - green shadows.
    float4 _HighlightBezierPoints; // x - red highlights.
    float _Contrast;
    float _Vibrance;
    float _Saturation;

    #pragma vertex FullScreenTrianglePostProcessVertexProgram

    // 4-point bezier: P = (1−t)^3*P1 + 3(1−t)^2*tP2 + 3(1−t)*t^2*P3 + t^3*P4
    // Interactive demo: https://javascript.info/bezier-curve
    inline float Bezier4(float t, float2 p1, float2 p2, float2 p3, float2 p4) {
        const float nt = 1.0 - t;
        const float2 p = nt * nt * nt * p1 + 3.0 * nt * nt * t * p2 + 3.0 * nt * t * t * p3 + t * t * t * p4;
        return p.y;
    }

    void RefitBezier(float2 passthru_0, float2 passthru_1, float2 passthru_2, float2 passthru_3,
                     out float2 out_tangent_1, out float2 out_tangent_2) {
        const float alpha = 1.0;

        const float d1 = pow(length(passthru_1 - passthru_0), alpha);
        const float d2 = pow(length(passthru_2 - passthru_1), alpha);
        const float d3 = pow(length(passthru_3 - passthru_2), alpha);

        // Modify tangent 1
        float a = d1 * d1;
        float b = d2 * d2;
        float c = (2 * d1 * d1) + (3 * d1 * d2) + (d2 * d2);
        float d = 3 * d1 * (d1 + d2);
        out_tangent_1.x = (a * passthru_2.x - b * passthru_0.x + c * passthru_1.x) / d;
        out_tangent_1.y = (a * passthru_2.y - b * passthru_0.y + c * passthru_1.y) / d;

        // Modify tangent 2
        a = d3 * d3;
        b = d2 * d2;
        c = (2 * d3 * d3) + (3 * d3 * d2) + (d2 * d2);
        d = 3 * d3 * (d3 + d2);
        out_tangent_2.x = (a * passthru_1.x - b * passthru_3.x + c * passthru_2.x) / d;
        out_tangent_2.y = (a * passthru_1.y - b * passthru_3.y + c * passthru_2.y) / d;
    }

    inline float InverseLerp(float t, float a, float b) {
        return (t - a) / (b - a);
    }

    inline float3 Vibrance(float3 color, float vibrance) {
        const float mx = max(color.r, max(color.g, color.b));
        const float avg = (color.r + color.g + color.b) / 3.0;
        const float nerfRed = mx - color.r;
        const float amt = (mx - avg) * 3.0 * -vibrance * nerfRed;
        return color + (mx - color) * amt;
    }

    inline float3 Saturation(float3 color, float saturation) {
        const float luma = dot(color, float3(0.2126729, 0.7151522, 0.0721750));
        return luma.xxx + saturation.xxx * (color - luma.xxx);
    }

    // Interactive demo: https://apoorvaj.io/cubic-bezier-through-four-points/
    inline float BoostHighlights(float t) {
        const float2 p0 = float2(0.0, 0.0);
        const float2 p1 = float2(0.5, 0.5);
        const float2 p2 = float2(0.75, 0.7 + 0.3 * _HighlightBezierPoints.x);
        const float2 p3 = float2(1.0, 1.0);

        float2 t1, t2;
        RefitBezier(p0, p1, p2, p3, t1, t2);

        if (t < p1.x) {
            return Bezier4(InverseLerp(t, p0.x, p1.x), p0, p0 + (p1 - p0) / 3.0, p1 - (t1 - p1), p1);
        }

        if (t < p2.x) {
            return Bezier4(InverseLerp(t, p1.x, p2.x), p1, t1, t2, p2);
        }

        return Bezier4(InverseLerp(t, p2.x, p3.x), p2, p2 - (t2 - p2), p3 + (p2 - p3) / 3.0, p3);
    }

    float4 CompositeFragmentProgram(PostProcessVaryings input) : SV_Target {
        UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
        const float2 uv = UnityStereoTransformScreenSpaceTex(input.texcoord) * _SourceSize.xy;
        const float4 color = LOAD_TEXTURE2D_X(_MainTex, uv);

        float4 output = color;

        // Blue shadows.
        output.b = Bezier4(color.b, float2(0.0, _ShadowBezierPoints.x * 0.25),
                           float2(0.25, max(0.25, _ShadowBezierPoints.x * 0.25)), float2(0.5, 0.5),
                           float2(1.0, 1.0));

        // Green shadows.
        output.g = Bezier4(color.g, float2(0.0, _ShadowBezierPoints.y * 0.1),
                           float2(0.3, max(0.3, _ShadowBezierPoints.y * 0.1)), float2(0.4, 0.4),
                           float2(1.0, 1.0));

        // Red highlights.
        output.r = BoostHighlights(saturate(color.r));

        // S-curve contrast.
        {
            const float c = 1. / 3 - 1. / 3 * _Contrast;
            const float2 p1 = float2(0.0, 0.0);
            const float2 p2 = float2(1. / 3, c);
            const float2 p3 = float2(2. / 3, 1.0 - c);
            const float2 p4 = float2(1.0, 1.0);
            output.r = Bezier4(saturate(output.r), p1, p2, p3, p4);
            output.g = Bezier4(saturate(output.g), p1, p2, p3, p4);
            output.b = Bezier4(saturate(output.b), p1, p2, p3, p4);
        }

        // Preserve HDR.
        output = max(color, output);
        output.rgb = Vibrance(output.rgb, _Vibrance);
        output.rgb = Saturation(output.rgb, _Saturation + 1.0);

        // Non-linear blend "CryEngine 3 Graphics Gems" [Sousa13]
        const half blend = sqrt(_Intensity * TWO_PI);
        float3 dstColor = output.rgb * saturate(blend);
        const half dstAlpha = saturate(1.0 - blend);

        dstColor = color.rgb * dstAlpha + dstColor;
        return float4(dstColor, color.a);
    }
    ENDHLSL

    SubShader
    {
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            Name "Color Grading"

            HLSLPROGRAM
            #pragma fragment CompositeFragmentProgram
            ENDHLSL
        }
    }

    Fallback Off
}