#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

public class IDManager : EditorWindow
{
    [MenuItem("Tools/Item ID Manager")]
    public static void ShowWindow() => GetWindow<IDManager>("Item IDs");

    void OnGUI()
    {
        GUILayout.Label("Item ID Automation", EditorStyles.boldLabel);
        GUILayout.Space(10);

        if (GUILayout.Button("Assign Items IDs", GUILayout.Height(30)))
        {
            AssignAllItemIDs();
        }

        if (GUILayout.Button("Assign Quests IDs", GUILayout.Height(30)))
        {
            AssignAllQuestsIDs();
        }
    }

    void AssignAllItemIDs()
    {
        string[] guids = AssetDatabase.FindAssets("t:ItemData");
        int updatedCount = 0;

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            ItemData item = AssetDatabase.LoadAssetAtPath<ItemData>(path);

            if (item != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(path);

                string generatedId = $"item_{fileName.ToLower().Replace(" ", "_")}";

                SerializedObject so = new SerializedObject(item);

                SerializedProperty idProperty = so.FindProperty("<Id>k__BackingField");

                if (idProperty != null)
                {
                    if (idProperty.stringValue != generatedId)
                    {
                        idProperty.stringValue = generatedId;
                        so.ApplyModifiedProperties();

                        EditorUtility.SetDirty(item);
                        updatedCount++;
                    }
                }
                else
                {
                    Debug.LogError($"[ItemIDManager] Не удалось найти backing field свойства Id у ассета: {fileName}");
                }
            }
        }

        if (updatedCount > 0)
        {
            AssetDatabase.SaveAssets();
            Debug.Log($"[IDManager] Успешно обновлены ID у {updatedCount} предметов на основе имен файлов.");
        }
        else
        {
            Debug.Log("[IDManager] Все ID уже соответствуют именам файлов. Изменений не требуется.");
        }
    }

    void AssignAllQuestsIDs()
    {
        string[] guids = AssetDatabase.FindAssets("t:QuestData");
        int updatedCount = 0;

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            QuestData quest = AssetDatabase.LoadAssetAtPath<QuestData>(path);

            if (quest != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(path);

                string generatedId = $"quest_{fileName.ToLower().Replace(" ", "_")}";

                SerializedObject so = new SerializedObject(quest);

                SerializedProperty idProperty = so.FindProperty("<Id>k__BackingField");

                if (idProperty != null)
                {
                    if (idProperty.stringValue != generatedId)
                    {
                        idProperty.stringValue = generatedId;
                        so.ApplyModifiedProperties();

                        EditorUtility.SetDirty(quest);
                        updatedCount++;
                    }
                }
                else
                {
                    Debug.LogError($"[IDManager] Не удалось найти backing field свойства Id у ассета: {fileName}");
                }
            }
        }

        if (updatedCount > 0)
        {
            AssetDatabase.SaveAssets();
            Debug.Log($"[IDManager] Успешно обновлены ID у {updatedCount} предметов на основе имен файлов.");
        }
        else
        {
            Debug.Log("[IDManager] Все ID уже соответствуют именам файлов. Изменений не требуется.");
        }
    }
}
#endif
