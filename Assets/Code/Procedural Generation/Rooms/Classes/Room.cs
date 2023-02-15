using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room 
{
    public Vector2 gridPos;
    public int distToCentre = -1;
    public int type;
    public int doors = 0;
    public int x, y;

    public Room(Vector2 _gridPos, int _type)
    {
        gridPos = _gridPos;
        type = _type;
    }
}