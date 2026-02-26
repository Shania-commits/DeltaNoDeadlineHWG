using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerControlScript : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 15f;
    public float turnSpeed = 220f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Freeze unwanted rotations (keep only Y rotation for turning)
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        // Smooth interpolation for physics-based movement
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    void FixedUpdate()
    {
        // Get input
        float horizontal = Input.GetAxis("Horizontal"); // A / D
        float vertical = Input.GetAxis("Vertical");     // W / S

        // ---- MOVEMENT ----
        Vector3 moveDirection = transform.forward * vertical;
        Vector3 targetPosition = rb.position + moveDirection * speed * Time.fixedDeltaTime;

        // Move the Rigidbody while respecting collisions
        rb.MovePosition(targetPosition);

        // ---- TURNING ----
        float turnAmount = horizontal * turnSpeed * Time.fixedDeltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turnAmount, 0f);
        rb.MoveRotation(rb.rotation * turnRotation);
    }
}