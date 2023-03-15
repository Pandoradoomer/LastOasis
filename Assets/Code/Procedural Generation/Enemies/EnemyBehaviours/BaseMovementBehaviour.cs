using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IMovementBehaviour
{
    public Vector2 GetNextMovement();

    public void StopMovement();

    public void ResumeMovement();
}

