using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] CollectableData data;
    public int stackSize;                                      //How many of current item stored in inventory
    private Inventory invScript;

    private void Start()
    {
        invScript = FindObjectOfType<Inventory>();          
    }
    private void OnValidate()
    {
        GetComponent<SpriteRenderer>().sprite = data.sprite;
    }

    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);
            invScript.Add(data);                            //Gets a reference to the inventory script Add method, accepts argument of data of type CollectableData

        }
    }
    public Collectable(CollectableData collect)             //Constructor to pass in collectableData value
    {
        data = collect;                                     //Set CollectableData variable "data" to collect
        AddToStack();
    }

    public void AddToStack()
    {

        stackSize++;
    }

    public void RemoveFromStack()
    {
        stackSize--;
    }

    public void Consumeable()
    {
        Destroy(gameObject);
    }
}

