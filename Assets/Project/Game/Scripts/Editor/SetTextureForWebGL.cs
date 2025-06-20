using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class SetTextureForWebGL : UnityEditor.Editor
    {
        [MenuItem("Tools/Textures/Convert to WebGL Settings")]
    private static void ConvertTexturesToWebGL()
    {
        var textures = Selection.GetFiltered<Texture2D>(SelectionMode.Assets);
        int changedCount = 0;

        foreach (var texture in textures)
        {
            string path = AssetDatabase.GetAssetPath(texture);
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
            if (importer == null)
                continue;

            Texture2D loaded = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
            if (loaded == null)
                continue;

            int maxDimension = Mathf.Max(loaded.width, loaded.height);

            // Округляем вверх до ближайшей степени двойки
            int[] sizes = {32, 64, 128, 256, 512, 1024, 2048, 4096, 8192};
            int closestSize = 32;
            foreach (var size in sizes)
            {
                if (maxDimension <= size)
                {
                    closestSize = size;
                    break;
                }
            }

            bool changed = false;

            // Установка базовых настроек
            if (importer.maxTextureSize != closestSize)
            {
                importer.maxTextureSize = closestSize;
                changed = true;
            }

            if (importer.textureCompression != TextureImporterCompression.CompressedLQ)
            {
                importer.textureCompression = TextureImporterCompression.CompressedLQ;
                changed = true;
            }

            // Установка настроек для WebGL
            TextureImporterPlatformSettings webglSettings = importer.GetPlatformTextureSettings("WebGL");
            if (!webglSettings.overridden || 
                webglSettings.maxTextureSize != closestSize || 
                webglSettings.textureCompression != TextureImporterCompression.CompressedLQ ||
                webglSettings.format != TextureImporterFormat.ASTC_8x8)
            {
                webglSettings.overridden = true;
                webglSettings.maxTextureSize = closestSize;
                webglSettings.textureCompression = TextureImporterCompression.CompressedLQ;
                webglSettings.format = TextureImporterFormat.ASTC_8x8;
                importer.SetPlatformTextureSettings(webglSettings);
                changed = true;
            }

            if (!changed) continue;
            EditorUtility.SetDirty(importer);
            importer.SaveAndReimport();
            changedCount++;
        }

        Debug.Log($"✅ Обработано текстур для WebGL: {changedCount}");
    }

    }
}
