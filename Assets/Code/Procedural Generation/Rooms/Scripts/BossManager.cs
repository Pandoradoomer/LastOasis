using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    [SerializeField] private BoxCollider2D roomTrigger;
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private Enemy boss;

    private List<GameObject> spawnedEnemies = new List<GameObject>();
    private bool hasSpawned = false;
    private bool isBossRoom = false;
    // Start is called before the first frame update
    void Start()
    {
        EventManager.Instance.EnemyDestroyed += OnEnemyDestroyed;
        if (gameObject.name == "BossRoom")
            isBossRoom = true;
        else
            this.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && isBossRoom)
        {
            if (roomTrigger != null)
            {
                if (!hasSpawned)
                {
                    hasSpawned = true;
                    SpawnEnemies();
                }
            }
        }
    }

    private void OnEnemyDestroyed(GameObject o)
    {
        spawnedEnemies.Remove(o);
        Destroy(o);
    }

    private void SpawnEnemies()
    {
        GameObject go = Instantiate(bossPrefab, transform.position, Quaternion.identity);
        var behaviour = boss.Behaviour;
        go.AddComponent(behaviour.GetClass());
    }
}
