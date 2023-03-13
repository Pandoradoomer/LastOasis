using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class FungusBehaviour : BaseMoveAndAttackBehaviour
{
    [Header("Attack Variables")]
    [SerializeField]
    FungusAttack attackHitbox;
    public float stunTime;
    public float windUpTime;
    public float strikeTime;

    public float timer;
    private float maxTimer;

    [Header("Pathfinding Variables")]
    public List<GridCell> path = new List<GridCell>();
    GridCell nextCell = null;
    public EnemyBase eb;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        maxTimer = timer;
        eb = GetComponent<EnemyBase>();
        attackHitbox.GetComponent<Collider2D>().enabled = false;
    }


    protected override void DoAct()
    {
        if (canMove && !isAttacking)
            rb.velocity = GetMovement();
        else
            rb.velocity = Vector2.zero;
        if (timer > 0)
            timer -= Time.deltaTime;
        else if (!isAttacking)
            DoAttack();
    }

    protected override void DoAttack()
    {
        StartCoroutine(AttackFunctions.Swing(this, this, windUpTime, strikeTime));
        isAttacking = true;
    }

    protected override void DoBeginAttack()
    {
        attackHitbox.GetComponent<Collider2D>().enabled = true;
        attackHitbox.IsAttacking = true;
        attackHitbox.GetComponent<SpriteRenderer>().enabled = true;
    }


    protected override void DoStopAttack()
    {
        attackHitbox.GetComponent<SpriteRenderer>().enabled = false;
        attackHitbox.GetComponent<Collider2D>().enabled = false;
        attackHitbox.AddStacks();
        isAttacking = false;
        attackHitbox.IsAttacking = false;
        timer = maxTimer;
    }

    protected override Vector2 GetMovement()
    {
        return MovementFunctions.FollowPlayer(speed, transform.position, eb.rs, path, ref nextCell);
    }

    protected override void OnHitAction()
    {

    }

    protected override void DoSetAnimatorVariables()
    {
    }
    protected override void AddAdditionalEventListeners()
    {
    }
    protected override void RemoveAdditionalEventListeners()
    {
    }

}
