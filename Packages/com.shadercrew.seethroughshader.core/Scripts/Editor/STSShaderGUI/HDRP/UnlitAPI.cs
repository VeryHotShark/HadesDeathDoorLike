// SEE: https://github.com/Unity-Technologies/Graphics/blob/632f80e011f18ea537ee6e2f0be3ff4f4dea6a11/Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Unlit/UnlitAPI.cs

#if USING_HDRP

namespace ShaderCrew.SeeThroughShader
{
    using static HDMaterialProperties;
    internal static class UnlitAPI
    {
        // All Validate functions must be static. It allows to automatically update the shaders with a script if code changes
        internal static void ValidateMaterial(UnityEngine.Material material)
        {
            material.SetupBaseUnlitKeywords();
            material.SetupBaseUnlitPass();

            if (material.HasProperty(kEmissiveColorMap))
                MaterialUtils.SetKeyword(material, "_EMISSIVE_COLOR_MAP", material.GetTexture(kEmissiveColorMap));
            if (material.HasProperty(kUseEmissiveIntensity) && material.GetFloat(kUseEmissiveIntensity) != 0)
                material.UpdateEmissiveColorFromIntensityAndEmissiveColorLDR();

            // All the bits exclusively related to lit are ignored inside the BaseLitGUI function.
            BaseLitAPI.SetupStencil(material, receivesSSR: false, useSplitLighting: false);
        }
    }
}

#endif