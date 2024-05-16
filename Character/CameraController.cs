using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Player reference
    public Transform player;

    // Camera settings
    public float mouseSensitivity = 2f;
    public float maxLookUpAngle = 80f;
    public float maxLookDownAngle = -80f;
    public float defaultDistance = 5f;
    public float zoomSpeed = 2f;
    public LayerMask collisionMask;

    // Camera rotation variables
    private float rotationX = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Set initial camera position
        transform.localPosition = new Vector3(0f, 0f, -defaultDistance);
    }

    // Update is called once per frame
    void Update()
    {
        // Handle camera rotation
        RotateCamera();

        // Handle zooming
        ZoomCamera();
    }

    // LateUpdate is called after Update each frame
    void LateUpdate()
    {
        // Rotate the camera with the player's center axis
        transform.LookAt(player);
    }

    // Function to rotate the camera based on mouse input
    void RotateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, maxLookDownAngle, maxLookUpAngle);

        transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
        player.Rotate(Vector3.up * mouseX);
    }

    // Function to zoom the camera in and out
    void ZoomCamera()
    {
        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        transform.Translate(Vector3.forward * scrollWheel * zoomSpeed);
    }
}
