using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class ChestControl : MonoBehaviour
{
    //public Animation anim;
    public bool isOpen;

    public static ChestControl instance { get; private set; }

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
    public void OpenChest()
    {
        if (!isOpen)
        {
            isOpen = true;
            Debug.Log("Chest Opened");
            //On open chest, spawn coins, display canvas ui interaction
        }
    }
}
