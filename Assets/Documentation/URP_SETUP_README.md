# Shadow Heist URP and 2D Lighting Setup

This document provides instructions for implementing URP and 2D lighting in the Shadow Heist project to enable the shadow-based stealth mechanics.

## Implementation Steps

### 1. Install Universal Render Pipeline Package

First, install the Universal Render Pipeline package through the Package Manager:

1. Open Window > Package Manager
2. Click the "+" button in the top-left corner
3. Select "Add package from git URL..."
4. Enter: `com.unity.render-pipelines.universal`
5. Click "Add"

### 2. Set Up URP Pipeline Asset

Unity needs to be configured to use the URP assets:

1. Navigate to Edit > Project Settings > Graphics
2. In the "Scriptable Render Pipeline Settings" field, select the "UniversalRP" asset from the Settings folder

### 3. Convert Existing Materials

All materials need to be converted to be URP-compatible:

1. Navigate to Edit > Render Pipeline > Universal Render Pipeline > Upgrade Project Materials to URP Materials
2. Click "Proceed" when prompted

### 4. Convert 3D Lights to 2D Lights

Use the provided utility to convert existing lights:

1. Select Shadow Heist > Convert To 2D Lights from the menu bar
2. This will automatically convert all scene lights to 2D lights and disable the original lights
3. Fine-tune light settings as needed

### 5. Set Up Shadow Casters

Add shadow caster components to wall objects:

1. Select Shadow Heist > Setup Shadow Casters from the menu bar
2. This will add ShadowCaster2D components to appropriate objects
3. Adjust the shadow shapes manually if needed for precise shadows

### 6. Verify Shadow Detection

Test the shadow detection system:

1. Make sure the player object has the ShadowDetector script attached
2. Add the VisibilityMeter script to your UI canvas
3. Test the scene to verify shadow detection is working

## Features Added

This implementation adds the following key components:

- **Universal Render Pipeline**: Foundation for modern rendering and 2D lighting
- **2D Light System**: Converts standard lights to 2D lights with proper shadow casting
- **Shadow Detection**: System to detect when the player is in shadow areas
- **Light Switch Interaction**: Interactive light switches for creating stealth opportunities 
- **Visibility Meter UI**: UI component to display player visibility

## Troubleshooting

- **Pink Materials**: If materials appear pink, select Edit > Render Pipeline > Universal Render Pipeline > Upgrade Project Materials to URP Materials again
- **Missing Shadows**: Ensure objects have ShadowCaster2D components and are on the correct layer
- **Light Issues**: Check that 2D lights have appropriate Outer Radius and Shadow settings
- **Shadow Detection Problems**: Verify Layer Masks in the ShadowDetector script

## Next Steps

After URP setup, proceed to:

1. Implement the player controller with shadow interaction
2. Set up the zone-aware camera system
3. Complete the interactive elements and triggers
4. Add the visibility meter UI to the game HUD