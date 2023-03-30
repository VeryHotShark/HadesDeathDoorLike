using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace ShaderCrew.SeeThroughShader
{
    public class ReferenceMaterialCreator : EditorWindow
    {
        string path = "";
        string nameText = "referenceMaterial";
        GUIContent test = new GUIContent();

        string saveLocation;
        bool saveLocationSelected = false;
        bool isCreated = false;


        GUIStyle textStyle;

        bool m_FirstTimeApply = true;

        private string getCurrentProjectWindowPath()
        {
            Object obj = Selection.activeObject;
            if (obj != null) 
            {
                string path = AssetDatabase.GetAssetPath(obj.GetInstanceID());

                if (path.Length > 0)
                {
                    if (Directory.Exists(path))
                    {
                        return path;
                    }
                }
            }

            return "";
        }
        void DoSetup()
        {
            textStyle = EditorStyles.wordWrappedLabel;
            textStyle.richText = true;

        }

        void OnGUI()
        {
            if (m_FirstTimeApply)
            {
                m_FirstTimeApply = false;
                DoSetup();
            }

            EditorUtils.usualStart("Reference Material Creator");
            GUILayout.Space(10);
            System.DateTime epoch = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
            long ms = (long)(System.DateTime.UtcNow - epoch).TotalMilliseconds;
            long result = ms / 1000;
            if (nameText == "referenceMaterial")
            {
                nameText = nameText + result;
            }

            if (!isCreated)
            {



                nameText = EditorGUILayout.TextField("Choose a name: ", nameText);

                string currentProjectWindowPath = getCurrentProjectWindowPath();

                if (GUILayout.Button("Select Save Location"))
                {
                    //string appPath = Application.dataPath;
                    saveLocation = EditorUtility.OpenFolderPanel("Select Save Location", currentProjectWindowPath, "");
                    if (saveLocation.StartsWith(Application.dataPath))
                    {
                        saveLocation = "Assets" + saveLocation.Substring(Application.dataPath.Length);
                    }

                    if (saveLocation != null && saveLocation.Length != 0)
                    {
                        saveLocationSelected = true;
                    }
                }


                if (saveLocationSelected)
                {
                    path = saveLocation;
                } else
                {
                    path = currentProjectWindowPath;
                }
                EditorGUILayout.LabelField("<b>Save Location:</b>: <i>" + path + "</i>", textStyle);
                path += "/" + nameText + ".mat";

                
                EditorGUILayout.LabelField("<b>Reference Material Shader:</b>: <i>" + GeneralUtils.getUnityVersionAndRenderPipelineCorrectedShaderString().versionAndRPCorrectedShader + "</i>", textStyle);

                EditorUtils.DrawUILineGray(1, 0);

                EditorGUILayout.LabelField("<b>Preview</b>: <i>" + path + "</i>",textStyle);



                //GUILayout.Space(10);
                if (GUILayout.Button("Create"))
                {
                    Material referenceMaterial = new Material(Shader.Find(GeneralUtils.getUnityVersionAndRenderPipelineCorrectedShaderString().versionAndRPCorrectedShader));
                    referenceMaterial.color = Color.white;
                    referenceMaterial.SetFloat("_isReferenceMaterial", 1);
                    AssetDatabase.CreateAsset(referenceMaterial, path);
                    isCreated = true;

                }
                if (GUILayout.Button("Cancel")) this.Close();
            }
            else
            {
                GUIStyle replacementStyle = new GUIStyle();
                replacementStyle.normal.textColor = Color.white;
                replacementStyle.alignment = TextAnchor.MiddleCenter;
                replacementStyle.fontStyle = FontStyle.Bold;

                GUIStyle centerStyle = new GUIStyle();
                centerStyle.normal.textColor = EditorStyles.wordWrappedLabel.normal.textColor;
                centerStyle.alignment = TextAnchor.MiddleCenter;
                centerStyle.fontStyle = FontStyle.Bold;

                EditorGUILayout.LabelField("Congratulations! ", centerStyle);
                EditorGUILayout.LabelField(nameText + ".mat" + " successfully got created!", replacementStyle);
                GUILayout.Space(20);
                EditorGUILayout.LabelField("Path: " + path, EditorStyles.wordWrappedLabel);
                if (GUILayout.Button("Close")) this.Close();
            }
            //this.Repaint();
            EditorUtils.usualEnd();


        }
    }

}