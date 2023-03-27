using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerWeapon
{
    //Onequip player weapon is = prefab, current weapon = prefab sprite
    public abstract void OnEquip();
    //public abstract void Dequip();
    public abstract void DealDamage(int damage);
}
