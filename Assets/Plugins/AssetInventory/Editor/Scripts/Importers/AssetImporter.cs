using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace AssetInventory
{
    public abstract class AssertImporter : AssertProgress
    {
        protected Asset Fetch(Asset asset)
        {
            if (asset.AssetSource == Asset.Source.Package)
            {
                return DBAdapter.DB.Find<Asset>(a => a.SafeName == asset.SafeName);
            }
            if (asset.AssetSource == Asset.Source.Archive)
            {
                return DBAdapter.DB.Find<Asset>(a => a.Location == asset.Location);
            }

            // return non-deprecated version first (deprecated < published in sorting)
            // use location as main key
            Asset result = null;
            if (!string.IsNullOrEmpty(asset.Location))
            {
                result = DBAdapter.DB.Table<Asset>()
                    .Where(a => a.Location == asset.Location)
                    .OrderBy(a => a.OfficialState)
                    .LastOrDefault();
            }

            if (result == null && asset.ForeignId > 0)
            {
                // use more specific data if available to differentiate between multi-version assets
                result = DBAdapter.DB.Table<Asset>()
                    .Where(a => a.ForeignId == asset.ForeignId && a.Location == asset.Location)
                    .OrderBy(a => a.OfficialState)
                    .LastOrDefault();
            }

            if (result == null)
            {
                result = DBAdapter.DB.Table<Asset>()
                    .Where(a => a.SafeName == asset.SafeName && a.SafeCategory == asset.SafeCategory && a.SafePublisher == asset.SafePublisher)
                    .OrderBy(a => a.OfficialState)
                    .LastOrDefault();
            }
            return result;
        }

        protected bool Exists(AssetFile file)
        {
            if (string.IsNullOrEmpty(file.Guid))
            {
                return DBAdapter.DB.ExecuteScalar<int>("select count(*) from AssetFile where Path == ? and AssetId == ? limit 1", file.Path, file.AssetId) > 0;
            }
            return DBAdapter.DB.ExecuteScalar<int>("select count(*) from AssetFile where Guid == ? and AssetId == ? limit 1", file.Guid, file.AssetId) > 0;
        }

        protected AssetFile Fetch(AssetFile file)
        {
            if (string.IsNullOrEmpty(file.Guid))
            {
                return DBAdapter.DB.Find<AssetFile>(f => f.Path == file.Path && f.AssetId == file.AssetId);
            }
            return DBAdapter.DB.Find<AssetFile>(f => f.Guid == file.Guid && f.AssetId == file.AssetId);
        }

        protected void Persist(Asset asset)
        {
            if (asset.Id > 0)
            {
                DBAdapter.DB.Update(asset);
                return;
            }

            Asset existing = Fetch(asset);
            if (existing != null)
            {
                asset.Id = existing.Id;
                if (asset.ForeignId > 0) existing.ForeignId = asset.ForeignId;
                existing.Version = asset.Version;
                existing.SafeCategory = asset.SafeCategory;
                existing.SafePublisher = asset.SafePublisher;
                existing.CurrentState = asset.CurrentState;
                existing.AssetSource = asset.AssetSource;
                existing.PackageSize = asset.PackageSize;
                existing.Location = asset.Location;
                existing.PreviewImage = asset.PreviewImage;

                DBAdapter.DB.Update(existing);
            }
            else
            {
                DBAdapter.DB.Insert(asset);
            }
        }

        protected void Persist(AssetFile file)
        {
            if (file.Id > 0)
            {
                DBAdapter.DB.Update(file);
                return;
            }

            AssetFile existing = Fetch(file);
            if (existing != null)
            {
                file.Id = existing.Id;
                DBAdapter.DB.Update(file);
            }
            else
            {
                DBAdapter.DB.Insert(file);
            }
        }

        protected void UpdateOrInsert(Asset asset)
        {
            if (asset.Id > 0)
            {
                DBAdapter.DB.Update(asset);
            }
            else
            {
                DBAdapter.DB.Insert(asset);
            }
        }

        protected async Task ProcessMediaAttributes(string file, AssetFile af, Asset asset)
        {
            // special processing for supported file types
            if (af.Type == "png" || af.Type == "jpg")
            {
                Texture2D tmpTexture = new Texture2D(1, 1);
                try
                {
                    byte[] assetContent = File.ReadAllBytes(file);
                    if (tmpTexture.LoadImage(assetContent))
                    {
                        af.Width = tmpTexture.width;
                        af.Height = tmpTexture.height;
                    }
                }
                catch (Exception e)
                {
                    // can happen if resulting path is longer than system limit
                    Debug.LogWarning($"Could not process media file '{Path.GetFileName(file)}' from {af}: {e.Message}");
                }
            }

            if (AssetInventory.IsFileType(af.FileName, "Audio"))
            {
                string contentFile = asset.AssetSource != Asset.Source.Directory ? await AssetInventory.EnsureMaterializedAsset(asset, af) : file;
                try
                {
                    AudioClip clip = await AssetUtils.LoadAudioFromFile(contentFile);
                    if (clip != null) af.Length = clip.length;
                }
                catch
                {
                    Debug.LogWarning($"Audio file '{Path.GetFileName(file)}' from {af} seems to have incorrect format.");
                }
            }
        }

        protected FolderSpec GetDefaultImportSpec()
        {
            return new FolderSpec
            {
                pattern = "*.*",
                createPreviews = true,
                folderType = 1,
                scanFor = 6
            };
        }
    }
}