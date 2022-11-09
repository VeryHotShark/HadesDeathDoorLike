using UnityEditor;
using UnityEngine;

namespace AssetInventory
{
    public sealed class FolderSettingsUI : PopupWindowContent
    {
        private FolderSpec _spec;

        public void Init(FolderSpec spec)
        {
            _spec = spec;
        }

        public override void OnGUI(Rect rect)
        {
            editorWindow.maxSize = new Vector2(300, 130);
            editorWindow.minSize = editorWindow.maxSize;
            int width = 110;

            EditorGUILayout.Space();

            EditorGUI.BeginChangeCheck();

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(UIStyles.Content("Content", "Type of content to scan for"), EditorStyles.boldLabel, GUILayout.Width(width));
            _spec.folderType = EditorGUILayout.Popup(_spec.folderType, UIStyles.FolderTypes);
            GUILayout.EndHorizontal();

            switch (_spec.folderType)
            {
                case 1: // media
                    GUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(UIStyles.Content("Find", "File types to search for"), EditorStyles.boldLabel, GUILayout.Width(width));
                    _spec.scanFor = EditorGUILayout.Popup(_spec.scanFor, UIStyles.MediaTypes);
                    GUILayout.EndHorizontal();

                    if (_spec.scanFor == 6)
                    {
                        GUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField(UIStyles.Content("Pattern", "e.g. *.jpg;*.wav"), EditorStyles.boldLabel, GUILayout.Width(width));
                        _spec.pattern = EditorGUILayout.TextField(_spec.pattern);
                        GUILayout.EndHorizontal();
                    }

                    GUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(UIStyles.Content("Create Previews", "Recommended. Will generate previews and additional metadata but requires more time during indexing."), EditorStyles.boldLabel, GUILayout.Width(width));
                    _spec.createPreviews = EditorGUILayout.Toggle(_spec.createPreviews);
                    GUILayout.EndHorizontal();
                    break;

                case 2: // zip
                    if (AssetInventory.DEBUG_MODE)
                    {
                        GUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField(UIStyles.Content("Prefer Packages", "If the zip contains unity package files the package will be indexed instead and all other files ignored."), EditorStyles.boldLabel, GUILayout.Width(width));
                        _spec.preferPackages = EditorGUILayout.Toggle(_spec.preferPackages);
                        GUILayout.EndHorizontal();
                    }
                    break;
            }

            if (EditorGUI.EndChangeCheck()) AssetInventory.SaveConfig();
        }
    }
}