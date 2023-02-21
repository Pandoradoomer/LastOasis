using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "chestitemscriptableobject", menuName = "ScriptableObjects/Items/Chest")]
public class ChestBase : ScriptableObject
{
    [SerializeField] private float spawnProbability;
}
