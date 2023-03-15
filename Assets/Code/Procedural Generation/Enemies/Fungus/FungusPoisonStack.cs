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
        Singleton.Instance.PlayerStats.currentHealth -= damagePerStack;
    }
}
