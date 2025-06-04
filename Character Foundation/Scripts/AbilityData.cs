using UnityEngine;

[CreateAssetMenu(fileName = "AbilityData", menuName = "Sinclair/Ability Data")]
public class AbilityData : ScriptableObject
{
    public string abilityName;
    [TextArea]
    public string description;
    public int mpCost;
    public int power;
    public AnimationClip animation;
}
