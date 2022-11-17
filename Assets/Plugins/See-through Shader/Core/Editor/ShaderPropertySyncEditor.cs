using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ShaderCrew.SeeThroughShader
{
    [CustomEditor(typeof(ShaderPropertySync))]
    public class ShaderPropertySyncEditor : Editor
    {
        private bool showDescription;
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            //SeeThroughShaderEditorUtils.usualStart("Shader Property Synchronizer");
            showDescription = EditorUtils.usualStartWithDescription(Strings.SHADER_PROPERTY_SYNC_TITLE,
                                                            Strings.SHADER_PROPERTY_SYNC_DESCRIPTION,
                                                            showDescription);
            var oriCol = EditorStyles.label.normal.textColor;
            EditorStyles.label.normal.textColor = Color.white;
            base.DrawDefaultInspector();
            EditorUtils.usualEnd();
            EditorStyles.label.normal.textColor = oriCol;
            serializedObject.ApplyModifiedProperties();
        }

    }
}
