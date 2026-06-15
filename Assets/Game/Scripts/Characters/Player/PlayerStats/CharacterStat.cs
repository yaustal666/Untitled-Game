using System;

public enum StatType
{
    MaxHealth,
    Strength,
    Crit,
    CritChance
}

public class CharacterStat
{
    public event Action<float> StatChanged;
    public float Value => _runtimeStat;
    public float Base => _base;

    private float _base;
    private float _runtimeStat;

    public CharacterStat(float baseVal)
    {
        _base = baseVal;
    }

    public void SetRuntimeValue(float val)
    {
        _runtimeStat = val;
        StatChanged?.Invoke(_runtimeStat);
    }
}