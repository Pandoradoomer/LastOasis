using Newtonsoft.Json.Schema;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyManager : MonoBehaviour
{
    //TODO: add to singleton;
    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private List<Enemy> enemies;

    private Dictionary<int, List<GameObject>> spawnedEnemies;
    private List<int> hasSpawned;



    // Start is called before the first frame update
    void Start()
    {
        EventManager.StartListening(Event.EnemyDestroyed, OnEnemyDestroyed);
        EventManager.StartListening(Event.RoomEnter, OnRoomEnter);
        EventManager.StartListening(Event.RoomExit, OnRoomExit);
        spawnedEnemies = new Dictionary<int, List<GameObject>>();
        hasSpawned= new List<int>();
    }

    private void OnDestroy()
    {
        EventManager.StopListening(Event.EnemyDestroyed, OnEnemyDestroyed);
        EventManager.StopListening(Event.RoomEnter, OnRoomEnter);
        EventManager.StopListening(Event.RoomExit, OnRoomExit);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnRoomEnter(IEventPacket packet)
    {
        EnemySpawnPacket e = packet as EnemySpawnPacket;
        if(hasSpawned.Contains(e.roomIndex))
        {
            ActivateEnemies(e.roomIndex);
        }
        else
        {
            hasSpawned.Add(e.roomIndex);
            SpawnEnemies(e);
        }
    }

    private void OnRoomExit(IEventPacket packet)
    {
        RoomExitPacket rep = packet as RoomExitPacket;
        DisableEnemies(rep.roomIndex);
    }
    private void OnEnemyDestroyed(IEventPacket packet)
    {
        EnemyDestroyedPacket edp = packet as EnemyDestroyedPacket;

        //removing the reference to the enemy in the list of spawned enemies
        int index = edp.go.GetComponent<EnemyBase>().roomIndex;
        spawnedEnemies[index].Remove(edp.go);
        if (spawnedEnemies[index].Count == 0)
            spawnedEnemies.Remove(index);

        //awarding the player the necessary items
        foreach(var kvp in edp.lootToDrop)
        {
            if(kvp.Key is CollectableData)
            {
                Singleton.Instance.Inventory.Add(kvp.Key as CollectableData, kvp.Value);
            }
        }

        Destroy(edp.go);
    }

    private void ActivateEnemies(int index)
    {
        if (spawnedEnemies.ContainsKey(index) == false)
            return;
        foreach(var enemy in spawnedEnemies[index])
        {
            IEnemyBehaviour behaviourComponent = enemy.GetComponent<IEnemyBehaviour>();
                if(behaviourComponent != null )
                {
                    var specificComponent = behaviourComponent as MonoBehaviour;
                    specificComponent.enabled = true;
                }
        }
    }

    private void DisableEnemies(int index)
    {
        if (spawnedEnemies.ContainsKey(index) == false)
            return;
        foreach (var enemy in spawnedEnemies[index])
        {
            IEnemyBehaviour behaviourComponent = enemy.GetComponent<IEnemyBehaviour>();
            if (behaviourComponent != null)
            {
                behaviourComponent.Freeze();
            }
        }
    }

    private void SpawnEnemies(EnemySpawnPacket e)
    {
        if (e.isBoss)
            return;
        //for the sake of the task, let's spawn enemies inside every room, at least 1
        float currentDifficulty = 0;
        while(currentDifficulty < e.difficulty)
        {
            Vector2 pos = Vector2.zero;
            pos.x = Random.Range(-4.0f, 4.0f);
            pos.y = Random.Range(-4.0f, 4.0f);

            var go = Instantiate(enemyPrefab, e.roomCentre + pos, Quaternion.identity);

            int index = Random.Range(0, enemies.Count);
            Enemy enemyData = enemies[index];

            //Setting the colour
            go.GetComponent<SpriteRenderer>().color = enemyData.color;

            //Add behaviour;
            var behaviour = enemyData.Behaviour;
            go.AddComponent(behaviour.GetClass());

            //setting the index;
            EnemyBase eb = go.GetComponent<EnemyBase>();
            eb.roomIndex = e.roomIndex;
            eb.currentHealth = enemyData.MaxHealth;
            foreach(ItemDrop id in enemyData.itemDrops)
            {
                float random = Random.Range(0.0f, 1.0f);
                if(random <= id.dropProbability)
                {
                    eb.lootToDrop.Add(id.itemType, Random.Range(id.minItemQuantity, id.maxItemQuantity + 1));
                }
            }

            AddEnemyToDictionary(go, e.roomIndex);

            //increasing the difficulty
            currentDifficulty += enemies[index].difficulty;
        }
    }

    void AddEnemyToDictionary(GameObject go, int index)
    {
        if(spawnedEnemies.ContainsKey(index))
        {
            spawnedEnemies[index].Add(go);
        }
        else
        {
            spawnedEnemies.Add(index, new List<GameObject>());
            spawnedEnemies[index].Add(go);
        }
    }
}
