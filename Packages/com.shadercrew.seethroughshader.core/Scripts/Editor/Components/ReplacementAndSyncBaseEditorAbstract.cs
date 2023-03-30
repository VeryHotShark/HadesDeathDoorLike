using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using static ShaderCrew.SeeThroughShader.GroupReplacementAndSyncBaseAbstract;

namespace ShaderCrew.SeeThroughShader
{
    public abstract class ReplacementAndSyncBaseEditorAbstract : Editor
    {

        private bool showDescription;

        private SerializedProperty seeThroughShader;
        private SerializedProperty referenceMaterial;
        private SerializedProperty parentTransform;
        private SerializedProperty materialExemptions;
        private SerializedProperty layerMaskToAdd;
        private SerializedProperty keepMaterialsInSyncWithReference;

        private SerializedProperty replacementGroupType;
        private SerializedProperty triggerID;
        private SerializedProperty triggerBox;

        private SerializedProperty materialList;
        private SerializedProperty gameObjectList;


        private ReorderableList gameObjectsReorderableList;
        private ReorderableList materialsReorderableList;


        private void OnEnable()
        {
            
            //shaderPropertySync = (ShaderPropertySync)target;

            //seeThroughShader = serializedObject.FindProperty(nameof(GroupShaderReplacement.seeThroughShader));
            referenceMaterial = serializedObject.FindProperty(nameof(GroupReplacementAndSyncBaseAbstract.referenceMaterial));

            parentTransform = serializedObject.FindProperty(nameof(GroupReplacementAndSyncBaseAbstract.parentTransform));

            materialExemptions = serializedObject.FindProperty(nameof(GroupReplacementAndSyncBaseAbstract.materialExemptions));
            layerMaskToAdd = serializedObject.FindProperty(nameof(GroupReplacementAndSyncBaseAbstract.layerMaskToAdd));

            keepMaterialsInSyncWithReference = serializedObject.FindProperty(nameof(GroupReplacementAndSyncBaseAbstract.keepMaterialsInSyncWithReference));

            replacementGroupType = serializedObject.FindProperty(nameof(GroupReplacementAndSyncBaseAbstract.replacementGroupType));
            triggerID = serializedObject.FindProperty(nameof(GroupReplacementAndSyncBaseAbstract.triggerID));
            triggerBox = serializedObject.FindProperty(nameof(GroupReplacementAndSyncBaseAbstract.triggerBox));

            materialList = serializedObject.FindProperty(nameof(GroupReplacementAndSyncBaseAbstract.materialList));
            gameObjectList = serializedObject.FindProperty(nameof(GroupReplacementAndSyncBaseAbstract.gameObjectList));

            gameObjectsReorderableList = new ReorderableList(serializedObject, gameObjectList)
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
                    EditorGUI.LabelField(rect, "List of GameObjects", style);

                },
                drawElementCallback = (rect, index, focused, active) =>
                {

                    var element = gameObjectList.GetArrayElementAtIndex(index);

                    var backgroundColor = GUI.backgroundColor;
                    int count = 0;
                    for (int i = 0; i < gameObjectList.arraySize; i++)
                    {
                        if (gameObjectList.GetArrayElementAtIndex(i).objectReferenceValue == element.objectReferenceValue)
                        {
                            count++;
                        }
                    }
                    if (element.objectReferenceValue == null || count > 1)
                    {
                        GUI.backgroundColor = Color.red;
                    }
                    else
                    {
                        GUI.backgroundColor = backgroundColor;
                    }




                    //GUI.color = Color.cyan;
                    GUIStyle insertVarNameHere = new GUIStyle();
                    insertVarNameHere.fontStyle = FontStyle.Bold;
                    EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUI.GetPropertyHeight(element)), element, new GUIContent("GameObject " + (index + 1) + ": "));

                    GUI.backgroundColor = backgroundColor;


                    if (element.objectReferenceValue == null)
                    {
                        rect.y += EditorGUI.GetPropertyHeight(element);
                        EditorGUI.HelpBox(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight * 2), "GameObject  " + (index + 1) + "  may not be empty!", MessageType.Error);
                    }
                    else if (count > 1)
                    {
                        rect.y += EditorGUI.GetPropertyHeight(element);
                        EditorGUI.HelpBox(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight * 2), "Duplicate! GameObject  " + (index + 1) + "  has to be unique!", MessageType.Error);
                    }

                },

                elementHeightCallback = index =>
                {
                    var element = gameObjectList.GetArrayElementAtIndex(index);
                    int count = 0;
                    for (int i = 0; i < gameObjectList.arraySize; i++)
                    {
                        if (gameObjectList.GetArrayElementAtIndex(i).objectReferenceValue == element.objectReferenceValue)
                        {
                            count++;
                        }
                    }
                    var height = EditorGUI.GetPropertyHeight(element);
                    if (element.objectReferenceValue == null || count > 1)
                    {
                        height += EditorGUIUtility.singleLineHeight * 2;
                    }
                    return height;
                },

                onAddCallback = list =>
                {
                    list.serializedProperty.arraySize++;

                    var newElement = list.serializedProperty.GetArrayElementAtIndex(list.serializedProperty.arraySize - 1);
                    newElement.objectReferenceValue = null;
                },
                onRemoveCallback = list =>
                {
                    List<GameObject> temp = new List<GameObject>();
                    for (int i = 0; i < list.serializedProperty.arraySize; i++)
                    {
                        temp.Add((GameObject)list.serializedProperty.GetArrayElementAtIndex(i).objectReferenceValue);
                    }
                    list.serializedProperty.arraySize--;
                    temp.RemoveAt(list.index);
                    for (int i = 0; i < temp.Count; i++)
                    {
                        var d = list.serializedProperty.GetArrayElementAtIndex(i);
                        d.objectReferenceValue = temp[i];
                    }
                    //ReorderableList.defaultBehaviours.DoRemoveButton(list);

                }
            };


            materialsReorderableList = new ReorderableList(serializedObject, materialList)
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
                    EditorGUI.LabelField(rect, "List of Materials", style);

                },
                drawElementCallback = (rect, index, focused, active) =>
                {

                    var element = materialList.GetArrayElementAtIndex(index);

                    var backgroundColor = GUI.backgroundColor;
                    int count = 0;
                    for (int i = 0; i < materialList.arraySize; i++)
                    {
                        if (materialList.GetArrayElementAtIndex(i).objectReferenceValue == element.objectReferenceValue)
                        {
                            count++;
                        }
                    }
                    if (element.objectReferenceValue == null || count > 1)
                    {
                        GUI.backgroundColor = Color.red;
                    }
                    else
                    {
                        GUI.backgroundColor = backgroundColor;
                    }




                    //GUI.color = Color.cyan;
                    GUIStyle insertVarNameHere = new GUIStyle();
                    insertVarNameHere.fontStyle = FontStyle.Bold;
                    EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUI.GetPropertyHeight(element)), element, new GUIContent("Material " + (index + 1) + ": "));

                    GUI.backgroundColor = backgroundColor;


                    if (element.objectReferenceValue == null)
                    {
                        rect.y += EditorGUI.GetPropertyHeight(element);
                        EditorGUI.HelpBox(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight * 2), "Material  " + (index + 1) + "  may not be empty!", MessageType.Error);
                    }
                    else if (count > 1)
                    {
                        rect.y += EditorGUI.GetPropertyHeight(element);
                        EditorGUI.HelpBox(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight * 2), "Duplicate! Material  " + (index + 1) + "  has to be unique!", MessageType.Error);
                    } 
                    else if (this.target.GetType() == typeof(ShaderPropertySync) && !((Material)element.objectReferenceValue).HasProperty(SeeThroughShaderConstants.STS_SHADER_IDENTIFIER_PROPERTY))
                    {
                        rect.y += EditorGUI.GetPropertyHeight(element);
                        EditorGUI.HelpBox(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight * 2), "Material " + (index + 1) + " has to be a STS material!", MessageType.Error);
                    }
                    else if (this.target.GetType() == typeof(GroupShaderReplacement) && ((Material)element.objectReferenceValue).HasProperty(SeeThroughShaderConstants.STS_SHADER_IDENTIFIER_PROPERTY))
                    {
                        rect.y += EditorGUI.GetPropertyHeight(element);
                        EditorGUI.HelpBox(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight * 2), "Material " + (index + 1) + " can NOT be a STS material!", MessageType.Error);
                    }

                },

                elementHeightCallback = index =>
                {
                    var element = materialList.GetArrayElementAtIndex(index);
                    int count = 0;
                    for (int i = 0; i < materialList.arraySize; i++)
                    {
                        if (materialList.GetArrayElementAtIndex(i).objectReferenceValue == element.objectReferenceValue)
                        {
                            count++;
                        }
                    }
                    var height = EditorGUI.GetPropertyHeight(element);
                    if (element.objectReferenceValue == null || count > 1 
                    || (this.target.GetType() == typeof(ShaderPropertySync) && !((Material)element.objectReferenceValue).HasProperty(SeeThroughShaderConstants.STS_SHADER_IDENTIFIER_PROPERTY))
                    || (this.target.GetType() == typeof(GroupShaderReplacement) && ((Material)element.objectReferenceValue).HasProperty(SeeThroughShaderConstants.STS_SHADER_IDENTIFIER_PROPERTY)))
                    {
                        height += EditorGUIUtility.singleLineHeight * 2;
                    }
                    return height;
                },

                onAddCallback = list =>
                {
                    list.serializedProperty.arraySize++;

                    var newElement = list.serializedProperty.GetArrayElementAtIndex(list.serializedProperty.arraySize - 1);
                    newElement.objectReferenceValue = null;
                },
                onRemoveCallback = list =>
                {
                    List<Material> temp = new List<Material>();
                    for (int i = 0; i < list.serializedProperty.arraySize; i++)
                    {
                        temp.Add((Material)list.serializedProperty.GetArrayElementAtIndex(i).objectReferenceValue);
                    }
                    list.serializedProperty.arraySize--;
                    temp.RemoveAt(list.index);
                    for (int i = 0; i < temp.Count; i++)
                    {
                        var d = list.serializedProperty.GetArrayElementAtIndex(i);
                        d.objectReferenceValue = temp[i];
                    }
                    //ReorderableList.defaultBehaviours.DoRemoveButton(list);

                }
            };

        }

        public abstract string Title
        {
            get;
        }

        public abstract string Description
        {
            get;
        }


        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            //SeeThroughShaderEditorUtils.usualStart("Shader Property Synchronizer");

            showDescription = EditorUtils.LogoOnlyStartWithDescription(Title,
                                                           Description,
                                                            showDescription);
            //EditorUtils.LogoOnlyStart(Title);
            var oriCol = EditorStyles.label.normal.textColor;
            EditorStyles.label.normal.textColor = Color.white;


            Rect rectt = EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUI.Box(rectt, GUIContent.none);
            GUIStyle replacementStyle = new GUIStyle();
            replacementStyle.normal.textColor = Color.white;
            replacementStyle.alignment = TextAnchor.MiddleCenter;
            replacementStyle.fontStyle = FontStyle.Bold;


            EditorUtils.Header("Choose The Reference Material", replacementStyle);
            //GUILayout.Label("Choose The Reference Material", replacementStyle);
            //EditorUtils.DrawUILine();
            EditorGUILayout.PropertyField(referenceMaterial);
            if (referenceMaterial.objectReferenceValue == null)
            {
                EditorGUILayout.HelpBox("You have to set a reference material, otherwise the 'See-through Shader' will be limited to the default shader settings and so won't work as expected!", MessageType.Error);

            }
            else if (!((Material)referenceMaterial.objectReferenceValue).HasProperty(SeeThroughShaderConstants.STS_SHADER_IDENTIFIER_PROPERTY))
            {
                EditorGUILayout.HelpBox("The reference material is NOT a 'See-through Shader' material! Replacement will NOT work with a NON-STS shader!", MessageType.Error);

            }
            else if (((Material)referenceMaterial.objectReferenceValue).GetFloat("_IsReplacementShader") == 1)
            {
                EditorGUILayout.HelpBox("\"Global Replacement\" is enabled for the selected reference material. Please disable, otherwise you will get an incorrect behaviour!", MessageType.Error);
                if (GUILayout.Button("Disabel \"Global Replacement\""))
                {
                    ((Material)referenceMaterial.objectReferenceValue).SetFloat("_IsReplacementShader", 0);
                    ((Material)referenceMaterial.objectReferenceValue).DisableKeyword("_REPLACEMENT");
                }
            }


            EditorGUILayout.PropertyField(keepMaterialsInSyncWithReference);
            

            EditorGUILayout.EndVertical();

            EditorUtils.makeHorizontalSeparation();


            rectt = EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUI.Box(rectt, GUIContent.none);


            //rectt = EditorGUILayout.BeginVertical();
            //var c = GUI.color;
            //GUI.color = new Color(0.4f, 0.4f, 0.4f);
            //GUI.Box(rectt, GUIContent.none);
            //GUI.color = c;
            //GUI.Box(rectt, GUIContent.none);
            //GUILayout.Label("Choose The Group Type", replacementStyle);
            //EditorGUILayout.EndVertical();
            //EditorUtils.DrawUILine();

            EditorUtils.Header("Choose The Group Type", replacementStyle);
            //    GUILayout.Label("Choose The Group Type", replacementStyle);
            //    EditorUtils.DrawUILine();
            EditorGUILayout.PropertyField(replacementGroupType, new GUIContent("Group Type"));

            GUILayout.Space(5);

            Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(1));

            r.width = r.width + 6;
            r.x = r.x - 3;

            EditorGUI.DrawRect(r, new Color(0.2f, 0.2f, 0.2f, 1));
            switch ((ReplacementGroupType)replacementGroupType.enumValueIndex)
            {
                case ReplacementGroupType.Parent:
                    {
                        EditorGUILayout.PropertyField(parentTransform);
                        if (parentTransform.objectReferenceValue == null)
                        {
                            EditorGUILayout.HelpBox("You didn't choose a parent transform, that means that this GameObject, '" +
                                this.target.name + "' will be the parent. Every child  of it that isn't in" +
                                " the exemption list will get the See-through Shader assigned to its material during runtime.", MessageType.Info);
                        }
                        break;
                    }
                case ReplacementGroupType.Box:
                    {
                        EditorGUILayout.PropertyField(triggerBox);
                        if (triggerBox.objectReferenceValue == null)
                        {
                            EditorGUILayout.HelpBox("You didn't choose a trigger box, that means that the " + this.target.GetType().Name + " script won't work!.", MessageType.Warning);
                        }
                        break;
                    }
                case ReplacementGroupType.Id:
                    {
                        EditorGUILayout.PropertyField(triggerID);
                        if (string.IsNullOrEmpty(triggerID.stringValue))
                        {
                            EditorGUILayout.HelpBox("You didn't choose an Id, that means that the  " + this.target.GetType().Name + " script won't work!.", MessageType.Warning);
                        }
                        break;
                    }
                case ReplacementGroupType.ListOfGameObjects:
                    {
                        gameObjectsReorderableList.DoLayoutList();
                        //EditorGUILayout.PropertyField(gameObjectList);
                        if (gameObjectList.arraySize <= 0)
                        {
                            EditorGUILayout.HelpBox("You didn't add any GameObjects to the list, that means that the  " + this.target.GetType().Name + " script won't work!.", MessageType.Warning);
                        }
                        break;
                    }
                case ReplacementGroupType.ListOfMaterials:
                    {
                        materialsReorderableList.DoLayoutList();
                        //EditorGUILayout.PropertyField(materialList);
                        if (materialList.arraySize <= 0)
                        {
                            EditorGUILayout.HelpBox("You haven't added any Materials to the list, that means that the  " + this.target.GetType().Name + " script won't work!.", MessageType.Warning);
                        }
                        break;
                    }
                default: break;
            }


            EditorGUILayout.EndVertical();

            EditorUtils.makeHorizontalSeparation();


            rectt = EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUI.Box(rectt, GUIContent.none);

            EditorUtils.Header("Choose Which Layers Are Affected", replacementStyle);

            //GUILayout.Label("Choose Which Layers Are Affected", replacementStyle);
            //EditorUtils.DrawUILine();
            EditorGUILayout.PropertyField(layerMaskToAdd);
            if (layerMaskToAdd.intValue == 0)
            {
                EditorGUILayout.HelpBox("You didn't select any layer! The  " + this.target.GetType().Name + " script won't work!.", MessageType.Warning);
            }
            EditorGUILayout.EndVertical();

            EditorUtils.makeHorizontalSeparation();


            rectt = EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUI.Box(rectt, GUIContent.none);

            EditorUtils.Header("Choose The Exemptions", replacementStyle);
            //GUILayout.Label("Choose The Exemptions", replacementStyle);
            //EditorUtils.DrawUILine();
            EditorGUILayout.PropertyField(materialExemptions);

            //EditorUtils.makeHorizontalSeparation();

            EditorGUILayout.EndVertical();
            //base.DrawDefaultInspector();
            //EditorUtils.usualEnd();
            EditorUtils.LogoOnlyEnd();
            //EditorStyles.label.normal.textColor = oriCol;

            serializedObject.ApplyModifiedProperties();
        }

    }
}