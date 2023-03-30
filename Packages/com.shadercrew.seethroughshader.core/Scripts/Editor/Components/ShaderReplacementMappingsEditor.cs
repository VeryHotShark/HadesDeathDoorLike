using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;


namespace ShaderCrew.SeeThroughShader
{
    [CustomEditor(typeof(ShaderReplacementMappings))]
    public class ShaderReplacementMappingsEditor : Editor
    {

        private ShaderReplacementMappings stsCustomShaderRegistry;


        private SerializedProperty customShadersReplacementMappingProperty;

        private ReorderableList shaderMappingList;


        private SerializedProperty customShaderRegistrationListProperty;
        private ReorderableList customShaderRegistrationReordableList;

        List<string> stsShaderNameList;

        List<string> allShadersInProject;
        List<string> stsCustomShaderNameList;

        private SerializedProperty testFloatProperty;
        private SerializedProperty sMMMProperty;

        public override void OnInspectorGUI()
        {


            serializedObject.Update();


            //stsShaderNameList = GeneralUtils.STS_SHADER_LIST;
            //if (customShaderRegistrationListProperty != null)
            //{
            //    for (int i = 0; i < customShaderRegistrationListProperty.arraySize; i++)
            //    {
            //        SerializedProperty itemProperty = customShaderRegistrationListProperty.GetArrayElementAtIndex(i);
            //        if (itemProperty != null && itemProperty.objectReferenceValue != null && !stsShaderNameList.Contains(itemProperty.objectReferenceValue.name))
            //        {
            //            stsShaderNameList.Add(itemProperty.objectReferenceValue.name);
            //        }
            //    }
            //}

            //stsShaderNameList = new List<string>();
            //allShadersInProject = new List<string>();





            EditorUtils.LogoOnlyStart(Strings.SHADER_REPLACEMENT_MAPPINGS);


            GUIStyle replacementStyle = new GUIStyle();
            replacementStyle.normal.textColor = Color.white;
            replacementStyle.alignment = TextAnchor.MiddleCenter;
            replacementStyle.fontStyle = FontStyle.Bold;


            Rect rectt = EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUI.Box(rectt, GUIContent.none);

            EditorUtils.Header("Custom See-through Shaders", replacementStyle);
            //GUILayout.Label("Custom See-through Shaders", replacementStyle);
            //EditorUtils.DrawUILine();

            if (stsCustomShaderNameList.Count > 0)
            {
                foreach (string item in stsCustomShaderNameList)
                {
                    EditorGUILayout.LabelField(item);
                }
            }
            else
            {
                EditorGUILayout.LabelField("No custom See-through Shaders could be found!");
            }


            EditorGUILayout.EndVertical();
            EditorUtils.makeHorizontalSeparation();
            //customShaderRegistrationReordableList.DoLayoutList();

            if (shaderMappingList != null)
            {

                shaderMappingList.DoLayoutList();

            }


            EditorUtils.LogoOnlyEnd();
            serializedObject.ApplyModifiedProperties();
        }


        private void OnEnable()
        {
            allShadersInProject = new List<string>();
            stsShaderNameList = new List<string>();
            stsCustomShaderNameList = new List<string>();

            Shader[] allShaders = Resources.FindObjectsOfTypeAll<Shader>();
            foreach (Shader item in allShaders)
            {
                if (!item.name.StartsWith("Hidden") && !item.name.StartsWith("hidden") && item.name != "Master")
                {
                    if(!allShadersInProject.Contains(item.name))
                    {
                        allShadersInProject.Add(item.name);
                    }


                    if (item.FindPropertyIndex("_CurveObstructionDestroyRadius") != -1 && item.name != "Master")
                    {

                        if (!stsShaderNameList.Contains(item.name))
                        {
                            stsShaderNameList.Add(item.name);
                        }
                        if (!GeneralUtils.STS_SHADER_LIST.Contains(item.name))
                        {
                            if (!stsCustomShaderNameList.Contains(item.name))
                            {
                                stsCustomShaderNameList.Add(item.name);
                            }
                        }
                    }
                }
            }

            stsCustomShaderRegistry = (ShaderReplacementMappings)target;

            customShadersReplacementMappingProperty = serializedObject.FindProperty(nameof(stsCustomShaderRegistry.customShadersReplacementMappings));


            shaderMappingList = new ReorderableList(serializedObject, customShadersReplacementMappingProperty)
            {
                displayAdd = true,
                displayRemove = true,
                draggable = false,
                drawHeaderCallback = rect =>
                {
                    //GUI.color = Color.cyan;
                    //GUI.backgroundColor = Color.red;

                    GUIStyle style = new GUIStyle(EditorStyles.boldLabel);
                    style.normal.textColor = Color.white;
                    style.alignment = TextAnchor.MiddleCenter;
                    EditorGUI.LabelField(rect, "Shader Replacement Mappings", style);




                },
                drawElementCallback = (rect, index, focused, active) =>
                {

                    var element = customShadersReplacementMappingProperty.GetArrayElementAtIndex(index);

                    var backgroundColor = GUI.backgroundColor;
                    int count = 0;
                    for (int i = 0; i < customShadersReplacementMappingProperty.arraySize; i++)
                    {
                        //if (customShadersReplacementMappingProperty.GetArrayElementAtIndex(i).objectReferenceValue == element.objectReferenceValue)
                        if (customShadersReplacementMappingProperty.GetArrayElementAtIndex(i).FindPropertyRelative("initialShader").stringValue == element.FindPropertyRelative("initialShader").stringValue)
                        {
                            count++;
                        }
                    }

                    if (count > 1)
                    {
                        GUI.backgroundColor = Color.red;
                    }

                    //EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUI.GetPropertyHeight(element)), element, new GUIContent("Shader " + (index + 1) + ": "));
                    //EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUI.GetPropertyHeight(element)), element, new GUIContent("Shader " + (index + 1) + ": "));
                    //float unit = rect.width / 4;
                    //float x = rect.x;
                    //EditorGUI.LabelField(new Rect(x, rect.y, unit, EditorGUI.GetPropertyHeight(element)), "Replace ");
                    //x += unit;
                    //EditorGUI.PropertyField(new Rect(x, rect.y, unit * 3, EditorGUI.GetPropertyHeight(element)), element, GUIContent.none);

                    //x = rect.x;
                    //float y = rect.y + EditorGUI.GetPropertyHeight(element);
                    //EditorGUI.LabelField(new Rect(rect.x, y, unit, EditorGUI.GetPropertyHeight(element)), "With ");
                    //x += unit;
                    //EditorGUI.PropertyField(new Rect(x, y, unit * 3, EditorGUI.GetPropertyHeight(element)), element, GUIContent.none);
                    //// EditorGUI.PropertyField(new Rect(rect.width / 2 + 30 , rect.y, rect.width / 2, EditorGUI.GetPropertyHeight(element)), element, GUIContent.none);

                    float labelWidth = 70;
                    float shaderWidth = rect.width - labelWidth;
                    float x = rect.x;
                    float y = rect.y + 3;
                    EditorGUI.LabelField(new Rect(x, y, labelWidth, EditorGUI.GetPropertyHeight(element)), "Replace ");
                    x += labelWidth;

                    //EditorGUI.PropertyField(new Rect(x, y, shaderWidth, EditorGUI.GetPropertyHeight(element)), element, GUIContent.none);
                    int indexInitialShader = allShadersInProject.IndexOf(element.FindPropertyRelative("initialShader").stringValue);
                    if (indexInitialShader == -1)
                        indexInitialShader = 0;
                    indexInitialShader = EditorGUI.Popup(new Rect(x, y, shaderWidth, EditorGUI.GetPropertyHeight(element)), indexInitialShader, allShadersInProject.ToArray());
                    GUI.backgroundColor = backgroundColor;

                    y = y + EditorGUI.GetPropertyHeight(element);
                    EditorGUI.LabelField(new Rect(rect.x, y, labelWidth, EditorGUI.GetPropertyHeight(element)), "With ");


                    int indexReplacementShader = stsShaderNameList.IndexOf(element.FindPropertyRelative("replacementShader").stringValue);
                    if (indexReplacementShader == -1)
                        indexReplacementShader = 0;

                    indexReplacementShader = EditorGUI.Popup(new Rect(x, y, shaderWidth, EditorGUI.GetPropertyHeight(element)), indexReplacementShader, stsShaderNameList.ToArray());
                    //EditorGUI.PropertyField(new Rect(x, y, shaderWidth, EditorGUI.GetPropertyHeight(element)), element, GUIContent.none);
                    // EditorGUI.PropertyField(new Rect(rect.width / 2 + 30 , rect.y, rect.width / 2, EditorGUI.GetPropertyHeight(element)), element, GUIContent.none);


                    if (index != 0)
                    {
                        EditorGUI.DrawRect(new Rect(rect.x, rect.y, rect.width, 1), Color.black);

                    }



                    if (element != null)
                    {
                        if (element.FindPropertyRelative("initialShader") != null)
                        {
                            element.FindPropertyRelative("initialShader").stringValue = allShadersInProject[indexInitialShader];
                        }
                        if (element.FindPropertyRelative("replacementShader") != null)
                        {
                            element.FindPropertyRelative("replacementShader").stringValue = stsShaderNameList[indexReplacementShader];
                        }
                    }

                    if (count > 1)
                    {
                        y = y + EditorGUI.GetPropertyHeight(element);
                        EditorGUI.HelpBox(new Rect(rect.x, y, rect.width, EditorGUIUtility.singleLineHeight * 2), "Duplicate! Shader " + (index + 1) + " has to be unique! Many-To-One relationship required! The transpose is not allowed!", MessageType.Error);
                    }

                },

                elementHeightCallback = index =>
                {
                    var element = customShadersReplacementMappingProperty.GetArrayElementAtIndex(index);
                    int count = 0;
                    for (int i = 0; i < customShadersReplacementMappingProperty.arraySize; i++)
                    {
                        if (customShadersReplacementMappingProperty.GetArrayElementAtIndex(i).FindPropertyRelative("initialShader").stringValue 
                            == element.FindPropertyRelative("initialShader").stringValue)
                        {
                            count++;
                        }
                    }
                    var height = EditorGUI.GetPropertyHeight(element) * 2 + 6;

                    if (count > 1)
                    {
                        height += EditorGUIUtility.singleLineHeight * 2;
                    }

                    return height;
                },

                onAddCallback = list =>
                {
                    //customShadersReplacementMappingProperty.InsertArrayElementAtIndex(customShadersReplacementMappingProperty.arraySize);

                    list.serializedProperty.arraySize++;
                    //var newElement = list.serializedProperty.GetArrayElementAtIndex(list.serializedProperty.arraySize - 1);
                    //newElement.objectReferenceValue = null;

                },
                onRemoveCallback = list =>
                {
                    customShadersReplacementMappingProperty.DeleteArrayElementAtIndex(list.index);
                    //List<GameObject> temp = new List<GameObject>();
                    //for (int i = 0; i < list.serializedProperty.arraySize; i++)
                    //{
                    //    temp.Add((GameObject)list.serializedProperty.GetArrayElementAtIndex(i).objectReferenceValue);
                    //}
                    //list.serializedProperty.arraySize--;
                    //temp.RemoveAt(list.index);
                    //for (int i = 0; i < temp.Count; i++)
                    //{
                    //    var d = list.serializedProperty.GetArrayElementAtIndex(i);
                    //    d.objectReferenceValue = temp[i];
                    //}
                    //ReorderableList.defaultBehaviours.DoRemoveButton(list);

                }

            };

        }
    }
}