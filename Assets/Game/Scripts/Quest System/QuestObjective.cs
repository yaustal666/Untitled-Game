public class QuestObjective
{    
    public QuestObjectiveType ObjectiveType { get; private set; }
    public bool IsCompleted => CurrentAmount >= RequiredAmount;
    public int RequiredAmount { get; private set; }
    public string TargetId { get; private set; }
    public int CurrentAmount { get; private set; }

    public QuestObjective(QuestObjectiveData data)
    {
        ObjectiveType = data.ObjectiveType;
        TargetId = data.TargetId;
        RequiredAmount = data.RequiredAmount;
        CurrentAmount = 0;
    }

    public bool EvaluateProgress(QuestObjectiveType eventType, string targetId, int amount)
    {
        if (IsCompleted) return false;
        if (ObjectiveType != eventType || TargetId != targetId) return false;

        CurrentAmount = CurrentAmount + amount;
        return true;
    }

    public void RestoreProgress(int amount)
    {
        CurrentAmount = amount;
    }
}
