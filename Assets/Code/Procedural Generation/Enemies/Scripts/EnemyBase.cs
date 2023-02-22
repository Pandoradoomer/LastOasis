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
    }

    private void Update()
    {
        if(currentHealth <= 0)
        {
            //TO-DO Andrei: Requires debugging
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {

        }
    }
}
