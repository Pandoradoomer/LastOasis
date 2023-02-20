using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public int roomIndex;
    public float currentHealth = 10;
    public Dictionary<Item, int> lootToDrop;
    private void Awake()
    {
        lootToDrop = new Dictionary<Item, int>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            currentHealth -= 2;
            if (currentHealth <= 0)
            {
                EventManager.TriggerEvent(Event.EnemyDestroyed,
                    new EnemyDestroyedPacket()
                    {
                        go = this.gameObject,
                        lootToDrop = lootToDrop
                    });
            }
        }
    }
}
