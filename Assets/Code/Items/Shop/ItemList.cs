using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class ItemList : MonoBehaviour
{

    [SerializeField] ConsumeableData item_health;
    public enum itemList
    {
        HealthPot,
        Sword,
        Armour,
    }
    public enum NPC
    {
        Captain,
        Shopkeeper
    }

    public static int GetCost(itemList item)
    {
        switch (item)
        {
            default:
            case itemList.HealthPot: return 10;
            case itemList.Sword: return 20;
            case itemList.Armour: return 30;
        }
    }

    public static string GetName(itemList item)
    {
        switch (item)
        {
            default:
            case itemList.HealthPot: return "Health Potion";
            case itemList.Sword: return "Sword";
            case itemList.Armour: return "Armour";
        }
    }

    public static Sprite GetSprite(itemList item)
    {
        switch (item)
        {
            default:
            case itemList.HealthPot: return ItemSprites.sprite.pot;
            case itemList.Sword: return ItemSprites.sprite.sword;
            case itemList.Armour: return ItemSprites.sprite.armour;
        }
    }

    public static Sprite NPCsprites(NPC pic)
    {
        switch (pic)
        {
            default:
            case NPC.Captain: return ItemSprites.sprite.Captain;
            case NPC.Shopkeeper: return ItemSprites.sprite.ShopKeeper;
        }
    }

    public static ConsumeableData ItemObject(itemList item, ConsumeableData item_health)
    {
        switch (item)
        {
            default:
            case itemList.HealthPot: return item_health;
        }
    }
}
