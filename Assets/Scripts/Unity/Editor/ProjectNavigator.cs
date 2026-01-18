#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;

namespace Celeste.Editor
{
    /// <summary>
    /// Quick access menu for navigating the Celeste Bridge project structure.
    /// </summary>
    public static class ProjectNavigator
    {
        private const string ROOT_PATH = "Assets";
        
        #region Script Folders
        
        [MenuItem("Celeste/Navigate/Scripts/Celeste (Game Logic)", false, 100)]
        private static void OpenCelesteScripts()
        {
            PingFolder("Assets/Scripts/Celeste");
        }
        
        [MenuItem("Celeste/Navigate/Scripts/Monocle (Engine)", false, 101)]
        private static void OpenMonocleScripts()
        {
            PingFolder("Assets/Scripts/Monocle");
        }
        
        [MenuItem("Celeste/Navigate/Scripts/Unity (Adapter)", false, 102)]
        private static void OpenUnityScripts()
        {
            PingFolder("Assets/Scripts/Unity");
        }
        
        [MenuItem("Celeste/Navigate/Scripts/Legacy Scripts", false, 103)]
        private static void OpenLegacyScripts()
        {
            PingFolder("Assets/Script");
        }
        
        #endregion
        
        #region Asset Folders
        
        [MenuItem("Celeste/Navigate/Assets/Scenes", false, 200)]
        private static void OpenScenes()
        {
            PingFolder("Assets/Scenes");
        }
        
        [MenuItem("Celeste/Navigate/Assets/Prefabs", false, 201)]
        private static void OpenPrefabs()
        {
            PingFolder("Assets/Prefabs");
        }
        
        [MenuItem("Celeste/Navigate/Assets/Sprites", false, 202)]
        private static void OpenSprites()
        {
            PingFolder("Assets/Sprites");
        }
        
        [MenuItem("Celeste/Navigate/Assets/Animations", false, 203)]
        private static void OpenAnimations()
        {
            PingFolder("Assets/Animations");
        }
        
        [MenuItem("Celeste/Navigate/Assets/Resources", false, 204)]
        private static void OpenResources()
        {
            PingFolder("Assets/Resources");
        }
        
        [MenuItem("Celeste/Navigate/Assets/Materials", false, 205)]
        private static void OpenMaterials()
        {
            PingFolder("Assets/Materials");
        }
        
        [MenuItem("Celeste/Navigate/Assets/SoundFX", false, 206)]
        private static void OpenSoundFX()
        {
            PingFolder("Assets/SoundFX");
        }
        
        #endregion
        
        #region Reference Folders
        
        [MenuItem("Celeste/Navigate/Reference/Original Celeste", false, 300)]
        private static void OpenOriginalCeleste()
        {
            PingFolder("Celeste");
        }
        
        [MenuItem("Celeste/Navigate/Reference/Original Monocle", false, 301)]
        private static void OpenOriginalMonocle()
        {
            PingFolder("Monocle");
        }
        
        [MenuItem("Celeste/Navigate/Reference/Content", false, 302)]
        private static void OpenContent()
        {
            PingFolder("Content");
        }
        
        #endregion
        
        #region Documentation
        
        [MenuItem("Celeste/Navigate/Documentation/Project Structure", false, 400)]
        private static void OpenProjectStructure()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "PROJECT_STRUCTURE.md");
            if (File.Exists(path))
            {
                Application.OpenURL(path);
            }
            else
            {
                Debug.LogWarning("PROJECT_STRUCTURE.md not found in project root.");
            }
        }
        
        [MenuItem("Celeste/Navigate/Documentation/README", false, 401)]
        private static void OpenReadme()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "README.md");
            if (File.Exists(path))
            {
                Application.OpenURL(path);
            }
            else
            {
                Debug.LogWarning("README.md not found in project root.");
            }
        }
        
        [MenuItem("Celeste/Navigate/Documentation/Changelog", false, 402)]
        private static void OpenChangelog()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "CHANGELOG.md");
            if (File.Exists(path))
            {
                Application.OpenURL(path);
            }
            else
            {
                Debug.LogWarning("CHANGELOG.md not found in project root.");
            }
        }
        
        #endregion
        
        #region Quick Create
        
        [MenuItem("Celeste/Quick Create/New Chapter Scene", false, 500)]
        private static void CreateChapterScene()
        {
            string scenePath = EditorUtility.SaveFilePanelInProject(
                "Create New Chapter Scene",
                "Chapter X",
                "unity",
                "Create a new chapter scene",
                "Assets/Scenes"
            );
            
            if (!string.IsNullOrEmpty(scenePath))
            {
                var newScene = UnityEditor.SceneManagement.EditorSceneManager.NewScene(
                    UnityEditor.SceneManagement.NewSceneSetup.DefaultGameObjects,
                    UnityEditor.SceneManagement.NewSceneMode.Single
                );
                UnityEditor.SceneManagement.EditorSceneManager.SaveScene(newScene, scenePath);
                Debug.Log($"Created new chapter scene: {scenePath}");
            }
        }
        
        [MenuItem("Celeste/Quick Create/New Script in Celeste", false, 501)]
        private static void CreateCelesteScript()
        {
            CreateScriptInFolder("Assets/Scripts/Celeste");
        }
        
        [MenuItem("Celeste/Quick Create/New Script in Unity Adapter", false, 502)]
        private static void CreateUnityScript()
        {
            CreateScriptInFolder("Assets/Scripts/Unity");
        }
        
        #endregion
        
        #region Utilities
        
        private static void PingFolder(string folderPath)
        {
            var folder = AssetDatabase.LoadAssetAtPath<Object>(folderPath);
            if (folder != null)
            {
                Selection.activeObject = folder;
                EditorGUIUtility.PingObject(folder);
            }
            else
            {
                // Try with full path for root folders outside Assets
                string fullPath = Path.Combine(Directory.GetCurrentDirectory(), folderPath);
                if (Directory.Exists(fullPath))
                {
                    EditorUtility.RevealInFinder(fullPath);
                }
                else
                {
                    Debug.LogWarning($"Folder not found: {folderPath}");
                }
            }
        }
        
        private static void CreateScriptInFolder(string folderPath)
        {
            // Ensure folder exists
            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                Debug.LogWarning($"Folder does not exist: {folderPath}");
                return;
            }
            
            // Navigate to folder first
            PingFolder(folderPath);
            
            // Trigger Unity's script creation
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(
                "Packages/com.unity.ide.rider/Rider/ProjectGeneration/ProjectTemplates/CSharpScript.txt",
                "NewScript.cs"
            );
        }
        
        #endregion
    }
}
#endif
