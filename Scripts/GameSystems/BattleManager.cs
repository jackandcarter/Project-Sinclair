using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Very simple battle system with a turn order based on agility.
/// Characters can perform basic attacks.
/// </summary>
public class BattleManager : MonoBehaviour
{
    public List<CharacterData> playerCharacters = new List<CharacterData>();
    public List<CharacterData> enemyCharacters = new List<CharacterData>();
    public InventoryManager inventory; // Tracks consumable items
    public ConsumableItem defaultItem;  // Item used for demo actions
    public AbilitySystem abilitySystem; // Handles ability execution

    // Prefab used to contain combatants during battle
    public BattleBarrier barrierPrefab;

    // Radius of the instantiated barrier
    public float barrierRadius = 10f;

    private BattleBarrier activeBarrier;

    private readonly List<BattleCharacter> turnQueue = new List<BattleCharacter>();
    private int currentTurnIndex;

    private readonly BattleActionQueue actionQueue = new BattleActionQueue();

    private void Start()
    {
        if (abilitySystem == null)
        {
            abilitySystem = GetComponent<AbilitySystem>();
        }
        SetupBattle();
        StartCoroutine(BattleLoop());
    }

    private void Update()
    {
        float dt = Time.deltaTime;
        foreach (BattleCharacter c in turnQueue)
        {
            if (c.currentHP <= 0)
                continue;
            c.UpdateATB(dt);
            if (c.IsATBFull && actionQueue.TryPopAction(c, out var act))
            {
                act?.Invoke();
                c.ResetATB();
            }
        }
    }

    private void SetupBattle()
    {
        if (barrierPrefab != null && activeBarrier == null)
        {
            activeBarrier = Instantiate(barrierPrefab, transform.position, Quaternion.identity);
            activeBarrier.radius = barrierRadius;
        }

        foreach (CharacterData data in playerCharacters)
        {
            turnQueue.Add(new BattleCharacter(data, true));
        }
        foreach (CharacterData data in enemyCharacters)
        {
            turnQueue.Add(new BattleCharacter(data, false));
        }

        // Sort characters by agility (highest goes first)
        turnQueue.Sort((a, b) => b.data.agility.CompareTo(a.data.agility));
        currentTurnIndex = 0;
    }

    private IEnumerator BattleLoop()
    {
        while (!IsBattleOver())
        {
            BattleCharacter active = turnQueue[currentTurnIndex];
            yield return StartCoroutine(TakeTurn(active));

            currentTurnIndex = (currentTurnIndex + 1) % turnQueue.Count;
        }

        Debug.Log("Battle ended");
        if (activeBarrier != null)
        {
            Destroy(activeBarrier.gameObject);
            activeBarrier = null;
        }
    }

    private IEnumerator TakeTurn(BattleCharacter character)
    {
        BattleCharacter target = SelectTarget(character);
        if (target == null)
        {
            yield break;
        }

        // Demonstration of using an item when below half HP
        if (character.isPlayer && defaultItem != null && inventory != null && character.currentHP <= character.data.maxHP / 2 && inventory.GetQuantity(defaultItem) > 0)
        {
            UseItem(character, defaultItem, character);
        }
        else if (abilitySystem != null && character.data.abilities.Count > 0 && abilitySystem.CanUseAbility(character, character.data.abilities[0]))
        {
            AbilityData ability = character.data.abilities[0];
            abilitySystem.UseAbility(character, target, ability);
        }
        else
        {
            // Simple attack using character strength and defense values
            int damage = Mathf.Max(1, character.data.strength - target.data.defense);

            if (!(target.isPlayer && DevConsole.IsGodMode))
            {
                target.currentHP -= damage;
            }
            else
            {
                damage = 0;
            }

            Debug.Log($"{character.data.characterName} attacks {target.data.characterName} for {damage} damage!");
        }

        yield return new WaitForSeconds(1f);
    }

    private BattleCharacter SelectTarget(BattleCharacter attacker)
    {
        List<BattleCharacter> potentialTargets = attacker.isPlayer ? GetAliveCharacters(false) : GetAliveCharacters(true);
        return potentialTargets.Count > 0 ? potentialTargets[0] : null;
    }

    private bool IsBattleOver()
    {
        bool playersAlive = GetAliveCharacters(true).Count > 0;
        bool enemiesAlive = GetAliveCharacters(false).Count > 0;
        return !(playersAlive && enemiesAlive);
    }

    private List<BattleCharacter> GetAliveCharacters(bool playerTeam)
    {
        List<BattleCharacter> list = new List<BattleCharacter>();
        foreach (BattleCharacter c in turnQueue)
        {
            if (c.isPlayer == playerTeam && c.currentHP > 0)
            {
                list.Add(c);
            }
        }
        return list;
    }

    /// <summary>
    /// Finds the runtime battle character associated with the given data.
    /// </summary>
    public BattleCharacter FindCharacter(CharacterData data)
    {
        foreach (BattleCharacter c in turnQueue)
        {
            if (c.data == data)
            {
                return c;
            }
        }
        return null;
    }

    /// <summary>
    /// Uses a consumable item on the target.
    /// </summary>
    public void UseItem(BattleCharacter user, ConsumableItem item, BattleCharacter target)
    {
        if (inventory != null && user != null)
        {
            inventory.UseItem(item, target);
            Debug.Log($"{user.data.characterName} uses {item.itemName} on {target.data.characterName}.");
        }
    }

    /// <summary>
    /// Queues an action to be executed when the target's ATB gauge is full.
    /// </summary>
    public void QueueAction(BattleCharacter target, System.Action action)
    {
        actionQueue.QueueAction(target, action);
    }

    /// <summary>
    /// Reorders a queued action for the target.
    /// </summary>
    public void ReorderAction(BattleCharacter target, int oldIndex, int newIndex)
    {
        actionQueue.ReorderAction(target, oldIndex, newIndex);
    }
}

/// <summary>
/// Runtime battle representation of a character.
/// </summary>
public class BattleCharacter
{
    public CharacterData data;
    public int currentHP;
    public int currentMP;
    public bool isPlayer;

    public const float MaxATB = 100f;
    public float atbGauge;

    /// <summary>
    /// Returns true when the ATB gauge has reached MaxATB.
    /// </summary>
    public bool IsATBFull => atbGauge >= MaxATB;

    public BattleCharacter(CharacterData data, bool isPlayer)
    {
        this.data = data;
        this.isPlayer = isPlayer;
        currentHP = data.maxHP;
        currentMP = data.maxMP;
        atbGauge = 0f;
    }

    /// <summary>
    /// Advances the ATB gauge based on agility.
    /// </summary>
    public void UpdateATB(float deltaTime)
    {
        atbGauge += deltaTime * data.agility;
        if (atbGauge > MaxATB)
            atbGauge = MaxATB;
    }

    /// <summary>
    /// Resets the ATB gauge to zero.
    /// </summary>
    public void ResetATB()
    {
        atbGauge = 0f;
    }
}
