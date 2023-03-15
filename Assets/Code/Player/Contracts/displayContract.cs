using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static ItemList;

public class displayContract : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI contractName;
    [SerializeField] TextMeshProUGUI contractDesc;
    [SerializeField] TextMeshProUGUI bonusText;

    private Button close;
    private Button Buy;
    public Rigidbody2D rb;
    public static displayContract Instance { get; private set; }

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        close = transform.Find("Exit").GetComponent<Button>();
        Hide();
    }

    // Update is called once per frame
    void Update()
    {
        close.onClick.AddListener(() => {
            Hide();
        });
        close.onClick.AddListener(() => {
            BuyContract();
        });
    }

    //placeholder to buy contrracts
    void BuyContract()
    {

    }

    //display contract info
    void contractlist()
    {
        contractName.text = "Name";
        contractDesc.text = "Description of contract";
    }

    void calbonus()
    {
        {
            //calculate bonus stats
            bonusText.text = "Bonus Stats";
        }
    }


    public void Hide()
    {
        gameObject.SetActive(false);
        rb.constraints = RigidbodyConstraints2D.None;
    }

    public void Show()
    {
        gameObject.SetActive(true);
        contractlist();
        calbonus();
    }
}
