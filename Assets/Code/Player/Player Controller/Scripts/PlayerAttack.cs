using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float swingRadius;
    public float swingDuration;
    public float swingDelay;
    public float swingDamage;
    public LayerMask targetLayer;

    private CircleCollider2D swordCollider;
    private bool isSwinging = false;
    private SpriteRenderer sr;

    private void Awake()
    {
        swordCollider = GetComponent<CircleCollider2D>();
        sr = GetComponent<SpriteRenderer>();
        sr.enabled = false;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && !isSwinging && PlayerController.instance.currentState != PlayerController.CURRENT_STATE.DASHING)
        {
            StartCoroutine(SwingSword());
        }
    }

    private IEnumerator SwingSword()
    {
        // TODO: Add animation to make it look like a sword swing.
        // DISCUSSION: Do we want the player to be unable to move the sword once the swing has happened?
        PlayerController.instance.currentState = PlayerController.CURRENT_STATE.ATTACK;
        isSwinging = true;
        sr.enabled = isSwinging;

        swordCollider.enabled = true;
        yield return new WaitForSeconds(swingDuration);
        swordCollider.enabled = false;

        yield return new WaitForSeconds(swingDelay);

        isSwinging = false;
        sr.enabled = isSwinging;
        PlayerController.instance.currentState = PlayerController.CURRENT_STATE.RUNNING;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Compares the target layer to the object that was hit.
        if (isSwinging && (targetLayer & 1 << other.gameObject.layer) != 0)
        {
            Debug.Log("Hit: " + other.name);
            other.gameObject.GetComponent<EnemyBase>().currentHealth -= swingDamage;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, swingRadius);
    }
}
