using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles a sequence of missions given by Lieutenant Reich at D\u00farnir's Pass.
/// Missions are stored as QuestDefinition assets and added to the player's
/// QuestManager as they are started.
/// </summary>
public class LieutenantReichMissions : MonoBehaviour
{
    public QuestManager questManager;               // Reference to the QuestManager
    public List<QuestDefinition> missions = new();  // Ordered mission list

    private int currentIndex;

    /// <summary>
    /// Begins the next mission in the list if available.
    /// </summary>
    public void StartNextMission()
    {
        if (questManager == null || currentIndex >= missions.Count)
        {
            return;
        }

        questManager.AddQuest(missions[currentIndex]);
    }

    /// <summary>
    /// Marks the current mission complete and advances the list.
    /// </summary>
    public void CompleteCurrentMission()
    {
        if (questManager == null || currentIndex >= missions.Count)
        {
            return;
        }

        QuestDefinition quest = missions[currentIndex];
        questManager.CompleteQuest(quest);
        currentIndex++;
    }
}
