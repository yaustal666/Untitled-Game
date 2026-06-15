using System;
using System.Collections.Generic;

public class StatsCalculator : IDisposable
{
    private Player _player;

    private List<StatModifier> _hpModifiers = new();
    private List<StatModifier> _strengthModifiers = new();
    private List<StatModifier> _critModifiers = new();
    private List<StatModifier> _critChanceModifiers = new();

    private Dictionary<StatType, List<StatModifier>> _mappingTypeToModifierList;
    private Dictionary<StatType, StatPipeline> _mappingTypeToPipeline = new();

    public StatsCalculator(Player player)
    {
        _player = player;

        _mappingTypeToModifierList = new()
        {
            { StatType.MaxHealth, _hpModifiers },
            { StatType.Strength, _strengthModifiers },
            { StatType.Crit, _critModifiers },
            { StatType.CritChance, _critChanceModifiers }
        };

        _player.Mask.StatChanged += AddStatModifier;

        SetupPipelines();
    }

    public void Dispose()
    {
        _player.Mask.StatChanged -= AddStatModifier;
    }

    private void SetupPipelines()
    {
        SetupHealthPipeline();
        SetupStrengthPipeline();
        SetupCritPipeline();
        SetupCritChancePipeline();
    }

    public void AddStatModifier(StatModifier modifier)
    {
        UnityEngine.Debug.Log(modifier);
        var type = modifier.StatType;
        var modlist = _mappingTypeToModifierList[type];

        int sourceIndex = -1;
        for (int i = 0; i < modlist.Count; i++)
        {
            if (modlist[i].Source == modifier.Source)
            {
                sourceIndex = i;
                break;
            }
        }

        if (sourceIndex != -1)
        {
            modlist[sourceIndex] = modifier;
        }
        else
        {
            modlist.Add(modifier);
        }

        var newStatValue = _mappingTypeToPipeline[type].Execute(_player.Stats.GetBaseValue(type), _player.Stats, modlist);
        UnityEngine.Debug.Log(modifier.StatType.ToString() + " " + newStatValue.ToString());
        _player.Stats.ModifyStat(type, newStatValue);
    }

    private void SetupHealthPipeline()
    {
        var pipeline = new StatPipeline();
        pipeline.AddStep(new FlatModifiersStep())
                .AddStep(new MultiplerModifiersStep());
        _mappingTypeToPipeline.Add(StatType.MaxHealth, pipeline);
    }

    private void SetupStrengthPipeline()
    {
        var pipeline = new StatPipeline();
        pipeline.AddStep(new FlatModifiersStep())
                .AddStep(new MultiplerModifiersStep());
        _mappingTypeToPipeline.Add(StatType.Strength, pipeline);
    }

    private void SetupCritPipeline()
    {
        var pipeline = new StatPipeline();
        pipeline.AddStep(new FlatModifiersStep())
                .AddStep(new MultiplerModifiersStep());
        _mappingTypeToPipeline.Add(StatType.Crit, pipeline);
    }

    private void SetupCritChancePipeline()
    {
        var pipeline = new StatPipeline();
        pipeline.AddStep(new FlatModifiersStep())
                .AddStep(new MultiplerModifiersStep());
        _mappingTypeToPipeline.Add(StatType.CritChance, pipeline);
    }
}