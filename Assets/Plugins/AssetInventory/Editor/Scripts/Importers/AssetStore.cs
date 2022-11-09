using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace AssetInventory
{
    public static class AssetStore
    {
        private const string URL_PURCHASES = "https://packages-v2.unity.com/-/api/purchases";
        private const string URL_TOKEN_INFO = "https://api.unity.com/v1/oauth2/tokeninfo?access_token=";
        private const string URL_USER_INFO = "https://api.unity.com/v1/users";
        private const string URL_ASSET_DETAILS = "https://packages-v2.unity.com/-/api/product";
        private const string URL_ASSET_DOWNLOAD = "https://packages-v2.unity.com/-/api/legacy-package-download-info";
        private const int PAGE_SIZE = 100; // more is not supported by Asset Store

        private static ListRequest _listRequest;
        private static SearchRequest _searchRequest;
        private static List<PackageInfo> _allPackages;
        private static PackageCollection _projectPackages;
        private static List<string> _failedPackages;
        private static string _currentSearch;
        private static bool _offlineMode;

        public static bool CancellationRequested { get; set; }

        public static async Task<AssetPurchases> RetrievePurchases()
        {
            AssetInventory.CurrentMain = "Phase 1/3: Fetching purchases";
            AssetInventory.MainCount = 1;
            AssetInventory.MainProgress = 1;
            int progressId = MetaProgress.Start("Fetching purchases");

            string token = CloudProjectSettings.accessToken;
            AssetPurchases result = await AssetUtils.FetchAPIData<AssetPurchases>($"{URL_PURCHASES}?offset=0&limit={PAGE_SIZE}", token);

            // if more results than page size retrieve rest as well and merge
            // doing all requests in parallel is not possible with Unity's web client since they can only run on the main thread
            if (result != null && result.total > PAGE_SIZE)
            {
                int pageCount = AssetUtils.GetPageCount(result.total, PAGE_SIZE) - 1;
                AssetInventory.MainCount = pageCount + 1;
                for (int i = 1; i <= pageCount; i++)
                {
                    AssetInventory.MainProgress = i + 1;
                    MetaProgress.Report(progressId, i + 1, pageCount + 1, string.Empty);
                    if (CancellationRequested) break;

                    AssetPurchases pageResult = await AssetUtils.FetchAPIData<AssetPurchases>($"{URL_PURCHASES}?offset={i * PAGE_SIZE}&limit={PAGE_SIZE}", token);
                    if (pageResult != null)
                    {
                        result.results.AddRange(pageResult.results);
                    }
                    else
                    {
                        Debug.LogError("Could only retrieve a partial list of asset purchases. Try again later.");
                    }
                }
            }

            AssetInventory.CurrentMain = null;
            MetaProgress.Remove(progressId);

            return result;
        }

        public static async Task<AssetDetails> RetrieveAssetDetails(int id, string eTag = null)
        {
            string token = CloudProjectSettings.accessToken;
            string newEtag = eTag;
            AssetDetails result = await AssetUtils.FetchAPIData<AssetDetails>($"{URL_ASSET_DETAILS}/{id}", token, eTag, newCacheTag => newEtag = newCacheTag);
            if (result != null) result.ETag = newEtag;

            return result;
        }

        public static async Task<DownloadInfo> RetrieveAssetDownloadInfo(int id)
        {
            string token = CloudProjectSettings.accessToken;
            DownloadInfoResult result = await AssetUtils.FetchAPIData<DownloadInfoResult>($"{URL_ASSET_DOWNLOAD}/{id}", token);

            // special handling of "." also in AssetStoreDownloadInfo 
            if (result?.result?.download != null)
            {
                result.result.download.filename_safe_category_name = result.result.download.filename_safe_category_name.Replace(".", string.Empty);
                result.result.download.filename_safe_package_name = result.result.download.filename_safe_package_name.Replace(".", string.Empty);
                result.result.download.filename_safe_publisher_name = result.result.download.filename_safe_publisher_name.Replace(".", string.Empty);
            }

            return result?.result?.download;
        }

        public static void GatherProjectMetadata()
        {
            _listRequest = Client.List();
            EditorApplication.update += ListProgress;
        }

        private static void ListProgress()
        {
            if (!_listRequest.IsCompleted) return;
            EditorApplication.update -= ListProgress;

            if (_listRequest.Status == StatusCode.Success)
            {
                _projectPackages = _listRequest.Result;
            }
        }

        public static void GatherAllMetadata()
        {
            _searchRequest = Client.SearchAll(_offlineMode);
            EditorApplication.update += SearchProgress;
        }

        private static void SearchProgress()
        {
            if (!_searchRequest.IsCompleted) return;
            EditorApplication.update -= SearchProgress;

            if (_searchRequest.Status == StatusCode.Success)
            {
                _allPackages = _searchRequest.Result?.ToList();
            }
            else if (_searchRequest.Status == StatusCode.Failure)
            {
                // fallback to offline mode
                if (!_offlineMode)
                {
                    _offlineMode = true;
                    GatherAllMetadata();
                }
            }
        }

        private static void IncrementalSearchProgress()
        {
            if (!_searchRequest.IsCompleted) return;
            EditorApplication.update -= IncrementalSearchProgress;

            if (_searchRequest.Status == StatusCode.Success && _searchRequest.Result != null)
            {
                if (_allPackages == null) _allPackages = new List<PackageInfo>();
                _allPackages.AddRange(_searchRequest.Result);
            }
            else
            {
                if (_failedPackages == null) _failedPackages = new List<string>();
                _failedPackages.Add(_currentSearch);
            }
            _currentSearch = null;
        }

        public static bool IsMetadataAvailable(bool considerSearch = true)
        {
            FillBufferOnDemand();

            if (considerSearch && _searchRequest != null && !_searchRequest.IsCompleted) return false;
            return _projectPackages != null && (_allPackages != null || (_searchRequest != null && _searchRequest.IsCompleted));
        }

        public static PackageInfo GetPackageInfo(string name)
        {
            FillBufferOnDemand();

            PackageInfo result = _projectPackages?.FirstOrDefault(p => p.name == name);
            if (result == null) result = _allPackages?.FirstOrDefault(p => p.name == name);
            if (result == null && (_searchRequest == null || _searchRequest.IsCompleted) && (_failedPackages == null || !_failedPackages.Contains(name)))
            {
                _currentSearch = name;
                _searchRequest = Client.Search(name);
                EditorApplication.update += IncrementalSearchProgress;
            }

            return result;
        }

        public static PackageCollection GetProjectPackages()
        {
            FillBufferOnDemand();

            return _projectPackages;
        }

        public static void FillBufferOnDemand()
        {
            if (_projectPackages == null && _listRequest == null) GatherProjectMetadata();
            if (_allPackages == null && _searchRequest == null) GatherAllMetadata();
        }

        public static bool IsInstalled(AssetInfo info)
        {
            if (info.AssetSource != Asset.Source.Package) return false;

            FillBufferOnDemand();

            return IsInstalled(info.SafeName, info.GetVersionToUse());
        }

        public static bool IsInstalled(string name, string version)
        {
            PackageInfo result = _projectPackages?.FirstOrDefault(p => p.name == name);
            return result != null && result.version == version;
        }

        public static void OpenInPackageManager(AssetInfo info)
        {
            if (info.ForeignId > 0)
            {
                OpenAssetInPackageManager(info.GetItemLink());
            }
            else if (info.AssetSource == Asset.Source.Package)
            {
                OpenPackageInPackageManager(info.SafeName);
            }
        }

        public static void OpenAssetInPackageManager(string url)
        {
#if UNITY_2020_1_OR_NEWER
            Assembly assembly = Assembly.Load("UnityEditor.PackageManagerUIModule");
            Type asc = assembly.GetType("UnityEditor.PackageManager.UI.PackageManagerWindow");
            MethodInfo openURL = asc.GetMethod("OpenURL", BindingFlags.NonPublic | BindingFlags.Static);

            openURL?.Invoke(null, new object[] {url});
#else
            // loading assembly will fail below 2020
            OpenPackageInPackageManager(url);
#endif
        }

        public static void OpenPackageInPackageManager(string id)
        {
            UnityEditor.PackageManager.UI.Window.Open(id);
        }
    }
}