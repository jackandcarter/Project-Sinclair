using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Simple developer console that can be toggled in play mode.
/// </summary>
public class DevConsole : MonoBehaviour
{
    public KeyCode toggleKey = KeyCode.BackQuote;
    public InventoryManager inventory;

    private bool isOpen;
    private string input = string.Empty;

    private readonly Dictionary<string, Action<string[]>> commands = new();

    /// <summary>
    /// Whether player characters are invincible.
    /// </summary>
    public static bool IsGodMode { get; private set; }

    private void Awake()
    {
        RegisterCommand("give", CmdGive);
        RegisterCommand("teleport", CmdTeleport);
        RegisterCommand("godmode", CmdGodMode);
    }

    private void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            isOpen = !isOpen;
            input = string.Empty;
        }

        if (isOpen && Input.GetKeyDown(KeyCode.Return))
        {
            HandleInput(input);
            input = string.Empty;
        }
    }

    private void OnGUI()
    {
        if (!isOpen)
        {
            return;
        }

        GUI.Box(new Rect(5, 5, 410, 30), string.Empty);
        GUI.SetNextControlName("DevConsoleInput");
        input = GUI.TextField(new Rect(10, 10, 400, 20), input);
        GUI.FocusControl("DevConsoleInput");
    }

    /// <summary>
    /// Registers a console command.
    /// </summary>
    public void RegisterCommand(string name, Action<string[]> callback)
    {
        if (string.IsNullOrWhiteSpace(name) || callback == null)
        {
            return;
        }
        commands[name.ToLowerInvariant()] = callback;
    }

    private void HandleInput(string line)
    {
        if (string.IsNullOrWhiteSpace(line))
        {
            return;
        }

        string[] parts = line.Split(' ');
        if (parts.Length == 0)
        {
            return;
        }

        string cmd = parts[0].ToLowerInvariant();
        if (commands.TryGetValue(cmd, out Action<string[]> action))
        {
            action(parts);
        }
        else
        {
            Debug.Log($"Unknown command: {cmd}");
        }
    }

    private void CmdGive(string[] args)
    {
        if (args.Length < 3 || inventory == null)
        {
            Debug.Log("Usage: give <item> <amount>");
            return;
        }

        string itemName = args[1].ToLowerInvariant();
        if (!int.TryParse(args[2], out int amount))
        {
            amount = 1;
        }

        ConsumableItem item = LoadItem(itemName);
        if (item != null)
        {
            inventory.AddItem(item, amount);
            Debug.Log($"Gave {amount}x {item.itemName}");
        }
        else
        {
            Debug.Log($"Item not found: {itemName}");
        }
    }

    private ConsumableItem LoadItem(string name)
    {
        ConsumableItem item = Resources.Load<ConsumableItem>(name);
        if (item != null)
        {
            return item;
        }

        switch (name)
        {
            case "waterbottle":
            case "water bottle":
                WaterBottle wb = ScriptableObject.CreateInstance<WaterBottle>();
                wb.itemName = "Water Bottle";
                wb.description = "Restores a small amount of HP.";
                wb.healAmount = 10;
                return wb;
        }

        return null;
    }

    private void CmdTeleport(string[] args)
    {
        if (args.Length < 2)
        {
            Debug.Log("Usage: teleport <scene>");
            return;
        }
        SceneManager.LoadScene(args[1]);
    }

    private void CmdGodMode(string[] args)
    {
        IsGodMode = !IsGodMode;
        Debug.Log("God mode " + (IsGodMode ? "enabled" : "disabled"));
    }
}
