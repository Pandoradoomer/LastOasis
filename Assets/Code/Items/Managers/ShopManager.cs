using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static ItemList;
using static UnityEditor.Progress;

public class ShopManager : MonoBehaviour
{
    //private Transform shop;
    // Start is called before the first frame update
    private Sprite itemImage;
    public Rigidbody2D rb;
    public TextMeshProUGUI coinText;
    private Button close;

    public static ShopManager Instance { get; private set; }

    void Awake()
    {
        Instance = this;
        close = transform.Find("Close").GetComponent<Button>();
        NPCDisplay(ItemList.NPCsprites(NPC.Shopkeeper));
        Hide();
    }

    // Update is called once per frame
    void Update()
    {
        displayCoins();
        close.onClick.AddListener(() => {
            Hide();
        });
    }



    //display number of coins player has along with mini coin image
    void displayCoins()
    {
        coinText.text = $"{Singleton.Instance.Inventory.GetCoins()}";
    }

    //display NPC placeholder and display NPC text
    void NPCDisplay(Sprite sprite)
    {
        GameObject NPC = GameObject.Find("NPC");
        if (NPC != null)
        {
            Debug.Log("Found");
        }
        NPC.GetComponent<Image>().sprite = sprite;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        rb.constraints = RigidbodyConstraints2D.None;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
}
