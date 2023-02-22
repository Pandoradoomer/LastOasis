using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IEnemyBehaviour
{
    public void Act();
    public void Freeze();
}

public interface IMovementBehaviour
{
    public Vector2 GetNextMovement();
}

public interface IAttackBehaviour
{
    public IEnumerator Attack();

    public bool CanAttack();
}
