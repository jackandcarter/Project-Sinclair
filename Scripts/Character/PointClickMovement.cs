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

    private NavMeshAgent agent;
    private InputSystem_Actions input;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        input = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        input.Enable();
        input.UI.RightClick.performed += OnMoveClick;
    }

    private void OnDisable()
    {
        input.UI.RightClick.performed -= OnMoveClick;
        input.Disable();
    }

    private void OnMoveClick(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        Vector2 pos = Pointer.current != null ? Pointer.current.position.ReadValue() : Vector2.zero;
        Ray ray = Camera.main.ScreenPointToRay(pos);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            agent.SetDestination(hit.point);

            if (destinationEffect != null)
            {
                GameObject fx = Instantiate(destinationEffect, hit.point, Quaternion.identity);
                Destroy(fx, effectDuration);
            }
        }
    }
}
