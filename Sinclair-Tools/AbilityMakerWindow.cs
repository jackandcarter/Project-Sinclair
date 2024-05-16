using UnityEngine;
using UnityEditor;

public class AbilityMakerWindow : EditorWindow
{
    [MenuItem("Window/Ability Maker")]
    public static void ShowWindow()
    {
        // Show existing window instance. If one doesn't exist, create one.
        EditorWindow.GetWindow(typeof(AbilityMakerWindow), false, "Ability Maker");
    }

    private void OnGUI()
    {
        GUILayout.Label("Ability Maker Main Menu", EditorStyles.boldLabel);

        // Create buttons for various actions
        if (GUILayout.Button("Create New Ability"))
        {
            CreateNewAbility();
        }

        if (GUILayout.Button("Open Existing Ability"))
        {
            OpenExistingAbility();
        }

        if (GUILayout.Button("Save Ability"))
        {
            SaveAbility();
        }

        if (GUILayout.Button("Load Ability"))
        {
            LoadAbility();
        }
    }

    private void CreateNewAbility()
    {
        // Logic for creating a new ability
        Debug.Log("Creating a new ability...");
    }

    private void OpenExistingAbility()
    {
        // Logic for opening an existing ability
        Debug.Log("Opening an existing ability...");
    }

    private void SaveAbility()
    {
        // Logic for saving the current ability
        Debug.Log("Saving the ability...");
    }

    private void LoadAbility()
    {
        // Logic for loading a saved ability
        Debug.Log("Loading an ability...");
    }
}
