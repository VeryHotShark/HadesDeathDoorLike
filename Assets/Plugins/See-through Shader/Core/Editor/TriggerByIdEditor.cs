using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ShaderCrew.SeeThroughShader
{
    [CustomEditor(typeof(TriggerById))]
    public class TriggerByIdEditor : Editor
    {
        private SerializedProperty showDebugRays;
        bool showDescription = false;
        private void OnEnable()
        {

        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            //SeeThroughShaderEditorUtils.usualStart("Trigger By Id");
            showDescription = EditorUtils.usualStartWithDescription(Strings.TRIGGER_BY_ID_TITLE,
                                                                                    Strings.TRIGGER_BY_ID_DESCRIPTION,
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