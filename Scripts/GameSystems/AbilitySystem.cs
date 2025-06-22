using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Runtime system for handling ability costs, cooldowns and execution.
/// </summary>
public class AbilitySystem : MonoBehaviour
{
    private readonly Dictionary<BattleCharacter, Dictionary<AbilityData, float>> cooldowns = new();

    public bool CanUseAbility(BattleCharacter user, AbilityData ability)
    {
        if (user == null || ability == null)
            return false;
        if (user.currentMP < ability.manaCost)
            return false;
        if (cooldowns.TryGetValue(user, out var dict) && dict.TryGetValue(ability, out float cd) && cd > 0f)
            return false;
        return true;
    }

    public void UseAbility(BattleCharacter user, BattleCharacter target, AbilityData ability)
    {
        if (!CanUseAbility(user, ability))
            return;
        user.currentMP -= ability.manaCost;
        ability.Apply(user, target);
        if (!cooldowns.TryGetValue(user, out var dict))
            cooldowns[user] = dict = new Dictionary<AbilityData, float>();
        dict[ability] = ability.cooldown;
    }

    private void Update()
    {
        float dt = Time.deltaTime;
        foreach (var kv in cooldowns)
        {
            var abilities = new List<AbilityData>(kv.Value.Keys);
            foreach (var ab in abilities)
            {
                kv.Value[ab] -= dt;
                if (kv.Value[ab] <= 0f)
                    kv.Value.Remove(ab);
            }
        }
    }
}

