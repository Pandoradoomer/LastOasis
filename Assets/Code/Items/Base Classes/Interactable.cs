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
    //Prefab list
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
        if (playerInRange && !open && InteractWithNoEnemies())
        {
            if (Input.GetKeyDown(chestOpenKey))
            {   
                interactAction?.Invoke();
                Chest.OpenChest();

                //Set random range value of coins to corresponding variable
                int easyCoins = Random.Range(1, Chest.maxNumberCoins/4);
                int mediumCoins = Random.Range(5, Chest.maxNumberCoins/2);
                int hardCoins = Random.Range(10, Chest.maxNumberCoins);
                //Loop through number of coins and spawn the random value
                for (int i = 0; i < easyCoins; i++)
                {
                    if (GetDifficulty() <= 1.5)
                    {
                        Transform t = transform;
                        Vector3 r = new Vector3(Random.Range(-2.5f, 1.5f), Random.Range(-2.0f, 1.0f));
                        t.position += r;
                        Singleton.Instance.ItemSpawnManager.Spawn(itemToSpawn, t, easyCoins);
                        t.position -= r;
                    }
                }
                for (int i =0; i < mediumCoins; i++)
                {
                    if (GetDifficulty() <= 3 && GetDifficulty() > 1.5)
                    {
                        Transform t = transform;
                        Vector3 r = new Vector3(Random.Range(-2.5f, 1.5f), Random.Range(-2.0f, 1.0f));
                        t.position += r;
                        Singleton.Instance.ItemSpawnManager.Spawn(itemToSpawn, t, mediumCoins);
                        t.position -= r;
                    }
                }
                //Hardest difficulty loops through 10-20 coins and spawns the range as long as the float difficulty meets the condition
                for (int i =0; i < hardCoins;i++)
                {
                    if (GetDifficulty() <= 6 && GetDifficulty() > 3)
                    {
                        Transform t = transform;
                        Vector3 r = new Vector3(Random.Range(-2.5f, 1.5f), Random.Range(-2.0f, 1.0f));
                        t.position += r;
                        Singleton.Instance.ItemSpawnManager.Spawn(itemToSpawn, t, hardCoins);
                        t.position -= r;
                    }
                }




                    //var go = Instantiate(coinPrefab, new Vector3(Random.Range(-2.5f,1.0f), Random.Range(-2.0f,1.0f), -1), transform.rotation);
                    //go.GetComponent<Collectable>().stackSize = Random.Range(1, 10);
               // }
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

    private float GetDifficulty()
    {
        //Get current room index of the chest, store it in variable
        int curRoom = gameObject.GetComponent<ChestControl>().roomIndex;
        //Find all objects with roomscript at runtime and store them in an array
        RoomScript[] rooms = FindObjectsOfType<RoomScript>();
        RoomScript room = null;
        //Iterate through number of rooms
        for (int i = 0; i < rooms.Length; i++)
        {
            //Set the current index of the arrays room index equal to the chest index
            if (rooms[i].roomIndex == curRoom)
            {
                room = rooms[i];
                break;
            }


        }
        //Store the rooms difficulty the chest is in
        float totalDif = room.roomDifficulty;
        Debug.Log("Total dif" + totalDif);
        Debug.Log("Index" + curRoom);
        //Return the float so it can be called
        return totalDif;

        //Total their difficulty = sum of enemies spawned * enemy difficulty => Room Difficulty

    }

    private bool InteractWithNoEnemies()
    {
       
        //Get list of spawned enemies in the current room room
        bool canInteract = true;
        //Get room index of chest 
        int curRoom = gameObject.GetComponent<ChestControl>().roomIndex;
        //Get room index off enemybase
        EnemyBase[] enemyIndex = GameObject.FindObjectsOfType<EnemyBase>();
        EnemyBase enemy = null;

        Dictionary<int,List<EnemyRuntimeData>> enemiesInCurRoom = Singleton.Instance.EnemyManager.spawnedEnemies;
        var enemyGet = enemiesInCurRoom.ContainsKey(curRoom);
        int enemiesInRoom = 0;
        for (int i = 0; i < enemyIndex.Length; i++)
        {
            if (curRoom == enemyIndex[i].roomIndex)
            {
                enemy = enemyIndex[i];
                enemyGet = enemyIndex[i];
                enemiesInRoom++;
            }
        }
        //Checks for enemies in the room Index
        int newIndex = enemy.roomIndex;
        Debug.Log("Number of enemies in room" + " { " + newIndex + " } " + " is " + " { " + enemiesInRoom + " } " );
        //Return canInteract based on condition, prevent interaction while enemy count is over 0
        while (enemiesInRoom > 0)
        {
            return !canInteract;
        }

        return canInteract;

        //FIX BUG OF INTERACTING WITH CHEST IF NO ENEMIES SPAWN IN THE ROOM BUT A CHEST DOES
    }
}
