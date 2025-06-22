using UnityEngine;

/// <summary>
/// Represents an executable action triggered from a dock icon.
/// Abilities, items and menu shortcuts can implement this interface.
/// </summary>
public interface IDockAction
{
    /// <summary>
    /// Executes the action using the provided battle manager.
    /// </summary>
    void Execute(BattleManager manager);
}
