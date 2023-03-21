using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI _hpText;
    [SerializeField]
    Slider _hpSlider;
    [SerializeField]
    TextMeshProUGUI _coinText;
    [SerializeField]
    Slider _dashBar;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _hpText.text = $"{Singleton.Instance.PlayerStats.currentHealth}/{Singleton.Instance.PlayerStats.maxHealth}";
        _hpSlider.value = (float)Singleton.Instance.PlayerStats.currentHealth / (float)Singleton.Instance.PlayerStats.maxHealth;
        _coinText.text = $"× {Singleton.Instance.Inventory.GetCoins()}";
        _dashBar.value = Singleton.Instance.PlayerController.GetDashPercentage();
    }
}
