# 📁 Celeste Bridge - Project Structure

This document describes the folder organization for the Celeste Bridge Unity project.

---

## 📂 Root Directory Structure

```
Celeste-Bridge-unity/
│
├── 📁 Assets/                    # Unity Assets (Primary working directory)
│   ├── 📁 Scripts/              # Main scripts folder (USE THIS)
│   ├── 📁 Scenes/               # Unity scenes
│   ├── 📁 Prefabs/              # Reusable game objects
│   ├── 📁 Resources/            # Runtime-loaded assets
│   ├── 📁 Sprites/              # 2D sprite assets
│   ├── 📁 Animations/           # Animation controllers & clips
│   ├── 📁 Materials/            # Materials and shaders
│   ├── 📁 2DPhysicMaterial/     # Physics materials for 2D
│   ├── 📁 Plugins/              # Third-party plugins
│   ├── 📁 SoundFX/              # Sound effects
│   ├── 📁 FMODAssets/           # FMOD audio integration
│   └── 📁 TextMesh Pro/         # Text rendering assets
│
├── 📁 Celeste/                   # Original Celeste decompiled code (reference)
├── 📁 Monocle/                   # Monocle engine (original reference)
├── 📁 Content/                   # Game content (levels, data)
├── 📁 FMOD/                      # FMOD Studio project
├── 📁 Documentation/             # Project documentation
├── 📁 DLLs/                      # External dependencies
├── 📁 References/                # Reference assemblies
│
├── 📁 ProjectSettings/           # Unity project configuration
├── 📁 Packages/                  # Unity package manifest
├── 📁 Library/                   # Unity cache (auto-generated)
├── 📁 Logs/                      # Unity logs
└── 📁 UserSettings/              # Local user settings
```

---

## 📜 Scripts Organization

### Main Scripts Location: `Assets/Scripts/`

```
Assets/Scripts/
│
├── 📁 Celeste/                   # Celeste game logic (ported)
│   ├── Actor.cs                  # Base actor class
│   ├── Player.cs                 # Player controller
│   ├── Level.cs                  # Level management
│   ├── Session.cs                # Game session data
│   ├── SaveData.cs               # Save system
│   ├── Trigger.cs                # Trigger base class
│   └── ... (450+ game files)
│
├── 📁 Monocle/                   # Monocle engine (Unity adapted)
│   ├── Engine.cs                 # Core engine
│   ├── Entity.cs                 # ECS base entity
│   ├── Component.cs              # ECS component
│   ├── Scene.cs                  # Scene management
│   ├── Sprite.cs                 # Sprite handling
│   ├── Calc.cs                   # Math utilities
│   └── ... (80+ engine files)
│
├── 📁 Unity/                     # Unity-specific code
│   ├── 📁 AssetPipeline/        # Asset loading/processing
│   ├── 📁 BaseFramework/        # Unity base classes
│   ├── 📁 Compatibility/        # XNA/FNA compatibility layer
│   ├── 📁 Editor/               # Unity Editor scripts
│   ├── 📁 Modding/              # Mod support systems
│   ├── 📁 Overworld3D/          # 3D overworld rendering
│   ├── CelesteUnityAdapter.cs   # Main adapter class
│   └── UnityEngine.cs           # Unity engine bridge
│
├── 📁 SimplexNoise/              # Simplex noise generation
│
└── 📁 FMOD/                      # FMOD audio scripts
```

### Legacy Scripts: `Assets/Script/` (⚠️ Being migrated)

```
Assets/Script/
│
├── 📁 BaseFrameWork/             # Framework utilities
├── 📁 Camera/                    # Camera controllers
├── 📁 CheckPoint/                # Checkpoint system
├── 📁 Object/                    # Game objects
│   ├── ConveyorBlock.cs         # Moving platforms
│   ├── EndObject.cs             # Level end triggers
│   ├── hazardous.cs             # Hazard components
│   ├── 📁 SwitchingBlock/       # Toggle platforms
│   └── 📁 Tutor bird/           # Tutorial bird NPC
│
├── 📁 Player/                    # Player systems
│   ├── 📁 Hair/                 # Hair physics
│   ├── 📁 States/               # State machine states
│   └── 📁 VFX/                  # Visual effects
│
├── 📁 SaveData/                  # Save system (Unity)
└── 📁 UI/                        # UI scripts
```

---

## 🎮 Scenes Organization

### Location: `Assets/Scenes/`

| Scene | Description |
|-------|-------------|
| `MainMenu.unity` | Main menu screen |
| `SaveChose.unity` | Save file selection |
| `ChapterSelector.unity` | Chapter/level selection |
| `Prologue.unity` | Prologue chapter |
| `Chapter 1.unity` | Chapter 1 - Forsaken City |
| `CelesteMain.unity` | Main game scene |
| `MainScene.unity` | Alternative main scene |
| `obstacle.unity` | Testing/obstacle scene |
| `SampleScene.unity` | Unity default sample |

---

## 🖼️ Sprites Organization

### Location: `Assets/Sprites/`

```
Assets/Sprites/
│
├── 📁 Playable Characters/       # Madeline, Badeline sprites
├── 📁 Non-Playable Characters/   # NPCs (Theo, Granny, etc.)
├── 📁 Character Portraits/       # Dialog portraits
├── 📁 Enemies/                   # Enemy sprites
├── 📁 Hazards/                   # Spikes, lava, etc.
├── 📁 Objects/                   # Interactable objects
├── 📁 Tilesets/                  # Level tilesets
├── 📁 User Interface/            # UI elements
├── 📁 Miscellaneous/             # Other sprites
└── Sprites.png                   # Sprite atlas
```

---

## 🏗️ Prefabs Organization

### Location: `Assets/Prefabs/`

```
Assets/Prefabs/
│
├── 📁 GameManager/               # Game managers
├── 📁 Player/                    # Player prefabs
├── 📁 UI/                        # UI prefabs
├── 📁 Tutor Bird/               # Tutorial bird
├── ConveyorBlock.prefab         # Moving platform
└── EndObject.prefab             # Level end trigger
```

---

## 📚 Resources Organization

### Location: `Assets/Resources/`

```
Assets/Resources/
│
├── 📁 ChapterDatas/              # Chapter configuration data
└── StaticChaptersDataManager.asset  # Chapters data manager
```

---

## 🔧 Assembly Definitions

For proper code organization and faster compilation, use these assembly definitions:

| Assembly | Path | Description |
|----------|------|-------------|
| `Celeste.Runtime` | `Assets/Scripts/Celeste` | Core game code |
| `Monocle.Runtime` | `Assets/Scripts/Monocle` | Engine code |
| `Celeste.Unity` | `Assets/Scripts/Unity` | Unity adapter |
| `Celeste.Editor` | `Assets/Scripts/Unity/Editor` | Editor tools |
| `SimplexNoise` | `Assets/Scripts/SimplexNoise` | Noise generation |

---

## 📝 Naming Conventions

### Files
- **Scripts:** PascalCase (e.g., `PlayerController.cs`)
- **Prefabs:** PascalCase (e.g., `PlayerPrefab.prefab`)
- **Scenes:** PascalCase with spaces (e.g., `Chapter 1.unity`)
- **Sprites:** kebab-case or descriptive (e.g., `madeline-idle.png`)

### Folders
- Use PascalCase for new folders
- Keep original folder names from merged repos for compatibility

### Code
- **Classes/Structs:** PascalCase
- **Methods:** PascalCase
- **Private fields:** _camelCase
- **Public fields:** camelCase
- **Constants:** UPPER_SNAKE_CASE

---

## 🔀 Reference Folders (Do Not Modify)

These folders contain reference code from the original decompilation:

| Folder | Purpose |
|--------|---------|
| `/Celeste/` | Original Celeste decompiled source |
| `/Monocle/` | Original Monocle engine source |
| `/orig/` | Original unmodified files backup |
| `/Microsoft/` | XNA Framework reference |

**Note:** Always work with copies in `Assets/Scripts/` rather than modifying reference folders.

---

## 🚀 Getting Started Checklist

When working on this project:

1. ✅ Use `Assets/Scripts/` for all script work
2. ✅ Check `Assets/Script/` for Unity-specific implementations
3. ✅ Reference `/Celeste/` for original game logic
4. ✅ Store new prefabs in appropriate `Assets/Prefabs/` subfolder
5. ✅ Put runtime-loaded assets in `Assets/Resources/`
6. ✅ Create scenes in `Assets/Scenes/`
7. ✅ Keep sprites organized in `Assets/Sprites/` subfolders

---

## 📊 Key Files Reference

| File | Location | Purpose |
|------|----------|---------|
| Player logic | `Assets/Scripts/Celeste/Player.cs` | Main player controller |
| Level management | `Assets/Scripts/Celeste/Level.cs` | Level loading/management |
| Save system | `Assets/Scripts/Celeste/SaveData.cs` | Save/load functionality |
| Unity adapter | `Assets/Scripts/Unity/CelesteUnityAdapter.cs` | XNA-Unity bridge |
| Engine core | `Assets/Scripts/Monocle/Engine.cs` | Game loop & management |

---

*Last updated: January 18, 2026*
