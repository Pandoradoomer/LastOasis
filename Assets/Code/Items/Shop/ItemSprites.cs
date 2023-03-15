using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSprites : MonoBehaviour
{
    private static ItemSprites Itemsprite;
    public Sprite pot;
    public Sprite sword;
    public Sprite armour;
    public Sprite Captain;
    public Sprite ShopKeeper;
    public static ItemSprites sprite
    {
        get {
            if (Itemsprite == null)
            {
                Itemsprite = Instantiate(Resources.Load("GameAssets") as GameObject).GetComponent<ItemSprites>();
            }
            return Itemsprite;
        }
    }

}

