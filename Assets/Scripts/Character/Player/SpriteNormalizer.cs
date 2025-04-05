using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteNormalizer : MonoBehaviour
{
    [Tooltip("The reference sprite that other sprites will be scaled to match")]
    [SerializeField] private Sprite referenceSprite;
    
    [Tooltip("Scale multiplier to apply to all sprites (after normalization)")]
    [SerializeField] private float globalScale = 1.0f;

    private SpriteRenderer spriteRenderer;
    private PlayerController playerController;
    
    // Store original sizes to avoid recalculating
    private Dictionary<Sprite, Vector2> normalizedSizes = new Dictionary<Sprite, Vector2>();

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerController = GetComponent<PlayerController>();
        
        if (referenceSprite == null && spriteRenderer.sprite != null)
        {
            // Use current sprite as reference if none is specified
            referenceSprite = spriteRenderer.sprite;
            Debug.Log("SpriteNormalizer: Using current sprite as reference.");
        }
    }

    private void LateUpdate()
    {
        if (spriteRenderer.sprite != null && referenceSprite != null)
        {
            NormalizeCurrentSprite();
        }
    }

    private void NormalizeCurrentSprite()
    {
        Sprite currentSprite = spriteRenderer.sprite;
        
        // Skip if it's the reference sprite
        if (currentSprite == referenceSprite)
        {
            transform.localScale = new Vector3(globalScale, globalScale, 1f);
            return;
        }
        
        // Calculate or retrieve normalized size
        Vector2 normalizedSize;
        if (!normalizedSizes.TryGetValue(currentSprite, out normalizedSize))
        {
            // Reference size in pixels
            Vector2 referenceSize = new Vector2(
                referenceSprite.rect.width / referenceSprite.pixelsPerUnit,
                referenceSprite.rect.height / referenceSprite.pixelsPerUnit
            );
            
            // Current sprite size in pixels
            Vector2 currentSize = new Vector2(
                currentSprite.rect.width / currentSprite.pixelsPerUnit,
                currentSprite.rect.height / currentSprite.pixelsPerUnit
            );
            
            // Calculate scale factors to match reference size
            normalizedSize = new Vector2(
                referenceSize.x / currentSize.x,
                referenceSize.y / currentSize.y
            );
            
            // Cache result
            normalizedSizes[currentSprite] = normalizedSize;
        }
        
        // Apply normalized scale
        transform.localScale = new Vector3(
            normalizedSize.x * globalScale * (spriteRenderer.flipX ? -1 : 1), 
            normalizedSize.y * globalScale, 
            1f
        );
    }

    // Call this when the reference sprite changes
    public void SetReferenceSprite(Sprite newReference)
    {
        referenceSprite = newReference;
        normalizedSizes.Clear(); // Clear cache when reference changes
    }
}
