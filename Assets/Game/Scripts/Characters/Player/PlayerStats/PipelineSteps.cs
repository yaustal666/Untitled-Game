using System.Collections.Generic;

public class FlatModifiersStep : IStatPipelineStep
{
    public float Caclulate(float currentValue, PlayerStats stats, IReadOnlyList<StatModifier> modifiers)
    {
        var value = currentValue;
        for (int i = 0; i < modifiers.Count; i++)
        {
            if (modifiers[i].ModifierType == StatModificationType.Flat)
            {
                value += modifiers[i].Value;
            }
        }
        return value;
    }
}

public class MultiplerModifiersStep : IStatPipelineStep
{
    public float Caclulate(float currentValue, PlayerStats stats, IReadOnlyList<StatModifier> modifiers)
    {
        var value = currentValue;
        for (int i = 0; i < modifiers.Count; i++)
        {
            if (modifiers[i].ModifierType == StatModificationType.Multiplier)
            {
                value *= modifiers[i].Value;
            }
        }
        return value;
    }
}