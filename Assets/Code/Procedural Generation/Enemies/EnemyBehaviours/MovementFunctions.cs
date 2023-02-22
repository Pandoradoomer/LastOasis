using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MovementFunctions
{
    public static Vector2 FollowPlayer(float speed, Vector2 currentPos)
    {
        PlayerController pc = Singleton.Instance.PlayerController;
        Vector2 playerPos = pc.transform.position;

        Vector2 Direction = (playerPos - currentPos).normalized;
        return Direction * speed;
    }
}
