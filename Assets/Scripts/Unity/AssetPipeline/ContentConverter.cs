using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
namespace Unity.Celeste.AssetPipeline
{
    /// <summary>
    /// Converts Celeste Content assets to Unity Assets (Scenes, Sprites, Audio)
    /// Maintains compatibility with original Content Pipeline while enabling Unity's asset system
    /// </summary>
    public class ContentConverter : AssetPostprocessor
    {
        private static readonly Dictionary<string, Type> assetTypeMap = new Dictionary<string, Type>
        {
            { ".xnb", typeof(XNBAssetConverter) },
            { ".png", typeof(TextureConverter) },
            { ".jpg", typeof(TextureConverter) },
            { ".ogg", typeof(AudioConverter) },
            { ".wav", typeof(AudioConverter) },
            { ".bin", typeof(BinaryDataConverter) },
            { ".xml", typeof(XMLDataConverter) }
        };
        
        // Called when assets are imported
        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            foreach (string assetPath in importedAssets)
            {
                ProcessAsset(assetPath);
            }
        }
        
        private static void ProcessAsset(string assetPath)
        {
            string extension = Path.GetExtension(assetPath).ToLower();
            
            if (assetTypeMap.TryGetValue(extension, out Type converterType))
            {
                var converter = Activator.CreateInstance(converterType) as IAssetConverter;
                converter?.ConvertAsset(assetPath);
            }
        }
        
        [MenuItem("Celeste/Convert Content to Unity Assets")]
        public static void ConvertAllContent()
        {
            string contentPath = Path.Combine(Application.dataPath, "../Content");
            if (Directory.Exists(contentPath))
            {
                ConvertContentDirectory(contentPath);
                AssetDatabase.Refresh();
                Debug.Log("Content conversion completed");
            }
            else
            {
                Debug.LogWarning("Content directory not found. Please ensure Content folder exists in project root.");
            }
        }
        
        private static void ConvertContentDirectory(string contentPath)
        {
            foreach (string filePath in Directory.GetFiles(contentPath, "*", SearchOption.AllDirectories))
            {
                string relativePath = filePath.Replace(Application.dataPath + "/../", "");
                ProcessAsset(relativePath);
            }
        }
    }
    
    // Base interface for asset converters
    public interface IAssetConverter
    {
        void ConvertAsset(string assetPath);
    }
    
    // XNB (XNA Binary) asset converter
    public class XNBAssetConverter : IAssetConverter
    {
        public void ConvertAsset(string assetPath)
        {
            Debug.Log($"Converting XNB asset: {assetPath}");
            // TODO: Implement XNB to Unity asset conversion
            // This would parse XNB files and convert them to Unity-compatible formats
        }
    }
    
    // Texture converter for sprites and images
    public class TextureConverter : IAssetConverter
    {
        public void ConvertAsset(string assetPath)
        {
            Debug.Log($"Converting texture: {assetPath}");
            
            // Import as Unity texture with Celeste-appropriate settings
            TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            if (importer != null)
            {
                importer.textureType = TextureImporterType.Sprite;
                importer.spriteImportMode = SpriteImportMode.Multiple;
                importer.filterMode = FilterMode.Point; // Pixel-perfect filtering for Celeste
                importer.compressionQuality = 100;
                importer.SaveAndReimport();
            }
        }
    }
    
    // Audio converter
    public class AudioConverter : IAssetConverter
    {
        public void ConvertAsset(string assetPath)
        {
            Debug.Log($"Converting audio: {assetPath}");
            
            AudioImporter importer = AssetImporter.GetAtPath(assetPath) as AudioImporter;
            if (importer != null)
            {
                // Configure for Celeste audio requirements
                var settings = importer.defaultSampleSettings;
                settings.compressionFormat = AudioCompressionFormat.Vorbis;
                settings.quality = 1.0f;
                importer.defaultSampleSettings = settings;
                importer.SaveAndReimport();
            }
        }
    }
    
    // Binary data converter (for level data, etc.)
    public class BinaryDataConverter : IAssetConverter
    {
        public void ConvertAsset(string assetPath)
        {
            Debug.Log($"Converting binary data: {assetPath}");
            // TODO: Convert binary level data to Unity-compatible formats
        }
    }
    
    // XML data converter
    public class XMLDataConverter : IAssetConverter
    {
        public void ConvertAsset(string assetPath)
        {
            Debug.Log($"Converting XML data: {assetPath}");
            // TODO: Parse XML and convert to Unity ScriptableObjects or prefabs
        }
    }
}
#endif