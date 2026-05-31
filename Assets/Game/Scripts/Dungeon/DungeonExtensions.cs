using UnityEngine;

public static class GridExtensions
{
    public static int GetCell(this int[,] grid, Vector2Int index)
    {
        if (grid.IsInside(index))
        {
            return grid[index.x, index.y];
        }
        return -1;
    }

    public static void SetCell(this int[,] grid, Vector2Int index, int value)
    {
        grid[index.x, index.y] = value;
    }

    public static void Clear(this int[,] grid)
    {
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                grid[i, j] = 0;
            }
        }
    }
    public static bool IsEmpty(this int[,] grid)
    {
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                if (grid[i, j] == 1)
                    return false;
            }
        }
        return true;
    }

    public static bool IsInside(this int[,] grid, Vector2Int index)
    {
        return index.x >= 0 && index.x < grid.GetLength(0) && index.y >= 0 && index.y < grid.GetLength(1);
    }
}