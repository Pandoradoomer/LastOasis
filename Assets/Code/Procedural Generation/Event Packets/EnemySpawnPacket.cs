using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPacket : IEventPacket
{
    public Vector2 roomCentre;
    public int roomIndex;
    public float difficulty;
    public bool isBoss;
    public EnemySpawnPosition enemyPositions;
}
