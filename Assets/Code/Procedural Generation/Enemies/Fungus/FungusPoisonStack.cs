using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FungusPoisonStack : DepletableStack
{
    [SerializeField]
    private int damagePerStack;
    public FungusPoisonStack(int damagePerStack)
    {
        this.damagePerStack = damagePerStack;
    }
    protected override void OnDeplete()
    {
        Singleton.Instance.PlayerStats.currentHealth -= damagePerStack;
    }
}
