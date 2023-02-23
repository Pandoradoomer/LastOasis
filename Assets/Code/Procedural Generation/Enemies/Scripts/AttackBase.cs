using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBase : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private EnemyBase enemyBase;
    public bool IsAttacking = false;

    //Debug only; delete on commit
    [SerializeField]
    GameObject hitboxSprite;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
    }

    public void SetSpriteActive(bool enabled)
    {
        hitboxSprite.SetActive(enabled);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if(!IsAttacking)
            {
                //send message to attack
                EventManager.TriggerEvent(Event.EnemyHitboxEntered, new EnemyHitboxEnteredPacket()
                {
                    Hitbox = this.gameObject
                });
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if(IsAttacking)
            {
                EventManager.TriggerEvent(Event.EnemyHitPlayer, new EnemyHitPacket()
                {
                    healthDeplete = enemyBase.attackDamage
                });
            }
        }
    }
}
