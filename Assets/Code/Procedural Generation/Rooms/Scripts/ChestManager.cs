//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class ChestManager : MonoBehaviour
{
    [SerializeField] private GameObject chestPrefab;
    private Dictionary<int, List<GameObject>> spawnedChests;
    private List<int> hasSpawnedChest;
    [SerializeField]
    private List<ChestBase> chests;
    void Start()
    {
        //EventManager.StartListening(Event.RoomEnter, OnRoomEnter);
        EventManager.StartListening(Event.ChestSpawn, OnRoomEnter);
        //EventManager.StartListening(Event.RoomExit, OnRoomExit);

        spawnedChests = new Dictionary<int, List<GameObject>>();
        hasSpawnedChest = new List<int>();
    }

    private void OnDestroy()
    {
        //EventManager.StopListening(Event.RoomEnter, OnRoomEnter);
        EventManager.StopListening(Event.ChestSpawn, OnRoomEnter);
        //EventManager.StopListening(Event.RoomExit, OnRoomExit);

    }

    //SPAWN A CHEST UPON ENTERING A ROOM, GIVEN PROBABILITY BASED OFF ROOM DIFFICULTY
    private void OnRoomEnter(IEventPacket packet)
    {
        ChestSpawnPacket csp = packet as ChestSpawnPacket;
        if (hasSpawnedChest.Contains(csp.roomIndex))
        {
            ActivateChests(csp.roomIndex);
            Debug.Log("Chest activated");

        }
        else
        {
            //Add current room index to list of visited rooms, then spawn the chest
            hasSpawnedChest.Add(csp.roomIndex);
            SpawnChestInRoom(csp);



        }

    }
    private void SpawnChestInRoom(ChestSpawnPacket csp)
    {
        Vector3 pos = Vector2.zero;
        //For now spawn in room center
        pos.x = csp.roomCentre.x;
        pos.y = csp.roomCentre.y;
        //Instantiate new game object of chest prefab
        var go = Instantiate(chestPrefab, pos, Quaternion.identity);

        //Set index by getting component of script attached to chest prefab
        ChestControl cc = go.GetComponent<ChestControl>();
        cc.roomIndex = csp.roomIndex;
        //AddChestToDictionary(go, csp.roomIndex);
        Debug.Log("Spawned chest");
        Debug.Log("dic data " + spawnedChests.ContainsKey(csp.roomIndex));
    }
    private void OnRoomExit(IEventPacket packet)
    {
        RoomExitPacket rep = packet as RoomExitPacket;
        DisableChests(rep.roomIndex);
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
