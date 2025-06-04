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
        GUILayout.Label("Ability Editor", EditorStyles.boldLabel);

        ability = (AbilityData)EditorGUILayout.ObjectField("Ability", ability, typeof(AbilityData), false);
        if (ability == null)
        {
            EditorGUILayout.HelpBox("Select or create an Ability asset.", MessageType.Info);
            if (GUILayout.Button("Create New"))
            {
                CreateNewAbility();
            }
            return;
        }

        SerializedObject so = new SerializedObject(ability);
        so.Update();

        EditorGUILayout.PropertyField(so.FindProperty("abilityName"));
        EditorGUILayout.PropertyField(so.FindProperty("description"));
        EditorGUILayout.PropertyField(so.FindProperty("mpCost"));
        EditorGUILayout.PropertyField(so.FindProperty("power"));
        EditorGUILayout.PropertyField(so.FindProperty("animation"));

        so.ApplyModifiedProperties();
    }

    private void CreateNewAbility()
    {
        ability = ScriptableObject.CreateInstance<AbilityData>();
        string path = EditorUtility.SaveFilePanelInProject("Save Ability Data", "NewAbilityData", "asset", "Specify where to save the asset.");
        if (!string.IsNullOrEmpty(path))
        {
            AssetDatabase.CreateAsset(ability, path);
            AssetDatabase.SaveAssets();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = ability;
        }
    }
}
