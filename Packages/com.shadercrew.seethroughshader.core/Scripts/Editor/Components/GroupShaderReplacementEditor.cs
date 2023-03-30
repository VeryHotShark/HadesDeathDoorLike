using ShaderCrew.SeeThroughShader;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static ShaderCrew.SeeThroughShader.GeneralUtils;
using static ShaderCrew.SeeThroughShader.GroupReplacementAndSyncBaseAbstract;
using static ShaderCrew.SeeThroughShader.GroupShaderReplacement;

namespace ShaderCrew.SeeThroughShader
{
    [CustomEditor(typeof(GroupShaderReplacement))]
    public class GroupShaderReplacementEditor : ReplacementAndSyncBaseEditorAbstract
    {
        //private GroupShaderReplacement seeThroughShaderGroupReplacement;
        //private SerializedProperty seeThroughShader;
        //private SerializedProperty referenceMaterial;
        //private SerializedProperty parentTransform;
        //private SerializedProperty materialExemptions;
        //private SerializedProperty layerMaskToAdd;

        //private SerializedProperty replacementGroupType;
        //private SerializedProperty triggerID;
        //private SerializedProperty triggerBox;

        //private bool showDescription;

        public override string Title => Strings.GROUP_SHADER_REPLACEMENT_TITLE;

        public override string Description => Strings.GROUP_SHADER_REPLACEMENT_DESCRIPTION;

        //private void OnEnable()
        //{
        //    seeThroughShaderGroupReplacement = (GroupShaderReplacement)target;

        //    seeThroughShader = serializedObject.FindProperty(nameof(GroupShaderReplacement.seeThroughShader));
        //    referenceMaterial = serializedObject.FindProperty(nameof(GroupShaderReplacement.referenceMaterial));

        //    parentTransform = serializedObject.FindProperty(nameof(GroupShaderReplacement.parentTransform));

        //    materialExemptions = serializedObject.FindProperty(nameof(GroupShaderReplacement.materialExemptions));
        //    layerMaskToAdd = serializedObject.FindProperty(nameof(GroupShaderReplacement.layerMaskToAdd));

        //    replacementGroupType = serializedObject.FindProperty(nameof(GroupShaderReplacement.replacementGroupType));
        //    triggerID = serializedObject.FindProperty(nameof(GroupShaderReplacement.triggerID));
        //    triggerBox = serializedObject.FindProperty(nameof(GroupShaderReplacement.triggerBox));

        //}
        //public override void OnInspectorGUI()
        //{
        //    serializedObject.Update();
        //    //SeeThroughShaderEditorUtils.usualStart("Shader Replacement By Group");
        //    showDescription = EditorUtils.usualStartWithDescription(Strings.GROUP_SHADER_REPLACEMENT_TITLE,
        //                                                    Strings.GROUP_SHADER_REPLACEMENT_DESCRIPTION,
        //                                                    showDescription);
        //    var oriCol = EditorStyles.label.normal.textColor;
        //    EditorStyles.label.normal.textColor = Color.white;

        //    GUIStyle replacementStyle = new GUIStyle();
        //    replacementStyle.normal.textColor = Color.white;
        //    replacementStyle.alignment = TextAnchor.MiddleCenter;
        //    replacementStyle.fontStyle = FontStyle.Bold;

        //    //GUILayout.Label("Choose The Shader Version", replacementStyle);
        //    //SeeThroughShaderEditorUtils.DrawUILine();
        //    UnityVersionRenderPipelineShaderInfo unityVersionRenderPipelineShaderInfo = getUnityVersionAndRenderPipelineCorrectedShaderString();
        //    //EditorGUILayout.PropertyField(seeThroughShader);
        //    //if (seeThroughShader.objectReferenceValue == null)
        //    //{
        //    //    EditorGUILayout.HelpBox("If you don't choose a shader, the first shader found with the name '" +
        //    //        unityVersionRenderPipelineShaderInfo.versionAndRPCorrectedShader +
        //    //        "' will be used. This is the recommended shader as it seems like you are using the " +
        //    //        unityVersionRenderPipelineShaderInfo.renderPipeline + " with Unity " + unityVersionRenderPipelineShaderInfo.unityVersion, MessageType.Info);

        //    //}
        //    //else
        //    //{
        //    //    if (!((Shader)seeThroughShader.objectReferenceValue).name.Equals(unityVersionRenderPipelineShaderInfo.versionAndRPCorrectedShader))
        //    //    {
        //    //        EditorGUILayout.HelpBox("It seems like you are using the " + unityVersionRenderPipelineShaderInfo.renderPipeline +
        //    //            " with Unity " + unityVersionRenderPipelineShaderInfo.unityVersion + ". This would make '" +
        //    //            unityVersionRenderPipelineShaderInfo.versionAndRPCorrectedShader + "' the recommended choice!", MessageType.Warning);

        //    //    }

        //    //}

        //    //SeeThroughShaderEditorUtils.makeHorizontalSeparation();

        //    GUILayout.Label("Choose The Reference Material", replacementStyle);
        //    EditorUtils.DrawUILine();
        //    EditorGUILayout.PropertyField(referenceMaterial);
        //    if (referenceMaterial.objectReferenceValue == null)
        //    {
        //        EditorGUILayout.HelpBox("You have to set a reference material, otherwise the 'See-through Shader' will be limited to the default shader settings and so won't work as expected!", MessageType.Error);

        //    }
        //    else if (!((Material)referenceMaterial.objectReferenceValue).HasProperty(SeeThroughShaderConstants.STS_SHADER_IDENTIFIER_PROPERTY))
        //    {
        //        EditorGUILayout.HelpBox("The reference material is NOT a 'See-through Shader' material! Replacement will NOT work with a NON-STS shader!", MessageType.Error);

        //    }
        //    else if (((Material)referenceMaterial.objectReferenceValue).GetFloat("_IsReplacementShader") == 1)
        //    {
        //        EditorGUILayout.HelpBox("\"Global Replacement\" is enabled for the selected reference material. Please disable, otherwise you will get an incorrect behaviour!", MessageType.Error);
        //        if (GUILayout.Button("Disable \"Global Replacement\""))
        //        {
        //            ((Material)referenceMaterial.objectReferenceValue).SetFloat("_IsReplacementShader", 0);
        //            ((Material)referenceMaterial.objectReferenceValue).DisableKeyword("_REPLACEMENT");
        //        }
        //    }


        //    EditorUtils.makeHorizontalSeparation();
        //    GUILayout.Label("Choose The Group Type", replacementStyle);
        //    EditorUtils.DrawUILine();
        //    EditorGUILayout.PropertyField(replacementGroupType);
        //    switch ((ReplacementGroupType)replacementGroupType.enumValueIndex)
        //    {
        //        case ReplacementGroupType.Parent:
        //            {
        //                EditorGUILayout.PropertyField(parentTransform);
        //                if (parentTransform.objectReferenceValue == null)
        //                {
        //                    EditorGUILayout.HelpBox("You didn't choose a parent transform, that means that this GameObject, '" +
        //                        seeThroughShaderGroupReplacement.name + "' will be the parent. Every child  of it that isn't in" +
        //                        " the exemption list will get the See-through Shader assigned to its material during runtime.", MessageType.Info);
        //                }
        //                break;
        //            }
        //        case ReplacementGroupType.Box:
        //            {
        //                EditorGUILayout.PropertyField(triggerBox);
        //                if (triggerBox.objectReferenceValue == null)
        //                {
        //                    EditorGUILayout.HelpBox("You didn't choose a trigger box, that means that the Shader Replacement by Group won't work!.", MessageType.Warning);
        //                }
        //                break;
        //            }
        //        case ReplacementGroupType.Id:
        //            {
        //                EditorGUILayout.PropertyField(triggerID);
        //                if (string.IsNullOrEmpty(triggerID.stringValue))
        //                {
        //                    EditorGUILayout.HelpBox("You didn't choose an Id, that means that the Shader Replacement by Group won't work!.", MessageType.Warning);
        //                }
        //                break;
        //            }
        //        default: break;
        //    }


        //    //EditorUtils.makeHorizontalSeparation();

        //    //GUILayout.Label("Choose The Parent", replacementStyle);
        //    //EditorUtils.DrawUILine();
        //    //EditorGUILayout.PropertyField(parentTransform);
        //    //if (parentTransform.objectReferenceValue == null)
        //    //{
        //    //    EditorGUILayout.HelpBox("You didn't choose a parent transform, that means that this GameObject, '" +
        //    //        seeThroughShaderGroupReplacement.name + "' will be the parent. Every child  of it that isn't in" +
        //    //        " the exemption list will get the See-through Shader assigned to its material during runtime.", MessageType.Info);
        //    //}

        //    EditorUtils.makeHorizontalSeparation();

        //    GUILayout.Label("Choose Which Layers Are Affected", replacementStyle);
        //    EditorUtils.DrawUILine();
        //    EditorGUILayout.PropertyField(layerMaskToAdd);
        //    if (layerMaskToAdd.intValue == 0)
        //    {
        //        EditorGUILayout.HelpBox("You didn't select any layer! Shader assignment won't happen!", MessageType.Error);
        //    }
        //    EditorUtils.makeHorizontalSeparation();

        //    GUILayout.Label("Choose The Exemptions", replacementStyle);
        //    EditorUtils.DrawUILine();
        //    EditorGUILayout.PropertyField(materialExemptions);

        //    EditorUtils.makeHorizontalSeparation();

        //    EditorUtils.usualEnd();
        //    EditorStyles.label.normal.textColor = oriCol;
        //    serializedObject.ApplyModifiedProperties();
        //}
    }
}