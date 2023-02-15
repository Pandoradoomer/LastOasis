using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(this);
        }
    }

    public event Action<GameObject> EnemyDestroyed;
    public event Action<Transform> TeleportInvoked;
    public void OnDestroyObject(GameObject o)
    {
        EnemyDestroyed?.Invoke(o);
    }

    public void OnTeleportInvoked(Transform t)
    {
        TeleportInvoked?.Invoke(t);
    }


}
