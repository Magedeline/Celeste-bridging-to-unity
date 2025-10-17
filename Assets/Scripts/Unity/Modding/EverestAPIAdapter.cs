using System;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.Celeste.Modding
{
    /// <summary>
    /// Unity adapter for EverestAPI (Celeste modding framework)
    /// Maintains compatibility with existing Celeste mods while enabling Unity-specific features
    /// </summary>
    public class EverestAPIAdapter : MonoBehaviour
    {
        [Header("EverestAPI Configuration")]
        public bool enableEverestCompatibility = true;
        public string everestModsDirectory = "Mods";
        public bool autoLoadMods = true;
        
        [Header("Unity Modding Features")]
        public bool enableUnityModSupport = true;
        public bool enableBepInExIntegration = true;
        
        private List<IMod> loadedMods = new List<IMod>();
        private bool isInitialized = false;
        
        void Awake()
        {
            if (enableEverestCompatibility)
            {
                InitializeEverestAPI();
            }
            
            if (enableBepInExIntegration)
            {
                InitializeBepInEx();
            }
        }
        
        void Start()
        {
            if (autoLoadMods)
            {
                LoadMods();
            }
        }
        
        private void InitializeEverestAPI()
        {
            try
            {
                Debug.Log("Initializing EverestAPI compatibility layer");
                // TODO: Initialize EverestAPI compatibility
                // This would setup the mod loading infrastructure that Everest uses
                
                isInitialized = true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to initialize EverestAPI: {ex}");
            }
        }
        
        private void InitializeBepInEx()
        {
            Debug.Log("Initializing BepInEx integration for Unity-specific modding");
            // TODO: Setup BepInEx integration for Unity-specific mods
        }
        
        private void LoadMods()
        {
            if (!isInitialized)
            {
                Debug.LogWarning("EverestAPI not initialized, cannot load mods");
                return;
            }
            
            LoadEverestMods();
            LoadUnityMods();
        }
        
        private void LoadEverestMods()
        {
            string modsPath = System.IO.Path.Combine(Application.streamingAssetsPath, everestModsDirectory);
            
            if (!System.IO.Directory.Exists(modsPath))
            {
                Debug.Log($"Mods directory not found: {modsPath}");
                return;
            }
            
            Debug.Log($"Loading Everest mods from: {modsPath}");
            // TODO: Implement Everest mod loading
            // This would scan for .zip files and load them using Everest's mod system
        }
        
        private void LoadUnityMods()
        {
            if (!enableUnityModSupport)
                return;
                
            Debug.Log("Loading Unity-specific mods");
            // TODO: Load Unity-specific mods (prefabs, scripts, assets)
        }
        
        // Mod management methods
        public void RegisterMod(IMod mod)
        {
            if (!loadedMods.Contains(mod))
            {
                loadedMods.Add(mod);
                mod.Load();
                Debug.Log($"Registered mod: {mod.Name}");
            }
        }
        
        public void UnregisterMod(IMod mod)
        {
            if (loadedMods.Contains(mod))
            {
                mod.Unload();
                loadedMods.Remove(mod);
                Debug.Log($"Unregistered mod: {mod.Name}");
            }
        }
        
        public List<IMod> GetLoadedMods() => new List<IMod>(loadedMods);
        
        // Unity Editor integration
        #if UNITY_EDITOR
        [UnityEditor.MenuItem("Celeste/Reload Mods")]
        public static void ReloadMods()
        {
            var adapter = FindObjectOfType<EverestAPIAdapter>();
            if (adapter != null)
            {
                adapter.LoadMods();
            }
        }
        #endif
    }
    
    // Interface for mod compatibility
    public interface IMod
    {
        string Name { get; }
        string Version { get; }
        string Author { get; }
        
        void Load();
        void Unload();
        void Update();
    }
    
    // Base class for Unity-compatible Celeste mods
    public abstract class CelesteMod : MonoBehaviour, IMod
    {
        public abstract string Name { get; }
        public abstract string Version { get; }
        public abstract string Author { get; }
        
        public virtual void Load()
        {
            Debug.Log($"Loading mod: {Name} v{Version} by {Author}");
        }
        
        public virtual void Unload()
        {
            Debug.Log($"Unloading mod: {Name}");
        }
        
        public virtual void Update() { }
    }
}