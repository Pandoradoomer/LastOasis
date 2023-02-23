using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmerBehaviour : BaseMovementBehaviour
{
    public float speed = 1f;
    public float stunDuration = 0.4f;


    protected override void OnHitAction()
    {
        StartCoroutine(Stun(stunDuration));
    }

    protected override Vector2 GetMovement()
    {
        return MovementFunctions.FollowPlayer(speed, transform.position);
    }

    private IEnumerator Stun(float duration)
    {
        canMove = false;
        for (float i = 0; i <= duration; i += Time.deltaTime)
        {
            yield return null;
        }
        canMove = true;
    }
}

