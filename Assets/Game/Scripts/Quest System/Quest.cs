using System;
using System.Collections.Generic;
using System.Linq;

public enum QuestStatus
{
    NotStarted,
    Active,
    CanComplete,
    Completed
}

public class Quest
{
    public event Action<Quest, QuestObjective> OnObjectiveProgressChanged;

    private readonly QuestData _data;
    private readonly List<QuestObjective> _objectives;
    private QuestStatus _status;

    public string Id => _data.Id;
    public QuestData Data => _data;
    public QuestStatus Status => _status;
    public IReadOnlyList<QuestObjective> Objectives => _objectives;

    public Quest(QuestData data)
    {
        _data = data;
        _status = QuestStatus.NotStarted;
        _objectives = _data.ObjectivesData.Select(d => new QuestObjective(d)).ToList();
    }

    public void Activate()
    {
        if (_status != QuestStatus.NotStarted) return;
        _status = QuestStatus.Active;
    }

    public void HandleGameEvent(QuestObjectiveType eventType, string targetId, int amount)
    {
        if (_status != QuestStatus.Active) return;

        bool progressChanged = false;
        foreach (var objective in _objectives)
        {
            if (objective.EvaluateProgress(eventType, targetId, amount))
            {
                progressChanged = true;
                OnObjectiveProgressChanged?.Invoke(this, objective);
            }
        }

        if (progressChanged)
        {
            CheckStatusTransition();
        }
    }

    public void Complete()
    {
        if (_status != QuestStatus.CanComplete) return;
        _status = QuestStatus.Completed;
    }

    private void CheckStatusTransition()
    {
        if (_status != QuestStatus.Active) return;

        if (_objectives.All(o => o.IsCompleted))
        {
            _status = QuestStatus.CanComplete;
        }
    }

    public void RestoreState(QuestStatus savedStatus, List<int> savedProgress)
    {
        _status = savedStatus;
        for (int i = 0; i < _objectives.Count && i < savedProgress.Count; i++)
        {
            _objectives[i].RestoreProgress(savedProgress[i]);
        }
    }
}
