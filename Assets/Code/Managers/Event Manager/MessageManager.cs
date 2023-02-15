using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
public class MessageManager : MonoBehaviour
{
    public TextMeshProUGUI openChestText;
    public GameObject interactChest;
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
        interactChest.SetActive(true);
    }

    public void DisableChestText()
    {
        openChestText.gameObject.SetActive(false);
    }
}
