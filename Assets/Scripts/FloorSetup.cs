using UnityEngine;

/// <summary>
/// Sets up the floor properties according to the Shadow Heist Zone A specifications
/// </summary>
public class FloorSetup : MonoBehaviour
{
    void Start()
    {
        // Get the BoxCollider component
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        
        if (boxCollider != null)
        {
            // Set the size to match our floor dimensions (with a small thickness)
            boxCollider.size = new Vector3(10.0f, 0.1f, 10.0f);
            
            // Position the collider correctly
            boxCollider.center = Vector3.zero;
        }
        
        // Set the correct layer (Layer 9: Environment)
        gameObject.layer = 9;
        
        Debug.Log("Floor setup complete");
    }
}
