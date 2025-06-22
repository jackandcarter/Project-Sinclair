using UnityEngine;

/// <summary>
/// Environment-triggered event that performs an item action on a target.
/// </summary>
public class BattleActionEvent : MonoBehaviour
{
    public ConsumableItem item;
    public CharacterData target;

    /// <summary>
    /// Executes the event using the provided battle manager.
    /// </summary>
    public void Trigger(BattleManager manager)
    {
        if (manager == null || item == null || target == null)
        {
            return;
        }

        BattleCharacter character = manager.FindCharacter(target);
        if (character != null)
        {
            manager.UseItem(character, item, character);
        }
    }
}
