using UnityEngine;

[CreateAssetMenu(fileName = "AbilityData", menuName = "Sinclair/Ability Data")]
public class AbilityData : ScriptableObject
{
    public string abilityName;
    public string description;
    public int manaCost;
    public float cooldown;
}
