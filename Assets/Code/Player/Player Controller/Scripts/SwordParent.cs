using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordParent : MonoBehaviour
{
    private void Update()
    {
        CursorRotate();
    }

    private void CursorRotate()
    {
        if(PlayerController.instance.currentState != PlayerController.CURRENT_STATE.DASHING)
        {
            // Rotate sword to face mouse pointer
            Vector3 mousePos = Input.mousePosition;
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
            Vector3 lookDir = mouseWorldPos - transform.position;
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}
