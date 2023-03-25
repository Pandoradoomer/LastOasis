using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStacks : MonoBehaviour
{
    private Dictionary<Stack, int> stacks = new Dictionary<Stack, int>();
    private Dictionary<DepletableStack, float> stacksCooldown = new Dictionary<DepletableStack, float>();

    public static PlayerStacks Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
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
                AddDepletableStack(sap.stackToAdd);

            }
            else
            {
                stacks.Add(sap.stackToAdd, sap.stackNumber);
                AddDepletableStack(sap.stackToAdd);
            }
        }
    }

    void AddDepletableStack(Stack stack)
    {
        if (!(stack is DepletableStack))
            return;
        DepletableStack ds = (DepletableStack)stack;
        if(stacksCooldown.ContainsKey(ds))
        {
            stacksCooldown[ds] = ds.timeToDeplete;
        }
        else
        {
            stacksCooldown.Add(ds, ds.timeToDeplete);
        }
    }
    // Update is called once per frame
    void Update()
    {
        DepleteStacks();
    }

    void DepleteStacks()
    {
        List<DepletableStack> keys = new List<DepletableStack>(stacksCooldown.Keys);

        foreach(var key in keys)
        {
            //deplete every depletable stack by a frame counter;
            stacksCooldown[key] -= Time.deltaTime;
            if (stacksCooldown[key] <= 0)
            {
                //if timer reached 0

                //lower the stack number by 1
                stacks[key]--;
                stacksCooldown[key] = key.timeToDeplete;
                //call the stack's ondeplete
                key.OnDeplete();
                //if there are no more stacks
                if (stacks[key] == 0)
                {
                    //remove them from everywhere
                    stacks.Remove(key);
                    stacksCooldown.Remove(key);
                }
            }
        }
    }
}
