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
    private Singleton()
    {
        levelGeneration = GameObject.FindObjectOfType<LevelGeneration>();
        playerStats = GameObject.FindObjectOfType<PlayerStats>();
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


}
