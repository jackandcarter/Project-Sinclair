using UnityEngine;

/// <summary>
/// Data for a lecture that can grant stat bonuses.
/// </summary>
[CreateAssetMenu(fileName = "LectureDefinition", menuName = "Sinclair/Lecture Definition")]
public class LectureDefinition : ScriptableObject
{
    public string lectureTitle;
    public string description;
    public int hpBonus;
    public int mpBonus;
    public int strengthBonus;
    public int defenseBonus;
    public int agilityBonus;
}
