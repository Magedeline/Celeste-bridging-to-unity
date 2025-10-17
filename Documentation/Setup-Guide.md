# Celeste Unity Port Setup Guide

## Overview

This Unity port of Celeste maintains full compatibility with the original XNA Framework, Monocle engine, MonoGame, and FNA implementations while adding modern Unity features.

## Requirements

- Unity 2021 LTS (2021.3.45f2 or later)
- .NET Framework 4.8 or .NET 6+
- FMOD integration for audio
- Original Celeste Content folder (assets not included)

## Installation

### 1. Clone the Repository

```bash
git clone https://github.com/Magedeline/Celeste.git
cd Celeste
```

### 2. Open in Unity

1. Launch Unity Hub
2. Click "Open" and select the Celeste folder
3. Unity will automatically configure the project

### 3. Configure FMOD Integration

1. Download FMOD for Unity from the FMOD website
2. Import the FMOD Unity Integration package
3. Configure FMOD settings in Project Settings > FMOD

### 4. Add Original Content

**Important**: You must own the original game to use these assets.

1. Copy the Content folder from your Celeste installation to the project root
2. In Unity, go to Celeste > Convert Content to Unity Assets
3. Wait for the conversion process to complete

## Unity Features

### Framework Compatibility

- **XNA Framework**: Full compatibility maintained through compatibility layers
- **Monocle Engine**: Unity adapter preserves all Monocle functionality
- **MonoGame**: Cross-platform compatibility maintained
- **FNA**: Native implementation support

### Unity Integration

- **Asset Pipeline**: Converts Content to Unity Assets automatically
- **Physics Integration**: Optional Unity physics mode alongside original systems
- **Input System**: Unity Input System integration with fallback to original
- **Rendering**: Unity rendering pipeline with XNA compatibility
- **Room System**: Seamless room transitions with loading/unloading

### 3D Overworld

- Mountain, bird, moon, building, and heart 3D models
- Advanced camera system with look-ahead
- Dynamic lighting and skybox system
- Maintains 2D compatibility mode

## Modding Support

### EverestAPI Compatibility

The Unity port maintains full compatibility with existing Celeste mods:

1. Place Everest mods in the `StreamingAssets/Mods` folder
2. Enable EverestAPI compatibility in the Celeste Unity Adapter
3. Existing mods will work without modification

### Unity-Specific Modding

New Unity-based modding capabilities:

- **BepInEx Integration**: Unity-specific mod support
- **Prefab System**: Create mods using Unity prefabs
- **Asset Bundles**: Distribute mod content as Unity asset bundles
- **Visual Scripting**: Create mods using Unity's visual scripting

### Map Editor

Unity-based map editor with Loenn compatibility:

1. Open Celeste > Map Editor
2. Create or import existing maps
3. Export to Loenn-compatible format
4. Full compatibility with existing Loenn maps

## Configuration

### Celeste Unity Adapter Settings

Access via the CelesteUnityAdapter component:

- **Maintain XNA Compatibility**: Keep original framework behavior
- **Use Unity Physics**: Enable Unity physics integration
- **Use Unity Input**: Enable Unity Input System
- **Use Unity Rendering**: Use Unity's rendering pipeline
- **Target Frame Rate**: Set desired FPS (default: 60)

### Compatibility Settings

- **Framework Conversion**: Automatic conversion between frameworks
- **Asset Pipeline**: Content to Unity asset conversion
- **Modding Support**: EverestAPI and BepInEx integration

## Building

### Development Build

1. Open Build Settings (Ctrl+Shift+B)
2. Select your target platform
3. Enable "Development Build" for debugging
4. Click Build and Run

### Production Build

1. Disable "Development Build"
2. Set appropriate quality settings
3. Configure platform-specific settings
4. Build for distribution

## Troubleshooting

### Common Issues

**Assets not loading**: Ensure Content folder is properly converted using Celeste > Convert Content to Unity Assets

**Input not working**: Check that the Input System package is installed and configured

**Audio issues**: Verify FMOD integration is properly set up

**Mod compatibility**: Ensure EverestAPI compatibility is enabled in settings

### Performance Optimization

- Use Unity Profiler to identify bottlenecks
- Enable/disable Unity features as needed for performance
- Adjust quality settings for target platform
- Consider using Unity's rendering optimizations

## Development

### Adding Unity Features

To add new Unity-specific features while maintaining compatibility:

1. Create Unity components alongside original systems
2. Use compatibility layers for XNA/Monocle integration
3. Maintain optional fallbacks to original implementations
4. Test with existing mods and content

### Framework Integration

The compatibility system allows seamless integration:

```csharp
// XNA to Unity conversion
Vector2 unityVector = xnaVector.ToUnity();
Color unityColor = xnaColor.ToUnity();

// Unity to XNA conversion  
Microsoft.Xna.Framework.Vector2 xnaVector = unityVector.ToXNA();
Microsoft.Xna.Framework.Color xnaColor = unityColor.ToXNA();
```

## Legal Notice

This is an unofficial port. Please support the original developers by purchasing the official game at https://www.celestegame.com/

All original assets, code, and intellectual property belong to Extremely OK Games.