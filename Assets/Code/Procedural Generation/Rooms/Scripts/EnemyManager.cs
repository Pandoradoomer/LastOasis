using Newtonsoft.Json.Schema;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using Unity.VisualScripting;
using static UnityEditor.PlayerSettings;

public class EnemyManager : MonoBehaviour
{
    //TODO: add to singleton;
    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private List<Enemy> enemies;

    public Dictionary<int, List<EnemyRuntimeData>> spawnedEnemies;
    private List<int> hasSpawned;

    [Header("Debug")]
    public bool SpawnOnStart = false;


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
        if(Input.GetKeyDown(KeyCode.Keypad1))
        {
            List<int> keys = new List<int>(spawnedEnemies.Keys);
            foreach(int key in keys)
            {
                var auxList = new List<EnemyRuntimeData>(spawnedEnemies[key]);
                for(int i = 0; i < auxList.Count; i++)
                {
                    var erd = auxList[i];
                    //Destroy(erd.go);
                    EventManager.TriggerEvent(Event.EnemyDestroyed, new EnemyDestroyedPacket()
                    {
                        go = erd.go,
                        lootToDrop = erd.go.GetComponent<EnemyBase>().lootToDrop
                    });
                }
                spawnedEnemies.Remove(key);
                EventManager.TriggerEvent(Event.DoorsLockUnlock, new DoorLockUnlockPacket()
                {
                    roomIndex = Singleton.Instance.LevelGeneration.roomIndex,
                    isUnlock = true
                });
                EventManager.TriggerEvent(Event.SpawnObelisk, null);
            }

        }
    }


    private void OnRoomEnter(IEventPacket packet)
    {
        EnemySpawnPacket e = packet as EnemySpawnPacket;
        if(!hasSpawned.Contains(e.roomIndex))
        {
            hasSpawned.Add(e.roomIndex);
            SpawnEnemies(e);
            EventManager.TriggerEvent(Event.DoorsLockUnlock, new DoorLockUnlockPacket()
            {
                roomIndex = e.roomIndex,
                isUnlock = false
            });
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
        {
            spawnedEnemies.Remove(index);
            EventManager.TriggerEvent(Event.DoorsLockUnlock, new DoorLockUnlockPacket()
            {
                roomIndex = index,
                isUnlock = true
            });
            EventManager.TriggerEvent(Event.SpawnObelisk, null);
        }

        //awarding the player the necessary items
        RoomScript rs = edp.go.transform.root.GetComponent<RoomScript>();
        foreach(var kvp in edp.lootToDrop)
        {
            if(kvp.Key is CollectableData)
            {
                CollectableData cd = kvp.Key as CollectableData;
                if(cd.isCoin)
                {
                    Singleton.Instance.Inventory.AddCoins(kvp.Value);
                }
                else
                {
                    var go = Singleton.Instance.ItemSpawnManager.SpawnItem(kvp.Key, edp.go.transform, kvp.Value);
                    rs.AddtoSpawnedList(go);
                }
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
            enemy.go.SetActive(true);
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
                behaviourComponent.ResetEnemy();
            }
            enemy.go.SetActive(false);
        }
    }

    private void SpawnEnemies(EnemySpawnPacket e)
    {
        if (e.isBoss)
            return;
        if(!SpawnOnStart)
            if (e.isStart)
                return;
        //for the sake of the task, let's spawn enemies inside every room, at least 1
        float currentDifficulty = 0;
        List<Vector2> filledPositions = new List<Vector2>();
        GameObject room = Singleton.Instance.LevelGeneration.GetRoomFromIndex(e.roomIndex);
        for(int i = 0; i < e.enemyPositions.enemySpawns.Count; i++)
        {
            EnemySpawn es = e.enemyPositions.enemySpawns[i];
            var availableEnemies = enemies.FindAll(x => x.difficulty <= es.difficultyRange.y && x.difficulty >= es.difficultyRange.x);
            if (availableEnemies.Count == 0)
            {
                Debug.LogError($"Couldn't find enemy to spawn at spawn point {i} in room {room.name}");
                continue;
            }
            Enemy enemyData = availableEnemies[Random.Range(0, availableEnemies.Count)];
            var go = Instantiate(enemyData.prefabToSpawn, e.roomCentre + es.spawnPosition, Quaternion.identity);
            go.transform.parent = room.transform;

            EnemyRuntimeData erd = new EnemyRuntimeData()
            {
                go = go,
                SpawnPos = e.roomCentre + es.spawnPosition,
                maxHealth = enemyData.MaxHealth
            }; 
            
            //go.GetComponent<SpriteRenderer>().color = enemyData.color;

            //setting the index;
            EnemyBase eb = go.GetComponent<EnemyBase>();
            eb.roomIndex = e.roomIndex;
            eb.currentHealth = enemyData.MaxHealth;
            eb.attackDamage = enemyData.Damage;
            eb.multiplier = enemyData.multiplier;
            eb.enemyData = enemyData;
            eb.onCollisionDamage = enemyData.DamageOnCollision;
            eb.rs = room.GetComponent<RoomScript>();
            eb.rs.enemies.Add(eb);
            eb.GetComponent<SpriteRenderer>().sprite = enemyData.Sprite;
            foreach (ItemDrop id in enemyData.itemDrops)
            {
                float random = Random.Range(0.0f, 1.0f);
                if (random <= id.dropProbability)
                {
                    eb.lootToDrop.Add(id.itemType, Random.Range(id.minItemQuantity, id.maxItemQuantity + 1));
                }
            }
            AddEnemyToDictionary(erd, e.roomIndex);
        }
    }
    
    Vector2 DecideEnemySpawnPosition(EnemySpawnPosition esp, List<Vector2> takenPositions)
    {
        List<EnemySpawn> buffer = new List<EnemySpawn>(esp.enemySpawns);
        buffer.RemoveAll(x => takenPositions.Contains(x.spawnPosition));
        while(buffer.Count == 0)
        {
            buffer = new List<EnemySpawn>(esp.enemySpawns);
            int mult = (takenPositions.Count / esp.enemySpawns.Count) % 2;
            int inc = (takenPositions.Count / esp.enemySpawns.Count);
            if (mult == 0)
                mult--;
            for(int i = 0; i < buffer.Count; i++)
            {
                buffer[i].spawnPosition += Vector2.left * mult * inc * 0.5f;
            }

            buffer.RemoveAll(x => takenPositions.Contains(x.spawnPosition));

        }
        int index = Random.Range(0, buffer.Count);
        takenPositions.Add(buffer[index].spawnPosition);
        return buffer[index].spawnPosition;
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