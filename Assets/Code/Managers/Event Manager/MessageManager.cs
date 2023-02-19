using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
public class MessageManager : MonoBehaviour
{
    public TextMeshProUGUI openChestText;
    public TextMeshProUGUI interactChestText;
    [SerializeField]
    private TextMeshProUGUI pickupHealthActivateText;
    public bool displayMessages;

    void Update()
    {
        if (!displayMessages)
        {
            DisableChestText();
            DisableChestInteractText();
            DisableHealthConsumableText();
        }
    }
    //***************** CHEST MESSAGES *****************//
    public void DisplayChestText()
    {
        openChestText.gameObject.SetActive(true); 
    }

    public void DisableChestText()
    {
        openChestText.gameObject.SetActive(false);
    }

    public void DisplayChestInteractText()
    {
        interactChestText.gameObject.SetActive(true);
    }

    public void DisableChestInteractText()
    {
        interactChestText.gameObject.SetActive(false);
    }

    //***************** HEALTH MESSAGES *****************//

    public void DisplayHealthConsumableText()
    {
        pickupHealthActivateText.gameObject.SetActive(true);
    }

    public void DisableHealthConsumableText()
    {
        pickupHealthActivateText.gameObject.SetActive(false);
    }
}
