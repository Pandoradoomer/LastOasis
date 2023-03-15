using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextManager : MonoBehaviour
{
    [SerializeField]
    private GameObject textPrefab;
    // Start is called before the first frame update
    private List<GameObject> pooledObjects;
    [SerializeField]
    private int amountToPool;

    private void Awake()
    {
        pooledObjects = new List<GameObject>();
        GameObject tmp;
        for(int i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(textPrefab);
            tmp.SetActive(false);
            tmp.transform.parent = this.transform;
            pooledObjects.Add(tmp);
        }
    }

    private GameObject GetPooledObject()
    {
        for(int i = 0; i < amountToPool; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
                return pooledObjects[i];
        }
        return null;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
