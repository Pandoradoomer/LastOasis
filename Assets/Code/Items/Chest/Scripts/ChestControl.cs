using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class ChestControl : MonoBehaviour
{
    //public Animation anim;
    public bool isOpen;
    public UnityEvent dropItem;
    public void OpenChest()
    {
        if (!isOpen)
        {
            isOpen = true;
            Debug.Log("Chest Opened");
            dropItem.Invoke();
            //On open chest, spawn coins, display canvas ui interaction
        }
    }
}
