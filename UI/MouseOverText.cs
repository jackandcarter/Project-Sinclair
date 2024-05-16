using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouseOverText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Text buttonText; // Reference to the button text

    void Start()
    {
        // Get the button text component
        buttonText = GetComponentInChildren<Text>();

        // Hide the button text initially
        if (buttonText != null)
        {
            buttonText.enabled = false;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Show the button text when the mouse enters the object
        if (buttonText != null)
        {
            buttonText.enabled = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Hide the button text when the mouse exits the object
        if (buttonText != null)
        {
            buttonText.enabled = false;
        }
    }
}
