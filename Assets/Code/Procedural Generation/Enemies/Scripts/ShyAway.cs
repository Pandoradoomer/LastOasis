using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShyAway : MonoBehaviour, IEnemyBehaviour
{
    private GameObject player;
    private Rigidbody2D rb;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Act();
    }
    public void Act()
    {
        Vector2 playerPos = player.transform.position;
        Vector2 enemyPos = transform.position;

        Vector2 dir = (enemyPos - playerPos).normalized;

        rb.velocity = dir;
    }
    
    public void Freeze()
    {
        rb.velocity = Vector2.zero;
        this.enabled = false;
    }
}
