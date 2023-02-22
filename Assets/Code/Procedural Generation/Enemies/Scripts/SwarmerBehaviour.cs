using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmerBehaviour : MonoBehaviour, IEnemyBehaviour
{
    public float speed = 1f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Act();
    }
    public void Act()
    {
        rb.velocity = Move();
    }

    private Vector2 Move()
    {
        return MovementFunctions.FollowPlayer(speed, transform.position);
    }

    public void Freeze()
    {
        rb.velocity = Vector2.zero;
        this.enabled = false;
    }
}

