using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    
    //Inventory items in list:
    //Collectable (scriptable object)
    public List<Collectable> inventory = new List<Collectable>();
    //Dictionary handles item stacking from List
    private Dictionary<CollectableData, Collectable> itemDictionary = new Dictionary<CollectableData, Collectable>();
    public static Inventory instance { get; private set; }
    //Singleton class for Inventory
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    public void Add(CollectableData collectableData)
    {
        //CAP ITEM STACK TO 1
        if (itemDictionary.TryGetValue(collectableData, out Collectable item))      //Checks if item is already existing in Dictionary, returns null if it doesnt find
        {
            item.AddToStack();                                                      //If we get the value of collectableData and we find an item, then add it to the stack
            Debug.Log(collectableData.name + "(s)" + " in stack " + item.stackSize);
            //Adds collectable name to item stack and displays stack size
        }
        else
        {
            Collectable newCollectable = new Collectable(collectableData);              //If it doesnt exist in inventory, create a new collectable
            inventory.Add(newCollectable);                                              //Add the newly found collectable to the List
            itemDictionary.Add(collectableData, newCollectable);                        //Store this collectable along with its data in the dictionary
            Debug.Log(collectableData.name + " added to inv for first time");
        }
    }

    //Check for Collisions with enemies and remove items
    public void Remove(CollectableData collectableData)
    {
        if (itemDictionary.TryGetValue(collectableData, out Collectable item))          //Finds a collectable in the item dictionary (queries it)
        {
            item.RemoveFromStack();                                                     //Removes item from collectable stack
            Debug.Log(/*item.stackSize +*/ collectableData.name + " removed from stack");
            if (item.stackSize == 0)                                    //Remove collectable from the collectable list (inventory) and the dictionary, Once the stack is empty enter the if
            {
                inventory.Remove(item);                                 //Remove item from inv list
                itemDictionary.Remove(collectableData);                 //Remove collectableData from dict
                //REMOVE 1 ITEM AT A TIME

            }
            else if (item.stackSize == 0)
            {
                Debug.Log("You dont have any items to remove");
            }
        }
    }
    public void ClearInventory(CollectableData collectableData)
    {   //Clear inv when player dies in dungeon, keep items at save points
        //NEEDS MAKING MORE ROBUST, CLEAR MORE THAN ONE TYPE OF COLLECTABLE
        if (itemDictionary.TryGetValue(collectableData, out Collectable item))
        {
            if (inventory.Contains(item) && itemDictionary.Count > 0 && collectableData.name == "Coin"/*&& inventory.Count > 0*/)            //Removes Coin collectable both from inventory list and item dictionary
            {
                inventory.Clear();
                itemDictionary.Clear();
                Debug.Log("Cleared" + " " + item.stackSize + " " + collectableData.name + "(s)");
                //Debug.Log("Cleared" + item.stackSize + "items");
            }
            if (inventory.Contains(item) && itemDictionary.Count > 0 && collectableData.name == "Health Potion")
            {
                inventory.Clear();
                itemDictionary.Clear();
                Debug.Log("Cleared" + " " + item.stackSize + " " + collectableData.name + "(s)");
                //Debug.Log("You have no " + collectableData.name + " to remove");       //No items to remove from an empty dictionary
            }
            else
            {
                inventory.Clear();
                itemDictionary.Clear();
                Debug.Log("Cleared" + " " + item.stackSize + " " + collectableData.name + "(s)");
            }
        }


    }

    public void QueryInv(CollectableData collectableData)
    {
        if (itemDictionary.ContainsKey(collectableData))                                    //Checks if dictionary contains collectableData scriptable objects
        {
            Debug.Log("There is a " + collectableData.name);                                //If so then print the name of the CollectableData to console
        }
        else
        {
            Debug.Log("Item " + collectableData.name + " doesnt exist in your inventory");
        }
    }

    public void DisplayInv(CollectableData collectableData)
    {
        //Display in UI
    }

    public bool HasCoin(CollectableData collectableData)
    {
        if (itemDictionary.ContainsKey(collectableData))
        {
            return true;
        }
        return false;
    }


}
