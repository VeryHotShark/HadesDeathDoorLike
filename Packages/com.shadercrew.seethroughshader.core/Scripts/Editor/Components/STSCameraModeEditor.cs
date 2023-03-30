using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace ShaderCrew.SeeThroughShader
{
    [CustomEditor(typeof(STSCameraMode))]
    public class STSCameraModeEditor : Editor
    {
        //private SerializedProperty showDebugRays;

        private void OnEnable()
        {
            //showDebugRays = serializedObject.FindProperty(nameof(BuildingAutoDetector.showDebugRays));

        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorUtils.usualStart("Camera Mode");
            var oriCol = EditorStyles.label.normal.textColor;
            EditorStyles.label.normal.textColor = Color.white;
            base.DrawDefaultInspector();
            EditorUtils.usualEnd();
            EditorStyles.label.normal.textColor = oriCol;
            serializedObject.ApplyModifiedProperties();
        }

    }
}