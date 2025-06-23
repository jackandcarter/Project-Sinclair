using System;
using UnityEngine;

/// <summary>
/// Handles selecting <see cref="Targetable"/> objects via mouse clicks.
/// Raycasts from the main camera and highlights the currently selected target.
/// </summary>
public class TargetManager : MonoBehaviour
{
    public event Action<Targetable> TargetSelected;
    public event Action<Targetable> TargetDeselected;

    public float rayDistance = 100f;
    public LayerMask rayLayers = Physics.DefaultRaycastLayers;

    private Targetable currentTarget;

    /// <summary>
    /// Currently selected target, or null if none.
    /// </summary>
    public Targetable CurrentTarget => currentTarget;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, rayLayers))
            {
                Targetable t = hit.collider.GetComponentInParent<Targetable>();
                if (t != null && (t.CompareTag("Enemy") || t.CompareTag("NPC")))
                {
                    Select(t);
                }
                else
                {
                    Select(null);
                }
            }
        }
    }

    /// <summary>
    /// Sets the active target and handles highlight logic.
    /// </summary>
    public void Select(Targetable target)
    {
        if (currentTarget == target)
        {
            return;
        }

        if (currentTarget != null)
        {
            currentTarget.Highlight(false);
            TargetDeselected?.Invoke(currentTarget);
        }

        currentTarget = target;

        if (currentTarget != null)
        {
            currentTarget.Highlight(true);
            TargetSelected?.Invoke(currentTarget);
        }
    }
}
