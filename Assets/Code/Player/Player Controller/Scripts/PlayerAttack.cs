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
            return swingDelay - PlayerStats.Instance.currentDexterity;
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

    public void testStats()
    {
        swingDamage = PlayerStats.Instance.currentDamage;
        swingDelay = swingDelay - PlayerStats.Instance.currentDexterity;
    }
    private void Awake()
    {
        EventManager.StartListening(Event.DialogueStart, FreezePlayer);
        EventManager.StartListening(Event.DialogueFinish, UnfreezePlayer);
        targetLayer = LayerMask.GetMask("Enemy", "Destructible");
        animationLength = AnimationLength("AttackW");
    }
    private void Start()
    {
        swingDamage = PlayerStats.Instance.currentDamage;
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

        if(Instance.movement == Vector2.zero)
        {
            if (Input.GetMouseButton(0) && canAttack && (Instance.currentState == CURRENT_STATE.RUNNING || Instance.currentState == CURRENT_STATE.IDLE))
            {
                StartCoroutine(Attack());
            }
        }
        else
        { 
            if (Input.GetMouseButton(0) && canAttack && (Instance.currentState == CURRENT_STATE.RUNNING || Instance.currentState == CURRENT_STATE.IDLE))
            {
                StartCoroutine(MoveAttack(Instance.lastPlayerDirection, pushPower));
            }
        }
        testStats();
    }

    private IEnumerator Attack()
    {
        canAttack = false;
        Instance.animator.SetTrigger("isAttackingTrigger");
        Instance.currentState = CURRENT_STATE.ATTACK;
        yield return new WaitForSeconds(animationLength + 0.1f);
        Instance.currentState = CURRENT_STATE.RUNNING;
        Invoke("ResetAttack", SwingDelay);
    }

    private IEnumerator MoveAttack(Vector2 force, float power)
    {
        canAttack = false;
        Instance.animator.SetTrigger("isAttackingTrigger");
        Instance.rb.AddForce(force * power);
        Instance.currentState = CURRENT_STATE.MOVE_ATTACK;
        Invoke("ResetAttack", SwingDelay + animationLength + 0.1f);
        yield return new WaitForSeconds(0.2f);
        Instance.currentState = CURRENT_STATE.ATTACK;
        yield return new WaitForSeconds(animationLength - 0.1f);
        Instance.currentState = CURRENT_STATE.RUNNING;
    }

    private void ResetAttack()
    {
        canAttack = true;
    }

    private float AnimationLength(string clipName)
    {
        if (Instance.animator != null && Instance.animator.runtimeAnimatorController != null)
        {
            foreach (AnimationClip clip in Instance.animator.runtimeAnimatorController.animationClips)
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
