using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Debug visualization tool for shadow boundaries.
/// Attaching this to an empty GameObject will visualize shadow boundaries in the scene.
/// </summary>
public class ShadowBoundaryVisualizer : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool visualizeInGame = true;
    [SerializeField] private float raycastDistance = 20f;
    [SerializeField] private float boundarySphereSize = 0.15f;
    
    [Header("Colors")]
    [SerializeField] private Color shadowColor = new Color(0f, 0f, 1f, 0.3f);
    [SerializeField] private Color lightColor = new Color(1f, 1f, 0f, 0.3f);
    [SerializeField] private Color boundaryColor = new Color(1f, 0f, 1f, 0.8f);
    
    [Header("Layer Masks")]
    [SerializeField] private LayerMask shadowLayers;
    
    // Grid of points to check for shadows
    private Vector3[,] gridPoints;
    private bool[,] inShadow;
    
    // List of boundary points
    private List<Vector3> boundaryPoints = new List<Vector3>();
    
    private void Start()
    {
        if (!visualizeInGame)
            return;
            
        // Generate a grid of points to check for shadows
        int gridSize = Mathf.RoundToInt(raycastDistance * 2);
        gridPoints = new Vector3[gridSize, gridSize];
        inShadow = new bool[gridSize, gridSize];
        
        // Initialize grid positions
        float cellSize = raycastDistance * 2 / gridSize;
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                gridPoints[x, y] = new Vector3(
                    transform.position.x - raycastDistance + x * cellSize,
                    transform.position.y - raycastDistance + y * cellSize,
                    transform.position.z
                );
            }
        }
        
        // Start the boundary detection coroutine
        StartCoroutine(UpdateBoundaries());
    }
    
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying || !visualizeInGame)
            return;
            
        // Draw boundary points
        Gizmos.color = boundaryColor;
        foreach (Vector3 point in boundaryPoints)
        {
            Gizmos.DrawSphere(point, boundarySphereSize);
        }
    }
    
    private IEnumerator UpdateBoundaries()
    {
        while (true)
        {
            // Check each grid point for light/shadow
            for (int x = 0; x < gridPoints.GetLength(0); x++)
            {
                for (int y = 0; y < gridPoints.GetLength(1); y++)
                {
                    Vector3 point = gridPoints[x, y];
                    inShadow[x, y] = IsInShadow(point);
                }
                
                // Yield after each row to spread processing across frames
                yield return null;
            }
            
            // Find boundary points (adjacent light/shadow cells)
            boundaryPoints.Clear();
            
            for (int x = 0; x < gridPoints.GetLength(0) - 1; x++)
            {
                for (int y = 0; y < gridPoints.GetLength(1) - 1; y++)
                {
                    bool thisShadow = inShadow[x, y];
                    
                    // Check right neighbor
                    if (x < gridPoints.GetLength(0) - 1 && thisShadow != inShadow[x + 1, y])
                    {
                        Vector3 midPoint = (gridPoints[x, y] + gridPoints[x + 1, y]) * 0.5f;
                        boundaryPoints.Add(midPoint);
                    }
                    
                    // Check top neighbor
                    if (y < gridPoints.GetLength(1) - 1 && thisShadow != inShadow[x, y + 1])
                    {
                        Vector3 midPoint = (gridPoints[x, y] + gridPoints[x, y + 1]) * 0.5f;
                        boundaryPoints.Add(midPoint);
                    }
                    
                    // Check diagonal neighbor
                    if (x < gridPoints.GetLength(0) - 1 && y < gridPoints.GetLength(1) - 1 && 
                        thisShadow != inShadow[x + 1, y + 1])
                    {
                        Vector3 midPoint = (gridPoints[x, y] + gridPoints[x + 1, y + 1]) * 0.5f;
                        boundaryPoints.Add(midPoint);
                    }
                }
            }
            
            // Wait a bit before the next update
            yield return new WaitForSeconds(0.5f);
        }
    }
    
    private bool IsInShadow(Vector3 position)
    {
        // Find all 2D lights in the scene
        UnityEngine.Rendering.Universal.Light2D[] lights = FindObjectsByType<UnityEngine.Rendering.Universal.Light2D>(FindObjectsSortMode.None);
        
        foreach (UnityEngine.Rendering.Universal.Light2D light in lights)
        {
            if (!light.enabled)
                continue;
                
            // Calculate distance to light
            float distance = Vector2.Distance(position, light.transform.position);
            
            // Check if within light radius
            if (distance < light.pointLightOuterRadius)
            {
                // Check if there are obstacles between point and light
                Vector2 directionToLight = (light.transform.position - position).normalized;
                RaycastHit2D hit = Physics2D.Raycast(position, directionToLight, distance, shadowLayers);
                
                // If nothing is blocking the light, point is in light
                if (hit.collider == null)
                {
                    return false; // Not in shadow
                }
            }
        }
        
        // If no light reaches this point, it's in shadow
        return true;
    }
    
#if UNITY_EDITOR
    [UnityEditor.MenuItem("Shadow Heist/Create Shadow Visualizer")]
    private static void CreateVisualizer()
    {
        GameObject visualizer = new GameObject("ShadowBoundaryVisualizer");
        visualizer.AddComponent<ShadowBoundaryVisualizer>();
        
        UnityEditor.Selection.activeGameObject = visualizer;
        
        Debug.Log("Shadow Boundary Visualizer created. Adjust its transform position to visualize shadows in that area.");
    }
#endif
}