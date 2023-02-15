using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
public class MessageManager : MonoBehaviour
{
    public TextMeshProUGUI openChestText;
    public TextMeshProUGUI interactChestText;
    TextMeshProUGUI pickupCoinText;
    TextMeshProUGUI pickupHealthText;
    public bool displayMessage;
    void Start()
    {

    }
    void Update()
    {

    }

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
}
