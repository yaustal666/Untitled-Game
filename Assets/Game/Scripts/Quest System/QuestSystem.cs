using Reflex.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

public class QuestSystem : IDisposable, ISavable
{
    [Inject] private GameEvents _eventBus;

    public event Action<Quest> OnQuestActivated;

    private Dictionary<string, Quest> _quests = new();

    public QuestSystem(GameEvents eventBus, List<QuestData> questDataList, ISaveRegistry saveRegistry)
    {
        foreach (var questData in questDataList)
        {
            var quest = new Quest(questData);
            quest.QuestCompleted += OnQuestCompleted;
            _quests.Add(quest.Id, quest);
        }

        _eventBus = eventBus;
        _eventBus.Subscribe<EnemyKilledMessage>(OnEnemyKilled);
        _eventBus.Subscribe<ItemGainedMessage>(OnItemGained);

        saveRegistry.Register(this);
    }

    public void Dispose()
    {
        _eventBus.Unsubscribe<EnemyKilledMessage>(OnEnemyKilled);
        _eventBus.Unsubscribe<ItemGainedMessage>(OnItemGained);
    }

    private void OnQuestCompleted(string questId)
    {
        var quest = GetQuest(questId);
        _eventBus.Publish<QuestCompleted>(new QuestCompleted
        {
            rewards = quest.rewards,
        });
    }

    public void ActivateQuest(string questId)
    {
        var quest = GetQuest(questId);
        if (quest == null || quest.Status != QuestStatus.NotStarted) return;

        quest.Activate();

        UnityEngine.Debug.Log(questId + " Started");

        foreach (var objective in quest.Objectives)
        {
            if (objective.ObjectiveType == QuestObjectiveType.Collect)
            {
                var query = new GetItemCountQuery { ItemId = objective.TargetId };
                var amount = _eventBus.Query<GetItemCountQuery, int>(query);
                objective.RestoreProgress(amount);
            }
        }

        OnQuestActivated?.Invoke(quest);
        quest.CheckStatusTransition();
    }

    public QuestStatus GetQuestStatus(string questId)
    {
        return _quests.TryGetValue(questId, out var quest) ? quest.Status : QuestStatus.NotStarted;
    }

    public Quest GetQuest(string questId)
    {
        return _quests.TryGetValue(questId, out var quest) ? quest : null;
    }

    public List<Quest> GetAllQuests()
    {
        return _quests.Values.ToList();
    }

    private void OnEnemyKilled(EnemyKilledMessage e)
    {
        _quests.Values.ToList().ForEach(quest => quest.HandleGameEvent(QuestObjectiveType.Kill, e.EnemyId, e.Count));
    }

    private void OnItemGained(ItemGainedMessage e)
    {
        _quests.Values.ToList().ForEach(quest => quest.HandleGameEvent(QuestObjectiveType.Collect, e.ItemId, e.Amount));
    }

    public void Save(GameSaveData saveData)
    {
        foreach (var quest in _quests.Values)
        {
            if (quest.Status == QuestStatus.NotStarted) continue;

            saveData.QuestSystemData.Add(new QuestSaveData
            {
                QuestId = quest.Id,
                Status = quest.Status,
                ObjectivesProgress = quest.Objectives.Select(o => o.CurrentAmount).ToList()
            });
        }
    }

    public void Load(GameSaveData saveData)
    {
        foreach (var questSaveData in saveData.QuestSystemData)
        {
            if (_quests.TryGetValue(questSaveData.QuestId, out var quest))
            {
                quest.RestoreState(questSaveData.Status, questSaveData.ObjectivesProgress);
            }
        }
    }
}
