using System;

namespace AssetInventory
{
    [Serializable]
    public sealed class FolderSpec
    {
        public int folderType; // 0 = packages, 1 = media, 2 = zip
        public int scanFor; // 0 = all, 2 = audio, 3 = images, 4 = models, 6 = pattern
        public bool enabled = true;
        public string location;
        public string pattern;
        public bool createPreviews = true;
        public bool preferPackages = true;
    }
}