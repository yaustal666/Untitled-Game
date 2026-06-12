using Reflex.Attributes;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;

public class Dungeon : MonoBehaviour
{
    public bool Ready { get; private set; } = false;

    [Inject] private Player _player;
    public Transform PlayerTransform;

    public CinemachineConfiner2D _confiner;

    [SerializeField] private PrefabCollection _roomsLibrary;
    private DungeonGenerator gen;
    private Dictionary<Vector2Int, DungeonRoom> _mappingGridToRoom = new Dictionary<Vector2Int, DungeonRoom>();

    private int roomVerticalDistance = 100;
    private int roomHorizontalDistance = 100;

    private int dungeonSize = 7;
    private Vector2Int startRoomIndex = new(0, 3);
    private Vector2Int bossRoomIndex = new(6, 0);
    private Vector2Int townRoomIndex = new(3, 6);
    private Vector2Int generationStartCellIndex = new(0, 3);

    private Vector2Int currentRoomIndex;

    private void Awake()
    {
        startRoomIndex = new Vector2Int(0, Random.Range(1, 5));
        bossRoomIndex = new Vector2Int(6, Random.Range(0, 6));
        townRoomIndex = new Vector2Int(Random.Range(1, 5), 6);
    }

    private void Start()
    {
        gen = new DungeonGenerator(dungeonSize, generationStartCellIndex);

        gen.Generate();
        gen.AddRoom(startRoomIndex);
        gen.AddRoom(townRoomIndex);
        gen.AddRoom(bossRoomIndex);

        gen.PrintDungeon();

        currentRoomIndex = startRoomIndex;
        SpawnRooms();
        _confiner.BoundingShape2D = _mappingGridToRoom[startRoomIndex].CameraBoundary;
        Ready = true;
    }

    private void SpawnRooms()
    {
        for (int i = 0; i < dungeonSize; i++)
        {
            for (int j = 0; j < dungeonSize; j++)
            {
                Vector2Int index = new Vector2Int(i, j);

                if (gen.GetCell(index) != 0)
                {
                    var neighbours = GetNeighborsList(index);

                    var newRoom = Instantiate(_roomsLibrary.GetRandom()).GetComponent<DungeonRoom>();

                    newRoom.transform.Translate(roomHorizontalDistance * (j - 3), -roomVerticalDistance * i, 0);
                    newRoom.InitializeDoors(neighbours);
                    newRoom.PlayerExitRoom += OnPlayerExitRoom;
                    _mappingGridToRoom.Add(index, newRoom);
                }
            }
        }
    }

    private List<int> GetNeighborsList(Vector2Int cellIndex)
    {
        List<int> neighbours = new List<int>();

        Vector2Int neighbor;
        foreach (Vector2Int direction in GameConstants.roomTraverseOrder)
        {
            neighbor = cellIndex + direction;
            int value = gen.GetCell(neighbor);

            if (value == -1 || value == 0)
            {
                neighbours.Add(0);
            }
            else
            {
                neighbours.Add(1);
            }
        }

        return neighbours;
    }

    private void OnPlayerExitRoom(DoorDirection doorDirection)
    {
        var currentRoom = _mappingGridToRoom[currentRoomIndex];
        var nextRoom = _mappingGridToRoom[currentRoomIndex + DirectionToVec(doorDirection)];

        var entryPoint = nextRoom.GetEntry(doorDirection);
        PlayerTransform.position = entryPoint.position;
        _confiner.BoundingShape2D = nextRoom.CameraBoundary;
        currentRoomIndex = currentRoomIndex + DirectionToVec(doorDirection);
        nextRoom.Enter();
    }

    public Vector2Int DirectionToVec(DoorDirection direction) => direction switch
    {
        DoorDirection.Top => new Vector2Int(-1, 0),
        DoorDirection.Right => new Vector2Int(0, 1),
        DoorDirection.Bottom => new Vector2Int(1, 0),
        DoorDirection.Left => new Vector2Int(0, -1),
        _ => new Vector2Int(0, 0)
    };

    public Vector3 GetStartRoomPosition()
    {
        return _mappingGridToRoom[startRoomIndex].transform.position;
    }
}