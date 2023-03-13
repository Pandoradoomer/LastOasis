using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeepingTreeBehaviour : BaseAttackBehaviour
{

    [Header("Attack variables")]
    [SerializeField]
    AttackBase attackHitbox;
    public float windUpTime;
    public float strikeTime;
    public float timer;
    private float maxTimer;

    public bool isAttacking = false;

    protected override void DoAct()
    {
        if (timer > 0)
            timer -= Time.deltaTime;
        else
            DoAttack();
    }

    protected override void DoAttack()
    {
        StartCoroutine(AttackFunctions.Smash(this, windUpTime, strikeTime));
    }

    protected override void DoBeginAttack()
    {
        isAttacking = true;
    }

    protected override void DoStopAttack()
    {
        isAttacking = false;
        timer = maxTimer;
    }

    protected override void DoSetAnimatorVariables()
    {

    }




    // Start is called before the first frame update
    void Start()
    {
        maxTimer = timer;
    }

    // Update is called once per frame
    void Update()
    {

    }
    protected override void RemoveAdditionalEventListeners()
    {
    }
    protected override void AddAdditionalEventListeners()
    {
    }
    protected override void OnHitAction()
    {
    }
}
