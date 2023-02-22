using Newtonsoft.Json.Schema;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;



public class EnemyManager : MonoBehaviour
{
    //TODO: add to singleton;
    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private List<Enemy> enemies;

    public Dictionary<int, List<EnemyRuntimeData>> spawnedEnemies;
    private List<int> hasSpawned;



    // Start is called before the first frame update
    void Start()
    {
        EventManager.StartListening(Event.EnemyDestroyed, OnEnemyDestroyed);
        EventManager.StartListening(Event.RoomEnter, OnRoomEnter);
        EventManager.StartListening(Event.RoomExit, OnRoomExit);
        spawnedEnemies = new Dictionary<int, List<EnemyRuntimeData>>();
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
        spawnedEnemies[index].RemoveAll(x => x.go == edp.go);
        if (spawnedEnemies[index].Count == 0)
            spawnedEnemies.Remove(index);

        //awarding the player the necessary items
        foreach(var kvp in edp.lootToDrop)
        {
            if(kvp.Key is CollectableData)
            {
                Singleton.Instance.ItemSpawnManager.Spawn(kvp.Key as CollectableData, edp.go.transform, kvp.Value);
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
            IEnemyBehaviour behaviourComponent = enemy.go.GetComponent<IEnemyBehaviour>();
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
            IEnemyBehaviour behaviourComponent = enemy.go.GetComponent<IEnemyBehaviour>();
            enemy.go.transform.position = enemy.SpawnPos;
            enemy.go.GetComponent<EnemyBase>().currentHealth = enemy.maxHealth;
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
        List<Vector2> filledPositions = new List<Vector2>();
        while(currentDifficulty < e.difficulty)
        {
            Vector2 pos = Vector2.zero;
            pos = DecideEnemySpawnPosition(e.enemyPositions, filledPositions);

            int index = Random.Range(0, enemies.Count);
            Enemy enemyData = enemies[index];


            var go = Instantiate(enemyData.prefabToSpawn, e.roomCentre + pos, Quaternion.identity);

            EnemyRuntimeData erd = new EnemyRuntimeData()
            {
                go = go,
                SpawnPos = e.roomCentre + pos,
                maxHealth = enemyData.MaxHealth
            };
            //Setting the colour
            go.GetComponent<SpriteRenderer>().color = enemyData.color;

            //Add behaviour;
            //var behaviour = enemyData.Behaviour;
            //go.AddComponent(behaviour.GetClass());

            //setting the index;
            EnemyBase eb = go.GetComponent<EnemyBase>();
            eb.roomIndex = e.roomIndex;
            eb.currentHealth = enemyData.MaxHealth;
            eb.attackDamage = enemyData.Damage;
            foreach(ItemDrop id in enemyData.itemDrops)
            {
                float random = Random.Range(0.0f, 1.0f);
                if(random <= id.dropProbability)
                {
                    eb.lootToDrop.Add(id.itemType, Random.Range(id.minItemQuantity, id.maxItemQuantity + 1));
                }
            }
            AddEnemyToDictionary(erd, e.roomIndex);
            Debug.Log("Spawned enemy");
            //increasing the difficulty
            currentDifficulty += enemies[index].difficulty;
        }
    }
    
    Vector2 DecideEnemySpawnPosition(EnemySpawnPosition esp, List<Vector2> takenPositions)
    {
        List<Vector2> buffer = new List<Vector2>(esp.enemyPositions);
        buffer.RemoveAll(x => takenPositions.Contains(x));
        while(buffer.Count == 0)
        {
            buffer = new List<Vector2>(esp.enemyPositions);
            int mult = (takenPositions.Count / esp.enemyPositions.Count) % 2;
            int inc = (takenPositions.Count / esp.enemyPositions.Count);
            if (mult == 0)
                mult--;
            for(int i = 0; i < buffer.Count; i++)
            {
                buffer[i] += Vector2.left * mult * inc * 0.5f;
            }

            buffer.RemoveAll(x => takenPositions.Contains(x));
            

        }
        int index = Random.Range(0, buffer.Count);
        takenPositions.Add(buffer[index]);
        return buffer[index];
    }

    void AddEnemyToDictionary(EnemyRuntimeData erd, int index)
    {
        if(spawnedEnemies.ContainsKey(index))
        {
            spawnedEnemies[index].Add(erd);
        }
        else
        {
            spawnedEnemies.Add(index, new List<EnemyRuntimeData>());
            spawnedEnemies[index].Add(erd);
        }
    }
}

public class EnemyRuntimeData
{
    public GameObject go;
    public Vector2 SpawnPos;
    public float maxHealth;
}