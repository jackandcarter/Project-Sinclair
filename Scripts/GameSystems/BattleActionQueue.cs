using System;
using System.Collections.Generic;

/// <summary>
/// Maintains queues of battle actions per character.
/// </summary>
public class BattleActionQueue
{
    private readonly Dictionary<BattleCharacter, List<Action>> queues = new();
    public const int MaxActions = 10;

    /// <summary>
    /// Adds an action to the target character's queue if below MaxActions.
    /// </summary>
    public void QueueAction(BattleCharacter target, Action action)
    {
        if (target == null || action == null)
            return;
        if (!queues.TryGetValue(target, out var list))
        {
            list = new List<Action>();
            queues[target] = list;
        }
        if (list.Count >= MaxActions)
            return;
        list.Add(action);
    }

    /// <summary>
    /// Moves an action at oldIndex to newIndex within the target's queue.
    /// </summary>
    public void ReorderAction(BattleCharacter target, int oldIndex, int newIndex)
    {
        if (!queues.TryGetValue(target, out var list))
            return;
        if (oldIndex < 0 || oldIndex >= list.Count || newIndex < 0 || newIndex >= list.Count)
            return;
        Action act = list[oldIndex];
        list.RemoveAt(oldIndex);
        list.Insert(newIndex, act);
    }

    /// <summary>
    /// Attempts to pop the next queued action for the target.
    /// </summary>
    public bool TryPopAction(BattleCharacter target, out Action action)
    {
        action = null;
        if (!queues.TryGetValue(target, out var list) || list.Count == 0)
            return false;
        action = list[0];
        list.RemoveAt(0);
        return true;
    }
}
