using UnityEngine;

public class MouseInputHandler : MonoBehaviour
{
    // Reference to the player controller
    public PlayerController playerController;

    // Update is called once per frame
    void Update()
    {
        // Check if right mouse button is pressed down
        if (Input.GetMouseButtonDown(1))
        {
            playerController.OnRightMouseButtonDown();
        }

        // Check if right mouse button is released
        if (Input.GetMouseButtonUp(1))
        {
            playerController.OnRightMouseButtonUp();
        }
    }
}
