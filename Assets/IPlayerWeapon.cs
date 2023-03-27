using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerWeapon
{
    public abstract void OnEquip();
    public abstract void DealDamage(int damage);
}
