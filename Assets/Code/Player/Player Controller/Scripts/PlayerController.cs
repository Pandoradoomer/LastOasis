using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Getting state for future development once we have animations.
    public enum CURRENT_STATE
    {
        RUN,
        IDLE,
        ATTACK
    };

    public CURRENT_STATE currentState;

    [SerializeField] Rigidbody2D rb;
    [SerializeField] float speed;

    private Vector2 movement;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.StartListening(Event.BossTeleport, BossTeleport);
    }

    private void OnDestroy()
    {
        EventManager.StopListening(Event.BossTeleport, BossTeleport);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        movement.Normalize();
        rb.MovePosition(new Vector2(transform.position.x, transform.position.y) + movement * speed * Time.deltaTime);
    }

    void BossTeleport(IEventPacket packet)
    {
        BossTeleportPacket btp = packet as BossTeleportPacket;
        transform.position = btp.transform.position - Vector3.up * 4.0f;
    }
}
