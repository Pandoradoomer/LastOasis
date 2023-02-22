using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmerBehaviour : MonoBehaviour, IEnemyBehaviour, IMovementBehaviour
{
    public float speed = 1f;
    private Rigidbody2D rb;

    bool canMove = true;
    bool isFlipped = false;
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
        if (canMove)
            rb.velocity = GetNextMovement();
        else
            rb.velocity = Vector2.zero;
    }

    public Vector2 GetNextMovement()
    {
        Vector2 dir = MovementFunctions.FollowPlayer(speed, transform.position);
        FlipSrite(dir);
        return dir;
    }

    private void FlipSrite(Vector2 dir)
    {
        isFlipped = dir.x <= 0 ? true : false;
    }
    public void Freeze()
    {
        rb.velocity = Vector2.zero;
        this.enabled = false;
    }

    public void StopMovement()
    {
        canMove = false;
    }

    public void ResumeMovement()
    {
        canMove = true;
    }
}

