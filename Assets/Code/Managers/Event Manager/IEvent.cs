using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Event
{
    RoomEnter, //when player enters the room
    EnemyDestroyed, //when an enemy is destroyed
    BossTeleport, //when the player teleports to the boss room
    ChestSpawn, //when a chest spawns
    RoomExit, //when player exits the room
    EnemyHitboxEntered, //when player collides with enemy
    EnemyHitPlayer, //when enemy hits player
    PlayerHitEnemy, //when player hits enemy
    RoomSpawn, //when room spawns in (initialisation event)
    DoorsLockUnlock, //lock or unlock the doors
    DialogueStart,
    DialogueFinish,
    StackAdded,

}
