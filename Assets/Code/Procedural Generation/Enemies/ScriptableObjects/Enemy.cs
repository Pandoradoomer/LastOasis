using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum EnemyType
{
    GOON,
    BOSS
}

[CreateAssetMenu(fileName = "collectabledataScriptableobject", menuName = "ScriptableObjects/Enemies/Enemy")]

public class Enemy : ScriptableObject
{
    public string Name;
    public float MaxHealth;
    public float CurrentHealth;
    public Sprite Sprite;
    public EnemyType Type;
    public int Damage;
    public MonoScript Behaviour;
    public List<ItemDrop> itemDrops;
}
