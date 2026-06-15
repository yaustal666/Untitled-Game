using System;
using System.Collections.Generic;

[Serializable]
public class PlayerStats
{
    private Dictionary<StatType, CharacterStat> _stats = new();

    public PlayerStats()
    {
        _stats.Add(StatType.MaxHealth, new CharacterStat(100f));
        _stats.Add(StatType.Strength, new CharacterStat(10f));
        _stats.Add(StatType.Crit, new CharacterStat(100f));
        _stats.Add(StatType.CritChance, new CharacterStat(0.01f));
    }

    public CharacterStat GetStat(StatType type)
    {
        return _stats[type];
    }

    public float GetStatValue(StatType type)
    {
        return _stats[type].Value;
    }
    public float GetBaseValue(StatType type)
    {
        return _stats[type].Base;
    }

    public void ModifyStat(StatType type, float value)
    {
        _stats[type].SetRuntimeValue(value);
    }
}