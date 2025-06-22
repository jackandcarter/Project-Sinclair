using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tracks active and completed quests for the player.
/// </summary>
public class QuestManager : MonoBehaviour
{
    public List<QuestDefinition> activeQuests = new List<QuestDefinition>();
    public List<QuestDefinition> completedQuests = new List<QuestDefinition>();

    /// <summary>
    /// Adds a new quest if it isn't already active or completed.
    /// </summary>
    public void AddQuest(QuestDefinition quest)
    {
        if (quest == null || activeQuests.Contains(quest) || completedQuests.Contains(quest))
        {
            return;
        }

        activeQuests.Add(quest);
    }

    /// <summary>
    /// Marks a quest as completed and removes it from the active list.
    /// </summary>
    public void CompleteQuest(QuestDefinition quest)
    {
        if (quest == null)
        {
            return;
        }

        if (activeQuests.Remove(quest))
        {
            completedQuests.Add(quest);
            Debug.Log($"Quest completed: {quest.questName}");
        }
    }

    /// <summary>
    /// Returns true if the quest is currently active.
    /// </summary>
    public bool IsQuestActive(QuestDefinition quest)
    {
        return activeQuests.Contains(quest);
    }

    /// <summary>
    /// Returns true if the quest has already been completed.
    /// </summary>
    public bool IsQuestCompleted(QuestDefinition quest)
    {
        return completedQuests.Contains(quest);
    }
}
