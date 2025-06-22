using UnityEngine;
using UnityEditor;

public class AbilityMakerWindow : EditorWindow
{
    private AbilityData ability;

    [MenuItem("Window/Sinclair Tools/Ability Maker")]
    public static void ShowWindow()
    {
        GetWindow<AbilityMakerWindow>(false, "Ability Maker");
    }

    private void OnGUI()
    {
        GUILayout.Label("Ability Maker", EditorStyles.boldLabel);

        ability = (AbilityData)EditorGUILayout.ObjectField("Ability", ability, typeof(AbilityData), false);
        if (ability == null)
        {
            EditorGUILayout.HelpBox("Select or create an Ability asset.", MessageType.Info);
            if (GUILayout.Button("Create New"))
            {
                CreateNewAbility();
            }
            if (GUILayout.Button("Load Ability"))
            {
                LoadAbility();
            }
            return;
        }

        EditorGUILayout.Space();
        ability.abilityName = EditorGUILayout.TextField("Name", ability.abilityName);
        ability.description = EditorGUILayout.TextField("Description", ability.description);
        ability.manaCost = EditorGUILayout.IntField("Mana Cost", ability.manaCost);
        ability.cooldown = EditorGUILayout.FloatField("Cooldown", ability.cooldown);

        if (GUILayout.Button("Save"))
        {
            SaveAbility();
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(ability);
        }
    }

    private void CreateNewAbility()
    {
        ability = ScriptableObject.CreateInstance<AbilityData>();
        string path = EditorUtility.SaveFilePanelInProject("Save Ability Data", "NewAbilityData", "asset", "Specify where to save the asset.");
        if (!string.IsNullOrEmpty(path))
        {
            AssetDatabase.CreateAsset(ability, path);
            AssetDatabase.SaveAssets();
        }
    }

    private void OpenExistingAbility()
    {
        LoadAbility();
    }

    private void SaveAbility()
    {
        if (ability != null)
        {
            EditorUtility.SetDirty(ability);
            AssetDatabase.SaveAssets();
        }
    }

    private void LoadAbility()
    {
        string path = EditorUtility.OpenFilePanel("Load Ability Data", "Assets", "asset");
        if (!string.IsNullOrEmpty(path))
        {
            path = FileUtil.GetProjectRelativePath(path);
            ability = AssetDatabase.LoadAssetAtPath<AbilityData>(path);
        }
    }
}
