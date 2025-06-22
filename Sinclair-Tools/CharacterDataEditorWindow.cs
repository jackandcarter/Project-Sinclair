using UnityEngine;
using UnityEditor;

public class CharacterDataEditorWindow : EditorWindow
{
    private CharacterData data;

    [MenuItem("Window/Sinclair Tools/Character Data Editor")]
    public static void ShowWindow()
    {
        GetWindow<CharacterDataEditorWindow>(false, "Character Data Editor");
    }

    private void OnGUI()
    {
        GUILayout.Label("Character Data", EditorStyles.boldLabel);

        data = (CharacterData)EditorGUILayout.ObjectField("Data", data, typeof(CharacterData), false);
        if (data == null)
        {
            EditorGUILayout.HelpBox("Select or create a Character Data asset.", MessageType.Info);
            if (GUILayout.Button("Create New"))
            {
                CreateNewData();
            }
            return;
        }

        EditorGUILayout.Space();
        data.characterName = EditorGUILayout.TextField("Name", data.characterName);
        data.level = EditorGUILayout.IntField("Level", data.level);
        data.maxHP = EditorGUILayout.IntField("Max HP", data.maxHP);
        data.maxMP = EditorGUILayout.IntField("Max MP", data.maxMP);
        data.strength = EditorGUILayout.IntField("Strength", data.strength);
        data.defense = EditorGUILayout.IntField("Defense", data.defense);
        data.agility = EditorGUILayout.IntField("Agility", data.agility);

        EditorGUILayout.LabelField("Abilities");
        int count = Mathf.Max(0, EditorGUILayout.IntField("Size", data.abilities.Count));
        while (count > data.abilities.Count)
            data.abilities.Add(null);
        while (count < data.abilities.Count)
            data.abilities.RemoveAt(data.abilities.Count - 1);
        for (int i = 0; i < data.abilities.Count; i++)
        {
            data.abilities[i] = (AbilityData)EditorGUILayout.ObjectField($"Element {i}", data.abilities[i], typeof(AbilityData), false);
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(data);
        }
    }

    private void CreateNewData()
    {
        data = ScriptableObject.CreateInstance<CharacterData>();
        string path = EditorUtility.SaveFilePanelInProject("Save Character Data", "NewCharacterData", "asset", "Specify where to save the asset.");
        if (!string.IsNullOrEmpty(path))
        {
            AssetDatabase.CreateAsset(data, path);
            AssetDatabase.SaveAssets();
        }
    }
}
