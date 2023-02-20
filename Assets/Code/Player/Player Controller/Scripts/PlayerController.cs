using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    Rigidbody2D rb;
    [SerializeField]
    float speed;

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
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        float x = horizontal * speed * Time.deltaTime;
        float y = vertical * speed * Time.deltaTime;

        transform.position = new Vector2(transform.position.x + x, transform.position.y + y);
    }

    void BossTeleport(IEventPacket packet)
    {
        BossTeleportPacket btp = packet as BossTeleportPacket;
        transform.position = btp.transform.position - Vector3.up * 4.0f;
    }
}
