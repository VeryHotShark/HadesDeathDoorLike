using System.IO;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace AssetInventory
{
    public sealed class ZipImporter : AssertImporter
    {
        private const int BREAK_INTERVAL = 30;

        public async Task Index(FolderSpec spec)
        {
            ResetState(false);

            if (string.IsNullOrEmpty(spec.location)) return;

            string[] files = Directory.GetFiles(spec.location, "*.zip", SearchOption.AllDirectories);
            MainCount = files.Length;
            MainProgress = 1; // small hack to trigger UI update in the end

            int progressId = MetaProgress.Start("Updating zip files index");

            for (int i = 0; i < files.Length; i++)
            {
                if (CancellationRequested) break;
                if (i % BREAK_INTERVAL == 0) await Task.Yield(); // let editor breath in case many files are already indexed
                await Cooldown.Do();

                string package = files[i];

                Asset asset = new Asset();
                asset.SafeName = Path.GetFileNameWithoutExtension(package);
                asset.AssetSource = Asset.Source.Archive;
                asset.Location = package;

                Asset existing = DBAdapter.DB.Table<Asset>().FirstOrDefault(a => a.Location == asset.Location);
                long size; // determine late for performance, especially with many exclusions
                if (existing != null)
                {
                    if (existing.Exclude) continue;

                    size = new FileInfo(package).Length;
                    if (existing.CurrentState == Asset.State.Done && existing.PackageSize == size && existing.Location == package) continue;

                    asset = existing;
                }
                else
                {
                    size = new FileInfo(package).Length;
                }

                MetaProgress.Report(progressId, i + 1, files.Length, package);
                CurrentMain = package + " (" + EditorUtility.FormatBytes(size) + ")";
                MainProgress = i + 1;
                await Task.Yield();

                asset.PackageSize = size;
                Persist(asset);

                await IndexPackage(asset);

                asset.CurrentState = Asset.State.Done;
                Persist(asset);
            }
            MetaProgress.Remove(progressId);
            ResetState(true);
        }

        private async Task IndexPackage(Asset asset)
        {
            string tempPath = await AssetInventory.ExtractAsset(asset);
            if (string.IsNullOrEmpty(tempPath))
            {
                Debug.LogError($"{asset} could not be indexed due to issues extracting it: {asset.Location}");
                return;
            }

            FolderSpec importSpec = GetDefaultImportSpec();
            importSpec.location = tempPath;
            await new MediaImporter().Index(importSpec, asset, true, true);
        }
    }
}