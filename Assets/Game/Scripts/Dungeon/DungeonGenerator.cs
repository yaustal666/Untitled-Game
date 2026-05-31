using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DungeonGenerator
{
    #region Setup
    public float roomChance = 0.6f;
    public int maxRoomCount = 15;

    private int _width;
    private int _height;
    private int[,] _grid;
    private int roomsCount = 0;
    private Vector2Int _startPoint;

    public DungeonGenerator(int size, Vector2Int startPoint)
    {
        _width = size;
        _height = size;
        _startPoint = startPoint;

        _grid = new int[_width, _height];
        _grid.Clear();
    }

    public DungeonGenerator(int height, int width, Vector2Int startPoint)
    {
        _width = width;
        _height = height;
        _startPoint = startPoint;

        _grid = new int[_width, _height];
        _grid.Clear();
    }
    #endregion

    public void Generate()
    {
        _grid.Clear();
        _grid.SetCell(_startPoint, 1);

        var roomsToProcess = new Queue<Vector2Int>();
        roomsToProcess.Enqueue(_startPoint);

        while (roomsToProcess.Count > 0)
        {
            if (roomsCount >= maxRoomCount) break;

            var currentRoom = roomsToProcess.Dequeue();

            foreach (var direction in GameConstants.roomTraverseOrder)
            {
                var neighborPos = currentRoom + direction;

                int neighborVal = _grid.GetCell(neighborPos);
                if (neighborVal == 1 || neighborVal == -1) continue;

                bool shouldPlace = ShouldPlaceRoom(neighborPos);
                if (shouldPlace)
                {
                    _grid.SetCell(neighborPos, 1);
                    roomsToProcess.Enqueue(neighborPos);
                    roomsCount++;
                }
            }
        }
    }
    public void AddRoom(Vector2Int newRoomIndex)
    {
        if (!_grid.IsInside(newRoomIndex)) return;

        if (_grid.GetCell(newRoomIndex) == 1) return;

        if (_grid.IsEmpty())
        {
            _grid.SetCell(newRoomIndex, 1);
            return;
        }

        var closestRoomIndex = FindClosestRoomManhattan(newRoomIndex);

        if (closestRoomIndex.x == -1)
        {
            Debug.LogWarning("Could not find any existing room to connect to!");
            return;
        }

        CarveManhattanPath(closestRoomIndex, newRoomIndex);

        _grid.SetCell(newRoomIndex, 1);
    }

    private bool ShouldPlaceRoom(Vector2Int index)
    {
        int existingNeighborCount = CountExistingNeighbors(index);

        if (existingNeighborCount >= 2)
        {
            return Random.value < 0.1f;
        }

        return Random.value < roomChance;
    }

    private Vector2Int FindClosestRoomManhattan(Vector2Int roomIndex)
    {
        int maxDistance = Mathf.Max(_width, _height);

        for (int distance = 1; distance <= maxDistance; distance++)
        {
            for (int dx = -distance; dx <= distance; dx++)
            {
                int dy = distance - Mathf.Abs(dx);

                var checkIndex = new Vector2Int(roomIndex.x + dx, roomIndex.y + dy);
                if (_grid.GetCell(checkIndex) == 1) return checkIndex;


                checkIndex = new Vector2Int(roomIndex.x + dx, roomIndex.y - dy);
                if (_grid.GetCell(checkIndex) == 1) return checkIndex;
            }
        }

        return new Vector2Int(-1, -1);
    }

    private void CarveManhattanPath(Vector2Int start, Vector2Int end)
    {
        bool horizontalFirst = Random.value < 0.5f;

        if (horizontalFirst)
        {
            var currentPosition = CarveHorizontal(start, end);
            CarveVertical(currentPosition, end);
        }
        else
        {
            var currentPosition = CarveVertical(start, end);
            CarveHorizontal(currentPosition, end);
        }
    }

    private Vector2Int CarveHorizontal(Vector2Int start, Vector2Int end)
    {
        int step = Math.Sign(end.x - start.x);

        while (start.x != end.x)
        {
            start.x += step;
            if (_grid.GetCell(start) == 0)
            {
                _grid.SetCell(start, 1);
            }
        }

        return start;
    }

    private Vector2Int CarveVertical(Vector2Int start, Vector2Int end)
    {
        int step = Math.Sign(end.y - start.y);

        while (start.y != end.y)
        {
            start.y += step;
            if (_grid.GetCell(start) == 0)
            {
                _grid.SetCell(start, 1);
            }
        }

        return start;
    }

    private int CountExistingNeighbors(Vector2Int index)
    {
        int count = 0;
        foreach (var dir in GameConstants.roomTraverseOrder)
        {
            var neighbor = index + dir;
            if (_grid.GetCell(neighbor) == 1) count++;
        }

        return count;
    }

    public int GetCell(Vector2Int index)
    {
        return _grid.GetCell(index);
    }

    public void PrintDungeon()
    {
        string output = "";
        output += roomsCount + "\n";
        for (int x = 0; x < _height; x++)
        {
            for (int y = 0; y < _width; y++)
            {
                output += _grid[x, y] + "\t";
            }
            output += "\n";
        }
        Debug.Log(output);
    }
}