using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private BoxCollider2D roomTrigger;
    //TODO: add to singleton;
    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private List<Enemy> enemies;

    private List<GameObject> spawnedEnemies = new List<GameObject>();
    private bool hasSpawned = false;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.Instance.EnemyDestroyed += OnEnemyDestroyed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CheckEnemies();
        if(collision.gameObject.tag == "Player")
        {
            if (roomTrigger != null)
            {
                if (!hasSpawned)
                {
                    hasSpawned = true;
                    SpawnEnemies();
                }
                else
                {
                    ActivateEnemies();
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        CheckEnemies();
        if(roomTrigger != null && collision.gameObject.tag == "Player")
        {
            DisableEnemies();
        }
    }

    private void OnEnemyDestroyed(GameObject o)
    {
        spawnedEnemies.Remove(o);
        Destroy(o);
    }

    private void CheckEnemies()
    {
    }
    private void ActivateEnemies()
    {
        foreach(var enemy in spawnedEnemies)
        {
            IEnemyBehaviour behaviourComponent = enemy.GetComponent<IEnemyBehaviour>();
            if(behaviourComponent != null )
            {
                var specificComponent = behaviourComponent as MonoBehaviour;
                specificComponent.enabled = true;
            }
        }
    }

    private void DisableEnemies()
    {
        foreach (var enemy in spawnedEnemies)
        {
            IEnemyBehaviour behaviourComponent = enemy.GetComponent<IEnemyBehaviour>();
            if (behaviourComponent != null)
            {
                behaviourComponent.Freeze();
            }
        }
    }

    private void SpawnEnemies()
    {
        //for the sake of the task, let's spawn enemies inside every room, at least 1
        int x = 1;
        x += Random.Range(0, 2);
        for(int i = 0; i < x; i++)
        {
            Vector3 pos = Vector2.zero;
            pos.x = Random.Range(0.0f, 4.0f);
            pos.y = Random.Range(0.0f, 4.0f);
            var go = Instantiate(enemyPrefab, transform.position + pos, Quaternion.identity);

            int index = Random.Range(0, enemies.Count);
            var behaviour = enemies[index].Behaviour;

            go.AddComponent(behaviour.GetClass());
            spawnedEnemies.Add(go);

        }
    }
}
