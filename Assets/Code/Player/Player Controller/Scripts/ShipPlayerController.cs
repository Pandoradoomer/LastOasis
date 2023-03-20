using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipPlayerController : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float speed;

    private Vector2 movement;
    [SerializeField] private Vector2 lastPlayerDirection;
    public Animator animator;

    public bool isInDialogue = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {

        EventManager.StartListening(Event.DialogueStart, FreezePlayer);
        EventManager.StartListening(Event.DialogueFinish, UnfreezePlayer);
    }
    private void OnDestroy()
    {

        EventManager.StopListening(Event.DialogueStart, FreezePlayer);
        EventManager.StopListening(Event.DialogueFinish, UnfreezePlayer);
    }

    void FreezePlayer(IEventPacket packet)
    {
        rb.velocity = Vector2.zero;
        animator.SetBool("isMoving", false);
        isInDialogue = true;
    }

    void UnfreezePlayer(IEventPacket packet)
    {
        isInDialogue = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (isInDialogue)
            return;

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement.Normalize();
        if (movement != Vector2.zero)
            lastPlayerDirection = movement;
    }

    private void FixedUpdate()
    {
        if (isInDialogue)
            return;
        if (movement == Vector2.zero)
            animator.SetBool("isMoving", false);
        else
        {
            animator.SetBool("isMoving", true);
            animator.SetFloat("moveX", lastPlayerDirection.x);
            animator.SetFloat("moveY", lastPlayerDirection.y);
        }
        rb.velocity = movement * speed;
    }
}
