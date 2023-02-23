using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScript : MonoBehaviour
{
    public bool isBoss;
    public int roomIndex;
    public float roomDifficulty;
    public int distToCentre;
    public EnemySpawnPosition spawnPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            EventManager.TriggerEvent(Event.RoomEnter, new EnemySpawnPacket
            {
                roomCentre = transform.position,
                isBoss = isBoss,
                roomIndex = roomIndex,
                difficulty = roomDifficulty,
                enemyPositions = spawnPosition
            });
            EventManager.TriggerEvent(Event.ChestSpawn, new ChestSpawnPacket
            {
                roomCentre = transform.position,
                //chestPos = transform.position,
                roomIndex = roomIndex,
                difficulty = roomDifficulty,
                canSpawnChest = true
            });
        }
    }

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if(collision.gameObject.tag == "Player")
    //    {
    //        EventManager.TriggerEvent(Event.RoomExit,
    //            new RoomExitPacket
    //            {
    //                roomIndex = roomIndex
    //            }
    //            );
    //    }
    //}
}
