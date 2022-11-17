#if USING_BETTERSHADERS

using JBooth.BetterShaders;

using UnityEditor;
using UnityEngine;

namespace ShaderCrew.SeeThroughShader
{
    public class SeeThroughShaderStackableEditor : SubShaderMaterialEditor
    {


        SeeThroughShaderGUI seeThroughShaderGUI;

        Color oriCol;

        Color textColor;
        Color originalColor;
        bool m_FirstTimeApply = true;
        public override void OnGUI(MaterialEditor materialEditor,
             ShaderGUI shaderGUI,
             MaterialProperty[] props,
             Material mat)
        {

            if (seeThroughShaderGUI == null)
            {
                seeThroughShaderGUI = new SeeThroughShaderGUI();
            }
            FindProperties(props);

            seeThroughShaderGUI.m_MaterialEditor = materialEditor;
            Material material = materialEditor.target as Material;

            if (m_FirstTimeApply)
            {
                seeThroughShaderGUI.DoSetup(materialEditor);
                originalColor = EditorStyles.label.normal.textColor;


                if (EditorGUIUtility.isProSkin)
                {
                    textColor = Color.white;
                    originalColor = EditorStyles.label.normal.textColor;
                }
                else
                {
                    //textColor = EditorStyles.label.normal.textColor;
                    textColor = Color.black;
                    originalColor = new Color(0.9f, 0.9f, 0.9f, 1);
                }
                m_FirstTimeApply = false;

            }


            string name = seeThroughShaderGUI.GetSTSMaterialType();


            EditorUtils.usualStart(name);


            EditorStyles.label.normal.textColor = textColor;



            seeThroughShaderGUI.STSShaderPropertiesGUI(material);

            EditorUtils.makeHorizontalSeparation();




            EditorUtils.usualEnd();
            EditorStyles.label.normal.textColor = originalColor;

        }



        public void FindProperties(MaterialProperty[] props)
        {
            seeThroughShaderGUI.isReferenceMaterialMat = FindProp("_isReferenceMaterial", props);


            // See-through Shader
            seeThroughShaderGUI.dissolveMap = FindProp("_DissolveTex", props);
            seeThroughShaderGUI.dissolveColor = FindProp("_DissolveColor", props);
            seeThroughShaderGUI.dissolveSize = FindProp("_UVs", props);
            seeThroughShaderGUI.dissolveColorSaturation = FindProp("_DissolveColorSaturation", props);

            seeThroughShaderGUI.dissolveEmmission = FindProp("_DissolveEmission", props);
            seeThroughShaderGUI.dissolveEmmissionBooster = FindProp("_DissolveEmissionBooster", props);
            seeThroughShaderGUI.dissolveTexturedEmissionEdge = FindProp("_TexturedEmissionEdge", props);
            seeThroughShaderGUI.dissolveTexturedEmissionEdgeStrength = FindProp("_TexturedEmissionEdgeStrength", props);

            seeThroughShaderGUI.dissolveClippedShadowsEnabled = FindProp("_hasClippedShadows", props);


            seeThroughShaderGUI.dissolveTextureAnimationEnabled = FindProp("_AnimationEnabled", props);
            seeThroughShaderGUI.dissolveTextureAnimationSpeed = FindProp("_AnimationSpeed", props);
            seeThroughShaderGUI.dissolveTransitionDuration = FindProp("_TransitionDuration", props);


            seeThroughShaderGUI.interactionMode = FindProp("_InteractionMode", props);
            //centerPosition = FindProperty("_CenterPosition", props);

            seeThroughShaderGUI.obstructionMode = FindProp("_Obstruction", props);
            seeThroughShaderGUI.angleStrength = FindProp("_AngleStrength", props);
            seeThroughShaderGUI.coneStrength = FindProp("_ConeStrength", props);
            seeThroughShaderGUI.coneObstructionDestroyRadius = FindProp("_ConeObstructionDestroyRadius", props);

            seeThroughShaderGUI.cylinderStrength = FindProp("_CylinderStrength", props);
            seeThroughShaderGUI.cylinderObstructionDestroyRadius = FindProp("_CylinderObstructionDestroyRadius", props);

            seeThroughShaderGUI.circleStrength = FindProp("_CircleStrength", props);
            seeThroughShaderGUI.circleObstructionDestroyRadius = FindProp("_CircleObstructionDestroyRadius", props);

            seeThroughShaderGUI.curveStrength = FindProp("_CurveStrength", props);
            seeThroughShaderGUI.curveObstructionDestroyRadius = FindProp("_CurveObstructionDestroyRadius", props);

            seeThroughShaderGUI.dissolveObstructionCurve = FindProp("_ObstructionCurve", props);

            seeThroughShaderGUI.dissolveFallOff = FindProp("_DissolveFallOff", props);
            seeThroughShaderGUI.dissolveMask = FindProp("_DissolveMask", props);
            seeThroughShaderGUI.dissolveMaskEnabled = FindProp("_DissolveMaskEnabled", props);

            seeThroughShaderGUI.intrinsicDissolveStrength = FindProp("_IntrinsicDissolveStrength", props);


            seeThroughShaderGUI.ceilingEnabled = FindProp("_Ceiling", props);
            seeThroughShaderGUI.ceilingMode = FindProp("_CeilingMode", props);
            seeThroughShaderGUI.ceilingBlendMode = FindProp("_CeilingBlendMode", props);
            seeThroughShaderGUI.ceilingY = FindProp("_CeilingY", props);
            seeThroughShaderGUI.ceilingPlayerYOffset = FindProp("_CeilingPlayerYOffset", props);
            seeThroughShaderGUI.ceilingYGradientLength = FindProp("_CeilingYGradientLength", props);


            seeThroughShaderGUI.isometricExlusionEnabled = FindProp("_IsometricExclusion", props);
            seeThroughShaderGUI.isometricExclusionDistance = FindProp("_IsometricExclusionDistance", props);
            seeThroughShaderGUI.isometricExclusionGradientLength = FindProp("_IsometricExclusionGradientLength", props);

            seeThroughShaderGUI.floorEnabled = FindProp("_Floor", props);
            seeThroughShaderGUI.floorMode = FindProp("_FloorMode", props);
            seeThroughShaderGUI.floorY = FindProp("_FloorY", props);
            seeThroughShaderGUI.playerPosYOffset = FindProp("_PlayerPosYOffset", props);
            seeThroughShaderGUI.floorYTextureGradientLength = FindProp("_FloorYTextureGradientLength", props);



            seeThroughShaderGUI.zoningEnabled = FindProp("_Zoning", props);
            seeThroughShaderGUI.zoningMode = FindProp("_ZoningMode", props);
            seeThroughShaderGUI.zoningEdgeGradientLength = FindProp("_ZoningEdgeGradientLength", props);
            seeThroughShaderGUI.zoningIsRevealable = FindProp("_IsZoningRevealable", props);

            seeThroughShaderGUI.zoningSyncZonesWithFloorY = FindProp("_SyncZonesWithFloorY", props);
            seeThroughShaderGUI.zoningSyncZonesFloorYOffset = FindProp("_SyncZonesFloorYOffset", props);


            seeThroughShaderGUI.debugModeEnabled = FindProp("_PreviewMode", props);
            seeThroughShaderGUI.debugModeIndicatorLineThickness = FindProp("_PreviewIndicatorLineThickness", props);

            seeThroughShaderGUI.isReplacementShader = FindProp("_IsReplacementShader", props);

            seeThroughShaderGUI.defaultEffectRadius = FindProp("_DefaultEffectRadius", props);



            seeThroughShaderGUI.showContentDissolveArea = FindProp("_ShowContentDissolveArea", props);
            seeThroughShaderGUI.showContentInteractionOptionsArea = FindProp("_ShowContentInteractionOptionsArea", props);
            seeThroughShaderGUI.showContentObstructionOptionsArea = FindProp("_ShowContentObstructionOptionsArea", props);
            seeThroughShaderGUI.showContentAnimationArea = FindProp("_ShowContentAnimationArea", props);
            seeThroughShaderGUI.showContentZoningArea = FindProp("_ShowContentZoningArea", props);
            seeThroughShaderGUI.showContentReplacementOptionsArea = FindProp("_ShowContentReplacementOptionsArea", props);
            seeThroughShaderGUI.showContentDebugArea = FindProp("_ShowContentDebugArea", props);

            seeThroughShaderGUI.syncCullMode = FindProp("_SyncCullMode", props);

        }

        // TODO MISSING
        //public override void OnClosed(Material material)
        //{

        //    base.OnClosed(material);
        //    EditorUtility.SetDirty(seeThroughShaderEditorGUI.curveSO);
        //    AssetDatabase.SaveAssets();
        //}
    }
}
#endif