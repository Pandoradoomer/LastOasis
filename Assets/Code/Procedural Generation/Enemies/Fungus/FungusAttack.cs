using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FungusAttack : AttackBase
{
    int stacks = 0;
    float timer = 0;
    float timeToAddStacks = 0.5f;
    bool isInMist = false;
    bool hasAddedStacks = false;

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
                hasAddedStacks = false;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            AddStacks();
        }
    }

    public void AddStacks()
    {
        if(!hasAddedStacks)
        {
            isInMist = false;
            Debug.Log(stacks);
            stacks = 0;
            hasAddedStacks = true;
        }
    }

    private void Update()
    {
        if(isInMist)
        {
            timer += Time.deltaTime;
            if(timer > timeToAddStacks)
            {
                stacks++;
                timer = 0;
            }
        }
    }
}
