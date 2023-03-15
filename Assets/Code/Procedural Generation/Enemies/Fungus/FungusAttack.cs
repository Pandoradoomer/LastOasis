using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FungusAttack : AttackBase
{

    float timer = 0;
    float timeToAddStacks = 0.5f;
    bool isInMist = false;
    private FungusPoisonStack stack;

    private void Start()
    {
        stack = new FungusPoisonStack((int)enemyBase.attackDamage / 2, 0.75f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (IsAttacking)
            {
                //send message to attack
                EventManager.TriggerEvent(Event.EnemyHitPlayer, new EnemyHitPacket()
                {
                    healthDeplete = enemyBase.attackDamage
                });
                isInMist = true;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isInMist = false;
        }
    }

    public void StopAttack()
    {
        isInMist = false;
    }

    private void Update()
    {
        if(isInMist)
        {
            timer += Time.deltaTime;
            if(timer > timeToAddStacks)
            {
                EventManager.TriggerEvent(Event.StackAdded, new StackAddedPacket()
                {
                    stackToAdd = stack
                });
                timer = 0;
            }
        }
    }
}
