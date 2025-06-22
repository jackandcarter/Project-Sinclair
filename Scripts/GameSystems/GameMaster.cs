using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Central game manager that tracks the party and the currently controlled character.
/// </summary>
public class GameMaster : MonoBehaviour
{
    public TopDownCharacterController playerController;           // Player movement controller
    public List<CharacterData> partyMembers = new List<CharacterData>();
    public List<GameObject> partyModels = new List<GameObject>(); // Optional visuals for each character

    private int currentIndex;

    /// <summary>
    /// Currently selected character data or null if the index is invalid.
    /// </summary>
    public CharacterData CurrentCharacter =>
        currentIndex >= 0 && currentIndex < partyMembers.Count ? partyMembers[currentIndex] : null;

    /// <summary>
    /// Switches control to the character at the given party index.
    /// </summary>
    public void SwitchCharacter(int index)
    {
        if (playerController == null || index < 0 || index >= partyMembers.Count)
        {
            return;
        }

        currentIndex = index;
        CharacterData data = partyMembers[index];

        // Enable the visual model that corresponds to this character
        for (int i = 0; i < partyModels.Count; i++)
        {
            if (partyModels[i] != null)
            {
                partyModels[i].SetActive(i == index);
            }
        }

        // Rename the player object for clarity
        playerController.gameObject.name = data.characterName;

        Debug.Log($"Switched to {data.characterName}");
    }
}
