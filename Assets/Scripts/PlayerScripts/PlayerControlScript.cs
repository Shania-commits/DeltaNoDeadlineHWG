using UnityEngine;
using UnityEngine.InputSystem;

// This script handles player movement, crouching, and camera control using the new Input System.
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerControlScript : MonoBehaviour
{
    // Movement settings
    [Header("Movement Settings")]
    public float speed = 15f;
    public float turnSpeed = 220f;

    // Crouch settings
    [Header("Crouch Settings")]
    public float crouchSpeed = 8f;
    public float crouchHeight = 0.5f;
    public float normalHeight = 2f;
    public float crouchTransitionSpeed = 5f;

    // Camera settings
    [Header("Camera Settings")]
    public Transform cameraTransform;
    public float mouseSensitivity = 1f;
    public float maxPitchAngle = 90f;
    public float minPitchAngle = -90f;
    public Vector3 cameraOffset = new Vector3(0f, 0.3f, 0f);

    // Private variables
    private Rigidbody rb;
    private CapsuleCollider capsuleCollider;
    private PlayerInput playerInput;
    private InputActionAsset inputActions;
    private float currentHeight;
    private float normalColliderHeight;
    private bool isCrouching = false;

    private InputAction moveAction;
    private InputAction lookAction;
    private float yaw = 0f;
    private float pitch = 0f;

    public bool actionAvailable = false;

    //Tracks if the player is currently performing an interaction
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        playerInput = GetComponent<PlayerInput>();

        // Freeze unwanted rotations
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;

        // Smooth interpolation for physics-based movement
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        currentHeight = normalHeight;
        normalColliderHeight = capsuleCollider.height;

        // Get the input actions and subscribe to callbacks
        inputActions = playerInput.actions;
        InputActionMap playerActionMap = inputActions.FindActionMap("Player");
        
        moveAction = playerActionMap.FindAction("Move");
        lookAction = playerActionMap.FindAction("Look");
        
        InputAction crouchAction = playerActionMap.FindAction("Crouch");
        if (crouchAction != null)
        {
            // Toggle crouch on press
            crouchAction.performed += OnCrouch;
        }

        InputAction interactAction = playerActionMap.FindAction("Interact");
        if (interactAction != null)
        {
            interactAction.performed += OnInteract;
            interactAction.canceled += OnInteract;
        }

        // If camera not assigned, try to find it
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }

        // Force camera offset to desired value (override Inspector if needed)
        cameraOffset = new Vector3(0f, 0.6f, 0.3f);

        // Initialize yaw and pitch to 0 for forward-facing start
        yaw = 0f;
        pitch = 0f;

        // Set initial camera rotation to forward
        cameraTransform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }

    //Makes sure that the input action callbacks prevent unintended behavior.
    void OnDestroy()
    {
        // Unsubscribe from input actions
        if (inputActions != null)
        {
            InputActionMap playerActionMap = inputActions.FindActionMap("Player");
            
            InputAction crouchAction = playerActionMap.FindAction("Crouch");
            if (crouchAction != null)
            {
                crouchAction.performed -= OnCrouch;
            }

            InputAction interactAction = playerActionMap.FindAction("Interact");
            if (interactAction != null)
            {
                interactAction.performed -= OnInteract;
                interactAction.canceled -= OnInteract;
            }
        }
    }

    void Update()
    {
        // Handle mouse look
        Vector2 lookInput = lookAction.ReadValue<Vector2>();
        float mouseX = (lookInput.x / Screen.width) * mouseSensitivity * 360f;
        float mouseY = (lookInput.y / Screen.height) * mouseSensitivity * 360f;

        // Accumulate yaw and pitch
        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, minPitchAngle, maxPitchAngle);

        // Set camera rotation
        cameraTransform.rotation = Quaternion.Euler(pitch, yaw, 0f);

        // Rotate player body to match yaw for immersion
        transform.rotation = Quaternion.Euler(0f, yaw, 0f);
    }

    void LateUpdate()
    {
        // Position camera at player position + offset, adjusted for crouching and player rotation
        Vector3 adjustedOffset = cameraOffset;
        adjustedOffset.y *= currentHeight / normalHeight;  // Scale offset based on player height
        Vector3 worldOffset = transform.rotation * adjustedOffset;  // Rotate offset by player rotation
        cameraTransform.position = transform.position + worldOffset;

        // Debug: Uncomment to check (Camera height issues)
        // Debug.Log("Camera position: " + cameraTransform.position + ", Player position: " + transform.position + ", currentHeight: " + currentHeight + ", adjusted y: " + adjustedOffset.y);
    }

    void FixedUpdate()
    {
        // Get input
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        float horizontal = moveInput.x;
        float vertical = moveInput.y;

        // Determine current speed based on crouch state
        float currentSpeed = isCrouching ? crouchSpeed : speed;

        //Movement relative to camera
        Vector3 moveDirection = cameraTransform.forward * vertical + cameraTransform.right * horizontal;
        moveDirection.y = 0f; // Keep movement horizontal
        moveDirection.Normalize();

        Vector3 targetPosition = rb.position + moveDirection * currentSpeed * Time.fixedDeltaTime;

        // Move the Rigidbody while respecting collisions
        rb.MovePosition(targetPosition);

        // Update player height and collider
        UpdatePlayerHeight();
    }

    private void UpdatePlayerHeight()
    {
        float targetHeight = isCrouching ? crouchHeight : normalHeight;
        currentHeight = Mathf.Lerp(currentHeight, targetHeight, crouchTransitionSpeed * Time.fixedDeltaTime);

        // Apply height change to the player's transform
        Vector3 scale = transform.localScale;
        scale.y = currentHeight / normalHeight;
        transform.localScale = scale;

        // Apply proportional collider height change
        float colliderHeightRatio = currentHeight / normalHeight;
        capsuleCollider.height = normalColliderHeight * colliderHeightRatio;
    }

    //Tracks if an interaction button is pressed
    public void OnInteract(InputAction.CallbackContext context)
    {
        //Button pressed
        if (context.performed)
        {
            Debug.Log("Interaction triggered");
            PerformInteraction();
        }

        //Button released
        if (context.canceled)
        {
            Debug.Log("No more interaction");
            EndInteraction();
        }
    }

    // Toggle crouch when the crouch button is pressed
    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isCrouching = !isCrouching;
            Debug.Log(isCrouching ? "Crouch started" : "Crouch ended");
        }
    }

    //Interaction actions
    private void PerformInteraction()
    {
        Debug.Log("Key press");
        actionAvailable = true;
    }

    //Interaction actions
    private void EndInteraction()
    {
        Debug.Log("Key release");
        actionAvailable = false;
    }

}