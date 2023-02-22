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

    private void Awake()
    {
        swordCollider = GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && !isSwinging)
        {
            StartCoroutine(SwingSword());
        }
    }

    private IEnumerator SwingSword()
    {
        // TODO: Add animation to make it look like a sword swing.
        // DISCUSSION: Do we want the player to be unable to move the sword once the swing has happened?
        isSwinging = true;

        swordCollider.enabled = true;
        yield return new WaitForSeconds(swingDuration);
        swordCollider.enabled = false;

        yield return new WaitForSeconds(swingDelay);

        isSwinging = false;
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
