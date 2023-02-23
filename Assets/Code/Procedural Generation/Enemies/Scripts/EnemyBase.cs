using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public int roomIndex;
    public float currentHealth = 10;
    public float onCollisionDamage;
    public float attackDamage;
    public Dictionary<Item, int> lootToDrop;
    private void Awake()
    {
        lootToDrop = new Dictionary<Item, int>();
        EventManager.StartListening(Event.PlayerHitEnemy, OnHit);
    }

    private void Update()
    {
        if(currentHealth <= 0)
        {
            EventManager.TriggerEvent(Event.EnemyDestroyed, new EnemyDestroyedPacket()
            {
                go = gameObject,
                lootToDrop = lootToDrop
            });
        }
    }

    private void OnHit(IEventPacket packet)
    {
        PlayerHitPacket php = packet as PlayerHitPacket;
        if(php.enemy == this.gameObject)
        {
            currentHealth -= php.damage;
        }
    }

    private void OnDestroy()
    {
        EventManager.StopListening(Event.PlayerHitEnemy, OnHit);
    }
}
