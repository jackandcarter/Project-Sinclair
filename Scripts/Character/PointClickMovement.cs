using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

/// <summary>
/// Moves the attached NavMeshAgent to a clicked point on the terrain.
/// Can be used with TopDownCharacterController so the player moves by
/// either keyboard or mouse.
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class PointClickMovement : MonoBehaviour
{
    [Tooltip("Optional effect prefab to spawn at the clicked destination.")]
    public GameObject destinationEffect;
    public float effectDuration = 1f; // Lifetime of the spawned effect
    [Tooltip("How close the agent should get to Targetable objects.")]
    public float targetRange = 1f;
    [Tooltip("If true, holding the input will continuously update the destination.")]
    public bool continuousMovement;

    private NavMeshAgent agent;
    private InputSystem_Actions input;
    private InputAction moveClick;
    private InputAction holdMove;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        input = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        input.Enable();
        moveClick = input.asset.FindAction("MoveClick", false);
        if (moveClick == null)
        {
            moveClick = input.UI.RightClick;
        }
        moveClick.performed += OnMoveClick;
        holdMove = input.asset.FindAction("HoldMove", false);
    }

    private void OnDisable()
    {
        if (moveClick != null)
        {
            moveClick.performed -= OnMoveClick;
        }
        input.Disable();
    }

    private void Update()
    {
        if (continuousMovement && holdMove != null && holdMove.IsPressed())
        {
            MoveAgentToPointer();
        }
    }

    private void OnMoveClick(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        MoveAgentToPointer();
    }

    private void MoveAgentToPointer()
    {
        Vector2 pos = Pointer.current != null ? Pointer.current.position.ReadValue() : Vector2.zero;
        Ray ray = Camera.main.ScreenPointToRay(pos);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 destination = hit.point;
            Targetable t = hit.collider.GetComponentInParent<Targetable>();
            if (t != null)
            {
                agent.stoppingDistance = targetRange;
                destination = t.transform.position;
            }
            else
            {
                agent.stoppingDistance = 0f;
            }

            agent.SetDestination(destination);

            if (destinationEffect != null)
            {
                GameObject fx = Instantiate(destinationEffect, destination, Quaternion.identity);
                Destroy(fx, effectDuration);
            }
        }
    }
}
