using UnityEngine;

public class PlayerControlScript : MonoBehaviour
{
    public float speed = 13f;
    public float turnSpeed = 74f;
    private Rigidbody rb;

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
        float movementX = Input.GetAxis("Horizontal"); //Turns left and right
        float movementZ = Input.GetAxis("Vertical"); //Forwaed and back

        //Handles forward movement
        Vector3 movement = transform.forward * movementZ;
        rb.AddForce(movement * speed);

        //Handles turning
        Quaternion tRotation = Quaternion.Euler(0f, movementX * turnSpeed * Time.fixedDeltaTime, 0f);
        rb.MoveRotation(rb.rotation * tRotation);
    }
}
