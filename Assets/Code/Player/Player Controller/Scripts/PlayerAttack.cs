using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using static PlayerController;

public class PlayerAttack : MonoBehaviour
{
    public float swingDuration;
    public float swingDelay;
    public float swingDamage;
    private float swingDamageHolder;
    public LayerMask targetLayer;

    public CircleCollider2D swordCollider;
    public bool canAttack = true;
    public float animationLength;
    public SpriteRenderer sr;

    private bool isInDialogue = false;

    [SerializeField] private int combo;
    [SerializeField] private float comboTimer;
    [SerializeField] private float comboTimerHolder;
    [SerializeField] private float comboDelay;
    [SerializeField] private float damageMultiplier;
    [SerializeField] private bool isComboAttack;
    private void Awake()
    {
        //swordCollider = GetComponent<CircleCollider2D>();
        //sr.enabled = false;
        EventManager.StartListening(Event.DialogueStart, FreezePlayer);
        EventManager.StartListening(Event.DialogueFinish, UnfreezePlayer);
        //swingDamage = Singleton.Instance.PlayerStats.currentDamage;
        comboTimerHolder = comboTimer;
        swingDamageHolder = swingDamage;
        targetLayer = LayerMask.GetMask("Enemy", "Destructible");
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
            //StartCoroutine(SwingSword());
        if (Input.GetMouseButton(0) && canAttack /*&& !instance.invulnerability*/ && instance.currentState != CURRENT_STATE.DASHING && !isComboAttack)
        {
            StartCoroutine(AttackEnum());
            //Invoke("ResetAttack", swingDelay + animationLength);
        }
        if(isComboAttack)
        {
            instance.currentState = CURRENT_STATE.ATTACK;
            comboTimer -= Time.deltaTime;
            if(comboTimer > 0 && Input.GetMouseButton(0))
            {
                combo++;
                if(combo == 2)
                {
                    swingDamage *= damageMultiplier;
                }
                StartCoroutine(AttackEnum());
            }
        }
        ResetComboOnTime();
    }

    private IEnumerator SwingSword()
    {
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
        isComboAttack = false;
        instance.animator.SetTrigger("isAttackingTrigger");
        instance.animator.SetBool("isMoving", false);
        instance.currentState = CURRENT_STATE.ATTACK;
        yield return new WaitForSeconds(animationLength);
        instance.currentState = CURRENT_STATE.COMBO;
        yield return new WaitForSeconds(comboDelay);
        if (combo < 2)
        {
            comboTimer = comboTimerHolder;
            isComboAttack = true;
        }
        else
        {
            ResetCombo();
        }
    }

    private void ResetAttack()
    {
        canAttack = true;
        isComboAttack = false;
    }
    private void ResetComboOnTime()
    {
        if(comboTimer <= 0)
        {
            ResetCombo();
        }
    }

    private void ResetCombo()
    {
        isComboAttack = false;
        comboTimer = comboTimerHolder;
        combo = 0;
        swingDamage = swingDamageHolder;
        instance.currentState = CURRENT_STATE.RUNNING;
        Invoke("ResetAttack", swingDelay + animationLength);
    }

}
