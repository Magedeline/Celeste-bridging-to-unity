# Celeste XNA Version - Essential Files and Structure

This repository contains multiple versions and adaptations of Celeste. If you want to keep only the XNA version, here are the most important parts to retain:

## What to Keep

1. **Core Game Logic**
   - All gameplay code in the `Celeste/` directory (player, levels, collectibles, etc.)
   - The `Monocle/` directory (game engine code, if present)

2. **XNA-Specific Runtime Code**
   - `XNA.Runtime.csproj` (main XNA project file)
   - Any other `.csproj` files or code referencing XNA

3. **Content Pipeline Assets**
   - The `Content/` directory (sprites, sounds, levels, etc.)

4. **Project and Solution Files**
   - `Celeste-Bridge-unity.sln` or `Celeste.sln` (main solution file)
   - Any `.csproj` files required for XNA

5. **Essential Dependencies**
   - DLLs or libraries required for XNA (in `DLLs/` or `References/`)

## What to Exclude

- Unity-specific code and projects (`Unity.Celeste.Bridge.csproj`, `Celeste.Unity.csproj`, `Celeste.Unity.Editor.csproj`, `Assets/`, `ProjectSettings/`, etc.)
- Editor-only or Unity-only scripts and assets
- FMOD or other middleware if not used in XNA
- Documentation and meta files not needed for the XNA build

## Summary

**Keep:**
- `Celeste/`
- `Monocle/`
- `XNA.Runtime.csproj`
- `Content/`
- `Celeste-Bridge-unity.sln` or `Celeste.sln`
- Required DLLs/libraries

**Remove:**
- Unity-specific files and folders
- Unused middleware and documentation

This will leave you with a clean XNA version of Celeste, ready for building and running with the XNA framework.