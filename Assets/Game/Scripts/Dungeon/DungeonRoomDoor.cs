using System;
using UnityEngine;

public enum DoorDirection
{
    Top = 0,
    Right = 1,
    Bottom = 2,
    Left = 3
}

public class DungeonRoomDoor : MonoBehaviour
{
    public event Action<DoorDirection> PlayerLeaveRoom;

    [SerializeField] private Transform entryPoint;
    [SerializeField] private DoorDirection direction;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerLeaveRoom.Invoke(direction);
        }
    }

    public Transform getEntryPoint()
    {
        return entryPoint;
    }

    public void Open()
    {

    }

    public void Close()
    {

    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}