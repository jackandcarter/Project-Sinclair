using UnityEngine;

/// <summary>
/// Data describing a quest or side mission.
/// </summary>
[CreateAssetMenu(fileName = "QuestDefinition", menuName = "Sinclair/Quest Definition")]
public class QuestDefinition : ScriptableObject
{
    public string questName;
    public string description;
    public bool isMainQuest;
}
