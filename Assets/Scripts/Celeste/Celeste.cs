using Celeste.Pico8;
using Microsoft.Xna.Framework;
using Monocle;
#if ENABLE_STEAM
using Steamworks;
#endif
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Celeste
{
    /// <summary>
    /// Main Celeste game class - Unity adapted.
    /// In Unity, this is typically attached to a GameObject and managed by UnityEngine.
    /// The Engine base class has been adapted for Unity's update loop.
    /// </summary>
    public class Celeste : Engine
    {
        public enum PlayModes
        {
            Normal,
            Debug,
            Event,
            Demo,
        }

        public const int GameWidth = 320;
        public const int GameHeight = 180;
        public const int TargetWidth = 1920;
        public const int TargetHeight = 1080;
        public static PlayModes PlayMode = PlayModes.Normal;
        public const string EventName = "";
        public const bool Beta = false;
        public const string PLATFORM = "UNITY";
        public static new Celeste Instance;
        public static VirtualRenderTarget HudTarget;
        public static VirtualRenderTarget WipeTarget;
        public static DisconnectedControllerUI DisconnectUI;
        private bool firstLoad = true;
        public AutoSplitterInfo AutoSplitterInfo = new();
        public static Coroutine SaveRoutine;
        public static Stopwatch LoadTimer;
#if ENABLE_STEAM
        public static readonly AppId_t SteamID = new(504230U);
#endif
        private static int _mainThreadId;

        public static Vector2 TargetCenter => new Vector2(TargetWidth, TargetHeight) / 2f;

        /// <summary>
        /// Unity-compatible constructor. Settings might not be initialized yet in Unity context.
        /// </summary>
        public Celeste() : base(
            GameWidth, 
            GameHeight, 
            TargetWidth, 
            TargetHeight, 
            nameof(Celeste),
            Settings.Instance?.Fullscreen ?? false,
            Settings.Instance?.VSync ?? true)
        {
            Version = new
#if ENABLE_STEAM
                System.
#endif
                Version(1, 4, 0, 0);
            Instance = this;
            ExitOnEscapeKeypress = false;
#if ENABLE_STEAM
            Stats.MakeRequest();
#endif
            StatsForStadia.MakeRequest();
            UnityEngine.Debug.Log("CELESTE : " + Version + " (Unity)");
        }

        /// <summary>
        /// Initialize Celeste. Called from Unity's initialization (e.g., Awake or Start).
        /// </summary>
        public override void Initialize()
        {
            _mainThreadId = Thread.CurrentThread.ManagedThreadId;
            
            base.Initialize();
            
            if (Settings.Instance != null)
            {
                Settings.Instance.AfterLoad();
                if (Settings.Instance.Fullscreen)
                    ViewPadding = Settings.Instance.ViewportPadding;
                Settings.Instance.ApplyScreen();
            }
            
            SFX.Initialize();
            Tags.Initialize();
            Input.Initialize();
            Commands.Enabled = PlayMode == PlayModes.Debug;
            Scene = new GameLoader();
        }

        /// <summary>
        /// Load Celeste content. Called after Initialize.
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();
            UnityEngine.Debug.Log("BEGIN LOAD");
            LoadTimer = Stopwatch.StartNew();
            
            try
            {
                PlaybackData.Load();
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogWarning("PlaybackData.Load failed: " + ex.Message);
            }
            
            if (firstLoad)
            {
                firstLoad = false;
                HudTarget = VirtualContent.CreateRenderTarget("hud-target", TargetWidth + 2, TargetHeight + 2);
                WipeTarget = VirtualContent.CreateRenderTarget("wipe-target", TargetWidth + 2, TargetHeight + 2);
                
                try
                {
                    OVR.Load();
                    GFX.Load();
                    MTN.Load();
                }
                catch (Exception ex)
                {
                    UnityEngine.Debug.LogError("Content loading failed: " + ex.Message);
                }
            }
            
            if (GFX.Game != null)
            {
                Monocle.Draw.Particle = GFX.Game["util/particle"];
                Monocle.Draw.Pixel = new MTexture(GFX.Game["util/pixel"], 1, 1, 1, 1);
            }
            
            try
            {
                GFX.LoadEffects();
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogWarning("GFX.LoadEffects failed: " + ex.Message);
            }
        }

        /// <summary>
        /// Update Celeste. Call from Unity's Update with Time.deltaTime.
        /// </summary>
        public override void Update(float deltaTime)
        {
#if ENABLE_STEAM
            SteamAPI.RunCallbacks();
#endif
            SaveRoutine?.Update();
            AutoSplitterInfo.Update();
            Audio.Update();
            base.Update(deltaTime);
            Input.UpdateGrab();
        }

        protected override void OnSceneTransition(Scene last, Scene next)
        {
            if (last is not OverworldLoader || next is not Overworld)
                base.OnSceneTransition(last, next);
            TimeRate = 1f;
            Audio.PauseGameplaySfx = false;
            Audio.SetMusicParam("fade", 1f);
            Distort.Anxiety = 0.0f;
            Distort.GameRate = 1f;
            Glitch.Value = 0.0f;
        }

        /// <summary>
        /// Render Celeste. In Unity, call this from a camera's OnPostRender or similar.
        /// </summary>
        public override void Render()
        {
            base.Render();
            if (DisconnectUI != null)
                DisconnectUI.Render();
        }

        /// <summary>
        /// Freeze game for a duration (used for hit-stop effects, etc.)
        /// </summary>
        public static void Freeze(float time)
        {
            if (FreezeTimer >= (double)time)
                return;
            FreezeTimer = time;
            if (Scene == null)
                return;
            Scene.Tracker.GetEntity<CassetteBlockManager>()?.AdvanceMusic(time);
        }

        public static bool IsMainThread => Thread.CurrentThread.ManagedThreadId == _mainThreadId;

        /// <summary>
        /// Unity entry point - not used in Unity builds.
        /// Keep for compatibility or testing standalone builds.
        /// In Unity, initialization happens through MonoBehaviour lifecycle.
        /// </summary>
        [Conditional("STANDALONE_BUILD")]
        private static void Main(string[] args)
        {
            Celeste celeste;
            try
            {
                _mainThreadId = Thread.CurrentThread.ManagedThreadId;
                Settings.Initialize();
#if ENABLE_STEAM
                if (SteamAPI.RestartAppIfNecessary(SteamID))
                  return;
                if (!SteamAPI.Init())
                {
                  ErrorLog.Write("Steam not found!");
                  ErrorLog.Open();
                  return;
                }
                if (!Settings.Existed)
                  Settings.Instance.Language = SteamApps.GetCurrentGameLanguage();
#endif
                int num = Settings.Existed ? 1 : 0;
                for (int index = 0; index < args.Length - 1; ++index)
                {
                    if (args[index] is "--language" or "-l")
                        Settings.Instance.Language = args[++index];
                    else if (args[index] is "--default-language" or "-dl")
                    {
                        if (!Settings.Existed)
                            Settings.Instance.Language = args[++index];
                    }
                    else if (args[index] is "--gui" or "-g")
                        Input.OverrideInputPrefix = args[++index];
                }
                celeste = new Celeste();
                celeste.Initialize();
                celeste.LoadContent();
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError(ex.ToString());
                ErrorLog.Write(ex);
                try
                {
                    ErrorLog.Open();
                    return;
                }
                catch
                {
                    UnityEngine.Debug.LogError("Failed to open the log!");
                    return;
                }
            }
            
            // In Unity, the update loop is handled by Unity's MonoBehaviour
            // RunWithLogging is not used
        }

        /// <summary>
        /// Reload game assets (levels, graphics, etc.)
        /// </summary>
        public static void ReloadAssets(bool levels, bool graphics, bool hires, AreaKey? area = null)
        {
            if (levels)
                ReloadLevels(area);
            if (!graphics)
                return;
            ReloadGraphics(hires);
        }

        public static void ReloadLevels(AreaKey? area = null)
        {
            if (area == null)
            {
                UnityEngine.Debug.LogWarning("ReloadLevels called with null area - full reload not implemented");
                return;
            }
            
            // Implement level reloading logic here
            UnityEngine.Debug.Log($"Reloading levels for area: {area}");
        }

        public static void ReloadPortraits()
        {
            // Implement portrait reloading
            UnityEngine.Debug.Log("ReloadPortraits - not yet implemented");
        }

        public static void ReloadDialog()
        {
            // Implement dialog reloading
            UnityEngine.Debug.Log("ReloadDialog - not yet implemented");
        }

        /// <summary>
        /// Call an external process (for modding tools, etc.)
        /// </summary>
        private static void CallProcess(string path, string args = "", bool createWindow = false)
        {
            try
            {
                Process process = new()
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = path,
                        WorkingDirectory = Path.GetDirectoryName(path),
                        RedirectStandardOutput = false,
                        CreateNoWindow = !createWindow,
                        UseShellExecute = false,
                        Arguments = args
                    }
                };
                process.Start();
                process.WaitForExit();
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError($"Failed to call process {path}: {ex.Message}");
            }
        }

        /// <summary>
        /// Pause the game from anywhere (useful for debugging)
        /// </summary>
        public static bool PauseAnywhere()
        {
            switch (Scene)
            {
                case Level level:
                    if (level.CanPause)
                    {
                        level.Pause();
                        return true;
                    }
                    break;
                case Emulator emulator:
                    if (emulator.CanPause)
                    {
                        emulator.CreatePauseMenu();
                        return true;
                    }
                    break;
                case IntroVignette introVignette:
                    if (introVignette.CanPause)
                    {
                        introVignette.OpenMenu();
                        return true;
                    }
                    break;
                case CoreVignette coreVignette:
                    if (coreVignette.CanPause)
                    {
                        coreVignette.OpenMenu();
                        return true;
                    }
                    break;
            }
            return false;
        }
    }
}
