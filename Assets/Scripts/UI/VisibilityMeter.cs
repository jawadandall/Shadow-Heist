using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI component for displaying the player's visibility in the stealth system.
/// </summary>
public class VisibilityMeter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ShadowDetector shadowDetector;
    [SerializeField] private Image fillImage;
    
    [Header("Appearance")]
    [SerializeField] private Color invisibleColor = new Color(0f, 0.5f, 1f);
    [SerializeField] private Color visibleColor = new Color(1f, 0.2f, 0.2f);
    [SerializeField] private float pulseSpeed = 2f;
    [SerializeField] private float pulseIntensity = 0.3f;
    
    private void Start()
    {
        // Find shadow detector if not set
        if (shadowDetector == null)
        {
            shadowDetector = FindAnyObjectByType<ShadowDetector>();
        }
        
        // Subscribe to visibility changes
        if (shadowDetector != null)
        {
            shadowDetector.OnVisibilityChanged += UpdateVisibility;
        }
        else
        {
            Debug.LogError("No ShadowDetector found in the scene. Visibility meter will not update.");
        }
    }
    
    private void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks
        if (shadowDetector != null)
        {
            shadowDetector.OnVisibilityChanged -= UpdateVisibility;
        }
    }
    
    private void Update()
    {
        // Add pulse effect when more visible
        if (shadowDetector != null && fillImage != null)
        {
            float visibility = shadowDetector.Visibility;
            
            // Only pulse when somewhat visible
            if (visibility > 0.1f)
            {
                float pulse = Mathf.Sin(Time.time * pulseSpeed) * pulseIntensity * visibility;
                
                // Add pulse to fill amount
                fillImage.fillAmount = visibility + pulse;
            }
            else
            {
                fillImage.fillAmount = visibility;
            }
        }
    }
    
    private void UpdateVisibility(float visibility)
    {
        if (fillImage == null) return;
        
        // Update fill amount
        fillImage.fillAmount = visibility;
        
        // Update color based on visibility
        fillImage.color = Color.Lerp(invisibleColor, visibleColor, visibility);
    }
}