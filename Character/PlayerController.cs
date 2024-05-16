using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Movement speed variables
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float jumpForce = 10f;

    // Time in seconds before running speed kicks in
    public float runDelay = 1f;

    // Mouse sensitivity for horizontal rotation
    public float mouseSensitivity = 2f;

    // Camera reference
    public Transform cameraTransform;

    // Rigidbody component for physics interactions
    private Rigidbody rb;

    // Boolean to check if the player is grounded
    private bool isGrounded;

    // Time when player started moving
    private float moveStartTime;

    // Flag to track if right mouse button is held down
    private bool isRightMouseButtonDown = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        moveStartTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        // Handle movement input
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate movement direction based on input
        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        // Check if the player is grounded
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.1f);

        // Handle jumping
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        // Apply movement to the player
        MovePlayer(movement);

        // Handle mouse rotation
        if (isRightMouseButtonDown)
        {
            RotateCamera();
        }
    }

    // Function to move the player
    void MovePlayer(Vector3 movement)
    {
        // Start the timer if the player starts moving
        if (movement.magnitude > 0 && moveStartTime == 0)
        {
            moveStartTime = Time.time;
        }

        // Calculate time elapsed since the player started moving
        float elapsedTime = Time.time - moveStartTime;

        // Determine speed based on time elapsed
        float speed = elapsedTime >= runDelay ? runSpeed : walkSpeed;

        // Move the player
        rb.MovePosition(transform.position + transform.TransformDirection(movement) * speed * Time.deltaTime);
        
        // Reset timer if the player stops moving
        if (movement.magnitude == 0)
        {
            moveStartTime = 0f;
        }
    }

    // Function to rotate the camera based on mouse input
    void RotateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;

        // Rotate the player horizontally based on camera rotation
        transform.Rotate(Vector3.up * mouseX);

        // Rotate the player to face the same direction as the camera
        transform.rotation = Quaternion.Euler(0f, cameraTransform.eulerAngles.y, 0f);
    }

    // Called when right mouse button is pressed down
    public void OnRightMouseButtonDown()
    {
        isRightMouseButtonDown = true;
    }

    // Called when right mouse button is released
    public void OnRightMouseButtonUp()
    {
        isRightMouseButtonDown = false;
    }
}
