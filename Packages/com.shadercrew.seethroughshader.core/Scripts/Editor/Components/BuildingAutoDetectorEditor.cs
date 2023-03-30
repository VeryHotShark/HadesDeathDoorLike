using ShaderCrew.SeeThroughShader;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ShaderCrew.SeeThroughShader
{
    [CustomEditor(typeof(BuildingAutoDetector))]
    public class BuildingAutoDetectorEditor : Editor
    {
        private SerializedProperty showDebugRays;
        private bool showDescription;
        private void OnEnable()
        {
            showDebugRays = serializedObject.FindProperty(nameof(BuildingAutoDetector.showDebugRays));

        }
        public override void OnInspectorGUI()
        {


            serializedObject.Update();
            var oriCol = EditorStyles.label.normal.textColor;
            //SeeThroughShaderEditorUtils.usualStart("Building Auto-Detector");
            showDescription = EditorUtils.usualStartWithDescription(Strings.BUILDING_AUTO_DETECTOR_TITLE,
                                                                    Strings.BUILDING_AUTO_DETECTOR_DESCRIPTION,
                                                                    showDescription);
            EditorStyles.label.normal.textColor = Color.white;
            EditorGUILayout.PropertyField(showDebugRays);
            EditorUtils.usualEnd();
            EditorStyles.label.normal.textColor = oriCol;
            serializedObject.ApplyModifiedProperties();
        }

    }
}
