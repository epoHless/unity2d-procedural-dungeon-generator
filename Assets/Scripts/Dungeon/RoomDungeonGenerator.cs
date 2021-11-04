using System;
using Pixeye.Unity;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class RoomDungeonGenerator : SimpleDungeonGenerator
{
    [Foldout("Dungeon Settings", true)]
    [SerializeField] int minRoomWidth = 4;
    [SerializeField] int minRoomHeight = 4;

    [SerializeField] int dungeonWidth = 20;
    [SerializeField] int dungeonHeight = 20;
    [SerializeField] [Range(0, 10)] int offset = 1;
    [SerializeField] bool randomWalkRooms = false;
    [SerializeField] private int maxFloors = 3;
    [SerializeField] private int currentFloor = 0;

    [Foldout("Spawn Point and End Point", true)]
    [SerializeField] GameObject player;
    [SerializeField] GameObject endPoint;

    [Space]
    [Header("Dungeon Events")]
    [field: SerializeField] private UnityEvent onDungeonCreate;
    [field: SerializeField] private UnityEvent onEndPointReach;
    [field: SerializeField] private UnityEvent onDungeonEnd;

    SpawnObjectsOnTilemap spawnSystem;

    public static RoomDungeonGenerator instance;

    private void Awake()
    {
        instance = this;
        spawnSystem = GetComponent<SpawnObjectsOnTilemap>();
    }

    protected override void RunProceduralGeneration()
    {
        CreateRooms();
        spawnSystem = GetComponent<SpawnObjectsOnTilemap>();

        spawnSystem.FindLocationsOfTilesToSpawnEnemies();
    }

    private void Start()
    {
        tilemapVisualizer.Clear();
        CreateRooms();
    }

    public void CreateRooms()
    {
        currentFloor++;
        if (currentFloor == maxFloors)
        {
            onDungeonEnd?.Invoke();
            Debug.Log("Dungeon Completed");
        }
        
        var roomsList = ProceduralGenerationAlgorithms.BinarySpacePartitioning(new BoundsInt((Vector3Int)startPosition, new Vector3Int(dungeonWidth, dungeonHeight, 0)), minRoomWidth, minRoomHeight);

        var floor = new HashSet<Vector2Int>();

        if (randomWalkRooms)
        {
            floor = CreateRoomsRandomly(roomsList);
        }
        else
        {
            floor = CreateSimpleRooms(roomsList);
        }

        var roomCenters = roomsList.Select(room => (Vector2Int) Vector3Int.RoundToInt(room.center)).ToList();

        GenerateSpawnNEndPoints(roomCenters);

        tilemapVisualizer.PaintFloorTilesRandomly(floor);
        var corridors = ConnectRooms(roomCenters);
        floor.UnionWith(corridors);
        tilemapVisualizer.PaintCorridorTiles(corridors);

        WallGenerator.CreateWalls(floor, tilemapVisualizer);
        
        onDungeonCreate?.Invoke();
    }

    /// <summary>
    /// Choose a random point to place the player and a second random point to place the endpoint
    /// </summary>
    /// <param name="roomCenters"></param>
    public void GenerateSpawnNEndPoints(List<Vector2Int> roomCenters)
    {
        var randomSpawnPoint = Random.Range(0, roomCenters.Count);
        player.transform.position = new Vector3Int(roomCenters[randomSpawnPoint].x, roomCenters[randomSpawnPoint].y, (int)transform.position.z);

        var randomEndPoint = Random.Range(0, roomCenters.Count);

        if (randomSpawnPoint == randomEndPoint)
        {
            while (randomEndPoint == randomSpawnPoint)
            {
                randomEndPoint = Random.Range(0, roomCenters.Count);
            }
        }

        endPoint.transform.position = new Vector3Int(roomCenters[randomEndPoint].x, roomCenters[randomEndPoint].y, (int)transform.position.z);
    }

    private HashSet<Vector2Int> CreateRoomsRandomly(List<BoundsInt> roomsList)
    {
        var floor = new HashSet<Vector2Int>();

        for (int i = 0; i < roomsList.Count; i++)
        {
            var roomBounds = roomsList[i];
            var roomCenter = new Vector2Int(Mathf.RoundToInt(roomBounds.center.x), Mathf.RoundToInt(roomBounds.center.y));
            var roomFloor = RunRandomWalk(randomWalkParameters, roomCenter);
            foreach (var position in roomFloor)
            {
                if (position.x >= (roomBounds.xMin + offset) && position.x <= (roomBounds.xMax - offset) && position.y >= (roomBounds.yMin - offset) && position.y <= (roomBounds.yMax - offset))
                {
                    floor.Add(position);
                }
            }
        }
        return floor;
    }

    private HashSet<Vector2Int> ConnectRooms(List<Vector2Int> roomCenters)
    {
        var corridors = new HashSet<Vector2Int>();
        var currentRoomCenter = roomCenters[Random.Range(0, roomCenters.Count)];
        roomCenters.Remove(currentRoomCenter);

        while (roomCenters.Count > 0)
        {
            Vector2Int closest = FindClosestPointTo(currentRoomCenter, roomCenters);
            roomCenters.Remove(closest);
            HashSet<Vector2Int> newCorridor = CreateCorridor(currentRoomCenter, closest);
            currentRoomCenter = closest;
            corridors.UnionWith(newCorridor);
        }
        return corridors;
    }

    private HashSet<Vector2Int> CreateCorridor(Vector2Int currentRoomCenter, Vector2Int destination)
    {
        var corridor = new HashSet<Vector2Int>();
        var position = currentRoomCenter;
        corridor.Add(position);
        while (position.y != destination.y)
        {
            if (destination.y > position.y)
            {
                position += Vector2Int.up;
            }
            else if (destination.y < position.y)
            {
                position += Vector2Int.down;
            }
            corridor.Add(position);
        }
        while (position.x != destination.x)
        {
            if (destination.x > position.x)
            {
                position += Vector2Int.right;
            }
            else if (destination.x < position.x)
            {
                position += Vector2Int.left;
            }
            corridor.Add(position);
        }
        return corridor;
    }

    private Vector2Int FindClosestPointTo(Vector2Int currentRoomCenter, List<Vector2Int> roomCenters)
    {
        var closest = Vector2Int.zero;
        float distance = float.MaxValue;
        foreach (var position in roomCenters)
        {
            float currentDistance = Vector2.Distance(position, currentRoomCenter);
            if (currentDistance < distance)
            {
                distance = currentDistance;
                closest = position;
            }
        }
        return closest;
    }

    private HashSet<Vector2Int> CreateSimpleRooms(List<BoundsInt> roomsList)
    {
        var floor = new HashSet<Vector2Int>();
        foreach (var room in roomsList)
        {
            for (int col = offset; col < room.size.x - offset; col++)
            {
                for (int row = offset; row < room.size.y - offset; row++)
                {
                    Vector2Int position = (Vector2Int)room.min + new Vector2Int(col, row);
                    floor.Add(position);
                }
            }
        }
        return floor;
    }

    public void ResetTiles()
    {
        tilemapVisualizer.Clear();
    }

    #region Event Functions

    public void InvokeOnEndPointReachEvent()
    {
        onEndPointReach?.Invoke();
    }
    
    public void InvokeOnDungeonCreateEvent()
    {
        onDungeonCreate?.Invoke();
    }
    
    public void InvokeOnDungeonCreateEnd()
    {
        onDungeonEnd?.Invoke();
    }

    #endregion
}
