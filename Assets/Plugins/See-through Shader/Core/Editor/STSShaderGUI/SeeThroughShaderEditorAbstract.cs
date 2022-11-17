using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static ShaderCrew.SeeThroughShader.GeneralUtils;

namespace ShaderCrew.SeeThroughShader
{
    public abstract class SeeThroughShaderEditorAbstract : ShaderGUI
    {
        protected MaterialEditor m_MaterialEditor;
        bool m_FirstTimeApply = true;
        protected SeeThroughShaderGUI seeThroughShaderGUI;
        protected Color textColor;
        protected Color originalColor;



        protected RenderPipeline rp = RenderPipeline.NONE;


        public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
        {

            if (rp == RenderPipeline.NONE)
            {
                rp = GeneralUtils.getCurrentRenderPipeline();
            }

            m_MaterialEditor = materialEditor;
            Material material = materialEditor.target as Material;

            if (seeThroughShaderGUI == null)
            {
                seeThroughShaderGUI = new SeeThroughShaderGUI();
            }


            InitializeGUI(materialEditor, properties);

            FindPropertiesSeeThroughShader(properties);
            seeThroughShaderGUI.m_MaterialEditor = materialEditor;

            if (m_FirstTimeApply)
            {
                DoSetup(materialEditor);
                seeThroughShaderGUI.DoSetup(materialEditor);

                originalColor = EditorStyles.label.normal.textColor;


                if (EditorGUIUtility.isProSkin)
                {
                    textColor = Color.white;
                }
                else
                {
                    textColor = Color.black;
                }
                m_FirstTimeApply = false;

            }


            string name = seeThroughShaderGUI.GetSTSMaterialType();


            //EditorUtils.usualStart(name);
            EditorUtils.LogoOnlyStart(name);

            EditorStyles.label.normal.textColor = textColor;


            if (!material.name.Contains(SeeThroughShaderConstants.STS_INSTANCE_PREFIX))
            {
                seeThroughShaderGUI.isReferenceMaterialMat.floatValue = Convert.ToSingle(EditorGUILayout.ToggleLeft("Is Reference Material",
                                                      Convert.ToBoolean(seeThroughShaderGUI.isReferenceMaterialMat.floatValue)));

                EditorGUILayout.Space();

            }

            if (seeThroughShaderGUI.syncCullMode.floatValue == 1 || !seeThroughShaderGUI.isReferenceMaterial)
            {
                DoGUI(material, materialEditor, properties);
            }


            EditorGUILayout.Space();
            EditorGUILayout.Space();
            GUIStyle STSAreaStyle = new GUIStyle(EditorStyles.helpBox);
            Rect rect = EditorGUILayout.BeginVertical(STSAreaStyle);
            GUI.Box(rect, GUIContent.none);

            seeThroughShaderGUI.STSShaderPropertiesGUI(material);

            EditorGUILayout.EndVertical();

            EditorUtils.LogoOnlyEnd();
            EditorStyles.label.normal.textColor = originalColor;

        }


        public virtual void InitializeGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
        {

        }

        public virtual void DoSetup(MaterialEditor materialEditor)
        {

        }

        public abstract void DoGUI(Material material, MaterialEditor materialEditor, MaterialProperty[] properties);



        public void FindPropertiesSeeThroughShader(MaterialProperty[] props)
        {
            seeThroughShaderGUI.isReferenceMaterialMat = FindProperty("_isReferenceMaterial", props);


            // See-through Shader
            seeThroughShaderGUI.dissolveMap = FindProperty("_DissolveTex", props);
            seeThroughShaderGUI.dissolveColor = FindProperty("_DissolveColor", props);
            seeThroughShaderGUI.dissolveSize = FindProperty("_UVs", props);
            seeThroughShaderGUI.dissolveColorSaturation = FindProperty("_DissolveColorSaturation", props);

            seeThroughShaderGUI.dissolveEmmission = FindProperty("_DissolveEmission", props);
            seeThroughShaderGUI.dissolveEmmissionBooster = FindProperty("_DissolveEmissionBooster", props);
            seeThroughShaderGUI.dissolveTexturedEmissionEdge = FindProperty("_TexturedEmissionEdge", props);
            seeThroughShaderGUI.dissolveTexturedEmissionEdgeStrength = FindProperty("_TexturedEmissionEdgeStrength", props);

            seeThroughShaderGUI.dissolveClippedShadowsEnabled = FindProperty("_hasClippedShadows", props);


            seeThroughShaderGUI.dissolveTextureAnimationEnabled = FindProperty("_AnimationEnabled", props);
            seeThroughShaderGUI.dissolveTextureAnimationSpeed = FindProperty("_AnimationSpeed", props);
            seeThroughShaderGUI.dissolveTransitionDuration = FindProperty("_TransitionDuration", props);


            seeThroughShaderGUI.interactionMode = FindProperty("_InteractionMode", props);
            //centerPosition = FindProperty("_CenterPosition", props);

            seeThroughShaderGUI.obstructionMode = FindProperty("_Obstruction", props);
            seeThroughShaderGUI.angleStrength = FindProperty("_AngleStrength", props);
            seeThroughShaderGUI.coneStrength = FindProperty("_ConeStrength", props);
            seeThroughShaderGUI.coneObstructionDestroyRadius = FindProperty("_ConeObstructionDestroyRadius", props);

            seeThroughShaderGUI.cylinderStrength = FindProperty("_CylinderStrength", props);
            seeThroughShaderGUI.cylinderObstructionDestroyRadius = FindProperty("_CylinderObstructionDestroyRadius", props);

            seeThroughShaderGUI.circleStrength = FindProperty("_CircleStrength", props);
            seeThroughShaderGUI.circleObstructionDestroyRadius = FindProperty("_CircleObstructionDestroyRadius", props);

            seeThroughShaderGUI.curveStrength = FindProperty("_CurveStrength", props);
            seeThroughShaderGUI.curveObstructionDestroyRadius = FindProperty("_CurveObstructionDestroyRadius", props);

            seeThroughShaderGUI.dissolveObstructionCurve = FindProperty("_ObstructionCurve", props);

            seeThroughShaderGUI.dissolveFallOff = FindProperty("_DissolveFallOff", props);
            seeThroughShaderGUI.dissolveMask = FindProperty("_DissolveMask", props);
            seeThroughShaderGUI.dissolveMaskEnabled = FindProperty("_DissolveMaskEnabled", props);

            seeThroughShaderGUI.intrinsicDissolveStrength = FindProperty("_IntrinsicDissolveStrength", props);


            seeThroughShaderGUI.ceilingEnabled = FindProperty("_Ceiling", props);
            seeThroughShaderGUI.ceilingMode = FindProperty("_CeilingMode", props);
            seeThroughShaderGUI.ceilingBlendMode = FindProperty("_CeilingBlendMode", props);
            seeThroughShaderGUI.ceilingY = FindProperty("_CeilingY", props);
            seeThroughShaderGUI.ceilingPlayerYOffset = FindProperty("_CeilingPlayerYOffset", props);
            seeThroughShaderGUI.ceilingYGradientLength = FindProperty("_CeilingYGradientLength", props);


            seeThroughShaderGUI.isometricExlusionEnabled = FindProperty("_IsometricExclusion", props);
            seeThroughShaderGUI.isometricExclusionDistance = FindProperty("_IsometricExclusionDistance", props);
            seeThroughShaderGUI.isometricExclusionGradientLength = FindProperty("_IsometricExclusionGradientLength", props);

            seeThroughShaderGUI.floorEnabled = FindProperty("_Floor", props);
            seeThroughShaderGUI.floorMode = FindProperty("_FloorMode", props);
            seeThroughShaderGUI.floorY = FindProperty("_FloorY", props);
            seeThroughShaderGUI.playerPosYOffset = FindProperty("_PlayerPosYOffset", props);
            seeThroughShaderGUI.floorYTextureGradientLength = FindProperty("_FloorYTextureGradientLength", props);



            seeThroughShaderGUI.zoningEnabled = FindProperty("_Zoning", props);
            seeThroughShaderGUI.zoningMode = FindProperty("_ZoningMode", props);
            seeThroughShaderGUI.zoningEdgeGradientLength = FindProperty("_ZoningEdgeGradientLength", props);
            seeThroughShaderGUI.zoningIsRevealable = FindProperty("_IsZoningRevealable", props);

            seeThroughShaderGUI.zoningSyncZonesWithFloorY = FindProperty("_SyncZonesWithFloorY", props);
            seeThroughShaderGUI.zoningSyncZonesFloorYOffset = FindProperty("_SyncZonesFloorYOffset", props);


            seeThroughShaderGUI.debugModeEnabled = FindProperty("_PreviewMode", props);
            seeThroughShaderGUI.debugModeIndicatorLineThickness = FindProperty("_PreviewIndicatorLineThickness", props);

            seeThroughShaderGUI.isReplacementShader = FindProperty("_IsReplacementShader", props);

            seeThroughShaderGUI.defaultEffectRadius = FindProperty("_DefaultEffectRadius", props);



            seeThroughShaderGUI.showContentDissolveArea = FindProperty("_ShowContentDissolveArea", props);
            seeThroughShaderGUI.showContentInteractionOptionsArea = FindProperty("_ShowContentInteractionOptionsArea", props);
            seeThroughShaderGUI.showContentObstructionOptionsArea = FindProperty("_ShowContentObstructionOptionsArea", props);
            seeThroughShaderGUI.showContentAnimationArea = FindProperty("_ShowContentAnimationArea", props);
            seeThroughShaderGUI.showContentZoningArea = FindProperty("_ShowContentZoningArea", props);
            seeThroughShaderGUI.showContentReplacementOptionsArea = FindProperty("_ShowContentReplacementOptionsArea", props);
            seeThroughShaderGUI.showContentDebugArea = FindProperty("_ShowContentDebugArea", props);

            seeThroughShaderGUI.syncCullMode = FindProperty("_SyncCullMode", props);
        }

        public override void OnClosed(Material material)
        {

            base.OnClosed(material);
            EditorUtility.SetDirty(seeThroughShaderGUI.curveSO);
            AssetDatabase.SaveAssets();
        }
    }
}