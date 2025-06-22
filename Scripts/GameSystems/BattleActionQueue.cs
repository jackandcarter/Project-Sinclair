using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple queue that executes dock actions sequentially.
/// </summary>
public class BattleActionQueue : MonoBehaviour
{
    public static BattleActionQueue Instance { get; private set; }

    [Tooltip("Battle manager that processes queued actions.")]
    public BattleManager battleManager;

    private readonly Queue<IDockAction> queue = new();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    /// <summary>
    /// Adds a dock action to the queue.
    /// </summary>
    public void QueueAction(IDockAction action)
    {
        if (action != null)
            queue.Enqueue(action);
    }

    private void Update()
    {
        if (battleManager == null || queue.Count == 0)
            return;

        IDockAction action = queue.Dequeue();
        action.Execute(battleManager);
    }
}
