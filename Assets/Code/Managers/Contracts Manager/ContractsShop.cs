using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Contract", menuName = "Shop Menu/Contracts")]
public class ContractsShop : ScriptableObject
{
    public enum GAIN_STAT
    {
        Health,
        Damage,
        Defence,
        Dexterity,
        Speed
    }
    public List<GAIN_STAT> GainList;
    public enum LOSE_STAT
    {
        Health,
        Damage,
        Defence,
        Dexterity,
        Speed
    }
    public List<LOSE_STAT> LostList;

    public int ContractCost;
    public string ContractName;
    [TextArea] public string ContractDescription;
    public List<float> AmountToGainList;
    public List<float> AmountToLoseList;
    public Sprite NpcImage;
    public string NpcName;

    public bool ContractBought;

    public void PurchaseContract()
    {
        for(int i = 0; i < GainList.Count; i++)
        {
            switch(GainList[i])
            {
                case GAIN_STAT.Health:
                    break;
                case GAIN_STAT.Damage:
                    Singleton.Instance.PlayerStats.currentDamage *= 2;
                    break;
                case GAIN_STAT.Defence:
                    break;
                case GAIN_STAT.Dexterity:
                    Singleton.Instance.PlayerStats.currentDexterity *= 1.2f;
                    break;
                case GAIN_STAT.Speed:
                    Singleton.Instance.PlayerStats.currentSpeed *= 1.2f;
                    break;
            }
        }

        for (int i = 0; i < LostList.Count; i++)
        {
            switch (LostList[i])
            {
                case LOSE_STAT.Health:
                    break;
                case LOSE_STAT.Damage:
                    Singleton.Instance.PlayerStats.currentDamage /= 1.5f;
                    break;
                case LOSE_STAT.Defence:
                    break;
                case LOSE_STAT.Dexterity:
                    Singleton.Instance.PlayerStats.currentDexterity /= 2;
                    break;
                case LOSE_STAT.Speed:
                    break;
            }
        }

        Singleton.Instance.Inventory.RemoveCoins(ContractCost);
        ContractBought = true;
    }

    public string GainText()
    {
        string gainString = string.Empty;

        for (int i = 0; i < AmountToGainList.Count; i++)
        {
            gainString += "+" + AmountToGainList[i].ToString("F1") + " " + GainList[i].ToString() + "\n";
        }

        return gainString;
    }
    public string LoseText()
    {
        string loseString = string.Empty;

        for (int i = 0; i < AmountToLoseList.Count; i++)
        {
            loseString += AmountToLoseList[i].ToString("F1") + " " + LostList[i].ToString() + "\n";
        }

        return loseString;
    }

    public void CalculateAmountToGain()
    {
        AmountToGainList.Clear();

        for (int i = 0; i < GainList.Count; i++)
        {
            switch (GainList[i])
            {
                case GAIN_STAT.Health:
                    break;
                case GAIN_STAT.Damage:
                    AmountToGainList.Add(Singleton.Instance.PlayerStats.currentDamage * 2 - Singleton.Instance.PlayerStats.currentDamage);
                    break;
                case GAIN_STAT.Defence:
                    break;
                case GAIN_STAT.Dexterity:
                    AmountToGainList.Add(Singleton.Instance.PlayerStats.currentDexterity * 1.2f - Singleton.Instance.PlayerStats.currentDexterity);
                    break;
                case GAIN_STAT.Speed:
                    AmountToGainList.Add(Singleton.Instance.PlayerStats.currentSpeed * 1.2f - Singleton.Instance.PlayerStats.currentSpeed);
                    break;
            }
        }
    }

    public void CalculateAmountToLose()
    {
        AmountToLoseList.Clear();

        for (int i = 0; i < LostList.Count; i++)
        {
            switch (LostList[i])
            {
                case LOSE_STAT.Health:
                    break;
                case LOSE_STAT.Damage:
                    AmountToLoseList.Add(Singleton.Instance.PlayerStats.currentDamage / 1.5f - Singleton.Instance.PlayerStats.currentDamage);
                    break;
                case LOSE_STAT.Defence:
                    break;
                case LOSE_STAT.Dexterity:
                    AmountToLoseList.Add(Singleton.Instance.PlayerStats.currentDexterity / 2 - Singleton.Instance.PlayerStats.currentDexterity);
                    break;
                case LOSE_STAT.Speed:
                    break;
            }
        }
    }
}
