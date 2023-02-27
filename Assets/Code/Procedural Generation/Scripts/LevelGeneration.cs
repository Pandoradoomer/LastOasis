using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

//reference: https://www.youtube.com/watch?v=nADIYwgKHv4
public class LevelGeneration : MonoBehaviour
{
    Vector2 worldSize = new Vector2(4, 4);

    Room[,] rooms;

    List<Vector2> takenPositions = new List<Vector2>();

    int gridSizeX, gridSizeY;
    int numberOfRooms = 20;
    /// <summary>
    /// If the range has bigger limits (i.e. [0.7,0.8]), the rooms will aim to clump together and form more homogenous dungeons
    /// If the range has smaller limits (i.e. [0.1,0.2], the rooms will aim to have as few neighbours as possible and branch out
    /// A large range (i.e. [0.1, 0.8] -> 0.7, as opposed to [0.4,0.5] -> 0.1, will yield a more random dungeon
    /// 
    /// Keep the numbers between 0 and 1 (non inclusive)
    /// </summary>
    [SerializeField]
    float randomCompareStart = 0.2f, randomCompareEnd = 0.01f; 

    public GameObject roomPrefab;
    private GameObject bossRoom;
    int bossRoomIndex = -1;
    private  GameObject startRoom;
    float roomSize = 11.0f;

    [SerializeField]
    private Vector2 easyDifficultyRange;
    [SerializeField]
    private Vector2 mediumDifficultyRange;
    [SerializeField]
    private Vector2 highDifficultyRange;
    [SerializeField]
    List<EnemySpawnPosition> enemySpawnPositions;

    int startRoomX, startRoomY;

    // Start is called before the first frame update
    void Awake()
    {
        if(numberOfRooms >= (worldSize.x * 2) * (worldSize.y * 2))
        {
            numberOfRooms = Mathf.RoundToInt((worldSize.x * 2) * (worldSize.y * 2));
        }
        gridSizeX = Mathf.RoundToInt(worldSize.x);
        gridSizeY = Mathf.RoundToInt(worldSize.y);
        CreateRooms();
        SetRoomDoors();
        SetStartRoom();
        CalculateDistanceFromStart();
        DrawMap();
        CreateBossRoom();
        SetRoomVariables();
        SetPlayerAtStart();
        EventManager.StartListening(Event.RoomExit, OnRoomExit);
    }

    void CreateRooms()
    {
        rooms = new Room[gridSizeX *2, gridSizeY *2];
        rooms[gridSizeX, gridSizeY] = new Room(Vector2.zero, 1);
        rooms[gridSizeX, gridSizeY].x = gridSizeX;
        rooms[gridSizeX, gridSizeY].y = gridSizeY;
        takenPositions.Add(Vector2.zero);
        Vector2 checkPos = Vector2.zero;

        float randomCompare;
        //add rooms
        for(int i = 0; i < numberOfRooms - 1; i++)
        {
            float randomPerc = ((float)i) / (((float)numberOfRooms - 1));
            randomCompare = Mathf.Lerp(randomCompareStart, randomCompareEnd, randomPerc);
            //grab new position
            checkPos = NewPosition();
            //test new position
            if(NumberOfNeighbours(checkPos, takenPositions) > 1 && Random.value > randomCompare)
            {
                int iterations = 0;
                do
                {
                    checkPos = SelectiveNewPosition();
                    iterations++;
                }
                while (NumberOfNeighbours(checkPos, takenPositions) > 1 && iterations < 100);
                if(iterations >= 50)
                {
                    Debug.LogError("Error: could not create with fewer neighbours than:" + NumberOfNeighbours(checkPos, takenPositions));
                }
            }

            rooms[(int)checkPos.x + gridSizeX, (int)checkPos.y + gridSizeY] = new Room(checkPos, 0);
            rooms[(int)checkPos.x + gridSizeX, (int)checkPos.y + gridSizeY].x = (int)checkPos.x + gridSizeX;
            rooms[(int)checkPos.x + gridSizeX, (int)checkPos.y + gridSizeY].y = (int)checkPos.y + gridSizeY;

            takenPositions.Insert(0, checkPos);
        }
    }

    Vector2 NewPosition()
    {
        int x = 0, y = 0;
        Vector2 checkingPos = Vector2.zero;
        //at random, returns a free neighbour of a room that's been added
        do
        {
            int index = Mathf.RoundToInt(Random.value * (takenPositions.Count - 1));
            x = (int)takenPositions[index].x;
            y = (int)takenPositions[index].y;
            //equal chance to go both directions
            bool upDown = (Random.value < 0.5f);
            bool positive = (Random.value < 0.5f);

            if(upDown)
            {
                if (positive)
                    y++;
                else
                    y--;
            }
            else
            {
                if (positive)
                    x++;
                else
                    x--;
            }
            checkingPos = new Vector2(x, y);
        }while(takenPositions.Contains(checkingPos) || x >= gridSizeX || x < -gridSizeX || y >= gridSizeY || y < -gridSizeY);
        return checkingPos;
    }

    Vector2 SelectiveNewPosition()
    {
        int index = 0, inc = 0;
        int x = 0, y = 0;
        //seek to add cells to rooms with only one neighbour (i.e. extend branches), but don't try for too many times if you can't
        Vector2 checkingPos = Vector2.zero;
        do
        {
            inc = 0;
            do
            {
                index = Mathf.RoundToInt(Random.value * (takenPositions.Count - 1));
                inc++;
            } while (NumberOfNeighbours(takenPositions[index], takenPositions) > 1 && inc < 100);
            x = (int)takenPositions[index].x;
            y = (int)takenPositions[index].y;
            bool upDown = (Random.value < 0.5f);
            bool positive = (Random.value < 0.5f);

            if (upDown)
            {
                if (positive)
                    y++;
                else
                    y--;
            }
            else
            {
                if (positive)
                    x++;
                else
                    x--;
            }
            checkingPos = new Vector2(x, y);
        } while (takenPositions.Contains(checkingPos) || x >= gridSizeX || x < -gridSizeX || y >= gridSizeY || y < -gridSizeY);
        if(inc >= 100)
        {
            Debug.LogError("Error: could not find position with only one neighbour");
        }
        return checkingPos;
    }
    int NumberOfNeighbours(Vector2 checkingPos, List<Vector2> takenPositions)
    {
        int ret = 0;
        if (takenPositions.Contains(checkingPos + Vector2.right))
            ret++;
        if (takenPositions.Contains(checkingPos + Vector2.left))
            ret++;
        if (takenPositions.Contains(checkingPos + Vector2.up))
            ret++;
        if (takenPositions.Contains(checkingPos + Vector2.down))
            ret++;
        return ret;
    }

   
    void SetRoomDoors()
    {
        //goes through every room and checks if neighbouring cells are set to null
        //implementation 'quirks'
        //1. x goes left to right, y goes down to up. y+1 is **above**, x+1 is **right**
        //2. 'openings' or 'doors' are stored as one integer, where we query the bits to see if there's a door there; the bits are written above the checks

        for(int x = 0; x < gridSizeX * 2; x++)
        {
            for(int y = 0; y < gridSizeY * 2; y++)
            {
                if (rooms[x, y] == null)
                    continue;

                Vector2 gridPosition = new Vector2(x, y);
                rooms[x, y].doors = 0;
                //bit 0 - UP, bit 1 - RIGHT, bit 2 - DOWN, bit 3 - LEFT
                if(y - 1 >= 0)
                {
                    if (rooms[x, y - 1] != null)
                        rooms[x, y].doors |= (1 << 2);
                }
                if(y + 1 < gridSizeY * 2)
                {
                    if (rooms[x, y + 1] != null)
                        rooms[x, y].doors |= (1 << 0);
                }
                if (x - 1 >= 0)
                {
                    if (rooms[x - 1, y] != null)
                        rooms[x, y].doors |= (1 << 3);
                }
                if (x + 1 < gridSizeX * 2)
                {
                    if (rooms[x + 1, y] != null)
                        rooms[x, y].doors |= (1 << 1);
                }

            }
        }
    }

    void SetStartRoom()
    {
        for(int x = 0; x < gridSizeX * 2; x++)
        {
            for(int y = 0; y < gridSizeY * 2; y++)
            {
                if (rooms[x,y] != null)
                {
                    startRoomX = x;
                    startRoomY = y;
                    return;
                }
            }
        }
    }

    void CalculateDistanceFromStart()
    {
        rooms[startRoomX, startRoomY].distToStart = 0;
        Queue<Room> roomQueue = new Queue<Room>();
        roomQueue.Enqueue(rooms[startRoomX, startRoomY]);
        while(roomQueue.Count > 0)
        {
            Room room = roomQueue.Dequeue();
            List<Room> neighbours = GetCellNeighbours(room.x, room.y);
            foreach(Room neighbor in neighbours)
            {
                if(neighbor.distToStart == -1)
                {
                    List<Room> neighbourNeighbours = GetCellNeighbours(neighbor.x, neighbor.y);
                    int minDist = 9999;
                    foreach(Room nb in neighbourNeighbours)
                    {
                        if (nb.distToStart != -1 && nb.distToStart < minDist)
                            minDist = nb.distToStart;
                    }
                    neighbor.distToStart = minDist + 1;
                    roomQueue.Enqueue(neighbor);
                }
            }
        }
    }

    List<Room> GetCellNeighbours(int i, int j)
    {
        List<Room> neighbours = new List<Room>();
        if(i - 1 >= 0)
        {
            if (rooms[i - 1, j] != null)
                neighbours.Add(rooms[i - 1, j]);
        }
        if(j-1 >=0)
        {
            if (rooms[i, j - 1] != null)
                neighbours.Add(rooms[i, j - 1]);
        }
        if(i+1 < gridSizeX * 2)
        {
            if (rooms[i + 1, j] != null)
                neighbours.Add(rooms[i + 1, j]);
        }
        if(j + 1 < gridSizeY * 2)
        {
            if (rooms[i, j + 1] != null)
                neighbours.Add(rooms[i, j + 1]);
        }
        return neighbours;
    }

    //exists only so we can reset the map on spacebar
    List<SpawnedRoomData> spawnedRooms = new List<SpawnedRoomData>();
    void DrawMap()
    {
        for(int x = 0; x < gridSizeX * 2; x++)
        {
            for(int y = 0; y < gridSizeY * 2; y++)
            {
                Room room = rooms[x, y];
                if (room == null)
                    continue;
                Vector2 drawPos = room.gridPos * roomSize;

                var go = Instantiate(roomPrefab, drawPos, Quaternion.identity);
                go.name = $"Room {spawnedRooms.Count}";
                rooms[x, y].go = go;
                //RoomScript rs = go.GetComponent<RoomScript>();
                //rs.roomIndex = spawnedRooms.Count;
                //rs.isBoss = rs.roomIndex == bossRoomIndex;
                //rs.distToStart = room.distToStart;
                //float difficulty = GenerateDifficulty(rs.distToStart);
                //rs.roomDifficulty = difficulty;
                //rs.spawnPosition = enemySpawnPositions[Random.Range(0, 2)];
                //EventManager.TriggerEvent(Event.RoomSpawn, new RoomSpawnPacket()
                //{
                //    go = go,
                //    doors = room.doors
                //});
                spawnedRooms.Add(new SpawnedRoomData(go, x, y));
                //DoorManager dm = go.GetComponent<DoorManager>();
                //dm.doorsBits = room.doors;
                //dm.x = x;
                //dm.y = y;
                //dm.ReinitialiseDoors(room.doors);
            }
        }
    }

    void SetRoomVariables()
    {
        foreach(var room in spawnedRooms)
        {
            RoomScript rs = room.go.GetComponent<RoomScript>();
            rs.roomIndex = spawnedRooms.IndexOf(room);
            rs.isBoss = rs.roomIndex == bossRoomIndex;
            rs.isStart = spawnedRooms.IndexOf(room) == 0;
            rs.distToStart = rooms[room.x, room.y].distToStart;
            rs.roomDifficulty = GenerateDifficulty(rs.distToStart);
            rs.spawnPosition = enemySpawnPositions[Random.Range(0, 2)];
            EventManager.TriggerEvent(Event.RoomSpawn, new RoomSpawnPacket()
            {
                go = room.go,
                doors = rooms[room.x, room.y].doors
            });
            DoorManager dm = room.go.GetComponent<DoorManager>();
            dm.doorsBits = rooms[room.x,room.y].doors;
            dm.x = room.x;
            dm.y = room.y;
            dm.ReinitialiseDoors(rooms[room.x, room.y].doors);
            room.go.SetActive(false);
        }
    }

    void SetPlayerAtStart()
    {
        Singleton.Instance.PlayerController.transform.position = rooms[startRoomX, startRoomY].gridPos * roomSize;
        rooms[startRoomX, startRoomY].go.SetActive(true);
        rooms[startRoomX, startRoomY].go.name = "StartRoom";
    }

    public int GetNeighbourOfRoom(int roomIndex, Direction dir)
    {
        int x = spawnedRooms[roomIndex].x;
        int y = spawnedRooms[roomIndex].y;

        switch(dir)
        {
            case Direction.N:
                y++; break;
            case Direction.S:
                y--; break;
            case Direction.E:
                x++; break;
            case Direction.W:
                x--; break;
        }

        return rooms[x, y].go.GetComponent<RoomScript>().roomIndex;
    }

    void OnRoomExit(IEventPacket packet)
    {
        RoomExitPacket rep = packet as RoomExitPacket;
        SpawnedRoomData currentRoom = spawnedRooms[rep.roomIndex];
        SpawnedRoomData nextRoom = spawnedRooms[rep.nextRoomIndex];

        rooms[currentRoom.x, currentRoom.y].go.SetActive(false);
        rooms[nextRoom.x, nextRoom.y].go.SetActive(true);
        Vector2 newPlayerPos = rooms[nextRoom.x, nextRoom.y].go.transform.position;
        float mult = (roomSize - 3.0f) / 2.0f;
        switch (rep.direction)
        {
            case(Direction.N):
                newPlayerPos += Vector2.down * mult; break;
            case (Direction.S):
                newPlayerPos += Vector2.up * mult; break;
            case (Direction.W):
                newPlayerPos += Vector2.right * mult; break;
            case (Direction.E):
                newPlayerPos += Vector2.left * mult; break;
        }
        Singleton.Instance.PlayerController.transform.position = newPlayerPos;
    }

    private float GenerateDifficulty(int distance)
    {
        float r;
        if(distance < 2)
        {
            r = Random.Range(easyDifficultyRange.x, easyDifficultyRange.y);
        }
        else if (distance < 4) 
        {
            r = Random.Range(mediumDifficultyRange.x, mediumDifficultyRange.y);
        }
        else
        {
            r = Random.Range(highDifficultyRange.x, highDifficultyRange.y);
        }

        return Mathf.Floor(r * 2 + 0.5f) / 2;
    }

    /// <summary>
    /// Calculate the distance between the spawn room which is positioned at V2.Zero.
    /// Get the furthest room away from the spawn room to make it a boss room.
    /// Need to expand onto the code to make sure the correct room is chosen.
    /// </summary>

    private void CreateBossRoom()
    {
        int largestDistance = 0;
        int largestDistanceIndex = 0;
        for(int i = 0; i < spawnedRooms.Count; i++)
        {
            var srd = spawnedRooms[i];
            Room room = rooms[srd.x, srd.y];
            //RoomScript rs = spawnedRooms[i].go.GetComponent<RoomScript>();
            if (room.distToStart > largestDistance)
            {
                largestDistance = room.distToStart;
                largestDistanceIndex = i;
            }

        }
        bossRoomIndex = largestDistanceIndex;
        bossRoom = spawnedRooms[largestDistanceIndex].go;
        bossRoom.GetComponent<RoomScript>().isBoss = true;
        bossRoom.name = "BossRoom";

    }

    void TeleportToBossRoom()
    {
        EventManager.TriggerEvent(Event.BossTeleport,
            new BossTeleportPacket
            {
                transform = bossRoom.transform
            });
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            BossManager.instance.bossHPSlider.SetActive(true);
            //foreach (GameObject go in spawnedRooms)
            //    Destroy(go);
            spawnedRooms.Clear();
            takenPositions.Clear();
            CreateRooms(); 
            SetRoomDoors();
            DrawMap();
            CreateBossRoom();
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
            TeleportToBossRoom();

    }
}


public class SpawnedRoomData
{
    //the actual gameobject with all the data
    public GameObject go;
    //the grid coordinates
    public int x, y;

    public SpawnedRoomData(GameObject go, int x, int y)
    {
        this.go = go;
        this.x = x;
        this.y = y;
    }
}
