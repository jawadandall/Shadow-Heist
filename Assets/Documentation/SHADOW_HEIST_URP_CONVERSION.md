# Shadow Heist: URP and 2D Lighting Conversion

This document explains how to convert the Shadow Heist project to use Universal Render Pipeline (URP) with 2D lighting for implementing the shadow-based stealth mechanics.

## Files Added

1. **ShadowHeistSetupInstructions.cs**
   - Location: Assets/Scripts/Utilities/
   - Purpose: Detailed step-by-step instructions for conversion
   - Usage: Open the script file to read the instructions

2. **LightConversionHelper.cs**
   - Location: Assets/Scripts/Utilities/
   - Purpose: Gathers and logs information about lights and objects to help with manual conversion
   - Usage: Attach to any GameObject and play the scene to see information in the console

3. **ShadowDetector.cs**
   - Location: Assets/Scripts/Stealth/
   - Purpose: Detects when the player is in shadow for stealth mechanics
   - Usage: Attach to the Player GameObject

4. **LightSwitch.cs**
   - Location: Assets/Scripts/Interaction/
   - Purpose: Handles light switch interaction for toggling lights
   - Usage: Attach to the LightSwitch GameObject and assign a target light

## Implementation Process

### Phase 1: URP Setup (Package and Configuration)

1. Install the Universal Render Pipeline package
2. Create or set up the URP Asset (ShadowHeistURP)
3. Configure Project Settings to use URP
4. Convert materials to be URP-compatible

### Phase 2: 2D Lighting Implementation

1. Run the LightConversionHelper to get information about lights
2. Convert all standard lights to 2D lights using the provided information
3. Add Shadow Caster 2D components to walls and obstacles
4. Create a Global Light for ambient illumination

### Phase 3: Stealth Mechanics Setup

1. Attach the ShadowDetector script to the Player
2. Configure shadow detection parameters
3. Attach the LightSwitch script to the LightSwitch object
4. Set up the Visibility Meter UI (to be implemented)

## Manual Conversion Details

Due to limitations with direct component addition, the conversion requires some manual steps:

### Converting Lights to 2D

For each light (Light1, Light2, Light3, Light4):

1. Select the light GameObject
2. Add Component > Rendering > 2D > Light 2D
3. Set these properties:
   - Light Type: Point
   - Intensity: Same as original light
   - Color: Same as original light
   - Outer Radius: Same as original range
   - Inner Radius: Half of outer radius
   - Enable shadows
4. Disable the original Light component

### Adding Shadow Casters

For each wall/obstacle:

1. Select the GameObject
2. Add Component > Rendering > 2D > Shadow Caster 2D
3. Configure shadow properties as needed

## Scene Testing

After conversion, test the scene:

1. Player should appear fully visible in light
2. Player should be invisible when in shadow areas
3. Light Switch should toggle Light3 on/off
4. Shadow boundaries should be well-defined

## Next Steps After Conversion

1. Integrate the shadow detection with player visibility mechanics
2. Implement the visibility meter UI 
3. Create guard vision and detection systems
4. Add sound detection mechanics
5. Set up interactive objects (hiding spots, etc.)

## Troubleshooting

- If materials appear pink, run the material conversion again
- If 2D lights don't cast shadows, check that shadow casters are properly configured
- If shadow detection doesn't work, check layer settings and ray casting
