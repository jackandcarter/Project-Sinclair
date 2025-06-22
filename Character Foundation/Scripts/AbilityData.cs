using UnityEngine;

[CreateAssetMenu(fileName = "AbilityData", menuName = "Sinclair/Ability Data")]
public class AbilityData : ScriptableObject
{
    public string abilityName;
    public string description;
    public int manaCost;
    public float cooldown;

    /// <summary>
    /// Applies this ability's effect from the user to the target.
    /// Override in derived classes for custom behavior.
    /// </summary>
    public virtual void Apply(BattleCharacter user, BattleCharacter target) { }
}

