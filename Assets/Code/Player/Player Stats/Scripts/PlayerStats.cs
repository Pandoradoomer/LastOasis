using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] public float maxHealth = 10.0f;
    [SerializeField] public float currentHealth;
    [SerializeField] public int coinCounter = 0;
    [SerializeField] CollectableData item_coin;
    [SerializeField] CollectableData item_coin_pile;
    [SerializeField] CollectableData item_coin_bag;
    [SerializeField] ConsumeableData item_health;
    [SerializeField] private Slider playerHealthSlider;

    public TextMeshProUGUI coinText;
    public TextMeshProUGUI healthText;
    public static PlayerStats instance { get; private set; }
    void Start()
    {
        coinText.text = "Coins: " + 0;
        playerHealthSlider.maxValue = maxHealth;
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
        currentHealth = maxHealth;
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
        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
        playerHealthSlider.value = currentHealth;

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
                currentHealth -= collision.gameObject.GetComponent<EnemyBase>().onCollisionDamage;
                PlayerController.instance.invulnerability = true;
            }
            if (collision.gameObject.CompareTag("Boss"))
            {
                currentHealth -= collision.gameObject.GetComponent<BossPattern>().onCollisionDamage;
                PlayerController.instance.invulnerability = true;
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
                currentHealth -= collision.gameObject.GetComponent<EnemyBase>().onCollisionDamage;
                PlayerController.instance.invulnerability = true;
            }
            if (collision.gameObject.CompareTag("Boss"))
            {
                currentHealth -= collision.gameObject.GetComponent<BossPattern>().onCollisionDamage;
                PlayerController.instance.invulnerability = true;
            }
        }
    }

    private void OnEnemyHit(IEventPacket packet)
    {
        EnemyHitPacket ehp = packet as EnemyHitPacket;
        if(!PlayerController.instance.invulnerability)
        {
            currentHealth -= ehp.healthDeplete;
            PlayerController.instance.invulnerability = true;
        }
    }
    private void OnDestroy()
    {
        EventManager.StopListening(Event.EnemyHitPlayer, OnEnemyHit);
    }
}
