//SEE: https://github.com/Unity-Technologies/Graphics/blob/6fdc7996098aa184495c0a3c0da10135bfe18340/Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/BaseLitAPI.cs

#if USING_HDRP

namespace ShaderCrew.SeeThroughShader
{
    using UnityEngine;
    using static HDMaterialProperties;


    abstract class BaseLitAPI
    {
        // Wind
        protected const string kWindEnabled = "_EnableWind";

        public static DisplacementMode GetFilteredDisplacementMode(Material material)
        {
            return material.GetFilteredDisplacementMode((DisplacementMode)material.GetFloat(kDisplacementMode));
        }

        // All Setup Keyword functions must be static. It allow to create script to automatically update the shaders with a script if code change
        static public void SetupBaseLitKeywords(Material material)
        {
            material.SetupBaseUnlitKeywords();

            bool doubleSidedEnable = material.HasProperty(kDoubleSidedEnable) ? material.GetFloat(kDoubleSidedEnable) > 0.0f : false;
            if (doubleSidedEnable)
            {
                DoubleSidedNormalMode doubleSidedNormalMode = (DoubleSidedNormalMode)material.GetFloat(kDoubleSidedNormalMode);
                switch (doubleSidedNormalMode)
                {
                    case DoubleSidedNormalMode.Mirror: // Mirror mode (in tangent space)
                        material.SetVector("_DoubleSidedConstants", new Vector4(1.0f, 1.0f, -1.0f, 0.0f));
                        break;

                    case DoubleSidedNormalMode.Flip: // Flip mode (in tangent space)
                        material.SetVector("_DoubleSidedConstants", new Vector4(-1.0f, -1.0f, -1.0f, 0.0f));
                        break;

                    case DoubleSidedNormalMode.None: // None mode (in tangent space)
                        material.SetVector("_DoubleSidedConstants", new Vector4(1.0f, 1.0f, 1.0f, 0.0f));
                        break;
                }
            }

            if (material.HasProperty(kDisplacementMode))
            {
                var displacementMode = GetFilteredDisplacementMode(material);

                bool enableDisplacement = displacementMode != DisplacementMode.None;
                bool enableVertexDisplacement = displacementMode == DisplacementMode.Vertex;
                bool enablePixelDisplacement = displacementMode == DisplacementMode.Pixel;
                bool enableTessellationDisplacement = displacementMode == DisplacementMode.Tessellation;

                MaterialUtils.SetKeyword(material, "_VERTEX_DISPLACEMENT", enableVertexDisplacement);
                MaterialUtils.SetKeyword(material, "_PIXEL_DISPLACEMENT", enablePixelDisplacement);
                // Only set if tessellation exist
                MaterialUtils.SetKeyword(material, "_TESSELLATION_DISPLACEMENT", enableTessellationDisplacement);

                bool displacementLockObjectScale = material.GetFloat(kDisplacementLockObjectScale) > 0.0f;
                bool displacementLockTilingScale = material.GetFloat(kDisplacementLockTilingScale) > 0.0f;
                // Tessellation reuse vertex flag.
                MaterialUtils.SetKeyword(material, "_VERTEX_DISPLACEMENT_LOCK_OBJECT_SCALE", displacementLockObjectScale && (enableVertexDisplacement || enableTessellationDisplacement));
                MaterialUtils.SetKeyword(material, "_PIXEL_DISPLACEMENT_LOCK_OBJECT_SCALE", displacementLockObjectScale && enablePixelDisplacement);
                MaterialUtils.SetKeyword(material, "_DISPLACEMENT_LOCK_TILING_SCALE", displacementLockTilingScale && enableDisplacement);

#if UNITY_2021_OR_NEWER
                // Depth offset is only enabled if per pixel displacement is
                bool depthOffsetEnable = (material.GetFloat(kDepthOffsetEnable) > 0.0f) && enablePixelDisplacement;
                MaterialUtils.SetKeyword(material, "_DEPTHOFFSET_ON", depthOffsetEnable);
#endif
            }

            MaterialUtils.SetKeyword(material, "_VERTEX_WIND", false);

            material.SetupMainTexForAlphaTestGI("_BaseColorMap", "_BaseColor");

            // Use negation so we don't create keyword by default
            MaterialUtils.SetKeyword(material, "_DISABLE_DECALS", material.HasProperty(kSupportDecals) && material.GetFloat(kSupportDecals) == 0.0f);
            MaterialUtils.SetKeyword(material, "_DISABLE_SSR", material.HasProperty(kReceivesSSR) && material.GetFloat(kReceivesSSR) == 0.0f);
            MaterialUtils.SetKeyword(material, "_DISABLE_SSR_TRANSPARENT", material.HasProperty(kReceivesSSRTransparent) && material.GetFloat(kReceivesSSRTransparent) == 0.0f);
            MaterialUtils.SetKeyword(material, "_ENABLE_GEOMETRIC_SPECULAR_AA", material.HasProperty(kEnableGeometricSpecularAA) && material.GetFloat(kEnableGeometricSpecularAA) == 1.0f);

            if (material.HasProperty(kRefractionModel))
            {
                var refractionModelValue = (RefractionModel)material.GetFloat(kRefractionModel);
                // We can't have refraction in pre-refraction queue and the material needs to be transparent
                var canHaveRefraction = material.GetSurfaceType() == SurfaceType.Transparent && !HDRenderQueue.k_RenderQueue_PreRefraction.Contains(material.renderQueue);
                MaterialUtils.SetKeyword(material, "_REFRACTION_PLANE", (refractionModelValue == RefractionModel.Planar) && canHaveRefraction);
                MaterialUtils.SetKeyword(material, "_REFRACTION_SPHERE", (refractionModelValue == RefractionModel.Sphere) && canHaveRefraction);
                MaterialUtils.SetKeyword(material, "_REFRACTION_THIN", (refractionModelValue == RefractionModel.Thin) && canHaveRefraction);
            }
        }

        static public void SetupStencil(Material material, bool receivesSSR, bool useSplitLighting)
        {
            ComputeStencilProperties(receivesSSR, useSplitLighting, out int stencilRef, out int stencilWriteMask,
                out int stencilRefDepth, out int stencilWriteMaskDepth, out int stencilRefGBuffer, out int stencilWriteMaskGBuffer,
                out int stencilRefMV, out int stencilWriteMaskMV
            );

            // As we tag both during motion vector pass and Gbuffer pass we need a separate state and we need to use the write mask
            if (material.HasProperty(kStencilRef))
            {
                material.SetInt(kStencilRef, stencilRef);
                material.SetInt(kStencilWriteMask, stencilWriteMask);
            }
            if (material.HasProperty(kStencilRefDepth))
            {
                material.SetInt(kStencilRefDepth, stencilRefDepth);
                material.SetInt(kStencilWriteMaskDepth, stencilWriteMaskDepth);
            }
            if (material.HasProperty(kStencilRefGBuffer))
            {
                material.SetInt(kStencilRefGBuffer, stencilRefGBuffer);
                material.SetInt(kStencilWriteMaskGBuffer, stencilWriteMaskGBuffer);
            }
            if (material.HasProperty(kStencilRefDistortionVec))
            {
                material.SetInt(kStencilRefDistortionVec, (int)StencilUsage.DistortionVectors);
                material.SetInt(kStencilWriteMaskDistortionVec, (int)StencilUsage.DistortionVectors);
            }
            if (material.HasProperty(kStencilRefMV))
            {
                material.SetInt(kStencilRefMV, stencilRefMV);
                material.SetInt(kStencilWriteMaskMV, stencilWriteMaskMV);
            }
        }

        static public void ComputeStencilProperties(bool receivesSSR, bool useSplitLighting, out int stencilRef, out int stencilWriteMask,
            out int stencilRefDepth, out int stencilWriteMaskDepth, out int stencilRefGBuffer, out int stencilWriteMaskGBuffer,
            out int stencilRefMV, out int stencilWriteMaskMV)
        {
            // Stencil usage rules:
            // TraceReflectionRay need to be tagged during depth prepass
            // RequiresDeferredLighting need to be tagged during GBuffer
            // SubsurfaceScattering need to be tagged during either GBuffer or Forward pass
            // ObjectMotionVectors need to be tagged in velocity pass.
            // As motion vectors pass can be use as a replacement of depth prepass it also need to have TraceReflectionRay
            // As GBuffer pass can have no depth prepass, it also need to have TraceReflectionRay
            // Object motion vectors is always render after a full depth buffer (if there is no depth prepass for GBuffer all object motion vectors are render after GBuffer)
            // so we have a guarantee than when we write object motion vectors no other object will be draw on top (and so would have require to overwrite motion vectors).
            // Final combination is:
            // Prepass: TraceReflectionRay
            // Motion vectors: TraceReflectionRay, ObjectVelocity
            // GBuffer: LightingMask, ObjectVelocity
            // Forward: LightingMask

            stencilRef = (int)StencilUsage.Clear; // Forward case
            stencilWriteMask = (int)StencilUsage.RequiresDeferredLighting | (int)StencilUsage.SubsurfaceScattering;
            stencilRefDepth = 0;
            stencilWriteMaskDepth = 0;
            stencilRefGBuffer = (int)StencilUsage.RequiresDeferredLighting;
            stencilWriteMaskGBuffer = (int)StencilUsage.RequiresDeferredLighting | (int)StencilUsage.SubsurfaceScattering;
            stencilRefMV = (int)StencilUsage.ObjectMotionVector;
            stencilWriteMaskMV = (int)StencilUsage.ObjectMotionVector;

            if (useSplitLighting)
            {
                stencilRefGBuffer |= (int)StencilUsage.SubsurfaceScattering;
                stencilRef |= (int)StencilUsage.SubsurfaceScattering;
            }

            if (receivesSSR)
            {
                stencilRefDepth |= (int)StencilUsage.TraceReflectionRay;
                stencilRefGBuffer |= (int)StencilUsage.TraceReflectionRay;
                stencilRefMV |= (int)StencilUsage.TraceReflectionRay;
            }

            stencilWriteMaskDepth |= (int)StencilUsage.TraceReflectionRay;
            stencilWriteMaskGBuffer |= (int)StencilUsage.TraceReflectionRay;
            stencilWriteMaskMV |= (int)StencilUsage.TraceReflectionRay;
        }

        static public void SetupBaseLitMaterialPass(Material material)
        {
            material.SetupBaseUnlitPass();
        }

        static public void SetupDisplacement(Material material, int layerCount = 1)
        {
            DisplacementMode displacementMode = GetFilteredDisplacementMode(material);
            for (int i = 0; i < layerCount; i++)
            {
                var heightAmplitude = layerCount > 1 ? kHeightAmplitude + i : kHeightAmplitude;
                var heightCenter = layerCount > 1 ? kHeightCenter + i : kHeightCenter;
                if (material.HasProperty(heightAmplitude) && material.HasProperty(heightCenter))
                {
                    var heightPoMAmplitude = layerCount > 1 ? kHeightPoMAmplitude + i : kHeightPoMAmplitude;
                    var heightParametrization = layerCount > 1 ? kHeightParametrization + i : kHeightParametrization;
                    var heightTessAmplitude = layerCount > 1 ? kHeightTessAmplitude + i : kHeightTessAmplitude;
                    var heightTessCenter = layerCount > 1 ? kHeightTessCenter + i : kHeightTessCenter;
                    var heightOffset = layerCount > 1 ? kHeightOffset + i : kHeightOffset;
                    var heightMin = layerCount > 1 ? kHeightMin + i : kHeightMin;
                    var heightMax = layerCount > 1 ? kHeightMax + i : kHeightMax;

                    if (displacementMode == DisplacementMode.Pixel)
                    {
                        material.SetFloat(heightAmplitude, material.GetFloat(heightPoMAmplitude) * 0.01f); // Convert centimeters to meters.
                        material.SetFloat(heightCenter, 1.0f); // PoM is always inward so base (0 height) is mapped to 1 in the texture
                    }
                    else
                    {
                        var parametrization = (HeightmapParametrization)material.GetFloat(heightParametrization);
                        if (parametrization == HeightmapParametrization.MinMax)
                        {
                            float offset = material.GetFloat(heightOffset);
                            float minHeight = material.GetFloat(heightMin);
                            float amplitude = material.GetFloat(heightMax) - minHeight;

                            material.SetFloat(heightAmplitude, amplitude * 0.01f); // Convert centimeters to meters.
                            material.SetFloat(heightCenter, -(minHeight + offset) / Mathf.Max(1e-6f, amplitude));
                        }
                        else
                        {
                            float offset = material.GetFloat(heightOffset);
                            float center = material.GetFloat(heightTessCenter);
                            float amplitude = material.GetFloat(heightTessAmplitude);

                            material.SetFloat(heightAmplitude, amplitude * 0.01f); // Convert centimeters to meters.
                            material.SetFloat(heightCenter, -offset / Mathf.Max(1e-6f, amplitude) + center);
                        }
                    }
                }
            }
        }
    }



    //SEE: https://github.com/Unity-Technologies/Graphics/blob/6fdc7996098aa184495c0a3c0da10135bfe18340/Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/ScreenSpaceLighting/ScreenSpaceRefraction.cs
    internal enum RefractionModel
    {
        None = 0,
        Planar = 1,
        Sphere = 2,
        Thin = 3
    };


    //SEE: https://github.com/Unity-Technologies/Graphics/blob/632f80e011f18ea537ee6e2f0be3ff4f4dea6a11/Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/HDStencilUsage.cs

    internal enum StencilUsage
    {
        Clear = 0,

        // Note: first bit is free and can still be used by both phases.

        // --- Following bits are used before transparent rendering ---

        RequiresDeferredLighting = (1 << 1),
        SubsurfaceScattering = (1 << 2),     //  SSS, Split Lighting
        TraceReflectionRay = (1 << 3),     //  SSR or RTR - slot is reuse in transparent
        Decals = (1 << 4),     //  Use to tag when an Opaque Decal is render into DBuffer
        ObjectMotionVector = (1 << 5),     //  Animated object (for motion blur, SSR, SSAO, TAA)

        // --- Stencil  is cleared after opaque rendering has finished ---

        // --- Following bits are used exclusively for what happens after opaque ---
        ExcludeFromTAA = (1 << 1),    // Disable Temporal Antialiasing for certain objects
        DistortionVectors = (1 << 2),    // Distortion pass - reset after distortion pass, shared with SMAA
        SMAA = (1 << 2),    // Subpixel Morphological Antialiasing
                            // Reserved TraceReflectionRay = (1 << 3) for transparent SSR or RTR
        WaterSurface = (1 << 4), // Reserved for water surface usage
        AfterOpaqueReservedBits = 0x38,        // Reserved for future usage

        // --- Following are user bits, we don't touch them inside HDRP and is up to the user to handle them ---
        UserBit0 = (1 << 6),
        UserBit1 = (1 << 7),

        HDRPReservedBits = 255 & ~(UserBit0 | UserBit1),
    }
}
#endif