using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class SkeletonBehaviour : BaseMoveAndAttackBehaviour
{
    [SerializeField]
    AttackBase attackHitbox;
    public float stunTime;
    public float windUpTime;
    public float strikeTime;

    private float initialXPos;
    private float initialYPos;
    private float initialXScale;
    private float initialYScale;

    public int xDir, yDir;

    new void Start()
    {
        base.Start();
        initialXScale = attackHitbox.transform.localScale.x;
        initialYScale = attackHitbox.transform.localScale.y;
        initialXPos = attackHitbox.transform.localPosition.x;
        initialYPos = attackHitbox.transform.localPosition.y;

    }    
    private IEnumerator Stun(float duration)
    {
        canMove = false;
        for(float i = 0; i <= duration; i += Time.deltaTime)
        {
            yield return null;
        }
        canMove = true;
    }
    protected override void DoAct()
    {
        if(!isAttacking && canMove)
            TurnAttackHitbox(rb.velocity);
        if (canMove)
            rb.velocity = GetNextMovement();
        else
            rb.velocity = Vector2.zero;
    }

    void TurnAttackHitbox(Vector2 dir)
    {
        dir = dir.normalized;

        //it is closer on the x axis
        if(Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            yDir = 0;
            attackHitbox.transform.localScale = new Vector3(initialXScale, initialYScale, 1);
            if (dir.x < 0)
            {
                attackHitbox.transform.localPosition = new Vector3(-initialXPos, initialYPos, 0);
                xDir = -1;
            }
            else
            {
                attackHitbox.transform.localPosition = new Vector3(initialXPos, initialYPos, 0);
                xDir = 1;
            }
        }
        //closer on the y axis
        else
        {
            xDir = 0;
            attackHitbox.transform.localScale = new Vector3(initialYScale, initialXScale, 1);
            if(dir.y < 0)
            {
                attackHitbox.transform.localPosition = new Vector3(initialYPos, -initialXPos, 0);
                yDir = -1;
            }
            else
            {
                attackHitbox.transform.localPosition = new Vector3(initialYPos, initialXPos, 0);
                yDir = -1;
            }

        }
    }

    protected override void DoSetAnimatorVariables()
    {

    }
    protected override void OnHitAction()
    {
        StartCoroutine(Stun(stunTime));
    }

    protected override Vector2 GetMovement()
    {
        return MovementFunctions.FollowPlayer(speed, transform.position);
    }

    public void OnHitboxEntered(IEventPacket packet)
    {
        EnemyHitboxEnteredPacket ehep = packet as EnemyHitboxEnteredPacket;
        if (ehep.Hitbox == attackHitbox.gameObject && !isAttacking)
        {
            Attack();
        }
    }
    protected override void DoAttack()
    {
        StartCoroutine(AttackFunctions.Swing(this, this, windUpTime, strikeTime));
    }

    protected override void AddAdditionalEventListeners()
    {
        EventManager.StartListening(Event.EnemyHitboxEntered, OnHitboxEntered);
    }

    protected override void RemoveAdditionalEventListeners()
    {

        EventManager.StopListening(Event.EnemyHitboxEntered, OnHitboxEntered);
    }

    protected override void DoBeginAttack()
    {
        attackHitbox.IsAttacking = true;
        attackHitbox.gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }

    protected override void DoStopAttack()
    {
        attackHitbox.IsAttacking = false;
        attackHitbox.gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }
}
