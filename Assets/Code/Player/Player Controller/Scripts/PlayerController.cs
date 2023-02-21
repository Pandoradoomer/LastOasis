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

    private bool canDash = true;
    private bool isDashing = false;

    [SerializeField] private float dashDistance, dashCooldown, dashLength;

    public static PlayerController instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        EventManager.StartListening(Event.BossTeleport, BossTeleport);
    }

    private void OnDestroy()
    {
        EventManager.StopListening(Event.BossTeleport, BossTeleport);
    }

    private void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement.Normalize();

        if (canDash)
        {
            if (Input.GetKey(KeyCode.LeftShift))
                Dash();
        }

        if (isDashing)
            currentState = CURRENT_STATE.DASHING;
        else
            currentState = CURRENT_STATE.RUNNING;
    }

    void FixedUpdate()
    {
        if(currentState == CURRENT_STATE.RUNNING)
            rb.MovePosition(rb.position + movement * speed * Time.deltaTime);
    }

    void BossTeleport(IEventPacket packet)
    {
        BossTeleportPacket btp = packet as BossTeleportPacket;
        transform.position = btp.transform.position - Vector3.up * 4.0f;
    }

    private void Dash()
    {
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
        isDashing = false;
        int playerLayer = gameObject.layer;
        int enemyLayer = LayerMask.NameToLayer("Enemy");
        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, false);
    }
}
