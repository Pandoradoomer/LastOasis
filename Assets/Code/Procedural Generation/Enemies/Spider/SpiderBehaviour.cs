using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderBehaviour : BaseMoveAndAttackBehaviour
{
    [Header("Attack Properties")]
    [SerializeField]
    AttackBase attackHitbox;
    public float stunTime;
    public float windUpTime;
    public float strikeTime;
    

    //Base positions for the hitbox to use for reset/rescale
    private float initialXPos;
    private float initialYPos;
    private float initialXScale;
    private float initialYScale;
    //Enemy Direction Facing
    public int xDir, yDir;

    public bool isChillOut = true;
    public float chillOutTimer = 0.0f;
    private float maxChillOutTimer = 0.0f;
    public float chillOutDuration = 0.0f;
    private float maxChillOutDuration = 0.0f;

    private Coroutine AttackCoroutine;
    //Pathfinding Variables
    public List<GridCell> path = new List<GridCell>();
    GridCell nextCell = null;
    public EnemyBase eb;

    [SerializeField]
    SpriteRenderer spotSpriteRenderer;
    
    new void Start()
    {
        base.Start();
        initialXScale = attackHitbox.transform.localScale.x;
        initialYScale = attackHitbox.transform.localScale.y;
        initialXPos = attackHitbox.transform.localPosition.x;
        initialYPos = attackHitbox.transform.localPosition.y;
        eb = GetComponent<EnemyBase>();
        SetMultipliers();
        spotSpriteRenderer.color = eb.enemyData.color;
        chillOutTimer = Random.Range(4.0f, 5.0f);
        chillOutDuration = Random.Range(0.5f, 1.0f);

    }

    void SetMultipliers()
    {

    }
    private IEnumerator Stun(float duration)
    {
        canMove = false;
        StopCoroutine(AttackCoroutine);
        DoStopAttack();
        for (float i = 0; i <= duration; i += Time.deltaTime)
        {
            yield return null;
        }
        canMove = true;
    }
    protected override void DoAct()
    {
        if (!isAttacking && canMove)
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
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
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
            if (dir.y < 0)
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
        return MovementFunctions.FollowPlayer(speed, transform.position, eb.rs, path, ref nextCell);
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
        AttackCoroutine = StartCoroutine(AttackFunctions.Swing(this, this, windUpTime, strikeTime));
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
