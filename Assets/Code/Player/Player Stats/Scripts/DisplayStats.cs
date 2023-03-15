using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static ItemList;

public class DisplayStats : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI dexText;
    [SerializeField] TextMeshProUGUI dmgText;
    [SerializeField] TextMeshProUGUI defText;
    [SerializeField] TextMeshProUGUI hpText;
    [SerializeField] TextMeshProUGUI spdText;

    private Button close;
    [SerializeField] GameObject player;
    PlayerAttack playerAttack;

    public static DisplayStats Instance { get; private set; }

    void Awake()
    {
        Instance = this;
        close = transform.Find("Close").GetComponent<Button>();
        Hide();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        close.onClick.AddListener(() => {
            Hide();
        });
    }

    float calDex()
    {
        playerAttack = player.GetComponent<PlayerAttack>();
        float delay = playerAttack.swingDelay;
        return delay;
    }

    float caldmg()
    {
        playerAttack = player.GetComponent<PlayerAttack>();
        float damage = playerAttack.swingDamage;
        return damage;
    }

    float caldef()
    {
        float defense = 0;
        //calculate based on contract
        return defense;
    }
    int caldhp()
    {
        int hp = 0;
        //calculate based on contract
        return hp;
    }
    int calSpd()
    {
        int speed = 3;
        //calculate based on contract
        return speed;
    }

    public void displayStats()
    {
        float dex = calDex();
        dexText.text = dex.ToString();
        dmgText.text = caldmg().ToString();
        defText.text = caldef().ToString();
        hpText.text = caldhp().ToString();
        spdText.text = calSpd().ToString();
    }
    public void Hide()
    {
        gameObject.SetActive(false);
        SceneChange sceneChange = player.GetComponent<SceneChange>();
        sceneChange.ResetDisplay();
    }


    public void Show()
    {
        displayStats();
        gameObject.SetActive(true);
    }
}
