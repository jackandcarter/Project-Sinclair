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
