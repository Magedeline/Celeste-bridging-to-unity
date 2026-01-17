# Git LFS (Large File Storage) Guide

## What is Git LFS?

Git LFS is an extension that replaces large files (such as audio, video, datasets, and graphics) with text pointers inside Git, while storing the actual file contents on a remote server.

## Installation

### Windows
```bash
# Using Git for Windows (includes LFS)
# Or download from: https://git-lfs.github.com/

# After installation, initialize LFS in your user account
git lfs install
```

## Setup in Your Repository

### 1. Track File Types

Tell Git LFS which files to track:

```bash
# Track specific file extensions
git lfs track "*.psd"
git lfs track "*.png"
git lfs track "*.mp3"
git lfs track "*.wav"
git lfs track "*.fbx"
git lfs track "*.unitypackage"

# Track specific files
git lfs track "path/to/large-file.zip"
```

This creates/updates a `.gitattributes` file.

### 2. Commit the .gitattributes File

```bash
git add .gitattributes
git commit -m "Configure Git LFS tracking"
```

## Pushing with Git LFS

### Standard Push

```bash
# Add your files
git add .

# Commit as usual
git commit -m "Add large files"

# Push to remote (LFS files are uploaded automatically)
git push origin main
```

### Push with Verbose Output

```bash
# See LFS upload progress
git push origin main --verbose
```

### Force Push (Use with Caution)

```bash
git push origin main --force
```

## Common Commands

### Check Tracked Patterns

```bash
git lfs track
```

### List LFS Files

```bash
# List all LFS files in the repo
git lfs ls-files

# List all LFS files with size
git lfs ls-files -s
```

### Check LFS Status

```bash
git lfs status
```

### Fetch LFS Files

```bash
# Fetch LFS objects for current branch
git lfs fetch

# Fetch LFS objects for all branches
git lfs fetch --all
```

### Pull LFS Files

```bash
git lfs pull
```

## Troubleshooting

### Authentication Issues

```bash
# Clear cached credentials
git credential reject https://github.com

# Re-authenticate on next push
git push origin main
```

### Verify LFS Installation

```bash
git lfs version
```

### Check LFS Configuration

```bash
git lfs env
```

### Reset LFS Hooks

```bash
git lfs install --force
```

### Large Push Failing

If a push is timing out or failing:

```bash
# Increase buffer size
git config http.postBuffer 524288000

# Retry the push
git push origin main
```

### Migrate Existing Files to LFS

```bash
# Migrate existing files (rewrites history!)
git lfs migrate import --include="*.psd,*.png" --everything
git push origin main --force
```

## Best Practices

1. **Track early**: Set up LFS tracking before adding large files
2. **Use .gitattributes**: Always commit your `.gitattributes` file
3. **Check file sizes**: Review what's being tracked with `git lfs ls-files -s`
4. **Bandwidth limits**: Be aware of your hosting provider's LFS bandwidth/storage limits
5. **Clone with LFS**: Use `git lfs clone` for faster initial clones of LFS repos

## Example Workflow

```bash
# 1. Initialize LFS (first time only)
git lfs install

# 2. Track large file types
git lfs track "*.png"
git lfs track "*.wav"

# 3. Add .gitattributes
git add .gitattributes
git commit -m "Setup LFS tracking"

# 4. Add your large files
git add Assets/Textures/*.png
git add Assets/Audio/*.wav

# 5. Commit
git commit -m "Add game assets"

# 6. Push (LFS handles the large files)
git push origin main
```

## Useful Links

- [Git LFS Official Site](https://git-lfs.github.com/)
- [Git LFS Documentation](https://git-lfs.com/)
- [GitHub LFS Documentation](https://docs.github.com/en/repositories/working-with-files/managing-large-files)
