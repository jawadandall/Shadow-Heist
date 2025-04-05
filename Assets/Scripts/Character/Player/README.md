# Player Controller Setup

This document explains how to set up and use the player controller for Shadow Heist.

## Setup Instructions

1. Create an empty GameObject in your scene and name it "Player"
2. Add the following components to the Player GameObject:
   - Rigidbody2D (set to Kinematic)
   - Box Collider 2D (adjust size to match your sprites)
   - SpriteRenderer
   - PlayerController script
   - PlayerSpriteSetup script
   - SpriteNormalizer script
   - SpriteNormalizerSetup script

3. Once you've added all the components, right-click on the PlayerSpriteSetup component in the Inspector and select "Assign Player Sprites"
   - This will automatically find and assign all the character sprites based on their filenames

4. Right-click on the SpriteNormalizerSetup component and select "Set Standing Idle as Reference"
   - This will set the standing idle sprite as the reference for normalizing all other sprites

5. Configure the movement settings in the PlayerController component:
   - Walk Speed: Normal walking speed (default: 3.0)
   - Run Speed: Running speed (default: 5.0)
   - Crouch Walk Speed: Speed when crouched (default: 1.5)

## Controls

- **WASD or Arrow Keys**: Move the player
- **Left/Right Shift**: Hold to run
- **Left/Right Control**: Toggle crouch mode

## Sprite System

The controller will automatically change sprites based on:
- Movement direction (side or away/towards)
- Movement state (idle, walking, running, crouching)
- Facing direction (sprites will be mirrored when facing left)

## Sprite Normalization

The SpriteNormalizer component ensures all sprites maintain consistent dimensions:

1. **Reference Sprite**: By default, this is set to the standing idle sprite
2. **Global Scale**: You can adjust this value to scale all sprites up or down while maintaining their relative proportions

### Analyzing Sprite Dimensions

To see the current dimensions of all sprites, right-click on the SpriteNormalizerSetup component and select "Analyze All Sprites". This will print the pixel dimensions, pixels per unit, and Unity unit dimensions of each sprite to the console.

## Customization

You can manually assign different sprites in the Inspector if needed, or adjust the movement speeds to fit your gameplay needs.
