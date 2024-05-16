using UnityEngine;
using UnityEngine.UI;

public class UIPanelToggle : MonoBehaviour
{
    public GameObject panelToShowHide; // Reference to the UI panel to show/hide

    private bool isPanelVisible = false;

    void Start()
    {
        // Ensure the panel starts off hidden
        if (panelToShowHide != null)
        {
            panelToShowHide.SetActive(false);
        }
    }

    public void TogglePanelVisibility()
    {
        if (panelToShowHide != null)
        {
            isPanelVisible = !isPanelVisible;
            panelToShowHide.SetActive(isPanelVisible);
        }
    }
}
