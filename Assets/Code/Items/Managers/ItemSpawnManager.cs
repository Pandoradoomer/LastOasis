using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject collectablePrefab;
    [SerializeField]
    GameObject consumablePrefab;

    //TODO: Add Spawn for ConsumableData as well
    public void Spawn(CollectableData data, Transform t, int amount)
    {
        var go = Instantiate(collectablePrefab, t.position, Quaternion.identity);

        //Set Sprite Info
        SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
        sr.sprite = data.Sprite;
        //Set Collectable info
        Collectable coll = go.GetComponent<Collectable>();
        coll.stackSize = amount;
        coll.SetCollectableData(data);
    }
}
