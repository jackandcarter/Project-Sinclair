using UnityEngine;

/// <summary>
/// Base class for consumable items that can affect a battle character.
/// </summary>
public class ConsumableItem : ScriptableObject
{
    public string itemName;
    public string description;
    public int healAmount;

    /// <summary>
    /// Apply this item's effect to the target character.
    /// </summary>
    public virtual void Apply(BattleCharacter target)
    {
        if (target != null)
        {
            target.ChangeHP(healAmount);
            Debug.Log($"{itemName} heals {target.data.characterName} for {healAmount} HP.");
        }
    }
}
