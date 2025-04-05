# Camera Setup for Shadow Heist

This document explains how to set up the camera system to follow the player.

## Camera Setup Instructions

1. **Select your main camera** in the scene (usually named "Main Camera")

2. **Add the CameraFollow script** to the Main Camera

3. **Configure the camera settings** in the Inspector:
   - Target: Drag your Player GameObject here (if left empty, it will automatically find a GameObject with the "Player" tag)
   - Smooth Speed: Controls how quickly the camera follows the player (default: 0.125)
   - Offset: Position offset from the player (default: 0, 0, -10)
   - Orthographic Size: Controls the zoom level (default: 25)

4. **Ensure your Player has the "Player" tag** if you want automatic targeting

## Adjusting for Level Design

When designing your stealth levels, you may want to adjust the camera settings:

1. **For larger areas**:
   - Increase the Orthographic Size to show more of the environment
   - Adjust the Smooth Speed for faster camera movement

2. **For tight corridors**:
   - Decrease the Orthographic Size to focus more on the player
   - Consider a smaller Smooth Speed for more precise camera movement

## Runtime Adjustments

You can adjust the camera zoom at runtime using code:

```csharp
// Find the camera and adjust zoom
CameraFollow cameraFollow = Camera.main.GetComponent<CameraFollow>();
if (cameraFollow != null)
{
    cameraFollow.SetZoom(5f); // Set to a new zoom level
}
```

This can be useful for certain gameplay sequences or to highlight specific areas.
