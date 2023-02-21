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
    public GameObject coinPrefab;
    public GameObject coinPilePrefab;
    public GameObject coinBagPrefab;
    //TODO Dylan: modify this so that it holds a list of all items to spawn, alongside their amounts
    //You'll need to do something like the way enemies select which items to drop.
    [SerializeField]
    private CollectableData itemToSpawn;

    
    void Start()
    {

    }
    //TODO Dylan: Use the in-house Event System not the in-built one

    void Update()
    {
        SpawnCoinsFromChest();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var open = gameObject.GetComponent<ChestControl>().isOpen;
        //Prompts player with open message if in range and chest hasnt been opened
        if (collision.gameObject.CompareTag("Player") && !open)
        {
            playerInRange = true;
            MessageManager.instance.DisplayChestText();
        }
        //Notifies user theyve already interacted with this chest 
        if (collision.gameObject.CompareTag("Player") && open)
        {
            playerInRange = true;
            MessageManager.instance.DisableChestText();
            MessageManager.instance.DisplayChestInteractText();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            //When player is out of range, disable canvas text elements
            playerInRange = false;
            MessageManager.instance.DisableChestText();
            MessageManager.instance.DisableChestInteractText();

        }
    }

    public void SpawnCoinsFromChest()
    {
        var open = gameObject.GetComponent<ChestControl>().isOpen;
        var Chest = gameObject.GetComponent<ChestControl>();

        if (playerInRange && !open)
        {
            if (Input.GetKeyDown(chestOpenKey))
            {   
                interactAction?.Invoke();
               // ChestControl.instance.OpenChest();
                Chest.OpenChest();
                //open.Op
                Chest.maxNumberCoins = Random.Range(0, Chest.maxNumberCoins);
                GetDifficulty();
                for (int i = 0; i < Chest.maxNumberCoins; i++)
                {
                    //Get room difficulty from script, clamp item cap based on condition of difficulty
                    //If difficulty is above 10 spawn a coin bag in chest
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

    private void GetDifficulty()
    {
        //Get room difficulty of current room index
        float roomDif = Singleton.Instance.RoomScript.roomDifficulty;
        //get difficulty from enemyspawnpacket

        //Get room index from ChestManager script
        int currentRoom = Singleton.Instance.RoomScript.roomIndex.GetHashCode();
        //Get the variables from the current room youre in
        Debug.Log("Current difficulty " + roomDif);
        Debug.Log("Current room " + currentRoom);

        //Get number of Enemy type 1's, Get number of enemy type 2's in room
        int numberOfEnemies = Singleton.Instance.EnemyManager.spawnedEnemies.Count;
        //float difficulty = Singleton.Instance.Enemy.difficulty;
        Debug.Log("number " + numberOfEnemies);
        //Debug.Log("difficulty " + difficulty);


        //Total their difficulty = sum of enemies spawned * enemy difficulty

    }
}
