using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class TopDownCharacterController : MonoBehaviour
{
    public float moveSpeed = 5f; // Movement speed

    /// <summary>Speed at which the character rotates toward movement direction.</summary>
    public float rotationSpeed = 10f;

    /// <summary>Time to reach the desired move velocity.</summary>
    public float smoothTime = 0.1f;

    /// <summary>Multiplier applied when sprinting.</summary>
    public float sprintMultiplier = 2f;

    private CharacterController characterController;
    private Animator animator;

    private Vector2 moveInput; // Input direction from Input System
    private Vector3 moveDirection; // 3D movement direction

    private InputSystem_Actions inputActions; // Use the generated Input Actions class

    private Vector3 currentVelocity; // Smoothed velocity
    private Vector3 velocityRef; // Velocity reference for SmoothDamp
    private bool isSprinting;
    private float baseMoveSpeed;

    private void Awake()
    {
        // Initialize Input Actions
        inputActions = new InputSystem_Actions();

        // Subscribe to movement input
        inputActions.Player.Move.performed += OnMovePerformed;
        inputActions.Player.Move.canceled += OnMoveCanceled;
        inputActions.Player.Sprint.performed += OnSprintPerformed;
        inputActions.Player.Sprint.canceled += OnSprintCanceled;

        // Subscribe to additional player actions
        inputActions.Player.Attack.performed += OnAttackPerformed;
        inputActions.Player.Interact.performed += OnInteractPerformed;
        inputActions.Player.Jump.performed += OnJumpPerformed;
        inputActions.Player.Crouch.performed += OnCrouchPerformed;
        inputActions.Player.Crouch.canceled += OnCrouchCanceled;
    }

    private void OnDestroy()
    {
        // Unsubscribe from movement input
        inputActions.Player.Move.performed -= OnMovePerformed;
        inputActions.Player.Move.canceled -= OnMoveCanceled;
        inputActions.Player.Sprint.performed -= OnSprintPerformed;
        inputActions.Player.Sprint.canceled -= OnSprintCanceled;

        // Unsubscribe from additional player actions
        inputActions.Player.Attack.performed -= OnAttackPerformed;
        inputActions.Player.Interact.performed -= OnInteractPerformed;
        inputActions.Player.Jump.performed -= OnJumpPerformed;
        inputActions.Player.Crouch.performed -= OnCrouchPerformed;
        inputActions.Player.Crouch.canceled -= OnCrouchCanceled;
    }

    private void OnEnable()
    {
        inputActions.Player.Enable(); // Enable the Player Input Map
    }

    private void OnDisable()
    {
        inputActions.Player.Disable(); // Disable the Player Input Map
    }

    private void Start()
    {
        // Get references
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        baseMoveSpeed = moveSpeed;
    }

    private void Update()
    {
        // Convert 2D input into a 3D movement vector
        Vector3 desiredDirection = new Vector3(moveInput.x, 0, moveInput.y).normalized;

        // Update movement speed based on sprint state
        moveSpeed = isSprinting ? baseMoveSpeed * sprintMultiplier : baseMoveSpeed;

        // Desired velocity taking sprint into account
        Vector3 targetVelocity = desiredDirection * moveSpeed;

        // Smooth towards the desired velocity
        currentVelocity = Vector3.SmoothDamp(currentVelocity, targetVelocity, ref velocityRef, smoothTime);
        characterController.Move(currentVelocity * Time.deltaTime);

        // Track the actual movement direction for animation and external queries
        moveDirection = new Vector3(currentVelocity.x, 0f, currentVelocity.z);

        if (moveDirection.magnitude > 0.1f)
        {
            animator.SetBool("Walk", true);

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime);
        }
        else
        {
            animator.SetBool("Walk", false);
        }
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        // Read movement input from the Input System
        moveInput = context.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        // Reset input when movement stops
        moveInput = Vector2.zero;
    }

    private void OnSprintPerformed(InputAction.CallbackContext context)
    {
        isSprinting = true;
    }

    private void OnSprintCanceled(InputAction.CallbackContext context)
    {
        isSprinting = false;
    }

    private void OnAttackPerformed(InputAction.CallbackContext context)
    {
        // Trigger attack animation
        animator.SetTrigger("Attack");
    }

    private void OnInteractPerformed(InputAction.CallbackContext context)
    {
        // Trigger interact animation
        animator.SetTrigger("Interact");
    }

    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        // Trigger jump animation
        animator.SetTrigger("Jump");
    }

    private void OnCrouchPerformed(InputAction.CallbackContext context)
    {
        // Enter crouch state
        animator.SetBool("Crouch", true);
    }

    private void OnCrouchCanceled(InputAction.CallbackContext context)
    {
        // Exit crouch state
        animator.SetBool("Crouch", false);
    }

    /// <summary>
    /// Returns whether the player is currently moving.
    /// </summary>
    public bool IsMoving()
    {
        return moveDirection.magnitude > 0.1f;
    }
}
