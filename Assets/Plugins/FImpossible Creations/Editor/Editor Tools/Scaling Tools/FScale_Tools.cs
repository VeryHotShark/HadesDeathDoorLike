using FIMSpace.FTex;
using UnityEditor;
using UnityEngine;

namespace FIMSpace.FEditor
{
    public static class FScale_Tools
    {
        [MenuItem("Assets/FImpossible Creations/Texture Tools/Change Scale", priority = 0)]
        public static void RescaleTexture()
        {
            FScale_ScalerWindow.Init();
        }

        [MenuItem("Assets/FImpossible Creations/Texture Tools/Quick Rescale", priority = 1)]
        public static void QuickRescaleTexture()
        {
            FScale_QuickScaleWindow.Init();
        }

        [MenuItem("Assets/FImpossible Creations/Texture Tools/Convert any to PNG", priority = 2)]
        public static void ToPNGConversion()
        {
            FScale_QuickConverterWindow.Init();
        }

        [MenuItem("Assets/FImpossible Creations/Texture Tools/Channel Insert", priority = 3)]
        public static void ChannelInserter()
        {
            FScale_ChannelInserter.Init();
        }

        [MenuItem("Assets/FImpossible Creations/Texture Tools/Scale to nearest power of 2", priority = 14)]
        public static void ScaleToPowerOf2()
        {
            try
            {
                for (int i = 0; i < Selection.objects.Length; i++)
                {
                    Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(AssetDatabase.GetAssetPath(Selection.objects[i]));

                    EditorUtility.DisplayProgressBar("Scaling textures...", "Scaling texture " + texture.name, (float)i / (float)Selection.objects.Length);

                    if (texture != null)
                        FScale_EditorToolsMethods.ScaleTextureFile(texture, texture, new Vector2(FTex_Methods.FindNearestPowOf2(texture.width), FTex_Methods.FindNearestPowOf2(texture.height)));
                }

                EditorUtility.ClearProgressBar();
            }
            catch (System.Exception exc)
            {
                EditorUtility.ClearProgressBar();
                Debug.LogError("[ICONS MANAGER] Something went wrong when scaling textures! " + exc);
            }
        }

        [MenuItem("Assets/FImpossible Creations/Texture Tools/Scale to power of 2 Lower", priority = 27)]
        public static void ScaleToPowerOf2Lower()
        {
            try
            {
                for (int i = 0; i < Selection.objects.Length; i++)
                {
                    Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(AssetDatabase.GetAssetPath(Selection.objects[i]));

                    EditorUtility.DisplayProgressBar("Scaling textures...", "Scaling texture " + texture.name, (float)i / (float)Selection.objects.Length);

                    if (texture != null)
                        FScale_EditorToolsMethods.ScaleTextureFile(texture, texture, new Vector2(FTex_Methods.FindLowerPowOf2(texture.width), FTex_Methods.FindLowerPowOf2(texture.height)));
                }

                EditorUtility.ClearProgressBar();
            }
            catch (System.Exception exc)
            {
                EditorUtility.ClearProgressBar();
                Debug.LogError("[ICONS MANAGER] Something went wrong when scaling textures! " + exc);
            }
        }

        [MenuItem("Assets/FImpossible Creations/Texture Tools/Scale to power of 2 Higher", priority = 26)]
        public static void ScaleToPowerOf2Higher()
        {
            try
            {
                for (int i = 0; i < Selection.objects.Length; i++)
                {
                    Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(AssetDatabase.GetAssetPath(Selection.objects[i]));

                    EditorUtility.DisplayProgressBar("Scaling textures...", "Scaling texture " + texture.name, (float)i / (float)Selection.objects.Length);

                    if (texture != null)
                        FScale_EditorToolsMethods.ScaleTextureFile(texture, texture, new Vector2(FTex_Methods.FindHigherPowOf2(texture.width), FTex_Methods.FindHigherPowOf2(texture.height)));
                }

                EditorUtility.ClearProgressBar();
            }
            catch (System.Exception exc)
            {
                EditorUtility.ClearProgressBar();
                Debug.LogError("[ICONS MANAGER] Something went wrong when scaling textures! " + exc);
            }

        }


        [MenuItem("Assets/FImpossible Creations/Texture Tools/Change Scale", true)]
        [MenuItem("Assets/FImpossible Creations/Texture Tools/Quick Rescale", true)]
        [MenuItem("Assets/FImpossible Creations/Texture Tools/Seamless Looper Window", true)]
        public static bool CheckRescaleTextureAllSelected()
        {
            if (!Selection.activeObject) return false;

            for (int i = 0; i < Selection.objects.Length; i++) // We need just one file to be texture to return true
            {
                AssetImporter tex = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(Selection.objects[i]));
                if (tex as TextureImporter) return true;
            }

            return false;
        }
    }
}