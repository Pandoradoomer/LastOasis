using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class SceneChange : MonoBehaviour
{
    private Vector2 move;
    private float moveSpeed = 3.0f;
    public Rigidbody2D rb;
    bool wheelprompt = false;
    bool shopprompt = false;
    [SerializeField] CollectableData item_coin;

    void Update()
    {
        move.x = Input.GetAxisRaw("Horizontal");
        move.y = Input.GetAxisRaw("Vertical");
        move.Normalize();

        rb.velocity = move * moveSpeed;
        if (wheelprompt && Input.GetKeyDown(KeyCode.E))
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            //Debug.Log("Key pressed while colliding with object!");
            PopupManager.Instance.Confirm("Are you sure you want to embark?", () =>
            {
                //Debug.Log("Yes");
                teleport();
            }, () =>
            {
                stay();
                //Debug.Log("No");
            });
        }
        if (shopprompt && Input.GetKeyDown(KeyCode.E))
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            ShopManager.Instance.Show();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Mast"))
        {
            SceneManager.LoadScene("SampleScene");
        } 
        if (collision.gameObject.CompareTag("Captain"))
        {
            MessageManager.instance.DisplayCaptainText();
        }   
        if (collision.gameObject.CompareTag("Chef"))
        {
            MessageManager.instance.DisplayChefText();
        }   
        if (collision.gameObject.CompareTag("Carpenter"))
        {
            MessageManager.instance.DisplayCarpenterText();
        }     
        if (collision.gameObject.CompareTag("Shopkeeper"))
        {
            MessageManager.instance.DisplayShopkeeperText();
            shopprompt = true;
        }     
        if (collision.gameObject.CompareTag("CabinBoy"))
        {
            MessageManager.instance.DisplayCabinBoyText();
        }      
        if (collision.gameObject.CompareTag("Gunner"))
        {
            MessageManager.instance.DisplayGunnerText();
        }       
        if (collision.gameObject.CompareTag("Surgeon"))
        {
            MessageManager.instance.DisplaySurgeonText();
        }    
        if (collision.gameObject.CompareTag("QuarterMaster"))
        {
            MessageManager.instance.DisplayQMText();
        }       
        if (collision.gameObject.CompareTag("SeaArtist"))
        {
            MessageManager.instance.DisplaySAText();
        }
        if (collision.gameObject.CompareTag("Wheel"))
        {
            MessageManager.instance.DisplaywheelText();
            wheelprompt = true;
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Captain"))
        {
            MessageManager.instance.DisableCaptainText();
        }      
        if (collision.gameObject.CompareTag("Chef"))
        {
            MessageManager.instance.DisableChefText();
        }      
        if (collision.gameObject.CompareTag("Carpenter"))
        {
            MessageManager.instance.DisableCarpenterText();
        }     
        if (collision.gameObject.CompareTag("Shopkeeper"))
        {
            MessageManager.instance.DisableShopkeeperText();
        }       
        if (collision.gameObject.CompareTag("CabinBoy"))
        {
            MessageManager.instance.DisableCabinBoyText();
        }       
        if (collision.gameObject.CompareTag("Gunner"))
        {
            MessageManager.instance.DisableGunnerText();
        }       
        if (collision.gameObject.CompareTag("Surgeon"))
        {
            MessageManager.instance.DisableSurgeonText();
        }       
        if (collision.gameObject.CompareTag("QuarterMaster"))
        {
            MessageManager.instance.DisableQMText();
        }        
        if (collision.gameObject.CompareTag("SeaArtist"))
        {
            MessageManager.instance.DisableSAText();
        }
        if (collision.gameObject.CompareTag("Wheel"))
        {
            MessageManager.instance.DisablewheelText();
            wheelprompt = false;
        }
    }

    void teleport()
    {
        int coins = Singleton.Instance.Inventory.GetCoins();
        Singleton.Instance.Inventory.Remove(item_coin, coins);
        SceneManager.LoadScene("StartTestScene");
    }

    void stay()
    {
        rb.constraints = RigidbodyConstraints2D.None;
    }
}
