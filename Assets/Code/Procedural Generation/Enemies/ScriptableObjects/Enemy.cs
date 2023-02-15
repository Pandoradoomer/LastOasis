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
    public float difficulty;
    public MonoScript Behaviour;
    public List<ItemDrop> itemDrops;
    //debug only, differentiates enemies, remove on future iterations, or when we have sprites
    public Color color;
}
