using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct EnemyEntry
{
    GameObject enemyPrefab;
    Transform position;
}

public class EnemyLayout : ScriptableObject
{
    [SerializeField] private List<EnemyEntry> layout;
}