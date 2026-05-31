public class QuestObjective
{
    private QuestObjectiveData _data;
    private int _currentAmount;

    public bool IsCompleted => _currentAmount >= _data.RequiredAmount;
    public QuestObjectiveType Type => _data.ObjectiveType;
    public string TargetId => _data.TargetId;

    public QuestObjective(QuestObjectiveData data)
    {
        _data = data;
        _currentAmount = 0;
    }

    public bool EvaluateProgress(QuestObjectiveType eventType, string targetId, int amount)
    {
        if (IsCompleted) return false;
        if (_data.ObjectiveType != eventType || _data.TargetId != targetId) return false;

        _currentAmount = System.Math.Min(_currentAmount + amount, _data.RequiredAmount);
        return true;
    }

    public void RestoreProgress(int amount)
    {
        _currentAmount = amount;
    }
}
