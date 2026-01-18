# Celeste.cs Unity Adaptation

## Overview
The `Celeste.cs` file has been successfully adapted to work with Unity while maintaining compatibility with the original Celeste codebase structure.

## Key Changes Made

### 1. **Platform Identifier**
- Changed `PLATFORM` constant from `"PC"` to `"UNITY"` to reflect the Unity environment

### 2. **Constructor Updates**
- Made constructor more defensive with null-coalescing operators for `Settings.Instance`
- Removed `IsFixedTimeStep` property (not applicable in Unity's update model)
- Changed from `Console.WriteLine` to `UnityEngine.Debug.Log`
- Added "(Unity)" suffix to version log message

### 3. **Initialize Method**
- Added `_mainThreadId` initialization
- Made `Settings.Instance` checks null-safe
- Added try-catch blocks around potentially failing initialization code
- Kept base initialization order to maintain compatibility

### 4. **LoadContent Method**
- Wrapped `PlaybackData.Load()` in try-catch with warning logging
- Wrapped `OVR.Load()`, `GFX.Load()`, `MTN.Load()` in try-catch with error logging
- Wrapped `GFX.LoadEffects()` in try-catch with warning logging
- Changed all `Console.WriteLine` to `UnityEngine.Debug.Log`

### 5. **Update Method Signature**
- Changed from `Update(GameTime gameTime)` to `Update(float deltaTime)`
- Removed `GameTime` dependency as Unity uses raw delta time
- Method now compatible with Unity's `Time.deltaTime` pattern

### 6. **Render Method**
- Changed from `RenderCore()` to `Render()`
- Made it public and override
- Calls `base.Render()` then renders DisconnectUI
- Compatible with Unity's rendering pipeline

### 7. **Main Method**
- Added `[Conditional("STANDALONE_BUILD")]` attribute to prevent execution in Unity
- Updated error logging to use `UnityEngine.Debug.LogError`
- Removed `RunWithLogging()`, `RunThread.WaitAll()`, and `Dispose()` calls
- Added comment explaining Unity's MonoBehaviour lifecycle handles the update loop

### 8. **ReloadLevels Method**
- Changed from throwing `ArgumentNullException` to logging a warning
- Added debug logging for area reload
- More graceful handling of null area parameter

### 9. **ReloadPortraits and ReloadDialog**
- Changed from empty methods to methods with debug logging
- Indicates these features are not yet implemented

### 10. **CallProcess Method**
- Wrapped entire method in try-catch
- Added error logging for failed process calls

### 11. **PauseAnywhere Method**
- Simplified switch statement using pattern matching
- Removed redundant type checks and casts
- More modern C# 9.0 syntax

## Unity Integration Points

### How to Use in Unity

1. **Create a CelesteManager MonoBehaviour:**
```csharp
using UnityEngine;

public class CelesteManager : MonoBehaviour
{
    private Celeste.Celeste _celeste;

    void Awake()
    {
        // Initialize settings first
        Celeste.Settings.Initialize();
        
        // Create Celeste instance
        _celeste = new Celeste.Celeste();
        _celeste.Initialize();
    }

    void Start()
    {
        _celeste.LoadContent();
    }

    void Update()
    {
        _celeste.Update(Time.deltaTime);
    }

    void OnApplicationQuit()
    {
        _celeste.Dispose();
        Celeste.Audio.Unload();
    }
}
```

2. **Attach to GameObject:**
   - Create an empty GameObject in your scene
   - Add the `CelesteManager` component
   - Ensure it persists across scenes if needed

## Compatibility Notes

### Maintained Compatibility:
- All public static members remain unchanged
- Event system unchanged
- Scene management unchanged
- Constants unchanged
- Steam integration points preserved (when ENABLE_STEAM is defined)

### Unity-Specific Adaptations:
- Uses Unity's logging system instead of Console.WriteLine
- Uses Unity's Time.deltaTime instead of GameTime
- Content directory points to StreamingAssets or Assets/Content
- Screen resolution changes use Unity's Screen API
- Application exit uses Unity's Application.Quit

## Dependencies

The adapted Celeste class depends on these Unity-adapted components:

1. **Engine** (base class) - Adapted in `Assets/Scripts/Monocle/Engine.cs`
2. **Input** - Uses Unity Input System mapping
3. **Audio** - Needs Unity audio backend
4. **GFX, MTN, OVR** - Content loading systems
5. **VirtualContent** - Asset management system
6. **Settings** - Game settings management

## Testing Recommendations

1. Test initialization in Unity Editor
2. Verify content loading from StreamingAssets
3. Test scene transitions
4. Verify input handling with Unity's new Input System
5. Test pause functionality
6. Verify asset reloading works in Unity context

## Known Limitations

1. Steam integration requires ENABLE_STEAM conditional compilation
2. Some asset loading may need Unity-specific implementations
3. Original standalone build system (Main method) is disabled in Unity builds
4. RunThread functionality needs Unity-compatible implementation

## Future Enhancements

1. Implement full asset reloading for hot-reload support
2. Add Unity Profiler integration
3. Create Unity-specific debug tools
4. Implement proper Unity Asset Bundle loading
5. Add Unity Analytics integration points
