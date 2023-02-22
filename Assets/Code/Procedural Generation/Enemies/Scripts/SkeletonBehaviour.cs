using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SkeletonBehaviour : MonoBehaviour, IEnemyBehaviour
{
    [SerializeField]
    AttackBase attackHitbox;

    public float speed;
    private Rigidbody2D rb;
    // Start is called before the first frame update

    bool canMove = true;
    bool isAttacking = false;

    public float windUpTime;
    public float strikeTime;

    //TODO: fix duplicate code
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        EventManager.StartListening(Event.EnemyHitboxEntered, OnHitboxEntered);
    }

    // Update is called once per frame
    void Update()
    {
        Act();
    }
    public void Act()
    {
        if (canMove)
            rb.velocity = Move();
        else
            rb.velocity = Vector2.zero;
    }

    private Vector2 Move()
    {
        Vector2 dir = MovementFunctions.FollowPlayer(speed, transform.position);
        attackHitbox.transform.rotation = dir.x <= 0 ? Quaternion.Euler(new Vector3(0,0,180)) : Quaternion.identity;
        return dir;
    }

    public void OnHitboxEntered(IEventPacket packet)
    {
        EnemyHitboxEnteredPacket ehep = packet as EnemyHitboxEnteredPacket;
        if(ehep.Hitbox == attackHitbox.gameObject && !isAttacking)
        {
            StartCoroutine(Attack(windUpTime, strikeTime));
        }
    }

    public void Freeze()
    {
        rb.velocity = Vector2.zero;
        this.enabled = false;
    }

    public IEnumerator Attack(float windUpTime, float strikeTime)
    {
        isAttacking = true;
        canMove = false;
        for(float i = 0; i < windUpTime; i += Time.deltaTime)
        {
            yield return null;
        }
        attackHitbox.IsAttacking = true;
        attackHitbox.SetSpriteActive(true);
        for(float i = 0; i < strikeTime; i+= Time.deltaTime)
        {
            yield return null;
        }
        attackHitbox.IsAttacking = false;
        attackHitbox.SetSpriteActive(false);
        canMove = true;
        isAttacking = false;
        
    }

    public void OnDestroy()
    {
        EventManager.StopListening(Event.EnemyHitboxEntered, OnHitboxEntered);
    }
}
