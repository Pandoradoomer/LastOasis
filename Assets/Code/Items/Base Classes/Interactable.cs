using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Interactable : MonoBehaviour
{
    public bool playerInRange;
    public KeyCode chestOpenKey;
    public UnityEvent interactAction;
    private MessageManager messages;


    void Start()
    {
        messages = GetComponent<MessageManager>();

    }
    //TODO Dylan: Use the in-house Event System not the in-built one

    void Update()
    {
        if (playerInRange)
        {
            if(Input.GetKeyDown(chestOpenKey))
            {
                interactAction.Invoke();
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            playerInRange = true;
            messages.DisplayChestText();
            Debug.Log("Player in range");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
            messages.DisableChestText();
            Debug.Log("Player not in range");
        }
    }
}
