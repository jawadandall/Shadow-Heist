using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Detects if the player is in shadow for the stealth system.
/// </summary>
public class ShadowDetector : MonoBehaviour
{
    [Header("Shadow Detection")]
    [Tooltip("Layers to detect for shadows")]
    [SerializeField] private LayerMask shadowLayers = 1;
    
    [Tooltip("Number of raycasts to use for detection")]
    [SerializeField] private int raycasts = 5;
    
    [Tooltip("Maximum distance to cast rays")]
    [SerializeField] private float maxDistance = 20f;
    
    [Header("Visibility")]
    [Tooltip("How quickly visibility changes (higher = faster)")]
    [SerializeField] private float visibilityTransitionSpeed = 3f;
    
    [Tooltip("Visibility modifier when crouching")]
    [SerializeField] private float crouchVisibilityModifier = 0.7f;
    
    [Header("Debug")]
    [SerializeField] private bool showDebugRays = true;
    [SerializeField] private Color inShadowColor = Color.blue;
    [SerializeField] private Color inLightColor = Color.yellow;
    
    // Current visibility state (0 = invisible, 1 = fully visible)
    private float currentVisibility = 0f;
    
    // Reference to player crouching state
    private bool isCrouching = false;
    
    // Public property for other scripts to access
    public float Visibility => currentVisibility;
    public bool IsInShadow { get; private set; }
    
    // Event for when visibility changes
    public delegate void VisibilityChanged(float newVisibility);
    public event VisibilityChanged OnVisibilityChanged;
    
    private void Update()
    {
        // Calculate if in shadow
        CheckIfInShadow();
        
        // Update visibility based on shadow state
        UpdateVisibility();
    }
    
    public void SetCrouching(bool crouching)
    {
        isCrouching = crouching;
    }
    
    private void CheckIfInShadow()
    {
        // Number of rays hitting a light
        int hitsLight = 0;
        
        // Cast rays in different directions to detect lights
        for (int i = 0; i < raycasts; i++)
        {
            // Calculate angle for this ray
            float angle = (i / (float)raycasts) * 360f;
            Vector3 direction = new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad), 0f);
            
            // Cast ray
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, maxDistance, shadowLayers);
            
            // Draw debug ray
            if (showDebugRays)
            {
                Debug.DrawRay(transform.position, direction * maxDistance, Color.red);
            }
            
            // Check if ray hit a light
            if (hit.collider != null)
            {
                // Check if the collider has a Light2D component
                UnityEngine.Rendering.Universal.Light2D light = hit.collider.GetComponent<UnityEngine.Rendering.Universal.Light2D>();
                if (light != null && light.enabled)
                {
                    hitsLight++;
                }
            }
        }
        
        // Also cast a ray directly upward to check for overhead lights
        RaycastHit2D upHit = Physics2D.Raycast(transform.position, Vector2.up, maxDistance, shadowLayers);
        if (upHit.collider != null)
        {
            UnityEngine.Rendering.Universal.Light2D light = upHit.collider.GetComponent<UnityEngine.Rendering.Universal.Light2D>();
            if (light != null && light.enabled)
            {
                hitsLight++;
            }
        }
        
        // Player is in shadow if no rays hit a light
        IsInShadow = (hitsLight == 0);
        
        // Draw debug color around player
        if (showDebugRays)
        {
            Debug.DrawRay(transform.position, Vector3.up * 0.5f, IsInShadow ? inShadowColor : inLightColor);
        }
    }
    
    private void UpdateVisibility()
    {
        // Calculate target visibility based on shadow state and crouching
        float targetVisibility = IsInShadow ? 0f : 1f;
        
        // Apply crouching modifier if crouching
        if (isCrouching)
        {
            targetVisibility *= crouchVisibilityModifier;
        }
        
        // Smoothly interpolate to target visibility
        currentVisibility = Mathf.Lerp(currentVisibility, targetVisibility, Time.deltaTime * visibilityTransitionSpeed);
        
        // Notify listeners when visibility changes
        OnVisibilityChanged?.Invoke(currentVisibility);
    }
    
    // Alternative method using Light2D components directly
    public void CheckIfInShadowUsingLights()
    {
        // Find all 2D lights in the scene
        UnityEngine.Rendering.Universal.Light2D[] lights = FindObjectsByType<UnityEngine.Rendering.Universal.Light2D>(FindObjectsSortMode.None);
        
        bool inAnyLight = false;
        
        foreach (UnityEngine.Rendering.Universal.Light2D light in lights)
        {
            if (!light.enabled)
                continue;
                
            // Calculate distance to light
            float distance = Vector2.Distance(transform.position, light.transform.position);
            
            // Check if within light radius
            if (distance < light.pointLightOuterRadius)
            {
                // Check if there are obstacles between player and light
                Vector2 directionToLight = (light.transform.position - transform.position).normalized;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToLight, distance, shadowLayers);
                
                // If nothing is blocking the light, player is in light
                if (hit.collider == null)
                {
                    inAnyLight = true;
                    break;
                }
            }
        }
        
        IsInShadow = !inAnyLight;
    }
}