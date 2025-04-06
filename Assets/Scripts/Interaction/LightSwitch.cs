using UnityEngine;


/// <summary>
/// Handles light switch interaction for the Shadow Heist stealth system.
/// </summary>
public class LightSwitch : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("The light that this switch controls")]
    [SerializeField] private UnityEngine.Rendering.Universal.Light2D targetLight;
    
    [Tooltip("Distance within which the player can interact with the switch")]
    [SerializeField] private float interactionRange = 1.5f;
    
    [Tooltip("Key used to interact with the switch")]
    [SerializeField] private KeyCode interactionKey = KeyCode.E;
    
    [Header("References")]
    [Tooltip("Optional text mesh for interaction prompt")]
    [SerializeField] private TextMesh promptText;
    
    private Transform playerTransform;
    private bool playerInRange = false;
    
    private void Start()
    {
        // Find player if not set already
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
            }
        }
        
        // Find the target light if not set
        if (targetLight == null)
        {
            // Try finding by name convention (e.g., if switch is named "LightSwitch3", look for "Light3")
            string switchName = gameObject.name;
            string lightName = switchName.Replace("Switch", "");
            
            GameObject lightObj = GameObject.Find(lightName);
            if (lightObj != null)
            {
                targetLight = lightObj.GetComponent<UnityEngine.Rendering.Universal.Light2D>();
            }
            
            // If still not found, look for nearest light
            if (targetLight == null)
            {
                UnityEngine.Rendering.Universal.Light2D[] lights = FindObjectsByType<UnityEngine.Rendering.Universal.Light2D>(FindObjectsSortMode.None);
                float closestDistance = float.MaxValue;
                
                foreach (UnityEngine.Rendering.Universal.Light2D light in lights)
                {
                    float distance = Vector3.Distance(transform.position, light.transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        targetLight = light;
                    }
                }
            }
        }
        
        // Hide prompt initially
        if (promptText != null)
        {
            promptText.gameObject.SetActive(false);
        }
    }
    
    private void Update()
    {
        if (playerTransform == null || targetLight == null)
            return;
            
        // Check if player is in range
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        playerInRange = distanceToPlayer <= interactionRange;
        
        // Show/hide interaction prompt
        if (promptText != null)
        {
            promptText.gameObject.SetActive(playerInRange);
        }
        
        // Handle interaction
        if (playerInRange && Input.GetKeyDown(interactionKey))
        {
            ToggleLight();
        }
    }
    
    public void ToggleLight()
    {
        if (targetLight != null)
        {
            targetLight.enabled = !targetLight.enabled;
            
            // Play sound effect if available
            AudioSource audioSource = GetComponent<AudioSource>();
            if (audioSource != null && audioSource.clip != null)
            {
                audioSource.Play();
            }
        }
    }
    
    // Visualize the interaction range in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
        
        if (targetLight != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, targetLight.transform.position);
        }
    }
}