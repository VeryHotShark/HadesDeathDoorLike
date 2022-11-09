using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.PackageManager;
using UnityEngine;

namespace AssetInventory
{
    [Serializable]
    // used to contain results of join calls
    public sealed class AssetInfo : AssetFile
    {
        public enum ImportStateOptions
        {
            Unknown = 0,
            Queued = 1,
            Missing = 2,
            Importing = 3,
            Imported = 4,
            Failed = 5
        }

        public enum DependencyStateOptions
        {
            Unknown = 0,
            Calculating = 1,
            Done = 2,
            NotPossible = 3
        }

        public Asset.Source AssetSource { get; set; }
        public string Location { get; set; }
        public string OriginalLocation { get; set; }
        public string OriginalLocationKey { get; set; }
        public string Registry { get; set; }
        public string Repository { get; set; }
        public PackageSource PackageSource { get; set; }
        public int ForeignId { get; set; }
        public long PackageSize { get; set; }
        public string SafeName { get; set; }
        public string DisplayName { get; set; }
        public string SafePublisher { get; set; }
        public string DisplayPublisher { get; set; }
        public string SafeCategory { get; set; }
        public string DisplayCategory { get; set; }
        public string PreviewImage { get; set; }
        public Asset.State CurrentState { get; set; }
        public Asset.SubState CurrentSubState { get; set; }
        public string Slug { get; set; }
        public int Revision { get; set; }
        public string Description { get; set; }
        public string KeyFeatures { get; set; }
        public string CompatibilityInfo { get; set; }
        public string SupportedUnityVersions { get; set; }
        public string Keywords { get; set; }
        public string Version { get; set; }
        public string LatestVersion { get; set; }
        public string PreferredVersion { get; set; }
        public string License { get; set; }
        public string LicenseLocation { get; set; }
        public DateTime LastRelease { get; set; }
        public string AssetRating { get; set; }
        public int RatingCount { get; set; }
        public string MainImage { get; set; }
        public string MainImageSmall { get; set; }
        public string MainImageIcon { get; set; }
        public string Requirements { get; set; }
        public string ReleaseNotes { get; set; }
        public string OfficialState { get; set; }
        public bool IsHidden { get; set; }
        public bool Exclude { get; set; }
        public string ETag { get; set; }
        public DateTime LastOnlineRefresh { get; set; }
        public int FileCount { get; set; }
        public long UncompressedSize { get; set; }

        // runtime only
        public AssetDownloader PackageDownloader;
        public Texture2D PreviewTexture { get; set; }
        public string ProjectPath { get; set; }
        public bool InProject => !string.IsNullOrWhiteSpace(ProjectPath);
        public bool IsIndexed => AssetSource == Asset.Source.Directory || (FileCount > 0 && CurrentState == Asset.State.Done);
        public bool IsDeprecated => OfficialState == "deprecated";
        public bool IsAbandoned => OfficialState == "disabled";
        public bool IsMaterialized { get; set; }
        public ImportStateOptions ImportState { get; set; }
        public DependencyStateOptions DependencyState { get; set; } = DependencyStateOptions.Unknown;
        public List<AssetFile> Dependencies { get; set; }
        public List<AssetFile> MediaDependencies { get; set; }
        public List<AssetFile> ScriptDependencies { get; set; }
        public long DependencySize { get; set; }
        public bool WasOutdated { get; set; }

        private bool _tagsDone;
        private List<TagInfo> _packageTags;
        private List<TagInfo> _assetTags;
        private int _tagHash;
        private string _versionsCached;
        private bool _versionComparisonResult;

        public List<TagInfo> PackageTags
        {
            get
            {
                EnsureTagsLoaded();
                return _packageTags;
            }
        }

        public void SetTagsDirty() => _tagsDone = false;

        public List<TagInfo> AssetTags
        {
            get
            {
                EnsureTagsLoaded();
                return _assetTags;
            }
        }

        public bool Downloaded
        {
            get
            {
                if (AssetSource == Asset.Source.Package) return true;
                if (SafeName == Asset.NONE) return true;
                if (string.IsNullOrEmpty(Location)) return false;
                if (_downloaded == null) _downloaded = File.Exists(Location);
                return _downloaded.Value;
            }
        }

        private bool? _downloaded;

        private void EnsureTagsLoaded()
        {
            if (!_tagsDone || AssetInventory.TagHash != _tagHash)
            {
                _assetTags = AssetInventory.GetAssetTags(Id);
                _packageTags = AssetInventory.GetPackageTags(AssetId);
                _tagsDone = true;
                _tagHash = AssetInventory.TagHash;
            }
        }

        public string GetVersionToUse() => string.IsNullOrEmpty(PreferredVersion) ? Version : PreferredVersion;

        public string GetDisplayName(bool extended = true)
        {
            string result = string.IsNullOrEmpty(DisplayName) ? SafeName : DisplayName;
            if (extended && AssetSource == Asset.Source.Package && !string.IsNullOrWhiteSpace(GetVersionToUse())) result += " - " + GetVersionToUse();
            return result;
        }

        public string GetDisplayPublisher() => string.IsNullOrEmpty(DisplayPublisher) ? SafePublisher : DisplayPublisher;
        public string GetDisplayCategory() => string.IsNullOrEmpty(DisplayCategory) ? SafeCategory : DisplayCategory;

        public string GetChangeLog(string versionOverride = null)
        {
            if (string.IsNullOrWhiteSpace(ReleaseNotes) && Registry == "Unity")
            {
                SemVer version = new SemVer(string.IsNullOrEmpty(versionOverride) ? Version : versionOverride);
                return $"https://docs.unity3d.com/Packages/{SafeName}@{version.Major}.{version.Minor}/changelog/CHANGELOG.html";
            }
            return ReleaseNotes;
        }

        public string GetChangeLogURL(string versionOverride = null)
        {
            string changeLog = GetChangeLog(versionOverride);

            return AssetUtils.IsUrl(changeLog) ? changeLog : null;
        }

        public bool IsUpdateAvailable()
        {
            if (WasOutdated) return false;
            if (IsAbandoned || IsDeprecated) return false;
            if (string.IsNullOrWhiteSpace(Version) || string.IsNullOrWhiteSpace(LatestVersion)) return false;

            // this can be called thousands of times per frame, needs caching
            string cache = Version + LatestVersion;
            if (_versionsCached == cache) return _versionComparisonResult;

            _versionsCached = cache;
            _versionComparisonResult = new SemVer(Version) < new SemVer(LatestVersion);

            return _versionComparisonResult;
        }

        public bool IsUpdateAvailable(List<AssetInfo> assets)
        {
            bool isOlderVersion = IsUpdateAvailable();
            if (isOlderVersion && assets != null)
            {
                // if asset in that version is already loaded don't flag as update available
                if (assets.Any(a => a.ForeignId == ForeignId && a.Version == LatestVersion && !string.IsNullOrEmpty(a.Location))) return false;
            }
            return isOlderVersion;
        }

        public bool IsDownloading()
        {
            return PackageDownloader != null && PackageDownloader.GetState().state == AssetDownloader.State.Downloading;
        }

        public AssetInfo WithTreeData(string name, int id = 0, int depth = 0)
        {
            m_Name = name;
            m_ID = id;
            m_Depth = depth;

            return this;
        }

        public AssetInfo WithTreeId(int id)
        {
            m_ID = id;

            return this;
        }

        public AssetInfo WithProjectPath(string path)
        {
            ProjectPath = path;

            return this;
        }

        public Asset ToAsset()
        {
            return new Asset
            {
                AssetSource = AssetSource,
                DisplayCategory = DisplayCategory,
                SafeCategory = SafeCategory,
                CurrentState = CurrentState,
                CurrentSubState = CurrentSubState,
                Id = AssetId,
                Slug = Slug,
                Revision = Revision,
                Description = Description,
                KeyFeatures = KeyFeatures,
                CompatibilityInfo = CompatibilityInfo,
                SupportedUnityVersions = SupportedUnityVersions,
                Keywords = Keywords,
                Version = Version,
                LatestVersion = LatestVersion,
                License = License,
                LicenseLocation = LicenseLocation,
                LastRelease = LastRelease,
                AssetRating = AssetRating,
                RatingCount = RatingCount,
                MainImage = MainImage,
                Requirements = Requirements,
                ReleaseNotes = ReleaseNotes,
                OfficialState = OfficialState,
                IsHidden = IsHidden,
                Exclude = Exclude,
                ETag = ETag,
                Location = Location,
                OriginalLocation = OriginalLocation,
                OriginalLocationKey = OriginalLocationKey,
                ForeignId = ForeignId,
                SafeName = SafeName,
                DisplayName = DisplayName,
                PackageSize = PackageSize,
                SafePublisher = SafePublisher,
                DisplayPublisher = DisplayPublisher
            };
        }

        public string GetItemLink()
        {
            return $"https://assetstore.unity.com/packages/slug/{ForeignId}";
        }

        public void Refresh()
        {
            _downloaded = null;
            WasOutdated = false;
        }

        public override string ToString()
        {
            return $"Asset Info '{GetDisplayName()}'";
        }
    }
}