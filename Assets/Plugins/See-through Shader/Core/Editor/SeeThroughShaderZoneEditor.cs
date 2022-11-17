using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ShaderCrew.SeeThroughShader
{
    [CustomEditor(typeof(SeeThroughShaderZone))]
    public class SeeThroughShaderZoneEditor : Editor
    {
        private SeeThroughShaderZone seeThroughShaderZone;
        private bool showDescription;

        private SerializedProperty transitionDuration;

        private void OnEnable()
        {
            seeThroughShaderZone = target as SeeThroughShaderZone;
            transitionDuration = serializedObject.FindProperty(nameof(SeeThroughShaderZone.transitionDuration));

        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            //SeeThroughShaderEditorUtils.usualStart("Trigger Box");
            showDescription = EditorUtils.usualStartWithDescription(Strings.STS_ZONE,
                                                    Strings.STS_ZONE_DESCRIPTION,
                                                    showDescription);
            var oriCol = EditorStyles.label.normal.textColor;
            EditorStyles.label.normal.textColor = Color.white;
            //base.DrawDefaultInspector();
            EditorGUILayout.PropertyField(transitionDuration);

            if (seeThroughShaderZone != null)
            {
                string buttonText;

                if (!seeThroughShaderZone.isActivated)
                {
                    buttonText = "Activate Zone";
                }
                else
                {
                    buttonText = "Deactivate Zone";
                }
                if (GUILayout.Button(buttonText))
                {
                    if (seeThroughShaderZone.isActivated)
                    {
                        seeThroughShaderZone.toggleZoneActivation();
                    }
                    else
                    {
                        seeThroughShaderZone.toggleZoneActivation();
                    }
                }
            }

            EditorUtils.usualEnd();
            EditorStyles.label.normal.textColor = oriCol;
            serializedObject.ApplyModifiedProperties();
        }

    }
}