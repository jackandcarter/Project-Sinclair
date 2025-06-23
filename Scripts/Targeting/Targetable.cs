using System;
using UnityEngine;

/// <summary>
/// Component allowing an object to be targeted by <see cref="TargetManager"/>.
/// Tracks basic health and manages a highlight effect when selected.
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

    [Tooltip("Optional component toggled when the target is highlighted.")]
    public Behaviour highlight;

    private void Awake()
    {
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        if (highlight != null)
        {
            highlight.enabled = false;
        }
    }

    /// <summary>
    /// Enables or disables the highlight effect.
    /// </summary>
    public void Highlight(bool enable)
    {
        if (highlight != null)
        {
            highlight.enabled = enable;
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
