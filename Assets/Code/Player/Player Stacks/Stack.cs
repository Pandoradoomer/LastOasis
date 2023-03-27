using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Stack : ScriptableObject
{
    public abstract void OnAdd();
}

public abstract class DepletableStack : Stack
{
    public abstract void OnDeplete();
    public abstract void OnFinalDeplete();
}
public abstract class TimeDepletableStack : DepletableStack
{
    public float timeToDeplete;
}

public abstract class EventDepletableStack : DepletableStack
{
    public Event depleteOnEvent;

    public override void OnAdd()
    {
        EventManager.StartListening(depleteOnEvent, OnDeplete);
    }

    public void OnDeplete(IEventPacket packet)
    {
        if(DepleteCondition(packet))
        if (PlayerStacks.Instance.RemoveStack(this))
        {
            OnFinalDeplete();
        }
    }
    public abstract bool DepleteCondition(IEventPacket packet);
    public override void OnFinalDeplete()
    {
        EventManager.StopListening(depleteOnEvent, OnDeplete);
    }
}

public abstract class PermanentStack : Stack
{

}

public abstract class PersistentStack : Stack
{

}

public class TestEventDepletableStack : EventDepletableStack
{
    StatModifier modifier = null;
    public TestEventDepletableStack()
    {
        depleteOnEvent = Event.RoomExit;
        modifier = new StatModifier(Stat.Speed, StatModifierType.PERCENTAGE, 50.0f);
    }

    public override bool DepleteCondition(IEventPacket packet)
    {
        RoomExitPacket rep = packet as RoomExitPacket;
        if(rep != null)
        {
            return !LevelGeneration.Instance.WasRoomVisited(rep.nextRoomIndex);
        }
        return false;
    }

    public override void OnAdd()
    {
        base.OnAdd();
        PlayerStats.Instance.AddModifier(modifier);
    }
    public override void OnDeplete()
    {
        
    }

    public override void OnFinalDeplete()
    {
        base.OnFinalDeplete();
        PlayerStats.Instance.RemoveModifier(modifier);
    }
}
