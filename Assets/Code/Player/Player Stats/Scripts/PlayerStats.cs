using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;

public enum Stats
{
    Health,
    Speed,
    Defence,
    Dexterity,
    Damage
}

public enum MaxStats
{
    MaxHealth,
    MaxSpeed,
    MaxDefence,
    MaxDexterity,
    MaxDamage
}

public class PlayerStats : MonoBehaviour
{
    /*
    public static float health
    {
        get
        {
            if (currentHealth >= cappedHealth)
            {
                currentHealth = cappedHealth;
            }
            else if (currentHealth <= 0)
            {
                currentHealth = 0;
            }
            return currentHealth;

        }
        set
        {
            currentHealth = value;
            
        }
    }

    [SerializeField] public static float baseHealth = 100.0f;
    [SerializeField] public static float currentHealth = baseHealth;
    [SerializeField] private static float cappedHealth = 165.0f;
    
    public static float damage
    {
        get
        {
            if (currentDamage >= cappedDamage)
            {
                currentDamage = cappedDamage;
            }
            else if (currentDamage <= 0)
            {
                currentDamage= 0;
            }
            return currentDamage;
        }
        set
        {
            currentDamage = value;
        }
    }

    [SerializeField] public static float baseDamage = 10.0f;
    [SerializeField] public static float currentDamage = baseDamage;
    [SerializeField] private static float cappedDamage = 64.0f;

    public static float moveSpeed
    {
        get
        {
            if (currentMoveSpeed >= cappedMoveSpeed)
            {
                currentMoveSpeed = cappedMoveSpeed;
            }
            else if (currentMoveSpeed <= 0)
            {
                currentMoveSpeed = 0;
            }
            return currentMoveSpeed;
        }
        set
        {
            currentMoveSpeed = value;
        }
    }


    [SerializeField] public static float baseMovementSpeed = 2.0f;
    [SerializeField] public static float currentMoveSpeed = baseMovementSpeed;
    [SerializeField] private static float cappedMoveSpeed = 3.55f;

    public static float dexterity
    {
        get
        {
            if (currentDexterity >= cappedDexterity)
            {
                currentDexterity = cappedDexterity;
            }
            else if (currentDexterity <= 0)
            {
                currentDexterity = 0;
            }

            return currentDexterity;
        }
        set
        {
            currentDexterity = value;
        }
    }

    [SerializeField] public static float baseDexterity = 1.0f;
    [SerializeField] public static float currentDexterity = baseDexterity;
    [SerializeField] private static float cappedDexterity = 1.275f;

    public static float defence
    {
        get
        {
            if (currentDefence >= cappedDefence)
            {
                currentDefence = cappedDefence;
            }
            else if (currentDefence <= 0)
            {
                currentDefence = 0;
            }
            return currentDefence;
        }

        set
        {
            currentDefence = value;
        }
    }



    [SerializeField] public static float baseDefence = 1f;
    [SerializeField] public static float currentDefence = baseDefence;
    [SerializeField] private static float cappedDefence = 55.0f;*/

    //Values that the player is currently holding
    [Header("Current Values")]
    public int currentHealth = -1;
    public float currentSpeed = -1;
    public float currentDexterity = -1;
    public int currentDamage = -1;
    public float currentDefence = -1;

    private bool hasBeenInit = false;
    //Values used for INITIALISATION ONLY
    [Header("Base values")]
    public int baseHealth = 100;
    public float baseSpeed = 2.0f;
    public float baseDexterity = 0.0f;
    public int baseDamage = 5;
    public float baseDefence = 1.0f;

    //Values used to define the maximum value of a stat
    //Useful for when we temporarily increase/decrease a stat, so we can then reset them to this value
    [Header("Max values")]
    public int maxHealth = -1;
    public float maxSpeed = -1;
    public float maxDexterity = -1;
    public int maxDamage = -1;
    public float maxDefence = -1;
    public static PlayerStats instance { get; private set; }

    bool isDead = false;
    void Start()
    {

    }
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        if (PlayerPrefs.HasKey("isSet"))
            hasBeenInit = Convert.ToBoolean(PlayerPrefs.GetString("isSet"));
        if(!hasBeenInit)
        {
            hasBeenInit = true;
            currentHealth = maxHealth = baseHealth;
            currentSpeed = maxSpeed = baseSpeed;
            currentDexterity = maxDexterity = baseDexterity;
            currentDefence = maxDefence = baseDefence;
            currentDamage = maxDamage = baseDamage;
        }
        else
        {
            LoadValues();
        }
        EventManager.StartListening(Event.EnemyHitPlayer, OnEnemyHit);
        EventManager.StartListening(Event.PlayerDeath, OnPlayerDeath);
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {   
        if(Input.GetKeyDown(KeyCode.R))
        {
            ResetStatValues();
        }
        if(Input.GetKeyDown(KeyCode.K))
        {
            currentHealth = 0;
        }

        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            //Reload scene
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            if(!isDead)
            {
                isDead = true;
                EventManager.TriggerEvent(Event.PlayerDeath, null);
            }
            Debug.Log("Player died");
        }
        //if (currentHealth >= baseHealth)
        //{
        //    currentHealth = baseHealth;
        //}
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            maxHealth += 5;
            currentHealth = maxHealth;

            Debug.Log("Current health: " + currentHealth);
            Debug.Log("Base health: " + maxHealth);
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            maxHealth -= 5;
            currentHealth = maxHealth;

            Debug.Log("Current health: " + currentHealth);
            Debug.Log("Base health: " + maxHealth);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            maxDamage += 2;
            currentDamage = maxDamage;

            Debug.Log("Current damage: " + currentDamage);
            Debug.Log("Base damage: " + maxDamage);
        }

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            maxDamage -= 2;
            currentDamage = maxDamage;

            Debug.Log("Current damage: " + currentDamage);
            Debug.Log("Base damage: " + maxDamage);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            maxSpeed += 0.1f;
            currentSpeed = maxSpeed;

            Debug.Log("Current move speed: " + currentSpeed);
            Debug.Log("Base move speed: " + maxSpeed);
        }

        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            maxSpeed -= 0.1f;
            currentSpeed = maxSpeed;

            Debug.Log("Current move speed: " + currentSpeed);
            Debug.Log("Base move speed: " + maxSpeed);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            maxDefence += 1.0f;
            currentDefence = maxDefence;

            Debug.Log("Current defence: " + currentDefence);
            Debug.Log("Base defence: " + maxDefence);

        }

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            maxDefence -= 1.0f;
            currentDefence = maxDefence;

            Debug.Log("Current defence: " + currentDefence);
            Debug.Log("Base defence: " + maxDefence);

        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            maxDexterity += 0.005f;
            currentDexterity = maxDexterity;
            
            Debug.Log("Current dexterity: " + currentDexterity);
            Debug.Log("Base dexterity: " + maxDexterity);
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            maxDexterity -= 0.005f;
            currentDexterity = maxDexterity;

            Debug.Log("Current dexterity: " + currentDexterity);
            Debug.Log("Base dexterity: " + maxDexterity);
        }
    }


    void OnTriggerEnter2D(Collider2D coll)
    {
        //if (coll.gameObject.CompareTag("Coin"))
        //{
        //    coinCounter += Random.Range(item_coin.minValue,item_coin.maxValue);
        //    coinText.text = "Coins: " + coinCounter;
        //}
        //
        //else if (coll.gameObject.CompareTag("CoinPile"))
        //{
        //    coinCounter += Random.Range(item_coin_pile.minValue, item_coin_pile.maxValue);
        //    coinText.text = "Coins: " + coinCounter;
        //}
        //
        //else if (coll.gameObject.CompareTag("CoinBag"))
        //{
        //    coinCounter += Random.Range(item_coin_bag.minValue, item_coin_bag.maxValue);
        //    coinText.text = "Coins: " + coinCounter;
        //}


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // When collided with enemy or boss, the player will take damage.
        if (!PlayerController.instance.invulnerability)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                int healthDeplete = (int)collision.gameObject.GetComponent<EnemyBase>().onCollisionDamage;
                currentHealth -= healthDeplete;
                PlayerController.instance.invulnerability = true;
                EventManager.TriggerEvent(Event.DamageDealt, new DamageDealtPacket()
                {
                    textColor = Color.red,
                    position = transform.position,
                    damage = healthDeplete
                });
            }
            if (collision.gameObject.CompareTag("Boss"))
            {
                int healthDeplete = (int)collision.gameObject.GetComponent<BossPattern>().onCollisionDamage;
                currentHealth -= healthDeplete;
                PlayerController.instance.invulnerability = true;
                EventManager.TriggerEvent(Event.DamageDealt, new DamageDealtPacket()
                {
                    textColor = Color.red,
                    position = transform.position,
                    damage = healthDeplete
                });
            }
        }
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        // When player does not move from the collision point, player will still take damage.
        if (!PlayerController.instance.invulnerability)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                int healthDeplete = (int)collision.gameObject.GetComponent<EnemyBase>().onCollisionDamage;
                currentHealth -= healthDeplete;
                PlayerController.instance.invulnerability = true;
                EventManager.TriggerEvent(Event.DamageDealt, new DamageDealtPacket()
                {
                    textColor = Color.red,
                    position = transform.position,
                    damage = healthDeplete
                });
            }
            if (collision.gameObject.CompareTag("Boss"))
            {
                int healthDeplete = (int)collision.gameObject.GetComponent<BossPattern>().onCollisionDamage;
                currentHealth -= healthDeplete;
                PlayerController.instance.invulnerability = true;
                EventManager.TriggerEvent(Event.DamageDealt, new DamageDealtPacket()
                {
                    textColor = Color.red,
                    position = transform.position,
                    damage = healthDeplete
                });
            }
        }
    }

    private void OnEnemyHit(IEventPacket packet)
    {
        EnemyHitPacket ehp = packet as EnemyHitPacket;
        if (!PlayerController.instance.invulnerability)
        {
            currentHealth -= (int)ehp.healthDeplete;
            PlayerController.instance.invulnerability = true;
            EventManager.TriggerEvent(Event.DamageDealt, new DamageDealtPacket()
            {
                textColor = Color.red,
                position = Singleton.Instance.PlayerController.transform.position,
                damage = (int)ehp.healthDeplete
            });
        }
    }
    private void OnDestroy()
    {
        EventManager.StopListening(Event.EnemyHitPlayer, OnEnemyHit);
        EventManager.StopListening(Event.PlayerDeath, OnPlayerDeath);
        SaveValues();
    }

    public void SaveValues()
    {
        PlayerPrefs.SetInt(Stats.Health.ToString(), currentHealth);
        PlayerPrefs.SetInt(Stats.Damage.ToString(), currentDamage);
        PlayerPrefs.SetFloat(Stats.Defence.ToString(), currentDefence);
        PlayerPrefs.SetFloat(Stats.Dexterity.ToString(), currentDexterity);
        PlayerPrefs.SetFloat(Stats.Speed.ToString(), currentSpeed);

        PlayerPrefs.SetInt(MaxStats.MaxHealth.ToString(), maxHealth);
        PlayerPrefs.SetInt(MaxStats.MaxDamage.ToString(), maxDamage);
        PlayerPrefs.SetFloat(MaxStats.MaxDefence.ToString(), maxDefence);
        PlayerPrefs.SetFloat(MaxStats.MaxDexterity.ToString(), maxDexterity);
        PlayerPrefs.SetFloat(MaxStats.MaxSpeed.ToString(), maxSpeed);
        PlayerPrefs.SetString("isSet", hasBeenInit.ToString());
    }

    public void LoadValues()
    {

        currentHealth = PlayerPrefs.GetInt(Stats.Health.ToString(), currentHealth);
        currentDamage = PlayerPrefs.GetInt(Stats.Damage.ToString(), currentDamage);
        currentDefence = PlayerPrefs.GetFloat(Stats.Defence.ToString(), currentDefence);
        currentDexterity = PlayerPrefs.GetFloat(Stats.Dexterity.ToString(), currentDexterity);
        currentSpeed = PlayerPrefs.GetFloat(Stats.Speed.ToString(), currentSpeed);

        maxHealth =  PlayerPrefs.GetInt(MaxStats.MaxHealth.ToString(), maxHealth);
        maxDamage = PlayerPrefs.GetInt(MaxStats.MaxDamage.ToString(), maxDamage);
        maxDefence = PlayerPrefs.GetFloat(MaxStats.MaxDefence.ToString(), maxDefence);
        maxDexterity = PlayerPrefs.GetFloat(MaxStats.MaxDexterity.ToString(), maxDexterity);
        maxSpeed = PlayerPrefs.GetFloat(MaxStats.MaxSpeed.ToString(), maxSpeed);
        hasBeenInit = Convert.ToBoolean(PlayerPrefs.GetString("isSet"));
    }

    public void ResetStatValues()
    {
        currentHealth = maxHealth = baseHealth;
        currentDamage = maxDamage = baseDamage;
        currentSpeed = maxSpeed = baseSpeed;
        currentDexterity = currentDexterity = baseDexterity;
        currentDefence = maxDefence = baseDefence;
    }

    private void OnPlayerDeath(IEventPacket packet)
    {
        //currentHealth = maxHealth;
        if(PlayerPrefs.HasKey(PlayerPrefsKeys.DEATH_NUMBER.ToString()))
        {
            int value = PlayerPrefs.GetInt(PlayerPrefsKeys.DEATH_NUMBER.ToString());
            PlayerPrefs.SetInt(PlayerPrefsKeys.DEATH_NUMBER.ToString(), value + 1);
        }
        else
        {
            PlayerPrefs.SetInt(PlayerPrefsKeys.DEATH_NUMBER.ToString(), 1);
        }

    }

    public void ResetDeath()
    {
        currentHealth = maxHealth;
        isDead = false;
    }
}