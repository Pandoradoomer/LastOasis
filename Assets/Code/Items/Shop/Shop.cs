using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEditor;
using static ItemList;
using UnityEditorInternal.Profiling.Memory.Experimental;

public class Shop : MonoBehaviour
{

    GameObject ItemTemplate;
    GameObject Item;
    [SerializeField] Transform shopList;
    [SerializeField] CollectableData item_health;
    private TextMeshProUGUI cost;
    private TextMeshProUGUI ItemName;
    private Sprite itemImage;
    private Button Buy;
    // Start is called before the first frame update
    void Start()
    {
        ItemTemplate = shopList.GetChild(0).gameObject;
        ListItems();
    }

    void ListItems()
    {
        float itemHeight = 0.0f;
        foreach (itemList item in Enum.GetValues(typeof(itemList)))
        {
            GameObject newItem = Instantiate(ItemTemplate, shopList);
            Debug.Log(item);
            itemImage = ItemList.GetSprite(item);
            int value = ItemList.GetCost(item);
            string name = ItemList.GetName(item);
            ItemDetails(newItem, itemImage,name,value);
            newItem.transform.position = new Vector3(newItem.transform.position.x, newItem.transform.position.y - itemHeight, newItem.transform.position.z);
            itemHeight += 125;
        }
        Destroy(ItemTemplate);
    }

    void ItemDetails(GameObject item, Sprite pic, string name, int value)
    {
        ItemName = item.transform.Find("Name").GetComponent<TextMeshProUGUI>();
        ItemName.text = name;
        cost = item.transform.Find("Cost").GetComponent<TextMeshProUGUI>();
        cost.text = value.ToString();
        itemImage = ItemList.GetSprite(itemList.HealthPot);
        item.transform.Find("ItemPic").GetComponent<Image>().sprite = pic;
        Buy = item.transform.Find("Buy").GetComponent<Button>();
        Buy.onClick.AddListener(() => {
            buyItem(item);
        });



        void buyItem(GameObject item)
        {
            PopupManager.Instance.Confirm("Are you sure you want to buy this?", () =>
            {
                //Debug.Log("Yes");
                AddItem(GetItem(item));
                Buy = item.transform.Find("Buy").GetComponent<Button>();
                Buy.interactable = false;
                Buy.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = "Purchased";
            }, () =>
            {
                cancel();
                //Debug.Log("No");
            });
        }
    }

    //update when items added
    CollectableData GetItem(GameObject item)
    {
        string name = item.name;
        switch (name)
        {
            default:
            case "Health Potion": return item_health;
            case "Sword": return item_health;
            case "Amour": return item_health;
        }
    }
    void AddItem(CollectableData item)
    {
        Inventory inventoryManager = GetComponent<Inventory>();
        //inventoryManager.Add(item, 1);
        Debug.Log("Bought");

    }

    void cancel()
    {
        Debug.Log("Cancel");
    }
}
