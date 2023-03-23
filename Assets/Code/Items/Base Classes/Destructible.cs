using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    public Dictionary<Item, int> lootToDrop;
    private RoomScript rs;
    [SerializeField]
    GameObject _coinSpawner;
    private void Awake()
    {
        rs = transform.root.gameObject.GetComponent<RoomScript>();
        lootToDrop = new Dictionary<Item, int>();
    }
    void Start()
    {
        EventManager.StartListening(Event.PlayerHitEnemy, OnHit);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddHealth()
    {
        //Debug.Log($"Health added to {gameObject.name} in Room {transform.root.gameObject.name}");
        lootToDrop.Add(Singleton.Instance.ItemSpawnManager.healthPotion, 1);
    }

    public void AddCoin(int value)
    {
        //Debug.Log($"Coin added to {gameObject.name} in Room {transform.root.gameObject.name}");
        lootToDrop.Add(Singleton.Instance.ItemSpawnManager.coinData, value);
    }

    private void OnHit(IEventPacket packet)
    {
        PlayerHitPacket php = packet as PlayerHitPacket;
        if(php.enemy == this.gameObject)
            Destroy(this.gameObject);
    }
    private void OnDestroy()
    {
        if(gameObject.scene.isLoaded)
        {
            foreach (var kvp in lootToDrop)
            {
                if(kvp.Key is CollectableData)
                {
                    var collect = kvp.Key as CollectableData;
                    if(collect.isCoin)
                    {
                        Singleton.Instance.Inventory.AddCoins(kvp.Value);
                        Instantiate(_coinSpawner, this.transform.position, Quaternion.identity);
                    }
                }
                else
                {
                    var go = Singleton.Instance.ItemSpawnManager.SpawnItem(kvp.Key, this.transform, kvp.Value);
                    rs.AddtoSpawnedList(go);
                }
            }
            rs.RemoveDestructibleFromList(this);
        }
        EventManager.StopListening(Event.PlayerHitEnemy, OnHit);
    }


}
