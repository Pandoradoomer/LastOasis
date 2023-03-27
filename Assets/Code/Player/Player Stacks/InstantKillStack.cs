using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InstantKillStack", menuName = "Stacks/Instant Kill Stack")]
public class InstantKillStack : EventDepletableStack
{

    private StatModifier modifier = null;

    private void OnValidate()
    {
        if (modifier == null)
            modifier = new StatModifier(Stat.Damage, StatModifierType.SET, 9999.0f);

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
