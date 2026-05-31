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
        _stats.Add(StatType.CritChance, new CharacterStat(0.01f));
    }

    public float GetStatValue(StatType type)
    {
        return _stats[type].Value;
    }

    public void ModifyStat(StatType type, int value)
    {
        _stats[type].AddDifference(value);
    }
}