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

        // Simple attack using character strength and defense values
        int damage = Mathf.Max(1, character.data.strength - target.data.defense);
        target.currentHP -= damage;

        Debug.Log($"{character.data.characterName} attacks {target.data.characterName} for {damage} damage!");

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
