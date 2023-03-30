using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace ShaderCrew.SeeThroughShader
{
    public static class GeneralUtils
    {
        // native STS Shaders
        public static readonly List<string> STS_SHADER_LIST = new List<string>
        {
        "SeeThroughShader/BiRP/Standard",
        "SeeThroughShader/BiRP/Unlit/Color",
        "SeeThroughShader/BiRP/Unlit/Texture",

        "SeeThroughShader/HDRP/2022/Lit",
        "SeeThroughShader/HDRP/2021/Lit",
        "SeeThroughShader/HDRP/2020/Lit",
        "SeeThroughShader/HDRP/2019/Lit",

        "SeeThroughShader/URP/2022/Lit",
        "SeeThroughShader/URP/2021/Lit",
        "SeeThroughShader/URP/2020/Lit",
        "SeeThroughShader/URP/2019/Lit",


        };



        public static readonly Dictionary<string, string> STS_BIRP_SHADER_DICTIONARY = new Dictionary<string, string>
        {
            { "Standard", "SeeThroughShader/BiRP/Standard" },
            { "Unlit/Color", "SeeThroughShader/BiRP/Unlit/Color" },
            { "Unlit/Texture", "SeeThroughShader/BiRP/Unlit/Texture" },

            { SeeThroughShaderConstants.STS_SHADER_DEFAULT_KEY, "SeeThroughShader/BiRP/Standard" },
        };


#if USING_HDRP
        public static readonly Dictionary<string,string> STS_HDRP_SHADER_DICTIONARY = new Dictionary<string, string>
        {

#if UNITY_2022
            //{ "HDRP/Unlit", "SeeThroughShader/HDRP/2022/Unlit" },
            { "HDRP/Lit", "SeeThroughShader/HDRP/2022/Lit" },
            { SeeThroughShaderConstants.STS_SHADER_DEFAULT_KEY, "SeeThroughShader/HDRP/2022/Lit" },
#elif UNITY_2021
            //{ "HDRP/Unlit", "SeeThroughShader/HDRP/2021/Unlit" },
            { "HDRP/Lit", "SeeThroughShader/HDRP/2021/Lit" },
            { SeeThroughShaderConstants.STS_SHADER_DEFAULT_KEY, "SeeThroughShader/HDRP/2021/Lit" },
#elif UNITY_2020
            //{ "HDRP/Unlit", "SeeThroughShader/HDRP/2020/Unlit" },
            { "HDRP/Lit", "SeeThroughShader/HDRP/2020/Lit" },
            { SeeThroughShaderConstants.STS_SHADER_DEFAULT_KEY, "SeeThroughShader/HDRP/2020/Lit" },
#else
            //{ "HDRP/Unlit", "SeeThroughShader/HDRP/2019/Unlit" },
            { "HDRP/Lit", "SeeThroughShader/HDRP/2019/Lit" },
            { SeeThroughShaderConstants.STS_SHADER_DEFAULT_KEY, "SeeThroughShader/HDRP/2019/Lit" },
#endif
        };
#endif


#if USING_URP
        public static readonly Dictionary<string, string> STS_URP_SHADER_DICTIONARY = new Dictionary<string, string>
    {

#if UNITY_2022
        //{ "URP/Unlit", "SeeThroughShader/URP/2022/Lit" },
        { "URP/Lit", "SeeThroughShader/URP/2022/Lit" },
        { SeeThroughShaderConstants.STS_SHADER_DEFAULT_KEY, "SeeThroughShader/URP/2022/Lit" },
#elif UNITY_2021
        //{ "URP/Unlit", "SeeThroughShader/URP/2021/Lit" },
        { "URP/Lit", "SeeThroughShader/URP/2021/Lit" },
        { SeeThroughShaderConstants.STS_SHADER_DEFAULT_KEY, "SeeThroughShader/URP/2021/Lit" },
#elif UNITY_2020
        //{ "URP/Unlit", "SeeThroughShader/URP/2020/Lit" },
        { "URP/Lit", "SeeThroughShader/URP/2020/Lit" },
        { SeeThroughShaderConstants.STS_SHADER_DEFAULT_KEY, "SeeThroughShader/URP/2020/Lit" },
#elif UNITY_2019
        //{ "URP/Unlit", "SeeThroughShader/URP/2019/Lit" },
        { "URP/Lit", "SeeThroughShader/URP/2019/Lit" },
        { SeeThroughShaderConstants.STS_SHADER_DEFAULT_KEY, "SeeThroughShader/URP/2019/Lit" },
#else
        { "URP/Lit", "SeeThroughShader/URP/2021/Lit" },
        { SeeThroughShaderConstants.STS_SHADER_DEFAULT_KEY, "SeeThroughShader/URP/2021/Lit" },
#endif
        };
#endif

        //public static readonly List<string> STS_REFMAT_LIST = new List<string>
        //{
        //    "refMaterial","refMaterial2","refMaterial3","refMaterial4",
        //};

        public static readonly List<string> STS_SYNC_PROPERTIES_LIST = new List<string>
    {
        "_Obstruction", "_AnimationSpeed", "_AnimationEnabled", "_TransitionDuration",
        "_DissolveFallOff", "_PreviewMode", "_CircleObstructionDestroyRadius", "_CircleStrength",
        "_DissolveEmissionBooster", "_AngleStrength", "_IntrinsicDissolveStrength", "_ConeStrength",
        "_ConeObstructionDestroyRadius", "_CylinderStrength", "_CylinderObstructionDestroyRadius",
        "_Floor", "_FloorMode", "_FloorY", "_PlayerPosYOffset", "_UVs", "_hasClippedShadows", "_DissolveColorSaturation",
        "_DissolveEmission", "_FloorYTextureGradientLength", "_DefaultEffectRadius",
        //"_TextureVisibility",
        "_TexturedEmissionEdgeStrength", "_TexturedEmissionEdge",
        "_DissolveMaskEnabled",
        "_IsometricExclusion", "_IsometricExclusionDistance", "_IsometricExclusionGradientLength",
        "_Ceiling", "_CeilingMode", "_CeilingBlendMode", "_CeilingY","_CeilingPlayerYOffset", "_CeilingYGradientLength",


        // TODO FIX
#if USING_HDRP
        //"_CullMode", "_CullModeForward",
#else
        "_Cull", //"_Mode", 
#endif

        "_Zoning", "_ZoningMode", "_ZoningEdgeGradientLength", "_IsZoningRevealable", "_SyncZonesWithFloorY", "_SyncZonesFloorYOffset",
        "_PreviewIndicatorLineThickness",
        "_CurveStrength", "_CurveObstructionDestroyRadius",
        "_InteractionMode",

        "_SyncCullMode"
    };

        public static readonly List<string> STS_NONSYNC_PROPERTIES_LIST = new List<string>
    {
            SeeThroughShaderConstants.KEYWORD_OBSTRUCTION_CURVE, //ShaderGraph
            SeeThroughShaderConstants.KEYWORD_DISSOLVEMASK, //ShaderGraph
            SeeThroughShaderConstants.KEYWORD_ZONING, //ShaderGraph
            SeeThroughShaderConstants.KEYWORD_REPLACEMENT, //ShaderGraph
            SeeThroughShaderConstants.KEYWORD_PLAYERINDEPENDENT, //ShaderGraph

            SeeThroughShaderConstants.PROPERTY_DISSOLVE_TEX, 
            SeeThroughShaderConstants.PROPERTY_DISSOLVE_MASK,
            SeeThroughShaderConstants.PROPERTY_OBSTRUCTION_CURVE,
            SeeThroughShaderConstants.PROPERTY_DISSOLVE_COLOR,
            SeeThroughShaderConstants.PROPERTY_IS_REFERENCE_MATERIAL,
            SeeThroughShaderConstants.PROPERTY_NUM_OF_PLAYERS_INSIDE,
            SeeThroughShaderConstants.PROPERTY_T_VALUE,
            SeeThroughShaderConstants.PROPERTY_T_DIRECTION,
            SeeThroughShaderConstants.PROPERTY_ID,
            SeeThroughShaderConstants.PROPERTY_TRIGGER_MODE,
            SeeThroughShaderConstants.PROPERTY_RAYCAST_MODE,
            SeeThroughShaderConstants.PROPERTY_IS_EXEMPT,
            SeeThroughShaderConstants.PROPERTY_ID,
            SeeThroughShaderConstants.PROPERTY_IS_REPLACEMENT_SHADER,

            SeeThroughShaderConstants.PROPERTY_SHOW_CONTENT_DISSOLVE_AREA,
            SeeThroughShaderConstants.PROPERTY_SHOW_CONTENT_INTERACTION_OPTIONS_AREA,
            SeeThroughShaderConstants.PROPERTY_SHOW_CONTENT_OBSTRUCTION_OPTIONS_AREA,
            SeeThroughShaderConstants.PROPERTY_SHOW_CONTENT_ANIMATION_AREA,
            SeeThroughShaderConstants.PROPERTY_SHOW_CONTENT_ZONING_AREA,
            SeeThroughShaderConstants.PROPERTY_SHOW_CONTENT_REPLACEMENT_OPTIONS_AREA,
            SeeThroughShaderConstants.PROPERTY_SHOW_CONTENT_DEBUG_AREA,

    };


        public static readonly List<string> STS_KEYWORDS_LIST = new List<string>
    {
        "_DISSOLVEMASK",
        "_ZONING",
        "_OBSTRUCTION_CURVE",
        "_PLAYERINDEPENDENT",
        //"_REPLACEMENT"
    };

        public enum RenderPipeline
        {
            HDRP,
            URP,
            BiRP,
            NONE
        }

        public static Dictionary<string, string> getUnityToSTSShaderMapping()
        {
            Dictionary<string, string> UnityToSTSShaderMapping = null;
            if (GraphicsSettings.currentRenderPipeline)
            {
                if (GraphicsSettings.currentRenderPipeline.GetType().ToString().Contains("HighDefinition"))
                {
#if USING_HDRP
                    UnityToSTSShaderMapping = STS_HDRP_SHADER_DICTIONARY;
#endif
                }
                else //UTP
                {
#if USING_URP
                    UnityToSTSShaderMapping = STS_URP_SHADER_DICTIONARY;
#endif
                }
            }
            else
            {
                UnityToSTSShaderMapping = STS_BIRP_SHADER_DICTIONARY;
            }

            return UnityToSTSShaderMapping;
        }




        public class UnityVersionRenderPipelineShaderInfo
        {
            public string unityVersion;
            public string renderPipeline;
            public string versionAndRPCorrectedShader;
            public string shaderFolder;


            public UnityVersionRenderPipelineShaderInfo(string unityVersion, string renderPipeline, string shader, string shaderFolder)
            {
                this.unityVersion = unityVersion;
                this.renderPipeline = renderPipeline;
                this.versionAndRPCorrectedShader = shader;
                this.shaderFolder = shaderFolder;
            }
        }
        public static UnityVersionRenderPipelineShaderInfo getUnityVersionAndRenderPipelineCorrectedShaderString()
        {
            string unityVersion;
            string renderPipeline;
            string shaderString;
            string shaderFolder;
            unityVersion = Application.unityVersion;
            if (GraphicsSettings.currentRenderPipeline)
            {
                if (GraphicsSettings.currentRenderPipeline.GetType().ToString().Contains("HighDefinition"))
                {
                    renderPipeline = "HDRP";
                    if (unityVersion.Substring(0, 4).Equals("2019"))
                    {
                        shaderString = "SeeThroughShader/HDRP/2019/Lit";
                        shaderFolder = "SeeThroughShader/HDRP/2019/...";
                    }
                    else if (unityVersion.Substring(0, 4).Equals("2020"))
                    {
                        shaderString = "SeeThroughShader/HDRP/2020/Lit";
                        shaderFolder = "SeeThroughShader/HDRP/2020/...";
                    }
                    else if (unityVersion.Substring(0, 4).Equals("2021"))
                    {
                        shaderString = "SeeThroughShader/HDRP/2021/Lit";
                        shaderFolder = "SeeThroughShader/HDRP/2021/...";
                    }
                    else
                    {
                        shaderString = "SeeThroughShader/HDRP/2022/Lit";
                        shaderFolder = "SeeThroughShader/HDRP/2022/...";
                    }

                }
                else
                {
                    renderPipeline = "URP";
                    if (unityVersion.Substring(0, 4).Equals("2019"))
                    {
                        shaderString = "SeeThroughShader/URP/2019/Lit";
                        shaderFolder = "SeeThroughShader/URP/2019/...";
                    }
                    else if (unityVersion.Substring(0, 4).Equals("2020"))
                    {
                        shaderString = "SeeThroughShader/URP/2020/Lit";
                        shaderFolder = "SeeThroughShader/URP/2020/...";
                    }
                    else if (unityVersion.Substring(0, 4).Equals("2021"))
                    {
                        shaderString = "SeeThroughShader/URP/2021/Lit";
                        shaderFolder = "SeeThroughShader/URP/2021/...";
                    }
                    else
                    {
                        shaderString = "SeeThroughShader/URP/2022/Lit";
                        shaderFolder = "SeeThroughShader/URP/2022/...";
                    }
                }
            }
            else
            {
                renderPipeline = "Built-in RP";
                shaderString = "SeeThroughShader/BiRP/Standard";
                shaderFolder = "SeeThroughShader/BiRP/...";
            }

            return new UnityVersionRenderPipelineShaderInfo(unityVersion, renderPipeline, shaderString, shaderFolder);
        }


        public static RenderPipeline getCurrentRenderPipeline()
        {
            RenderPipeline rp;
            if (GraphicsSettings.currentRenderPipeline)
            {
                if (GraphicsSettings.currentRenderPipeline.GetType().ToString().Contains("HighDefinition"))
                {
                    rp = RenderPipeline.HDRP;
                }
                else
                {
                    rp = RenderPipeline.URP;
                }
            }
            else
            {
                rp = RenderPipeline.BiRP;
            }

            return rp;
        }

        //public static void updateSeeThroughShaderMaterialProperties(Transform[] transforms, string seeThroughShaderName, Material referenceMaterial)
        //{

        //    Material firstInstancedMaterial = getFirstInstancedMaterial(transforms, seeThroughShaderName);
        //    List<string> namesOfChangedProperties = getNamesOfAllChangedPropertyValues(firstInstancedMaterial, referenceMaterial);
        //    if (namesOfChangedProperties.Count > 0)
        //    {
        //        foreach (Transform transform in transforms)
        //        {
        //            Renderer rendererNonLOD = transform.GetComponent<Renderer>();

        //            if (transform.GetComponent<LODGroup>() != null)
        //            {
        //                foreach (LOD lod in transform.GetComponent<LODGroup>().GetLODs())
        //                {
        //                    foreach (Renderer renderer in lod.renderers)
        //                    {
        //                        if (renderer == rendererNonLOD)
        //                        {
        //                            rendererNonLOD = null;
        //                        }
        //                        if (renderer != null && renderer.materials.Length > 0)
        //                        {
        //                            foreach (Material mat in renderer.materials)
        //                            {
        //                                if (mat != null && mat.shader.name == seeThroughShaderName)
        //                                {
        //                                    updateMaterialProperties(mat, referenceMaterial, namesOfChangedProperties);
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }

        //            if (rendererNonLOD != null)
        //            {
        //                Material[] materials = rendererNonLOD.materials;
        //                if (materials.Length > 0)
        //                {
        //                    foreach (Material material in materials)
        //                    {
        //                        if (material != null && material.shader.name == seeThroughShaderName)
        //                        {
        //                            updateMaterialProperties(material, referenceMaterial, namesOfChangedProperties);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        /// <summary>
        /// Gets all materials, including the ones in various LOD levels, which are associated with given transforms
        public static List<Material> getAllSTSMaterialsFromTransforms(Transform[] transforms, string seeThroughShaderName, Material referenceMaterial)
        {
            List<Material> materialList = new List<Material>();        

            foreach (Transform transform in transforms)
            {
                Renderer rendererNonLOD = transform.GetComponent<Renderer>();

                if (transform.GetComponent<LODGroup>() != null)
                {
                    foreach (LOD lod in transform.GetComponent<LODGroup>().GetLODs())
                    {
                        foreach (Renderer renderer in lod.renderers)
                        {
                            if (renderer == rendererNonLOD)
                            {
                                rendererNonLOD = null;
                            }
                            if (renderer != null && renderer.materials.Length > 0)
                            {
                                foreach (Material material in renderer.materials)
                                {
                                    if (material != null && material.shader.name == seeThroughShaderName)
                                    {
                                        if(!materialList.Contains(material))
                                        {
                                            materialList.Add(material);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (rendererNonLOD != null)
                {
                    Material[] materials = rendererNonLOD.materials;
                    if (materials.Length > 0)
                    {
                        foreach (Material material in materials)
                        {
                            if (material != null && material.shader.name == seeThroughShaderName)
                            {
                                if (!materialList.Contains(material))
                                {
                                    materialList.Add(material);
                                }
                            }
                        }
                    }
                }
            }

            return materialList;
        }


        ///// <summary>
        ///// Gets all materials, including the ones in various LOD levels, which are associated with given transforms
        //public static List<Material> getAllSTSMaterialsFromTransforms(Transform[] transforms, List<Shader> STSShaders, Material referenceMaterial)
        //{
        //    List<Material> materialList = new List<Material>();

        //    foreach (Transform transform in transforms)
        //    {
        //        Renderer rendererNonLOD = transform.GetComponent<Renderer>();

        //        if (transform.GetComponent<LODGroup>() != null)
        //        {
        //            foreach (LOD lod in transform.GetComponent<LODGroup>().GetLODs())
        //            {
        //                foreach (Renderer renderer in lod.renderers)
        //                {
        //                    if (renderer == rendererNonLOD)
        //                    {
        //                        rendererNonLOD = null;
        //                    }
        //                    if (renderer != null && renderer.materials.Length > 0)
        //                    {
        //                        foreach (Material material in renderer.materials)
        //                        {
        //                            if (material != null && STSShaders.Contains(material.shader))
        //                            {
        //                                if (!materialList.Contains(material))
        //                                {
        //                                    materialList.Add(material);
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //        if (rendererNonLOD != null)
        //        {
        //            Material[] materials = rendererNonLOD.materials;
        //            if (materials.Length > 0)
        //            {
        //                foreach (Material material in materials)
        //                {
        //                    if (material != null && STSShaders.Contains(material.shader))
        //                    {
        //                        if (!materialList.Contains(material))
        //                        {
        //                            materialList.Add(material);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    return materialList;
        //}

        /// <summary>
        /// Gets all materials, including the ones in various LOD levels, which are associated with given transforms
        public static List<Material> getAllSTSMaterialsFromTransforms(Transform[] transforms, List<Shader> STSShaders, Material referenceMaterial)
        {
            //List<Material> materialTracker = new List<Material>();

            List<Material> materialList = new List<Material>();

            foreach (Transform transform in transforms)
            {
                Renderer rendererNonLOD = transform.GetComponent<Renderer>();

                if (transform.GetComponent<LODGroup>() != null)
                {
                    foreach (LOD lod in transform.GetComponent<LODGroup>().GetLODs())
                    {
                        foreach (Renderer renderer in lod.renderers)
                        {
                            if (renderer == rendererNonLOD)
                            {
                                rendererNonLOD = null;
                            }
                            if (renderer != null && renderer.sharedMaterials.Length > 0)
                            {
                                foreach (Material material in renderer.sharedMaterials)
                                {
                                    if (material != null && STSShaders.Contains(material.shader))
                                    {
                                        if (!materialList.Contains(material))
                                        {
                                            materialList.Add(material);
                                        }

                                        //if (!materialTracker.Contains(material))
                                        //{
                                        //    materialList.Add(new Material(material));
                                        //    materialTracker.Add(material);
                                        //}
                                    }
                                }
                            }
                        }
                    }
                }

                if (rendererNonLOD != null)
                {
                    Material[] materials = rendererNonLOD.sharedMaterials;
                    if (materials.Length > 0)
                    {
                        foreach (Material material in materials)
                        {
                            if (material != null && STSShaders.Contains(material.shader))
                            {
                                if (!materialList.Contains(material))
                                {
                                    materialList.Add(material);
                                }

                                //if (!materialTracker.Contains(material))
                                //{
                                //    materialList.Add(new Material(material));
                                //    materialTracker.Add(material);
                                //}
                            }
                        }
                    }
                }
            }

            return materialList;
        }

        public static void updateSeeThroughShaderMaterialPropertiesAndKeywords(Transform[] transforms, string seeThroughShaderName, Material referenceMaterial)
        {

            Material firstInstancedMaterial = getFirstInstancedMaterial(transforms, seeThroughShaderName);
            List<string> namesOfChangedProperties = getNamesOfAllChangedPropertyValues(firstInstancedMaterial, referenceMaterial);
            List<string> namesOfChangedKeywords = getNamesOfAllChangedKeywordValues(firstInstancedMaterial, referenceMaterial);
            if(namesOfChangedProperties != null && namesOfChangedKeywords != null)
            {    
                if (namesOfChangedProperties.Count > 0 || namesOfChangedKeywords.Count > 0)
                {
                        List<Material> allSTSMaterials = getAllSTSMaterialsFromTransforms(transforms, seeThroughShaderName, referenceMaterial);
                        foreach (Material material in allSTSMaterials)
                        {
                            if (namesOfChangedProperties.Count > 0)
                            {
                                updateMaterialProperties(material, referenceMaterial, namesOfChangedProperties);
                            }

                            if (namesOfChangedKeywords.Count > 0)
                            {
                                updateMaterialKeywords(material, referenceMaterial, namesOfChangedKeywords);
                            }
                        }
                }
            }   
            else
            {
                Debug.LogWarning("namesOfChangedProperties == null: " + (namesOfChangedProperties == null));
                Debug.LogWarning("namesOfChangedKeywords == null: " + (namesOfChangedKeywords == null));
            }
        }


        public static void updateSeeThroughShaderMaterialPropertiesAndKeywords(Transform[] transforms, List<Shader> STSShaders, Material referenceMaterial)
        {

            Material firstInstancedMaterial = getFirstInstancedMaterial(transforms, STSShaders);
            List<string> namesOfChangedProperties = getNamesOfAllChangedPropertyValues(firstInstancedMaterial, referenceMaterial);
            List<string> namesOfChangedKeywords = getNamesOfAllChangedKeywordValues(firstInstancedMaterial, referenceMaterial);
            if (namesOfChangedProperties.Count > 0 || namesOfChangedKeywords.Count > 0)
            {
                List<Material> allSTSMaterials = getAllSTSMaterialsFromTransforms(transforms, STSShaders, referenceMaterial);
                foreach (Material material in allSTSMaterials)
                {
                    if (namesOfChangedProperties.Count > 0)
                    {
                        updateMaterialProperties(material, referenceMaterial, namesOfChangedProperties);
                    }

                    if (namesOfChangedKeywords.Count > 0)
                    {
                        updateMaterialKeywords(material, referenceMaterial, namesOfChangedKeywords);
                    }
                }
            }
        }



        public static void updateSeeThroughShaderMaterialPropertiesAndKeywords(Material[] materialsWithSTS, List<Shader> STSShaders, Material referenceMaterial)
        {
            Material firstInstancedMaterial = null;
            foreach (Material material in materialsWithSTS)
            {
                if (material != null && STSShaders.Contains(material.shader))
                {
                    firstInstancedMaterial = material;
                    break;
                }
            }
            List<string> namesOfChangedProperties = getNamesOfAllChangedPropertyValues(firstInstancedMaterial, referenceMaterial);
            List<string> namesOfChangedKeywords = getNamesOfAllChangedKeywordValues(firstInstancedMaterial, referenceMaterial);
            if (namesOfChangedProperties.Count > 0 || namesOfChangedKeywords.Count > 0)
            {
                foreach (Material material in materialsWithSTS)
                {
                    if (namesOfChangedProperties.Count > 0)
                    {
                        updateMaterialProperties(material, referenceMaterial, namesOfChangedProperties);
                    }

                    if (namesOfChangedKeywords.Count > 0)
                    {
                        updateMaterialKeywords(material, referenceMaterial, namesOfChangedKeywords);
                    }
                }
            }
        }

        public static void updateSeeThroughShaderMaterialKeywords(Transform[] transforms, string seeThroughShaderName, Material referenceMaterial)
        {

            Material firstInstancedMaterial = getFirstInstancedMaterial(transforms, seeThroughShaderName);
            List<string> namesOfChangedKeywords = getNamesOfAllChangedKeywordValues(firstInstancedMaterial, referenceMaterial);
            if (namesOfChangedKeywords.Count > 0)
            {
                List<Material> allSTSMaterials = getAllSTSMaterialsFromTransforms(transforms, seeThroughShaderName, referenceMaterial);
                foreach (Material material in allSTSMaterials)
                {
                    if (namesOfChangedKeywords.Count > 0)
                    {
                        updateMaterialKeywords(material, referenceMaterial, namesOfChangedKeywords);
                    }
                }
            }
        }

        private static void updateMaterialProperties(Material instancedMaterial, Material referenceMaterial, List<string> namesOfChangedProperties)
        {
            foreach (string propertyName in namesOfChangedProperties)
            {
                if (propertyName.Equals(SeeThroughShaderConstants.PROPERTY_DISSOLVE_TEX))
                {
                    Texture disTex = referenceMaterial.GetTexture(SeeThroughShaderConstants.PROPERTY_DISSOLVE_TEX);
                    instancedMaterial.SetTexture(SeeThroughShaderConstants.PROPERTY_DISSOLVE_TEX, disTex);
                }
                else if (propertyName.Equals(SeeThroughShaderConstants.PROPERTY_DISSOLVE_MASK))
                {
                    Texture disMask = referenceMaterial.GetTexture(SeeThroughShaderConstants.PROPERTY_DISSOLVE_MASK);
                    instancedMaterial.SetTexture(SeeThroughShaderConstants.PROPERTY_DISSOLVE_MASK, disMask);
                }
                else if (propertyName.Equals(SeeThroughShaderConstants.PROPERTY_OBSTRUCTION_CURVE))
                {
                    Texture obstructionCurve = referenceMaterial.GetTexture(SeeThroughShaderConstants.PROPERTY_OBSTRUCTION_CURVE);
                    instancedMaterial.SetTexture(SeeThroughShaderConstants.PROPERTY_OBSTRUCTION_CURVE, obstructionCurve);
                }
                else if (propertyName.Equals(SeeThroughShaderConstants.PROPERTY_DISSOLVE_COLOR))
                {
                    instancedMaterial.SetColorArray(SeeThroughShaderConstants.PROPERTY_DISSOLVE_COLOR, referenceMaterial.GetColorArray(SeeThroughShaderConstants.PROPERTY_DISSOLVE_COLOR));
                }
                else
                {
                    bool isCulling = false;
#if USING_HDRP
                   // isCulling = propertyName == "_CullMode" || propertyName == "_CullModeForward";
#elif USING_URP
                    
#else
                    isCulling = (propertyName == "_Cull");
#endif
                    if (isCulling)
                    {
                        if (referenceMaterial.GetFloat("_SyncCullMode") == 1)
                        {
                            float temp = referenceMaterial.GetFloat(propertyName);
                            instancedMaterial.SetFloat(propertyName, temp);
                        }
                    } else
                    {
                        float temp = referenceMaterial.GetFloat(propertyName);
                        instancedMaterial.SetFloat(propertyName, temp);
                    }

                }
            }
        }


        private static void updateMaterialKeywords(Material instancedMaterial, Material referenceMaterial, List<string> namesOfChangedKeywords)
        {
            foreach (string keyword in namesOfChangedKeywords)
            {
                bool isKeywordEnabled = referenceMaterial.IsKeywordEnabled(keyword);
                if(isKeywordEnabled)
                {
                    instancedMaterial.EnableKeyword(keyword);
                } else
                {
                    instancedMaterial.DisableKeyword(keyword);
                }
            }
        }

        private static List<string> getNamesOfAllChangedPropertyValues(Material instancedMaterial, Material referenceMaterial) //, bool force = false)
        {
            if (instancedMaterial != null && referenceMaterial != null)
            {

                if (GeneralUtils.STS_SHADER_LIST.Contains(instancedMaterial.shader.name) || STSCustomShaderMappingsStorage.Instance.STSCustomShaderMappingsDict.ContainsKey(instancedMaterial.shader.name)
                    &&
                    GeneralUtils.STS_SHADER_LIST.Contains(referenceMaterial.shader.name) || STSCustomShaderMappingsStorage.Instance.STSCustomShaderMappingsDict.ContainsKey(referenceMaterial.shader.name)
                    )
                {

                    List<string> namesOfChangedProperties = new List<string>();
                    foreach (string propertyName in GeneralUtils.STS_SYNC_PROPERTIES_LIST)
                    {
                        if(propertyName == "_Cull" && instancedMaterial.shader.name != "SeeThroughShader/BiRP/Standard")
                        {

                        } else
                        {
                            float instancedValue = instancedMaterial.GetFloat(propertyName);
                            float referenceValue = referenceMaterial.GetFloat(propertyName);
                            if ((instancedValue != referenceValue)) // || force)
                            {
                                namesOfChangedProperties.Add(propertyName);
                            }
                        }

                    }
                    string dissolveTexPropertyName = SeeThroughShaderConstants.PROPERTY_DISSOLVE_TEX;
                    Texture instancedDissolveTexture = instancedMaterial.GetTexture(dissolveTexPropertyName);
                    Texture referenceDissolveTexture = referenceMaterial.GetTexture(dissolveTexPropertyName);
                    if (instancedDissolveTexture != null && referenceDissolveTexture != null)
                    {
                        if (!String.Equals(instancedDissolveTexture.name, referenceDissolveTexture.name))
                        {
                            namesOfChangedProperties.Add(dissolveTexPropertyName);
                        }
                    } 
                    else if ((instancedDissolveTexture != null) != (referenceDissolveTexture != null))
                    {
                        namesOfChangedProperties.Add(dissolveTexPropertyName);
                    }


                    string dissolveMaskPropertyName = SeeThroughShaderConstants.PROPERTY_DISSOLVE_MASK;
                    Texture instancedDissolveMask = instancedMaterial.GetTexture(dissolveMaskPropertyName);
                    Texture referenceDissolveMask = referenceMaterial.GetTexture(dissolveMaskPropertyName);
                    if (instancedDissolveMask != null && referenceDissolveMask != null)
                    {
                        if (!String.Equals(instancedDissolveMask.name, referenceDissolveMask.name))
                        {
                            namesOfChangedProperties.Add(dissolveMaskPropertyName);
                        }
                    }
                    else if ((instancedDissolveMask != null) != (referenceDissolveMask != null))
                    {
                        namesOfChangedProperties.Add(dissolveMaskPropertyName);
                    }

                    string obstructionCurvePropertyName = SeeThroughShaderConstants.PROPERTY_OBSTRUCTION_CURVE;
                    Texture instancedObstructionCurve = instancedMaterial.GetTexture(obstructionCurvePropertyName);
                    Texture referenceObstructionCurve = referenceMaterial.GetTexture(obstructionCurvePropertyName);
                    if (instancedObstructionCurve != null && referenceObstructionCurve != null)
                    {
                        if (!String.Equals(instancedObstructionCurve.name, referenceObstructionCurve.name))
                        {
                            namesOfChangedProperties.Add(obstructionCurvePropertyName);
                        }
                    }
                    else if ((instancedObstructionCurve != null) != (referenceObstructionCurve != null))
                    {
                        namesOfChangedProperties.Add(obstructionCurvePropertyName);
                    }

                    string dissolveColorPropertyName = SeeThroughShaderConstants.PROPERTY_DISSOLVE_COLOR;
                    Color[] instancedColor = instancedMaterial.GetColorArray(dissolveColorPropertyName);
                    Color[] referenceColor = referenceMaterial.GetColorArray(dissolveColorPropertyName);
                    if (!instancedColor[0].Equals(referenceColor[0]))
                    {
                        namesOfChangedProperties.Add(dissolveColorPropertyName);
                    }

                    return namesOfChangedProperties;

                }
                else
                {
                    if (!GeneralUtils.STS_SHADER_LIST.Contains(instancedMaterial.shader.name)
                        && !STSCustomShaderMappingsStorage.Instance.STSCustomShaderMappingsDict.ContainsKey(instancedMaterial.shader.name))
                    {
                        Debug.LogWarning("The instanced material(" + instancedMaterial.name + ") has a shader("+ instancedMaterial.shader.name + ") that wasn't recognized by the See-through Shader System");

                    }

                    if (!GeneralUtils.STS_SHADER_LIST.Contains(referenceMaterial.shader.name)
                        && !STSCustomShaderMappingsStorage.Instance.STSCustomShaderMappingsDict.ContainsKey(instancedMaterial.shader.name))
                    {
                        Debug.LogWarning("The reference material(" + referenceMaterial.name + ") has a shader(" + referenceMaterial.shader.name + ") that wasn't recognized by the See-through Shader System");
                    }
                    return null;
                }


            }
            else
            {
                if (instancedMaterial == null)
                {
                    Debug.LogWarning("Some instanced material wasn't found while updating the STS properties. Please check your replacement method");

                }
                if (referenceMaterial == null)
                {
                    Debug.LogWarning("Some reference material wasn't found while updating the STS properties. Please check your replacement method");
                } else
                {
                    if (!GeneralUtils.STS_SHADER_LIST.Contains(referenceMaterial.shader.name) && !STSCustomShaderMappingsStorage.Instance.STSCustomShaderMappingsDict.ContainsKey(referenceMaterial.shader.name))
                    {
                        Debug.LogWarning("The reference material(" + referenceMaterial.name + ") has a shader(" + referenceMaterial.shader.name + ") that wasn't recognized by the See-through Shader System");
                    }
                }
                return null;
            }
        }


        private static List<string> getNamesOfAllChangedKeywordValues(Material instancedMaterial, Material referenceMaterial)
        {
            if (instancedMaterial != null && referenceMaterial != null)
            {

                if (GeneralUtils.STS_SHADER_LIST.Contains(instancedMaterial.shader.name) || STSCustomShaderMappingsStorage.Instance.STSCustomShaderMappingsDict.ContainsKey(instancedMaterial.shader.name)
                    &&
                    GeneralUtils.STS_SHADER_LIST.Contains(referenceMaterial.shader.name) || STSCustomShaderMappingsStorage.Instance.STSCustomShaderMappingsDict.ContainsKey(referenceMaterial.shader.name)
                    )
                {

                    List<string> namesOfChangedProperties = new List<string>();
                    foreach (string keyword in GeneralUtils.STS_KEYWORDS_LIST)
                    {

                        bool instancedValue = instancedMaterial.IsKeywordEnabled(keyword);
                        bool referenceValue = referenceMaterial.IsKeywordEnabled(keyword);
                        if (instancedValue != referenceValue)
                        {
                            namesOfChangedProperties.Add(keyword);
                        }
                    }

                    return namesOfChangedProperties;

                }
                else
                {
                    if (!GeneralUtils.STS_SHADER_LIST.Contains(instancedMaterial.shader.name) && !STSCustomShaderMappingsStorage.Instance.STSCustomShaderMappingsDict.ContainsKey(instancedMaterial.shader.name))
                    {
                        Debug.LogWarning("The instanced material(" + instancedMaterial.name + ") has a shader(" + instancedMaterial.shader.name + ") that wasn't recognized by the See-through Shader System!");

                    }

                    if (!GeneralUtils.STS_SHADER_LIST.Contains(referenceMaterial.shader.name) && !STSCustomShaderMappingsStorage.Instance.STSCustomShaderMappingsDict.ContainsKey(referenceMaterial.shader.name))
                    {
                        Debug.LogWarning("The reference material(" + referenceMaterial.name + ") has a shader(" + referenceMaterial.shader.name + ") that wasn't recognized by the See-through Shader System!");
                    }
                    return null;
                }


            }
            else
            {
                if (instancedMaterial == null)
                {
                    Debug.LogWarning("Some instanced material wasn't found while updating the STS properties. Please check your replacement method");

                }
                if (referenceMaterial == null)
                {
                    Debug.LogWarning("Some reference material wasn't found while updating the STS properties. Please check your replacement method");
                } else
                {
                    if (!GeneralUtils.STS_SHADER_LIST.Contains(referenceMaterial.shader.name))
                    {
                        Debug.LogWarning("The reference material(" + referenceMaterial.name + ") has a shader(" + referenceMaterial.shader.name + ") that wasn't recognized by the See-through Shader System");
                    }
                }
                return null;
            }
        }


        public static void RenameInstancedMaterialName(Material materialInstance, string referenceMaterialName)
        {
            string name = materialInstance.name;
            name = name.Replace(" (Instance)", "");
            materialInstance.name = name + " (" + SeeThroughShaderConstants.STS_INSTANCE_PREFIX + " '" + referenceMaterialName + "')";
        }

        public static void AddSTSInstancePrefix(Material materialInstance)
        {
            string name = materialInstance.name;
            name = name.Replace(" (Instance)", "");
            if(!name.Contains(SeeThroughShaderConstants.STS_INSTANCE_PREFIX))
            {
                materialInstance.name = name + " (" + SeeThroughShaderConstants.STS_INSTANCE_PREFIX + ")";
            }
        }

        public static void AddSTSTriggerPrefix(Material materialInstance)
        {
            string name = materialInstance.name;
            name = name.Replace(" (Instance)", "");
            if (!name.Contains(SeeThroughShaderConstants.STS_TRIGGER_PREFIX))
            {
                materialInstance.name = name + " (" + SeeThroughShaderConstants.STS_TRIGGER_PREFIX + ")";
            }
        }



        public static void RenameInstancedMaterialNameSynchronization(Material materialInstance, string referenceMaterialName)
        {
            string name = materialInstance.name;
            //name = name.Replace(" (Instance)", "");
            materialInstance.name = name + " (" + SeeThroughShaderConstants.STS_SYNCHRONIZATION_PREFIX + " '" + referenceMaterialName + "')";
        }

        private static Material getFirstInstancedMaterial(Transform[] transforms, string seeThroughShaderName)
        {
            if (transforms != null && transforms.Length > 0)
            {
                foreach (Transform transform in transforms)
                {
                    if (transform != null)
                    {
                        Renderer renderer = transform.GetComponent<Renderer>();
                        if (renderer != null)
                        {
                            Material[] materials = renderer.materials;
                            if (materials != null && materials.Length > 0)
                            {
                                foreach (Material material in materials)
                                {
                                    if (material != null && material.shader.name == seeThroughShaderName)
                                    {
                                        return material;
                                    }
                                }
                            }

                        }
                    }
                }
            }
            return null;
        }

        //private static Material getFirstInstancedMaterial(Transform[] transforms, List<Shader> STSShaders)
        //{
        //    if (transforms != null && transforms.Length > 0)
        //    {
        //        foreach (Transform transform in transforms)
        //        {
        //            if (transform != null)
        //            {
        //                Renderer renderer = transform.GetComponent<Renderer>();
        //                if (renderer != null)
        //                {
        //                    Material[] materials = renderer.materials;
        //                    if (materials != null && materials.Length > 0)
        //                    {
        //                        foreach (Material material in materials)
        //                        {
        //                            if (material != null && STSShaders.Contains(material.shader))
        //                            {
        //                                return material;
        //                            }
        //                        }
        //                    }

        //                }
        //            }
        //        }
        //    }
        //    return null;
        //}       
        private static Material getFirstInstancedMaterial(Transform[] transforms, List<Shader> STSShaders)
        {
            if (transforms != null && transforms.Length > 0)
            {
                foreach (Transform transform in transforms)
                {
                    if (transform != null)
                    {
                        Renderer renderer = transform.GetComponent<Renderer>();
                        if (renderer != null)
                        {
                            Material[] materials = renderer.sharedMaterials;
                            if (materials != null && materials.Length > 0)
                            {
                                foreach (Material material in materials)
                                {
                                    if (material != null && STSShaders.Contains(material.shader))
                                    {
                                        return material;
                                    }
                                }
                            }

                        }
                    }
                }
            }
            return null;
        }

        public static float getFirstPropertyValueFoundInMaterialList(List<Material> materialList, string propertyString)
        {
            float id = -1;
            if (materialList != null && materialList.Count > 0)
            {
                foreach (Material mat in materialList)
                {
                    if (mat != null)
                    {
                        if (!mat.HasProperty(propertyString) || mat.GetFloat(propertyString) == 0)
                        {
                        }
                        else
                        {
                            if (id == -1)
                            {
                                id = mat.GetFloat(propertyString);
                            }
                            else
                            {
                                Debug.Assert(id == mat.GetFloat(propertyString), "Materials have different Ids! Bug?");
                            }

                        }
                    }
                }
            }
            else
            {
                Debug.Assert(materialList.Count > 0, "Empty Material List. No materials with the See-through Shader could be found. It seems like you didn't apply the " +
                    "See-through Shader correctly. Please check if the See-through Shader is on the GameObjects during runtime. Are your GlobalShaderReplacement and/or" +
                    " GroupShaderReplacement settings correct? Maybe you selected the wrong layermask?");
            }
            if (id == -1)
            {
                id = IdGenerator.Instance.Id;
            }
            return id;
        }


        public static List<string> MaterialsNoApplyListToNameList(List<Material> materialExemptions)
        {
            List<string> materialNoApplyNames;
            if (materialExemptions != null && materialExemptions.Count > 0)
            {
                materialNoApplyNames = new List<string>();
                foreach (Material mat in materialExemptions)
                {
                    if (!materialNoApplyNames.Contains(mat.name))
                    {
                        materialNoApplyNames.Add(mat.name);
                    }
                }
                return materialNoApplyNames;
            }
            else
            {
                return null;
            }
        }


        public static void AddIfSeeThroughShaderMaterial(GameObject gameObject, string seeThroughShaderName, List<Material> listMaterial)
        {
            Renderer renderer = gameObject.GetComponent<Renderer>();
            if (renderer != null && renderer.materials.Length > 0)
            {
                if (gameObject.gameObject.GetComponent<SeeThroughShaderPlayer>() == null || !gameObject.gameObject.GetComponent<SeeThroughShaderPlayer>().isActiveAndEnabled)// && item.gameObject.GetComponent<SeeThroughShaderExemption>() == null)
                {
                    for (int j = 0; j < renderer.materials.Length; j++)
                    {
                        if (renderer.materials[j] != null && renderer.materials[j].shader.name == seeThroughShaderName)
                        {
                            listMaterial.Add(renderer.materials[j]);
                        }
                    }
                }
            }
        }

        public static void AddIfSeeThroughShaderMaterial(GameObject gameObject, List<string> STSShaderNames, List<Material> listMaterial)
        {
            Renderer renderer = gameObject.GetComponent<Renderer>();
            if (renderer != null && renderer.materials.Length > 0)
            {
                if (gameObject.gameObject.GetComponent<SeeThroughShaderPlayer>() == null || !gameObject.gameObject.GetComponent<SeeThroughShaderPlayer>().isActiveAndEnabled)// && item.gameObject.GetComponent<SeeThroughShaderExemption>() == null)
                {
                    for (int j = 0; j < renderer.materials.Length; j++)
                    {
                        if (renderer.materials[j] != null && STSShaderNames.Contains(renderer.materials[j].shader.name))
                        {
                            listMaterial.Add(renderer.materials[j]);
                        }
                    }
                }
            }
        }


        //public static void AddIfSeeThroughShaderMaterial(GameObject gameObject, List<GameObject> listGameObject)
        //{
        //    Renderer renderer = gameObject.GetComponent<Renderer>();
        //    if (renderer != null && renderer.materials.Length > 0)
        //    {
        //        if (gameObject.gameObject.GetComponent<SeeThroughShaderPlayer>() == null || !gameObject.gameObject.GetComponent<SeeThroughShaderPlayer>().isActiveAndEnabled)// && item.gameObject.GetComponent<SeeThroughShaderExemption>() == null)
        //        {
        //            for (int j = 0; j < renderer.materials.Length; j++)
        //            {
        //                if (renderer.materials[j] != null && STSCustomShaderMappingsStorage.Instance.AllSTSShaders.Contains(renderer.materials[j].shader.name))
        //                {
        //                    listMaterial.Add(renderer.materials[j]);
        //                }
        //            }
        //        }
        //    }
        //}



        // UNUSED
        //public static void adjustHDRPMaterial(Material original, Material STS)
        //{
        //    if (original.HasProperty("_NormalMap"))
        //    {
        //        Texture normalMap = original.GetTexture("_NormalMap");
        //        if (normalMap != null)
        //        {
        //            STS.SetTexture("_BumpMap", normalMap);
        //            STS.EnableKeyword("_NORMALMAP");
        //        }

        //    }
        //    if (original.HasProperty("_NormalScale"))
        //    {
        //        float normalScale = original.GetFloat("_NormalScale");
        //        STS.SetFloat("_BumpScale", normalScale);
        //    }
        //    if (original.HasProperty("_EmissiveColorMap"))
        //    {
        //        Texture emissiveColorMap = original.GetTexture("_EmissiveColorMap");
        //        if (emissiveColorMap != null)
        //        {
        //            STS.SetTexture("_EmissionMap", emissiveColorMap);
        //            STS.EnableKeyword("_EMISSION");
        //        }
        //    }
        //    if (original.HasProperty("_EmissiveColor"))
        //    {
        //        Color emissiveColor = original.GetColor("_EmissiveColor");
        //        if (emissiveColor != null)
        //        {
        //            STS.SetColor("_EmissionColor", emissiveColor);
        //            STS.EnableKeyword("_EMISSION");
        //            STS.globalIlluminationFlags = MaterialGlobalIlluminationFlags.AnyEmissive;
        //        }
        //    }
        //}

        // UNUSED
        //public static void adjustURPMaterial(Material original, Material STS)
        //{
        //    if (original.HasProperty("_MetallicGlossMap"))
        //    {
        //        if (original.IsKeywordEnabled("_METALLICSPECGLOSSMAP"))
        //        {
        //            STS.EnableKeyword("_METALLICGLOSSMAP");
        //        }

        //    }
        //    if (original.HasProperty("_Smoothness"))
        //    {
        //        float smoothness = original.GetFloat("_Smoothness");
        //        STS.SetFloat("_GlossMapScale", smoothness);
        //    }
        //}
    }
}