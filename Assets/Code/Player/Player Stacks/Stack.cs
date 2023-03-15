using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Stack
{

}

public abstract class DepletableStack : Stack
{
    float timeToDeplete;
    protected abstract void OnDeplete();
}

public abstract class NonDepletableStack : Stack
{
    protected abstract void OnAdd();
}
