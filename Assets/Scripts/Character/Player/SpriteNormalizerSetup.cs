using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(SpriteNormalizer))]
public class SpriteNormalizerSetup : MonoBehaviour
{
#if UNITY_EDITOR
    [ContextMenu("Set Standing Idle as Reference")]
    void SetIdleAsReference()
    {
        SpriteNormalizer normalizer = GetComponent<SpriteNormalizer>();
        PlayerController controller = GetComponent<PlayerController>();
        
        if (controller == null)
        {
            Debug.LogError("PlayerController component not found!");
            return;
        }
        
        // Use SerializedObject to access private fields
        SerializedObject serializedController = new SerializedObject(controller);
        SerializedProperty idleSpriteProperty = serializedController.FindProperty("standingIdleSprite");
        
        if (idleSpriteProperty != null && idleSpriteProperty.objectReferenceValue != null)
        {
            Sprite idleSprite = (Sprite)idleSpriteProperty.objectReferenceValue;
            
            SerializedObject serializedNormalizer = new SerializedObject(normalizer);
            SerializedProperty referenceSpriteProperty = serializedNormalizer.FindProperty("referenceSprite");
            
            referenceSpriteProperty.objectReferenceValue = idleSprite;
            serializedNormalizer.ApplyModifiedProperties();
            
            Debug.Log("Set standing idle sprite as the reference for normalization.");
        }
        else
        {
            Debug.LogError("Could not find standing idle sprite in PlayerController!");
        }
    }
    
    [ContextMenu("Analyze All Sprites")]
    void AnalyzeAllSprites()
    {
        PlayerController controller = GetComponent<PlayerController>();
        if (controller == null)
        {
            Debug.LogError("PlayerController component not found!");
            return;
        }
        
        SerializedObject serializedController = new SerializedObject(controller);
        
        // Find all sprite properties
        string[] spritePropertyNames = new string[] {
            "standingIdleSprite", "walkingSideSprite", "walkingAwaySprite",
            "runningSideSprite", "runningAwaySprite", "crouchingIdleSprite",
            "crouchWalkSideSprite", "crouchWalkAwaySprite"
        };
        
        Debug.Log("Sprite Dimensions Analysis:");
        foreach (string propName in spritePropertyNames)
        {
            SerializedProperty prop = serializedController.FindProperty(propName);
            if (prop != null && prop.objectReferenceValue != null)
            {
                Sprite sprite = (Sprite)prop.objectReferenceValue;
                Debug.Log($"{propName}: Size = {sprite.rect.width}x{sprite.rect.height} pixels, " +
                          $"PPU = {sprite.pixelsPerUnit}, " +
                          $"Unity Units = {sprite.rect.width/sprite.pixelsPerUnit}x{sprite.rect.height/sprite.pixelsPerUnit}");
            }
            else
            {
                Debug.Log($"{propName}: Not assigned");
            }
        }
    }
#endif
}
