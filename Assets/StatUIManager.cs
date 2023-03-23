using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static System.Runtime.CompilerServices.RuntimeHelpers;

public class StatUIManager : MonoBehaviour
{
   // public GameObject statPanel;
   // public bool isInStatPage;
   //// public bool statActive;
   // bool keyPressed;
   // [SerializeField] Button exit;
   // [SerializeField] Button saveStats;
   // [SerializeField] Button loadStats;
   // [SerializeField] Button resetStats;
    [SerializeField] TextMeshProUGUI healthValue;
    [SerializeField] TextMeshProUGUI movementSpeedValue;
    [SerializeField] TextMeshProUGUI dexterityValue;
    [SerializeField] TextMeshProUGUI damageValue;
    [SerializeField] TextMeshProUGUI defenceValue;
    [SerializeField] TextMeshProUGUI coinValue;
    void Start()
    {
        StartCoroutine(DisableEscKey());
        //exit.onClick.AddListener(UnfreezePlayer);
        //saveStats.onClick.AddListener(UnfreezePlayer);
        //loadStats.onClick.AddListener(UnfreezePlayer);
       // resetStats.onClick.AddListener(UnfreezePlayer);
        //Get singleton data for player stats here
        healthValue.text = Singleton.Instance.PlayerStats.currentHealth.ToString();
        movementSpeedValue.text = Singleton.Instance.PlayerStats.currentSpeed.ToString("0.00");
        dexterityValue.text = Singleton.Instance.PlayerStats.currentDexterity.ToString("0.00");
        damageValue.text = Singleton.Instance.PlayerStats.currentDamage.ToString();
        defenceValue.text = Singleton.Instance.PlayerStats.currentDefence.ToString();
        coinValue.text = Singleton.Instance.Inventory.GetCoins().ToString();
    }

    //Load on awake

    private void Awake()
    {
        //PlayerStats.health = PlayerPrefs.GetFloat("health");
        //Debug.Log(PlayerPrefs.GetFloat("health"));
        //
        //PlayerStats.damage = PlayerPrefs.GetFloat("damage");
        //Debug.Log(PlayerPrefs.GetFloat("damage"));
        //
        //PlayerStats.moveSpeed = PlayerPrefs.GetFloat("moveSpeed");
        //Debug.Log(PlayerPrefs.GetFloat("moveSpeed"));
        //
        //PlayerStats.dexterity = PlayerPrefs.GetFloat("dexterity");
        //Debug.Log(PlayerPrefs.GetFloat("dexterity"));
        //
        //PlayerStats.dexterity = PlayerPrefs.GetFloat("defence");
        //Debug.Log(PlayerPrefs.GetFloat("defence"));
    }
    void Update()
    {
       DisplayStats();
    }

    void CheckForSave()
    {
        //Save on scene change or quit, implement button functionlity
    }
    void UnfreezePlayer()
    {
        EventManager.TriggerEvent(Event.DialogueFinish, null);

        //isInStatPage = false;
    }

    public void SaveStats()
    {
        
        //PlayerPrefs.SetInt("health", Singleton.Instance.PlayerStats.health);
        //Debug.Log("Your health is: " + PlayerPrefs.GetFloat("health"));
        //
        //PlayerPrefs.SetFloat("damage", PlayerStats.damage);
        //Debug.Log("Your damage is: " + PlayerPrefs.GetFloat("damage"));
        //
        //PlayerPrefs.SetFloat("moveSpeed", PlayerStats.moveSpeed);
        //Debug.Log("Your move speed is: " + PlayerPrefs.GetFloat("moveSpeed"));
        //
        //PlayerPrefs.SetFloat("dexterity", PlayerStats.dexterity);
        //Debug.Log("Your dexterity is: " + PlayerPrefs.GetFloat("dexterity"));
        //
        //PlayerPrefs.SetFloat("defence", PlayerStats.defence);
        //Debug.Log("Your defence is: " + PlayerPrefs.GetFloat("defence"));
    }

    public void ResetSavedStats()
    {
        //PlayerPrefs.DeleteAll();
        PlayerPrefs.DeleteKey("health");
        PlayerPrefs.DeleteKey("damage");
        PlayerPrefs.DeleteKey("moveSpeed");
        PlayerPrefs.DeleteKey("dexterity");
        PlayerPrefs.DeleteKey("defence");
        //Reset back to base stat values
       
    }

    public void LoadStats()
    {
        //PlayerStats.health = PlayerPrefs.GetFloat("health");
        //Debug.Log(PlayerPrefs.GetFloat("health"));
        //
        //PlayerStats.damage = PlayerPrefs.GetFloat("damage");
        //Debug.Log(PlayerPrefs.GetFloat("damage"));
        //
        //PlayerStats.moveSpeed = PlayerPrefs.GetFloat("moveSpeed");
        //Debug.Log(PlayerPrefs.GetFloat("moveSpeed"));
        //
        //PlayerStats.dexterity = PlayerPrefs.GetFloat("dexterity");
        //Debug.Log(PlayerPrefs.GetFloat("dexterity"));
        //
        //PlayerStats.dexterity = PlayerPrefs.GetFloat("defence");
        //Debug.Log(PlayerPrefs.GetFloat("defence"));
    }

    IEnumerator DisableEscKey()
    {
        //Disable Esc to open stat panel until transition panel anim is done
        //keyPressed = false;
        if (!Input.GetKeyDown(KeyCode.Escape))
            {
            yield return null;
        }
        yield return new WaitForSeconds(3.0f);
        //DisplayStats();
        
    }

    void DisplayStats()
    {
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    keyPressed = true;
        //    if (statPanel.activeInHierarchy)
        //    {
        //        statPanel.SetActive(false);
        //        EventManager.TriggerEvent(Event.DialogueFinish, null);
        //        isInStatPage = false;
        //        MessageManager.instance.chefText.alpha = 1;
        //        MessageManager.instance.carpenterText.alpha = 1;
        //        MessageManager.instance.surgeonText.alpha = 1;
        //        MessageManager.instance.qmText.alpha = 1;
        //        MessageManager.instance.gunnerText.alpha = 1;
        //    }
        //    else
        //    {
        //        statPanel.SetActive(true);
        //        EventManager.TriggerEvent(Event.DialogueStart, new StartDialoguePacket());
        //        isInStatPage = true;
        //        MessageManager.instance.chefText.alpha = 0;
        //        MessageManager.instance.carpenterText.alpha = 0;
        //        MessageManager.instance.surgeonText.alpha = 0;
        //        MessageManager.instance.qmText.alpha = 0;
        //        MessageManager.instance.gunnerText.alpha = 0;
        //    }
        //}
        ////Checks if stat panel is active at any given time
        //else if (statPanel.activeInHierarchy)
        //{
        //    //freezes player repeatedly 
        //    EventManager.TriggerEvent(Event.DialogueStart, new StartDialoguePacket());
        //    isInStatPage = true;
        //}
        healthValue.text = Singleton.Instance.PlayerStats.currentHealth.ToString();
        movementSpeedValue.text = Singleton.Instance.PlayerStats.currentSpeed.ToString("0.00");
        dexterityValue.text = Singleton.Instance.PlayerStats.currentDexterity.ToString("0.00");
        damageValue.text = Singleton.Instance.PlayerStats.currentDamage.ToString();
        defenceValue.text = Singleton.Instance.PlayerStats.currentDefence.ToString();
        coinValue.text = Singleton.Instance.Inventory.GetCoins().ToString();

    }
}
