# Celeste Bridge - Contributing Guide

## 📁 Where to Put Things

This guide helps you know where to add new files and assets in the project.

---

## 🎮 Adding New Game Features

### New Game Entity (Enemy, Object, etc.)
1. **Script:** `Assets/Scripts/Celeste/` - Create your entity class
2. **Prefab:** `Assets/Prefabs/` - Create prefab in appropriate subfolder:
   - `Assets/Prefabs/Enemies/` for enemies
   - `Assets/Prefabs/Objects/` for interactable objects
   - `Assets/Prefabs/Hazards/` for hazards
3. **Sprites:** `Assets/Sprites/` - Add sprites in appropriate subfolder

### New Player State
1. **State Script:** `Assets/Script/Player/States/`
2. **Animation:** `Assets/Animations/Player/`

### New Level/Chapter
1. **Scene:** `Assets/Scenes/Chapter X.unity`
2. **Chapter Data:** `Assets/Resources/ChapterDatas/`
3. **Level Content:** `Content/Levels/`

---

## 🔧 Adding New Systems

### New Unity-Specific System
Location: `Assets/Scripts/Unity/`

Examples:
- Save system wrapper → `Assets/Scripts/Unity/SaveSystemWrapper.cs`
- Input system adapter → `Assets/Scripts/Unity/InputAdapter.cs`
- Platform-specific code → `Assets/Scripts/Unity/Compatibility/`

### New Editor Tool
Location: `Assets/Scripts/Unity/Editor/`

All editor scripts should be in this folder to ensure they're only compiled for the Unity Editor.

### New Monocle Engine Extension
Location: `Assets/Scripts/Monocle/`

**Note:** Prefer extending rather than modifying existing Monocle classes.

---

## 🎨 Adding New Assets

### Sprites
```
Assets/Sprites/
├── Playable Characters/      # Player sprites
├── Non-Playable Characters/  # NPCs
├── Enemies/                  # Enemy sprites
├── Hazards/                  # Spikes, lava, etc.
├── Objects/                  # Interactables
├── Tilesets/                 # Level tiles
├── User Interface/           # UI sprites
├── Character Portraits/      # Dialog portraits
└── Miscellaneous/            # Other sprites
```

### Audio
- **Sound Effects:** `Assets/SoundFX/`
- **FMOD Events:** `Assets/FMODAssets/`
- **FMOD Project:** `FMOD/`

### Materials & Shaders
- **Materials:** `Assets/Materials/`
- **Shaders:** `Assets/Shaders/` (create if needed)

---

## 📝 Naming Conventions

### Files
| Type | Convention | Example |
|------|------------|---------|
| Scripts | PascalCase | `PlayerController.cs` |
| Prefabs | PascalCase | `PlayerPrefab.prefab` |
| Scenes | PascalCase + Spaces | `Chapter 1.unity` |
| Sprites | lowercase-kebab | `madeline-idle.png` |
| Materials | PascalCase | `PlayerMaterial.mat` |

### Code
| Type | Convention | Example |
|------|------------|---------|
| Classes | PascalCase | `PlayerController` |
| Methods | PascalCase | `ProcessMovement()` |
| Private fields | _camelCase | `_currentSpeed` |
| Public fields | camelCase | `moveSpeed` |
| Constants | UPPER_SNAKE | `MAX_SPEED` |
| Interfaces | IPascalCase | `IInteractable` |

---

## ⚠️ Important Notes

### Do NOT Modify
- `/Celeste/` - Original decompiled reference
- `/Monocle/` - Original engine reference  
- `/orig/` - Backup files
- `/Microsoft/` - XNA Framework reference

### Work In These Folders
- `Assets/Scripts/` - All active game scripts
- `Assets/Script/` - Unity-specific implementations
- `Assets/Scenes/` - Unity scenes
- `Assets/Prefabs/` - Game prefabs
- `Assets/Resources/` - Runtime-loaded assets

---

## 🔀 Git Workflow

### Branch Naming
- `feature/description` - New features
- `fix/description` - Bug fixes
- `refactor/description` - Code refactoring
- `docs/description` - Documentation

### Commit Messages
```
type(scope): description

Types: feat, fix, docs, style, refactor, test, chore
```

Example: `feat(player): add wall jump ability`

---

## 🧪 Testing

1. Test in Unity Editor first
2. Build and test on target platform
3. Verify original game behavior matches when applicable
4. Check for performance regressions

---

*See [PROJECT_STRUCTURE.md](PROJECT_STRUCTURE.md) for complete folder reference.*
