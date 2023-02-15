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
        //Prompts player with open message if in range and chest hasnt been opened
        if(collision.gameObject.CompareTag("Player") && !ChestControl.instance.isOpen)
        {
            playerInRange = true;
            messages.DisplayChestText();
            Debug.Log("Player in range");
        }
        //Notifies user theyve already interacted with this chest 
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
            //When player is out of range, disable canvas text elements
            playerInRange = false;
            messages.DisableChestText();
            messages.DisableChestInteractText();
            Debug.Log("Player not in range");
        }
    }

    public void SpawnCoinsFromChest()
    {
        //Checks if player is in range and hasnt been interacted with
        if (playerInRange && !ChestControl.instance.isOpen)
        {
            if (Input.GetKeyDown(chestOpenKey))
            {   
                //Key input E to invoke an event
                interactAction.Invoke();
                //Gets reference to the chest control function
                ChestControl.instance.OpenChest();
                //Instantiates coin prefab at a fixed position in scene from the chest
                GameObject newCoin;
                newCoin = Instantiate(coinPrefab, new Vector3(-2.5f, -2.0f, -1.0f), transform.rotation);

            }
        }
    }
}
