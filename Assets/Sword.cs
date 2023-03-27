using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour, IPlayerWeapon
{
    [SerializeField] private GameObject sword;

    public void DealDamage(int damage)
    {
        throw new System.NotImplementedException();
    }
    public void OnEquip()
    {
        Debug.Log("You eqipped: " + this.gameObject.name);
        if (Singleton.Instance.Inventory.HasWeapon(this.gameObject))
        {
            Debug.Log("You already have this weapon");
        }
        else
        {
            Singleton.Instance.Inventory.AddWeapon(this.gameObject);
        }

    }

    void Start()
    {

    }

    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnEquip();
        }
    }
}
