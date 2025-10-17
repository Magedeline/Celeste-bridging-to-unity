# Unity Integration Documentation

## Architecture Overview

The Celeste Unity port uses a layered architecture to maintain compatibility while enabling Unity features:

```
Unity Layer (Optional)
├── Unity Physics
├── Unity Input System  
├── Unity Rendering Pipeline
└── Unity Asset System

Compatibility Layer
├── XNA Framework Adapter
├── Monocle Engine Adapter
├── MonoGame Compatibility
└── FNA Compatibility

Original Celeste Systems
├── Monocle Engine
├── Celeste Game Logic
├── Entity Systems
└── Content Pipeline
```

## Component System

### CelesteUnityAdapter

Main component that bridges Unity and Celeste systems:

```csharp
public class CelesteUnityAdapter : MonoBehaviour
{
    [Header("Compatibility")]
    public bool maintainXNACompatibility = true;
    public bool useUnityPhysics = false;
    public bool useUnityInput = false;
    public bool useUnityRendering = false;
}
```

### UnityMonocleEngine

Adapts the Monocle engine to work within Unity:

- Manages Unity/Monocle update loops
- Handles rendering pipeline integration
- Maintains entity system compatibility

## Physics Integration

### Hybrid Physics System

The port supports both original Celeste physics and Unity physics:

```csharp
// Original Celeste physics (default)
public class Player : Actor
{
    // Uses original collision detection and movement
}

// Unity physics integration (optional)
public class UnityPlayer : MonoBehaviour
{
    // Can use Unity Rigidbody and Colliders
    // Syncs with original Player entity
}
```

### Configuration

```csharp
// Enable Unity physics for specific entities
celesteAdapter.useUnityPhysics = true;

// Maintain original physics for gameplay precision
celesteAdapter.maintainXNACompatibility = true;
```

## Input System

### Dual Input Support

Supports both original input system and Unity Input System:

```csharp
// Original input (always available)
if (Input.Jump.Pressed)
{
    player.Jump();
}

// Unity Input System (when enabled)
if (jumpAction.triggered)
{
    player.Jump();
}
```

### Input Mapping

Unity Input Actions can be mapped to Celeste controls:

- Jump → Space, Gamepad South
- Dash → X, Gamepad East  
- Grab → C, Gamepad West
- Menu → Escape, Gamepad Start

## Rendering Pipeline

### Rendering Modes

1. **XNA Compatibility Mode** (Default)
   - Uses original SpriteBatch rendering
   - Maintains pixel-perfect graphics
   - Full compatibility with existing code

2. **Unity Rendering Mode** (Optional)
   - Uses Unity's rendering pipeline
   - Enables post-processing effects
   - Supports Unity's lighting system

### Render Targets

```csharp
// Render to Unity RenderTexture for compatibility
public RenderTexture celesteRenderTarget;

// Can be displayed in Unity UI or processed further
rawImage.texture = celesteRenderTarget;
```

## Asset Pipeline

### Content Conversion

Automatic conversion from Celeste Content to Unity Assets:

```
Content/Graphics/Atlases/ → Assets/Textures/Atlases/
Content/Audio/ → Assets/Audio/
Content/Maps/ → Assets/Maps/
Content/Data/ → Assets/Data/
```

### Asset Processing

```csharp
[AssetPostprocessor]
public class CelesteAssetProcessor
{
    void OnPreprocessTexture()
    {
        // Configure texture import for pixel art
        TextureImporter importer = assetImporter as TextureImporter;
        importer.filterMode = FilterMode.Point;
        importer.compressionQuality = 100;
    }
}
```

## Camera System

### Celeste Camera Integration

The original Celeste camera system is preserved while adding Unity camera features:

```csharp
public class CelesteCamera : MonoBehaviour
{
    // Syncs with original Monocle camera
    public Camera unityCamera;
    private Monocle.Camera monocleCamera;
    
    void Update()
    {
        // Sync Unity camera with Monocle camera
        SyncCameraTransforms();
    }
}
```

### 3D Mountain Integration

```csharp
public class MountainRenderer3D : MonoBehaviour
{
    // Renders 3D mountain while maintaining 2D compatibility
    public RenderTexture renderTo2D;
    
    void OnRenderObject()
    {
        // Render 3D mountain to texture for 2D system
        if (maintain2DCompatibility)
        {
            mountainCamera.targetTexture = renderTo2D;
        }
    }
}
```

## Entity System

### Entity Bridge

Connects Unity GameObjects with Celeste Entities:

```csharp
public class EntityBridge : MonoBehaviour
{
    public Entity celesteEntity;
    
    void Update()
    {
        // Sync Unity transform with Celeste entity
        if (celesteEntity != null)
        {
            transform.position = celesteEntity.Position.ToUnity();
            transform.rotation = Quaternion.Euler(0, 0, celesteEntity.Rotation);
        }
    }
}
```

### Component Mapping

Unity components can be mapped to Celeste components:

- Transform ↔ Position/Rotation
- Rigidbody ↔ Actor movement
- Collider ↔ Hitbox/Hurtbox
- Renderer ↔ Sprite/Image

## Scene Management

### Room Wrapper System

Seamless room transitions with Unity scene management:

```csharp
public class RoomWrapper : MonoBehaviour
{
    public string roomName;
    public LevelData levelData;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TransitionToRoom(roomName);
        }
    }
}
```

### Loading System

Asynchronous room loading with Unity's scene system:

```csharp
public class CelesteSceneManager : MonoBehaviour
{
    public async void LoadRoomAsync(string roomName)
    {
        // Load Unity scene for room
        await SceneManager.LoadSceneAsync(roomName, LoadSceneMode.Additive);
        
        // Initialize Celeste level data
        InitializeCelesteRoom(roomName);
    }
}
```

## Performance Optimization

### Batching

Unity's batching system for sprites:

```csharp
// Enable sprite batching for Celeste sprites
SpriteRenderer[] sprites = FindObjectsOfType<SpriteRenderer>();
foreach (var sprite in sprites)
{
    sprite.material.enableInstancing = true;
}
```

### Culling

Frustum culling integration:

```csharp
public class CelesteCulling : MonoBehaviour
{
    void Update()
    {
        // Cull entities outside camera view
        CullEntitiesOutsideFrustum();
    }
}
```

## Debugging

### Unity Debugger Integration

```csharp
#if UNITY_EDITOR
public class CelesteDebugger : MonoBehaviour
{
    void OnDrawGizmos()
    {
        // Draw Celeste entity bounds
        foreach (Entity entity in Scene.Tracker.Entities)
        {
            Gizmos.DrawWireCube(entity.Position.ToUnity(), entity.Size.ToUnity());
        }
    }
}
#endif
```

### Console Commands

Unity console integration:

```csharp
[ConsoleCommand("celeste_debug")]
public static void ToggleCelesteDebug()
{
    Engine.Commands.Enabled = !Engine.Commands.Enabled;
}
```