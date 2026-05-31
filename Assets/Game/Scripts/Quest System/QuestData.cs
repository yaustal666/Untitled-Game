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
    Stat
}

[Serializable]
public struct QuestRewardData
{
    public QuestRewardType RewardType;
    public string ItemId;
    public string StatId;
    public int ItemAmount;
    public float StatAmount;
}

[CreateAssetMenu(fileName = "QuestData", menuName = "ScriptableObjects/Quest Data")]
public class QuestData : ScriptableObject
{
    [field: SerializeField] public string Id { get; private set; }
    [field: SerializeField] public string Title { get; private set; }
    [field: SerializeField][TextArea] public string Description { get; private set; }

    [SerializeField] private List<QuestObjectiveData> _objectivesData = new();
    [SerializeField] private List<QuestRewardData> _rewardsData = new();

    public List<QuestObjectiveData> ObjectivesData => _objectivesData;
    public List<QuestRewardData> RewardsData => _rewardsData;
}
