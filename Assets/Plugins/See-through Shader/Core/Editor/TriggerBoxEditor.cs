using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ShaderCrew.SeeThroughShader
{
    [CustomEditor(typeof(TriggerBox))]
    public class TriggerBoxEditor : Editor
    {
        private SerializedProperty showDebugRays;
        private bool showDescription;
        private void OnEnable()
        {

        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            //SeeThroughShaderEditorUtils.usualStart("Trigger Box");
            showDescription = EditorUtils.usualStartWithDescription(Strings.TRIGGER_BOX_TITLE,
                                                    Strings.TRIGGER_BOX_DESCRIPTION,
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