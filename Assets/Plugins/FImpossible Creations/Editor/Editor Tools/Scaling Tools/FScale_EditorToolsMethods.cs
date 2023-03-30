using FIMSpace.FTex;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace FIMSpace.FEditor
{
    public static class FScale_EditorToolsMethods
    {
        #region Acquirung texture for editing


        public static TextureImporter GetTextureAsset(Texture2D source)
        {
            string sPath = AssetDatabase.GetAssetPath(source);

            TextureImporter sourceTex = (TextureImporter)AssetImporter.GetAtPath(sPath);
            TextureImporter outTex = sourceTex;

            if (sourceTex != null && outTex != null)
            {
                return outTex;
            }
            else
            {
                Debug.LogError("[ICONS MANAGER EDITOR] No Texture!");
                return null;
            }
        }

        public static TextureInfo GetTextureInfo(TextureImporter sourceTex, Texture2D source)
        {
            return new TextureInfo(sourceTex, source);
        }

        public static TextureInfo StartEditingTextureAsset(TextureImporter sourceTex, Texture2D source, TextureInfo src, bool saveAndReimport = true)
        {
            try
            {
                string sPath = AssetDatabase.GetAssetPath(source);
                FETextureExtension extension = FTex_Methods.GetFileExtension(sPath);

                if (extension == FETextureExtension.UNSUPPORTED)
                {
                    Debug.LogError("[ICONS MANAGER EDITOR] Not supported format to scale texture, Icons Manager supports only .JPG .PNG .TGA .EXR files!");
                    return src;
                }

                // Making source texture be open for GetPixels method
                // Setting output texture params be able for pixels replacement
                sourceTex.isReadable = true;
                sourceTex.textureType = TextureImporterType.Default;
                sourceTex.textureCompression = TextureImporterCompression.Uncompressed;

                TextureImporterPlatformSettings sourceSets = sourceTex.GetPlatformTextureSettings("Standalone");
                sourceSets.format = TextureImporterFormat.RGBA32;
                sourceSets.overridden = true;
                sourceTex.SetPlatformTextureSettings(sourceSets);

                // Refreshing assets for our changes
                if (saveAndReimport) sourceTex.SaveAndReimport();
            }
            catch (System.Exception exc)
            {
                src.RestoreOn(sourceTex, source, false);
                Debug.LogError("[ICONS MANAGER EDITOR] Something went wrong when rescalling image file! " + exc);
            }

            return src;
        }



        public static void EndEditingTextureAsset(Color32[] newPixels, TextureInfo info, TextureImporter sourceTex, Texture2D output, bool saveAndReimport = true)
        {
            string oPath = AssetDatabase.GetAssetPath(output);

            if (newPixels != null)
            {
                output.SetPixels32(newPixels);

                FETextureExtension extension = FTex_Methods.GetFileExtension(oPath);
                byte[] fileBytes = null;
                switch (extension)
                {
                    case FETextureExtension.JPG: fileBytes = output.EncodeToJPG(95); break;
                    case FETextureExtension.PNG: fileBytes = output.EncodeToPNG(); break;
                    case FETextureExtension.TGA: fileBytes = FTex_AdditionalEncoders.EncodeToTGA(output); break;
                    case FETextureExtension.TIFF: fileBytes = FTex_AdditionalEncoders.EncodeToTIFF(output); break;
                    case FETextureExtension.EXR: fileBytes = output.EncodeToEXR(); break;
                }

                // Applying changes to file
                if (fileBytes != null) File.WriteAllBytes(oPath, fileBytes);
            }

            info.RestoreOn(sourceTex, output);

            if (saveAndReimport)
            {
                // Refreshing assets in editor window
                sourceTex.SaveAndReimport();

                AssetDatabase.ImportAsset(oPath);
                AssetDatabase.Refresh();
            }
        }


        public static Texture2D DuplicateAsset(Texture2D source)
        {
            string path = AssetDatabase.GetAssetPath(source);
            string newPath = Path.GetDirectoryName(path) + "/" + Path.GetFileNameWithoutExtension(path) + "-Backup" + Path.GetExtension(path);

            Texture2D copied = null;
            if (AssetDatabase.CopyAsset(path, newPath))
                copied = (Texture2D)AssetDatabase.LoadAssetAtPath(newPath, typeof(Texture2D));

            return copied;
        }


        public struct TextureInfo
        {
            public bool wasReadable;
            public TextureImporterType sType;
            public TextureFormat outFormat;
            public TextureImporterCompression comp;
            public TextureImporterPlatformSettings sourceSets;
            public bool doMips;
            public int anisoLevel;
            public FilterMode filter;
            public TextureImporterShape shape;

            public TextureInfo(TextureImporter sourceTex, Texture2D source)
            {
                wasReadable = sourceTex.isReadable;
                sType = sourceTex.textureType;
                outFormat = source.format;
                comp = sourceTex.textureCompression;
                sourceSets = sourceTex.GetPlatformTextureSettings("Standalone");
                doMips = source.mipmapCount > 1;
                anisoLevel = source.anisoLevel;
                filter = source.filterMode;
                shape = sourceTex.textureShape;
            }

            public void RestoreOn(TextureImporter sourceTex, Texture2D source, bool apply = true)
            {
                if (apply)
                {
                    source.Apply(doMips, !wasReadable);
                }

                sourceTex.mipmapEnabled = doMips;
                sourceTex.isReadable = wasReadable;
                sourceTex.textureType = sType;
                sourceTex.textureCompression = comp;
                sourceTex.SetPlatformTextureSettings(sourceSets);
                sourceTex.anisoLevel = anisoLevel;
                sourceTex.filterMode = filter;
                sourceTex.textureShape = shape;
            }
        }


        public static Texture2D DuplicateAsPNG(Texture2D source, string postFix = "-ToPNG", bool saveAndReimport = true, bool addNewAlphaChannelIfNeeded = false)
        {
            TextureImporter imp = GetTextureAsset(source);
            string path = AssetDatabase.GetAssetPath(source);
            string directory = System.IO.Path.GetDirectoryName(path);

            TextureInfo info = GetTextureInfo(imp, source);
            TextureInfo ainfo = info;
            StartEditingTextureAsset(imp, source, ainfo);

            // Generate png out of source texture pixels and data
            Texture2D newPng = new Texture2D(source.width, source.height, TextureFormat.RGBA32, source.mipmapCount > 1);
            Color32[] px = source.GetPixels32();
            if (addNewAlphaChannelIfNeeded) for (int i = 0; i < px.Length; i++) px[i].a = byte.MaxValue;
            newPng.SetPixels32(px);


            newPng.Apply(source.mipmapCount > 1, false);

            // Save new png texture asset in directory
            string nPath = directory + "/" + source.name + postFix + ".png";
            File.WriteAllBytes(nPath, newPng.EncodeToPNG());
            AssetDatabase.Refresh(ImportAssetOptions.Default);
            AssetDatabase.ImportAsset(nPath, ImportAssetOptions.Default);

            // Set texture asset same settings like source texture asset
            TextureImporter pimp = (TextureImporter)AssetImporter.GetAtPath(nPath);
            if (pimp != null)
            {
                TextureInfo pinfo = info;
                if (addNewAlphaChannelIfNeeded)
                {
                    pinfo.outFormat = TextureFormat.RGBA32;
                }

                pinfo.RestoreOn(pimp, newPng, false);
                AssetDatabase.Refresh(ImportAssetOptions.Default);
                AssetDatabase.ImportAsset(nPath, ImportAssetOptions.Default);
                if (saveAndReimport) pimp.SaveAndReimport();

                // Finalize editings
                //EndEditingTextureAsset(null, info, pimp, newPng);
            }

            EndEditingTextureAsset(null, info, imp, source);

            newPng = AssetDatabase.LoadAssetAtPath<Texture2D>(nPath);

            return newPng;
        }


        #endregion



        public static void ScaleTextureFile(Texture2D source, Texture2D output, Vector2 dimensions, int quality = 4)
        {
            if (output)
                if (output.width == (int)dimensions.x && output.height == (int)dimensions.y)
                {
                    Debug.Log("[ICONS MANAGER EDITOR] " + source.name + " have already dimensions " + dimensions.x + " x " + dimensions.y);
                    return;
                }

            // Getting textures
            string sPath = AssetDatabase.GetAssetPath(source);
            string oPath = AssetDatabase.GetAssetPath(output);

            TextureImporter sourceTex = (TextureImporter)AssetImporter.GetAtPath(sPath);
            TextureImporter outTex = sourceTex;

            if (source != output) outTex = (TextureImporter)AssetImporter.GetAtPath(oPath);

            if (sourceTex != null && outTex != null)
            {
                // Remember some important texture asset parameters to restore them after changes
                bool swasReadable = sourceTex.isReadable;
                bool owasReadable = outTex.isReadable;
                TextureImporterType oType = outTex.textureType;
                TextureImporterType sType = sourceTex.textureType;
                TextureFormat outFormat = output.format;
                TextureImporterCompression comp = outTex.textureCompression;
                TextureImporterPlatformSettings preSets = outTex.GetPlatformTextureSettings("Standalone");
                TextureImporterPlatformSettings sourceSets = outTex.GetPlatformTextureSettings("Standalone");

                try
                {
                    FETextureExtension extension = FTex_Methods.GetFileExtension(oPath);

                    if (extension == FETextureExtension.UNSUPPORTED)
                    {
                        Debug.LogError("[ICONS MANAGER EDITOR] Not supported format to scale texture, Icons Manager supports only .JPG .PNG .TGA .EXR files!");
                        return;
                    }

                    // Remember some important texture asset parameters to restore them after changes
                    bool doMips = output.mipmapCount > 1;

                    // Making source texture be open for GetPixels method
                    sourceTex.isReadable = true;
                    sourceTex.textureType = TextureImporterType.Default;

                    // Setting output texture params be able for pixels replacement
                    outTex.isReadable = true;
                    outTex.textureType = TextureImporterType.Default;
                    outTex.textureCompression = TextureImporterCompression.Uncompressed;
                    sourceSets.format = TextureImporterFormat.RGBA32;
                    outTex.SetPlatformTextureSettings(sourceSets);

                    // Refreshing assets for our changes
                    sourceTex.SaveAndReimport();
                    outTex.SaveAndReimport();

                    // Rescaling image
                    Color32[] newPixels = FTex_ScaleLanczos.ScaleTexture(source.GetPixels32(), source.width, source.height, (int)dimensions.x, (int)dimensions.y, quality);

                    //int startBytes = File.ReadAllBytes(oPath).Length;

                    // Applying to texture asset
                    output.Reinitialize((int)dimensions.x, (int)dimensions.y);
                    output.SetPixels32(newPixels);

                    byte[] fileBytes = null;
                    switch (extension)
                    {
                        case FETextureExtension.JPG: fileBytes = output.EncodeToJPG(95); break;
                        case FETextureExtension.PNG: fileBytes = output.EncodeToPNG(); break;
                        case FETextureExtension.TGA: fileBytes = FTex_AdditionalEncoders.EncodeToTGA(output); break;
                        case FETextureExtension.TIFF: fileBytes = FTex_AdditionalEncoders.EncodeToTIFF(output); break;
                        case FETextureExtension.EXR: fileBytes = output.EncodeToEXR(); break;
                    }

                    // Applying changes to file
                    if (fileBytes != null)
                    {
                        File.WriteAllBytes(oPath, fileBytes);
                    }

                    output.Apply(doMips, !owasReadable);

                    // Restoring parameters
                    if (fileBytes != null)
                        if (!Mathf.IsPowerOfTwo(output.width) || !Mathf.IsPowerOfTwo(output.height))
                        {
                            Debug.Log("<b>[ICONS MANAGER EDITOR]</b> " + output.name + " resized to " + output.width + "x" + output.height + " So there is no power of 2, that means texture can't be compressed to take less memory in build. If it was intended ignore this message. (changing texture settings 'Power of two' under '/advanced/' to 'None')");
                            outTex.npotScale = TextureImporterNPOTScale.None;
                        }


                    sourceTex.isReadable = swasReadable;
                    sourceTex.textureType = sType;
                    sourceTex.SetPlatformTextureSettings(sourceSets);

                    outTex.isReadable = owasReadable;
                    outTex.textureType = oType;
                    outTex.textureCompression = comp;
                    outTex.SetPlatformTextureSettings(preSets);

                    // Refreshing assets in editor window
                    sourceTex.SaveAndReimport();
                    outTex.SaveAndReimport();
                }
                catch (System.Exception exc)
                {
                    sourceTex.isReadable = swasReadable;
                    sourceTex.textureType = sType;
                    sourceTex.SetPlatformTextureSettings(sourceSets);

                    outTex.isReadable = owasReadable;
                    outTex.textureType = oType;
                    outTex.textureCompression = comp;
                    outTex.SetPlatformTextureSettings(preSets);

                    AssetDatabase.ImportAsset(sPath);
                    if (source != output) AssetDatabase.ImportAsset(oPath);
                    AssetDatabase.Refresh();
                    Debug.LogError("[ICONS MANAGER EDITOR] Something went wrong when rescalling image file! " + exc);
                }
            }
            else
            {
                Debug.LogError("[ICONS MANAGER EDITOR] No Texture to Rescale!");
            }
        }
    }
}