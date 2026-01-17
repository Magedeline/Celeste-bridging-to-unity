# 📁 Desktop_Mod vs Desktop Folder Separation Guide

## Overview

This guide explains how to maintain separation between the **Desktop** folder (original FMOD audio banks) and the **Desktop_Mod** folder (modded/custom audio banks) in the Celeste Unity port.

---

## 📂 Folder Structure

```
Content/
└── FMOD/
    ├── Desktop/           ← Original Celeste audio banks (DO NOT MODIFY)
    │   ├── Master.bank
    │   ├── Master.strings.bank
    │   ├── Music.bank
    │   └── ...
    │
    └── Desktop_Mod/       ← Custom/modded audio banks (YOUR MODIFICATIONS)
        ├── Custom.bank
        ├── ModMusic.bank
        └── ...
```

---

## ⚠️ Important Rules

### Desktop Folder (Original)
| Rule | Description |
|------|-------------|
| ❌ **Never modify** | Keep original files untouched |
| ❌ **Never delete** | Required for base game functionality |
| ✅ **Read-only** | Treat as immutable reference |
| ✅ **Backup** | Keep a backup copy somewhere safe |

### Desktop_Mod Folder (Modded)
| Rule | Description |
|------|-------------|
| ✅ **All modifications here** | Place custom banks in this folder |
| ✅ **Safe to delete** | Can be reset without affecting base game |
| ✅ **Version control** | Track changes in git |
| ✅ **Override system** | Files here override Desktop equivalents |

---

## 🔧 Setup Instructions

### Step 1: Create the Desktop_Mod Folder

```
Content/FMOD/Desktop_Mod/
```

If it doesn't exist, create it manually or run:
```bash
mkdir -p Content/FMOD/Desktop_Mod
```

### Step 2: Add to .gitignore (Original Files)

Add this to your `.gitignore` to exclude the original Desktop folder (copyrighted content):

```gitignore
# Original FMOD banks (copyrighted - do not commit)
Content/FMOD/Desktop/

# Keep Desktop_Mod tracked for your custom work
!Content/FMOD/Desktop_Mod/
```

### Step 3: Configure Audio Loading Priority

The audio system loads banks in this priority order:

1. **Desktop_Mod/** - Checked first (your custom banks)
2. **Desktop/** - Fallback to original banks

---

## 🎵 How Audio Loading Works

```csharp
// In Audio.cs - Bank loading logic
string modPath = Path.Combine(Engine.ContentDirectory, "FMOD", "Desktop_Mod", name);
string originalPath = Path.Combine(Engine.ContentDirectory, "FMOD", "Desktop", name);

// Priority: Desktop_Mod first, then Desktop
if (File.Exists(modPath))
    LoadBank(modPath);  // Custom bank
else
    LoadBank(originalPath);  // Original bank
```

---

## 📋 Best Practices

### 1. Naming Conventions

| Type | Naming Convention | Example |
|------|-------------------|---------|
| Override banks | Same name as original | `Music.bank` |
| New custom banks | Unique prefix | `Mod_CustomMusic.bank` |
| Test banks | Test prefix | `Test_Experimental.bank` |

### 2. File Organization

```
Desktop_Mod/
├── overrides/          ← Banks that replace originals
│   └── Music.bank
├── additions/          ← Completely new banks
│   └── Mod_NewSounds.bank
└── experimental/       ← Work in progress
    └── WIP_Testing.bank
```

### 3. Version Control Strategy

```gitignore
# .gitignore recommendations

# Ignore original copyrighted content
Content/FMOD/Desktop/

# Track your modifications
Content/FMOD/Desktop_Mod/**

# Ignore temporary/experimental files
Content/FMOD/Desktop_Mod/experimental/
Content/FMOD/Desktop_Mod/*.tmp
```

---

## 🔄 Workflow Examples

### Adding a Custom Sound Bank

1. Create your bank using FMOD Studio
2. Export to `Content/FMOD/Desktop_Mod/`
3. Test in-game
4. Commit to version control

### Overriding Original Music

1. Copy naming convention from `Desktop/Music.bank`
2. Create your modified `Music.bank` in FMOD Studio
3. Place in `Desktop_Mod/Music.bank`
4. The mod version will load instead of original

### Resetting to Original State

```bash
# Simply delete or rename Desktop_Mod folder
rm -rf Content/FMOD/Desktop_Mod/
# or
mv Content/FMOD/Desktop_Mod/ Content/FMOD/Desktop_Mod_backup/
```

---

## 🛠️ Troubleshooting

### Problem: Mod audio not loading
**Solution**: Check that the file name matches exactly (case-sensitive)

### Problem: Original audio playing instead of mod
**Solution**: Verify `Desktop_Mod` folder path is correct and file exists

### Problem: Audio errors on startup
**Solution**: Ensure bank files are valid FMOD banks (not corrupted)

### Problem: Can't find Desktop_Mod folder
**Solution**: Create it manually at `Content/FMOD/Desktop_Mod/`

---

## 📝 Quick Reference

| Action | Desktop | Desktop_Mod |
|--------|---------|-------------|
| Modify files | ❌ No | ✅ Yes |
| Delete files | ❌ No | ✅ Yes |
| Add new files | ❌ No | ✅ Yes |
| Commit to git | ❌ No (copyrighted) | ✅ Yes |
| Required for game | ✅ Yes | ❌ No |

---

## 🔗 Related Documentation

- [Setup Guide](Setup-Guide.md) - Initial project setup
- [Unity Integration](Unity-Integration.md) - Unity-specific features
- [FMOD Documentation](https://fmod.com/docs) - Official FMOD docs

---

> **Remember**: Always keep original `Desktop` files untouched. All your custom work should go in `Desktop_Mod`. This ensures you can always restore the base game and cleanly manage your modifications.
