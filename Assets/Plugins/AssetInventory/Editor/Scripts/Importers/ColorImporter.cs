using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AssetInventory
{
    public sealed class ColorImporter : AssertImporter
    {
        public IEnumerator Index()
        {
            ResetState(false);
            int progressId = MetaProgress.Start("Extracting color information");

            string previewFolder = AssetInventory.GetPreviewFolder();

            List<AssetFile> files = DBAdapter.DB.Table<AssetFile>()
                .Where(a => a.PreviewFile != null && a.DominantColor == null)
                .ToList();

            SubCount = files.Count;
            for (int i = 0; i < files.Count; i++)
            {
                if (CancellationRequested) break;
                yield return Cooldown.DoCo();

                AssetFile file = files[i];
                MetaProgress.Report(progressId, i + 1, files.Count, file.FileName);

                string previewFile = Path.Combine(previewFolder, file.PreviewFile);
                if (!File.Exists(previewFile)) continue;

                CurrentSub = $"Extracting colors from {file.FileName}";
                SubProgress = i + 1;

                yield return AssetUtils.LoadTexture(previewFile, result =>
                {
                    if (result == null) return;

                    Color mostUsedColor = ColorUtils.GetMostUsedColor(result);
                    Color colorGroup = ColorUtils.GetNearestColor(mostUsedColor);

                    file.DominantColor = "#" + ColorUtility.ToHtmlStringRGB(mostUsedColor);
                    file.DominantColorGroup = "#" + ColorUtility.ToHtmlStringRGB(colorGroup);

                    Persist(file);
                });
            }
            MetaProgress.Remove(progressId);
            ResetState(true);
        }
    }
}