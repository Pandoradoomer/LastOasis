using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using static PlayerController;

public class PlayerAttack : MonoBehaviour
{
    public float SwingDelay
    {
        get
        {
            return swingDelay - PlayerStats.Dexterity;
        }
        set
        {
            swingDelay = value;
        }
    }

    public float swingDelay;
    public float swingDamage;

    public float pushPower;

    public LayerMask targetLayer;

    public bool canAttack = true;
    public float animationLength;
    public SpriteRenderer sr;

    private bool isInDialogue = false;
    private void Awake()
    {
        EventManager.StartListening(Event.DialogueStart, FreezePlayer);
        EventManager.StartListening(Event.DialogueFinish, UnfreezePlayer);
        targetLayer = LayerMask.GetMask("Enemy", "Destructible");
        animationLength = AnimationLength("AttackW");
    }
    private void Start()
    {
        swingDamage = Singleton.Instance.PlayerStats.currentDamage;
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

        if(instance.movement == Vector2.zero)
        {
            if (Input.GetMouseButton(0) && canAttack && (instance.currentState == CURRENT_STATE.RUNNING || instance.currentState == CURRENT_STATE.IDLE))
            {
                StartCoroutine(Attack());
            }
        }
        else
        { 
            if (Input.GetMouseButton(0) && canAttack && (instance.currentState == CURRENT_STATE.RUNNING || instance.currentState == CURRENT_STATE.IDLE))
            {
                StartCoroutine(MoveAttack(instance.lastPlayerDirection, pushPower));
            }
        }
    }

    private IEnumerator Attack()
    {
        canAttack = false;
        instance.animator.SetTrigger("isAttackingTrigger");
        instance.currentState = CURRENT_STATE.ATTACK;
        yield return new WaitForSeconds(animationLength + 0.1f);
        instance.currentState = CURRENT_STATE.RUNNING;
        Invoke("ResetAttack", SwingDelay);
    }

    private IEnumerator MoveAttack(Vector2 force, float power)
    {
        canAttack = false;
        instance.animator.SetTrigger("isAttackingTrigger");
        instance.rb.AddForce(force * power);
        instance.currentState = CURRENT_STATE.MOVE_ATTACK;
        Invoke("ResetAttack", SwingDelay + animationLength + 0.1f);
        yield return new WaitForSeconds(0.2f);
        instance.currentState = CURRENT_STATE.ATTACK;
        yield return new WaitForSeconds(animationLength - 0.1f);
        instance.currentState = CURRENT_STATE.RUNNING;
    }

    private void ResetAttack()
    {
        canAttack = true;
    }

    private float AnimationLength(string clipName)
    {
        if (instance.animator != null && instance.animator.runtimeAnimatorController != null)
        {
            foreach (AnimationClip clip in instance.animator.runtimeAnimatorController.animationClips)
            {
                if (clip.name == clipName)
                {
                    return clip.length;
                }
            }
        }
        Debug.LogError("Make sure the animation name is correct.");
        return 0;
    }
}
