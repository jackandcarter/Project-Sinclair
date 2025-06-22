using UnityEngine;
using UnityEditor;

public class ItemMakerWindow : EditorWindow
{
    private ConsumableItem item;

    [MenuItem("Window/Sinclair Tools/Item Maker")]
    public static void ShowWindow()
    {
        GetWindow<ItemMakerWindow>(false, "Item Maker");
    }

    private void OnGUI()
    {
        GUILayout.Label("Item Maker", EditorStyles.boldLabel);

        item = (ConsumableItem)EditorGUILayout.ObjectField("Item", item, typeof(ConsumableItem), false);
        if (item == null)
        {
            EditorGUILayout.HelpBox("Select or create a Consumable Item asset.", MessageType.Info);
            if (GUILayout.Button("Create New"))
            {
                CreateNewItem();
            }
            if (GUILayout.Button("Load Item"))
            {
                LoadItem();
            }
            return;
        }

        EditorGUILayout.Space();
        item.itemName = EditorGUILayout.TextField("Name", item.itemName);
        item.description = EditorGUILayout.TextField("Description", item.description);
        item.healAmount = EditorGUILayout.IntField("Heal Amount", item.healAmount);

        if (GUILayout.Button("Save"))
        {
            SaveItem();
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(item);
        }
    }

    private void CreateNewItem()
    {
        item = ScriptableObject.CreateInstance<ConsumableItem>();
        string path = EditorUtility.SaveFilePanelInProject("Save Consumable Item", "NewConsumableItem", "asset", "Specify where to save the asset.");
        if (!string.IsNullOrEmpty(path))
        {
            AssetDatabase.CreateAsset(item, path);
            AssetDatabase.SaveAssets();
        }
    }

    private void SaveItem()
    {
        if (item != null)
        {
            EditorUtility.SetDirty(item);
            AssetDatabase.SaveAssets();
        }
    }

    private void LoadItem()
    {
        string path = EditorUtility.OpenFilePanel("Load Consumable Item", "Assets", "asset");
        if (!string.IsNullOrEmpty(path))
        {
            path = FileUtil.GetProjectRelativePath(path);
            item = AssetDatabase.LoadAssetAtPath<ConsumableItem>(path);
        }
    }
}
