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

    private readonly List<BattleCharacter> turnQueue = new List<BattleCharacter>();
    private int currentTurnIndex;

    private void Start()
    {
        SetupBattle();
        StartCoroutine(BattleLoop());
    }

    private void SetupBattle()
    {
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
}

/// <summary>
/// Runtime battle representation of a character.
/// </summary>
public class BattleCharacter
{
    public CharacterData data;
    public int currentHP;
    public bool isPlayer;

    public BattleCharacter(CharacterData data, bool isPlayer)
    {
        this.data = data;
        this.isPlayer = isPlayer;
        currentHP = data.maxHP;
    }
}
