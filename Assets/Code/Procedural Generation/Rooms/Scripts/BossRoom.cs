using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom : MonoBehaviour
{
    public static BossRoom instance;
    private LevelGeneration levelGen;
    public GameObject bossCollidersPrefab;
    public GameObject bossPrefab;
    private GameObject boss;
    private GameObject bossRoom;

    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(instance);
    }
    private void Start()
    {
        levelGen = GetComponent<LevelGeneration>();
        bossRoom = Instantiate(bossCollidersPrefab, levelGen.bossRoom.transform.position, Quaternion.identity);
        bossRoom.transform.parent = levelGen.bossRoom.transform;
    }

    private void LateUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Destroy(bossRoom);
            Destroy(boss);
            bossRoom = Instantiate(bossCollidersPrefab, levelGen.bossRoom.transform.position, Quaternion.identity);
            bossRoom.transform.parent = levelGen.bossRoom.transform;
        }
    }

    public void SpawnBoss()
    {
        boss = Instantiate(bossPrefab, levelGen.bossRoom.transform.position, Quaternion.identity);
        boss.transform.position = new Vector2(boss.transform.position.x, boss.transform.position.y + 2);
    }
}
