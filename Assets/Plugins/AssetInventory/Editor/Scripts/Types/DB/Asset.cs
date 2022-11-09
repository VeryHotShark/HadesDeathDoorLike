using System;
using System.Globalization;
using Newtonsoft.Json;
using SQLite;
using UnityEditor.PackageManager;

namespace AssetInventory
{
    [Serializable]
    public sealed class Asset
    {
        public const string NONE = "-no attached package-";

        public enum State
        {
            New = 0,
            InProcess = 1,
            Done = 2,
            Unknown = 3
        }

        public enum SubState
        {
            None = 0,
            Outdated = 1
        }

        public enum Source
        {
            AssetStorePackage = 0,
            CustomPackage = 1,
            Directory = 2,
            Package = 3,
            Archive = 4
        }

        [PrimaryKey, AutoIncrement] public int Id { get; set; }
        public Source AssetSource { get; set; }
        public string Location { get; set; }
        public string OriginalLocation { get; set; }
        public string OriginalLocationKey { get; set; }
        public string Registry { get; set; }
        public string Repository { get; set; }
        public PackageSource PackageSource { get; set; }
        [Indexed] public int ForeignId { get; set; }
        public long PackageSize { get; set; }
        public string PreviewImage { get; set; }
        [Indexed] public string SafeName { get; set; }
        public string DisplayName { get; set; }
        [Indexed] public string SafePublisher { get; set; }
        public string DisplayPublisher { get; set; }
        [Indexed] public string SafeCategory { get; set; }
        public string DisplayCategory { get; set; }
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

        // SPDX identifier format: https://spdx.org/licenses/
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
        public State CurrentState { get; set; }
        public SubState CurrentSubState { get; set; }

        public Asset()
        {
        }

        public Asset(Package package)
        {
            CopyFrom(package);
        }

        public Asset(PackageInfo package)
        {
            CopyFrom(package);
        }

        public string GetCalculatedLocation()
        {
            if (string.IsNullOrEmpty(SafePublisher) || string.IsNullOrEmpty(SafeCategory) || string.IsNullOrEmpty(SafeName)) return null;

            return System.IO.Path.Combine(AssetInventory.GetAssetDownloadPath(), SafePublisher, SafeCategory, SafeName + ".unitypackage");
        }

        public Asset CopyFrom(PackageInfo package)
        {
            AssetSource = Source.Package;
            DisplayName = package.displayName;
            Description = package.description;
            SafeCategory = package.type;
            if (!string.IsNullOrEmpty(package.type))
            {
                TextInfo ti = new CultureInfo("en-US", false).TextInfo;
                DisplayCategory = ti.ToTitleCase(package.type);
            }
            SafeName = package.name;
            SafePublisher = package.author?.name;
#if UNITY_2020_1_OR_NEWER
            ReleaseNotes = package.changelogUrl;
            LicenseLocation = package.licensesUrl;
#endif
            if (!string.IsNullOrEmpty(package.version))
            {
                // only set to higher versions, as otherwise there might be import ping-pong situations
                if (new SemVer(package.version) > new SemVer(Version))
                {
                    Version = package.version;
                }
            }
            if (package.keywords != null) Keywords = string.Join(", ", package.keywords);
            PackageSource = package.source;
            LastRelease = package.datePublished ?? DateTime.MinValue;

            if (string.IsNullOrWhiteSpace(SafePublisher) && package.registry != null && package.registry.isDefault) SafePublisher = "Unity";

            // registry
            if (package.registry != null)
            {
                if (package.registry.isDefault)
                {
                    Registry = "Unity";
                }
                else
                {
                    ScopedRegistry scopedReg = new ScopedRegistry(package.registry);
                    Registry = JsonConvert.SerializeObject(scopedReg);
                }
            }

#if UNITY_2020_1_OR_NEWER
            // repository
            if (package.repository != null)
            {
                Repository repo = new Repository(package.repository);
                if (string.IsNullOrEmpty(repo.revision) && !string.IsNullOrWhiteSpace(package.git?.revision)) repo.revision = package.git.revision;
                Repository = JsonConvert.SerializeObject(repo);
            }
#endif
            // additional source specific settings
            switch (package.source)
            {
                case PackageSource.Git:
                case PackageSource.Embedded:
                case PackageSource.LocalTarball:
                    Location = package.resolvedPath;
                    break;
            }

            return this;
        }

        public Asset CopyFrom(Package package)
        {
            AssetSource = Source.Package;
            DisplayName = package.displayName;
            Description = package.description;
            SafeCategory = package.type;
            if (!string.IsNullOrEmpty(package.type))
            {
                TextInfo ti = new CultureInfo("en-US", false).TextInfo;
                DisplayCategory = ti.ToTitleCase(package.type);
            }
            SafeName = package.name;
            SafePublisher = package.author?.name;
            ReleaseNotes = package.changelogUrl;
            License = package.license;
            LicenseLocation = package.licensesUrl;
            SupportedUnityVersions = package.unity;
            if (!string.IsNullOrEmpty(package.version))
            {
                // only set to higher versions, as otherwise there might be import ping-pong situations
                if (new SemVer(package.version) > new SemVer(Version))
                {
                    Version = package.version;
                }
            }
            if (package.keywords != null) Keywords = string.Join(", ", package.keywords);
            IsHidden = package.hideInEditor;

            return this;
        }

        public override string ToString()
        {
            string name = string.IsNullOrEmpty(DisplayName) ? SafeName : DisplayName;
            return $"Package '{name}' ({Location})";
        }

        public static Asset GetNoAsset()
        {
            Asset noAsset = new Asset();
            noAsset.SafeName = NONE;
            noAsset.DisplayName = NONE;
            noAsset.AssetSource = Source.Directory;

            return noAsset;
        }

        public string GetSafeVersion()
        {
            return Version.Replace("/", "").Replace("\\", "");
        }
    }
}