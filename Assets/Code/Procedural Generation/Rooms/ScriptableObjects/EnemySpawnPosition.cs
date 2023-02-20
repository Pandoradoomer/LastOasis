using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyPositionsObject", menuName = "ScriptableObjects/Rooms/Enemy Spawn Position")]
public class EnemySpawnPosition : ScriptableObject
{
    public List<Vector2> enemyPositions;
    public bool isExpandable;
}

