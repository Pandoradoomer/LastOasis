using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

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
    [SerializeField]
    Image deathPanel;
    [SerializeField]
    TextMeshProUGUI _deadText;
    [SerializeField]
    TextMeshProUGUI _inventoryLostText;
    [SerializeField]
    Button _returnButton;
    [SerializeField]
    TextMeshProUGUI _returnButtonText;

    [SerializeField]
    float deathPanelFadeInTime = 1.5f;
    [SerializeField]
    float deathTextDelay = 0.5f;
    [SerializeField]
    float deathTextFadeIn = 1.0f;
    [SerializeField]
    float inventoryTextDelay = 0.5f;
    [SerializeField]
    float inventoryFadeIn = 1.0f;
    [SerializeField]
    float buttonDelay = 0.5f;
    [SerializeField]
    float buttonFadeIn = 1.0f;
    
    void Start()
    {
        EventManager.StartListening(Event.PlayerDeath, OnPlayerDeath);
    }
    private void OnDestroy()
    {
        EventManager.StopListening(Event.PlayerDeath, OnPlayerDeath);
    }

    void OnPlayerDeath(IEventPacket packet)
    {
        StartCoroutine(ShowDeathScreen());
    }

    // Update is called once per frame
    void Update()
    {
        _hpText.text = $"{Singleton.Instance.PlayerStats.currentHealth}/{Singleton.Instance.PlayerStats.maxHealth}";
        _hpSlider.value = (float)Singleton.Instance.PlayerStats.currentHealth / (float)Singleton.Instance.PlayerStats.maxHealth;
        _coinText.text = $"× {Singleton.Instance.Inventory.GetCoins()}";
        _dashBar.value = Singleton.Instance.PlayerController.GetDashPercentage();
    }
    public IEnumerator ShowDeathScreen()
    {
        Color backPanelColor = deathPanel.color;
        backPanelColor.a = 0;
        deathPanel.color = backPanelColor;
        deathPanel.gameObject.SetActive(true);
        for(float i = 0; i < deathPanelFadeInTime; i+= Time.deltaTime)
        {
            backPanelColor.a = Mathf.Lerp(0.0f, 1.0f, i / deathPanelFadeInTime);
            deathPanel.color = backPanelColor;
            yield return null;
        }
        for (float i = 0; i < deathTextDelay; i += Time.deltaTime)
            yield return null;

        Color deathTextColor = _deadText.color;
        deathTextColor.a = 0.0f;
        _deadText.color = deathTextColor;
        _deadText.gameObject.SetActive(true);
        for(float i = 0; i < deathTextFadeIn; i+= Time.deltaTime)
        {
            deathTextColor.a = Mathf.Lerp(0.0f, 1.0f, i / deathTextFadeIn);
            _deadText.color = deathTextColor;
            yield return null;
        }
        for (float i = 0; i < inventoryTextDelay; i += Time.deltaTime)
            yield return null;

        Color inventoryTextColor = _inventoryLostText.color;
        _inventoryLostText.text = $"You've lost {Singleton.Instance.Inventory.GetCoins()} coins xD";
        inventoryTextColor.a = 0.0f;
        _inventoryLostText.color = inventoryTextColor;
        _inventoryLostText.gameObject.SetActive(true);
        for(float i = 0; i < inventoryFadeIn; i+= Time.deltaTime)
        {
            inventoryTextColor.a = Mathf.Lerp(0.0f, 1.0f, i / inventoryFadeIn);
            _inventoryLostText.color = inventoryTextColor;
            yield return null;
        }
        for(float i = 0; i < buttonDelay; i+= Time.deltaTime)
        {
            yield return null;
        }
        Color returnButtonColor = _returnButtonText.color;
        returnButtonColor.a = 0.0f;
        _returnButtonText.color = returnButtonColor;
        _returnButton.gameObject.SetActive(true);
        for(float i = 0; i < buttonFadeIn; i+= Time.deltaTime)
        {
            returnButtonColor.a = Mathf.Lerp(0.0f, 1.0f, i / buttonFadeIn);
            _returnButtonText.color = returnButtonColor;
            yield return null;
        }
        _returnButton.Select();
        _returnButton.onClick.AddListener(delegate { ReturnToShip(); });




        yield return null;
    }

    public IEnumerator FadeOutDeath()
    {
        Color c = Color.white;
        _returnButton.enabled = false;
        EventSystem.current.SetSelectedGameObject(null);
        for(float i = 0; i < deathTextFadeIn; i+= Time.deltaTime)
        {
            c.a = Mathf.Lerp(0.0f, 1.0f, (deathTextFadeIn - i) / deathTextFadeIn);
            _deadText.color = c;
            _inventoryLostText.color = c;
            _returnButtonText.color = c;
            yield return null;
        }
    }

    void ReturnToShip()
    {
        _returnButton.image = null;
        StartCoroutine(ReturnTransition());
    }
    IEnumerator ReturnTransition()
    {
        Singleton.Instance.Inventory.ClearInventory();
        yield return StartCoroutine(FadeOutDeath());
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Ship");
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

    }
}
