using System;
using System.Collections.Generic;
using System.Linq;

public enum QuestStatus
{
    NotStarted,
    Active,
    Completed
}

public class Quest
{
    public event Action<Quest, QuestObjective> ObjectiveProgressChanged;
    public event Action<string> QuestCompleted;

    public string Id { get; private set; }
    public QuestStatus Status { get; private set; }
    public List<QuestObjective> Objectives => _objectives;

    private List<QuestObjective> _objectives;
    public List<QuestRewardData> rewards;

    public Quest(QuestData data)
    {
        Id = data.Id;
        Status = QuestStatus.NotStarted;
        _objectives = data.ObjectivesData.Select(d => new QuestObjective(d)).ToList();
        rewards = data.RewardsData;
    }

    public void Activate()
    {
        if (Status != QuestStatus.NotStarted) return;
        Status = QuestStatus.Active;
    }

    public void HandleGameEvent(QuestObjectiveType eventType, string targetId, int amount)
    {
        if (Status != QuestStatus.Active) return;

        bool progressChanged = false;
        foreach (var objective in _objectives)
        {
            if (objective.EvaluateProgress(eventType, targetId, amount))
            {
                progressChanged = true;
                ObjectiveProgressChanged?.Invoke(this, objective);
            }
        }

        if (progressChanged)
        {
            CheckStatusTransition();
        }
    }

    public void CheckStatusTransition()
    {
        if (Status != QuestStatus.Active) return;

        if (_objectives.All(objective => objective.IsCompleted))
        {
            Status = QuestStatus.Completed;
            QuestCompleted?.Invoke(Id);
        }
    }

    // TODO: search linQ
    public void RestoreState(QuestStatus savedStatus, List<int> savedProgress)
    {
        Status = savedStatus;
        for (int i = 0; i < _objectives.Count && i < savedProgress.Count; i++)
        {
            _objectives[i].RestoreProgress(savedProgress[i]);
        }
    }
}
