using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestManager : MonoBehaviour
{
    [SerializeField] private GameObject chestPrefab;
    private Dictionary<int, List<GameObject>> spawnedChests;
    private List<int> hasSpawnedChest;
    void Start()
    {
       // EventManager.Instance.ChestSpawn += SpawnChestInRoom;
        EventManager.Instance.ChestSpawn += OnRoomEnter;
        spawnedChests = new Dictionary<int, List<GameObject>>();
        hasSpawnedChest = new List<int>();
    }

    //SPAWN A CHEST UPON ENTERING A ROOM, GIVEN PROBABILITY BASED OFF ROOM DIFFICULTY
    void Update()
    {
        
    }

    private void OnRoomEnter(ChestSpawnPacket o)
    {
        if (hasSpawnedChest.Contains(o.roomIndex))
        {
            ActivateChests(o.roomIndex);
        }
        else
        {
            hasSpawnedChest.Add(o.roomIndex);
            SpawnChestInRoom(o);
        }
    }
    private void SpawnChestInRoom(ChestSpawnPacket o)
    {
        Vector2 pos = Vector2.zero;
        //For now spawn in room center
        pos.x = o.roomCenter.x;
        pos.y = o.roomCenter.y;

        var go = Instantiate(chestPrefab, o.roomCenter + pos, Quaternion.identity);
        AddChestToDictionary(go, o.roomIndex);

    }

    private void ActivateChests(int index)
    {
        if (spawnedChests.ContainsKey(index) == false)
            return;
    }

    private void DisableChests(int index)
    {
        if (spawnedChests.ContainsKey(index) == false)
            return;
    }
    void AddChestToDictionary(GameObject go, int index)
    {
        if (spawnedChests.ContainsKey(index))
        {
            spawnedChests[index].Add(go);
        }
        else
        {
            spawnedChests.Add(index, new List<GameObject>());
            spawnedChests[index].Add(go);
        }
    }
}
