using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnScript : MonoBehaviour
{
    public Rigidbody2D rb;
    // Start is called before the first frame update
    // Update is called once per frame
    void Update()
    {
        if (room() && Input.GetKeyDown(KeyCode.L))
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            //Debug.Log("Key pressed while colliding with object!");
            PopupManager.Instance.Confirm("Are you sure you want to return to the ship?", () =>
            {
                //Debug.Log("Yes");
                ship();
            }, () =>
            {
                stay();
                //Debug.Log("No");
            });
        }
    }

    void ship()
    {
        SceneManager.LoadScene("Ship", LoadSceneMode.Single);
    }

    void stay()
    {
        rb.constraints = RigidbodyConstraints2D.None;
    }

    //check conditions for return
    bool room()
    {
        return true;
    }
}
