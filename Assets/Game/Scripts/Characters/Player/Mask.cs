using System;
using System.Collections.Generic;
using UnityEngine;

public enum MaskColor
{
    Red,
    Green,
    Purple,
    Yellow
};

public struct MaskCellValidationResult
{
    public bool IsSuccess;
    public List<Vector2Int> Conflicts;

    public MaskCellValidationResult(bool isSuccess)
    {
        IsSuccess = isSuccess;
        Conflicts = new List<Vector2Int>();
    }
}

public class Mask : ISavable
{
    private int[,] _grid;
    private Dictionary<MaskColor, int> _colorCounts;
    public Action<StatModifier> StatChanged;

    public Mask(ISaveRegistry saveRegistry)
    {
        _grid = new int[20, 20];
        _colorCounts = new Dictionary<MaskColor, int>
        {
            { MaskColor.Red, 0 },
            { MaskColor.Green, 0 },
            { MaskColor.Purple, 0 },
            { MaskColor.Yellow, 0 }
        };
        saveRegistry.Register(this);
    }

    public void Save(GameSaveData saveData)
    {
        saveData.MaskData.Grid = (int[,])_grid.Clone();
    }

    public void Load(GameSaveData saveData)
    {
        if (saveData?.MaskData?.Grid == null) return;

        _grid = (int[,])saveData.MaskData.Grid.Clone();

        foreach (MaskColor color in Enum.GetValues(typeof(MaskColor)))
        {
            _colorCounts[color] = 0;
        }

        int width = _grid.GetLength(0);
        int height = _grid.GetLength(1);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int value = _grid[x, y];
                if (value > 0)
                {
                    _colorCounts[(MaskColor)(value - 1)]++;
                }
            }
        }

        NotifyAllStats();
    }


    public bool PaintCell(Vector2Int index, MaskColor color)
    {
        MaskCellValidationResult report = EvaluatePlacement(index, color);
        if (!report.IsSuccess) return false;

        _grid.SetCell(index, (int)color + 1);
        _colorCounts[color]++;

        StatChanged?.Invoke(new StatModifier
        {
            ModifierType = StatModificationType.Flat,
            StatType = ColorToStat(color),
            Source = "Mask_" + color.ToString(),
            Value = _colorCounts[color]
        });
        return true;
    }

    public MaskCellValidationResult EvaluatePlacement(Vector2Int index, MaskColor color)
    {
        MaskCellValidationResult report = new MaskCellValidationResult(false);
        if (_colorCounts[MaskColor.Green] == 0 && color == MaskColor.Green)
        {
            report.IsSuccess = true;
            return report;
        }

        if (_grid.GetCell(index) != 0)
        {
            report.IsSuccess = false;
            return report;
        }

        _grid.SetCell(index, (int)color + 1);
        _colorCounts[color]++;

        report.Conflicts = ValidateEntireGrid();

        _grid.SetCell(index, 0);
        _colorCounts[color]--;

        report.IsSuccess = report.Conflicts.Count == 0;
        return report;
    }

    public bool ClearCell(Vector2Int index)
    {
        if (!_grid.IsInside(index)) return false;

        int previousValue = _grid.GetCell(index);
        if (previousValue == 0) return true;

        MaskColor oldColor = (MaskColor)(previousValue - 1);

        _grid.SetCell(index, 0);
        _colorCounts[oldColor]--;

        List<Vector2Int> globalConflicts = ValidateEntireGrid();

        if (globalConflicts.Count > 0)
        {
            _grid.SetCell(index, previousValue);
            _colorCounts[oldColor]++;
            return false;
        }

        StatChanged?.Invoke(new StatModifier
        {
            ModifierType = StatModificationType.Flat,
            StatType = ColorToStat(oldColor),
            Source = "Mask_" + oldColor.ToString(),
            Value = _colorCounts[oldColor]
        });
        return true;
    }

    private List<Vector2Int> ValidateEntireGrid()
    {
        List<Vector2Int> violations = new List<Vector2Int>();
        int width = _grid.GetLength(0);
        int height = _grid.GetLength(1);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int cellValue = _grid[x, y];
                if (cellValue == 0) continue;

                Vector2Int pos = new Vector2Int(x, y);
                MaskColor cellColor = (MaskColor)(cellValue - 1);

                _grid[x, y] = 0;
                bool correctPlacement = CheckRulesForCell(pos, cellColor);
                _grid[x, y] = cellValue;

                if (!correctPlacement)
                {
                    violations.Add(pos);
                }
            }
        }

        return violations;
    }

    private bool CheckRulesForCell(Vector2Int index, MaskColor color)
    {
        switch (color)
        {
            case MaskColor.Red:
                if (CountNeighborsOfColor(index, MaskColor.Red) >= 3)
                    return false;
                break;

            case MaskColor.Green:
                if (_colorCounts[MaskColor.Green] == 0)
                    return true;
                if (CountNeighborsOfColor(index, MaskColor.Green) == 0)
                    {
                    Debug.Log("Neighbors not found");
                    return false; }
                break;

            case MaskColor.Purple:
                int yellowNeighbors = CountNeighborsOfColor(index, MaskColor.Yellow);
                int redNeighbors = CountNeighborsOfColor(index, MaskColor.Red);
                if ((yellowNeighbors + redNeighbors) == 0)
                    return false;
                break;

            case MaskColor.Yellow:
                if (_colorCounts[MaskColor.Yellow] > _colorCounts[MaskColor.Red])
                    return false;
                break;
        }

        return true;
    }

    public int GetColorCount(MaskColor color)
    {
        return _colorCounts.TryGetValue(color, out int count) ? count : 0;
    }

    private int CountNeighborsOfColor(Vector2Int index, MaskColor color)
    {
        int count = 0;
        int targetValue = (int)color + 1;

        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0) continue;

                Vector2Int neighbor = new Vector2Int(index.x + dx, index.y + dy);
                if (_grid.IsInside(neighbor) && _grid.GetCell(neighbor) == targetValue)
                {
                    count++;
                }
            }
        }
        return count;
    }

    public void ClearAll()
    {
        _grid.Clear();
        foreach (MaskColor color in Enum.GetValues(typeof(MaskColor)))
        {
            _colorCounts[color] = 0;
        }
        NotifyAllStats();
    }

    private void NotifyAllStats()
    {
        foreach (MaskColor color in Enum.GetValues(typeof(MaskColor)))
        {
            //StatChanged?.Invoke(new StatModifier
            //{
            //    ModifierType = StatModificationType.Flat,
            //    StatType = ColorToStat(color),
            //    Source = "Mask_" + color.ToString(),
            //    Value = _colorCounts[color]
            //});
        }
    }

    public StatType ColorToStat(MaskColor color)
    {
        return color switch
        {
            MaskColor.Green => StatType.MaxHealth,
            MaskColor.Red => StatType.Strength,
            MaskColor.Yellow => StatType.Crit,
            MaskColor.Purple => StatType.CritChance,
            _ => throw new NotImplementedException()
        };
    }

    public void PrintDebug()
    {
        string output = "";
        foreach (var kvp in _colorCounts)
        {
            output += kvp.Key.ToString() + " " + kvp.Value.ToString() + "\n";
        }
        Debug.Log(output);

        _grid.Print();
    }
}
