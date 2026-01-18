using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using System.Reflection;
using System.Runtime;
using UnityEngine;

namespace Monocle
{
    /// <summary>
    /// Unity-adapted Engine class.
    /// Original XNA Game class functionality is replaced with Unity MonoBehaviour patterns.
    /// For Unity, this class acts as a compatibility layer - actual game loop is handled by Unity.
    /// </summary>
    public class Engine : IDisposable
    {
        public string Title;
        public Version Version;
        public static Action OverloadGameLoop;
        private static int viewPadding;
        private static bool resizing;
        public static float TimeRate = 1f;
        public static float TimeRateB = 1f;
        public static float FreezeTimer;
        public static bool DashAssistFreeze;
        public static bool DashAssistFreezePress;
        public static int FPS;
        private TimeSpan counterElapsed = TimeSpan.Zero;
        private int fpsCounter;
        private static string _contentDirectory;
        public static Microsoft.Xna.Framework.Color ClearColor;
        public static bool ExitOnEscapeKeypress;
        private Scene scene;
        private Scene nextScene;
        public static Microsoft.Xna.Framework.Matrix ScreenMatrix;

        // Unity-specific
        private bool _isInitialized;
        private float _lastUpdateTime;

        public static Engine Instance { get; private set; }

        public static GraphicsDeviceManager Graphics { get; private set; }

        public static Commands Commands { get; private set; }

        public static Pooler Pooler { get; private set; }

        public static int Width { get; private set; }

        public static int Height { get; private set; }

        public static int ViewWidth { get; private set; }

        public static int ViewHeight { get; private set; }

        public static int ViewPadding
        {
            get => viewPadding;
            set
            {
                viewPadding = value;
                Instance?.UpdateView();
            }
        }

        public static float DeltaTime { get; private set; }

        public static float RawDeltaTime { get; private set; }

        public static ulong FrameCounter { get; private set; }

        /// <summary>
        /// Absolute content directory path.
        /// </summary>
        public static string ContentDirectory
        {
            get
            {
                if (string.IsNullOrEmpty(_contentDirectory))
                {
                    // In Unity, use Application.streamingAssetsPath or a custom path
                    _contentDirectory = Path.Combine(UnityEngine.Application.streamingAssetsPath, "Content");
                    if (!Directory.Exists(_contentDirectory))
                    {
                        _contentDirectory = Path.Combine(UnityEngine.Application.dataPath, "Content");
                    }
                }
                return _contentDirectory;
            }
            set => _contentDirectory = value;
        }

        // Stub Content manager for compatibility
        public ContentManager Content { get; private set; }

        // Stub GraphicsDevice for compatibility
        public GraphicsDevice GraphicsDevice { get; private set; }

        // Stub window properties
        public bool IsMouseVisible { get; set; }
        public TimeSpan InactiveSleepTime { get; set; }

        public bool IsActive => UnityEngine.Application.isFocused;

        // Approximation of XNA fixed timestep; used by some simulation helpers.
        public TimeSpan TargetElapsedTime { get; set; } = TimeSpan.FromSeconds(1.0 / 60.0);

        public GameWindow Window { get; } = new GameWindow();

        public Engine(
            int width,
            int height,
            int windowWidth,
            int windowHeight,
            string windowTitle,
            bool fullscreen,
            bool vsync)
        {
            Instance = this;
            Title = windowTitle;
            Width = width;
            Height = height;
            ViewWidth = windowWidth;
            ViewHeight = windowHeight;
            ClearColor = Microsoft.Xna.Framework.Color.Black;
            InactiveSleepTime = new TimeSpan(0L);
            
            // Initialize stub graphics
            Graphics = new GraphicsDeviceManager(this);
            GraphicsDevice = Graphics.GraphicsDevice;
            Content = new ContentManager(null);
            
            GCSettings.LatencyMode = GCLatencyMode.SustainedLowLatency;
        }

        // Default constructor for Unity initialization
        public Engine() : this(320, 180, 1920, 1080, "Celeste", false, true)
        {
        }

        /// <summary>
        /// Initialize the engine. Call this from Unity's Awake or Start.
        /// </summary>
        public virtual void Initialize()
        {
            if (_isInitialized) return;
            
            MInput.Initialize();
            Tracker.Initialize();
            Pooler = new Pooler();
            Commands = new Commands();
            
            _isInitialized = true;
        }

        /// <summary>
        /// Load content. Call after Initialize.
        /// </summary>
        public virtual void LoadContent()
        {
            VirtualContent.Reload();
            Monocle.Draw.Initialize();
        }

        /// <summary>
        /// Unload content.
        /// </summary>
        public virtual void UnloadContent()
        {
            VirtualContent.Unload();
        }

        /// <summary>
        /// Update the engine. Call from Unity's Update.
        /// </summary>
        /// <param name="deltaTime">Time since last frame (typically Time.deltaTime)</param>
        public virtual void Update(float deltaTime)
        {
            RawDeltaTime = deltaTime;
            DeltaTime = RawDeltaTime * TimeRate * TimeRateB;
            ++FrameCounter;
            
            MInput.Update();
            
            if (OverloadGameLoop != null)
            {
                OverloadGameLoop();
            }
            else
            {
                #if CELESTE
                if (DashAssistFreeze)
                {
                    if (Input.Dash.Check || !DashAssistFreezePress)
                    {
                        if (Input.Dash.Check)
                            DashAssistFreezePress = true;
                        if (scene != null)
                        {
                            scene.Tracker.GetEntity<PlayerDashAssist>()?.Update();
                            if (scene is Level level)
                                level.UpdateTime();
                            scene.Entities.UpdateLists();
                        }
                    }
                    else
                        DashAssistFreeze = false;
                }
                #else
                if (DashAssistFreeze)
                {
                    DashAssistFreeze = false;
                }
                #endif
                
                if (!DashAssistFreeze)
                {
                    if (FreezeTimer > 0.0)
                        FreezeTimer = Math.Max(FreezeTimer - RawDeltaTime, 0.0f);
                    else if (scene != null)
                    {
                        scene.BeforeUpdate();
                        scene.Update();
                        scene.AfterUpdate();
                    }
                }
                
                if (Commands.Open)
                    Commands.UpdateOpen();
                else if (Commands.Enabled)
                    Commands.UpdateClosed();
                
                if (scene != nextScene)
                {
                    Scene oldScene = scene;
                    scene?.End();
                    scene = nextScene;
                    OnSceneTransition(oldScene, nextScene);
                    scene?.Begin();
                }
            }
            
            // FPS counter
            ++fpsCounter;
            counterElapsed += TimeSpan.FromSeconds(deltaTime);
            if (counterElapsed >= TimeSpan.FromSeconds(1.0))
            {
                FPS = fpsCounter;
                fpsCounter = 0;
                counterElapsed -= TimeSpan.FromSeconds(1.0);
            }
        }

        /// <summary>
        /// Render the scene. In Unity, this is typically handled by Unity's rendering.
        /// This method maintains compatibility for code that calls it.
        /// </summary>
        public virtual void Render()
        {
            scene?.BeforeRender();
            
            // In Unity, actual rendering is done by the Unity camera/renderer
            // This is kept for API compatibility
            
            if (scene != null)
            {
                scene.Render();
                scene.AfterRender();
            }
            
            if (Commands.Open)
                Commands.Render();
        }

        protected virtual void OnSceneTransition(Scene from, Scene to)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            TimeRate = 1f;
            DashAssistFreeze = false;
        }

        public static Scene Scene
        {
            get => Instance?.scene;
            set
            {
                if (Instance != null)
                    Instance.nextScene = value;
            }
        }

        public static Viewport Viewport { get; private set; }

        public static void SetWindowed(int width, int height)
        {
            if (width <= 0 || height <= 0)
                return;
            
            // In Unity, use Screen.SetResolution
            UnityEngine.Screen.SetResolution(width, height, false);
            ViewWidth = width;
            ViewHeight = height;
            Instance?.UpdateView();
        }

        public static void SetFullscreen()
        {
            // In Unity, use Screen.SetResolution with fullscreen
            UnityEngine.Screen.SetResolution(
                UnityEngine.Screen.currentResolution.width,
                UnityEngine.Screen.currentResolution.height,
                true);
            Instance?.UpdateView();
        }

        private void UpdateView()
        {
            float backBufferWidth = UnityEngine.Screen.width;
            float backBufferHeight = UnityEngine.Screen.height;
            
            if ((double)backBufferWidth / Width > (double)backBufferHeight / Height)
            {
                ViewWidth = (int)((double)backBufferHeight / Height * Width);
                ViewHeight = (int)backBufferHeight;
            }
            else
            {
                ViewWidth = (int)backBufferWidth;
                ViewHeight = (int)((double)backBufferWidth / Width * Height);
            }
            
            float num = ViewHeight / (float)ViewWidth;
            ViewWidth -= ViewPadding * 2;
            ViewHeight -= (int)((double)num * ViewPadding * 2.0);
            
            ScreenMatrix = Microsoft.Xna.Framework.Matrix.CreateScale(ViewWidth / (float)Width);
            
            Viewport = new Viewport
            {
                X = (int)(backBufferWidth / 2.0 - ViewWidth / 2),
                Y = (int)(backBufferHeight / 2.0 - ViewHeight / 2),
                Width = ViewWidth,
                Height = ViewHeight,
                MinDepth = 0.0f,
                MaxDepth = 1f
            };
        }

        public static void ReloadGraphics(bool hires) { }

        public void Dispose()
        {
            UnloadContent();
            MInput.Shutdown();
        }

        // Stub methods for XNA Game compatibility
        public void Exit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            UnityEngine.Application.Quit();
#endif
        }

        public void Run() { }
        public void RunWithLogging() { }

        // Events for graphics device (stubs for compatibility)
        public event EventHandler<EventArgs> Activated;
        public event EventHandler<EventArgs> Deactivated;

        public void OnActivated()
        {
            scene?.GainFocus();
            Activated?.Invoke(this, EventArgs.Empty);
        }

        public void OnDeactivated()
        {
            scene?.LoseFocus();
            Deactivated?.Invoke(this, EventArgs.Empty);
        }
    }

    // Stub for XNA Game class that Engine used to inherit from
    public abstract class Game : IDisposable
    {
        public ContentManager Content { get; protected set; }
        public GameWindow Window { get; protected set; }
        public GraphicsDevice GraphicsDevice { get; protected set; }
        public bool IsMouseVisible { get; set; }
        public TimeSpan InactiveSleepTime { get; set; }

        protected virtual void Initialize() { }
        protected virtual void LoadContent() { }
        protected virtual void UnloadContent() { }
        protected virtual void Update(GameTime gameTime) { }
        protected virtual void Draw(GameTime gameTime) { }
        protected virtual void OnActivated(object sender, EventArgs args) { }
        protected virtual void OnDeactivated(object sender, EventArgs args) { }
        protected virtual void OnExiting(object sender, EventArgs args) { }

        public void Run() { }
        public void Exit() { }
        public virtual void Dispose() { }
    }

    // Stub for XNA GameWindow
    public class GameWindow
    {
        public string Title { get; set; }
        public bool AllowUserResizing { get; set; }
        public Microsoft.Xna.Framework.Rectangle ClientBounds => new Microsoft.Xna.Framework.Rectangle(0, 0, UnityEngine.Screen.width, UnityEngine.Screen.height);
        public event EventHandler<EventArgs> ClientSizeChanged;
    }

    // Stub for XNA ContentManager (kept here for proximity to Engine)
    public class ContentManager : IDisposable
    {
        public string RootDirectory { get; set; } = "Content";

        public ContentManager(IServiceProvider serviceProvider) { }

        public T Load<T>(string assetName)
        {
            // In Unity, use Resources.Load or Addressables
            return default;
        }

        public void Unload() { }
        public void Dispose() { }
    }
}
