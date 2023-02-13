using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private BoxCollider2D roomTrigger;
    //TODO: add to singleton;
    [SerializeField]
    private GameObject enemyPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(roomTrigger != null)
        {
            SpawnEnemies();
            roomTrigger.enabled = false;
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
            Instantiate(enemyPrefab, transform.position + pos, Quaternion.identity);

        }
    }
}
