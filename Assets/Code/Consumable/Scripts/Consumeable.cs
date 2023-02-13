using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumeable : MonoBehaviour
{
    [SerializeField] ConsumeableData data;
    public float health = 3.0f;
    private void OnValidate()
    {
        GetComponent<SpriteRenderer>().sprite = data.Sprite;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            PlayerStats.instance.currentHealth += health;
        }
    }

}
