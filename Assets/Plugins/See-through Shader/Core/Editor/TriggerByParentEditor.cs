using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;

namespace ShaderCrew.SeeThroughShader
{
    [CustomEditor(typeof(TriggerByParent))]
    public class TriggerByParentEditor : Editor
    {
        private SerializedProperty isDedicatedEnterExitTrigger;
        private AnimBool isDedicatedEnterExitTriggerAnim;

        private SerializedProperty dedicatedEnterTrigger;
        private SerializedProperty dedicatedExitTrigger;

        private SerializedProperty dedicatedTriggerParent;

        public string[] options = new string[] { "Enter", "Exit" };
        public int index = 0;


        bool showDescription = false;

        private void OnEnable()
        {
            isDedicatedEnterExitTrigger = serializedObject.FindProperty(nameof(TriggerByParent.isDedicatedEnterExitTrigger));
            isDedicatedEnterExitTriggerAnim = new AnimBool(false);
            isDedicatedEnterExitTriggerAnim.valueChanged.AddListener(Repaint);

            dedicatedEnterTrigger = serializedObject.FindProperty(nameof(TriggerByParent.dedicatedEnterTrigger));
            dedicatedExitTrigger = serializedObject.FindProperty(nameof(TriggerByParent.dedicatedExitTrigger));

            if (dedicatedEnterTrigger.boolValue == true)
            {
                index = 0;
            }
            else if (dedicatedExitTrigger.boolValue == true)
            {
                index = 1;
            }
            else
            {
                index = 0;
            }

            dedicatedTriggerParent = serializedObject.FindProperty(nameof(TriggerByParent.dedicatedTriggerParent));
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            //SeeThroughShaderEditorUtils.usualStart("Trigger By Parent");
            showDescription = EditorUtils.usualStartWithDescription(Strings.TRIGGER_BY_PARENT_TITLE,
                                                                            Strings.TRIGGER_BY_PARENT_DESCRIPTION,
                                                                            showDescription);
            var oriCol = EditorStyles.label.normal.textColor;
            EditorStyles.label.normal.textColor = Color.white;

            //base.DrawDefaultInspector();

            GUIStyle optionStyle = new GUIStyle();
            optionStyle.normal.textColor = Color.white;
            optionStyle.fontStyle = FontStyle.Bold;

            isDedicatedEnterExitTrigger.boolValue = EditorGUILayout.ToggleLeft("Is this a dedicated Enter- or Exit Trigger?", isDedicatedEnterExitTrigger.boolValue, optionStyle);
            isDedicatedEnterExitTriggerAnim.target = isDedicatedEnterExitTrigger.boolValue;
            if (EditorGUILayout.BeginFadeGroup(isDedicatedEnterExitTriggerAnim.faded))
            {
                index = EditorGUILayout.Popup("Dedicated Trigger Type: ", index, options);

                string otherTrigger;
                if (index == 0)
                {
                    dedicatedEnterTrigger.boolValue = true;
                    dedicatedExitTrigger.boolValue = false;
                    otherTrigger = "Exit";
                }
                else
                {
                    dedicatedEnterTrigger.boolValue = false;
                    dedicatedExitTrigger.boolValue = true;
                    otherTrigger = "Enter";
                }
                EditorGUILayout.HelpBox("Don't forget to also add a " + otherTrigger + " trigger", MessageType.Info);

                EditorGUILayout.PropertyField(dedicatedTriggerParent);
                if (dedicatedTriggerParent.objectReferenceValue == null)
                {
                    EditorGUILayout.HelpBox("You can NOT use dedicated triggers without a dedicated trigger parent. This won't work! Please add a Dedicated Trigger Parent", MessageType.Error);

                }
            }
            else
            {
            }
            EditorGUILayout.EndFadeGroup();

            if (isDedicatedEnterExitTrigger.boolValue == false)
            {
                dedicatedEnterTrigger.boolValue = false;
                dedicatedExitTrigger.boolValue = false;
                EditorGUILayout.HelpBox("The collider of this GameObject is both the Enter and Exit trigger", MessageType.Info);

            }
            EditorUtils.usualEnd();
            EditorStyles.label.normal.textColor = oriCol;

            serializedObject.ApplyModifiedProperties();
        }

    }
}