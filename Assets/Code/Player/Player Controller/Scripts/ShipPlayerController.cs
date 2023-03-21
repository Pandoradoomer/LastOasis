using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipPlayerController : MonoBehaviour, IPlayerController
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
        //speed = Singleton.Instance.PlayerStats.currentSpeed;
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
        FreezePlayer();
    }

    public void FreezePlayer()
    {

        rb.velocity = Vector2.zero;
        animator.SetBool("isMoving", false);
        isInDialogue = true;
    }

    void UnfreezePlayer(IEventPacket packet)
    {
        UnfreezePlayer();
    }
    public void UnfreezePlayer()
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Chef"))
        {
            MessageManager.instance.DisplayChefText();
        }

        if (collision.CompareTag("Carpenter"))
        {
            MessageManager.instance.DisplayCarpenterText();
        }

        if (collision.CompareTag("Surgeon"))
        {
            MessageManager.instance.DisplaySurgeonText();
        }

        if (collision.CompareTag("QuarterMaster"))
        {
            MessageManager.instance.DisplayQMText();
        }

        if (collision.CompareTag("Gunner"))
        {
            MessageManager.instance.DisplayGunnerText();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Chef"))
        {
            MessageManager.instance.DisableChefText();
        }

        if (collision.CompareTag("Carpenter"))
        {
            MessageManager.instance.DisableCarpenterText();
        }

        if (collision.CompareTag("Surgeon"))
        {
            MessageManager.instance.DisableSurgeonText();
        }

        if (collision.CompareTag("QuarterMaster"))
        {
            MessageManager.instance.DisableQMText();
        }

        if (collision.CompareTag("Gunner"))
        {
            MessageManager.instance.DisableGunnerText();
        }
    }


}
