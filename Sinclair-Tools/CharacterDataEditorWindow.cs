using UnityEngine;
using UnityEditor;

public class CharacterDataEditorWindow : EditorWindow
{
    private CharacterData data;
    private Vector2 scroll;

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

        SerializedObject so = new SerializedObject(data);
        so.Update();

        scroll = EditorGUILayout.BeginScrollView(scroll);
        EditorGUILayout.PropertyField(so.FindProperty("characterName"));
        EditorGUILayout.PropertyField(so.FindProperty("level"));
        EditorGUILayout.PropertyField(so.FindProperty("maxHP"));
        EditorGUILayout.PropertyField(so.FindProperty("maxMP"));
        EditorGUILayout.PropertyField(so.FindProperty("strength"));
        EditorGUILayout.PropertyField(so.FindProperty("defense"));
        EditorGUILayout.PropertyField(so.FindProperty("agility"));
        EditorGUILayout.PropertyField(so.FindProperty("magic"));
        EditorGUILayout.PropertyField(so.FindProperty("luck"));
        EditorGUILayout.PropertyField(so.FindProperty("abilities"), true);
        EditorGUILayout.EndScrollView();

        so.ApplyModifiedProperties();
    }

    private void CreateNewData()
    {
        data = ScriptableObject.CreateInstance<CharacterData>();
        string path = EditorUtility.SaveFilePanelInProject("Save Character Data", "NewCharacterData", "asset", "Specify where to save the asset.");
        if (!string.IsNullOrEmpty(path))
        {
            AssetDatabase.CreateAsset(data, path);
            AssetDatabase.SaveAssets();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = data;
        }
    }
}
