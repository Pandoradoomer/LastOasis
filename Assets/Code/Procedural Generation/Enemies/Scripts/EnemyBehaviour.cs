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

    public void StopMovement();

    public void ResumeMovement();
}

public interface IAttackBehaviour
{
    public void BeginAttack();

    public void StopAttack();

    public void Attack();
}
