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
        EventManager.Instance.EnemyDestroyed += OnEnemyDestroyed;
        EventManager.Instance.RoomEnter += OnRoomEnter;
        EventManager.Instance.RoomExit += OnRoomExit;
        spawnedEnemies = new Dictionary<int, List<GameObject>>();
        hasSpawned= new List<int>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnRoomEnter(EnemySpawnPacket e)
    {
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

    private void OnRoomExit(int index)
    {
        DisableEnemies(index);
    }

    private void OnEnemyDestroyed(GameObject o)
    {
        int index = o.GetComponent<EnemyBase>().roomIndex;
        spawnedEnemies[index].Remove(o);
        if (spawnedEnemies[index].Count == 0)
            spawnedEnemies.Remove(index);
        Destroy(o);
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

            //Setting the colour
            go.GetComponent<SpriteRenderer>().color = enemies[index].color;

            //Add behaviour;
            var behaviour = enemies[index].Behaviour;
            go.AddComponent(behaviour.GetClass());

            //setting the index;
            go.GetComponent<EnemyBase>().roomIndex = e.roomIndex;
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
