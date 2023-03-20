using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StatUIManager : MonoBehaviour
{
    public GameObject statPanel;
    public bool isInStatPage;
    public bool statActive;
    [SerializeField] Button exit;
    [SerializeField] Button saveStats;
    [SerializeField] Button loadStats;
    [SerializeField] Button resetStats;
    [SerializeField] TextMeshProUGUI healthValue;
    [SerializeField] TextMeshProUGUI damageValue;
    [SerializeField] TextMeshProUGUI movementSpeedValue;
    [SerializeField] TextMeshProUGUI dexterityValue;
    [SerializeField] TextMeshProUGUI defenceValue;
    void Start()
    {
        exit.onClick.AddListener(UnfreezePlayer);
        saveStats.onClick.AddListener(UnfreezePlayer);
        loadStats.onClick.AddListener(UnfreezePlayer);
        resetStats.onClick.AddListener(UnfreezePlayer);
    }

 
    //Load on awake

    private void Awake()
    {
        PlayerStats.health = PlayerPrefs.GetFloat("health");
        Debug.Log(PlayerPrefs.GetFloat("health"));

        PlayerStats.damage = PlayerPrefs.GetFloat("damage");
        Debug.Log(PlayerPrefs.GetFloat("damage"));

        PlayerStats.moveSpeed = PlayerPrefs.GetFloat("moveSpeed");
        Debug.Log(PlayerPrefs.GetFloat("moveSpeed"));

        PlayerStats.dexterity = PlayerPrefs.GetFloat("dexterity");
        Debug.Log(PlayerPrefs.GetFloat("dexterity"));

        PlayerStats.dexterity = PlayerPrefs.GetFloat("defence");
        Debug.Log(PlayerPrefs.GetFloat("defence"));
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (statPanel.activeInHierarchy)
            {
                statPanel.SetActive(false);
                EventManager.TriggerEvent(Event.DialogueFinish, null);
                isInStatPage = false; 
            }
            else
            {
                statPanel.SetActive(true);
                EventManager.TriggerEvent(Event.DialogueStart, new StartDialoguePacket());
                isInStatPage = true;
            }
        }
        healthValue.text = PlayerStats.health.ToString();
        damageValue.text = PlayerStats.damage.ToString();
        movementSpeedValue.text = PlayerStats.moveSpeed.ToString();
        dexterityValue.text = PlayerStats.dexterity.ToString();
        defenceValue.text = PlayerStats.defence.ToString();
    }

    void CheckForSave()
    {
        //Save on scene change or quit, implement button functionlity
    }
    void UnfreezePlayer()
    {
        EventManager.TriggerEvent(Event.DialogueFinish, null);

        isInStatPage = false;
    }

    public void SaveStats()
    {
        PlayerPrefs.SetFloat("health", PlayerStats.health);
        Debug.Log("Your health is: " + PlayerPrefs.GetFloat("health"));

        PlayerPrefs.SetFloat("damage", PlayerStats.damage);
        Debug.Log("Your damage is: " + PlayerPrefs.GetFloat("damage"));

        PlayerPrefs.SetFloat("moveSpeed", PlayerStats.moveSpeed);
        Debug.Log("Your move speed is: " + PlayerPrefs.GetFloat("moveSpeed"));

        PlayerPrefs.SetFloat("dexterity", PlayerStats.dexterity);
        Debug.Log("Your dexterity is: " + PlayerPrefs.GetFloat("dexterity"));

        PlayerPrefs.SetFloat("defence", PlayerStats.defence);
        Debug.Log("Your defence is: " + PlayerPrefs.GetFloat("defence"));
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
        PlayerStats.health = PlayerPrefs.GetFloat("health");
        Debug.Log(PlayerPrefs.GetFloat("health"));

        PlayerStats.damage = PlayerPrefs.GetFloat("damage");
        Debug.Log(PlayerPrefs.GetFloat("damage"));

        PlayerStats.moveSpeed = PlayerPrefs.GetFloat("moveSpeed");
        Debug.Log(PlayerPrefs.GetFloat("moveSpeed"));

        PlayerStats.dexterity = PlayerPrefs.GetFloat("dexterity");
        Debug.Log(PlayerPrefs.GetFloat("dexterity"));

        PlayerStats.dexterity = PlayerPrefs.GetFloat("defence");
        Debug.Log(PlayerPrefs.GetFloat("defence"));
    }
}
