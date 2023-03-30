using System.Diagnostics;
using UnityEditor;
using UnityEngine;

namespace FIMSpace.FEditor
{
    public class FScale_ChannelInserter : EditorWindow
    {
        public Texture2D From;
        public enum EChannelSelect { R, G, B, A, RGB }
        public EChannelSelect ChannelFrom = EChannelSelect.R;
        public EChannelSelect ApplyTo = EChannelSelect.A;

        public static void Init()
        {
            FScale_ChannelInserter window = (FScale_ChannelInserter)GetWindow(typeof(FScale_ChannelInserter));

            window.minSize = new Vector2(270f, 225f);

            window.titleContent = new GUIContent("Channel Insert");
            window.Show();
        }


        void OnGUI()
        {
            Texture2D texture = null;

            if (Selection.objects.Length > 0)
            {
                for (int i = 0; i < Selection.objects.Length; i++)
                {
                    texture = AssetDatabase.LoadAssetAtPath<Texture2D>(AssetDatabase.GetAssetPath(Selection.objects[i]));
                    if (texture != null) break;
                }
            }

            if (texture == null)
            {
                EditorGUILayout.HelpBox("You must select at least one texture file!", MessageType.Warning);
                return;
            }

            EditorGUILayout.LabelField("Texture to edit: " + texture.name);

            From = (Texture2D)EditorGUILayout.ObjectField(From == null ? "From:" : "From " + From.name, From, typeof(Texture2D), false);
            ChannelFrom = (EChannelSelect)EditorGUILayout.EnumPopup("Get channel:", ChannelFrom);
            ApplyTo = (EChannelSelect)EditorGUILayout.EnumPopup("Apply it to:", ApplyTo);



            FGUI_Inspector.DrawUILine(Color.white * 0.35f, 2, 5);

            if (From != null)
            {
                if (From.width != texture.width || From.height != texture.height)
                {
                    EditorGUILayout.HelpBox("Dimensions of the textures must be equal!", MessageType.Info);
                }
                else
                {
                    if (GUILayout.Button( (ApplyTo == EChannelSelect.A ? "(Requires A channel) " : "") + "Insert '" + ChannelFrom + "' to '" + ApplyTo + "' channel of " + texture.name))
                    {
                        ProcessChanneling(From, texture);
                    }


                    if (GUILayout.Button("Duplicate (png) and Insert Channel (" + texture.name + ")"))
                    {
                        Texture2D duplicated = FScale_EditorToolsMethods.DuplicateAsPNG(texture, "-PNG", true, ApplyTo == EChannelSelect.A);
                        if ( duplicated != null) ProcessChanneling(From, duplicated);
                    }

                }
            }
            else
            {
                EditorGUILayout.HelpBox("You must select 'From' texture", MessageType.Info);
            }

        }

        public void ProcessChanneling(Texture2D source, Texture2D target)
        {
            TextureImporter srcImporter = FScale_EditorToolsMethods.GetTextureAsset(source);
            var srcInfo = FScale_EditorToolsMethods.GetTextureInfo(srcImporter, source);

            TextureImporter tgtImporter = FScale_EditorToolsMethods.GetTextureAsset(target);
            var tgtInfo = FScale_EditorToolsMethods.GetTextureInfo(tgtImporter, target);

            try
            {
                EditorUtility.DisplayProgressBar("Channeling textures...", "Scaling texture " + target.name, 0.2f);

                FScale_EditorToolsMethods.StartEditingTextureAsset(srcImporter, source, srcInfo);
                FScale_EditorToolsMethods.StartEditingTextureAsset(tgtImporter, target, tgtInfo);

                Color32[] srcPixels = source.GetPixels32();
                Color32[] newPixels = target.GetPixels32();

                for (int i = 0; i < newPixels.Length; i++)
                    newPixels[i] = SwapChannel(srcPixels[i], newPixels[i], ChannelFrom, ApplyTo);

                FScale_EditorToolsMethods.EndEditingTextureAsset(srcPixels, srcInfo, srcImporter, source);
                FScale_EditorToolsMethods.EndEditingTextureAsset(newPixels, tgtInfo, tgtImporter, target);

                EditorUtility.ClearProgressBar();
            }
            catch (System.Exception exc)
            {
                srcInfo.RestoreOn(srcImporter, source, false);
                tgtInfo.RestoreOn(tgtImporter, source, false);

                EditorUtility.ClearProgressBar();
                UnityEngine.Debug.LogError("[ICONS MANAGER] Something went wrong when channeling textures! " + exc);
            }
        }

        public Color32 SwapChannel(Color32 source, Color32 target, EChannelSelect from, EChannelSelect to)
        {
            Color32 newC = target;
            byte tgt = target.r;

            switch (from)
            {
                case EChannelSelect.R: tgt = source.r; break;
                case EChannelSelect.G: tgt = source.g; break;
                case EChannelSelect.B: tgt = source.b; break;
                case EChannelSelect.A: tgt = source.a; break;
                case EChannelSelect.RGB: tgt = (byte)(Mathf.Min( (source.r + source.g + source.b) / 3, byte.MaxValue)); break;
            }

            switch (to)
            {
                case EChannelSelect.R: newC.r = tgt; break;
                case EChannelSelect.G: newC.g = tgt; break;
                case EChannelSelect.B: newC.b = tgt; break;
                case EChannelSelect.A: newC.a = tgt; break;
                case EChannelSelect.RGB: newC.r = tgt; newC.g = tgt; newC.b = tgt; break;
            }

            return newC;
        }

    }
}