using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Sinclair/Character Data")]
public class CharacterData : ScriptableObject
{
    public string characterName;
    public int level;
    public int maxHP;
    public int maxMP;
    public int strength;
    public int defense;
    public int agility;

    // Abilities the character has learned
    public List<AbilityData> abilities = new List<AbilityData>();
}

