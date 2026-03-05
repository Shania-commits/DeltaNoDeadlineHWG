using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerControlScript : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 15f;
    public float turnSpeed = 220f;

    [Header("Crouch Settings")]
    public float crouchSpeed = 8f;
    public float crouchHeight = 0.5f;
    public float normalHeight = 2f;
    public float crouchTransitionSpeed = 5f;

    private Rigidbody rb;
    private CapsuleCollider capsuleCollider;
    private PlayerInput playerInput;
    private InputActionAsset inputActions;
    private float currentHeight;
    private float normalColliderHeight;
    private bool isCrouching = false;

    public bool actionAvailable = false;

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
        
        InputAction crouchAction = playerActionMap.FindAction("Crouch");
        if (crouchAction != null)
        {
            crouchAction.performed += OnCrouch;
            crouchAction.canceled += OnCrouch;
        }

        InputAction interactAction = playerActionMap.FindAction("Interact");
        if (interactAction != null)
        {
            interactAction.performed += OnInteract;
            interactAction.canceled += OnInteract;
        }
    }

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
                crouchAction.canceled -= OnCrouch;
            }

            InputAction interactAction = playerActionMap.FindAction("Interact");
            if (interactAction != null)
            {
                interactAction.performed -= OnInteract;
                interactAction.canceled -= OnInteract;
            }
        }
    }

    void FixedUpdate()
    {
        // Get input
        float horizontal = Input.GetAxis("Horizontal"); // A / D
        float vertical = Input.GetAxis("Vertical");     // W / S

        // Determine current speed based on crouch state
        float currentSpeed = isCrouching ? crouchSpeed : speed;

        //Movement
        Vector3 moveDirection = transform.forward * vertical;
        Vector3 targetPosition = rb.position + moveDirection * currentSpeed * Time.fixedDeltaTime;

        // Move the Rigidbody while respecting collisions
        rb.MovePosition(targetPosition);

        //Turning
        float turnAmount = horizontal * turnSpeed * Time.fixedDeltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turnAmount, 0f);
        rb.MoveRotation(rb.rotation * turnRotation);

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

    //Tracks if crouch button is pressed
    public void OnCrouch(InputAction.CallbackContext context)
    {
        //Button pressed
        if (context.performed)
        {
            Debug.Log("Crouch started");
            isCrouching = true;
        }

        //Button released
        if (context.canceled)
        {
            Debug.Log("Crouch ended");
            isCrouching = false;
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