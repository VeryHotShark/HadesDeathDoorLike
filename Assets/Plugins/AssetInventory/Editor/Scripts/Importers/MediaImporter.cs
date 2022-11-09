using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace AssetInventory
{
    public sealed class MediaImporter : AssertImporter
    {
        private const int BREAK_INTERVAL = 30;

        public async Task Index(FolderSpec spec, Asset attachedAsset = null, bool storeRelativePath = false, bool actAsSubImporter = false)
        {
            if (!actAsSubImporter) ResetState(false);

            if (string.IsNullOrEmpty(spec.location)) return;

            List<string> searchPatterns = new List<string>();
            List<string> types = new List<string>();
            switch (spec.scanFor)
            {
                case 0:
                    types.AddRange(new[] {"Audio", "Images", "Models"});
                    break;

                case 2:
                    types.Add("Audio");
                    break;

                case 3:
                    types.Add("Images");
                    break;

                case 4:
                    types.Add("Models");
                    break;

                case 6:
                    if (!string.IsNullOrWhiteSpace(spec.pattern)) searchPatterns.AddRange(spec.pattern.Split(';'));
                    break;
            }
            types.ForEach(t => searchPatterns.AddRange(AssetInventory.TypeGroups[t].Select(ext => $"*.{ext}")));

            string[] files = IOUtils.GetFiles(spec.location, searchPatterns, SearchOption.AllDirectories).ToArray();
            SubCount = files.Length;
            if (!actAsSubImporter) MainProgress = 1; // small hack to trigger UI update in the end
            if (spec.createPreviews) PreviewGenerator.Init(files.Length);

            int progressId = MetaProgress.Start(actAsSubImporter ? "Updating files index" : "Updating media files index");

            if (attachedAsset == null)
            {
                attachedAsset = DBAdapter.DB.Find<Asset>(a => a.SafeName == Asset.NONE);
                if (attachedAsset == null)
                {
                    attachedAsset = Asset.GetNoAsset();
                    Persist(attachedAsset);
                }
            }
            string previewPath = AssetInventory.GetPreviewFolder();

            for (int i = 0; i < files.Length; i++)
            {
                if (CancellationRequested) break;
                if (i % BREAK_INTERVAL == 0) await Task.Yield(); // let editor breath in case many files are already indexed
                await Cooldown.Do();

                string file = files[i];
                string metaFile = $"{file}.meta";
                string type = Path.GetExtension(file).Replace(".", string.Empty).ToLowerInvariant();
                if (type == "meta") continue; // never index .meta files

                MetaProgress.Report(progressId, i + 1, files.Length, file);
                CurrentSub = file;
                SubProgress = i + 1;

                AssetFile af = new AssetFile();
                af.AssetId = attachedAsset.Id;
                af.SourcePath = file;
                af.Path = storeRelativePath ? file.Substring(spec.location.Length + 1) : file;
                if (File.Exists(metaFile)) af.Guid = AssetUtils.ExtractGuidFromFile(metaFile);

                if (Exists(af)) continue;

                long size = new FileInfo(file).Length;
                CurrentSub = file + " (" + EditorUtility.FormatBytes(size) + ")";
                await Task.Yield(); // let editor breath

                af.FileName = Path.GetFileName(af.SourcePath);
                af.Size = size;
                af.Type = type;
                if (AssetInventory.Config.gatherExtendedMetadata)
                {
                    await ProcessMediaAttributes(file, af, attachedAsset); // must be run on main thread
                }
                Persist(af);

                if (spec.createPreviews && (AssetInventory.IsFileType(af.FileName, "Audio") || AssetInventory.IsFileType(af.FileName, "Images") || AssetInventory.IsFileType(af.FileName, "Models")))
                {
                    // let Unity generate a preview for whitelisted types (CS and ASMDEF will trigger recompile and fail the indexer) 
                    string previewFile = Path.Combine(previewPath, "af-" + af.Id + ".png");

                    PreviewGenerator.RegisterPreviewRequest(af.Id, af.SourcePath, previewFile, req =>
                    {
                        if (!File.Exists(req.DestinationFile)) return;
                        AssetFile paf = DBAdapter.DB.Find<AssetFile>(req.Id);
                        if (paf == null) return;

                        if (req.Obj != null)
                        {
                            if (req.Obj is Texture2D tex)
                            {
                                paf.Width = tex.width;
                                paf.Height = tex.height;
                            }
                            if (req.Obj is AudioClip clip)
                            {
                                paf.Length = clip.length;
                            }
                        }

                        paf.PreviewFile = Path.GetFileName(previewFile);
                        paf.DominantColor = null;
                        paf.DominantColorGroup = null;

                        Persist(paf);
                    });

                    // from time to time store the previews in case something goes wrong
                    PreviewGenerator.EnsureProgress();
                    if (PreviewGenerator.ActiveRequestCount() > 100)
                    {
                        CurrentSub = "Generating preview images...";
                        await PreviewGenerator.ExportPreviews(10);
                    }
                }
            }
            if (spec.createPreviews)
            {
                CurrentSub = "Finalizing preview images...";
                await PreviewGenerator.ExportPreviews();
                PreviewGenerator.Clear();
            }
            MetaProgress.Remove(progressId);
            if (!actAsSubImporter) ResetState(true);
        }
    }
}