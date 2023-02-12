using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "consumeabledatScriptableobject", menuName = "ScriptableObjects/Consumeable")]
public class ConsumeableData : ScriptableObject
{
    public string consumeableName;
    public Sprite sprite;
    [SerializeField] public int Counter { get; set; } = 0;

    public void Increment()
    {
        ++Counter;
    }
}
