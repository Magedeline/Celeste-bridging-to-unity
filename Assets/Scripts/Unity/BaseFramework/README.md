# Celeste Unity Bridge

A Unity 2D bridge for Celeste game mechanics, based on the [AdamNbz/celeste-2d-pc-version](https://github.com/AdamNbz/celeste-2d-pc-version) framework.

## Overview

This bridge allows you to use Celeste's precise platformer mechanics in Unity while maintaining compatibility with the original Celeste codebase. The physics constants and behavior are derived from the original Celeste `Player.cs`.

## Structure

```
Assets/Scripts/Unity/BaseFramework/
├── States/
│   ├── State.cs              # Base abstract state class
│   ├── PlayerState.cs        # Base player state with Celeste constants
│   └── Player/
│       ├── Idle.cs           # Standing still on ground
│       ├── Walk.cs           # Moving on ground
│       ├── Jump.cs           # Ascending jump
│       ├── Fall.cs           # Falling/descending
│       ├── Dash.cs           # Quick dash movement
│       ├── Climb.cs          # Wall climbing
│       └── Death.cs          # Death state
├── Controllers/
│   └── UnityPlayerController.cs  # Main player controller
├── Manager/
│   └── UnityGameManager.cs   # Game manager singleton
├── Camera/
│   └── UnityCameraController.cs  # Camera follow system
├── VFX/
│   ├── HairMovement.cs       # Celeste-style hair physics
│   └── LandingEffect.cs      # Landing dust effect
└── Objects/
    ├── UnityCheckpoint.cs    # Checkpoint/respawn system
    ├── Hazardous.cs          # Generic hazard
    ├── Spikes.cs             # Directional spike hazard
    ├── ConveyorBlock.cs      # Moving platform
    └── Spring.cs             # Bounce spring
```

## Celeste Physics Constants

The following constants from the original Celeste are used (scaled for Unity):

| Constant | Celeste Value | Unity Scaled |
|----------|---------------|--------------|
| MaxFall | 160 | 16 |
| Gravity | 900 | 90 |
| MaxRun | 90 | 9 |
| RunAccel | 1000 | 100 |
| JumpSpeed | -105 | 10.5 |
| VarJumpTime | 0.2 | 0.2 |
| DashSpeed | 240 | 24 |
| DashTime | 0.15 | 0.15 |
| WallSlideTime | 1.2 | 1.2 |
| ClimbMaxStamina | 110 | 110 |
| WallJumpHSpeed | 130 | 13 |

## Setup

### 1. Player Setup

1. Create a Player GameObject
2. Add components:
   - `Rigidbody2D` (Gravity Scale: 3, Freeze Rotation Z)
   - `BoxCollider2D` or `CapsuleCollider2D`
   - `Animator`
   - `UnityPlayerController`
3. Create child GameObjects:
   - `FootPosition` (for ground detection)
   - `HandPosition` (for wall detection)
4. Configure Input Actions in the inspector

### 2. Camera Setup

1. Add `UnityCameraController` to your Main Camera
2. Configure camera style (Overhead, OffsetFollow, etc.)
3. Set smoothing and bounds as needed

### 3. Game Manager Setup

1. Create an empty GameObject named "GameManager"
2. Add `UnityGameManager` component
3. Assign player prefab
4. Configure respawn settings

### 4. Level Objects

- Add `UnityCheckpoint` to checkpoint triggers
- Add `Hazardous` or `Spikes` to hazardous objects
- Add `Spring` to bounce pads
- Tag the player as "Player"
- Set up "Ground" layer for collision detection

## Input Actions

The player controller uses Unity's new Input System. Configure these actions:

```
MoveAction: Vector2 (WASD/Arrow keys/Left Stick)
JumpAction: Button (Space/A button)
DashAction: Button (Shift/X button)
ClimbAction: Button (Z/LB button)
```

## State Machine

The state machine follows this pattern:

```csharp
// States transition via SetState
if (!playerController.IsOnTheGround())
{
    playerController.SetState(new Fall(playerController));
    return;
}
```

Each state:
- `Enter()` - Called when entering the state
- `Exit()` - Called when leaving the state
- `Update()` - Called every frame
- `FixedUpdate()` - Called every physics update

## Hair System

The hair system creates a trailing physics effect:

```csharp
// Set hair color for different states
hairMovement.SetColor(HairMovement.Colors.Normal);   // Red (has dash)
hairMovement.SetColor(HairMovement.Colors.NoDash);   // Blue (no dash)
hairMovement.SetColor(HairMovement.Colors.TwoDash);  // Pink (2 dashes)
```

## Credits

- Original Celeste by Maddy Makes Games
- Unity bridge inspired by [AdamNbz/celeste-2d-pc-version](https://github.com/AdamNbz/celeste-2d-pc-version)
- Hair movement based on Symbiosinx implementation

## License

This bridge is for educational purposes. Celeste is property of Maddy Makes Games.
