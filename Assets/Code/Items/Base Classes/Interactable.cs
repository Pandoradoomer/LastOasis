using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Interactable : MonoBehaviour
{
    public bool playerInRange;
    public KeyCode chestOpenKey;
    public UnityEvent interactAction;
   // public event <Action> interactInvoked;
    //private MessageManager messages;
    public GameObject coinPrefab;
    public GameObject coinPilePrefab;
    public GameObject coinBagPrefab;
    //TODO Dylan: modify this so that it holds a list of all items to spawn, alongside their amounts
    //You'll need to do something like the way enemies select which items to drop.
    [SerializeField]
    private CollectableData itemToSpawn;
    void Start()
    {
       // messages = GetComponent<MessageManager>();

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
            MessageManager.instance.DisplayChestText();
           // messages.DisplayChestText();
            Debug.Log("Player in range");
        }
        //Notifies user theyve already interacted with this chest 
        if (collision.gameObject.CompareTag("Player") && ChestControl.instance.isOpen)
        {
            playerInRange = true;
            //messages.DisableChestText();
            MessageManager.instance.DisableChestText();
            MessageManager.instance.DisplayChestInteractText();
            //messages.DisplayChestInteractText();
            Debug.Log("Player in range");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            //When player is out of range, disable canvas text elements
            playerInRange = false;
            //messages.DisableChestText();
            MessageManager.instance.DisableChestText();
            MessageManager.instance.DisableChestInteractText();

            //messages.DisableChestInteractText();
            Debug.Log("Player not in range");
        }
    }

    public void SpawnCoinsFromChest()
    {
        if (playerInRange && !ChestControl.instance.isOpen)
        {
            if (Input.GetKeyDown(chestOpenKey))
            {   
                interactAction?.Invoke();
                ChestControl.instance.OpenChest();
                ChestControl.instance.maxNumberCoins = Random.Range(0, ChestControl.instance.maxNumberCoins);
                for (int i = 0; i < ChestControl.instance.maxNumberCoins; i++)
                {
                    Transform t = transform;
                    Vector3 r = new Vector3(Random.Range(-2.5f, 1.5f), Random.Range(-2.0f, 1.0f));
                    t.position += r;
                    Singleton.Instance.ItemSpawnManager.Spawn(itemToSpawn, t, Random.Range(1, 10));
                    t.position -= r;
                    //var go = Instantiate(coinPrefab, new Vector3(Random.Range(-2.5f,1.0f), Random.Range(-2.0f,1.0f), -1), transform.rotation);
                    //go.GetComponent<Collectable>().stackSize = Random.Range(1, 10);
                }
                /// <summary>
                /// Checks if player is in range and hasnt been interacted with
                /// Key input E to invoke an event
                /// Gets reference to the chest control function
                /// Instantiates coin prefab at a fixed position in scene from the chest
                /// Spawn random number of coins depending on the Inspector value assigned
                /// </summary>

            }
        }
    }
}
