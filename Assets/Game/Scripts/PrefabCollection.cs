using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/PrefabCollection", fileName = "PrefabCollection")]
public class PrefabCollection : ScriptableObject
{
    [SerializeField] private List<GameObject> prefabs = new List<GameObject>();
    [SerializeField] private GameObject startRoom;
    [SerializeField] private string path;

    public GameObject GetRandom()
    {
        var roomIdx = Random.Range(0, prefabs.Count);
        return prefabs[roomIdx];
    }

    public GameObject GetStartRoom()
    {
        return startRoom;
    }

    public GameObject Get(int idx)
    {
        return prefabs[idx];
    }

#if UNITY_EDITOR
    [ContextMenu("Load from Folder")]
    public void LoadFromFolder()
    {
        prefabs.Clear();

        GameObject[] loadedRooms = Resources.LoadAll<GameObject>("Rooms");
        prefabs.AddRange(loadedRooms);

        UnityEditor.EditorUtility.SetDirty(this);
        Debug.Log($"Loaded {prefabs.Count} rooms from Resources/Rooms");
    }
#endif
}