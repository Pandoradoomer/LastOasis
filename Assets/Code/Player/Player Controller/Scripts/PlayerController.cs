using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class PlayerController : MonoBehaviour
{
    // Getting state for future development once we have animations.
    public enum CURRENT_STATE
    {
        RUNNING,
        IDLE,
        ATTACK,
        DASHING,
        SCENE_CHANGE
    };

    public CURRENT_STATE currentState;

    [SerializeField] Rigidbody2D rb;
    [SerializeField] float speed;

    private Vector2 movement;

    private SpriteRenderer sr;
    public Animator animator;
    private Color originalColor;

    public bool invulnerability;
    private float invulnerabilityHolder;
    private float blinkTimer;
    private bool canDash = true;

    private bool isInDialogue = false;

    [SerializeField] private float invulnerabilityDuration;
    [SerializeField] private float dashDistance, dashCooldown, dashLength;
    [SerializeField] private Vector2 lastPlayerDirection;

    public Transform doorWayPoint;

    public KeyCode lastKeyPressed;

    public static PlayerController instance;
    private void Awake()
    {
        instance = this;
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        invulnerabilityHolder = invulnerabilityDuration;
        originalColor = GetComponent<SpriteRenderer>().color;

        EventManager.StartListening(Event.BossTeleport, BossTeleport);
        EventManager.StartListening(Event.DialogueStart, FreezePlayer);
        EventManager.StartListening(Event.DialogueFinish, UnfreezePlayer);
    }

    private void OnDestroy()
    {
        EventManager.StopListening(Event.BossTeleport, BossTeleport);
        EventManager.StopListening(Event.DialogueStart, FreezePlayer);
        EventManager.StopListening(Event.DialogueFinish, UnfreezePlayer);
    }

    void FreezePlayer(IEventPacket packet)
    {
        rb.velocity = Vector2.zero;
        isInDialogue = true;
    }

    void UnfreezePlayer(IEventPacket packet)
    {
        isInDialogue = false;
    }
    private void Update()
    {
        if (isInDialogue)
            return;

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement.Normalize();

        //var key = GetLastKeyPressed();
        //if (key != KeyCode.None)
        //    lastKeyPressed = key;

        if (canDash && currentState == CURRENT_STATE.RUNNING)
        {
            if (Input.GetKey(KeyCode.LeftShift) && movement != Vector2.zero)
                Dash();
        }

        if (movement != Vector2.zero)
        {
            lastPlayerDirection = movement;

            if(currentState == CURRENT_STATE.IDLE)
                currentState = CURRENT_STATE.RUNNING;

            //if (currentState == CURRENT_STATE.RUNNING)
            //    animator.SetBool("isMoving", true);
        } 
        else
        {
            if(currentState == CURRENT_STATE.RUNNING)
                currentState = CURRENT_STATE.IDLE;
        }
        //else
        //{
        //    animator.SetBool("isMoving", false);
        //}
        ChangeColorOnDash();
        Invulnerability();
    }

    KeyCode GetLastKeyPressed()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            return KeyCode.W;
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            return KeyCode.S;
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            return KeyCode.D;
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            return KeyCode.A;
        return KeyCode.None;

    }

    private Dictionary<KeyCode, Vector2> keyMapping = new Dictionary<KeyCode, Vector2>()
    {
        {KeyCode.W, Vector2.up },
        {KeyCode.S, Vector2.down},
        {KeyCode.D, Vector2.right},
        {KeyCode.A, Vector2.left }
    };

    void FixedUpdate()
    {
        if (isInDialogue)
            return;
        //Vector2 lastDir = Vector2.zero;
        //if (lastKeyPressed != KeyCode.None)
        //    lastDir = keyMapping[lastKeyPressed];

        switch (currentState)
        {
            case CURRENT_STATE.IDLE:
                rb.velocity = Vector2.zero;
                animator.SetBool("isMoving", false);
                break;
            case CURRENT_STATE.RUNNING:
                rb.MovePosition(rb.position + movement * speed * Time.deltaTime);
                animator.SetBool("isMoving", true);
                animator.SetFloat("moveX", lastPlayerDirection.x);
                animator.SetFloat("moveY", lastPlayerDirection.y);
                //rb.MovePosition(rb.position + movement * speed * Time.deltaTime);
                //if (Mathf.Abs(lastPlayerDirection.y) == Mathf.Abs(lastPlayerDirection.x) && lastPlayerDirection.x != 0)
                //{
                //    animator.SetFloat("moveX", lastDir.x);
                //    animator.SetFloat("moveY", lastDir.y);
                //}
                //else
                //{
                //    animator.SetFloat("moveX", lastPlayerDirection.x);
                //    animator.SetFloat("moveY", lastPlayerDirection.y);
                //}
                break;
            case CURRENT_STATE.ATTACK:
                rb.velocity = Vector2.zero;
                break;
            //case CURRENT_STATE.COMBO:
            //    rb.velocity = Vector2.zero;
            //    animator.SetFloat("moveX", lastPlayerDirection.x);
            //    animator.SetFloat("moveY", lastPlayerDirection.y);
            //    break;
            case CURRENT_STATE.SCENE_CHANGE:
                Vector2 movePos = Vector2.MoveTowards(transform.position, doorWayPoint.position, 2 * Time.deltaTime);
                rb.MovePosition(movePos);
                break;

        }

    }

    void BossTeleport(IEventPacket packet)
    {
        BossTeleportPacket btp = packet as BossTeleportPacket;
        transform.position = btp.transform.position - Vector3.up * 4.0f;
    }

    private void Dash()
    {
        currentState = CURRENT_STATE.DASHING;

        rb.velocity = Vector2.zero;

        Vector2 dashDirection = movement.normalized * dashDistance;

        int playerLayer = gameObject.layer;
        int enemyLayer = LayerMask.NameToLayer("Enemy");
        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, true);

        rb.AddForce(dashDirection, ForceMode2D.Impulse);

        canDash = false;
        Invoke("ResetDash", dashCooldown);
        Invoke("DisableIsDashing", dashLength);
    }

    private void ResetDash()
    {
        canDash = true;
    }

    private void DisableIsDashing()
    {
        currentState = CURRENT_STATE.RUNNING;
        int playerLayer = gameObject.layer;
        int enemyLayer = LayerMask.NameToLayer("Enemy");
        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, false);
    }

    private void Invulnerability()
    {
        if (invulnerability)
        {
            blinkTimer -= Time.deltaTime;
            ChangePlayerAlpha();
            IgnoreCollider(true);

            if (blinkTimer <= 0.5f)
            {
                blinkTimer = 1;
                invulnerabilityDuration--;

                if (invulnerabilityDuration < 0)
                {
                    invulnerability = false;
                    invulnerabilityDuration = invulnerabilityHolder;
                    blinkTimer = 1;
                    ChangePlayerAlpha();
                    IgnoreCollider(false);
                }
            }
        }
    }

    private void ChangePlayerAlpha()
    {
        Color playerColor = sr.color;
        playerColor.a = blinkTimer;
        gameObject.GetComponent<SpriteRenderer>().color = playerColor;
    }

    private void ChangeColorOnDash()
    {
        Color playerColor = Color.gray;
        if (currentState == CURRENT_STATE.DASHING)
        {
            playerColor.a = 0.8f;
            sr.color = playerColor;
        }
        else
            sr.color = originalColor;
    }

    private void IgnoreCollider(bool ignore)
    {
        int playerLayer = gameObject.layer;
        int enemyLayer = LayerMask.NameToLayer("Enemy");
        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, ignore);
    }

    public Transform FindWayPoint()
    {
        GameObject[] wayPoints = GameObject.FindGameObjectsWithTag("DoorWaypoint");
        Transform tempTrans = null;
        float tempDist = 3;
        for (int i = 0; i < wayPoints.Length; i++)
        {
            float distance = Vector2.Distance(PlayerController.instance.transform.position, wayPoints[i].transform.position);
            if (distance < tempDist)
            {
                tempTrans = wayPoints[i].transform;
                tempDist = distance;
            }
        }
        return tempTrans;
    }
}