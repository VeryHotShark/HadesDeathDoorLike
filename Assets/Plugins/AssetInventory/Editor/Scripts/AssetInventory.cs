using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using JD.EditorAudioUtils;
using Newtonsoft.Json;
using Unity.EditorCoroutines.Editor;
using Unity.SharpZipLib.Zip;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AssetInventory
{
    public delegate void TagsChangedData();

    public delegate void ActionDone();

    public static class AssetInventory
    {
        public const string TOOL_VERSION = "1.6.0";
        public const string ASSET_STORE_LINK = "https://u3d.as/2Sy1";
        public const int ASSET_STORE_ID = 226927;
        public static readonly bool DEBUG_MODE = false;

        public static readonly string[] ScanDependencies = {"prefab", "mat", "controller", "anim", "asset", "physicmaterial", "physicsmaterial", "sbs", "sbsar", "cubemap", "shadergraph", "shadersubgraph"};

        public static string CurrentMain { get; set; }
        public static int MainCount { get; set; }
        public static int MainProgress { get; set; }
        public static string UsedConfigLocation { get; private set; }

        public static event TagsChangedData OnTagsChanged;
        public static event ActionDone OnIndexingDone;

        private const string TEMP_FOLDER = "_AssetInventoryTemp";
        private const int ASSET_DETAILS_REFRESH_THRESHOLD = 24 * 7;
        private const int BREAK_INTERVAL = 5;
        private const int MAX_DROPDOWN_ITEMS = 25;
        private const string CONFIG_NAME = "AssetInventoryConfig.json";
        private const string ASSET_STORE_FOLDER_NAME = "Asset Store-5.x";
        private static readonly Regex FileGuid = new Regex("guid: (?:([a-z0-9]*))");

        private static IEnumerable<TagInfo> Tags
        {
            get
            {
                if (_tags == null) LoadTagAssignments();
                return _tags;
            }
        }

        private static List<TagInfo> _tags;

        public static AssetInventorySettings Config
        {
            get
            {
                if (_config == null) LoadConfig();
                return _config;
            }
        }

        private static AssetInventorySettings _config;

        public static bool IndexingInProgress { get; private set; }
        public static bool ClearCacheInProgress { get; private set; }

        public static Dictionary<string, string[]> TypeGroups { get; } = new Dictionary<string, string[]>
        {
            {"Audio", new[] {"wav", "mp3", "ogg", "aiff", "aif", "mod", "it", "s3m", "xm"}},
            {"Images", new[] {"png", "jpg", "jpeg", "bmp", "tga", "tif", "tiff", "psd", "svg", "webp", "ico", "exr", "gif", "hdr", "iff", "pict"}},
            {"Video", new[] {"mp4"}},
            {"Prefabs", new[] {"prefab"}},
            {"Materials", new[] {"mat", "physicmaterial", "physicsmaterial", "sbs", "sbsar", "cubemap"}},
            {"Shaders", new[] {"shader", "shadergraph", "shadersubgraph", "compute"}},
            {"Models", new[] {"fbx", "obj", "blend", "dae", "3ds", "dxf", "max", "c4d", "mb", "ma"}},
            {"Scripts", new[] {"cs", "php"}},
            {"Libraries", new[] {"zip", "unitypackage", "so", "bundle", "dll", "jar"}},
            {"Documents", new[] {"md", "doc", "docx", "txt", "json", "rtf", "pdf", "html", "readme", "xml", "chm"}}
        };

        public static int TagHash { get; private set; }

        public static bool IsFileType(string path, string type)
        {
            if (path == null) return false;
            return TypeGroups[type].Contains(Path.GetExtension(path).ToLowerInvariant().Replace(".", string.Empty));
        }

        public static string GetStorageFolder()
        {
            if (!string.IsNullOrEmpty(Config.customStorageLocation)) return Path.GetFullPath(Config.customStorageLocation);

            return IOUtils.PathCombine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "AssetInventory");
        }

        private static string GetConfigLocation()
        {
            // search for local project-specific override first
            string guid = AssetDatabase.FindAssets(Path.GetFileNameWithoutExtension(CONFIG_NAME)).FirstOrDefault();
            if (guid != null) return AssetDatabase.GUIDToAssetPath(guid);

            return IOUtils.PathCombine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), CONFIG_NAME);
        }

        public static string GetPreviewFolder(string customFolder = null)
        {
            string previewPath = IOUtils.PathCombine(customFolder ?? GetStorageFolder(), "Previews");
            if (!Directory.Exists(previewPath)) Directory.CreateDirectory(previewPath);
            return previewPath;
        }

        public static string GetBackupFolder(bool createOnDemand = true)
        {
            string backupPath = string.IsNullOrWhiteSpace(Config.backupFolder) ? IOUtils.PathCombine(GetStorageFolder(), "Backups") : Config.backupFolder;
            if (createOnDemand && !Directory.Exists(backupPath)) Directory.CreateDirectory(backupPath);
            return backupPath;
        }

        private static string GetMaterializePath()
        {
            return IOUtils.PathCombine(GetStorageFolder(), "Extracted");
        }

        private static string GetMaterializedAssetPath(Asset asset)
        {
            return IOUtils.PathCombine(GetMaterializePath(), asset.SafeName);
        }

        public static async Task<string> ExtractAsset(Asset asset)
        {
            if (string.IsNullOrEmpty(asset.Location)) return null;
            if (!File.Exists(asset.Location))
            {
                Debug.LogError($"Asset has vanished since last refresh and cannot be indexed: {asset.Location}");
                return null;
            }

            string tempPath = GetMaterializedAssetPath(asset);
            int retries = 0;
            while (retries < 5 && Directory.Exists(tempPath))
            {
                try
                {
                    await Task.Run(() => Directory.Delete(tempPath, true));
                    break;
                }
                catch (Exception)
                {
                    retries++;
                    await Task.Delay(500);
                }
            }
            if (Directory.Exists(tempPath)) Debug.LogWarning($"Could not remove temporary directory: {tempPath}");

            if (asset.AssetSource == Asset.Source.Archive)
            {
                FastZip fastZip = new FastZip();
                await Task.Run(() => fastZip.ExtractZip(asset.Location, tempPath, null));
            }
            else
            {
                await Task.Run(() => TarUtil.ExtractGz(asset.Location, tempPath));
            }

            return Directory.Exists(tempPath) ? tempPath : null;
        }

        public static bool IsMaterialized(Asset asset, AssetFile assetFile)
        {
            if (asset.AssetSource == Asset.Source.Directory || asset.AssetSource == Asset.Source.Package)
            {
                return File.Exists(assetFile.SourcePath);
            }

            string sourcePath = Path.Combine(GetMaterializedAssetPath(asset), assetFile.SourcePath);
            return File.Exists(sourcePath);
        }

        public static async Task<string> EnsureMaterializedAsset(AssetInfo info)
        {
            string targetPath = await EnsureMaterializedAsset(info.ToAsset(), info);
            info.IsMaterialized = IsMaterialized(info.ToAsset(), info);
            return targetPath;
        }

        public static async Task<string> EnsureMaterializedAsset(Asset asset, AssetFile assetFile)
        {
            if (asset.AssetSource == Asset.Source.Directory || asset.AssetSource == Asset.Source.Package)
            {
                return File.Exists(assetFile.SourcePath) ? assetFile.SourcePath : null;
            }

            string sourcePath = Path.Combine(GetMaterializedAssetPath(asset), assetFile.SourcePath);
            if (!File.Exists(sourcePath)) await ExtractAsset(asset);
            if (!File.Exists(sourcePath)) return null;

            string targetPath = Path.Combine(Path.GetDirectoryName(sourcePath), "Content", Path.GetFileName(assetFile.Path));
            try
            {
                if (!File.Exists(targetPath))
                {
                    if (!Directory.Exists(Path.GetDirectoryName(targetPath))) Directory.CreateDirectory(Path.GetDirectoryName(targetPath));
                    File.Copy(sourcePath, targetPath);
                }
                string sourceMetaPath = sourcePath + ".meta";
                string targetMetaPath = targetPath + ".meta";
                if (File.Exists(sourceMetaPath) && !File.Exists(targetMetaPath)) File.Copy(sourceMetaPath, targetMetaPath);
            }
            catch (Exception e)
            {
                Debug.LogError($"Could not extract file. Most likely the target device ran out of space: {e.Message}");
                return null;
            }
            return targetPath;
        }

        public static async Task CalculateDependencies(AssetInfo assetInfo)
        {
            string targetPath = await EnsureMaterializedAsset(assetInfo.ToAsset(), assetInfo);
            if (targetPath == null) return;

            assetInfo.Dependencies = await DoCalculateDependencies(assetInfo, targetPath);
            assetInfo.DependencySize = assetInfo.Dependencies.Sum(af => af.Size);
            assetInfo.MediaDependencies = assetInfo.Dependencies.Where(af => af.Type != "cs").ToList();
            assetInfo.ScriptDependencies = assetInfo.Dependencies.Where(af => af.Type == "cs").ToList();

            // clean-up again on-demand
            string tempDir = Path.Combine(Application.dataPath, TEMP_FOLDER);
            if (Directory.Exists(tempDir))
            {
                await IOUtils.DeleteFileOrDirectory(tempDir);
                await IOUtils.DeleteFileOrDirectory(tempDir + ".meta");
                AssetDatabase.Refresh();
            }
        }

        private static async Task<List<AssetFile>> DoCalculateDependencies(AssetInfo assetInfo, string path)
        {
            List<AssetFile> result = new List<AssetFile>();

            // only scan file types that contain guid references
            if (!ScanDependencies.Contains(Path.GetExtension(path).Replace(".", string.Empty))) return result;

            string content = File.ReadAllText(path);
            if (!content.StartsWith("%YAML"))
            {
                // reserialize prefabs on-the-fly by copying them over which will cause Unity to change the encoding upon refresh
                // this will not work but throw missing script errors instead if there are any attached
                string targetDir = Path.Combine(Application.dataPath, TEMP_FOLDER);
                if (!Directory.Exists(targetDir)) Directory.CreateDirectory(targetDir);

                string targetFile = Path.Combine("Assets", TEMP_FOLDER, Path.GetFileName(path));
                File.Copy(path, targetFile);
                AssetDatabase.Refresh();
                content = File.ReadAllText(targetFile);

                if (!content.StartsWith("%YAML"))
                {
                    assetInfo.DependencyState = AssetInfo.DependencyStateOptions.NotPossible;
                    return result;
                }
            }
            MatchCollection matches = FileGuid.Matches(content);

            foreach (Match match in matches)
            {
                string guid = match.Groups[1].Value;
                if (result.Any(r => r.Guid == guid)) continue; // break recursion

                // find file with guid inside of the respective package only, don't look into others that might repackage it
                // since that can throw errors if the asset was not downloaded yet or has an identical Id although being different
                AssetFile af = DBAdapter.DB.Find<AssetFile>(a => a.Guid == guid && a.AssetId == assetInfo.AssetId);

                // ignore missing guids as they are not in the package so we can't do anything about them
                if (af == null) continue;

                result.Add(af);

                // recursive
                string targetPath = await EnsureMaterializedAsset(assetInfo.ToAsset(), af);
                if (targetPath == null)
                {
                    Debug.LogWarning($"Could not materialize dependency: {af.Path}");
                    continue;
                }

                result.AddRange(await DoCalculateDependencies(assetInfo, targetPath));
            }

            return result;
        }

        public static List<AssetInfo> LoadAssets()
        {
            string indexedQuery = "select *, Count(*) as FileCount, Sum(af.Size) as UncompressedSize from AssetFile af left join Asset on Asset.Id = af.AssetId group by af.AssetId order by Lower(Asset.SafeName)";
            List<AssetInfo> indexedResult = DBAdapter.DB.Query<AssetInfo>(indexedQuery);

            string allQuery = "select *, Id as AssetId from Asset order by Lower(SafeName)";
            List<AssetInfo> allResult = DBAdapter.DB.Query<AssetInfo>(allQuery);

            // sqlite does not support "right join", therefore merge two queries manually 
            List<AssetInfo> result = allResult;
            result.ForEach(asset =>
            {
                AssetInfo match = indexedResult.FirstOrDefault(indexedAsset => indexedAsset.Id == asset.Id);
                if (match == null) return;
                asset.FileCount = match.FileCount;
                asset.UncompressedSize = match.UncompressedSize;
            });

            return result;
        }

        public static string[] ExtractAssetNames(IEnumerable<AssetInfo> assets)
        {
            bool intoSubmenu = Config.groupLists && assets.Count(a => a.FileCount > 0) > MAX_DROPDOWN_ITEMS;
            List<string> result = new List<string> {"-all-", string.Empty};
            result.AddRange(assets
                .Where(a => !a.Exclude)
                .Where(a => a.FileCount > 0)
                .Select(a => intoSubmenu && !a.SafeName.StartsWith("-") ? a.SafeName.Substring(0, 1).ToUpperInvariant() + "/" + a.SafeName : a.SafeName)
                .OrderBy(s => s));

            if (result.Count == 2) result.RemoveAt(1);

            // move -none- to top
            int noneIdx = result.FindIndex(a => a == Asset.NONE);
            if (noneIdx >= 0)
            {
                string tmp = result[noneIdx];
                result.RemoveAt(noneIdx);
                result.Insert(1, tmp);

                if (result.Count == 3) result.RemoveAt(2);
            }

            return result.ToArray();
        }

        public static string[] ExtractTagNames(List<Tag> tags)
        {
            bool intoSubmenu = Config.groupLists && tags.Count > MAX_DROPDOWN_ITEMS;
            List<string> result = new List<string> {"-all-", string.Empty};
            result.AddRange(tags
                .Select(a => intoSubmenu && !a.Name.StartsWith("-") ? a.Name.Substring(0, 1).ToUpperInvariant() + "/" + a.Name : a.Name)
                .OrderBy(s => s));

            if (result.Count == 2) result.RemoveAt(1);

            return result.ToArray();
        }

        public static string[] ExtractPublisherNames(IEnumerable<AssetInfo> assets)
        {
            bool intoSubmenu = Config.groupLists && assets.Count(a => a.FileCount > 0) > MAX_DROPDOWN_ITEMS; // approximation, publishers != assets but roughly the same
            List<string> result = new List<string> {"-all-", string.Empty};
            result.AddRange(assets
                .Where(a => !a.Exclude)
                .Where(a => a.FileCount > 0)
                .Where(a => !string.IsNullOrEmpty(a.SafePublisher))
                .Select(a => intoSubmenu ? a.SafePublisher.Substring(0, 1).ToUpperInvariant() + "/" + a.SafePublisher : a.SafePublisher)
                .Distinct()
                .OrderBy(s => s));

            if (result.Count == 2) result.RemoveAt(1);

            return result.ToArray();
        }

        public static string[] ExtractCategoryNames(IEnumerable<AssetInfo> assets)
        {
            bool intoSubmenu = Config.groupLists;
            List<string> result = new List<string> {"-all-", string.Empty};
            result.AddRange(assets
                .Where(a => !a.Exclude)
                .Where(a => a.FileCount > 0)
                .Where(a => !string.IsNullOrEmpty(a.SafeCategory))
                .Select(a =>
                {
                    if (intoSubmenu)
                    {
                        string[] arr = a.GetDisplayCategory().Split('/');
                        return arr[0] + "/" + a.SafeCategory;
                    }
                    return a.SafeCategory;
                })
                .Distinct()
                .OrderBy(s => s));

            if (result.Count == 2) result.RemoveAt(1);

            return result.ToArray();
        }

        public static string[] LoadTypes()
        {
            List<string> result = new List<string> {"-all-", string.Empty};

            string query = "select Distinct(Type) from AssetFile where Type not null and Type != \"\" order by Type";
            List<string> raw = DBAdapter.DB.QueryScalars<string>($"{query}");

            List<string> groupTypes = new List<string>();
            foreach (KeyValuePair<string, string[]> group in TypeGroups)
            {
                groupTypes.AddRange(group.Value);
                foreach (string type in group.Value)
                {
                    if (raw.Contains(type))
                    {
                        result.Add($"{group.Key}");
                        break;
                    }
                }
            }
            if (result.Last() != "") result.Add(string.Empty);

            // others
            result.AddRange(raw.Where(r => !groupTypes.Contains(r)).Select(type => $"Others/{type}"));

            // all
            result.AddRange(raw.Select(type => $"All/{type}"));

            if (result.Count == 2) result.RemoveAt(1);

            return result.ToArray();
        }

        public static async Task<long> GetCacheFolderSize()
        {
            return await IOUtils.GetFolderSize(GetMaterializePath());
        }

        public static async Task<long> GetPreviewFolderSize()
        {
            return await IOUtils.GetFolderSize(GetPreviewFolder());
        }

        public static async void RefreshIndex(int assetId = 0, bool force = false)
        {
            IndexingInProgress = true;
            AssertProgress.CancellationRequested = false;

            Init();

            // pass 1: metadata
            // special handling for normal asset store assets since directory structure yields additional information
            if (Config.indexAssetStore && assetId == 0)
            {
                string assetStoreDownloadCache = GetAssetDownloadPath();
                if (!Directory.Exists(assetStoreDownloadCache))
                {
                    Debug.LogWarning($"Could not find the asset download folder: {assetStoreDownloadCache}");
                    EditorUtility.DisplayDialog("Error", $"Could not find the asset download folder: {assetStoreDownloadCache}.\n\nEither nothing was downloaded yet through the Package Manager or you changed the Asset cache location. In the latter case, please add the new location as an additional folder.", "OK");
                    IndexingInProgress = false;
                    return;
                }
                await new UnityPackageImporter().IndexRough(assetStoreDownloadCache, true, force);
            }

            if (Config.indexPackageCache && assetId == 0)
            {
                string packageDownloadCache = GetPackageDownloadPath();
                if (!Directory.Exists(packageDownloadCache))
                {
                    Debug.LogWarning($"Could not find the package download folder: {packageDownloadCache}");
                    EditorUtility.DisplayDialog("Error", $"Could not find the package download folder: {packageDownloadCache}.\n\nEither nothing was downloaded yet through the Package Manager or you changed the Package cache location. In the latter case, please add the new location as an additional folder.", "OK");
                    IndexingInProgress = false;
                    return;
                }
                await new PackageImporter().IndexRough(packageDownloadCache, true);
            }

            // pass 2: details
            if ((Config.indexAssetStore && Config.indexAssetPackageContents) || assetId > 0) await new UnityPackageImporter().IndexDetails(assetId);
            if (Config.indexPackageCache || assetId > 0) await new PackageImporter().IndexDetails(assetId);

            // scan custom folders
            for (int i = 0; i < Config.folders.Count; i++)
            {
                if (AssertProgress.CancellationRequested) break;

                FolderSpec spec = Config.folders[i];
                if (!spec.enabled) continue;
                if (!Directory.Exists(spec.location))
                {
                    Debug.LogWarning($"Specified folder to scan for assets does not exist anymore: {spec.location}");
                    continue;
                }
                switch (spec.folderType)
                {
                    case 0:
                        if (assetId == 0)
                        {
                            bool hasAssetStoreLayout = Path.GetFileName(spec.location) == ASSET_STORE_FOLDER_NAME;
                            await new UnityPackageImporter().IndexRough(spec.location, hasAssetStoreLayout, force);
                        }
                        await new UnityPackageImporter().IndexDetails(assetId);
                        break;

                    case 1:
                        if (assetId == 0) await new MediaImporter().Index(spec);
                        break;

                    case 2:
                        if (assetId == 0) await new ZipImporter().Index(spec);
                        break;

                    default:
                        Debug.LogError($"Unsupported folder scan type: {spec.folderType}");
                        break;
                }
            }

            // pass 3: index colors
            if (Config.extractColors && assetId == 0)
            {
                AssertProgress.Running = true;
                EditorCoroutineUtility.StartCoroutineOwnerless(new ColorImporter().Index());
                while (AssertProgress.Running) await Task.Delay(50);
            }

            // pass 4: backup
            if (Config.createBackups && assetId == 0)
            {
                AssetBackup backup = new AssetBackup();
                await backup.Sync();
            }

            IndexingInProgress = false;
            OnIndexingDone?.Invoke();
        }

        public static string GetAssetDownloadPath()
        {
#if UNITY_EDITOR_WIN
            return IOUtils.PathCombine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Unity", ASSET_STORE_FOLDER_NAME);
#endif
#if UNITY_EDITOR_OSX
            return IOUtils.PathCombine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Library", "Unity", ASSET_STORE_FOLDER_NAME);
#endif
#if UNITY_EDITOR_LINUX
            return IOUtils.PathCombine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), ".local/share/unity3d", ASSET_STORE_FOLDER_NAME);
#endif
        }

        private static string GetPackageDownloadPath()
        {
#if UNITY_EDITOR_WIN
            return IOUtils.PathCombine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Unity", "cache", "packages");
#endif
#if UNITY_EDITOR_OSX
            return IOUtils.PathCombine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Library", "Unity", "cache", "packages");
#endif
#if UNITY_EDITOR_LINUX
            return IOUtils.PathCombine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), ".config/unity3d/cache/packages");
#endif
        }

        public static void Init()
        {
            string folder = GetStorageFolder();
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
            DBAdapter.InitDB();

            PerformUpgrades();
            LoadTagAssignments();

            AssetStore.GatherProjectMetadata();
            AssetStore.GatherAllMetadata();
        }

        private static void PerformUpgrades()
        {
            // filename was introduced in version 2
            AppProperty dbVersion = DBAdapter.DB.Find<AppProperty>("Version");

            if (dbVersion == null)
            {
                // Upgrade from Initial to v2
                // add filenames to DB
                List<AssetFile> assetFiles = DBAdapter.DB.Table<AssetFile>().ToList();
                foreach (AssetFile assetFile in assetFiles)
                {
                    assetFile.FileName = Path.GetFileName(assetFile.Path);
                }
                DBAdapter.DB.UpdateAll(assetFiles);
            }
            else if (dbVersion.Value == "2" || dbVersion.Value == "3")
            {
                // force re-fetching of asset details to get state
                DBAdapter.DB.Execute("update Asset set ETag=null, LastOnlineRefresh=0");
            }

            if (dbVersion?.Value != "4")
            {
                AppProperty newVersion = new AppProperty("Version", "4");
                DBAdapter.DB.InsertOrReplace(newVersion);
            }
        }

        public static void ClearCache(Action callback = null)
        {
            ClearCacheInProgress = true;
            Task _ = Task.Run(() =>
            {
                if (Directory.Exists(GetMaterializePath())) Directory.Delete(GetMaterializePath(), true);
                callback?.Invoke();
                ClearCacheInProgress = false;
            });
        }

        private static void LoadConfig()
        {
            string configLocation = GetConfigLocation();
            UsedConfigLocation = configLocation;

            if (configLocation == null || !File.Exists(configLocation))
            {
                _config = new AssetInventorySettings();
                return;
            }
            _config = JsonConvert.DeserializeObject<AssetInventorySettings>(File.ReadAllText(configLocation));
            if (_config == null) _config = new AssetInventorySettings();
            if (_config.folders == null) _config.folders = new List<FolderSpec>();
        }

        public static void SaveConfig()
        {
            string configFile = GetConfigLocation();
            if (configFile == null) return;

            File.WriteAllText(configFile, JsonConvert.SerializeObject(_config));
        }

        public static void ResetConfig()
        {
            DBAdapter.Close(); // in case DB path changes

            _config = new AssetInventorySettings();
            SaveConfig();
            AssetDatabase.Refresh();
        }

        public static async Task<AssetPurchases> FetchOnlineAssets()
        {
            AssetStore.CancellationRequested = false;
            AssetPurchases assets = await AssetStore.RetrievePurchases();
            if (assets == null) return null; // happens if token was invalid 

            CurrentMain = "Phase 2/3: Updating purchases";
            MainCount = assets.results.Count;
            MainProgress = 1;
            int progressId = MetaProgress.Start("Updating purchases");

            bool tagsChanged = false;
            for (int i = 0; i < MainCount; i++)
            {
                MainProgress = i + 1;
                MetaProgress.Report(progressId, i + 1, MainCount, string.Empty);
                if (i % BREAK_INTERVAL == 0) await Task.Yield(); // let editor breath
                if (AssetStore.CancellationRequested) break;

                AssetPurchase purchase = assets.results[i];

                // update all known assets with that foreignId to support updating duplicate assets as well 
                List<Asset> existingAssets = DBAdapter.DB.Table<Asset>().Where(a => a.ForeignId == purchase.packageId).ToList();
                if (existingAssets.Count == 0)
                {
                    // create new asset on-demand
                    Asset asset = purchase.ToAsset();
                    asset.SafeName = purchase.CalculatedSafeName;
                    if (Config.excludeByDefault) asset.Exclude = true;
                    DBAdapter.DB.Insert(asset);
                    existingAssets.Add(asset);
                }

                for (int i2 = 0; i2 < existingAssets.Count; i2++)
                {
                    Asset asset = existingAssets[i2];

                    // temporarily store guessed safe name to ensure locally indexed files are mapped correctly
                    // will be overridden in detail run
                    asset.AssetSource = Asset.Source.AssetStorePackage;
                    asset.DisplayName = purchase.displayName.Trim();
                    asset.ForeignId = purchase.packageId;
                    if (string.IsNullOrEmpty(asset.SafeName)) asset.SafeName = purchase.CalculatedSafeName;

                    // override data with local truth in case header information exists
                    if (File.Exists(asset.Location))
                    {
                        AssetHeader header = UnityPackageImporter.ReadHeader(asset.Location);
                        UnityPackageImporter.ApplyHeader(header, asset);
                    }

                    DBAdapter.DB.Update(asset);

                    // handle tags
                    if (purchase.tagging != null)
                    {
                        foreach (string tag in purchase.tagging)
                        {
                            if (AddTagAssignment(asset.Id, tag, TagAssignment.Target.Package, true)) tagsChanged = true;
                        }
                    }
                }
            }

            if (tagsChanged)
            {
                LoadTags();
                LoadTagAssignments();
            }

            CurrentMain = null;
            MetaProgress.Remove(progressId);

            return assets;
        }

        public static async Task FetchAssetsDetails()
        {
            List<Asset> assets = DBAdapter.DB.Table<Asset>()
                .Where(a => a.ForeignId > 0)
                .ToList()
                .Where(a => (DateTime.Now - a.LastOnlineRefresh).TotalHours > ASSET_DETAILS_REFRESH_THRESHOLD)
                .ToList();

            CurrentMain = "Phase 3/3: Updating package details";
            MainCount = assets.Count;
            MainProgress = 1;
            int progressId = MetaProgress.Start("Updating package details");

            for (int i = 0; i < MainCount; i++)
            {
                Asset asset = assets[i];
                int id = asset.ForeignId;

                MainProgress = i + 1;
                MetaProgress.Report(progressId, i + 1, MainCount, string.Empty);
                if (i % BREAK_INTERVAL == 0) await Task.Yield(); // let editor breath
                if (AssetStore.CancellationRequested) break;

                AssetDetails details = await AssetStore.RetrieveAssetDetails(id, asset.ETag);
                if (details == null) // happens if unchanged through etag
                {
                    asset.LastOnlineRefresh = DateTime.Now;
                    DBAdapter.DB.Update(asset);
                    continue;
                }

                // check if disabled, then download links are not available anymore, deprecated would still work
                DownloadInfo downloadDetails = null;
                if (asset.AssetSource == Asset.Source.AssetStorePackage && details.state != "disabled")
                {
                    downloadDetails = await AssetStore.RetrieveAssetDownloadInfo(id);
                    if (downloadDetails == null || string.IsNullOrEmpty(downloadDetails.filename_safe_package_name))
                    {
                        Debug.Log($"Could not fetch download detail information for '{asset.SafeName}'");
                        continue;
                    }
                }

                // reload asset to ensure working on latest copy, otherwise might loose package size information
                if (downloadDetails != null)
                {
                    asset.SafeName = downloadDetails.filename_safe_package_name;
                    asset.SafeCategory = downloadDetails.filename_safe_category_name;
                    asset.SafePublisher = downloadDetails.filename_safe_publisher_name;
                    asset.OriginalLocation = downloadDetails.url;
                    asset.OriginalLocationKey = downloadDetails.key;
                    if (!string.IsNullOrEmpty(asset.Location) && asset.GetCalculatedLocation() != asset.Location) asset.CurrentSubState = Asset.SubState.Outdated;
                }
                asset.LastOnlineRefresh = DateTime.Now;
                asset.OfficialState = details.state;
                asset.ETag = details.ETag;
                asset.DisplayName = details.name;
                asset.DisplayPublisher = details.productPublisher.name;
                asset.DisplayCategory = details.category.name;

                if (string.IsNullOrEmpty(asset.SafeName)) asset.SafeName = AssetUtils.GuessSafeName(details.name);
                asset.Description = details.description;
                asset.Requirements = string.Join(", ", details.requirements);
                asset.Keywords = string.Join(", ", details.keyWords);
                asset.SupportedUnityVersions = string.Join(", ", details.supportedUnityVersions);
                asset.Revision = details.revision;
                asset.Slug = details.slug;
                asset.LatestVersion = details.version.name;
                asset.LastRelease = details.version.publishedDate;
                if (details.productReview != null)
                {
                    asset.AssetRating = details.productReview.ratingAverage;
                    asset.RatingCount = int.Parse(details.productReview.ratingCount);
                }
                asset.CompatibilityInfo = details.compatibilityInfo;
                asset.MainImage = details.mainImage?.url;
                asset.MainImageSmall = details.mainImage?.small_v2 ?? details.mainImage?.small;
                asset.MainImageIcon = details.mainImage?.icon;
                asset.ReleaseNotes = details.publishNotes;
                asset.KeyFeatures = details.keyFeatures;
                if (asset.PackageSize == 0 && details.uploads != null)
                {
                    // use size of download for latest Unity version
                    KeyValuePair<string, UploadInfo> upload = details.uploads.OrderBy(pair => new SemVer(pair.Key)).LastOrDefault();
                    if (upload.Value != null && int.TryParse(upload.Value.downloadSize, out int size))
                    {
                        asset.PackageSize = size;
                    }
                }

                // override data with local truth in case header information exists
                if (File.Exists(asset.Location))
                {
                    AssetHeader header = UnityPackageImporter.ReadHeader(asset.Location);
                    UnityPackageImporter.ApplyHeader(header, asset);
                }

                DBAdapter.DB.Update(asset);
                await Task.Delay(Random.Range(50, 1000)); // don't flood server
            }

            CurrentMain = null;
            MetaProgress.Remove(progressId);
        }

        public static int CountPurchasedAssets(IEnumerable<AssetInfo> assets)
        {
            return assets.Count(a => a.AssetSource == Asset.Source.AssetStorePackage);
        }

        public static List<AssetInfo> CalculateAssetUsage()
        {
            List<AssetInfo> result = new List<AssetInfo>();

            // identify asset packages through guids lookup
            List<string> guids = GatherGuids(new[] {Application.dataPath});
            foreach (string guid in guids)
            {
                List<AssetInfo> files = Guid2File(guid);
                if (files.Count == 0)
                {
                    // found non-indexed asset
                    AssetInfo ai = new AssetInfo();
                    ai.Guid = guid;
                    ai.CurrentState = Asset.State.Unknown;
                    result.Add(ai);
                    continue;
                }
                if (files.Count > 1)
                {
                    Debug.LogWarning("Duplicate guids found: " + string.Join(", ", files.Select(ai => ai.Path)));
                    continue;
                }
                result.Add(files[0]);
            }

            return result;
        }

        public static List<AssetInfo> Guid2File(string guid)
        {
            string query = "select * from AssetFile inner join Asset on Asset.Id = AssetFile.AssetId where Guid=?";
            List<AssetInfo> files = DBAdapter.DB.Query<AssetInfo>($"{query}", guid);
            return files;
        }

        private static List<string> GatherGuids(IEnumerable<string> folders)
        {
            List<string> result = new List<string>();

            foreach (string folder in folders)
            {
                // scan for all meta files and return corresponding asset
                string[] assets = Directory.GetFiles(folder, "*.meta", SearchOption.AllDirectories);
                for (int i = 0; i < assets.Length; i++)
                {
                    assets[i] = assets[i].Substring(0, assets[i].Length - 5).Replace("\\", "/");
                    assets[i] = assets[i].Substring(Application.dataPath.Length - 6); // leave "Assets/" in
                }
                foreach (string asset in assets)
                {
                    string guid = GetAssetGuid(asset);
                    if (string.IsNullOrEmpty(guid)) continue;

                    result.Add(guid);
                }
            }

            return result;
        }

        private static string GetAssetGuid(string assetFile)
        {
            string guid = AssetDatabase.AssetPathToGUID(assetFile);
            if (!string.IsNullOrEmpty(guid)) return guid;

            // hidden files might not be indexed
            string metaFile = $"{assetFile}.meta";
            if (!File.Exists(metaFile)) return null;

            using (StreamReader reader = new StreamReader(metaFile))
            {
                string line;
                while (!string.IsNullOrEmpty(line = reader.ReadLine()))
                {
                    if (!line.StartsWith("guid:")) continue;
                    return line.Substring(5).Trim();
                }
            }

            return null;
        }

        public static void MoveDatabase(string targetFolder)
        {
            string targetDBFile = Path.Combine(targetFolder, Path.GetFileName(DBAdapter.GetDBPath()));
            if (File.Exists(targetDBFile)) File.Delete(targetDBFile);
            string oldStorageFolder = GetStorageFolder();
            DBAdapter.Close();

            bool success = false;
            try
            {
                // for safety copy first, then delete old state after everything is done
                EditorUtility.DisplayProgressBar("Moving Database", "Copying database to new location...", 0.2f);
                File.Copy(DBAdapter.GetDBPath(), targetDBFile);
                EditorUtility.ClearProgressBar();

                EditorUtility.DisplayProgressBar("Moving Preview Images", "Copying preview images to new location...", 0.4f);
                IOUtils.CopyDirectory(GetPreviewFolder(), GetPreviewFolder(targetFolder));
                EditorUtility.ClearProgressBar();

                // set new location
                SwitchDatabase(targetFolder);
                success = true;
            }
            catch
            {
                EditorUtility.DisplayDialog("Error Moving Data", "There were errors moving the existing database to a new location. Check the error log for details. Current database location remains unchanged.", "OK");
            }

            if (success)
            {
                EditorUtility.DisplayProgressBar("Freeing Up Space", "Removing backup files from old location...", 0.8f);
                Directory.Delete(oldStorageFolder, true);
                EditorUtility.ClearProgressBar();
            }
        }

        public static void SwitchDatabase(string targetFolder)
        {
            DBAdapter.Close();
            AssetUtils.ClearCache();
            Config.customStorageLocation = targetFolder;
            SaveConfig();
            Init();
        }

        public static Asset ForgetAsset(AssetInfo info, bool removeExclusion = false)
        {
            DBAdapter.DB.Execute("delete from AssetFile where AssetId=?", info.AssetId);

            Asset existing = DBAdapter.DB.Find<Asset>(info.AssetId);
            if (existing == null) return null;

            existing.CurrentState = Asset.State.New;
            info.CurrentState = Asset.State.New;
            existing.LastOnlineRefresh = DateTime.MinValue;
            info.LastOnlineRefresh = DateTime.MinValue;
            existing.ETag = null;
            info.ETag = null;
            if (removeExclusion)
            {
                existing.Exclude = false;
                info.Exclude = false;
            }

            DBAdapter.DB.Update(existing);

            return existing;
        }

        public static void RemoveAsset(AssetInfo info, bool deleteFiles)
        {
            if (deleteFiles)
            {
                if (File.Exists(info.Location)) File.Delete(info.Location);
                if (Directory.Exists(info.Location)) Directory.Delete(info.Location, true);
                // TODO: delete preview images as well
            }
            Asset existing = ForgetAsset(info);
            if (existing == null) return;

            DBAdapter.DB.Execute("delete from Asset where Id=?", info.AssetId);
        }

        public static async Task<string> CopyTo(AssetInfo assetInfo, string selectedPath, bool withDependencies = false, bool withScripts = false)
        {
            string result = null;
            string sourcePath = await EnsureMaterializedAsset(assetInfo);
            if (sourcePath != null)
            {
                string finalPath = selectedPath;

                // put into subfolder if multiple files are affected
                if (withDependencies)
                {
                    finalPath = Path.Combine(finalPath, Path.GetFileNameWithoutExtension(assetInfo.FileName));
                    if (!Directory.Exists(finalPath)) Directory.CreateDirectory(finalPath);
                }

                string targetPath = Path.Combine(finalPath, Path.GetFileName(sourcePath));
                DoCopyTo(sourcePath, targetPath);
                result = targetPath;

                if (withDependencies)
                {
                    List<AssetFile> deps = withScripts ? assetInfo.Dependencies : assetInfo.MediaDependencies;
                    for (int i = 0; i < deps.Count; i++)
                    {
                        // check if already in target
                        if (!string.IsNullOrEmpty(deps[i].Guid))
                        {
                            if (!string.IsNullOrWhiteSpace(AssetDatabase.GUIDToAssetPath(deps[i].Guid))) continue;
                        }

                        sourcePath = await EnsureMaterializedAsset(assetInfo.ToAsset(), deps[i]);
                        if (sourcePath != null)
                        {
                            targetPath = Path.Combine(finalPath, Path.GetFileName(deps[i].Path));
                            DoCopyTo(sourcePath, targetPath);
                        }
                    }
                }

                AssetDatabase.Refresh();
                assetInfo.ProjectPath = AssetDatabase.GUIDToAssetPath(assetInfo.Guid);
            }

            return result;
        }

        private static void DoCopyTo(string sourcePath, string targetPath)
        {
            File.Copy(sourcePath, targetPath, true);

            string sourceMetaPath = sourcePath + ".meta";
            string targetMetaPath = targetPath + ".meta";
            if (File.Exists(sourceMetaPath)) File.Copy(sourceMetaPath, targetMetaPath, true);
        }

        public static async Task PlayAudio(AssetInfo assetInfo)
        {
            string targetPath = await EnsureMaterializedAsset(assetInfo);

            EditorAudioUtility.StopAllPreviewClips();
            if (targetPath != null)
            {
                AudioClip clip = await AssetUtils.LoadAudioFromFile(targetPath);
                if (clip != null) EditorAudioUtility.PlayPreviewClip(clip);
            }
        }

        public static void SetAssetExclusion(AssetInfo info, bool exclude)
        {
            Asset asset = DBAdapter.DB.Find<Asset>(info.AssetId);
            if (asset == null) return;

            asset.Exclude = exclude;
            info.Exclude = exclude;

            DBAdapter.DB.Update(asset);
        }

        public static void SetAssetVersionPreference(AssetInfo info, string version)
        {
            Asset asset = DBAdapter.DB.Find<Asset>(info.AssetId);
            if (asset == null) return;

            asset.PreferredVersion = version;
            info.PreferredVersion = version;

            // TODO: update minimal Unity version

            DBAdapter.DB.Update(asset);
        }

        public static bool AddTagAssignment(int targetId, string tag, TagAssignment.Target target, bool fromAssetStore = false)
        {
            Tag existingT = AddTag(tag, fromAssetStore);
            if (existingT == null) return false;

            TagAssignment existingA = DBAdapter.DB.Find<TagAssignment>(t => t.TagId == existingT.Id && t.TargetId == targetId && t.TagTarget == target);
            if (existingA != null) return false; // already added

            TagAssignment newAssignment = new TagAssignment(existingT.Id, target, targetId);
            DBAdapter.DB.Insert(newAssignment);

            return true;
        }

        public static bool AddTagAssignment(AssetInfo info, string tag, TagAssignment.Target target)
        {
            if (!AddTagAssignment(target == TagAssignment.Target.Asset ? info.Id : info.AssetId, tag, target)) return false;

            LoadTagAssignments(info);

            return true;
        }

        public static void RemoveTagAssignment(AssetInfo info, TagInfo tagInfo, bool autoReload = true)
        {
            DBAdapter.DB.Delete<TagAssignment>(tagInfo.Id);

            if (autoReload) LoadTagAssignments(info);
        }

        public static void RemoveTagAssignment(List<AssetInfo> infos, string name)
        {
            infos.ForEach(info =>
            {
                TagInfo tagInfo = info.PackageTags?.Find(t => t.Name == name);
                if (tagInfo == null) return;
                RemoveTagAssignment(info, tagInfo, false);
                info.SetTagsDirty();
            });
            LoadTagAssignments();
        }

        public static void LoadTagAssignments(AssetInfo info = null)
        {
            string dataQuery = "select *, TagAssignment.Id as Id from TagAssignment inner join Tag on Tag.Id = TagAssignment.TagId order by TagTarget, TargetId";
            _tags = DBAdapter.DB.Query<TagInfo>($"{dataQuery}").ToList();
            TagHash = Random.Range(0, int.MaxValue);

            info?.SetTagsDirty();
            OnTagsChanged?.Invoke();
        }

        public static List<TagInfo> GetAssetTags(int assetFileId)
        {
            return Tags?.Where(t => t.TagTarget == TagAssignment.Target.Asset && t.TargetId == assetFileId).OrderBy(t => t.Name).ToList();
        }

        public static List<TagInfo> GetPackageTags(int assetId)
        {
            return Tags?.Where(t => t.TagTarget == TagAssignment.Target.Package && t.TargetId == assetId).OrderBy(t => t.Name).ToList();
        }

        public static void SaveTag(Tag tag)
        {
            DBAdapter.DB.Update(tag);
            LoadTagAssignments();
        }

        public static Tag AddTag(string name, bool fromAssetStore = false)
        {
            name = name.Trim();
            if (string.IsNullOrWhiteSpace(name)) return null;

            Tag tag = DBAdapter.DB.Find<Tag>(t => t.Name.ToLower() == name.ToLower());
            if (tag == null)
            {
                tag = new Tag(name);
                tag.FromAssetStore = fromAssetStore;
                DBAdapter.DB.Insert(tag);

                OnTagsChanged?.Invoke();
            }
            else if (!tag.FromAssetStore && fromAssetStore)
            {
                tag.FromAssetStore = true;
                DBAdapter.DB.Update(tag); // don't trigger changed event in such cases, this is just for book-keeping
            }

            return tag;
        }

        public static void RenameTag(Tag tag, string newName)
        {
            newName = newName.Trim();
            if (string.IsNullOrWhiteSpace(newName)) return;

            tag.Name = newName;
            DBAdapter.DB.Update(tag);
            LoadTagAssignments();
        }

        public static void DeleteTag(Tag tag)
        {
            DBAdapter.DB.Execute("delete from TagAssignment where TagId=?", tag.Id);
            DBAdapter.DB.Delete<Tag>(tag.Id);
            LoadTagAssignments();
        }

        public static List<Tag> LoadTags()
        {
            return DBAdapter.DB.Table<Tag>().ToList().OrderBy(t => t.Name, StringComparer.OrdinalIgnoreCase).ToList();
        }

        public static void ScheduleRecreatePreviews(Asset asset)
        {
            DBAdapter.DB.Execute("update AssetFile set PreviewState=2 where AssetId=?", asset.Id);
        }

        public static async void RecreatePreviews()
        {
            string query = "select *, AssetFile.Id as Id from AssetFile inner join Asset on Asset.Id = AssetFile.AssetId where AssetFile.PreviewState == ?";
            List<AssetInfo> files = DBAdapter.DB.Query<AssetInfo>(query, AssetFile.PreviewOptions.Redo).ToList();
            PreviewGenerator.Init(files.Count);
            string previewPath = GetPreviewFolder();
            foreach (AssetInfo af in files)
            {
                // do not recreate for files with dependencies as that will throw lots of errors, need a full import for that (TODO)
                if (ScanDependencies.Contains(af.Type))
                {
                    DBAdapter.DB.Execute("update AssetFile set PreviewState=? where Id=?", AssetFile.PreviewOptions.Custom, af.Id);
                    continue;
                }

                await Task.Yield(); // let editor breath

                string previewFile = Path.Combine(previewPath, "af-" + af.Id + ".png");
                string sourcePath = await EnsureMaterializedAsset(af);
                if (sourcePath == null)
                {
                    DBAdapter.DB.Execute("update AssetFile set PreviewState=? where Id=?", AssetFile.PreviewOptions.Custom, af.Id);
                    continue;
                }

                PreviewGenerator.RegisterPreviewRequest(af.Id, sourcePath, previewFile, req =>
                {
                    AssetFile paf = DBAdapter.DB.Find<AssetFile>(req.Id);
                    if (paf == null) return;

                    // TODO: deduplicate
                    if (req.Obj is Texture2D tex)
                    {
                        paf.Width = tex.width;
                        paf.Height = tex.height;
                    }
                    if (req.Obj is AudioClip clip)
                    {
                        paf.Length = clip.length;
                    }
                    if (File.Exists(req.DestinationFile)) paf.PreviewFile = Path.GetFileName(previewFile);
                    paf.PreviewState = AssetFile.PreviewOptions.Custom;
                    paf.DominantColor = null;
                    paf.DominantColorGroup = null;

                    DBAdapter.DB.Update(paf);
                });
                if (PreviewGenerator.ActiveRequestCount() > 100) await PreviewGenerator.ExportPreviews(10);
            }
            await PreviewGenerator.ExportPreviews();
        }

        public static void ConnectToAssetStore(AssetInfo info, AssetDetails details)
        {
            Asset existing = DBAdapter.DB.Find<Asset>(info.AssetId);
            if (existing == null) return;

            existing.ETag = null;
            info.ETag = null;
            existing.ForeignId = int.Parse(details.packageId);
            info.ForeignId = int.Parse(details.packageId);
            existing.LastOnlineRefresh = DateTime.MinValue;
            info.LastOnlineRefresh = DateTime.MinValue;

            DBAdapter.DB.Update(existing);
        }

        public static void DisconnectFromAssetStore(AssetInfo info, bool removeMetadata)
        {
            Asset existing = DBAdapter.DB.Find<Asset>(info.AssetId);
            if (existing == null) return;

            existing.ForeignId = 0;
            info.ForeignId = 0;

            if (removeMetadata)
            {
                existing.AssetRating = null;
                info.AssetRating = null;
                existing.SafePublisher = null;
                info.SafePublisher = null;
                existing.DisplayPublisher = null;
                info.DisplayPublisher = null;
                existing.SafeCategory = null;
                info.SafeCategory = null;
                existing.DisplayCategory = null;
                info.DisplayCategory = null;
                existing.DisplayName = null;
                info.DisplayName = null;
                existing.OfficialState = null;
                info.OfficialState = null;
            }

            DBAdapter.DB.Update(existing);
        }

        public static string CreateDebugReport()
        {
            string result = "Debug Support Report\n";
            result += $"\nDate: {DateTime.Now}";
            result += $"\nVersion: {TOOL_VERSION}";
            result += $"\nUnity: {Application.unityVersion}";
            result += $"\nPlatform: {Application.platform}";
            result += $"\nOS: {Environment.OSVersion}";
            result += $"\nLanguage: {Application.systemLanguage}";

            List<AssetInfo> assets = LoadAssets();
            result += $"\n\n{assets.Count} Packages";
            foreach (AssetInfo asset in assets)
            {
                result += $"\n{asset} ({asset.SafeName}) - {asset.AssetSource} - {asset.Version}";
            }

            List<Tag> tags = LoadTags();
            result += $"\n\n{tags.Count} Tags";
            foreach (Tag tag in tags)
            {
                result += $"\n{tag}";
            }

            return result;
        }
    }
}