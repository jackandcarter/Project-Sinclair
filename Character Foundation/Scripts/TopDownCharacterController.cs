using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class TopDownCharacterController : MonoBehaviour
{
    public float moveSpeed = 5f; // Movement speed

    private CharacterController characterController;
    private Animator animator;

    private Vector2 moveInput; // Input direction from Input System
    private Vector3 moveDirection; // 3D movement direction

    private InputSystem_Actions inputActions; // Use the generated Input Actions class

    private void Awake()
    {
        // Initialize Input Actions
        inputActions = new InputSystem_Actions();

        // Subscribe to movement input
        inputActions.Player.Move.performed += OnMovePerformed;
        inputActions.Player.Move.canceled += OnMoveCanceled;

        // Subscribe to other player actions
        inputActions.Player.Attack.performed += OnAttackPerformed;
        inputActions.Player.Interact.performed += OnInteractPerformed;
        inputActions.Player.Jump.performed += OnJumpPerformed;
        inputActions.Player.Crouch.performed += OnCrouchPerformed;
        inputActions.Player.Crouch.canceled += OnCrouchCanceled;
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
    }

    private void Update()
    {
        // Convert 2D input into a 3D movement vector
        moveDirection = new Vector3(moveInput.x, 0, moveInput.y).normalized;

        // Move the character
        if (moveDirection.magnitude > 0.1f)
        {
            // Apply movement in world space
            characterController.Move(moveDirection * moveSpeed * Time.deltaTime);

            // Trigger walk animation
            animator.SetBool("Walk", true);

            // Make character face the movement direction
            transform.rotation = Quaternion.LookRotation(moveDirection, Vector3.up);
        }
        else
        {
            // Trigger idle animation
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

    private void OnAttackPerformed(InputAction.CallbackContext context)
    {
        animator.SetTrigger("Attack");
    }

    private void OnInteractPerformed(InputAction.CallbackContext context)
    {
        animator.SetTrigger("Interact");
    }

    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        animator.SetTrigger("Jump");
    }

    private void OnCrouchPerformed(InputAction.CallbackContext context)
    {
        animator.SetBool("Crouch", true);
    }

    private void OnCrouchCanceled(InputAction.CallbackContext context)
    {
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
