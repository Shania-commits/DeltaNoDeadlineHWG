using UnityEngine;

public class PlayerControlScript : MonoBehaviour
{
    public float speed = 13f;
    public float turnSpeed = 50f;
    private Rigidbody rb;

    [SerializeField] Transform cam;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        //Moves player
        float movementX = Input.GetAxis("Horizontal");
        float movementZ = Input.GetAxis("Vertical");

        /*Turn with camera
        Vector3 cameraForward = cam.forward;
        Vector3 cameraRight = cam.right;

        cameraForward.y = 0;
        cameraRight.y = 0;

        Vector3 forwardRelative = movementZ * cameraForward;
        Vector3 rightRelative = movementX * cameraRight;

        Vector3 moveDirection = forwardRelative + rightRelative; */

        Vector3 movement = new Vector3(movementX, 0f, movementZ);

        //rb.linearVelocity = new Vector3(moveDirection.x, rb.linearVelocity.y, moveDirection.z);
        rb.AddForce(movement * speed);

        //Turns during movement
        float turn = movementX * turnSpeed * Time.fixedDeltaTime;
        Quaternion tRotation = Quaternion.Euler(0f, turn, 0f);
        rb.MoveRotation(rb.rotation * tRotation);
    }
}
