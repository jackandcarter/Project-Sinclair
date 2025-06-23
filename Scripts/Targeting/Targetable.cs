using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Component allowing an object to be targeted by <see cref="TargetManager"/>.
/// Tracks basic health and manages one or more highlight effects when selected.
/// </summary>
[RequireComponent(typeof(Collider))]
public class Targetable : MonoBehaviour
{
    [Tooltip("Name used when displaying this target.")]
    public string displayName;

    [Tooltip("Maximum hit points for this target.")]
    public int maxHP = 100;
    public int currentHP = 100;

    /// <summary>
    /// Fired whenever the current HP value changes.
    /// Provides the new HP value as a parameter.
    /// </summary>
    public event Action<int> HealthChanged;

    [Tooltip("Components toggled when this target is highlighted.")]
    public List<Behaviour> highlightEffects = new();

    [Tooltip("Optional prefab spawned while highlighted (e.g., crosshair).")]
    public GameObject crosshairPrefab;

    [Tooltip("Offset for the spawned crosshair.")]
    public Vector3 crosshairOffset = Vector3.up;

    [Tooltip("Color applied to highlight effects if supported.")]
    public Color highlightColor = Color.yellow;

    private GameObject crosshairInstance;

    private void Awake()
    {
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        foreach (Behaviour b in highlightEffects)
        {
            if (b != null)
            {
                b.enabled = false;
            }
        }
    }

    /// <summary>
    /// Enables or disables all highlight effects.
    /// </summary>
    public void Highlight(bool enable)
    {
        foreach (Behaviour b in highlightEffects)
        {
            if (b != null)
            {
                b.enabled = enable;
                if (enable)
                {
                    ApplyColor(b.gameObject);
                }
            }
        }

        HandleCrosshair(enable);
    }

    private void HandleCrosshair(bool enable)
    {
        if (crosshairPrefab == null)
        {
            return;
        }

        if (enable)
        {
            if (crosshairInstance == null)
            {
                crosshairInstance = Instantiate(crosshairPrefab, transform);
                crosshairInstance.transform.localPosition = crosshairOffset;
                ApplyColor(crosshairInstance);
            }
            else
            {
                crosshairInstance.SetActive(true);
            }
        }
        else if (crosshairInstance != null)
        {
            Destroy(crosshairInstance);
            crosshairInstance = null;
        }
    }

    private void ApplyColor(GameObject obj)
    {
        if (obj == null)
        {
            return;
        }

        foreach (var renderer in obj.GetComponentsInChildren<SpriteRenderer>())
        {
            renderer.color = highlightColor;
        }
        foreach (var r in obj.GetComponentsInChildren<Renderer>())
        {
            if (r is SpriteRenderer) continue;
            if (r.material != null && r.material.HasProperty("_Color"))
            {
                r.material.color = highlightColor;
            }
        }
    }

    /// <summary>
    /// Adjusts this target's HP and invokes <see cref="HealthChanged"/>.
    /// </summary>
    public void ChangeHP(int amount)
    {
        int old = currentHP;
        currentHP = Mathf.Clamp(currentHP + amount, 0, maxHP);
        if (currentHP != old)
        {
            HealthChanged?.Invoke(currentHP);
        }
    }
}
