using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the demon invasion event in D\u00farnir's Pass. Plays an intro cutscene
/// and then starts a battle using the provided BattleManager.
/// </summary>
public class DemonInvasion : MonoBehaviour
{
    public CutsceneManager cutsceneManager;              // Camera and animation controller
    public List<CutsceneEvent> introEvents = new();       // Intro cutscene events
    public BattleManager battleManager;                   // Battle system
    public List<CharacterData> demonForces = new();       // Enemy combatants

    /// <summary>
    /// Entry point for starting the invasion sequence.
    /// </summary>
    public void BeginInvasion()
    {
        StartCoroutine(InvasionRoutine());
    }

    private IEnumerator InvasionRoutine()
    {
        if (cutsceneManager != null && introEvents.Count > 0)
        {
            yield return StartCoroutine(cutsceneManager.PlayCutscene(introEvents));
        }

        if (battleManager != null)
        {
            battleManager.enabled = false; // Reset any previous battle
            battleManager.enemyCharacters.Clear();
            battleManager.enemyCharacters.AddRange(demonForces);
            battleManager.enabled = true;  // BattleManager.Start will set up battle
        }
    }
}
