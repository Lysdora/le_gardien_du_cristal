# Artefact Tower - Unity 2D Project

## Project Overview

This is a Unity 2022.3.62f3 2D game project using Universal Render Pipeline (URP). The game appears to be a tower defense or action game with player-controlled characters avoiding or fighting monsters.

## Architecture & Core Systems

### Component Structure

- **PlayerController**: Rigidbody2D-based movement using `Input.GetAxisRaw()` for WASD/arrow controls. Uses physics-based movement in `FixedUpdate()` with normalized diagonal movement to prevent speed boost.
- **MonsterController**: Simple AI with left-moving transform-based movement (no physics). Moves at constant speed of 2 units/second.
- **CameraController**: Follows player using tag-based lookup with horizontal clamping (minX: -7, maxX: 40). Updates in `LateUpdate()` to avoid jitter.

### Key Conventions

- **Movement Pattern**: Player uses `Rigidbody2D.velocity` (physics-based), monsters use `transform.position` (direct manipulation). This is intentional for different gameplay behaviors.
- **Update Cycles**: Movement logic lives in `FixedUpdate()` for physics consistency. Input reading happens in `Update()` (PlayerController). Camera follows in `LateUpdate()`.
- **French Comments**: Code contains French debug messages (e.g., "Ajoute le tag player à ton player !"). Maintain this bilingual approach.

## Project Structure

```
Assets/
  ├── Scripts/          # All MonoBehaviour components
  ├── Prefabs/          # Player.prefab, Monster.prefab
  ├── Animations/       # PlayerAnimations/, Monstre/ folders
  ├── Sprites/          # Character and environment sprites (player1-4, monster, artefact, gemme, etc.)
  ├── Tilemaps/Ground/  # Terrain system
  └── Scenes/           # SampleScene.unity (main scene)
```

## Rendering & Layers

- **Sorting Layers** (back to front): Default → Objects → Player → Monstre
- **URP Renderer**: Using Universal Render Pipeline 14.0.12
- **2D Physics**: Standard gravity (y: -9.81), though player movement ignores it via velocity override

## Development Workflow

### Opening the Project

Open the project folder in Unity 2022.3.x. The main scene is `Assets/Scenes/SampleScene.unity`.

### Creating New Scripts

- Place all gameplay scripts in `Assets/Scripts/`
- Use standard Unity namespaces: `UnityEngine`, `System.Collections`
- Public variables for Inspector tweaking, private for internal state
- Use `[SerializeField]` for private Inspector fields (see CameraController's minX/maxX)

### Adding Components

- **Player entities**: Require `Rigidbody2D`, tag "Player", layer "Player"
- **Monsters**: No Rigidbody2D needed (transform-based), layer "Monstre"
- **Camera**: Attach to Main Camera, finds player via tag at runtime

### Critical Tags

The "Player" tag is essential for camera following. Missing tag triggers French debug log.

## Common Patterns

### Normalized Diagonal Movement

```csharp
// From PlayerController - prevents faster diagonal movement
rb.velocity = moveInput.sqrMagnitude > 1f
    ? moveInput.normalized * moveSpeed
    : moveInput * moveSpeed;
```

### Clamped Camera Following

```csharp
// From CameraController - follows X only, respects bounds
tempPos.x = Mathf.Clamp(tempPos.x, minX, maxX);
```

## Testing & Debugging

- Use Unity Play mode for testing
- Check Console for French debug messages
- Verify player tag assignment if camera doesn't follow
- Sprite rendering order follows sorting layer hierarchy (Player > Monstre)

## Dependencies

- **Unity Version**: 2022.3.62f3 (strict)
- **Key Packages**: URP 14.0.12, TextMeshPro 3.0.7, 2D Feature 2.0.1
- **No custom packages** beyond Unity registry
