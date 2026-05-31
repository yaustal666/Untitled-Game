using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LootTableEntry
{
    public string itemID;
    public float DropChance;
    public int Amount;

#if UNITY_EDITOR
    [SerializeField] public ItemData itemRef;
#endif
}

[CreateAssetMenu(fileName = "LootTable", menuName = "ScriptableObjects/Loot Table")]
public class LootTable : ScriptableObject
{
    public List<LootTableEntry> entries = new List<LootTableEntry>();

#if UNITY_EDITOR
    void OnValidate()
    {
        foreach (var entry in entries)
        {
            if (entry.itemRef != null)
                entry.itemID = entry.itemRef.Id;
        }
    }
#endif
}