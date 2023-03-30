using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ShaderCrew.SeeThroughShader
{
    [CustomEditor(typeof(ToggleByUI))]
    public class ToggleByUIEditor : Editor
    {

        private ToggleByUI toggleByUI;
        bool showDescription = false;
        bool isTriggered = false;
        private SerializedProperty button;
        private void OnEnable()
        {
            toggleByUI = target as ToggleByUI;
            button = serializedObject.FindProperty(nameof(ToggleByUI.button));

        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            //SeeThroughShaderEditorUtils.usualStart("Trigger By Parent");
            showDescription = EditorUtils.usualStartWithDescription(Strings.TOGGLE_BY_UI_TITLE,
                                                                            Strings.TOGGLE_BY_UI_DESCRIPTION,
                                                                            showDescription);
            var oriCol = EditorStyles.label.normal.textColor;
            EditorStyles.label.normal.textColor = Color.white;

            
            EditorGUILayout.PropertyField(button);
            string buttonText = "";

            if (!isTriggered)
            {
                buttonText = "Activate See-through Shader Effect";
            }
            else
            {
                buttonText = "Deactivate See-through Shader Effect";
            }
            if(toggleByUI != null)
            {
                if (GUILayout.Button(buttonText))
                {
                    isTriggered = !isTriggered;
                    if (isTriggered)
                    {
                        toggleByUI.activateSTSEffect();

                    }
                    else
                    {
                        toggleByUI.dectivateSTSEffect();
                    }
                }
            }

            //base.DrawDefaultInspector();

            //GUIStyle optionStyle = new GUIStyle();
            //optionStyle.normal.textColor = Color.white;
            //optionStyle.fontStyle = FontStyle.Bold;

            //isDedicatedEnterExitTrigger.boolValue = EditorGUILayout.ToggleLeft("Is this a dedicated Enter- or Exit Trigger?", isDedicatedEnterExitTrigger.boolValue, optionStyle);
            //isDedicatedEnterExitTriggerAnim.target = isDedicatedEnterExitTrigger.boolValue;
            //if (EditorGUILayout.BeginFadeGroup(isDedicatedEnterExitTriggerAnim.faded))
            //{
            //    index = EditorGUILayout.Popup("Dedicated Trigger Type: ", index, options);

            //    string otherTrigger;
            //    if (index == 0)
            //    {
            //        dedicatedEnterTrigger.boolValue = true;
            //        dedicatedExitTrigger.boolValue = false;
            //        otherTrigger = "Exit";
            //    }
            //    else
            //    {
            //        dedicatedEnterTrigger.boolValue = false;
            //        dedicatedExitTrigger.boolValue = true;
            //        otherTrigger = "Enter";
            //    }
            //    EditorGUILayout.HelpBox("Don't forget to also add a " + otherTrigger + " trigger", MessageType.Info);

            //    EditorGUILayout.PropertyField(dedicatedTriggerParent);
            //    if (dedicatedTriggerParent.objectReferenceValue == null)
            //    {
            //        EditorGUILayout.HelpBox("You can NOT use dedicated triggers without a dedicated trigger parent. This won't work! Please add a Dedicated Trigger Parent", MessageType.Error);

            //    }
            //}
            //else
            //{
            //}
            //EditorGUILayout.EndFadeGroup();

            //if (isDedicatedEnterExitTrigger.boolValue == false)
            //{
            //    dedicatedEnterTrigger.boolValue = false;
            //    dedicatedExitTrigger.boolValue = false;
            //    EditorGUILayout.HelpBox("The collider of this GameObject is both the Enter and Exit trigger", MessageType.Info);

            //}


            EditorUtils.usualEnd();
            EditorStyles.label.normal.textColor = oriCol;

            serializedObject.ApplyModifiedProperties();
        }

    }
}