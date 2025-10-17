using System;
using System.Collections;
using UnityEngine;
using Unity.Compatibility;
using Celeste;

namespace Unity.Celeste
{
    /// <summary>
    /// Main Unity adapter for Celeste that bridges the XNA/Monocle engine with Unity
    /// </summary>
    public class CelesteUnityAdapter : MonoBehaviour
    {
        [Header("Celeste Settings")]
        public bool maintainXNACompatibility = true;
        public bool useUnityPhysics = false;
        public bool useUnityInput = false;
        public bool useUnityRendering = false;
        
        [Header("Performance")]
        public int targetFrameRate = 60;
        public bool vsync = true;
        
        [Header("Resolution")]
        public int gameWidth = Celeste.Celeste.GameWidth;
        public int gameHeight = Celeste.Celeste.GameHeight;
        public int targetWidth = Celeste.Celeste.TargetWidth;
        public int targetHeight = Celeste.Celeste.TargetHeight;
        
        private Celeste.Celeste celesteInstance;
        private bool isInitialized = false;
        
        void Awake()
        {
            // Setup Unity-specific settings
            Application.targetFrameRate = targetFrameRate;
            QualitySettings.vSyncCount = vsync ? 1 : 0;
            
            // Don't destroy this object when loading new scenes
            DontDestroyOnLoad(gameObject);
        }
        
        void Start()
        {
            StartCoroutine(InitializeCeleste());
        }
        
        private IEnumerator InitializeCeleste()
        {
            try
            {
                // Initialize Celeste engine
                celesteInstance = new Celeste.Celeste();
                celesteInstance.Initialize();
                
                // Load content
                celesteInstance.LoadContent();
                
                isInitialized = true;
                Debug.Log("Celeste Unity integration initialized successfully");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to initialize Celeste: {ex}");
            }
            
            yield return null;
        }
        
        void Update()
        {
            if (!isInitialized || celesteInstance == null)
                return;
                
            try
            {
                // Update Celeste game logic
                var gameTime = new Microsoft.Xna.Framework.GameTime(
                    TimeSpan.FromSeconds(Time.time),
                    TimeSpan.FromSeconds(Time.deltaTime)
                );
                
                celesteInstance.Update(gameTime);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error updating Celeste: {ex}");
            }
        }
        
        void OnRenderObject()
        {
            if (!isInitialized || celesteInstance == null)
                return;
                
            if (useUnityRendering)
            {
                // Use Unity's rendering pipeline
                RenderWithUnity();
            }
            else
            {
                // Use original XNA/MonoGame rendering
                RenderWithXNA();
            }
        }
        
        private void RenderWithUnity()
        {
            // TODO: Implement Unity-specific rendering
            // This would involve converting XNA draw calls to Unity Graphics API
        }
        
        private void RenderWithXNA()
        {
            try
            {
                // Use original rendering system
                celesteInstance.RenderCore();
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error rendering Celeste: {ex}");
            }
        }
        
        void OnDestroy()
        {
            if (celesteInstance != null)
            {
                celesteInstance.Dispose();
                celesteInstance = null;
            }
        }
        
        void OnApplicationPause(bool pauseStatus)
        {
            if (celesteInstance != null && pauseStatus)
            {
                Celeste.Celeste.PauseAnywhere();
            }
        }
        
        void OnApplicationFocus(bool hasFocus)
        {
            if (celesteInstance != null && !hasFocus)
            {
                Celeste.Celeste.PauseAnywhere();
            }
        }
    }
}