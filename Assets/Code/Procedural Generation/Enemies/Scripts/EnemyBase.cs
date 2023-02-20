using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public int roomIndex;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            EventManager.TriggerEvent(Event.EnemyDestroyed,
                new EnemyDestroyedPacket()
                {
                    go = this.gameObject
                });
        }
    }
}
