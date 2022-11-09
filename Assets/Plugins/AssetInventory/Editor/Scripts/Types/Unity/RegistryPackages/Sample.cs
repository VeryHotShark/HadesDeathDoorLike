using System;

namespace AssetInventory
{
    [Serializable]
    public sealed class Sample
    {
        public string displayName;
        public string description;
        public string path;

        public override string ToString()
        {
            return $"Package Sample '{displayName}' ({path})";
        }
    }
}