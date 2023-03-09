using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorTrigger : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //int coins = { Singleton.Instance.Inventory.GetCoins() };
            //Debug.Log(coins);
            /*
        if (Inventory.instance.HasCoin(item_coin))
            {
                //Inventory.instance.Remove(item_coin, coins);
                if (coins <= 0)
                {
                    Debug.Log("You have no coins");
                }
            }
            SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
            */
        }
    }
}
