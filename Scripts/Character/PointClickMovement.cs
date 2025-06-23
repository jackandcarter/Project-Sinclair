using UnityEngine;
using UnityEngine.AI;

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

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
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
}
