using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CodeStage.PackageToFolder;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace AssetInventory
{
    public sealed class ImportUI : EditorWindow
    {
        public event ActionDone OnImportDone;

        private List<AssetInfo> _assets;
        private List<AssetInfo> _missingPackages;
        private List<AssetInfo> _importQueue;
        private Vector2 _scrollPos;
        private string _customFolder;
        private string _customFolderRel;
        private bool _importRunning;
        private bool _cancellationRequested;
        private AddRequest _addRequest;
        private AssetInfo _curInfo;
        private int _assetPackageCount;
        private int _packageCount;

        public static ImportUI ShowWindow()
        {
            ImportUI window = GetWindow<ImportUI>("Package Import Wizard");
            window.minSize = new Vector2(450, 150);

            return window;
        }

        private void Update()
        {
            if (_assets == null) return;

            // refresh list after downloads finish
            foreach (AssetInfo info in _assets)
            {
                if (info.PackageDownloader == null) continue;
                if (info.ImportState == AssetInfo.ImportStateOptions.Missing)
                {
                    AssetDownloadState state = info.PackageDownloader.GetState();
                    switch (state.state)
                    {
                        case AssetDownloader.State.Downloaded:
                            info.Refresh();
                            Init(_assets);
                            break;
                    }
                }
            }
        }

        public void OnEnable()
        {
            AssetDatabase.importPackageStarted += ImportStarted;
            AssetDatabase.importPackageCompleted += ImportCompleted;
            AssetDatabase.importPackageCancelled += ImportCancelled;
            AssetDatabase.importPackageFailed += ImportFailed;
        }

        public void OnDisable()
        {
            AssetDatabase.importPackageStarted += ImportStarted;
            AssetDatabase.importPackageCompleted += ImportCompleted;
            AssetDatabase.importPackageCancelled += ImportCancelled;
            AssetDatabase.importPackageFailed += ImportFailed;
        }

        private void ImportFailed(string packageName, string errorMessage)
        {
            AssetInfo info = FindAsset(packageName);
            if (info == null) return;

            info.ImportState = AssetInfo.ImportStateOptions.Failed;
            _assets.First(a => a.Id == info.Id).ImportState = info.ImportState;

            Debug.LogError($"Import of '{packageName}' failed: {errorMessage}");
        }

        private void ImportCancelled(string packageName)
        {
            AssetInfo info = FindAsset(packageName);
            if (info == null) return;

            info.ImportState = AssetInfo.ImportStateOptions.Queued;
            _assets.First(a => a.Id == info.Id).ImportState = info.ImportState;
        }

        private void ImportCompleted(string packageName)
        {
            AssetInfo info = FindAsset(packageName);
            if (info == null) return;

            info.ImportState = AssetInfo.ImportStateOptions.Imported;
            _assets.First(a => a.Id == info.Id).ImportState = info.ImportState;
        }

        private void ImportStarted(string packageName)
        {
            AssetInfo info = FindAsset(packageName);
            if (info == null) return;

            info.ImportState = AssetInfo.ImportStateOptions.Importing;
            _assets.First(a => a.Id == info.Id).ImportState = info.ImportState;
        }

        private AssetInfo FindAsset(string packageName)
        {
            return _importQueue?.Find(info => info.SafeName == packageName || info.Location == packageName + ".unitypackage" || info.Location == packageName);
        }

        public void Init(List<AssetInfo> assets)
        {
            _assets = assets;
            _assetPackageCount = assets.Count(info => info.AssetSource != Asset.Source.Package);
            _packageCount = assets.Count(info => info.AssetSource == Asset.Source.Package);

            if (!string.IsNullOrWhiteSpace(_customFolder))
            {
                _customFolderRel = "Assets" + _customFolder.Substring(Application.dataPath.Length);
            }

            // check for non-existing downloads first
            _missingPackages = new List<AssetInfo>();
            _importQueue = new List<AssetInfo>();
            foreach (AssetInfo info in _assets)
            {
                if (info.SafeName == Asset.NONE) continue;
                if (!info.Downloaded)
                {
                    info.ImportState = AssetInfo.ImportStateOptions.Missing;
                    _missingPackages.Add(info);
                }
                else
                {
                    info.ImportState = AssetInfo.ImportStateOptions.Queued;
                    _importQueue.Add(info);
                }
            }
        }

        public void OnGUI()
        {
            EditorGUILayout.Space();
            if (_assets == null || _assets.Count == 0)
            {
                EditorGUILayout.HelpBox("Select packages in the Asset Inventory for importing first.", MessageType.Info);
                return;
            }

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Packages", EditorStyles.boldLabel, GUILayout.Width(85));
            EditorGUILayout.LabelField(_assets.Count.ToString());
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Target Folder", EditorStyles.boldLabel, GUILayout.Width(85));
            EditorGUILayout.LabelField(string.IsNullOrWhiteSpace(_customFolderRel) ? "-default-" : _customFolderRel, GUILayout.ExpandWidth(true));
            if (GUILayout.Button("Select...", GUILayout.ExpandWidth(false))) SelectTargetFolder();
            if (!string.IsNullOrWhiteSpace(_customFolder) && GUILayout.Button("Clear", GUILayout.ExpandWidth(false)))
            {
                _customFolder = null;
                _customFolderRel = null;
                AssetInventory.SaveConfig();
            }
            GUILayout.EndHorizontal();

            if (_missingPackages.Count > 0)
            {
                EditorGUILayout.Space();
                if (_importQueue.Count > 0)
                {
                    EditorGUILayout.HelpBox($"{_missingPackages.Count} packages have not been downloaded yet and will be skipped.", MessageType.Warning);
                }
                else
                {
                    EditorGUILayout.HelpBox("The packages have not been downloaded yet. No import possible until done so.", MessageType.Warning);
                }
            }

            EditorGUILayout.Space(10);
            _scrollPos = GUILayout.BeginScrollView(_scrollPos, false, false, GUIStyle.none, GUI.skin.verticalScrollbar, GUILayout.ExpandWidth(true));
            foreach (AssetInfo info in _assets)
            {
                if (info.SafeName == Asset.NONE) continue;

                GUILayout.BeginHorizontal();
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.Toggle(info.Downloaded, GUILayout.Width(20));
                EditorGUI.EndDisabledGroup();
                EditorGUILayout.LabelField(new GUIContent(info.GetDisplayName(), info.AssetSource == Asset.Source.Package ? info.SafeName : info.Location));
                GUILayout.FlexibleSpace();
                if (info.ImportState == AssetInfo.ImportStateOptions.Missing)
                {
                    if (info.PackageDownloader == null) info.PackageDownloader = new AssetDownloader(info);
                    AssetDownloadState state = info.PackageDownloader.GetState();
                    switch (state.state)
                    {
                        case AssetDownloader.State.Unavailable:
                            if (info.PackageDownloader.IsDownloadSupported() && GUILayout.Button("Download", GUILayout.Width(80))) info.PackageDownloader.Download();
                            break;

                        case AssetDownloader.State.Downloading:
                            EditorGUILayout.LabelField(Mathf.RoundToInt(state.progress * 100f) + "%", GUILayout.Width(80));
                            break;
                    }
                }
                else
                {
                    EditorGUILayout.LabelField(info.ImportState.ToString(), GUILayout.Width(80));
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndScrollView();
            EditorGUILayout.Space();

            GUILayout.BeginHorizontal();
            EditorGUI.BeginDisabledGroup(_importRunning);
            EditorGUI.BeginDisabledGroup(_assetPackageCount == 0);
            if (GUILayout.Button("Import Interactive...")) BulkImportAssets(_assets);
            EditorGUI.EndDisabledGroup();
            if (GUILayout.Button("Import Automatically")) BulkImportAssets(_assets, false);
            EditorGUI.EndDisabledGroup();
            if (_importRunning && GUILayout.Button("Cancel All")) _cancellationRequested = true;
            GUILayout.EndHorizontal();
        }

        private void SelectTargetFolder()
        {
            string folder = EditorUtility.OpenFolderPanel("Select target folder in your project", _customFolder, "");
            if (string.IsNullOrEmpty(folder)) return;

            if (folder.StartsWith(Application.dataPath))
            {
                _customFolder = folder;
                _customFolderRel = "Assets" + folder.Substring(Application.dataPath.Length);
                AssetInventory.SaveConfig();
            }
            else
            {
                EditorUtility.DisplayDialog("Error", "The target folder must be inside your current Unity project.", "OK");
            }
        }

        private async void BulkImportAssets(List<AssetInfo> assetIds, bool interactive = true)
        {
            if (assetIds.Count == 0) return;

            _importRunning = true;
            _cancellationRequested = false;

            if (!string.IsNullOrWhiteSpace(_customFolder))
            {
                _customFolderRel = "Assets" + _customFolder.Substring(Application.dataPath.Length);
                if (!Directory.Exists(_customFolder)) Directory.CreateDirectory(_customFolder);
            }

            AssetDatabase.StartAssetEditing(); // TODO: will cause progress UI to stay on top and not close anymore
            try
            {
                foreach (AssetInfo info in _importQueue.Where(info => info.ImportState == AssetInfo.ImportStateOptions.Queued))
                {
                    _curInfo = info;
                    info.ImportState = AssetInfo.ImportStateOptions.Importing;

                    if (info.AssetSource == Asset.Source.Package)
                    {
                        AddRegistry(info.Registry);
                        switch (info.PackageSource)
                        {
                            case PackageSource.Git:
                                Repository repo = JsonConvert.DeserializeObject<Repository>(info.Repository);
                                if (repo == null)
                                {
                                    Debug.LogError($"Repository for {info} is not maintained.");
                                    continue;
                                }
                                if (string.IsNullOrWhiteSpace(repo.revision))
                                {
                                    _addRequest = Client.Add($"{repo.url}");
                                }
                                else
                                {
                                    _addRequest = Client.Add($"{repo.url}#{repo.revision}");
                                }
                                break;

                            default:
                                _addRequest = Client.Add($"{info.SafeName}@{info.GetVersionToUse()}");
                                break;
                        }
                        EditorApplication.update += AddProgress;
                    }
                    else
                    {
                        // launch directly or intercept package resolution to tweak paths
                        if (string.IsNullOrWhiteSpace(_customFolderRel))
                        {
                            AssetDatabase.ImportPackage(info.Location, interactive);
                        }
                        else
                        {
                            Package2Folder.ImportPackageToFolder(info.Location, _customFolderRel, interactive);
                        }
                    }

                    // wait until done
                    while (!_cancellationRequested && info.ImportState == AssetInfo.ImportStateOptions.Importing)
                    {
                        await Task.Delay(25);
                    }

                    if (info.ImportState == AssetInfo.ImportStateOptions.Importing) info.ImportState = AssetInfo.ImportStateOptions.Queued;
                    if (_cancellationRequested) break;
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error importing packages: {e.Message}");
            }

            // handle potentially pending imports and put them back in the queue
            _assets.ForEach(info =>
            {
                if (info.ImportState == AssetInfo.ImportStateOptions.Importing) info.ImportState = AssetInfo.ImportStateOptions.Queued;
            });
            _importRunning = false;

            AssetDatabase.StopAssetEditing();
            AssetDatabase.Refresh();
#if UNITY_2020_3_OR_NEWER
            Client.Resolve();
#endif
            OnImportDone?.Invoke();
        }

        private void AddRegistry(string registry)
        {
            if (string.IsNullOrEmpty(registry)) return;
            if (registry == "Unity") return;
            ScopedRegistry sr = JsonConvert.DeserializeObject<ScopedRegistry>(registry);
            if (sr == null) return;

            string manifestFile = Path.Combine(Application.dataPath, "..", "Packages", "manifest.json");
            JObject content = JObject.Parse(File.ReadAllText(manifestFile));
            JArray registries = (JArray) content["scopedRegistries"];
            if (registries == null)
            {
                registries = new JArray();
                content["scopedRegistries"] = registries;
            }

            // do nothing if already existent
            if (registries.Any(r => r["name"]?.Value<string>() == sr.name && r["url"]?.Value<string>() == sr.url)) return;

            registries.Add(JToken.FromObject(sr));

            File.WriteAllText(manifestFile, content.ToString());
        }

        private void AddProgress()
        {
            if (!_addRequest.IsCompleted) return;

            EditorApplication.update -= AddProgress;

            if (_addRequest.Status == StatusCode.Success)
            {
                _curInfo.ImportState = AssetInfo.ImportStateOptions.Imported;
            }
            else
            {
                _curInfo.ImportState = AssetInfo.ImportStateOptions.Failed;
                Debug.LogError($"Importing {_curInfo} failed: {_addRequest.Error.message}");
            }
        }
    }
}