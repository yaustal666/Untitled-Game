using System;
using System.Collections.Generic;
using UnityEngine;

public enum QuestObjectiveType
{
    Kill,
    Collect,
    Interact
}

[Serializable]
public struct QuestObjectiveData
{
    public QuestObjectiveType ObjectiveType;
    public string TargetId;
    public int RequiredAmount;
    public string Description;
}

public enum QuestRewardType
{
    Item,
    Currency
}

[Serializable]
public struct QuestRewardData
{
    public QuestRewardType RewardType;
    public string ItemId;
    public int ItemAmount;
    public int CurrencyAmount;
}

[CreateAssetMenu(fileName = "QuestData", menuName = "ScriptableObjects/Quest Data")]
public class QuestData : ScriptableObject
{
    [field: SerializeField] public string Id { get; private set; }
    public List<QuestObjectiveData> ObjectivesData => _objectivesData;
    public List<QuestRewardData> RewardsData => _rewardsData;

    [SerializeField] private List<QuestObjectiveData> _objectivesData = new();
    [SerializeField] private List<QuestRewardData> _rewardsData = new();
}