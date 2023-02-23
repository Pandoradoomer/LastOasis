using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class SkeletonBehaviour : MonoBehaviour, IEnemyBehaviour, IMovementBehaviour, IAttackBehaviour
{
    [SerializeField]
    AttackBase attackHitbox;

    public float speed;
    private Rigidbody2D rb;
    // Start is called before the first frame update

    bool canMove = true;
    bool isAttacking = false;

    public float windUpTime;
    public float strikeTime;

    //TODO: fix duplicate code
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        EventManager.StartListening(Event.EnemyHitboxEntered, OnHitboxEntered);
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
        attackHitbox.transform.rotation = dir.x <= 0 ? Quaternion.Euler(new Vector3(0,0,180)) : Quaternion.identity;
        return dir;
    }

    public void StopMovement()
    {
        canMove = false;
    }
    public void ResumeMovement()
    {
        canMove = true;
    }

    public void OnHitboxEntered(IEventPacket packet)
    {
        EnemyHitboxEnteredPacket ehep = packet as EnemyHitboxEnteredPacket;
        if(ehep.Hitbox == attackHitbox.gameObject && !isAttacking)
        {
            Attack();
        }
    }

    public void Freeze()
    {
        rb.velocity = Vector2.zero;
        canMove = true;
        StopAttack();
    }
    public void Attack()
    {
        Attack(windUpTime, strikeTime);
    }

    private void Attack(float windUpTime, float strikeTime)
    {
        StartCoroutine(AttackFunctions.Swing(this, this, windUpTime, strikeTime));
    }

    public void BeginAttack()
    {
        attackHitbox.IsAttacking = true;
        attackHitbox.SetSpriteActive(true);
    }
    public void StopAttack()
    {
        attackHitbox.IsAttacking = false;
        attackHitbox.SetSpriteActive(false);
    }

    public void OnDestroy()
    {
        EventManager.StopListening(Event.EnemyHitboxEntered, OnHitboxEntered);
    }
}
