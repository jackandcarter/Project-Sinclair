using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tracks consumable items for the player party.
/// </summary>
public class InventoryManager : MonoBehaviour
{
    private readonly Dictionary<ConsumableItem, int> items = new Dictionary<ConsumableItem, int>();

    /// <summary>
    /// Adds a quantity of an item to the inventory.
    /// </summary>
    public void AddItem(ConsumableItem item, int amount = 1)
    {
        if (item == null)
        {
            return;
        }

        if (items.ContainsKey(item))
        {
            items[item] += amount;
        }
        else
        {
            items[item] = amount;
        }
    }

    /// <summary>
    /// Uses an item on a target character, applying its effect.
    /// </summary>
    public bool UseItem(ConsumableItem item, BattleCharacter target)
    {
        if (item == null || !items.ContainsKey(item) || items[item] <= 0)
        {
            return false;
        }

        items[item]--;
        item.Apply(target);
        return true;
    }

    /// <summary>
    /// Returns how many of the given item the player has.
    /// </summary>
    public int GetQuantity(ConsumableItem item)
    {
        return items.TryGetValue(item, out int count) ? count : 0;
    }
}
