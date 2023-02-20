using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;
using UnityEngine.Timeline;

public class Singleton
{
    private static Singleton _instance;

    private LevelGeneration levelGeneration;
    private PlayerStats playerStats;
    private Inventory inventory;
    private ItemSpawnManager itemSpawnManager;
    private EnemyManager enemyManager;
    private Singleton()
    {
        levelGeneration = GameObject.FindObjectOfType<LevelGeneration>();
        playerStats = GameObject.FindObjectOfType<PlayerStats>();
        inventory = GameObject.FindObjectOfType<Inventory>();
        itemSpawnManager = GameObject.FindObjectOfType<ItemSpawnManager>();
        enemyManager = GameObject.FindObjectOfType<EnemyManager>();
    }

    public static Singleton Instance
    {
        get
        {
            if (_instance == null)
                _instance = new Singleton();
            return _instance;
        }
    }

    public LevelGeneration LevelGeneration { get => levelGeneration; }
    public PlayerStats PlayerStats { get => playerStats;}
    public Inventory Inventory { get => inventory; }
    public ItemSpawnManager ItemSpawnManager { get => itemSpawnManager;}
    public EnemyManager EnemyManager { get => enemyManager; }


}
