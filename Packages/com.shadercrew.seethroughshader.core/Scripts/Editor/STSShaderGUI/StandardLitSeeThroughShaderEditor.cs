using System.Collections;
using System.Collections.Generic;
using UnityEditor;
#if USING_URP
    using UnityEditor.Rendering.Universal.ShaderGUI;
#endif
using UnityEngine;
using static ShaderCrew.SeeThroughShader.GeneralUtils;

namespace ShaderCrew.SeeThroughShader
{
    public class StandardLitSeeThroughShaderEditor : SeeThroughShaderEditorAbstract
    {
        StandardLitGUI standardLitGUI;



#if USING_HDRP
#if UNITY_2021_1_OR_NEWER
                    // For lit GUI we don't display the heightmap nor layering options
                    const LitSurfaceInputsUIBlock.Features litSurfaceFeatures = LitSurfaceInputsUIBlock.Features.All ^ LitSurfaceInputsUIBlock.Features.HeightMap ^ LitSurfaceInputsUIBlock.Features.LayerOptions;

                    MaterialUIBlockList uiBlocks = new MaterialUIBlockList
                    {
                        new SurfaceOptionUIBlock(MaterialUIBlock.ExpandableBit.Base, features: SurfaceOptionUIBlock.Features.Lit),
                        //new TessellationOptionsUIBlock(MaterialUIBlock.ExpandableBit.Tessellation),
                        new LitSurfaceInputsUIBlock(MaterialUIBlock.ExpandableBit.Input, features: litSurfaceFeatures),
                        new DetailInputsUIBlock(MaterialUIBlock.ExpandableBit.Detail),
                        //new TransparencyUIBlock(MaterialUIBlock.ExpandableBit.Transparency, features: TransparencyUIBlock.Features.All & ~TransparencyUIBlock.Features.Distortion),
                        new EmissionUIBlock(MaterialUIBlock.ExpandableBit.Emissive),
                        new AdvancedOptionsUIBlock(MaterialUIBlock.ExpandableBit.Advance, AdvancedOptionsUIBlock.Features.StandardLit),
                    };

                    public override void ValidateMaterial(Material material) => LitAPI.ValidateMaterial(material);
#else

//SEE: https://github.com/Unity-Technologies/Graphics/blob/v10.10.0/com.unity.render-pipelines.high-definition/Editor/Material/Lit/LitGUI.cs
                    const LitSurfaceInputsUIBlock.Features litSurfaceFeatures = LitSurfaceInputsUIBlock.Features.All ^ LitSurfaceInputsUIBlock.Features.HeightMap ^ LitSurfaceInputsUIBlock.Features.LayerOptions;

                    MaterialUIBlockList uiBlocks = new MaterialUIBlockList
                    {
                        new SurfaceOptionUIBlock(MaterialUIBlock.Expandable.Base, features: SurfaceOptionUIBlock.Features.Lit),
                        //new TessellationOptionsUIBlock(MaterialUIBlock.Expandable.Tesselation),
                        new LitSurfaceInputsUIBlock(MaterialUIBlock.Expandable.Input, features: litSurfaceFeatures),
                        new DetailInputsUIBlock(MaterialUIBlock.Expandable.Detail),
                        //// We don't want distortion in Lit
                        //new TransparencyUIBlock(MaterialUIBlock.Expandable.Transparency, features: TransparencyUIBlock.Features.All & ~TransparencyUIBlock.Features.Distortion),
                        new EmissionUIBlock(MaterialUIBlock.Expandable.Emissive),
                        new AdvancedOptionsUIBlock(MaterialUIBlock.Expandable.Advance, AdvancedOptionsUIBlock.Features.StandardLit),
                    };
                    protected bool m_FirstFrame = true;
                    protected void ApplyKeywordsAndPassesIfNeeded(bool changed, Material[] materials)
                    {
                        if (changed || m_FirstFrame)
                        {
                            m_FirstFrame = false;

                            foreach (var material in materials)
                                SetupMaterialKeywordsAndPassInternal(material);
                        }
                    }


        protected void SetupMaterialKeywordsAndPassInternal(Material material) => LitAPI.ValidateMaterial(material);      


#endif
#endif

#if USING_URP
        LitShader LitShader = new LitShader();
#endif




        public override void InitializeGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
        {
            if (rp == RenderPipeline.BiRP)
            {
                if (standardLitGUI == null)
                {
                    standardLitGUI = new StandardLitGUI();
                }
                FindPropertiesStandardLit(properties);
                standardLitGUI.m_MaterialEditor = materialEditor;
            }
        }

        public override void DoSetup(MaterialEditor materialEditor)
        {
            if (rp == RenderPipeline.BiRP)
            {
                standardLitGUI.DoSetup(materialEditor);

                if (EditorGUIUtility.isProSkin)
                {
                    standardLitGUI.textColor = Color.white;                    
                }
                else
                {
                    standardLitGUI.textColor = Color.black;
                }
            }

        }

        public override void DoGUI(Material material, MaterialEditor materialEditor, MaterialProperty[] properties)
        {
            if (rp == RenderPipeline.HDRP || rp == RenderPipeline.URP)
            {
                Rect rectt = EditorGUILayout.BeginVertical();
                rectt = new Rect(0, rectt.y, Screen.width, rectt.height);
                GUI.Box(rectt, GUIContent.none);
            }
            else if (rp == RenderPipeline.BiRP)
            {
                Rect rectt = EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                GUI.Box(rectt, GUIContent.none);
            }

            if (!seeThroughShaderGUI.isReferenceMaterial)
            {

                GUIStyle replacementStyle = new GUIStyle();
                replacementStyle.normal.textColor = textColor;
                replacementStyle.alignment = TextAnchor.MiddleCenter;
                replacementStyle.fontStyle = FontStyle.Bold;
                replacementStyle.fontSize = 14;
                replacementStyle.richText = true;

                if (rp == RenderPipeline.HDRP)
                {
                    EditorUtils.DrawUILineFull(padding: 0);

                    EditorGUILayout.Space();
                    EditorGUILayout.Space();
                    GUILayout.Label("HDRP Lit Shader : <i>Properties</i>", replacementStyle);
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();
#if USING_HDRP
#if UNITY_2021_1_OR_NEWER
                    uiBlocks.OnGUI(materialEditor, properties);
                    ValidateMaterial(material);
#else
                    using (var changed = new EditorGUI.ChangeCheckScope())
                    {
                        uiBlocks.OnGUI(materialEditor, properties);
                        ApplyKeywordsAndPassesIfNeeded(changed.changed, uiBlocks.materials);
                    }
#endif
#endif
                    EditorUtils.DrawUILineFull(padding: 0);
                }
                else if(rp == RenderPipeline.URP)
                {
                    EditorUtils.DrawUILineFull(padding: 0);

                    EditorGUILayout.Space();
                    EditorGUILayout.Space();
                    GUILayout.Label("URP Lit Shader : <i>Properties</i>", replacementStyle);
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();


#if USING_URP
                    LitShader.SetMaterialKeywords(material, LitGUI.SetMaterialKeywords, LitDetailGUI.SetMaterialKeywords);
                    LitShader.OnGUI(materialEditor, properties);
#endif

                    EditorUtils.DrawUILineFull(padding: 0);
                }
                else if (rp == RenderPipeline.BiRP)
                {
                    standardLitGUI.StandardShaderPropertiesGUI(material);
                }

            }
            else
            {
                if (rp == RenderPipeline.BiRP)
                {
                    standardLitGUI.OnlyCullModeGUI();
                }

            }
            //EditorUtils.makeHorizontalSeparation();
            EditorGUILayout.EndVertical();
        }



        public void FindPropertiesStandardLit(MaterialProperty[] props)
        {

            // Standard
            standardLitGUI.blendMode = FindProperty("_Mode", props);
            standardLitGUI.cullMode = FindProperty("_Cull", props);
            //alphaMode = FindProperty("_AlphaMode", props);

            standardLitGUI.albedoMap = FindProperty("_MainTex", props);
            standardLitGUI.albedoColor = FindProperty("_Color", props);
            standardLitGUI.alphaCutoff = FindProperty("_Cutoff", props);

            standardLitGUI.metallicMap = FindProperty("_MetallicGlossMap", props);
            standardLitGUI.metallic = FindProperty("_Metallic", props);


            standardLitGUI.smoothness = FindProperty("_Glossiness", props);
            standardLitGUI.smoothnessScale = FindProperty("_GlossMapScale", props, false);
            standardLitGUI.smoothnessMapChannel = FindProperty("_SmoothnessTextureChannel", props, false);

            standardLitGUI.bumpScale = FindProperty("_BumpScale", props);
            standardLitGUI.bumpMap = FindProperty("_BumpMap", props);

            standardLitGUI.heigtMapScale = FindProperty("_Parallax", props);
            standardLitGUI.heightMap = FindProperty("_ParallaxMap", props);

            standardLitGUI.occlusionStrength = FindProperty("_OcclusionStrength", props);
            standardLitGUI.occlusionMap = FindProperty("_OcclusionMap", props);

            standardLitGUI.emissionMap = FindProperty("_EmissionMap", props);
            standardLitGUI.emissionColor = FindProperty("_EmissionColor", props);

            standardLitGUI.detailMask = FindProperty("_DetailMask", props);
            standardLitGUI.detailAlbedoMap = FindProperty("_DetailAlbedoMap", props);
            standardLitGUI.detailNormalMap = FindProperty("_DetailNormalMap", props);
            standardLitGUI.detailNormalMapScale = FindProperty("_DetailNormalMapScale", props);
            standardLitGUI.uvSetSecondary = FindProperty("_UVSec", props);

        }



    }
}