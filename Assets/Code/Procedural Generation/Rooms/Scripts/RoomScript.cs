using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScript : MonoBehaviour
{
    public bool isBoss;
    public bool isStart;
    public int roomIndex;
    public float roomDifficulty;
    public int distToStart;
    public EnemySpawnPosition spawnPosition;
    [SerializeField]
    private List<Destructible> destructibles;
    // Start is called before the first frame update
    void Start()
    {
        EventManager.StartListening(Event.DoorsLockUnlock, LockUnlockDoors);
    }

    private void OnDestroy()
    {
        EventManager.StopListening(Event.DoorsLockUnlock, LockUnlockDoors);
    }

    void LockUnlockDoors(IEventPacket packet)
    {
        DoorLockUnlockPacket dlup = packet as DoorLockUnlockPacket;
        if(dlup.roomIndex == roomIndex && !isStart)
        {
            DoorManager dm = GetComponent<DoorManager>();
            dm.SetAllDoors(dlup.isUnlock);
        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDestructible(float healthSpawnChance, float coinSpawnChance)
    {
        foreach(Destructible d in destructibles)
        {
            float r;
            r = Random.Range(0.0f, 1.0f);
            if(r < healthSpawnChance)
            {
                d.AddHealth();
            }
            r = Random.Range(0.0f, 1.0f);
            if(r < coinSpawnChance)
            {
                d.AddCoin(5 * (distToStart + 1));
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            EventManager.TriggerEvent(Event.RoomEnter, new EnemySpawnPacket
            {
                roomCentre = transform.position,
                isBoss = isBoss,
                isStart = isStart,
                roomIndex = roomIndex,
                difficulty = roomDifficulty,
                enemyPositions = spawnPosition
            });
            EventManager.TriggerEvent(Event.ChestSpawn, new ChestSpawnPacket
            {
                roomCentre = transform.position,
                roomIndex = roomIndex,
                difficulty = roomDifficulty,
                canSpawnChest = true
            });
        }
    }
}
