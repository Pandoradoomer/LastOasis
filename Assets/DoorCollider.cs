using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    N,
    E,
    S,
    W
}
public class DoorCollider : MonoBehaviour
{
    public Direction dir;
    [SerializeField]
    RoomScript room;
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
        if(collision.gameObject.tag == "Player")
        {
            EventManager.TriggerEvent(Event.RoomExit, new RoomExitPacket()
            {
                roomIndex = room.roomIndex,
                nextRoomIndex = Singleton.Instance.LevelGeneration.GetNeighbourOfRoom(room.roomIndex, dir),
                direction = dir
            });
        }
    }
}
