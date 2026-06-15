public enum StatModificationType
{
    Flat,
    Multiplier,
    Procent,
    Unique
}

public struct StatModifier
{
    public StatType StatType;
    public StatModificationType ModifierType;
    public string Source;
    public float Value;
}