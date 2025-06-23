using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Basic dialogue manager for displaying lines of text on a UI panel.
/// </summary>
public class DialogueManager : MonoBehaviour
{
    public GameObject dialoguePanel; // Panel containing dialogue UI
    public TMP_Text dialogueText;        // UI Text element for dialogue

    private readonly Queue<string> sentences = new Queue<string>();

    /// <summary>
    /// Begins a new dialogue sequence using the provided lines.
    /// </summary>
    public void StartDialogue(IEnumerable<string> lines)
    {
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(true);
        }

        sentences.Clear();
        foreach (string line in lines)
        {
            sentences.Enqueue(line);
        }

        DisplayNextSentence();
    }

    /// <summary>
    /// Displays the next sentence in the queue or ends the dialogue.
    /// </summary>
    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        if (dialogueText != null)
        {
            dialogueText.text = sentence;
        }
    }

    /// <summary>
    /// Hides the dialogue panel once the conversation finishes.
    /// </summary>
    public void EndDialogue()
    {
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }
    }
}
