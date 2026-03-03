using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerControlScript : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 15f;
    public float turnSpeed = 220f;

    private Rigidbody rb;

    public bool actionAvailable = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Freeze unwanted rotations
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;

        // Smooth interpolation for physics-based movement
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    void FixedUpdate()
    {
        // Get input
        float horizontal = Input.GetAxis("Horizontal"); // A / D
        float vertical = Input.GetAxis("Vertical");     // W / S

        //Movement
        Vector3 moveDirection = transform.forward * vertical;
        Vector3 targetPosition = rb.position + moveDirection * speed * Time.fixedDeltaTime;

        // Move the Rigidbody while respecting collisions
        rb.MovePosition(targetPosition);

        //Turning
        float turnAmount = horizontal * turnSpeed * Time.fixedDeltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turnAmount, 0f);
        rb.MoveRotation(rb.rotation * turnRotation);

        
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