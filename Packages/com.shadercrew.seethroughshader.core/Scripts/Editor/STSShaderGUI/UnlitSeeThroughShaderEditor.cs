using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static ShaderCrew.SeeThroughShader.GeneralUtils;

namespace ShaderCrew.SeeThroughShader
{
    public class UnlitSeeThroughShaderEditor : SeeThroughShaderEditorAbstract
    {
        public MaterialProperty albedoMap = null;
        public MaterialProperty albedoColor = null;



        public override void DoGUI(Material material, MaterialEditor materialEditor, MaterialProperty[] properties)
        {
            Rect rectt = EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUI.Box(rectt, GUIContent.none);

            if (!seeThroughShaderGUI.isReferenceMaterial)
            {
                Shader currentShader = material.shader;


                GUIStyle replacementStyle = new GUIStyle();
                replacementStyle.normal.textColor = textColor;
                replacementStyle.alignment = TextAnchor.MiddleCenter;
                replacementStyle.fontStyle = FontStyle.Bold;
                replacementStyle.fontSize = 14;
                replacementStyle.richText = true;

                if (rp == RenderPipeline.BiRP)
                {
                    EditorGUILayout.Space();
                    if (currentShader.name == "SeeThroughShader/BiRP/Unlit/Color")
                    {
                        GUILayout.Label("Unlit/Color Shader : <i>Properties</i>", replacementStyle);
                        EditorGUILayout.Space();

                        EditorUtils.DrawUILine();

                        float oriLabelWidth = EditorGUIUtility.labelWidth;
                        EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth - 114;
                        m_MaterialEditor.ColorProperty(albedoColor, albedoColor.displayName);
                        EditorGUIUtility.labelWidth = oriLabelWidth;

                    }
                    else if (currentShader.name == "SeeThroughShader/BiRP/Unlit/Texture")
                    {
                        GUILayout.Label("Unlit/Texture Shader : <i>Properties</i>", replacementStyle);
                        EditorGUILayout.Space();
                        EditorUtils.DrawUILine();


                        //float oriLabelWidth = EditorGUIUtility.labelWidth;
                        //EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth - 94;
                        m_MaterialEditor.ShaderProperty(albedoMap, albedoMap.displayName);
                        //EditorGUIUtility.labelWidth = oriLabelWidth;
                    }
                }

            }
            EditorGUILayout.EndVertical();
        }

        public override void InitializeGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
        {
            Shader currentShader = (materialEditor.target as Material).shader;

            if (rp == RenderPipeline.BiRP)
            {
                if (currentShader.name == "SeeThroughShader/BiRP/Unlit/Color")
                {
                    albedoColor = FindProperty("_Color", properties);

                }
                else if (currentShader.name == "SeeThroughShader/BiRP/Unlit/Texture")
                {
                    albedoMap = FindProperty("_MainTex", properties);
                }
            }
        }


    }
}