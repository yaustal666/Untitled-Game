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
        _eventBus = eventBus;

        foreach (var questData in questDataList)
        {
            var quest = new Quest(questData);
            _quests.Add(quest.Id, quest);
        }

        _eventBus.Subscribe<EnemyKilledMessage>(OnEnemyKilled);
        _eventBus.Subscribe<ItemGainedMessage>(OnItemGained);

        saveRegistry.Register(this);
    }

    public void Dispose()
    {
        _eventBus.Unsubscribe<EnemyKilledMessage>(OnEnemyKilled);
        _eventBus.Unsubscribe<ItemGainedMessage>(OnItemGained);
    }

    public QuestStatus GetQuestStatus(string questId)
    {
        return _quests.TryGetValue(questId, out var quest) ? quest.Status : QuestStatus.NotStarted;
    }

    public void ActivateQuest(string questId)
    {
        var quest = GetQuest(questId);
        if (quest == null || quest.Status != QuestStatus.NotStarted) return;

        quest.Activate();

        foreach (var objective in quest.Objectives)
        {
            if (objective.Type == QuestObjectiveType.Collect)
            {
                var query = new GetItemCountQuery { ItemId = objective.TargetId };
                var amount = _eventBus.Query<GetItemCountQuery, int>(query);
                objective.RestoreProgress(amount);
            }
        }

        OnQuestActivated?.Invoke(quest);
    }

    public void CompleteQuest(string questId)
    {
        var quest = GetQuest(questId);
        if (quest == null || quest.Status != QuestStatus.CanComplete) return;

        quest.Complete();
    }

    private void OnEnemyKilled(EnemyKilledMessage e)
    {
        _quests.Values.ToList().ForEach(quest => quest.HandleGameEvent(QuestObjectiveType.Kill, e.EnemyId, e.Count));
    }

    private void OnItemGained(ItemGainedMessage e)
    {
        _quests.Values.ToList().ForEach(quest => quest.HandleGameEvent(QuestObjectiveType.Collect, e.ItemId, e.Amount));
    }

    public Quest GetQuest(string questId)
    {
        return _quests.TryGetValue(questId, out var quest) ? quest : null;
    }

    public void Save(GameSaveData saveData)
    {
        //var dto = new QuestSystemSaveDto();

        //foreach (var quest in _quests.Values)
        //{
        //    if (quest.Status == QuestStatus.NotStarted) continue;

        //    dto.TrackedQuests.Add(new QuestSaveDto
        //    {
        //        QuestId = quest.Id,
        //        Status = quest.Status,
        //        ObjectivesProgress = quest.Objectives.Select(o => o.CurrentAmount).ToList()
        //    });
        //}

        //return dto;
    }

    public void Load(GameSaveData saveData)
    {
        //if (dto == null || dto.TrackedQuests == null) return;

        //foreach (var quest in _quests.Values)
        //{
        //    quest.RestoreState(QuestStatus.NotStarted, quest.Objectives.Select(_ => 0).ToList());
        //}

        //foreach (var savedQuestDto in dto.TrackedQuests)
        //{
        //    if (_quests.TryGetValue(savedQuestDto.QuestId, out var quest))
        //    {
        //        quest.RestoreState(savedQuestDto.Status, savedQuestDto.ObjectivesProgress);
        //    }
        //}
    }
}
