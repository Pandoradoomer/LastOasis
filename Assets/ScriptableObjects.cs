using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ScriptableObjects : ScriptableObject
{
    [SerializeField] public int Counter { get; set; } = 0;

    public void Increment()
    {
        ++Counter;
    }
}
