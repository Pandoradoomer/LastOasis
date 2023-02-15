using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Interactable : MonoBehaviour
{
    public bool playerInRange;
    public KeyCode chestOpenKey;
    public UnityEvent interactAction;
    public GameObject coinPrefab;

    private MessageManager messages;


    void Start()
    {
        messages = GetComponent<MessageManager>();
    }
<<<<<<< Updated upstream:Assets/Code/Items/Base Classes/Interactable.cs
    //TODO Dylan: Use the in-house Event System not the in-built one

=======
>>>>>>> Stashed changes:Assets/Interactable.cs
    void Update()
    {
        SpawnCoinsFromChest();
    }

    //************Chest Interaction*********
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Display prompt to open chest if not done so already
        if(collision.gameObject.CompareTag("Player") && !ChestControl.instance.isOpen)
        {
            playerInRange = true;
            messages.DisplayChestText();
            Debug.Log("Player in range");
        }
        //If chest has been interacted with, prompt player that its no longer useable
        if (collision.gameObject.CompareTag("Player") && ChestControl.instance.isOpen)
        {
            playerInRange = true;
            messages.DisableChestText();
            messages.DisplayChestStateInteract();
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            //When player is out of range, disable the canvas text elements
            playerInRange = false;
            messages.DisableChestText();
            messages.DisableChestStateInteract();
            Debug.Log("Player not in range");
        }


    }

    void SpawnCoinsFromChest()
    {
        //If player can interact with chest and it hasnt been opened
        if (playerInRange && !ChestControl.instance.isOpen)
        {
            if (Input.GetKeyDown(chestOpenKey))
            {
                //Invoke the 'E' interaction 
                interactAction.Invoke();
                //Get reference to openchest function
                ChestControl.instance.OpenChest();
                GameObject newObj;
                //Spawn coin at fixed Pos
                newObj = Instantiate(coinPrefab, new Vector3(-2.5f, 2.0f, -1.0f), transform.rotation);
                //Instantiate coins from a chest storage script
            }
        }
    }
}
