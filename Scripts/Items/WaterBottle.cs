using UnityEngine;

/// <summary>
/// Simple consumable that restores a small amount of HP.
/// </summary>
[CreateAssetMenu(fileName = "WaterBottle", menuName = "Sinclair/Items/Water Bottle")]
public class WaterBottle : ConsumableItem
{
    private void Reset()
    {
        itemName = "Water Bottle";
        description = "Restores a small amount of HP.";
        healAmount = 10;
    }
}
