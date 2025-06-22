using UnityEngine;

/// <summary>
/// Event that applies lecture stat bonuses to a character.
/// </summary>
public class LectureEvent : MonoBehaviour
{
    public CharacterData target;
    public LectureDefinition lecture;

    /// <summary>
    /// Applies the lecture bonuses to the target character.
    /// </summary>
    public void Trigger()
    {
        if (target == null || lecture == null)
        {
            return;
        }

        target.maxHP += lecture.hpBonus;
        target.maxMP += lecture.mpBonus;
        target.strength += lecture.strengthBonus;
        target.defense += lecture.defenseBonus;
        target.agility += lecture.agilityBonus;

        Debug.Log($"{target.characterName} attended {lecture.lectureTitle} and gained stat bonuses.");
    }
}
