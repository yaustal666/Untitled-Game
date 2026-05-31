using Reflex.Extensions;
using Reflex.Injectors;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DungeonRoom : MonoBehaviour
{
    public event Action<DoorDirection> PlayerExitRoom;
    public bool isRoomCompleted;

    [SerializeField] private List<DungeonRoomDoor> _doors;
    [SerializeField] private GameObject _enemyPrefab;

    [field: SerializeField] public Collider2D CameraBoundary { get; private set; }

    private void Start()
    {
        foreach (var door in _doors)
        {
            door.PlayerLeaveRoom += OnPlayerLeaveRoom;
        }
    }

    private void OnPlayerLeaveRoom(DoorDirection doorDirection)
    {
        PlayerExitRoom?.Invoke(doorDirection);
    }

    public Transform GetEntry(DoorDirection doorDirection)
    {
        int doorId = ((int)doorDirection + 2) % 4;
        return _doors[doorId].getEntryPoint();
    }

    public void InitializeDoors(List<int> doors)
    {
        for (int i = 0; i < 4; i++)
        {
            if (doors[i] == 0)
            {
                _doors[i].Disable();
                continue;
            }

            _doors[i].Open();
        }
    }

    public void Enter()
    {
        if (isRoomCompleted)
        {
            OpenDoors();
        }
        else
        {
            CloseDoors();
            SpawnEnemies();
        }
    }

    public void OpenDoors()
    {
        foreach (var door in _doors)
        {
            door.Open();
        }
    }

    public void CloseDoors()
    {
        foreach (var door in _doors)
        {
            door.Close();
        }
    }

    public void SpawnEnemies()
    {
        if (_enemyPrefab != null)
        {
            var sceneContainer = gameObject.scene.GetSceneContainer();
            var enemyObject = Instantiate(_enemyPrefab, transform.position, Quaternion.identity, transform);
            GameObjectInjector.InjectRecursive(enemyObject, sceneContainer);
        }
    }

}