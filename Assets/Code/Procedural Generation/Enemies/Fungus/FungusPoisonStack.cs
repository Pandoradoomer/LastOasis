using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FungusPoisonStack : DepletableStack
{
    [SerializeField]
    private int damagePerStack;
    public FungusPoisonStack(int damagePerStack, float timeToDeplete)
    {
        this.damagePerStack = damagePerStack;
        this.timeToDeplete = timeToDeplete;
    }
    public override void OnDeplete()
    {
        PlayerStats.Instance.currentHealth -= damagePerStack;
        EventManager.TriggerEvent(Event.DamageDealt, new DamageDealtPacket()
        {
            textColor = Color.green,
            damage = damagePerStack,
            position = PlayerController.Instance.transform.position
        });
    }
}
