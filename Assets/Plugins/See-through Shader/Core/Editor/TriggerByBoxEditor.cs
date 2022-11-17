using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ShaderCrew.SeeThroughShader
{
    [CustomEditor(typeof(TriggerByBox))]
    public class TriggerByBoxEditor : Editor
    {
        //private SerializedProperty showDebugRays;
        bool showDescription = false;
        private void OnEnable()
        {
            //showDebugRays = serializedObject.FindProperty(nameof(BuildingAutoDetector.showDebugRays));

        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            //SeeThroughShaderEditorUtils.usualStart("Trigger By Box");
            showDescription = EditorUtils.usualStartWithDescription(Strings.TRIGGER_BY_BOX_TITLE,
                                                                                    Strings.TRIGGER_BY_BOX_DESCRIPTION,
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
