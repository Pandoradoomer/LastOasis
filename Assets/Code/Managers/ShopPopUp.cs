using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopPopUp : MonoBehaviour
{
    public GameObject shopPanel;
    public bool isInShop;
    public bool shopActive;
    public bool playerInRange;
    [SerializeField] Button exit;
    void Start()
    {
        exit.onClick.AddListener(UnfreezePlayer);
    }

    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerInRange)
        {
            //If menu panel is already active in heirarchy
            if (shopPanel.activeInHierarchy)
            {
                shopPanel.SetActive(false);
                EventManager.TriggerEvent(Event.DialogueFinish, null);
                isInShop = false;

            }
            else
            {
                shopPanel.SetActive(true);
                //EventManager.StartListening(Event.DialogueStart, FreezePlayer);
                EventManager.TriggerEvent(Event.DialogueStart, new StartDialoguePacket());
                isInShop = true;

            }
        }
        //if (shopPanel.activeInHierarchy && isInShop)
        //{
        //    EventManager.StartListening(Event.DialogueStart, FreezePlayer);

        //}


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //Set player state to idle
            //EventManager.StartListening(Event.DialogueStart, FreezePlayer);
            playerInRange = true;
            Debug.Log("Player in range" + playerInRange);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
            //EventManager.StopListening(Event.DialogueFinish, UnfreezePlayer);
            Debug.Log("Player not in range" + playerInRange);
        }
    }

    //void FreezePlayer(IEventPacket packet)
    //{
    //    Singleton.Instance.PlayerController.rb.velocity = Vector2.zero;
    //    //rb.velocity = Vector2.zero;
    //    isInShop = true;
    //}

    void UnfreezePlayer()
    {
        EventManager.TriggerEvent(Event.DialogueFinish, null);

        isInShop = false;
    }

}
