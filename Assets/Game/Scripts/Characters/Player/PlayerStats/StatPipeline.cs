using System.Collections.Generic;

public interface IStatPipelineStep
{
    float Caclulate(float currentValue, PlayerStats stats, IReadOnlyList<StatModifier> modifiers);
}

public class StatPipeline
{
    private List<IStatPipelineStep> _steps = new();

    public StatPipeline AddStep(IStatPipelineStep step)
    {
        _steps.Add(step);
        return this;
    }

    public float Execute(float baseValue, PlayerStats stats, IReadOnlyList<StatModifier> modifiers)
    {
        var currentValue = baseValue;
        for (int i = 0; i < _steps.Count; i++)
        {
            currentValue = _steps[i].Caclulate(currentValue, stats, modifiers);
        }
        return currentValue;
    }
}