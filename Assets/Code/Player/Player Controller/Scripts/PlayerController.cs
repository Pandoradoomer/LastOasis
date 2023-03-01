using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Getting state for future development once we have animations.
    public enum CURRENT_STATE
    {
        RUNNING,
        IDLE,
        ATTACK,
        DASHING
    };

    public CURRENT_STATE currentState;

    [SerializeField] Rigidbody2D rb;
    [SerializeField] float speed;

    private Vector2 movement;

    private SpriteRenderer sr;
    private Color originalColor;

    public bool invulnerability;
    private float invulnerabilityHolder;
    private float blinkTimer;
    private bool canDash = true;
    private bool isDashing = false;

    private bool isInDialogue = false;

    [SerializeField] private float invulnerabilityDuration;
    [SerializeField] private float dashDistance, dashCooldown, dashLength;

    public static PlayerController instance;
    private void Awake()
    {
        instance = this;
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

        if (canDash)
        {
            // DISCUSSION: Do we want to disable dash when not moving or dash in direction player is looking at?
            if (Input.GetKey(KeyCode.LeftShift) && movement != Vector2.zero)
                Dash();
        }

        ChangeColorOnDash();
        Invulnerability();
    }

    void FixedUpdate()
    {
        switch(currentState)
        {
            case CURRENT_STATE.RUNNING:
                rb.MovePosition(rb.position + movement * speed * Time.deltaTime);
                break;
            case CURRENT_STATE.ATTACK:
                rb.velocity = Vector2.zero;
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

        isDashing = true;

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
        isDashing = false;
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
        if (currentState == CURRENT_STATE.DASHING)
            sr.color = Color.white;
        else
            sr.color = originalColor;
    }

    private void IgnoreCollider(bool ignore)
    {
        int playerLayer = gameObject.layer;
        int enemyLayer = LayerMask.NameToLayer("Enemy");
        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, ignore);
    }
}
