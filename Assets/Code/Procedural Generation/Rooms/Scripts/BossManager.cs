using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BossManager : MonoBehaviour
{
    public static BossManager instance;
    [SerializeField] private BoxCollider2D roomTrigger;
    [SerializeField] private GameObject bossPrefab;
    
    public Enemy boss;
    public GameObject bossHPSlider;

    private List<GameObject> spawnedEnemies = new List<GameObject>();
    private bool hasSpawned = false;
    private bool isBossRoom = false;
    // Start is called before the first frame update
    void Start()
    {
        EventManager.Instance.EnemyDestroyed += OnEnemyDestroyed;

        if (gameObject.name == "BossRoom")
        {
            isBossRoom = true;
            bossHPSlider = GameObject.Find("BossHP");
            bossHPSlider.SetActive(false);
            instance = this;
        }
        else
        {
            Destroy(this);
        }
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
                    SpawnBoss();
                }
            }
        }
    }

    private void OnEnemyDestroyed(GameObject o)
    {
        spawnedEnemies.Remove(o);
        Destroy(o);
    }

    private void SpawnBoss()
    {
        GameObject go = Instantiate(bossPrefab, transform.position, Quaternion.identity);
        var behaviour = boss.Behaviour;
        go.AddComponent(behaviour.GetClass());

        bossHPSlider.SetActive(true);
    }

    public void BossHP()
    {
        if(bossHPSlider.activeSelf)
            bossHPSlider.GetComponent<Slider>().value = boss.CurrentHealth;
    }
}
