using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
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
    [SerializeField] private static float cappedDefence = 55.0f;

    [SerializeField] public int coinCounter = 0;
    [SerializeField] CollectableData item_coin;
    [SerializeField] CollectableData item_coin_pile;
    [SerializeField] CollectableData item_coin_bag;
    [SerializeField] ConsumeableData item_health;
    [SerializeField] private Slider playerHealthSlider;
    //Current stat values are calculations added on top of base value
    //Capped value is used as a condition for: when the current value is >= capped value, then set currentValue = cappedValue
    //Invoke event to open UI panel when interacting with E on Npcs and mast. Create InteractManager with invoke methods 
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI healthText;
    public static PlayerStats instance { get; private set; }
    void Start()
    {
        coinText.text = "Coins: " + 0;
        playerHealthSlider.maxValue = baseHealth;
        EventManager.StartListening(Event.EnemyHitPlayer, OnEnemyHit);
    }
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        //currentHealth = baseHealth;
        //health = PlayerPrefs.GetFloat(health);
    }

    void Update()
    {
        healthText.text = "Health: " + currentHealth.ToString("#.00");      //Round to 2 d.p
        if (currentHealth <= 0)
        {
            //Reload scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            Debug.Log("Player died");
        }
        //if (currentHealth >= baseHealth)
        //{
        //    currentHealth = baseHealth;
        //}
        playerHealthSlider.value = currentHealth;
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            health += 5.0f;

            Debug.Log("Current health: " + health);
            Debug.Log("Base health: " + baseHealth);
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            health -= 5.0f;

            Debug.Log("Current health: " + health);
            Debug.Log("Base health: " + baseHealth);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            damage += 2.0f;

            Debug.Log("Current damage: " + currentDamage);
            Debug.Log("Base damage: " + baseDamage);
        }

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            damage -= 2.0f;

            Debug.Log("Current damage: " + damage);
            Debug.Log("Base damage: " + baseDamage);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            moveSpeed += 0.1f;

            Debug.Log("Current move speed: " + currentMoveSpeed);
            Debug.Log("Base move speed: " + baseMovementSpeed);
        }

        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            moveSpeed -= 0.1f;

            Debug.Log("Current move speed: " + currentMoveSpeed);
            Debug.Log("Base move speed: " + baseMovementSpeed);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            defence += 1.0f;

            Debug.Log("Current defence: " + currentDefence);
            Debug.Log("Base defence: " + baseDefence);

        }

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            defence -= 1.0f;

            Debug.Log("Current defence: " + currentDefence);
            Debug.Log("Base defence: " + baseDefence);

        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            dexterity += 0.005f;

            Debug.Log("Current dexterity: " + currentDexterity);
            Debug.Log("Base dexterity: " + baseDexterity);
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            dexterity -= 0.005f;

            Debug.Log("Current dexterity: " + currentDexterity);
            Debug.Log("Base dexterity: " + baseDexterity);
        }
        //if (Input.GetKeyDown(KeyCode.C) && Inventory.instance.HasCoin(item_coin))
        //{
        //    Inventory.instance.Remove(item_coin, 1);
        //    Debug.Log("You spent 1 coin");
        //    coinCounter--;
        //    coinText.text = "Coins: " + coinCounter;
        //    if (coinCounter <= 0)
        //    {
        //        Debug.Log("You have no coins");
        //    }
        //}
        //Fix bug of removing coins after its reached 0
        //Key press to spend all coins
        coinText.text = $"Coins: {Singleton.Instance.Inventory.GetCoins()}";
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
            currentHealth -= ehp.healthDeplete;
            PlayerController.instance.invulnerability = true;
            EventManager.TriggerEvent(Event.DamageDealt, new DamageDealtPacket()
            {
                textColor = Color.red,
                position = transform.position,
                damage = (int)ehp.healthDeplete
            });
        }
    }
    private void OnDestroy()
    {
        EventManager.StopListening(Event.EnemyHitPlayer, OnEnemyHit);
    }

    public void IncreaseHealth()
    {
        currentHealth = baseHealth + 2;
    }

    public void ResetStatValues()
    {
        currentHealth = baseHealth;
        currentDamage = baseDamage;
        currentMoveSpeed = baseMovementSpeed;
        currentDexterity = baseDexterity;
        currentDefence = baseDefence;
    }
}