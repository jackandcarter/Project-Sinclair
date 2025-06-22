using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple capture the flag game mode with teams and scoring.
/// Players are teleported from a waiting room into the arena.
/// </summary>
public class CaptureTheFlag : MonoBehaviour
{
    public CutsceneManager cutsceneManager;                 // For intro cutscene
    public List<CutsceneEvent> introCutscene = new List<CutsceneEvent>();

    public Transform waitingRoomSpawn;                       // Lobby spawn
    public Transform teamASpawn;                             // Spawn for team A
    public Transform teamBSpawn;                             // Spawn for team B

    public GameObject flagPrefab;                            // Flag object
    public Transform flagSpawnA;                             // Spawn location team A flag
    public Transform flagSpawnB;                             // Spawn location team B flag

    public int scoreToWin = 3;                               // Points needed
    private int scoreA;
    private int scoreB;

    public InventoryManager inventory;                       // Reward inventory
    public ConsumableItem spawnBuff;                         // Item used on spawn

    /// <summary>
    /// Called on start to play the rules cutscene and spawn flags.
    /// </summary>
    private void Start()
    {
        if (cutsceneManager != null && introCutscene.Count > 0)
        {
            StartCoroutine(cutsceneManager.PlayCutscene(introCutscene));
        }

        SpawnFlags();
    }

    /// <summary>
    /// Spawns both team flags in the arena.
    /// </summary>
    private void SpawnFlags()
    {
        if (flagPrefab == null)
        {
            return;
        }

        if (flagSpawnA != null)
        {
            Instantiate(flagPrefab, flagSpawnA.position, flagSpawnA.rotation);
        }
        if (flagSpawnB != null)
        {
            Instantiate(flagPrefab, flagSpawnB.position, flagSpawnB.rotation);
        }
    }

    /// <summary>
    /// Teleports a player to the waiting room.
    /// </summary>
    public void TeleportToWaitingRoom(GameObject player)
    {
        if (player != null && waitingRoomSpawn != null)
        {
            player.transform.position = waitingRoomSpawn.position;
        }
    }

    /// <summary>
    /// Teleports a player into the arena at their team spawn.
    /// Triggers any BattleActionEvent on the spawn point.
    /// </summary>
    public void TeleportToArena(GameObject player, bool teamA)
    {
        Transform spawn = teamA ? teamASpawn : teamBSpawn;
        if (player == null || spawn == null)
        {
            return;
        }

        player.transform.position = spawn.position;

        BattleActionEvent evt = spawn.GetComponent<BattleActionEvent>();
        if (evt != null)
        {
            evt.Trigger(FindObjectOfType<BattleManager>());
        }
        else if (inventory != null && spawnBuff != null)
        {
            // Fallback: apply item directly using inventory system
            BattleCharacter c = FindObjectOfType<BattleManager>()?.FindCharacter(player.GetComponent<CharacterData>());
            if (c != null)
            {
                inventory.UseItem(spawnBuff, c);
            }
        }
    }

    /// <summary>
    /// Records a captured flag and checks for victory.
    /// </summary>
    public void CaptureFlag(bool teamA)
    {
        if (teamA)
        {
            scoreA++;
        }
        else
        {
            scoreB++;
        }

        if (scoreA >= scoreToWin || scoreB >= scoreToWin)
        {
            EndMatch();
        }
    }

    private void EndMatch()
    {
        Debug.Log("Capture the Flag match ended");
    }
}
