using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FIMSpace.FEditor
{
    public class FScale_QuickScaleWindow : EditorWindow
    {
        public Vector2 NewDimensions;
        private List<Texture2D> textures;
        private bool dimSetted = false;

        public static void Init()
        {
            FScale_QuickScaleWindow window = (FScale_QuickScaleWindow)GetWindow(typeof(FScale_QuickScaleWindow));

            window.minSize = new Vector2(250f, 85f);
            window.maxSize = window.minSize;

            window.titleContent = new GUIContent("Resize Textures");
            window.Show();
        }


        void OnGUI()
        {
            if (textures == null) textures = new List<Texture2D>();
            textures.Clear();

            if (Selection.objects.Length > 0)
            {
                for (int i = 0; i < Selection.objects.Length; i++)
                {
                    Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(AssetDatabase.GetAssetPath(Selection.objects[i]));
                    if (texture != null) textures.Add(texture);
                }

                if (!dimSetted)
                    if (textures.Count > 0)
                    {
                        NewDimensions = new Vector2(textures[0].width, textures[0].height);
                        dimSetted = true;
                    }
            }

            if ( textures.Count == 0)
            {
                EditorGUILayout.HelpBox("You must select at least one texture file!", MessageType.Warning);
                return;
            }

            EditorGUILayout.LabelField("Textures to rescale: " + textures.Count);

            if (NewDimensions.x < 1) NewDimensions.x = 1;
            if (NewDimensions.y < 1) NewDimensions.y = 1;
            if (NewDimensions.x > 4096) NewDimensions.x = 4096;
            if (NewDimensions.y > 4096) NewDimensions.y = 4096;

            NewDimensions = EditorGUILayout.Vector2Field("New Dimensions", NewDimensions);

            FGUI_Inspector.DrawUILine(Color.white * 0.35f, 2, 5);

            if (GUILayout.Button("Scale Files (" + textures.Count + ")"))
            {
                try
                {
                    for (int i = 0; i < textures.Count; i++)
                    {
                        EditorUtility.DisplayProgressBar("Scaling textures...", "Scaling texture " + textures[i].name, (float)i / (float)textures.Count);

                        FScale_EditorToolsMethods.ScaleTextureFile(textures[i], textures[i], NewDimensions);
                    }

                    EditorUtility.ClearProgressBar();
                }
                catch (System.Exception exc)
                {
                    EditorUtility.ClearProgressBar();
                    Debug.LogError("[ICONS MANAGER] Something went wrong when scaling textures! " + exc);
                }
            }
        }

    }
}