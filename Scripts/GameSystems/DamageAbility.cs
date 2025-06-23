using UnityEngine;

/// <summary>
/// Simple offensive ability that deals damage to a single target.
/// </summary>
[CreateAssetMenu(fileName = "DamageAbility", menuName = "Sinclair/Abilities/Damage")]
public class DamageAbility : AbilityData
{
    public int damage = 5;

    public override void Apply(BattleCharacter user, BattleCharacter target)
    {
        if (target == null)
            return;
        int finalDamage = Mathf.Max(1, damage + user.data.strength - target.data.defense);
        if (!(target.isPlayer && DevConsole.IsGodMode))
        {
            target.ChangeHP(-finalDamage);
        }
        else
        {
            finalDamage = 0;
        }
        Debug.Log($"{user.data.characterName} casts {abilityName} on {target.data.characterName} for {finalDamage} damage!");
    }
}

