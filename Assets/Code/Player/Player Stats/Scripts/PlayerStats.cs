using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] public float maxHealth = 10.0f;
    [SerializeField] public float currentHealth;
    [SerializeField] public int coinCounter = 0;
    [SerializeField] CollectableData item_coin;
    [SerializeField] ConsumeableData item_health;
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI healthText;
    private Inventory inventory;
    //Make instance of inv class for robustness
    //Invoke, coin and health using unity events
    void Start()
    {
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        coinText.text = "Coins: " + 0;

    }
    private void Awake()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        currentHealth  -= 0.09f * Time.deltaTime;
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

        if (Input.GetKeyDown(KeyCode.C) && inventory.HasCoin(item_coin))
        {
            inventory.Remove(item_coin);
            Debug.Log("You spent 1 coin");
            coinCounter--;
            coinText.text = "Coins: " + coinCounter;
            if (coinCounter <= 0)
            {
                Debug.Log("You have no coins");
            }
        }
        //Fix bug of removing coins after its reached 0
        //Key press to spend all coins
      
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Coin"))
        {
            coinCounter++;
            coinText.text = "Coins: " + coinCounter;
        }
    }


}
