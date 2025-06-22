using UnityEngine;

/// <summary>
/// Simple trigger that switches the active playable character using the GameMaster.
/// </summary>
public class CharacterSwitchTrigger : MonoBehaviour
{
    public GameMaster gameMaster;  // Reference to the central GameMaster
    public int partyIndex;         // Index of the character to switch to

    /// <summary>
    /// Invoke this from story events or animations to change characters.
    /// </summary>
    public void Trigger()
    {
        if (gameMaster != null)
        {
            gameMaster.SwitchCharacter(partyIndex);
        }
    }
}
