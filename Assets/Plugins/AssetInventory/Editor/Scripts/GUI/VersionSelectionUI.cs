using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace AssetInventory
{
    public sealed class VersionSelectionUI : PopupWindowContent
    {
        private AssetInfo _assetInfo;
        private PackageInfo _packageInfo;
        private Vector2 _scrollPos;
        private Action<string> _callback;

        public void Init(AssetInfo info, Action<string> callback)
        {
            _assetInfo = info;
            _callback = callback;
        }

        public override Vector2 GetWindowSize()
        {
            return new Vector2(350, 300);
        }

        public override void OnGUI(Rect rect)
        {
            if (_assetInfo == null) return;
            if (!AssetStore.IsMetadataAvailable())
            {
                EditorGUILayout.HelpBox("Loading package metadata...", MessageType.Info);
                return;
            }
            if (_packageInfo == null) _packageInfo = AssetStore.GetPackageInfo(_assetInfo.SafeName);
            if (_packageInfo == null)
            {
                EditorGUILayout.HelpBox("Could not find matching package metadata.", MessageType.Warning);
                return;
            }

            _scrollPos = GUILayout.BeginScrollView(_scrollPos, false, false);
            List<string> attributes = new List<string>();
            Color oldCol = GUI.backgroundColor;
            foreach (string version in _packageInfo.versions.all.Reverse())
            {
                attributes.Clear();
                if (_packageInfo.versions.compatible.Contains(version))
                {
                    attributes.Add("compatible");
                }
                else
                {
                    GUI.backgroundColor = Color.yellow;
                }
                if (version == _packageInfo.versions.verified)
                {
                    GUI.backgroundColor = Color.green;
                    attributes.Add("verified");
                }
                if (AssetStore.IsInstalled(_packageInfo.name, version))
                {
                    attributes.Add("installed");
                }

                GUILayout.BeginHorizontal();
                if (GUILayout.Button(version, GUILayout.Width(140)))
                {
                    _callback?.Invoke(version);
                    editorWindow.Close();
                }
                EditorGUI.BeginDisabledGroup(_assetInfo.GetChangeLogURL(version) == null);
                if (GUILayout.Button(UIStyles.Content("?", "Changelog"), GUILayout.Width(20)))
                {
                    Application.OpenURL(_assetInfo.GetChangeLogURL(version));
                }
                EditorGUI.EndDisabledGroup();
                if (attributes.Count > 0) GUILayout.Label(string.Join(", ", attributes));
                GUILayout.EndHorizontal();

                GUI.backgroundColor = oldCol;
            }
            GUILayout.EndScrollView();
        }
    }
}