using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//holds all the enemy's data from which all the other behaviours are derived
public class EnemyBase : MonoBehaviour
{
    public int roomIndex;
    public float currentHealth = 10;
    public float onCollisionDamage;
    public float attackDamage;
    public Dictionary<Item, int> lootToDrop;
    public RoomScript rs;
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
        if(gameObject.scene.isLoaded)
            rs.enemies.Remove(this);
        EventManager.StopListening(Event.PlayerHitEnemy, OnHit);
    }
}
