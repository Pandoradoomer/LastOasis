using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomExitPacket : IEventPacket
{
    public int roomIndex;
    public int nextRoomIndex;
}
