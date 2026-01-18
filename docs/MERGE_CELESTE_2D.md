# Merge Documentation: celeste-2d-pc-version

This document records the merge of [AdamNbz/celeste-2d-pc-version](https://github.com/AdamNbz/celeste-2d-pc-version) into the Celeste Bridge Unity project.

## Merge Information

| Property | Value |
|----------|-------|
| **Date** | January 18, 2026 |
| **Source Repository** | `https://github.com/AdamNbz/celeste-2d-pc-version.git` |
| **Source Branch** | `main` |
| **Target Branch** | `master` |
| **Merge Branch** | `merge-celeste-2d-pc` |
| **Merge Type** | Unrelated histories merge |

## Commands Used

```bash
# Add remote
git remote add celeste-2d https://github.com/AdamNbz/celeste-2d-pc-version.git

# Fetch from remote
git fetch celeste-2d

# Create merge branch
git checkout -b merge-celeste-2d-pc

# Merge with unrelated histories
git merge celeste-2d/main --allow-unrelated-histories --no-commit
```

## Conflict Resolution

The following conflicts were resolved:

### Kept Ours (Original Project)

| File | Reason |
|------|--------|
| `ProjectSettings/AudioManager.asset` | Preserve Unity project configuration |
| `ProjectSettings/DynamicsManager.asset` | Preserve Unity project configuration |
| `ProjectSettings/EditorBuildSettings.asset` | Preserve Unity project configuration |
| `ProjectSettings/EditorSettings.asset` | Preserve Unity project configuration |
| `ProjectSettings/GraphicsSettings.asset` | Preserve Unity project configuration |
| `ProjectSettings/InputManager.asset` | Preserve Unity project configuration |
| `ProjectSettings/NavMeshAreas.asset` | Preserve Unity project configuration |
| `ProjectSettings/PackageManagerSettings.asset` | Preserve Unity project configuration |
| `ProjectSettings/Physics2DSettings.asset` | Preserve Unity project configuration |
| `ProjectSettings/ProjectSettings.asset` | Preserve Unity project configuration |
| `ProjectSettings/QualitySettings.asset` | Preserve Unity project configuration |
| `ProjectSettings/TagManager.asset` | Preserve Unity project configuration |
| `ProjectSettings/TimeManager.asset` | Preserve Unity project configuration |
| `ProjectSettings/UnityConnectSettings.asset` | Preserve Unity project configuration |
| `ProjectSettings/VFXManager.asset` | Preserve Unity project configuration |
| `ProjectSettings/VersionControlSettings.asset` | Preserve Unity project configuration |
| `.gitignore` | More comprehensive ignore rules |
| `.vscode/settings.json` | Development environment preferences |

### Manually Merged

| File | Resolution |
|------|------------|
| `README.md` | Combined content from both projects with merge documentation |

## New Files Added

The following directories and files were added from celeste-2d-pc-version:

### Scripts (Assets/Scripts/)
- Player mechanics and state machine
- Movement systems (walking, jumping, dashing, climbing)
- Camera systems
- Game managers
- UI controllers

### Sprites (Assets/Sprites/)
- Character sprites (Madeline, Badeline)
- Tileset assets
- Background elements
- UI elements

### Scenes (Assets/Scenes/)
- Chapter 1 levels
- Menu scenes

### Audio (Assets/Audio/ & FMODAssets/)
- FMOD audio integration
- Sound effects
- Music triggers

### Other
- `save/` - Save system data
- `LICENSE` - MIT License from celeste-2d-pc-version
- `celeste-2d-pc-version.slnx` - Solution file

## Post-Merge Tasks

After merging, the following tasks should be completed:

1. **Test Unity Project**
   - Open project in Unity 6
   - Check for compilation errors
   - Test scene loading

2. **Resolve Asset Conflicts**
   - Check for duplicate assets
   - Merge animation controllers if needed
   - Consolidate sprite atlases

3. **Update Package Dependencies**
   - Verify `Packages/manifest.json` is correct
   - Install any missing packages

4. **Test Gameplay**
   - Test player movement
   - Test level loading
   - Test audio playback

## Git Remote Configuration

After merge, remotes should be configured as:

```bash
origin      https://github.com/Magedeline/Celeste-bridging-to-unity.git  (fetch/push)
upstream    https://github.com/BloodLantern/Celeste.git                  (fetch/push)
celeste-2d  https://github.com/AdamNbz/celeste-2d-pc-version.git         (fetch/push)
```

## Syncing with Upstream Repositories

### Pull updates from celeste-2d-pc-version
```bash
git fetch celeste-2d
git merge celeste-2d/main --allow-unrelated-histories
# Resolve any conflicts
git commit
```

### Pull updates from BloodLantern/Celeste
```bash
git fetch upstream
git merge upstream/master
# Resolve any conflicts
git commit
```

## Notes

- Both projects target Unity but have different implementations
- The celeste-2d-pc-version project is a simplified 2D clone
- The Celeste Bridge project aims for full original game compatibility
- Merged assets may need manual integration into existing systems
