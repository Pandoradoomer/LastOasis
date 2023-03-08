using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerController;

public class PlayerAttack : MonoBehaviour
{
    public float swingDuration;
    public float swingDelay;
    public float swingDamage;
    public LayerMask targetLayer;

    public CircleCollider2D swordCollider;
    public bool canAttack = true;
    public SpriteRenderer sr;

    private bool isInDialogue = false;
    private void Awake()
    {
        //swordCollider = GetComponent<CircleCollider2D>();
        //sr.enabled = false;
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
            //StartCoroutine(SwingSword());
        if (Input.GetMouseButton(0) && canAttack && instance.currentState == CURRENT_STATE.RUNNING && !instance.invulnerability)
        {
            StartCoroutine(AttackEnum());
            Invoke("ResetAttack", swingDelay + 0.35f);
        }
    }

    private IEnumerator SwingSword()
    {
        // TODO: Add animation to make it look like a sword swing.
        // DISCUSSION: Do we want the player to be unable to move the sword once the swing has happened?
        instance.currentState = CURRENT_STATE.ATTACK;
        canAttack = true;
        sr.enabled = true;

        swordCollider.enabled = true;
        yield return new WaitForSeconds(swingDuration);
        swordCollider.enabled = false;

        yield return new WaitForSeconds(swingDelay);

        canAttack = false;
        sr.enabled = false;
        instance.currentState = CURRENT_STATE.RUNNING;
    }

    private IEnumerator AttackEnum()
    {
        canAttack = false;
        instance.animator.SetBool("isAttacking", true);
        instance.currentState = CURRENT_STATE.ATTACK;
        yield return null;
        instance.animator.SetBool("isAttacking", false);
        yield return new WaitForSeconds(0.35f);
        instance.currentState = CURRENT_STATE.RUNNING;
    }

    private void ResetAttack()
    {
        canAttack = true;
    }
}
