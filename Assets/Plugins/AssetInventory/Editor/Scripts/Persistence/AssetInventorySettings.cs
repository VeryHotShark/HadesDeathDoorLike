using System;
using System.Collections.Generic;

namespace AssetInventory
{
    [Serializable]
    public sealed class AssetInventorySettings
    {
        public int sortField;
        public bool sortDescending;
        public int maxResults = 5;
        public int tileText;
        public bool allowEasyMode = true;
        public bool autoPlayAudio = true;
        public bool pingSelected = true;
        public bool doubleClickImport;
        public bool groupLists = true;
        public bool onlyWithPreview;
        public int tileSize = 150;
        public float searchDelay = 0.3f;
        public string excludedExtensions = "asset;json;txt;cs;md;uss;asmdef;ttf;uxml";

        public bool showFilterBar;
        public bool showPackageFilterBar;
        public bool showDetailFilters = true;
        public bool showSavedSearches = true;

        public bool indexAssetStore = true;
        public bool indexPackageCache = true;
        public bool gatherExtendedMetadata = true;
        public bool extractPreviews = true;
        public bool extractColors;
        public bool excludeByDefault;
        public bool indexAssetPackageContents = true;
        public bool indexPackageContents = true;
        public bool showIconsForMissingPreviews = true;
        public bool importPackageKeywordsAsTags;
        public string customStorageLocation;

        public bool useCooldown = true;
        public int cooldownInterval = 10; // minutes
        public int cooldownDuration = 60; // seconds

        public bool createBackups;
        public bool onlyLatestPatchVersion = true;
        public int backupsPerAsset = 5;
        public string backupFolder;

        public int assetGrouping;
        public int assetDeprecation;
        public int packagesListing = 1; // only assets per default

        public List<FolderSpec> folders = new List<FolderSpec>();
        public List<SavedSearch> searches = new List<SavedSearch>();
    }
}