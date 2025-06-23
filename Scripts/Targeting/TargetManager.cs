using System;
using UnityEngine;
using UnityEngine.InputSystem;

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
    private InputSystem_Actions input;

    /// <summary>
    /// Currently selected target, or null if none.
    /// </summary>
    public Targetable CurrentTarget => currentTarget;

    private void Awake()
    {
        input = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        input.Enable();
        input.UI.Click.performed += OnClick;
    }

    private void OnDisable()
    {
        input.UI.Click.performed -= OnClick;
        input.Disable();
    }

    private void OnClick(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        Vector2 pos = Pointer.current != null ? Pointer.current.position.ReadValue() : Vector2.zero;
        Ray ray = Camera.main.ScreenPointToRay(pos);
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
