using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStacks : MonoBehaviour
{
    private Dictionary<Stack, int> stacks = new Dictionary<Stack, int>();
    void Start()
    {
        EventManager.StartListening(Event.StackAdded, AddStack);
    }

    private void OnDestroy()
    {
        EventManager.StopListening(Event.StackAdded, AddStack);
    }

    void AddStack(IEventPacket packet)
    {
        StackAddedPacket sap = packet as StackAddedPacket;
        if(sap != null)
        {
            if(stacks.ContainsKey(sap.stackToAdd))
            {
                stacks[sap.stackToAdd] += sap.stackNumber;
            }
            else
            {
                stacks.Add(sap.stackToAdd, sap.stackNumber);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
