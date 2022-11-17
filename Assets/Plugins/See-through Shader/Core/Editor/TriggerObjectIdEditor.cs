using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ShaderCrew.SeeThroughShader
{
    [CustomEditor(typeof(TriggerObjectId))]
    public class TriggerObjectIdEditor : Editor
    {
        private SerializedProperty showDebugRays;
        private bool showDescription;
        private void OnEnable()
        {

        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            //SeeThroughShaderEditorUtils.usualStart("Object Id For Trigger by Id");
            showDescription = EditorUtils.usualStartWithDescription(Strings.TRIGGER_OBJECT_ID_TITLE,
                                                    Strings.TRIGGER_OBJECT_ID_DESCRIPTION,
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
