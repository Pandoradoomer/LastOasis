using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance { get; private set; }
    private TextMeshProUGUI text;
    private Button yesBtn;
    private Button noBtn;

    //for keyboard UI movement
    RectTransform location;
    bool yesSelected = true;
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        text = transform.Find("Text").GetComponent<TextMeshProUGUI>();
        yesBtn = transform.Find("Yes").GetComponent<Button>();
        noBtn = transform.Find("No").GetComponent<Button>();
        Hide();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) 
        {
            yesSelected= !yesSelected;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) 
        {
            yesSelected= !yesSelected;
        }
    }


    public void Confirm(string question, Action yes, Action no)
    {
        gameObject.SetActive(true);
        Debug.Log(yesBtn.transform.localPosition);
        text.text = question;
        yesBtn.onClick.AddListener(()=>
        {
            Hide();
            yes();
        });
        noBtn.onClick.AddListener(() =>
        {
            Hide();
            no();
        });
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
