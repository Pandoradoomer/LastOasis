using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandboxScript : MonoBehaviour
{
    //public GameObject enemyPrefab;
    public GameObject[] enemies;
    public RoomScript roomScript;

    [SerializeField]
    int roomIndex = 1;
    [SerializeField]
    float roomDifficulty = 1;
    [SerializeField]
    int roomDistanceToStart = 1;
    [SerializeField]
    float onCollisionDamage = 1;
    [SerializeField]
    float attackDamage = 1;
    [SerializeField]
    float multiplier = 1;
    [SerializeField]
    bool enemyDelayedSpawn = false; //press 'i' to trigger a delayed spawn

    void Awake()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach(GameObject enemy in enemies)
        {
            if(enemy.GetComponent<EnemyBase>() != null)
            {
                //Debug.Log(enemy);
                enemy.GetComponent<EnemyBase>().roomIndex = roomIndex;
                enemy.GetComponent<EnemyBase>().onCollisionDamage = onCollisionDamage;
                enemy.GetComponent<EnemyBase>().attackDamage = attackDamage;
                enemy.GetComponent<EnemyBase>().multiplier = multiplier;
                enemy.GetComponent<EnemyBase>().rs = roomScript;
                if (enemyDelayedSpawn)
                {
                    enemy.SetActive(false);
                }
            }            
        }
        roomScript.roomIndex = roomIndex;
        roomScript.roomDifficulty = roomDifficulty;
        roomScript.distToStart = roomDistanceToStart;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            foreach (GameObject enemy in enemies)
            {
                if (enemy.GetComponent<EnemyBase>() != null)
                {
                    enemy.SetActive(true);
                }
            }
        }
    }
}
