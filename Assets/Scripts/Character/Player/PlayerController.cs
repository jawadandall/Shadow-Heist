using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 3.0f;
    [SerializeField] private float runSpeed = 5.0f;
    [SerializeField] private float crouchWalkSpeed = 1.5f;

    [Header("Sprite References")]
    [SerializeField] private Sprite standingIdleSprite;
    [SerializeField] private Sprite walkingSideSprite;
    [SerializeField] private Sprite walkingAwaySprite;
    [SerializeField] private Sprite runningSideSprite;
    [SerializeField] private Sprite runningAwaySprite;
    [SerializeField] private Sprite crouchingIdleSprite;
    [SerializeField] private Sprite crouchWalkSideSprite;
    [SerializeField] private Sprite crouchWalkAwaySprite;

    // Component references
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    // Player state
    private Vector2 movementInput;
    private bool isRunning = false;
    private bool isCrouching = false;
    private bool isFacingRight = true;
    private Vector2 lastNonZeroHorizontalInput = Vector2.right; // Track last non-zero horizontal input

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Configure Rigidbody2D for precise movement
        if (rb != null)
        {
            rb.gravityScale = 0f;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        // Set initial sprite
        spriteRenderer.sprite = standingIdleSprite;
    }

    private void Update()
    {
        // Get movement input
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        movementInput = new Vector2(horizontal, vertical).normalized;

        // Store last non-zero horizontal input for consistent facing direction
        if (horizontal != 0)
        {
            lastNonZeroHorizontalInput = new Vector2(horizontal, 0).normalized;
            isFacingRight = horizontal > 0;
        }

        // Check for run button (shift key)
        isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        // Check for crouch button (control key)
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
        {
            isCrouching = !isCrouching;
        }

        // Update sprite based on movement and state
        UpdateSprite();
    }

    private void FixedUpdate()
    {
        // Apply movement based on state
        float currentSpeed = DetermineSpeed();
        
        // Use MovePosition for more precise movement
        if (rb.bodyType == RigidbodyType2D.Kinematic)
        {
            // For Kinematic rigidbodies, use MovePosition
            Vector2 newPosition = rb.position + (movementInput * currentSpeed * Time.fixedDeltaTime);
            rb.MovePosition(newPosition);
        }
        else
        {
            // For Dynamic rigidbodies, set velocity directly
            rb.linearVelocity = movementInput * currentSpeed;
        }
    }

    private float DetermineSpeed()
    {
        if (isCrouching)
        {
            return crouchWalkSpeed;
        }
        else if (isRunning)
        {
            return runSpeed;
        }
        else
        {
            return walkSpeed;
        }
    }

    private void UpdateSprite()
    {
        // No movement - show idle sprite
        if (movementInput.magnitude < 0.1f)
        {
            spriteRenderer.sprite = isCrouching ? crouchingIdleSprite : standingIdleSprite;
            return;
        }

        // Determine which sprite to use based on direction and state
        // We want to use side sprites for ANY horizontal movement or diagonal movement or down
        // And only use away sprites for straight up movement
        bool useSideSprite = movementInput.x != 0 || movementInput.y < 0;
        bool useAwaySprite = movementInput.x == 0 && movementInput.y > 0;

        // Select the appropriate sprite based on movement type and player state
        if (useSideSprite)
        {
            if (isCrouching)
            {
                spriteRenderer.sprite = crouchWalkSideSprite;
            }
            else if (isRunning)
            {
                spriteRenderer.sprite = runningSideSprite;
            }
            else
            {
                spriteRenderer.sprite = walkingSideSprite;
            }
        }
        else if (useAwaySprite)
        {
            if (isCrouching)
            {
                spriteRenderer.sprite = crouchWalkAwaySprite;
            }
            else if (isRunning)
            {
                spriteRenderer.sprite = runningAwaySprite;
            }
            else
            {
                spriteRenderer.sprite = walkingAwaySprite;
            }
        }

        // Apply mirroring for left-facing sprites
        // Only flip X for side sprites and when facing left
        spriteRenderer.flipX = !isFacingRight;
    }
}
