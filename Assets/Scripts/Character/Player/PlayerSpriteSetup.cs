using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(PlayerController))]
public class PlayerSpriteSetup : MonoBehaviour
{
    // This script is only used in editor to automatically find and assign sprites
    
#if UNITY_EDITOR
    [ContextMenu("Assign Player Sprites")]
    void AssignSprites()
    {
        PlayerController controller = GetComponent<PlayerController>();
        SerializedObject serializedController = new SerializedObject(controller);
        
        // Find all sprite properties in the controller
        SerializedProperty standingIdleProperty = serializedController.FindProperty("standingIdleSprite");
        SerializedProperty walkingSideProperty = serializedController.FindProperty("walkingSideSprite");
        SerializedProperty walkingAwayProperty = serializedController.FindProperty("walkingAwaySprite");
        SerializedProperty runningSideProperty = serializedController.FindProperty("runningSideSprite");
        SerializedProperty runningAwayProperty = serializedController.FindProperty("runningAwaySprite");
        SerializedProperty crouchingIdleProperty = serializedController.FindProperty("crouchingIdleSprite");
        SerializedProperty crouchWalkSideProperty = serializedController.FindProperty("crouchWalkSideSprite");
        SerializedProperty crouchWalkAwayProperty = serializedController.FindProperty("crouchWalkAwaySprite");
        
        // Find all sprites in the characters folder
        string[] guids = AssetDatabase.FindAssets("t:Sprite", new[] { "Assets/Art/Characters" });
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
            string spriteName = sprite.name.ToLower();
            
            if (spriteName.Contains("standing_idle"))
            {
                standingIdleProperty.objectReferenceValue = sprite;
            }
            else if (spriteName.Contains("walking_side"))
            {
                walkingSideProperty.objectReferenceValue = sprite;
            }
            else if (spriteName.Contains("walking_away"))
            {
                walkingAwayProperty.objectReferenceValue = sprite;
            }
            else if (spriteName.Contains("running_side"))
            {
                runningSideProperty.objectReferenceValue = sprite;
            }
            else if (spriteName.Contains("running_away"))
            {
                runningAwayProperty.objectReferenceValue = sprite;
            }
            else if (spriteName.Contains("crouching_idle"))
            {
                crouchingIdleProperty.objectReferenceValue = sprite;
            }
            else if (spriteName.Contains("crouch_walk_side"))
            {
                crouchWalkSideProperty.objectReferenceValue = sprite;
            }
            else if (spriteName.Contains("crouch_walk_away"))
            {
                crouchWalkAwayProperty.objectReferenceValue = sprite;
            }
        }
        
        serializedController.ApplyModifiedProperties();
        Debug.Log("Player sprites assigned successfully!");
    }
#endif
}
