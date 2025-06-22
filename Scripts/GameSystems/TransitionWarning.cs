using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Displays a warning before transitioning from Olympus to D\u00farnir's Pass.
/// Useful for story segments that lock the player out of returning until
/// the mission arc concludes.
/// </summary>
public class TransitionWarning : MonoBehaviour
{
    public DialogueManager dialogueManager;   // Dialogue UI for the warning
    [TextArea]
    public string warningText = "You cannot return to Olympus until this arc is complete.";
    public string destinationScene;           // Scene to load after the warning

    /// <summary>
    /// Triggers the warning and loads the destination scene.
    /// </summary>
    public void ExecuteTransition()
    {
        if (dialogueManager != null)
        {
            dialogueManager.StartDialogue(new List<string> { warningText });
        }

        if (!string.IsNullOrEmpty(destinationScene))
        {
            SceneManager.LoadScene(destinationScene);
        }
    }
}
