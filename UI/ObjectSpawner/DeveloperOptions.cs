using UnityEngine;

public class DeveloperOptions : MonoBehaviour
{
    [SerializeField] private bool dropdownOpen = true;

    /// <summary>
    /// Returns whether the developer dropdown is currently open.
    /// </summary>
    public bool IsDropdownOpen()
    {
        return dropdownOpen;
    }

    /// <summary>
    /// Allows external callers to toggle the dropdown state.
    /// </summary>
    public void SetDropdownState(bool open)
    {
        dropdownOpen = open;
    }
}
