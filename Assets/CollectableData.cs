using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "collectabledataScriptableobject", menuName = "ScriptableObjects/Collectable")]
public class CollectableData : ScriptableObject
{
    public string itemName;
    public Sprite sprite;
    [SerializeField] public int Counter { get; set; } = 0;

    public void Increment()
    {
        ++Counter;
    }
}
