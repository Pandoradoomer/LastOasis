using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Interactable : MonoBehaviour
{
    public bool playerInRange;
    public KeyCode chestOpenKey;
    public UnityEvent interactAction;
    private MessageManager messages;
    public GameObject coinPrefab;

    void Start()
    {
        messages = GetComponent<MessageManager>();

    }
    //TODO Dylan: Use the in-house Event System not the in-built one

    void Update()
    {
        SpawnCoinsFromChest();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && !ChestControl.instance.isOpen)
        {
            playerInRange = true;
            messages.DisplayChestText();
            Debug.Log("Player in range");
        }
        if (collision.gameObject.CompareTag("Player") && ChestControl.instance.isOpen)
        {
            playerInRange = true;
            messages.DisableChestText();
            messages.DisplayChestInteractText();
            Debug.Log("Player in range");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
            messages.DisableChestText();
            messages.DisableChestInteractText();
            Debug.Log("Player not in range");
        }
    }

    public void SpawnCoinsFromChest()
    {
        if (playerInRange && !ChestControl.instance.isOpen)
        {
            if (Input.GetKeyDown(chestOpenKey))
            {
                interactAction.Invoke();
                ChestControl.instance.OpenChest();
                GameObject newCoin;
                newCoin = Instantiate(coinPrefab, new Vector3(-2.5f, -2.0f, -1.0f), transform.rotation);

            }
        }
    }
}
